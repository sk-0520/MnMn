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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Live
{
    public static class SmileLiveFeedUtility
    {
        #region function

        public static SmileLiveType ConvertType(string s)
        {
            switch(s) {
                case "channel":
                    return SmileLiveType.Channel;

                case "community":
                    return SmileLiveType.Community;

                default:
                    return SmileLiveType.Unknown;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regTime">TIME に一致するパターン。</param>
        /// <param name="input"></param>
        /// <returns></returns>
        static string GetTime(Regex regTime, string input)
        {
            var match = regTime.Match(input);
            if(match.Success) {
                return match.Groups["TIME"].Value;
            }

            return "00:00";
        }

        public static RawSmileLiveFeedDetailModel ConvertRawDescription(string rawDescription)
        {
            var result = new RawSmileLiveFeedDetailModel();

            var htmlDocument = HtmlUtility.CreateHtmlDocument(rawDescription);

            var imageElement = htmlDocument.DocumentNode.SelectSingleNode("//img");
            if(imageElement == null) {
                return null;
            }
            result.PictureUrl = imageElement.Attributes["src"].Value;
            
            var dateElement = htmlDocument.DocumentNode.SelectSingleNode("//b[1]");
            if(dateElement != null) {
                var date = RawValueUtility.ConvertDateTime(dateElement.InnerText);
                var openElement = htmlDocument.DocumentNode.SelectSingleNode("//b[2]");
                var openText = openElement.InnerText;
                //TODO: 埋め込み
                var pattern = @"(?<TIME>[0-2]\d:[0-5]\d)";
                var regOpenDoor = new Regex(@"開場：" + pattern);
                var regOpening = new Regex(@"開園：" + pattern);
                var openDoor = GetTime(regOpenDoor, openText);
                var opening = GetTime(regOpening, openText);
                result.DoorsOpen = date.Add(TimeSpan.Parse(openDoor)).ToString("u");
                result.Opening = date.Add(TimeSpan.Parse(opening)).ToString("u");
            }

            return result;
        }


        #endregion
    }
}
