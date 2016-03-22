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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.IF.Compatibility;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using HTMLConverter;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    public static class SmileVideoDescriptionUtility
    {
        #region define

        const string skipDomainPath = "schemas.microsoft.com/winfx/2006/xaml/presentation";

        #endregion

        static string MakeLink(string link, string text, string commandName)
        {
            var linkElementSource = $@"
                <Hyperlink Command='{{Binding {commandName}}}' CommandParameter='{link}'>
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
                        \w \. \- \( \) \?
                        / _ # $ % & =
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

                var linkElementSource = MakeLink(linkUri, m.Groups[0].Value, nameof(ISmileVideoDescription.OpenWebLinkCommand));

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
                    return MakeLink(link, target, nameof(ISmileVideoDescription.OpenMyListLinkCommand));
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
                    return MakeLink(link, target, nameof(ISmileVideoDescription.OpenVideoLinkCommand));
                } else {
                    return m.Groups[0].Value;
                }
            });

            return replacedSource;
        }

        public static string ConvertSafeXaml(string flowDocumentSource)
        {
            //TODO: Run*てのがあることは考慮してない。
            return flowDocumentSource
                .Replace("Run>", "TextBlock>")
                .Replace("<Run", "<TextBlock")
            ;
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

            convertedFlowDocumentSource = ConvertSafeXaml(convertedFlowDocumentSource);

            return convertedFlowDocumentSource;
        }
    }
}
