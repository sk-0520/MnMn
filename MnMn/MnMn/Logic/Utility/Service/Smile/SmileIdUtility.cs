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

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile
{
    /// <summary>
    /// 各種IDに関する処理。
    /// <para>IConvertCompatibility.ConvertValueから呼び出される想定で直接使用すべきではない。</para>
    /// </summary>
    internal static class SmileIdUtility
    {
        /// <summary>
        /// スクレイピングする必要のある動画IDか。
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public static bool IsScrapingVideoId(string videoId)
        {
            return videoId.StartsWith("so", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 文字列中から動画IDを取得する。
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns>null: とれんかった</returns>
        public static string GetVideoId(string inputValue)
        {
            if(string.IsNullOrWhiteSpace(inputValue)) {
                return null;
            }

            var regFormat = new Regex(
                $@"
                (
                    watch
                    \/
                )?
                (?<VIDEO_ID>
                    (sm|nm|so) # 他にもあるっぽいけど別段困らない
                    \d+
                )
                ",
                RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Singleline
            );
            var match = regFormat.Match(inputValue);
            if(match.Success) {
                return match.Groups["VIDEO_ID"].Value;
            } else {
                var regNumber = new Regex(
                    @"
                    watch
                    \/
                    (?<VIDEO_ID>
                        \d+
                    )
                    ",
                    RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Singleline
                );

                var numberMatch = regNumber.Match(inputValue);
                if(numberMatch.Success) {
                    return numberMatch.Groups["VIDEO_ID"].Value;
                }

                return null;
            }
        }

        /// <summary>
        /// 文字列中からマイリストIDを取得する。
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns>null: とれんかった</returns>
        public static string GetMyListId(string inputValue)
        {
            if(string.IsNullOrWhiteSpace(inputValue)) {
                return null;
            }

            var regFormat = new Regex(
                @"
                mylist
                \/
                (?<MYLIST_ID>
                    \d+
                )
                ",
                RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Singleline
            );
            var match = regFormat.Match(inputValue);
            if(match.Success) {
                return match.Groups["MYLIST_ID"].Value;
            } else {
                return null;
            }
        }

        /// <summary>
        /// 文字列中からユーザーIDを取得する。
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns>null: とれんかった</returns>
        public static string GetUserId(string inputValue)
        {
            if(string.IsNullOrWhiteSpace(inputValue)) {
                return null;
            }

            var regFormat = new Regex(
                @"
                user
                \/
                (?<USER_ID>
                    \d+
                )
                ",
                RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Singleline
            );
            var match = regFormat.Match(inputValue);
            if(match.Success) {
                return match.Groups["USER_ID"].Value;
            } else {
                return null;
            }
        }

    }
}
