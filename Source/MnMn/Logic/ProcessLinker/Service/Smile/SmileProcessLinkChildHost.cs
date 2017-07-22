using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.Smile
{
    public class SmileProcessLinkChildHost : ProcessLinkChildHostBase
    {
        public SmileProcessLinkChildHost(Mediation mediation)
            : base(mediation)
        {
            Video = new SmileVideoProcessLinkChildHost(Mediation);
            Live = new SmileLiveProcessLinkChildHost(Mediation);
        }

        #region property

        IProcessLinkChildHost Video { get; }
        IProcessLinkChildHost Live { get; }

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
                case ServiceType.Smile:
                    return ExecuteCore(serviceType, key, value);

                case ServiceType.SmileVideo:
                    return Video.Execute(serviceType, key, value);

                case ServiceType.SmileLive:
                    return Live.Execute(serviceType, key, value);

                default:
                    throw new ArgumentException($"{nameof(serviceType)}: {serviceType}, {nameof(key)}: {key}, {nameof(value)}: {value}");
            }
        }

        #endregion

    }
}
