using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Define;

namespace ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CodeCompilerBase: MarshalByRefObject, IDisposable
    {
        #region define

        static readonly string[] defaultAssemblyNames = new[] {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
            "System.Data.dll"
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

        public CodeCompilerBase(CodeLanguage codeLanguage, CompilerParameters compilerParameters)
        {
            if(compilerParameters == null) {
                throw new ArgumentNullException(nameof(compilerParameters));
            }

            CodeLanguage = codeLanguage;
            CompilerParameters = CompilerParameters;
        }

        ~CodeCompilerBase()
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
        /// <summary>
        /// パラメータ。
        /// </summary>
        public CompilerParameters CompilerParameters { get; }
        /// <summary>
        /// 使用するアセンブリ名。
        /// </summary>
        public IList<string> AssemblyNames { get; } = new List<string>(defaultAssemblyNames);

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
                var e = new CompileMessageEventArgs(kind, message);
                compileMessage(this, e);
            }
        }

        void AppendAssembly(CompilerParameters parameters, string dllName)
        {
            if(!parameters.ReferencedAssemblies.Contains(dllName)) {
                OnCompileMessage(CompileMessageKind.PreProcessor, $"add assembly: {dllName}");
                parameters.ReferencedAssemblies.Add(dllName);
            } else {
                OnCompileMessage(CompileMessageKind.PreProcessor, $"exists assembly: {dllName}");
            }
        }

        protected abstract CodeDomProvider CreateProvider();

        void CreateProviderCore()
        {
            var provider = CreateProvider();

            Provider = provider;
        }

        public bool Compile(string source)
        {
            var stopWatch = new Stopwatch();

            OnCompileMessage(CompileMessageKind.Compile, $"start: {DateTime.Now.ToString("u")}");

            stopWatch.Start();
            Results = Provider.CompileAssemblyFromSource(CompilerParameters, source);
            stopWatch.Stop();

            var compileElapsed = stopWatch.Elapsed;
            
            OnCompileMessage(CompileMessageKind.Compile, $"code: {Results.NativeCompilerReturnValue}, end: {DateTime.Now.ToString("u")}, time: {compileElapsed}");

            foreach(var msg in Results.Output) {
                OnCompileMessage(CompileMessageKind.Compile, msg);
            }

            foreach(var err in Results.Errors.Cast< CompilerError>()) {
                OnCompileMessage(CompileMessageKind.Error, err.ToString());
            }

            return Results.Errors.Count == 0;
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
