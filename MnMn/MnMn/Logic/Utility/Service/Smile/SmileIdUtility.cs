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

        public static object IsVideoId(string inputValue)
        {
            throw new NotImplementedException();
        }

        public static string GetMyListId(string s)
        {
            if(string.IsNullOrWhiteSpace(s)) {
                return null;
            }

            var regFormat = new Regex(
                $@"
                    (
                        mylist
                        \/
                    )?
                    (?<MYLIST_ID>
                        \d+
                    )
                    \s*
                    $
                ",
                RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Singleline
            );
            var match = regFormat.Match(s);
            if(match.Success) {
                return match.Groups["MYLIST_ID"].Value;
            } else {
                return null;
            }
        }
    }
}
