using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public abstract class SmileVideoFeedFinderViewModelBase: SmileVideoFinderViewModelBase
    {
        public SmileVideoFeedFinderViewModelBase(Mediation mediation)
            : base(mediation)
        { }

        #region property

        public HttpUserAgentHost UserAgentHost { get; } = new HttpUserAgentHost();

        #endregion

        #region command

        #endregion

        #region function

        protected override async Task LoadAsync_Impl(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            FinderLoadState = SmileVideoFinderLoadState.VideoSourceLoading;
            NowLoading = true;

            FeedSmileVideoModel feedModel = null;

            using(var page = CreatePageLoader()) {
                var feedResult = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                if(!feedResult.IsSuccess) {
                    FinderLoadState = SmileVideoFinderLoadState.Failure;
                } else {
                    FinderLoadState = SmileVideoFinderLoadState.VideoSourceChecking;
                    using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(feedResult.Result))) {
                        feedModel = SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoModel>(stream);
                    }

                }

            }

            if(feedModel == null) {
                NowLoading = false;
                FinderLoadState = SmileVideoFinderLoadState.Failure;
                return;
            }

            await Task.Run(() => {
                return feedModel.Channel.Items
                    .AsParallel()
                    .Select((item, index) => new SmileVideoInformationViewModel(Mediation, item, index + 1))
                ;
            }).ContinueWith(task => {
                var cancel = CancelLoading = new CancellationTokenSource();

                VideoInformationList.InitializeRange(task.Result);
                VideoInformationItems.Refresh();

                Task.Run(() => {
                    FinderLoadState = SmileVideoFinderLoadState.InformationLoading;
                    var loader = new SmileVideoInformationLoader(VideoInformationList);
                    return loader.LoadThumbnaiImageAsync(imageCacheSpan);
                }).ContinueWith(t => {
                    //VideoInformationItems.Refresh();
                    FinderLoadState = SmileVideoFinderLoadState.Completed;
                    NowLoading = false;
                    // return Task.CompletedTask;
                }, cancel.Token, TaskContinuationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        #endregion
    }
}
