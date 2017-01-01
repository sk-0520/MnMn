using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Define
{
    public class CompileMessageEventArgs: EventArgs
    {
        public CompileMessageEventArgs(string domainName, string identifier, CompileMessageKind kind, string message)
        {
            DomainName = domainName;
            Identifier = identifier;
            Kind = kind;
            Message = message;
        }

        #region property

        public string DomainName { get; }
        public string Identifier { get; }

        public CompileMessageKind Kind { get; }
        public string Message { get;}

        #endregion
    }
}
