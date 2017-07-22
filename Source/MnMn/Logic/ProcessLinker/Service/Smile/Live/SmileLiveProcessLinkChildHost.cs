using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.Smile.Live
{
    public class SmileLiveProcessLinkChildHost : ProcessLinkChildHostBase
    {
        public SmileLiveProcessLinkChildHost(Mediation mediation)
            : base(mediation)
        { }

        #region function

        ProcessLinkResultModel ExecuteCore(ServiceType serviceType, string key, string value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ProcessLinkChildHostBase

        public override ProcessLinkResultModel Execute(ServiceType serviceType, string key, string value)
        {
            if(serviceType != ServiceType.SmileLive) {
                throw new ArgumentException($"{nameof(serviceType)}: {serviceType}, {nameof(key)}: {key}, {nameof(value)}: {value}");
            }

            return ExecuteCore(serviceType, key, value);
        }

        #endregion
    }
}
