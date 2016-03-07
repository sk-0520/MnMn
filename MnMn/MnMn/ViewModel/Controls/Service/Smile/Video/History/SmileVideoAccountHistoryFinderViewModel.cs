using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
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
            Session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
        }

        #region property

        SmileSessionViewModel Session { get; }
        RawSmileVideoAccountHistoryModel AccountHistoryModel { get; set; }

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

        // ぶっちゃけAPI使うよりこっちの方が総通信数は少ないから使いたいけどHTML腐り過ぎてて使いたくない相反する思い。
        protected async override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var history = new Logic.Service.Smile.Video.Api.V1.History(Mediation);

            var htmlDocument = await history.LoadPageHtmlDocument();
            if(htmlDocument == null) {
                FinderLoadState = SmileVideoFinderLoadState.Failure;
                return null;
            }

            FinderLoadState = SmileVideoFinderLoadState.VideoSourceChecking;

            var feedModel = history.ConvertFeedModelFromPageHtml(htmlDocument);
            return feedModel;
        }

        protected override Task LoadAsync_Impl(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            return base.LoadAsync_Impl(thumbCacheSpan, imageCacheSpan, extends).ContinueWith(_ => {
                if(FinderLoadState == SmileVideoFinderLoadState.Failure) {
                    return Task.CompletedTask;
                }

                var history = new Logic.Service.Smile.Video.Api.V1.History(Mediation);
                return history.LoadHistoryAsync().ContinueWith(task => {
                    AccountHistoryModel = task.Result;
                }, CancelLoading.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.Current);
            });
        }

        protected async override Task<CheckModel> RemoveCheckedItemsAsync()
        {
            var history = new Logic.Service.Smile.Video.Api.V1.History(Mediation);
            if(!VideoInformationList.Any(v => v.IsChecked.GetValueOrDefault())) {
                return CheckModel.Failure();
            }

            var model = await history.LoadHistoryAsync();

            var removeItems = VideoInformationList
                .Where(v => v.IsChecked.GetValueOrDefault())
                .ToArray()
            ;

            var errors = new List<CheckModel>();

            foreach(var removeItem in removeItems) {
                // TODO: 即値
                var sleepTime = TimeSpan.FromMilliseconds(250);
                Thread.Sleep(sleepTime);
                var result = await history.RemoveVideoAsync(model, removeItem.VideoId);
                Mediation.Logger.Trace(result.Status.ToString());
                if(!result.IsSuccess) {
                    errors.Add(result);
                }
            }
            if(errors.Any()) {
                return errors.First();
            } else {
                return CheckModel.Success();
            }
        }

        #endregion
    }
}
