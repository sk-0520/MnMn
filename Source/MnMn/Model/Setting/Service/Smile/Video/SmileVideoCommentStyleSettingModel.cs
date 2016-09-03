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
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [DataContract]
    public class SmileVideoCommentStyleSettingModel: SettingModelBase, IDeepClone
    {
        #region property

        /// <summary>
        /// フォント種別。
        /// </summary>
        [DataMember, IsDeepClone]
        public string FontFamily { get; set; } = Constants.SettingServiceSmileVideoCommentFontFamily;
        /// <summary>
        /// フォントサイズ。
        /// </summary>
        [DataMember, IsDeepClone]
        public double FontSize { get; set; } = Constants.SettingServiceSmileVideoCommentFontSize;
        /// <summary>
        /// フォント透明度。
        /// </summary>
        [DataMember, IsDeepClone]
        public double FontAlpha { get; set; } = Constants.SettingServiceSmileVideoCommentFontAlpha;
        /// <summary>
        /// 太字。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool FontBold { get; set; } = Constants.SettingServiceSmileVideoCommentFontBold;
        /// <summary>
        /// イタリック。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool FontItalic { get; set; } = Constants.SettingServiceSmileVideoCommentFontItalic;
        /// <summary>
        /// コメント表示時間。
        /// </summary>
        [DataMember, IsDeepClone]
        public TimeSpan ShowTime { get; set; } = Constants.SettingServiceSmileVideoCommentShowTime;
        /// <summary>
        /// コメントの/と対になるx05cをバックスラッシュに置き換えるか。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool ConvertPairYenSlash { get; set; } = Constants.SettingServiceSmileVideoCommentConvertPairYenSlash;

        /// <summary>
        /// コメント描画方法。
        /// </summary>
        [DataMember, IsDeepClone]
        public TextShowKind TextShowKind { get; set; } = Constants.SettingServiceSmileVideoCommentTextShowKind;


        #endregion

        #region IDeepClone

        public IDeepClone DeepClone()
        {
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
