using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.ProcessLink;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.IdleTalk.Mutter
{
    public class IdleTalkMutterProcessLinkChildHost : ProcessLinkChildHostBase
    {
        public IdleTalkMutterProcessLinkChildHost(Mediation mediation)
            : base(mediation)
        {
        }

        #region function

        Task<ProcessLinkResultModel> ExecuteCore(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ProcessLinkChildHostBase

        public override Task<ProcessLinkResultModel> Execute(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            if(parameter.ServiceType != ServiceType.IdleTalkMutter) {
                throw new ArgumentException(GetExceptionString(parameter));
            }

            return ExecuteCore(parameter);
        }

        #endregion
    }
}
