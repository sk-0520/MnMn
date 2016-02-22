﻿/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
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

        protected virtual bool IsLoadVideoInformation { get { return Setting.LoadVideoInformation; } }
        protected abstract SmileVideoInformationFlags InformationFlags { get; }

        #region property

        public HttpUserAgentHost UserAgentHost { get; } = new HttpUserAgentHost();

        #endregion

        #region command

        #endregion

        #region function

        protected IEnumerable<SmileVideoInformationViewModel> ConvertInformationFromChannelItems(IEnumerable<FeedSmileVideoItemModel> channelItems)
        {
            return channelItems
                .AsParallel()
                .Select((item, index) => new SmileVideoInformationViewModel(Mediation, item, index + 1, InformationFlags))
            ;
        }

        protected void SetItems(IEnumerable<SmileVideoInformationViewModel> items)
        {
            VideoInformationList.InitializeRange(items);
            VideoInformationItems.Refresh();
        }

        protected Task LoadFinderAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            var cancel = CancelLoading = new CancellationTokenSource();
            return Task.Run(() => {
                FinderLoadState = SmileVideoFinderLoadState.InformationLoading;
                var loader = new SmileVideoInformationLoader(VideoInformationList);
                var imageTask = loader.LoadThumbnaiImageAsync(imageCacheSpan);
                var infoTask = IsLoadVideoInformation ? loader.LoadInformationAsync(thumbCacheSpan) : Task.CompletedTask;
                return Task.WhenAll(infoTask, infoTask);
            }).ContinueWith(t => {
                FinderLoadState = SmileVideoFinderLoadState.Completed;
                NowLoading = false;
            }, cancel.Token, TaskContinuationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());
        }

        protected async virtual Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            using(var page = CreatePageLoader()) {
                var feedResult = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                if(!feedResult.IsSuccess) {
                    FinderLoadState = SmileVideoFinderLoadState.Failure;
                    return null;
                } else {
                    FinderLoadState = SmileVideoFinderLoadState.VideoSourceChecking;
                    using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(feedResult.Result))) {
                        return SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoModel>(stream);
                    }
                }
            }
        }


        protected override async Task LoadAsync_Impl(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            FinderLoadState = SmileVideoFinderLoadState.VideoSourceLoading;
            NowLoading = true;

            FeedSmileVideoModel feedModel = await LoadFeedAsync();

            if(feedModel == null) {
                NowLoading = false;
                FinderLoadState = SmileVideoFinderLoadState.Failure;
                return;
            }

            await Task.Run(() => {
                return ConvertInformationFromChannelItems(feedModel.Channel.Items);
            }).ContinueWith(task => {
                SetItems(task.Result);
                LoadFinderAsync(thumbCacheSpan, imageCacheSpan);
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        #endregion
    }
}
