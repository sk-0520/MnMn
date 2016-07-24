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
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.History;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark
{
    public class SmileVideoBookmarkNodeFinderViewModel: SmileVideoFeedFinderViewModelBase
    {
        public SmileVideoBookmarkNodeFinderViewModel(Mediation mediation, SmileVideoBookmarkNodeViewModel node)
            : base(mediation)
        {
            Node = node;
        }

        #region property

        SmileVideoBookmarkNodeViewModel Node { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region SmileVideoHistoryFinderViewModelBase

        protected override SmileVideoInformationFlags InformationFlags { get; } = SmileVideoInformationFlags.MylistCounter | SmileVideoInformationFlags.CommentCounter | SmileVideoInformationFlags.ViewCounter;

        protected override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var result = new FeedSmileVideoModel();
            foreach(var model in Node.VideoItems) {
                var item = new FeedSmileVideoItemModel();

                item.Title = model.VideoTitle;
                //TODO: ああああああ
                item.Link =  "/" + model.VideoId;

                var detailModel = new RawSmileVideoFeedDetailModel();
                detailModel.Title = model.VideoTitle;
                detailModel.VideoId = model.VideoId;
                detailModel.Length = SmileVideoFeedUtility.ConvertM3H2TimeFromTimeSpan(model.Length);

                item.Description = SmileVideoFeedUtility.ConvertDescriptionFromFeedDetailModel(detailModel);
                result.Channel.Items.Add(item);
            }

            return Task.FromResult(result);
        }

        #endregion
    }
}
