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

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [DataContract]
    public class SmileVideoPlayerSettingModel: SettingModelBase
    {
        /// <summary>
        /// 画面上に一度に表示するコメント数。
        /// <para>0で全件。</para>
        /// </summary>
        [DataMember]
        public int DisplayCommentCount { get; set; } = Constants.SmileVideoPlayerDisplayCommentCount;

        /// <summary>
        /// プレイヤーの動画情報欄を表示するか。
        /// </summary>
        [DataMember]
        public bool ShowDetailArea { get; set; } = Constants.SmileVideoPlayerShowDetailArea;

        /// <summary>
        /// プレイヤーのコメント欄を表示するか。
        /// </summary>
        [DataMember]
        public bool ShowCommentList { get; set; } = Constants.SmileVideoPlayerShowCommentList;

        /// <summary>
        /// コメント欄を自動スクロールするか。
        /// </summary>
        [DataMember]
        public bool AutoScrollCommentList { get; set; } = true;

        /// <summary>
        /// コメントを表示するか。
        /// </summary>
        [DataMember]
        public bool VisibleComment { get; set; } = Constants.PlayerVisibleComment;

        /// <summary>
        /// リプレイ再生するか。
        /// </summary>
        [DataMember]
        public bool ReplayVideo { get; set; } = false;

        /// <summary>
        /// 再生音声。
        /// </summary>
        [DataMember]
        public int Volume { get; set; } = 100;

        /// <summary>
        /// ミュート。
        /// </summary>
        [DataMember]
        public bool IsMute { get; set; } = false;

        /// <summary>
        /// 自動再生を行うか。
        /// </summary>
        [DataMember]
        public bool AutoPlay { get; set; } = Constants.SmileVideoAutoPlay;
        /// <summary>
        /// プレイリスト実行中に動画読込がどれだけ停止したら次に進むか。
        /// </summary>
        [DataMember]
        public TimeSpan PlayListBufferingSkipTime { get; set; }

        [DataMember]
        public WindowStatusModel Window { get; set; } = new WindowStatusModel() {
            Left = 150,
            Top = 150,
            Width = 800,
            Height = 600,
        };
    }
}
