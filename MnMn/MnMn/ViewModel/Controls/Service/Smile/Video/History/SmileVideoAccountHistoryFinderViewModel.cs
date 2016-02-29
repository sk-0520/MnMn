using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.HalfBakedApi;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.History
{
    public class SmileVideoAccountHistoryFinderViewModel: SmileVideoHistoryFinderViewModelBase
    {
        public SmileVideoAccountHistoryFinderViewModel(Mediation mediation)
            : base(mediation, SmileVideoMediationKey.history)
        {
            Session = MediationUtility.GetResultFromRequestResponse<SmileSessionViewModel>(Mediation, new RequestModel(RequestKind.Session, ServiceType.Smile));
        }

        #region property

        SmileSessionViewModel Session { get; }

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.All;

        protected override PageLoader CreatePageLoader()
        {
            throw new NotSupportedException();
        }

        
        protected async override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var history = new AccountHistory(Mediation, Session);
            history.SessionSupport = true;
            var htmlDocument = await history.LoadPageHtmlDocument();
            if(htmlDocument == null) {
                FinderLoadState = SmileVideoFinderLoadState.Failure;
                return null;
            }

            FinderLoadState = SmileVideoFinderLoadState.VideoSourceChecking;

            var feedModel =  history.ConvertFeedModel(htmlDocument);
            return feedModel;
        }

        #endregion
    }
}
