using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Library.SpaghettiAssembly.Define
{
    public class CompileMessageEventArgs: EventArgs
    {
        public CompileMessageEventArgs(CompileMessageKind kind, string message)
        {
            Kind = kind;
            Message = message;
        }

        #region property

        public CompileMessageKind Kind { get; }
        public string Message { get;}

        #endregion
    }
}
