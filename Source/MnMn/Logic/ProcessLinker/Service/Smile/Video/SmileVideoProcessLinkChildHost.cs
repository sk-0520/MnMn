using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.Smile.Video
{
    public class SmileVideoProcessLinkChildHost : ProcessLinkChildHostBase
    {
        public SmileVideoProcessLinkChildHost(Mediation mediation)
            : base(mediation)
        { }

        #region function

        async Task<ProcessLinkResultModel> ExecuteCore_Video(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            var videoId = SmileIdUtility.GetVideoId(parameter.Value, Mediation);

            if(!string.IsNullOrWhiteSpace(videoId)) {
                var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));

                var videoInformation = await Mediation.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);

                await videoInformation.OpenVideoDefaultAsync(false);

                return new ProcessLinkResultModel(true, videoInformation.Title);
            }

            return new ProcessLinkResultModel(false, "not found videoid");
        }

        Task<ProcessLinkResultModel> ExecuteCore(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            switch(parameter.Key) {
                case "video":
                    return ExecuteCore_Video(parameter);

                default:
                    throw new ArgumentException(GetExceptionString(parameter));
            }
        }

        #endregion

        #region ProcessLinkChildHostBase

        public override Task<ProcessLinkResultModel> Execute(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            if(parameter.ServiceType != ServiceType.SmileVideo) {
                throw new ArgumentException(GetExceptionString(parameter));
            }

            return ExecuteCore(parameter);
        }

        #endregion
    }
}
