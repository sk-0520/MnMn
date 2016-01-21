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
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw
{
    public class RawVideoThumbModel: ModelBase
    {
        #region property
        /// <summary>
        /// 動画番号。
        /// </summary>
        [XmlElement("video_id")]
        public string VideoId { get; set; }
        /// <summary>
        /// タイトル。
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }
        /// <summary>
        /// 紹介文。
        /// </summary>
        [XmlElement("description")]
        public string Description { get; set; }
        /// <summary>
        /// サムネイルURL。
        /// </summary>
        [XmlElement("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// 投稿日時。
        /// </summary>
        [XmlElement("first_retrieve")]
        public string FirstRetrieve { get; set; }
        /// <summary>
        /// 長さ。
        /// <para>mmm:ss形式</para>
        /// </summary>
        [XmlElement("length")]
        public string Length { get; set; }
        /// <summary>
        /// 動画の種類。
        /// <para>Flash Video形式なら"flv"、MPEG-4形式なら"mp4"。(flv)、ニコニコムービーメーカーは、"swf"</para>
        /// </summary>
        [XmlElement("movie_type")]
        public string MovieType { get; set; }
        /// <summary>
        /// 動画ファイルサイズ。
        /// <para>byte</para>
        /// </summary>
        [XmlElement("size_high")]
        public string SizeHigh { get; set; }
        /// <summary>
        /// エコノミー動画ファイルサイズ。
        /// <para>byte</para>
        /// </summary>
        [XmlElement("size_low")]
        public string SizeLow { get; set; }
        /// <summary>
        /// 再生数。
        /// </summary>
        [XmlElement("view_counter")]
        public string ViewCounter { get; set; }
        /// <summary>
        /// コメント数
        /// </summary>
        [XmlElement("comment_num")]
        public string CommentNum { get; set; }
        /// <summary>
        /// マイリスト数
        /// </summary>
        [XmlElement("mylist_counter")]
        public string MylistCounter { get; set; }
        /// <summary>
        /// 最後のコメント
        /// </summary>
        [XmlElement("last_res_body")]
        public string LastResBody { get; set; }
        /// <summary>
        /// 動画URL。
        /// </summary>
        [XmlElement("watch_url")]
        public string WatchUrl { get; set; }
        /// <summary>
        /// 動画なら「video」、マイメモリーなら「mymemory」(video)
        /// </summary>
        [XmlElement("thumb_type")]
        public string ThumbType { get; set; }
        /// <summary>
        /// 外部再生。
        /// <para>0:OK</para>
        /// <para>1:NG</para>
        /// </summary>
        [XmlElement("embeddable")]
        public string Embeddable { get; set; }
        /// <summary>
        /// ニコニコ生放送。
        /// <para>1:OK</para>
        /// <para>0:NG</para>
        /// </summary>
        [XmlElement("no_live_play")]
        public string NoLivePlay { get; set; }
        [XmlElement("tags")]
        public CollectionModel<RawVideoTagListModel> Tags { get; set; } = new CollectionModel<RawVideoTagListModel>();
        /// <summary>
        /// アップロードユーザーID。
        /// </summary>
        [XmlElement("user_id")]
        public string UserId { get; set; }
        /// <summary>
        /// アップロードユーザー名。
        /// </summary>
        [XmlElement("user_nickname")]
        public string UserNickname { get; set; }
        /// <summary>
        /// アップロードユーザーアイコン。
        /// </summary>
        [XmlElement("user_icon_url")]
        public string UserIconUrl { get; set; }
        /// <summary>
        ///     チャンネルID
        /// </summary>
        public string ch_id { get; set; }
        /// <summary>
        /// チャンネル名
        /// </summary>
        public string ch_name { get; set; }
        /// <summary>
        /// チャンネルアイコン
        /// </summary>
        public string ch_icon_url { get; set; }
        #endregion
    }
}
