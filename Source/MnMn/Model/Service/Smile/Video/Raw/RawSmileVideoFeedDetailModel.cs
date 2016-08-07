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
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoFeedDetailModel:ModelBase
    {
        /// <summary>
        /// 動画ID。
        /// </summary>
        public string VideoId { get; set; }
        /// <summary>
        /// タイトル。
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 紹介文。
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// サムネイルURL。
        /// </summary>
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// 長さ。
        /// <para>mmm:ss形式</para>
        /// </summary>
        public string Length { get; set; }
        /// <summary>
        /// 投稿日時。
        /// </summary>
        public string FirstRetrieve { get; set; }
        /// <summary>
        /// 再生数。
        /// </summary>
        public string ViewCounter { get; set; }
        /// <summary>
        /// コメント数
        /// </summary>
        public string CommentNum { get; set; }
        /// <summary>
        /// マイリスト数
        /// </summary>
        public string MylistCounter { get; set; }

    }
}
