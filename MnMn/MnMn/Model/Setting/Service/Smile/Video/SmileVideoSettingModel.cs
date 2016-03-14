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
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [DataContract]
    public class SmileVideoSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// 自動再生を行うか。
        /// </summary>
        [DataMember]
        public bool VideoAutoPlay { get; set; } = Constants.SmileVideoAutoPlay;
        /// <summary>
        /// フォント種別。
        /// </summary>
        [DataMember]
        public string CommentFontFamily { get; set; } = Constants.CommentFontFamily;
        /// <summary>
        /// フォントサイズ。
        /// </summary>
        [DataMember]
        public double CommentFontSize { get; set; } = Constants.CommentFontSize;
        /// <summary>
        /// フォント透明度。
        /// </summary>
        [DataMember]
        public double CommentFontAlpha { get; set; } = Constants.CommentFontAlpha;
        /// <summary>
        /// コメント表示時間。
        /// </summary>
        [DataMember]
        public TimeSpan CommentShowTime { get; set; } = Constants.CommentShowTime;

        /// <summary>
        /// 動画検索で一度に取得する数。
        /// </summary>
        [DataMember]
        public int SearchCount { get; set; } = Constants.SmileVideoSearchCount;

        /// <summary>
        /// 一覧データから動画情報を取得するか。
        /// <para>※元データが一覧表示情報を持っていない場合に使用。</para>
        /// </summary>
        [DataMember]
        public bool LoadVideoInformation { get; set; } = Constants.SmileVideoLoadVideoInformation;

        /// <summary>
        /// 画面上に一度に表示するコメント数。
        /// <para>0で全件。</para>
        /// </summary>
        [DataMember]
        public int PlayerDisplayCommentCount { get; set; } = Constants.SmileVideoPlayerDisplayCommentCount;

        /// <summary>
        /// プレイヤーの動画情報欄を表示するか。
        /// </summary>
        [DataMember]
        public bool PlayerShowDetailArea { get; set; } = Constants.SmileVideoPlayerShowDetailArea;

        /// <summary>
        /// プレイヤーのコメント欄を表示するか。
        /// </summary>
        [DataMember]
        public bool PlayerShowCommentArea { get; set; } = Constants.SmileVideoPlayerShowCommentArea;

        /// <summary>
        /// コメントを表示するか。
        /// </summary>
        [DataMember]
        public bool PlayerVisibleComment { get; set; } = Constants.PlayerVisibleComment;

        /// <summary>
        /// リプレイ再生するか。
        /// </summary>
        [DataMember]
        public bool PlayerReplayVideo { get; set; } = false;

        /// <summary>
        /// 再生音声。
        /// </summary>
        [DataMember]
        public int PlayerVolume { get; set; } = 100;

        /// <summary>
        /// ミュート。
        /// </summary>
        [DataMember]
        public bool PlayerIsMute { get; set; } = false;

        #endregion
    }
}
