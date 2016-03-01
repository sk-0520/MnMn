using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.History
{
    public class SmileVideoAccountHistoryFinderViewModel: SmileVideoHistoryFinderViewModelBase
    {
        public SmileVideoAccountHistoryFinderViewModel(Mediation mediation)
            : base(mediation, SmileVideoMediationKey.historyPage)
        {
            Session = MediationUtility.GetResultFromRequestResponse<SmileSessionViewModel>(Mediation, new RequestModel(RequestKind.Session, ServiceType.Smile));
        }

        #region property

        SmileSessionViewModel Session { get; }

        #endregion

        #region function

        IEnumerable<SmileVideoInformationViewModel> ConvertInformationFromHistoryModel(RawSmileVideoAccountHistoryModel historyModel)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        //protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.All;
        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.Length;

        protected override PageLoader CreatePageLoader()
        {
            throw new NotSupportedException();
        }

        // ぶっちゃけAPI使うよりこっちの方が総通信数は少ないから使いたいけどHTML腐り過ぎてて使いたくない相反する思い。
        protected async override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var history = new Logic.Service.Smile.Video.Api.V1.History(Mediation, this.Session);
            history.SessionSupport = true;

            var htmlDocument = await history.LoadPageHtmlDocument();
            if(htmlDocument == null) {
                FinderLoadState = SmileVideoFinderLoadState.Failure;
                return null;
            }

            FinderLoadState = SmileVideoFinderLoadState.VideoSourceChecking;

            var feedModel = history.ConvertFeedModelFromPageHtml(htmlDocument);
            return feedModel;
        }
 
        #endregion
    }
}
