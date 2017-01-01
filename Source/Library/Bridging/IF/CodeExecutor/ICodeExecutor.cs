using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Define.CodeExecutor;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.CodeExecutor;

namespace ContentTypeTextNet.MnMn.Library.Bridging.IF.CodeExecutor
{
    public interface ICodeExecutor
    {
        #region proeprty

        event EventHandler<CompileMessageEventArgs> CompileMessage;

        CodeLanguage CodeLanguage { get; }

        string Identifier { get; }

        #endregion

        #region function

        void Initialize(CodeInitializeModel initializeModel);

        bool Compile(CompileParameterModel compilerParameter, string source);

        #endregion
    }
}
