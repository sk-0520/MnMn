using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.IdleTalk.Mutter;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.IdleTalk
{
    public class IdleTalkProcessLinkChildHost : ProcessLinkChildHostBase
    {
        public IdleTalkProcessLinkChildHost(Mediation mediation)
            : base(mediation)
        {
            Mutter = new IdleTalkMutterProcessLinkChildHost(Mediation);
        }

        #region property

        IProcessLinkChildHost Mutter { get; }

        #endregion

        #region function

        ProcessLinkResultModel ExecuteCore(ServiceType serviceType, string key, string value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ProcessLinkChildHostBase

        public override ProcessLinkResultModel Execute(ServiceType serviceType, string key, string value)
        {
            switch(serviceType) {
                case ServiceType.IdleTalk:
                    return ExecuteCore(serviceType, key, value);

                case ServiceType.IdleTalkMutter:
                    return Mutter.Execute(serviceType, key, value);

                default:
                    throw new ArgumentException($"{nameof(serviceType)}: {serviceType}, {nameof(key)}: {key}, {nameof(value)}: {value}");
            }
        }

        #endregion
    }
}
