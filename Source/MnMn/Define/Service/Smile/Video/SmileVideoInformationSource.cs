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
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video
{
    /// <summary>
    /// 動画の入力元。
    /// </summary>
    public enum SmileVideoInformationSource
    {
        /// <summary>
        /// 動画ID。
        /// <para>明示的に使用することはない。</para>
        /// </summary>
        VideoId,
        /// <summary>
        /// getthumbinfo api を使用。
        /// </summary>
        Getthumbinfo,
        /// <summary>
        /// フィードから取得。
        /// </summary>
        Feed,
        /// <summary>
        /// 公式検索から取得。
        /// </summary>
        OfficialSearch,
        /// <summary>
        /// コンテンツサーチから取得。
        /// </summary>
        ContentsSearch,
    }
}
