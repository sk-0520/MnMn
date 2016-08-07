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
    public enum SmileContentsSearchField
    {
        /// <summary>
        /// コンテンツID。
        /// <para>http://nico.ms/ の後に連結することでコンテンツへのURLになります。</para>
        /// </summary>
        ContentId,
        /// <summary>
        /// タイトル
        /// </summary>
        Title,
        /// <summary>
        /// コンテンツの説明文。
        /// </summary>
        Description,
        /// <summary>
        /// タグ(空白区切り)
        /// </summary>
        Tags,
        /// <summary>
        /// カテゴリタグ
        /// </summary>
        CategoryTags,
        /// <summary>
        /// 再生数または来場者数。
        /// </summary>
        ViewCounter,
        /// <summary>
        /// マイリスト数またはお気に入り数。
        /// </summary>
        MylistCounter,
        /// <summary>
        /// コメント数
        /// </summary>
        CommentCounter,
        /// <summary>
        /// コンテンツの投稿時間または生放送の開始時間。
        /// </summary>
        StartTime,
        /// <summary>
        /// サムネイルのURL
        /// </summary>
        ThumbnailUrl,
        /// <summary>
        /// コミュニティアイコンのURL
        /// </summary>
        CommunityIcon,
        /// <summary>
        /// （生放送のみ）タイムシフト予約者数
        /// </summary>
        ScoreTimeshiftReserved,
        /// <summary>
        /// （生放送のみ）放送ステータス（過去放送/生放送中/予約放送）
        /// </summary>
        LiveStatus
    }
}
