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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile
{
    public static class SmileUserUtility
    {
        public static string GetUserId(string accountElementInnerText)
        {
            var reg = new Regex(@"
                ID
                \s*
                :
                \s*
                (?<USER_ID>
                    \d+
                )
                ",
                RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
            );
            var match = reg.Match(accountElementInnerText);
            if(match.Success) {
                return match.Groups["USER_ID"].Value;
            }

            return null;
        }

        public static string GetVersion(string accountElementInnerText)
        {
            var reg = new Regex(@"
                ID
                \s*
                :
                \s*
                \d+
                \(
                (?<VERSION>
                    .+
                )
                \)
                ",
                RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
            );
            var match = reg.Match(accountElementInnerText);
            if(match.Success) {
                return match.Groups["VERSION"].Value;
            }

            return null;
        }

        public static bool IsPremium(string accountElementInnerText)
        {
            var reg = new Regex(@"
                ID
                \s*
                :
                \s*
                \d+
                \(
                .+
                \)
                \s*
                (?<ACCOUNT>
                    .*
                )
                \s*
                ",
                RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
            );
            var match = reg.Match(accountElementInnerText);
            if(match.Success) {
                var s = match.Groups["ACCOUNT"].Value;
                return s.IndexOf("プレミアム") != -1;
            }

            return false;
        }

        public static CheckResultModel<Gender> GetGender(string accountElementInnerText)
        {
            var reg = new Regex(@"
                性別
                \s*
                :
                \s*
                (?<GENDER>
                    .*
                )
                \s*
                ",
                RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
            );
            var match = reg.Match(accountElementInnerText);
            if(match.Success) {
                var s = match.Groups["GENDER"].Value;
                if(s.IndexOf("男") != -1) {
                    return CheckResultModel.Success(Gender.Male);
                }
                if(s.IndexOf("女") != -1) {
                    return CheckResultModel.Success(Gender.Female);
                }
            }

            return CheckResultModel.Failure<Gender>();
        }

        public static CheckResultModel<DateTime> GetBirthday(string accountElementInnerText)
        {
            var reg = new Regex(@"
                生年月日
                \s*
                :
                \s*
                (?<BIRTHDAY>
                    .*
                )
                \s*
                ",
                RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
            );
            var match = reg.Match(accountElementInnerText);
            if(match.Success) {
                var s = match.Groups["BIRTHDAY"].Value;
                var d = RawValueUtility.ConvertDateTime(s);
                if(d != RawValueUtility.UnknownDateTime) {
                    return CheckResultModel.Success(d);
                }
            }

            return CheckResultModel.Failure<DateTime>();
        }

        public static CheckResultModel<string> GetLocation(string accountElementInnerText)
        {
            var reg = new Regex(@"
                地域
                \s*
                :
                \s*
                (?<LOCATION>
                    .*
                )
                \s*
                ",
                RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
            );
            var match = reg.Match(accountElementInnerText);
            if(match.Success) {
                var s = match.Groups["LOCATION"].Value;
                if(s.IndexOf("非公開") == -1) {
                    return CheckResultModel.Success(s);
                }
            }

            return CheckResultModel.Failure<string>();
        }

        static HtmlNode FindNodeOrDefaultFromClassName(HtmlNode node, string className)
        {
            var result = node.Descendants()
                .Where(n => n.HasAttributes)
                .FirstOrDefault(n => n.Attributes.Any(a => a.Name == "class") && n.Attributes["class"].Value.Split(null).Select(s => s.Trim()).Any(s => s.Equals(className, StringComparison.OrdinalIgnoreCase)))
            ;

            return result;
        }

        public static CheckResultModel<string> GetPageLink(HtmlNode node, string className)
        {
            var element = FindNodeOrDefaultFromClassName(node, className);
            if(element == null) {
                return CheckResultModel.Failure<string>();
            }

            var anchorElement = element.SelectSingleNode(".//a");
            if(anchorElement == null) {
                return CheckResultModel.Failure<string>();
            }

            var link = anchorElement.Attributes["href"]?.Value;
            if(string.IsNullOrWhiteSpace(link)) {
                return CheckResultModel.Failure<string>();
            }

            return CheckResultModel.Success(link);
        }

        public static int GetMyListCount(string innerText)
        {
            var reg = new Regex(@"
                (?<NUM>
                    [\d,]+
                )
                \s*
                件
                ",
                RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
            );
            var match = reg.Match(innerText);
            if(match.Success) {
                return RawValueUtility.ConvertInteger(match.Groups["NUM"].Value);
            }
            Debug.WriteLine(innerText);
            return 0;
        }
    }
}
