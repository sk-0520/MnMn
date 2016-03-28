﻿/*
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

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [DataContract]
    public class SmileVideoCommentSettingModel: SettingModelBase
    {
        #region property

        /// <summary>
        /// フォント種別。
        /// </summary>
        [DataMember]
        public string FontFamily { get; set; } = Constants.CommentFontFamily;
        /// <summary>
        /// フォントサイズ。
        /// </summary>
        [DataMember]
        public double FontSize { get; set; } = Constants.CommentFontSize;
        /// <summary>
        /// フォント透明度。
        /// </summary>
        [DataMember]
        public double FontAlpha { get; set; } = Constants.CommentFontAlpha;
        /// <summary>
        /// 太字。
        /// </summary>
        [DataMember]
        public bool FontBold { get; set; }
        /// <summary>
        /// イタリック。
        /// </summary>
        [DataMember]
        public bool FontItalic { get; set; }
        /// <summary>
        /// コメント表示時間。
        /// </summary>
        [DataMember]
        public TimeSpan ShowTime { get; set; } = Constants.CommentShowTime;
        /// <summary>
        /// コメントの/と対になるx05cをバックスラッシュに置き換えるか。
        /// </summary>
        [DataMember]
        public bool ConvertPairYenSlash { get; set; }

        /// <summary>
        /// コメントの共有NGを有効にするか。
        /// </summary>
        [DataMember]
        public bool IsEnabledSharedNoGood { get; set; }

        /// <summary>
        /// コメントの共有NGレベル。
        /// </summary>
        [DataMember]
        public int SharedNoGoodScore { get; set; }

        /// <summary>
        /// コメント投稿を匿名にするか。
        /// </summary>
        [DataMember]
        public bool PostAnonymous { get; set; } = true;

        /// <summary>
        /// フィルタリングデータ。
        /// </summary>
        [DataMember]
        public SmileVideoFilteringSettingModel Filtering { get; set; } = new SmileVideoFilteringSettingModel();

        #endregion
    }
}
