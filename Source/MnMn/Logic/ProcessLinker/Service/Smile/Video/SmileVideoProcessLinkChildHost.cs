using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
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
        public SmileVideoProcessLinkChildHost(Mediator mediator)
            : base(mediator)
        { }

        #region function

        async Task<ProcessLinkResultModel> ExecuteCore_Show(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            var videoId = SmileIdUtility.GetVideoId(parameter.Value, Mediator);

            if(!string.IsNullOrWhiteSpace(videoId)) {
                var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));

                var videoInformation = await Mediator.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);

                await videoInformation.OpenVideoDefaultAsync(false);

                return new ProcessLinkResultModel(true, videoInformation.Title);
            }

            return new ProcessLinkResultModel(false, "not found videoid");
        }

        async Task<ProcessLinkResultModel> ExecuteCore_AddBookmark(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            var videoId = SmileIdUtility.GetVideoId(parameter.Value, Mediator);

            if(!string.IsNullOrWhiteSpace(videoId)) {
                var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));

                var videoInformation = await Mediator.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);

                var item = videoInformation.ToVideoItemModel();
                Mediator.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessUnorganizedBookmarkParameterModel(item)));

                return new ProcessLinkResultModel(true, videoInformation.Title);
            }

            return new ProcessLinkResultModel(false, "not found videoid");
        }

        async Task<ProcessLinkResultModel> ExecuteCore_AddCheckItLater(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            var videoId = SmileIdUtility.GetVideoId(parameter.Value, Mediator);

            if(!string.IsNullOrWhiteSpace(videoId)) {
                var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));

                var videoInformation = await Mediator.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);

                var item = videoInformation.ToVideoItemModel();
                Mediator.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessCheckItLaterParameterModel(item, SmileVideoCheckItLaterFrom.ManualOperation, true)));
                return new ProcessLinkResultModel(true, videoInformation.Title);
            }

            return new ProcessLinkResultModel(false, "not found videoid");
        }
        Task<ProcessLinkResultModel> ExecuteCore(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            switch(parameter.Key) {
                case "show":
                    return ExecuteCore_Show(parameter);

                case "add-bookmark":
                    return ExecuteCore_AddBookmark(parameter);

                case "add-later":
                    return ExecuteCore_AddCheckItLater(parameter);

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
