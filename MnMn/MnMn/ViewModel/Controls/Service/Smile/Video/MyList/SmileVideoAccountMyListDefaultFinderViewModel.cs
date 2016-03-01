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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList
{
    /// <summary>
    /// とりあえずマイリストのデータ取得用。
    /// <para>なんでこいつだけJsonなん。。。</para>
    /// </summary>
    public class SmileVideoAccountMyListDefaultFinderViewModel: SmileVideoAccountMyListFinderViewModel
    {
        public SmileVideoAccountMyListDefaultFinderViewModel(Mediation mediation)
            : base(mediation, null)
        { }

        #region property

        #endregion

        #region SmileVideoMyListFinderViewModel

        public override string MyListId { get; } = null;
        public override bool CanEdit { get; } = false;
        public override bool IsPublic { get; } = false;

        public override string MyListName { get { return global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_MyList_DefaultName; } }

        public override string MyListFolderId { get { return string.Empty; } }
        public override string MyListSort { get { return string.Empty; } }

        public override Color MyListFolderColor { get { return Constants.SmileVideoMyListFolderColor; } }

        protected async override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var myList = new Logic.Service.Smile.Api.V1.MyList(Mediation);

            var defaultGroup = await myList.GetAccountDefaultAsync();

            var feedModel = new FeedSmileVideoModel();
            //feedModel.Channel.Title = global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_MyList_DefaultName;
            foreach(var srcItem in defaultGroup.Items) {
                var dstItem = new FeedSmileVideoItemModel();

                dstItem.Title = srcItem.Data.Title;
                // TODO: かっこわるい
                dstItem.Link = "/" + srcItem.Data.WatchId;

                var detailModel = new RawSmileVideoFeedDetailModel();
                detailModel.FirstRetrieve = RawValueUtility.ConvertUnixTime(srcItem.Data.UpdateTime).ToString("u");
                detailModel.Description = srcItem.Description;
                detailModel.ViewCounter = srcItem.Data.ViewCounter;
                detailModel.MylistCounter = srcItem.Data.MylistCounter;
                detailModel.CommentNum = srcItem.Data.NumRes;
                detailModel.ThumbnailUrl = srcItem.Data.ThumbnailUrl;
                var lengthSec = RawValueUtility.ConvertLong(srcItem.Data.Length);
                var lengthTime = TimeSpan.FromSeconds(lengthSec);
                detailModel.Length = SmileVideoFeedUtility.ConvertM3H2TimeFromTimeSpan(lengthTime);

                dstItem.Description = SmileVideoFeedUtility.ConvertDescriptionFromFeedDetailModel(detailModel);

                feedModel.Channel.Items.Add(dstItem);
            }

            return feedModel;
        }

        #endregion
    }
}
