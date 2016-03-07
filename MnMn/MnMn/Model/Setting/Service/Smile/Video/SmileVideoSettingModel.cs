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
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    public class SmileVideoSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// 自動再生を行うか。
        /// </summary>
        public bool AutoPlay { get; set; } = Constants.SmileVideoAutoPlay;
        /// <summary>
        /// フォント種別。
        /// </summary>
        public string FontFamily { get; set; } = Constants.CommentFontFamily;
        /// <summary>
        /// フォントサイズ。
        /// </summary>
        public double FontSize { get; set; } = Constants.CommentFontSize;
        /// <summary>
        /// フォント透明度。
        /// </summary>
        public double FontAlpha { get; set; } = Constants.CommentFontAlpha;
        /// <summary>
        /// コメント表示時間。
        /// </summary>
        public TimeSpan ShowTime { get; set; } = Constants.CommentShowTime;

        /// <summary>
        /// 動画検索で一度に取得する数。
        /// </summary>
        public int SearchCount { get; set; } = Constants.SmileVideoSearchCount;

        /// <summary>
        /// 一覧データから動画情報を取得するか。
        /// <para>※元データが一覧表示情報を持っていない場合に使用。</para>
        /// </summary>
        public bool LoadVideoInformation { get; set; } = Constants.SmileVideoLoadVideoInformation;

        /// <summary>
        /// 画面上に一度に表示するコメント数。
        /// <para>0で全件。</para>
        /// </summary>
        public int PlayerDisplayCommentCount { get; set; } = Constants.SmileVideoPlayerDisplayCommentCount;

        /// <summary>
        /// プレイヤーの動画情報欄を表示するか。
        /// </summary>
        public bool PlayerShowDetailArea { get; set; } = Constants.SmileVideoPlayerShowDetailArea;

        /// <summary>
        /// プレイヤーのコメント欄を表示するか。
        /// </summary>
        public bool PlayerShowCommentArea { get; set; } = Constants.SmileVideoPlayerShowCommentArea;

        public bool PlayerVisibleComment { get; set; } = Constants.PlayerVisibleComment;

        #endregion
    }
}
