using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Define.CodeExecutor
{
    public class CompileMessageEventArgs: TraceMessageEventArgs
    {
        public CompileMessageEventArgs(string domainName, string identifier, string sequence, CompileMessageKind kind, string message)
            :base(domainName, identifier, sequence, message)
        {
            Kind = kind;
        }

        #region property

        public CompileMessageKind Kind { get; }

        #endregion
    }
}
