﻿/*
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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.Compatibility;
using HTMLConverter;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public abstract class DescriptionBase
    {
        #region define

        const string skipDomainPath = "schemas.microsoft.com/winfx/2006/xaml/presentation";

        #endregion

        protected DescriptionBase(IConvertCompatibility convertCompatibility, ServiceType serviceType)
        {
            ConvertCompatibility = convertCompatibility;
            ServiceType = serviceType;
        }

        #region property

        protected IConvertCompatibility ConvertCompatibility { get; }
        protected ServiceType ServiceType { get; }

        #endregion

        #region function

        protected string MakeLinkCore(string link, string text, string commandName)
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

        protected string ConvertRunTarget(string flowDocumentSource, MatchEvaluator func)
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


        public string ConvertLinkFromPlainText(string flowDocumentSource, string commandName)
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

                var linkElementSource = MakeLinkCore(linkUri, m.Groups[0].Value, commandName);

                return linkElementSource;
            });

            return replacedSource;
        }

        public string ConvertSafeXaml(string flowDocumentSource)
        {
            //TODO: Run*てのがあることは考慮してない。
            return flowDocumentSource
                .Replace("Run>", "TextBlock>")
                .Replace("<Run", "<TextBlock")
            ;
        }

        /// <summary>
        /// <see cref="ConvertFlowDocumentFromHtml"/>の内部実装。
        /// </summary>
        /// <param name="flowDocumentSource"></param>
        /// <returns></returns>
        protected abstract string ConvertFlowDocumentFromHtmlCore(string flowDocumentSource);

        /// <summary>
        /// 動画説明をそれっぽくXAMLの生データから変換。
        /// </summary>
        /// <param name="htmlSource"></param>
        /// <returns></returns>
        public string ConvertFlowDocumentFromHtml(string htmlSource)
        {
            var flowDocumentSource = HtmlToXamlConverter.ConvertHtmlToXaml(htmlSource, true);
            var convertedFlowDocumentSource = ConvertFlowDocumentFromHtmlCore(flowDocumentSource);

            return convertedFlowDocumentSource;
        }

        #endregion
    }
}
