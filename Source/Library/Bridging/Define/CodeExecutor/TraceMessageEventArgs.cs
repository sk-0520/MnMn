using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Define.CodeExecutor
{
    public class TraceMessageEventArgs: EventArgs
    {
        public TraceMessageEventArgs(string domainName, string identifier, string sequence, string message)
        {
            DomainName = domainName;
            Identifier = identifier;
            Sequence = sequence;
            Message = message;
        }

        #region property

        public string DomainName { get; }
        public string Identifier { get; }
        public string Sequence { get; }
        public string Message { get; }

        #endregion
    }
}
