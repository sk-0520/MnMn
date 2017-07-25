/*
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
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public abstract class SmileVideoFeedFinderViewModelBase: SmileVideoFinderViewModelBase
    {
        public SmileVideoFeedFinderViewModelBase(Mediator mediator, int baseNumber)
            : base(mediator, baseNumber)
        { }

        #region property

        protected abstract SmileVideoInformationFlags InformationFlags { get; }

        #endregion

        #region command

        #endregion

        #region function

        protected IEnumerable<SmileVideoInformationViewModel> ConvertInformationFromChannelItems(IEnumerable<FeedSmileVideoItemModel> channelItems)
        {
            return channelItems
                .Select((item, index) => {
                    var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(item, InformationFlags));
                    return Mediation.GetResultFromRequest<SmileVideoInformationViewModel>(request);
                })
            ;
        }

        protected abstract Task<FeedSmileVideoModel> LoadFeedAsync();

        protected override Task LoadCoreAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            return LoadFeedAsync().ContinueWith(task => {
                var feedModel = task.Result;
                if(feedModel == null) {
                    NowLoading = false;
                    FinderLoadState = SourceLoadState.Failure;
                    return null;
                } else {
                    return ConvertInformationFromChannelItems(feedModel.Channel.Items);
                }
            }).ContinueWith(task => {
                var items = task.Result;
                if(items != null) {
                    return SetItemsAsync(items, thumbCacheSpan);
                }
                return Task.CompletedTask;
            }, CancelLoading.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.Current);
        }

        #endregion
    }
}
