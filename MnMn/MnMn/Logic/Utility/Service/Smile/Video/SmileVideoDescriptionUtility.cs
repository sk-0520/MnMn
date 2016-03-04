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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.IF.Compatibility;
using HTMLConverter;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    internal static class SmileVideoDescriptionUtility
    {
        #region define

        const string skipDomainPath = "schemas.microsoft.com/winfx/2006/xaml/presentation";

        public const string linkKeyHttp = "http";
        public const string linkKeyVideo = "video";
        public const string linkKeyMyList = "mylist";

        #endregion

        static string MakeLink(string key, string link, string text)
        {
            var parameter = $"{key}:{link}";
            var linkElementSource = $@"
                <Hyperlink CommandParameter='{parameter}'>
                    <TextBlock Text='{text}' />
                </Hyperlink>
            "
                .SplitLines()
                .Select(s => s.Trim())
            ;

            return string.Join(string.Empty, linkElementSource);
        }

        static string ConvertLinkFromPlainText(string flowDocumentSource)
        {
            var regLink = new Regex(
                @"
                (?<SCHEME>
                    h?
                    ttp
                    s?
                    ://
                )
                (?<DOMAIN_PATH>
                    [
                        \w \. \- \( \)
                        / _ # $ % &
                    ]*
                )
                ",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture
            );

            var replacedSource = regLink.Replace(flowDocumentSource, m => {
                var domainPath = m.Groups["DOMAIN_PATH"].Value;
                if(domainPath.StartsWith(skipDomainPath)) {
                    return m.Groups[0].Value;
                }

                var scheme = m.Groups["SCHEME"].Value;
                if(scheme[0] != 'h') {
                    scheme = "h" + scheme;
                }

                var linkUri = scheme + domainPath;

                var linkElementSource = MakeLink(linkKeyHttp, linkUri, m.Groups[0].Value);

                return linkElementSource;
            });

            return replacedSource;
        }

        static string ConvertRunTarget(string flowDocumentSource, MatchEvaluator func)
        {
            var regTarget = new Regex(
                @"
                    (?<OPEN>
                        <Run>
                    )
                        (?<TARGET>
                            .+?
                        )
                    (?<CLOSE>
                        </Run>
                    )
                ",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture
           );

            var replacedSource = regTarget.Replace(flowDocumentSource, func);

            return replacedSource;
        }

        static string ConvertLinkFromMyList(IConvertCompatibility convertCompatibility, string flowDocumentSource)
        {
            var replacedSource = ConvertRunTarget(flowDocumentSource, m => {
                var target = m.Groups["TARGET"].Value;
                object outputValue;
                if(convertCompatibility.ConvertValue(out outputValue, typeof(string), SmileMediationKey.inputGetMyListId, target, typeof(string), ServiceType.Smile)) {
                    var link = (string)outputValue;
                    return MakeLink(linkKeyMyList, link, target);
                } else {
                    return m.Groups[0].Value;
                }
            });

            return replacedSource;
        }

        static string ConvertLinkFromVideoId(IConvertCompatibility convertCompatibility, string flowDocumentSource)
        {
            var replacedSource = ConvertRunTarget(flowDocumentSource, m => {
                var target = m.Groups["TARGET"].Value;
                object outputValue;
                if(convertCompatibility.ConvertValue(out outputValue, typeof(string), SmileMediationKey.inputGetVideoId, target, typeof(string), ServiceType.Smile)) {
                    var link = (string)outputValue;
                    return MakeLink(linkKeyVideo, link, target);
                } else {
                    return m.Groups[0].Value;
                }
            });

            return replacedSource;
        }

        /// <summary>
        /// 動画説明をそれっぽくXAMLの生データから変換。
        /// </summary>
        /// <param name="htmlSource"></param>
        /// <returns></returns>
        public static string ConvertFlowDocumentFromHtml(IConvertCompatibility convertCompatibility, string htmlSource)
        {
            var flowDocumentSource = HtmlToXamlConverter.ConvertHtmlToXaml(htmlSource, true);

            var convertedFlowDocumentSource = ConvertLinkFromPlainText(flowDocumentSource);
            convertedFlowDocumentSource = ConvertLinkFromMyList(convertCompatibility, convertedFlowDocumentSource);
            convertedFlowDocumentSource = ConvertLinkFromVideoId(convertCompatibility, convertedFlowDocumentSource);

            return convertedFlowDocumentSource;
        }
    }
}
