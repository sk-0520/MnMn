using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Define.CodeExecutor;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.CodeExecutor;

namespace ContentTypeTextNet.MnMn.Library.Bridging.IF.CodeExecutor
{
    public interface ICodeExecutor
    {
        #region proeprty

        event EventHandler<CompileMessageEventArgs> CompileMessage;
        event EventHandler<TraceMessageEventArgs> TraceMessage;

        CodeLanguage CodeLanguage { get; }

        #endregion

        #region function

        void Initialize(IReadOnlyCodeInitialize initializeModel);

        bool Compile(IReadOnlyCompileParameter compilerParameter, string source, string className);

        bool HasType(Type type);

        object Invoke(string methodName, params object[] args);
        object GetProperty(string propertyName);
        void SetProperty(string propertyName, object value);

        #endregion
    }
}
