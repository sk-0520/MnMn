using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLink.Service.IdleTalk.Mutter;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLink.Service.IdleTalk
{
    public class IdleTalkProcessLinkChildHost : ProcessLinkChildHostBase
    {
        public IdleTalkProcessLinkChildHost(Mediator mediator)
            : base(mediator)
        {
            Mutter = new IdleTalkMutterProcessLinkChildHost(Mediator);
        }

        #region property

        IProcessLinkChildHost Mutter { get; }

        #endregion

        #region function

        Task<ProcessLinkResultModel> ExecuteCore(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ProcessLinkChildHostBase

        public override Task<ProcessLinkResultModel> Execute(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            switch(parameter.ServiceType) {
                case ServiceType.IdleTalk:
                    return ExecuteCore(parameter);

                case ServiceType.IdleTalkMutter:
                    return Mutter.Execute(parameter);

                default:
                    throw new ArgumentException(GetExceptionString(parameter));
            }
        }

        #endregion
    }
}
