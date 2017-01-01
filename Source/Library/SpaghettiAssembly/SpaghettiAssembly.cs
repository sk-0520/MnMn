using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    public abstract class SpaghettiAssembly: ICodeExecutor
    {
        #region define

        static readonly string[] defaultAssemblyNames = new[] {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
            "System.Data.dll",
            //// MnMn 用
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "ContentTypeTextNet.SharedLibrary.dll"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "Bridging.dll"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MnMn.exe"),
            //"ContentTypeTextNet.SharedLibrary.dll",
            //"",
        };

        #endregion

        #region event

        public event EventHandler<CompileMessageEventArgs> CompileMessage;

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

        public SpaghettiAssembly(CodeLanguage codeLanguage)
        {
            CodeLanguage = codeLanguage;
        }

        ~SpaghettiAssembly()
        {
            Dispose(false);
        }

        #region property

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

        /// <summary>
        /// 言語。
        /// </summary>
        public CodeLanguage CodeLanguage { get; }

        public string Identifier { get; set; }

        protected CodeDomProvider Provider { get; private set; }
        protected CompilerResults Results { get; private set; }

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
                var e = new CompileMessageEventArgs(AppDomain.CurrentDomain.FriendlyName, Identifier, kind, message);
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

        protected abstract CodeDomProvider CreateProvider();

        public bool Compile(CompileParameterModel compilerParameter, string source)
        {
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
                } else{
                    OnCompileMessage(CompileMessageKind.Error, err.ToString());
                }
            }

            return !Results.Errors.HasErrors;
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

        #region IDisposable

        /// <summary>
        /// 解放。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
