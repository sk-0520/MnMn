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
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw.Feed.RankingRss2;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.NicoNico.Video
{
    public static class RankingUtility
    {
        #region function

        /// <summary>
        /// 動画IDの取得。
        /// 
        /// TODO: 適当。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetVideoId(RankingFeedItemModel item)
        {
            return item.Link.Split('/').Last();
        }

        static string HtmlNodeToString(HtmlNode node)
        {
            return node.InnerHtml;
        }

        /// <summary>
        /// ランキングの生説明部分から各要素に分解。
        /// </summary>
        /// <param name="rawDescription"></param>
        /// <returns>動画ID以外の情報を設定したデータ。</returns>
        public static RawVideoRankingDetailModel ConvertRawDescription(string rawDescription)
        {
            // HTMLの書式に合わせておく
            var html = "<html><body>" + rawDescription + "</body></html>";

            var doc = new HtmlDocument() {
                OptionCheckSyntax = false,
            };
            doc.LoadHtml(html);

            var image = doc.DocumentNode.SelectSingleNode("//*[@class='nico-thumbnail']/img");
            var description= doc.DocumentNode.SelectSingleNode("//*[@class='nico-description']");
            var length = doc.DocumentNode.SelectSingleNode("//*[@class='nico-info-length']");
            var date = doc.DocumentNode.SelectSingleNode("//*[@class='nico-info-date']");
            var viewCounter = doc.DocumentNode.SelectSingleNode("//*[@class='nico-info-total-view']");
            var resCounter = doc.DocumentNode.SelectSingleNode("//*[@class='nico-info-total-res']");
            var mylist = doc.DocumentNode.SelectSingleNode("//*[@class='nico-info-total-mylist']");

            var result = new RawVideoRankingDetailModel() {
                FirstRetrieve = HtmlNodeToString(date),
                Description = HtmlNodeToString(description),
                CommentNum = HtmlNodeToString(resCounter),
                Length = HtmlNodeToString(length),
                MylistCounter = HtmlNodeToString(mylist),
                ViewCounter = HtmlNodeToString(viewCounter),
                ThumbnailUrl = image.GetAttributeValue("src", string.Empty),
            };

            return result;
        }

        #endregion

    }
}
