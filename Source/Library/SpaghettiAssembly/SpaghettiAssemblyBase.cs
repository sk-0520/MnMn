using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.Bridging.Define.CodeExecutor;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.CodeExecutor;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.CodeExecutor;

namespace ContentTypeTextNet.MnMn.Library.SpaghettiAssembly
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class SpaghettiAssemblyBase: ICodeExecutor
    {
        #region define

        static readonly string[] defaultAssemblyNames = new[] {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
            "System.Data.dll",
        };

        #endregion

        #region event

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </summary>
        [field: NonSerialized]
        public event EventHandler Disposing;

        #endregion

        #region variable

        bool _isDisposed = false;

        #endregion

        public SpaghettiAssemblyBase(CodeLanguage codeLanguage)
        {
            CodeLanguage = codeLanguage;
        }

        ~SpaghettiAssemblyBase()
        {
            Dispose(false);
        }

        #region property

        public bool IsInitialized { get; private set; }

        protected CodeDomProvider Provider { get; private set; }
        protected CompilerResults Results { get; private set; }

        protected object Instance { get; private set; }
        protected Type InstanceType { get; private set; }

        Caching<string, MethodInfo> InstanceMethod { get; } = new Caching<string, MethodInfo>();
        Caching<string, PropertyInfo> InstanceProperty { get; } = new Caching<string, PropertyInfo>();


        #endregion

        #region function

        /// <summary>
        /// <see cref="IsDisposed"/>を再度無効にする場合に呼び出される。
        /// </summary>
        protected virtual void ResetDispose()
        {
            GC.ReRegisterForFinalize(this);
        }

        void OnCompileMessage(CompileMessageKind kind, string message)
        {
            var compileMessage = CompileMessage;
            if(compileMessage != null) {
                var e = new CompileMessageEventArgs(DomainName, Identifier, kind, message);
                compileMessage(this, e);
            }
        }

        void AppendAssembly(CompilerParameters parameters, string assemblyName)
        {
            if(!parameters.ReferencedAssemblies.Contains(assemblyName)) {
                OnCompileMessage(CompileMessageKind.PreProcessor, $"add assembly: {assemblyName}");
                parameters.ReferencedAssemblies.Add(assemblyName);
            } else {
                OnCompileMessage(CompileMessageKind.PreProcessor, $"exists assembly: {assemblyName}");
            }
        }

        void NeedInitialized()
        {
            if(!IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }
        }

        void NeedInstance()
        {
            if(Instance == null) {
                throw new InvalidOperationException(nameof(Instance));
            }
        }

        protected abstract CodeDomProvider CreateProvider();

        public virtual void Initialize(CodeInitializeModel initializeModel)
        {
            if(IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            Identifier = initializeModel.Identifier;
            DomainName = initializeModel.DomainName;

            IsInitialized = true;
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </summary>
        /// <param name="disposing">CLRの管理下か。</param>
        protected virtual void Dispose(bool disposing)
        {
            if(IsDisposed) {
                if(disposing) {
                    if(Provider != null) {
                        Provider.Dispose();
                    }
                }
                return;
            }

            if(Disposing != null) {
                Disposing(this, EventArgs.Empty);
            }

            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ICodeExecutor

        public event EventHandler<CompileMessageEventArgs> CompileMessage;

        /// <summary>
        /// 言語。
        /// </summary>
        public CodeLanguage CodeLanguage { get; }

        public string DomainName { get; private set; }
        public string Identifier { get; private set; }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed
        {
            get { return this._isDisposed; }
            protected set
            {
                if(this._isDisposed && !value) {
                    ResetDispose();
                }
                this._isDisposed = value;
            }
        }

        public bool Compile(CompileParameterModel compilerParameter, string source, string className)
        {
            NeedInitialized();

            if(string.IsNullOrWhiteSpace(source)) {
                throw new ArgumentException(nameof(source));
            }

            if(string.IsNullOrWhiteSpace(className)) {
                throw new ArgumentException(nameof(className));
            }

            var stopWatch = new Stopwatch();

            OnCompileMessage(CompileMessageKind.Compile, $"start: {DateTime.Now.ToString("u")}");

            stopWatch.Start();

            var compilerParameters = new CompilerParameters() {
                CompilerOptions = compilerParameter.CompilerOptions,
                GenerateExecutable = compilerParameter.GenerateExecutable,
                GenerateInMemory = compilerParameter.GenerateInMemory,
                IncludeDebugInformation = compilerParameter.IncludeDebugInformation,
                TreatWarningsAsErrors = compilerParameter.TreatWarningsAsErrors,
                WarningLevel = compilerParameter.WarningLevel,
            };
            foreach(var assemblyName in defaultAssemblyNames.Concat(compilerParameter.AssemblyNames)) {
                AppendAssembly(compilerParameters, assemblyName);
            }
            Provider = CreateProvider();

            Results = Provider.CompileAssemblyFromSource(compilerParameters, source);
            stopWatch.Stop();

            var compileElapsed = stopWatch.Elapsed;

            OnCompileMessage(CompileMessageKind.Compile, $"code: {Results.NativeCompilerReturnValue}, end: {DateTime.Now.ToString("u")}, time: {compileElapsed}");

            foreach(var msg in Results.Output) {
                OnCompileMessage(CompileMessageKind.Compile, msg);
            }

            foreach(var err in Results.Errors.Cast<CompilerError>()) {
                if(err.IsWarning) {
                    OnCompileMessage(CompileMessageKind.Warning, err.ToString());
                } else {
                    OnCompileMessage(CompileMessageKind.Error, err.ToString());
                }
            }

            var success = !Results.Errors.HasErrors;

            if(success) {
                Instance = Results.CompiledAssembly.CreateInstance(className);
                InstanceType = Instance.GetType();
            }

            return success;
        }

        /// <summary>
        /// 解放。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        public object Invoke(string methodName, params object[] args)
        {
            NeedInstance();

            var method = InstanceMethod.Get(methodName, () => {
                var typeMethod = InstanceType.GetMethod(methodName);
                return typeMethod;
            });

            return method.Invoke(Instance, args);
        }

        public object GetProperty(string propertyName)
        {
            NeedInstance();

            var property = InstanceProperty.Get(propertyName, () => {
                var typeProperty = InstanceType.GetProperty(propertyName);
                return typeProperty;
            });

            return property.GetValue(Instance);
        }

        public void SetProperty(string propertyName, object value)
        {
            NeedInstance();

            var property = InstanceProperty.Get(propertyName, () => {
                var typeProperty = InstanceType.GetProperty(propertyName);
                return typeProperty;
            });

            property.SetValue(Instance, value);
        }

        #endregion
    }
}
