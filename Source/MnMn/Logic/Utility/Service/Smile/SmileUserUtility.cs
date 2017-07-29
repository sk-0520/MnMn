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
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Model;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile
{
    public static class SmileUserUtility
    {
        public static string GetUserIdFromPinpointHtmlSource(string accountElementInnerText, IExpressionGetter expressionGetter)
        {
            var expression = expressionGetter.GetExpression(SmileMediatorKey.userInformationFromHtml, SmileMediatorKey.Id.userInformationFromHtml_userId, ServiceType.Smile);
            var match = expression.Regex.Match(accountElementInnerText);
            if(match.Success) {
                return match.Groups["USER_ID"].Value;
            }

            return null;
        }

        public static string GetVersionFromPinpointHtmlSource(string accountElementInnerText, IExpressionGetter expressionGetter)
        {
            var expression = expressionGetter.GetExpression(SmileMediatorKey.userInformationFromHtml, SmileMediatorKey.Id.userInformationFromHtml_version, ServiceType.Smile);
            var match = expression.Regex.Match(accountElementInnerText);
            if(match.Success) {
                return match.Groups["VERSION"].Value;
            }

            return null;
        }

        public static bool IsPremiumFromPinpointHtmlSource(string accountElementInnerText, IExpressionGetter expressionGetter)
        {
            var expression = expressionGetter.GetExpression(SmileMediatorKey.userInformationFromHtml, SmileMediatorKey.Id.userInformationFromHtml_isPremium, ServiceType.Smile);
            var match = expression.Regex.Match(accountElementInnerText);
            if(match.Success) {
                var s = match.Groups["ACCOUNT"].Value;
                return s.IndexOf("プレミアム") != -1;
            }

            return false;
        }

        public static IReadOnlyCheckResult<Gender> GetGenderFromPinpointHtmlSource(string accountElementInnerText, IExpressionGetter expressionGetter)
        {
            var expression = expressionGetter.GetExpression(SmileMediatorKey.userInformationFromHtml, SmileMediatorKey.Id.userInformationFromHtml_gender, ServiceType.Smile);
            var match = expression.Regex.Match(accountElementInnerText);
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

        public static IReadOnlyCheckResult<DateTime> GetBirthdayFromPinpointHtmlSource(string accountElementInnerText, IExpressionGetter expressionGetter)
        {
            var expression = expressionGetter.GetExpression(SmileMediatorKey.userInformationFromHtml, SmileMediatorKey.Id.userInformationFromHtml_birthday, ServiceType.Smile);
            var match = expression.Regex.Match(accountElementInnerText);
            if(match.Success) {
                var s = match.Groups["BIRTHDAY"].Value;
                var d = RawValueUtility.ConvertDateTime(s);
                if(d != RawValueUtility.UnknownDateTime) {
                    return CheckResultModel.Success(d);
                }
            }

            return CheckResultModel.Failure<DateTime>();
        }

        public static IReadOnlyCheckResult<string> GetLocationFromPinpointHtmlSource(string accountElementInnerText, IExpressionGetter expressionGetter)
        {
            var expression = expressionGetter.GetExpression(SmileMediatorKey.userInformationFromHtml, SmileMediatorKey.Id.userInformationFromHtml_location, ServiceType.Smile);
            var match = expression.Regex.Match(accountElementInnerText);
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

        public static IReadOnlyCheckResult<string> GetPageLink(HtmlNode node, string className)
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

        public static int GetMyListCountFromPinpointHtmlSource(string innerText, IExpressionGetter expressionGetter)
        {
            var expression = expressionGetter.GetExpression(SmileMediatorKey.userInformationFromHtml, SmileMediatorKey.Id.userInformationFromHtml_mylistCount, ServiceType.Smile);
            var match = expression.Regex.Match(innerText);
            if(match.Success) {
                return RawValueUtility.ConvertInteger(match.Groups["NUM"].Value);
            }
            Debug.WriteLine(innerText);
            return 0;
        }
    }
}
