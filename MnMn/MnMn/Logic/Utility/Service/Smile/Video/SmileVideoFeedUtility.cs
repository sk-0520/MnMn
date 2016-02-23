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
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    public static class SmileVideoFeedUtility
    {
        #region function

        /// <summary>
        /// 動画IDの取得。
        /// 
        /// TODO: 適当。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetVideoId(FeedSmileVideoItemModel item)
        {
            return item.Link.Split('/').Last();
        }

        /// <summary>
        /// タイトルの取得。
        /// 
        /// TODO: 適当。
        /// </summary>
        /// <param name="rawTitle"></param>
        /// <returns></returns>
        public static string GetTitle(string rawTitle)
        {
            var index = rawTitle?.IndexOf("：");
            if(index != -1 && index < rawTitle?.Length) {
                return rawTitle?.Substring(index.Value + 1) ?? null;
            } else {
                return rawTitle;
            }
        }

        static string HtmlNodeToString(HtmlNode node)
        {
            return node?.InnerHtml ?? string.Empty;
        }

        /// <summary>
        /// RSS生説明部分から各要素に分解。
        /// </summary>
        /// <param name="rawDescription"></param>
        /// <returns>動画ID, タイトル以外の情報を設定したデータ。</returns>
        public static RawSmileVideoFeedDetailModel ConvertRawDescription(string rawDescription)
        {
            // HTMLの書式に合わせておく
            // NOTE: 検索実装時にわかったけど合わせる必要なさげ
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

            var result = new RawSmileVideoFeedDetailModel() {
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

        public static string ConvertDescriptionFromFeedDetailModel(RawSmileVideoFeedDetailModel model)
        {
            var description = $@"
                <span class='nico-thumbnail'><img src='{model.ThumbnailUrl}' /></span>
                <span class='nico-description'>{model.Description}</span>
                <span class='nico-info-length'>{model.Length}</span>
                <span class='nico-info-date'>{model.FirstRetrieve}</span>
                <span class='nico-info-total-view'>{model.ViewCounter}</span>
                <span class='nico-info-total-res'>{model.CommentNum}</span>
                <span class='nico-info-total-mylist'>{model.MylistCounter}</span>
            ";

            return description;
        }

        public static int ConvertInteger(string s)
        {
            if(s.Any(c => c == ',')) {
                s = string.Concat(s.Where(c => c != ','));
            }

            return RawValueUtility.ConvertInteger(s);
        }

        public static string ConvertM3H2TimeFromTimeSpan(TimeSpan time)
        {
            return $"{(int)time.TotalMinutes:00}:{(int)time.Seconds: 00}";
        }

        #endregion

    }
}
