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

namespace ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video
{
    /// <summary>
    /// 並び替え方法。
    /// </summary>
    public enum SmileVideoSnapshotSortBy
    {
        /// <summary>
        /// コメントが新しい/古い順。
        /// </summary>
        LastCommentTime,
        /// <summary>
        /// 再生数が多い/少ない順。
        /// </summary>
        ViewCounter,
        /// <summary>
        /// 投稿日時が新しい/古い順。
        /// </summary>
        StartTime,
        /// <summary>
        /// マイリスト数が多い/少ない順。
        /// </summary>
        MylistCounter,
        /// <summary>
        /// コメント数が多い順/少ない順。
        /// </summary>
        CommentCounter,
        /// <summary>
        /// 再生時間が長い順/短い順。
        /// </summary>
        LengthSeconds,
    }
}
