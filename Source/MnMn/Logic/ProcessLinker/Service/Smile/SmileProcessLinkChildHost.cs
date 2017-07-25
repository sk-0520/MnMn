using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.Smile
{
    public class SmileProcessLinkChildHost : ProcessLinkChildHostBase
    {
        public SmileProcessLinkChildHost(Mediator mediation)
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

        Task<ProcessLinkResultModel> ExecuteCore(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ProcessLinkChildHostBase

        public override Task<ProcessLinkResultModel> Execute(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            switch(parameter.ServiceType) {
                case ServiceType.Smile:
                    return ExecuteCore(parameter);

                case ServiceType.SmileVideo:
                    return Video.Execute(parameter);

                case ServiceType.SmileLive:
                    return Live.Execute(parameter);

                default:
                    throw new ArgumentException(GetExceptionString(parameter));
            }
        }

        #endregion

    }
}
