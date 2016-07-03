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

        protected override SmileVideoInformationFlags InformationFlags { get; } = SmileVideoInformationFlags.Length;

        // ぶっちゃけAPI使うよりこっちの方が総通信数は少ないから使いたいけどHTML腐り過ぎてて使いたくない相反する思い。
        protected async override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var history = new Logic.Service.Smile.Video.Api.V1.History(Mediation);

            var htmlDocument = await history.LoadPageHtmlDocument();
            if(htmlDocument == null) {
                FinderLoadState = SourceLoadState.Failure;
                return null;
            }

            FinderLoadState = SourceLoadState.SourceChecking;

            var feedModel = history.ConvertFeedModelFromPageHtml(htmlDocument);
            return feedModel;
        }

        protected override Task LoadCoreAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            return base.LoadCoreAsync(thumbCacheSpan, imageCacheSpan, extends).ContinueWith(_ => {
                if(FinderLoadState == SourceLoadState.Failure) {
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
