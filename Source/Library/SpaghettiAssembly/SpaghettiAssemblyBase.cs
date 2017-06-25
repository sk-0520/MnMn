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
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
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
            "System.Net.Http.dll",
        };

        class CodeTracer: TraceListener
        {
            public CodeTracer(Action<string> writer)
            {
                Writer = writer;
            }

            #region property

            public Action<string> Writer { get; set; }

            #endregion

            #region TraceListener

            public override void Write(string message)
            {
                Writer(message);
            }

            public override void WriteLine(string message)
            {
                Writer(message + System.Environment.NewLine);
            }

            #endregion
        }

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

            Tracer = new CodeTracer(TraceWrite);
            Trace.Listeners.Add(Tracer);
            Trace.AutoFlush = true;
        }

        ~SpaghettiAssemblyBase()
        {
            Dispose(false);
        }

        #region property

        CodeTracer Tracer { get; }

        public bool IsInitialized { get; private set; }

        protected CodeDomProvider Provider { get; private set; }
        protected CompilerResults Results { get; private set; }

        protected object Instance { get; private set; }
        protected Type InstanceType { get; private set; }

        Dictionary<string, MethodInfo> InstanceMethod { get; } = new Dictionary<string, MethodInfo>();
        Dictionary<string, PropertyInfo> InstanceProperty { get; } = new Dictionary<string, PropertyInfo>();


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
                var e = new CompileMessageEventArgs(DomainName, Identifier, Sequence, kind, message);
                compileMessage(this, e);
            }
        }

        void OnTraceMessage(string message)
        {
            var traceMessage = TraceMessage;
            if(traceMessage != null) {
                var e = new TraceMessageEventArgs(DomainName, Identifier, Sequence, message);
                traceMessage(this, e);
            }
        }

        void TraceWrite(string s)
        {
            OnTraceMessage(s);
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

        protected abstract CodeDomProvider CreateProvider(IDictionary<string, string> options);

        public virtual void Initialize(IReadOnlyCodeInitialize initializeModel)
        {
            if(IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            DomainName = initializeModel.DomainName;
            Identifier = initializeModel.Identifier;
            Sequence = initializeModel.Sequence;

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

                    if(Tracer != null) {
                        Trace.Listeners.Remove(Tracer);
                        Tracer.Dispose();
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

        TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> map, TKey key, Func<TValue> func)
        {
            TValue value;
            if(!map.TryGetValue(key, out value)) {
                value = func();
                map.Add(key, value);
            }

            return value;
        }

        #endregion

        #region ICodeExecutor

        public event EventHandler<CompileMessageEventArgs> CompileMessage;
        public event EventHandler<TraceMessageEventArgs> TraceMessage;

        /// <summary>
        /// 言語。
        /// </summary>
        public CodeLanguage CodeLanguage { get; }

        public string DomainName { get; private set; }
        public string Identifier { get; private set; }
        public string Sequence { get; private set; }

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
                // NOTO: Main で死ぬからモデルの機能としては有効だけど MnMn(というかSpaghettiAssemblyBase)では無効にする
                GenerateExecutable = false,
                // NOTE: どうせこちらしか有効にしない
                //GenerateInMemory = compilerParameter.GenerateInMemory,
                GenerateInMemory = true,
                IncludeDebugInformation = compilerParameter.IncludeDebugInformation,
                TreatWarningsAsErrors = compilerParameter.TreatWarningsAsErrors,
                WarningLevel = compilerParameter.WarningLevel,
            };
            foreach(var assemblyName in defaultAssemblyNames.Concat(compilerParameter.AssemblyNames)) {
                AppendAssembly(compilerParameters, assemblyName);
            }
            var providerOptions = compilerParameter.ProviderOptions.ToDictionary(p => p.Key, p => p.Value);
            Provider = CreateProvider(providerOptions);

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

        public bool HasType(Type type)
        {
            NeedInstance();

            return type.IsAssignableFrom(InstanceType);
        }

        public object Invoke(string methodName, params object[] args)
        {
            NeedInstance();

            var method = GetValue(InstanceMethod, methodName, () => {
                var typeMethod = InstanceType.GetMethod(methodName);
                return typeMethod;
            });

            return method.Invoke(Instance, args);
        }

        public object GetProperty(string propertyName)
        {
            NeedInstance();

            var property = GetValue(InstanceProperty, propertyName, () => {
                var typeProperty = InstanceType.GetProperty(propertyName);
                return typeProperty;
            });

            return property.GetValue(Instance);
        }

        public void SetProperty(string propertyName, object value)
        {
            NeedInstance();

            var property = GetValue(InstanceProperty, propertyName, () => {
                var typeProperty = InstanceType.GetProperty(propertyName);
                return typeProperty;
            });

            property.SetValue(Instance, value);
        }

        #endregion
    }
}
