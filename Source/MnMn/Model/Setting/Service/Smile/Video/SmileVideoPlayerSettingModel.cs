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
    public class SmileVideoPlayerSettingModel: SettingModelBase
    {
        #region property

        [DataMember]
        public bool IsEnabledDisplayCommentLimit { get; set; } = Constants.SettingServiceSmileVideoPlayerDisplayCommentLimitIsEnabled;

        /// <summary>
        /// 画面上に一度に表示するコメント数。
        /// </summary>
        [DataMember]
        public int DisplayCommentLimitCount { get; set; } = Constants.SettingServiceSmileVideoPlayerDisplayCommentLimitCount;

        /// <summary>
        /// プレイヤーの動画情報欄を表示するか。
        /// </summary>
        [DataMember]
        public bool ShowDetailArea { get; set; } = Constants.SettingServiceSmileVideoPlayerShowDetailArea;

        /// <summary>
        /// プレイヤーのコメント欄を表示するか。
        /// </summary>
        [DataMember]
        public bool ShowCommentList { get; set; } = Constants.SettingServiceSmileVideoPlayerShowCommentList;

        /// <summary>
        /// コメント欄を自動スクロールするか。
        /// </summary>
        [DataMember]
        public bool AutoScrollCommentList { get; set; } = Constants.SettingServiceSmileVideoPlayerAutoScrollCommentList;

        /// <summary>
        /// コメントを表示するか。
        /// </summary>
        [DataMember]
        public bool VisibleComment { get; set; } = Constants.SettingServiceSmileVideoPlayerVisibleComment;

        /// <summary>
        /// コメント描画方法。
        /// </summary>
        [DataMember]
        public TextShowKind TextShowKind { get; set; } = Constants.SettingServiceSmileVideoPlayerTextShowKind;

        /// <summary>
        /// リプレイ再生するか。
        /// </summary>
        [DataMember]
        public bool ReplayVideo { get; set; } = Constants.SettingServiceSmileVideoPlayerReplay;

        /// <summary>
        /// 再生音声。
        /// </summary>
        [DataMember]
        public int Volume { get; set; } = Constants.SettingServiceSmileVideoPlayerVolume;

        /// <summary>
        /// ミュート。
        /// </summary>
        [DataMember]
        public bool IsMute { get; set; } = Constants.SettingServiceSmileVideoPlayerMute;

        /// <summary>
        /// 自動再生を行うか。
        /// </summary>
        [DataMember]
        public bool AutoPlay { get; set; } = Constants.SettingServiceSmileVideoPlayerAutoPlay;

        /// <summary>
        /// プレイリスト実行中に動画読込がどれだけ停止したら次に進むか。
        /// </summary>
        [DataMember]
        public TimeSpan PlayListBufferingSkipTime { get; set; } = Constants.SettingServiceSmileVideoPlayerBufferingStopSkipTime;

        [DataMember]
        public WindowStatusModel Window { get; set; } = new WindowStatusModel() {
            Left = Constants.SettingServiceSmileVideoWindowLeft,
            Top = Constants.SettingServiceSmileVideoWindowTop,
            Width = Constants.SettingServiceSmileVideoWindowWidth,
            Height = Constants.SettingServiceSmileVideoWindowHeight,
            Tommost = Constants.SettingServiceSmileVideoWindowTopmost,
        };

        /// <summary>
        /// コメント有効表示領域を使用するか。
        /// <para>予約: 今のところ設定に持たせない。</para>
        /// </summary>
        [DataMember]
        public bool CommentEnabledAreaIsEnabled { get; set; } = Constants.SettingServiceSmileVideoPlayerCommentEnableAreaIsEnabled;

        /// <summary>
        /// コメント有効表示領域の高さ。
        /// <para>全体領域の下辺を0として正の値分を非表示領域とする。</para>
        /// <para>予約: 今のところ設定に持たせない。</para>
        /// </summary>
        [DataMember]
        public double CommentEnabledPercent { get; set; } = Constants.SettingServiceSmileVideoPlayerCommentEnableAreaPercent;

        /// <summary>
        /// コメントリストの投稿時間を表示するか。
        /// </summary>
        [DataMember]
        public bool ShowPostTimestamp { get; set; } = Constants.SettingServiceSmileVideoPlayerShowPostTimestamp;

        #endregion
    }
}
