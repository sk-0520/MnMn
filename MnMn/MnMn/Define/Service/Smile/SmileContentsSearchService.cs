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

namespace ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile
{
    /// <summary>
    /// 検索対象のサービス。
    /// </summary>
    public enum SmileContentsSearchService
    {
        /// <summary>
        /// 動画。
        /// </summary>
        Video,
        /// <summary>
        /// 生放送。
        /// </summary>
        Live,
        /// <summary>
        /// 静画(イラスト)。
        /// </summary>
        Illust,
        /// <summary>
        /// 静画(マンガ)。
        /// </summary>
        Manga,
        /// <summary>
        /// 静画(書籍)。
        /// </summary>
        Book,
        /// <summary>
        /// チャンネル。
        /// </summary>
        Channel,
        /// <summary>
        /// ブロマガ記事(著名人)。
        /// </summary>
        ChannelArticle,
        /// <summary>
        /// ニュース。
        /// </summary>
        News,
    }
}
