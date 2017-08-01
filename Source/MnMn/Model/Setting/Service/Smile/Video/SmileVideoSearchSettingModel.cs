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
    [Serializable, DataContract]
    public class SmileVideoSearchSettingModel: SettingModelBase
    {
        /// <summary>
        /// 動画検索で一度に取得する数。
        /// </summary>
        [Obsolete]
        public int Count { get; set; } = Constants.SettingServiceSmileVideoSearchCount;
        /// <summary>
        /// 一覧データから動画情報を取得するか。
        /// <para>※元データが一覧表示情報を持っていない場合に使用。</para>
        /// </summary>
        [DataMember]
        public bool LoadInformation { get; set; } = Constants.SettingServiceSmileVideoLoadVideoInformation;

        [DataMember]
        public string DefaultMethodKey { get; set; }
        [DataMember]
        public string DefaultSortKey { get; set; }
        [DataMember]
        public SearchType DefaultType { get; set; } = SearchType.Tag;

        [DataMember]
        public SearchType FinderSearchType { get; set; } = SearchType.Tag;

        /// <summary>
        /// 検索履歴。
        /// </summary>
        [DataMember]
        public FixedSizeCollectionModel<SmileVideoSearchHistoryModel> SearchHistoryItems { get; set; } = new FixedSizeCollectionModel<SmileVideoSearchHistoryModel>(Constants.ServiceSmileVideoSearchHistoryCount);

        /// <summary>
        /// 検索ブックマーク表示状態。
        /// </summary>
        [DataMember]
        public bool ShowSearchBookmark { get; set; } = false;

        [DataMember]
        public double SearchBookmarkWidth { get; set; } = Constants.SettingServiceSmileVideoSearchBookmarkWidth;
        [DataMember]
        public double SearchFinderWidth { get; set; } = Constants.SettingServiceSmileVideoSearchFinderWidth;

        /// <summary>
        /// 検索ブックマーク。
        /// </summary>
        [DataMember]
        public CollectionModel<SmileVideoSearchBookmarkItemModel> SearchBookmarkItems { get; set; } = new CollectionModel<SmileVideoSearchBookmarkItemModel>();
    }
}
