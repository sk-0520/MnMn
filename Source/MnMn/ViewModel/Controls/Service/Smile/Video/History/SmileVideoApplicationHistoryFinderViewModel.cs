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
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.History
{
    public class SmileVideoApplicationHistoryFinderViewModel: SmileVideoHistoryFinderViewModelBase
    {
        public SmileVideoApplicationHistoryFinderViewModel(Mediation mediation)
            : base(mediation, SmileVideoMediationKey.historyApp)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region SmileVideoHistoryFinderViewModelBase

        protected override bool IsRemovedReload { get; } = false;

        protected override SmileVideoInformationFlags InformationFlags { get; } = SmileVideoInformationFlags.FirstRetrieve | SmileVideoInformationFlags.Length;

        protected override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var result = new FeedSmileVideoModel();
            foreach(var model in Setting.History) {
                var item = new FeedSmileVideoItemModel();

                item.Title = model.VideoTitle;
                item.Link = model.WatchUrl.OriginalString;

                var detailModel = new RawSmileVideoFeedDetailModel();
                detailModel.Title = model.VideoTitle;
                detailModel.VideoId = model.VideoId;
                detailModel.FirstRetrieve = model.FirstRetrieve.ToString("s");
                detailModel.Length = SmileVideoFeedUtility.ConvertM3H2TimeFromTimeSpan(model.Length);

                item.Description = SmileVideoFeedUtility.ConvertDescriptionFromFeedDetailModel(detailModel);
                result.Channel.Items.Add(item);
            }

            return Task.FromResult(result);
        }

        protected override Task<CheckModel> RemoveCheckedItemsAsync()
        {
            var items = GetCheckedItems()
                .ToArray();
            ;

            if(items.Any()) {
                foreach(var item in items) {
                    var model = Setting.History.FirstOrDefault(i => i.VideoId == item.Information.VideoId);
                    if(model != null) {
                        Setting.History.Remove(model);
                        FinderItemList.Remove(item);
                    }
                }
                foreach(var item in FinderItemList.Select((item, index) => new { Item = item, Index = index })) {
                    item.Item.Number = item.Index + 1;
                }

                return Task.FromResult(CheckModel.Success());
            } else {
                return Task.FromResult(CheckModel.Failure());
            }
        }

        #endregion
    }
}
