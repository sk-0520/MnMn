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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [DataContract]
    public class SmileVideoSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// 共通設定。
        /// </summary>
        [DataMember]
        public SmileVideoCommonSettingModel Common { get; set; } = new SmileVideoCommonSettingModel();

        /// <summary>
        /// コメント設定。
        /// </summary>
        [DataMember]
        public SmileVideoCommentSettingModel Comment { get; set; } = new SmileVideoCommentSettingModel();

        /// <summary>
        /// 検索設定。
        /// </summary>
        [DataMember]
        public SmileVideoSearchSettingModel Search { get; set; } = new SmileVideoSearchSettingModel();

        /// <summary>
        /// ダウンロード設定。
        /// </summary>
        [DataMember]
        public SmileVideoDownloadSetting Download { get; set; } = new SmileVideoDownloadSetting();

        /// <summary>
        /// プレイヤー設定。
        /// </summary>
        [DataMember]
        public SmileVideoPlayerSettingModel Player { get; set; } = new SmileVideoPlayerSettingModel();

        /// <summary>
        /// 動画再生設定。
        /// </summary>
        [DataMember]
        public SmileVideoExecuteSettingModel Execute { get; set; } = new SmileVideoExecuteSettingModel();

        /// <summary>
        /// ランキング設定。
        /// </summary>
        [DataMember]
        public SmileVideoRankingSettingModel Ranking { get; set; } = new SmileVideoRankingSettingModel();

        /// <summary>
        /// 履歴。
        /// </summary>
        [DataMember]
        public FixedSizeCollectionModel<SmileVideoPlayHistoryModel> History { get; set; } = new FixedSizeCollectionModel<SmileVideoPlayHistoryModel>(Constants.ServiceSmileVideoPlayHistoryCount, false);

        /// <summary>
        /// ブックマーク。
        /// </summary>
        [DataMember]
        //public SmileVideoBookmarkItemSettingModel Bookmark { get; } = new SmileVideoBookmarkItemSettingModel();
        public SmileVideoBookmarkSettingModel Bookmark { get; } = new SmileVideoBookmarkSettingModel();

        /// <summary>
        /// あとで見る。
        /// </summary>
        [DataMember]
        public FixedSizeCollectionModel<SmileVideoCheckItLaterModel> CheckItLater { get; set; } = new FixedSizeCollectionModel<SmileVideoCheckItLaterModel>(Constants.ServiceSmileVideoCheckItLaterCount, false);

        /// <summary>
        /// 動画フィルタリングデータ。
        /// </summary>
        [DataMember]
        public SmileVideoFinderFilteringSettingModel FinderFiltering { get; set; } = new SmileVideoFinderFilteringSettingModel();


        #endregion
    }
}
