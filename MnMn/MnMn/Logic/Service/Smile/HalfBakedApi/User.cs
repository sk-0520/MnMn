using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.HalfBakedApi
{
    public class User: SessionApiBase<SmileSessionViewModel>
    {
        public User(Mediation mediation, ServiceType sessionServiceType) 
            : base(mediation, sessionServiceType)
        { }

        #region function

        public RawSmileSimpleUserAccountModel GetSimpleUserAccountModelFromHtmlSource(string htmlSource)
        {
            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            htmlDocument.LoadHtml(htmlSource);

            var regUser = new Regex(
                @"
                var
                \s+
                User
                \s*
                =
                \s*
                \{
                    (?<VALUE>
                        .*
                    )
                \}
                \s*
                ;?
                ",
                RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
            );

            var userElement = htmlDocument.DocumentNode.Descendants()
                .Where(n => n.Name == "script")
                .Select(e => e.InnerText)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .FirstOrDefault(s => regUser.IsMatch(s))
            ;

            if(userElement == null) {
                return null;
            }

            var match = regUser.Match(userElement);
            var rawUser = match.Groups["VALUE"];

            var jsonUser = "{" + rawUser + "}";

            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonUser))) {
                return SerializeUtility.LoadJsonDataFromStream<RawSmileSimpleUserAccountModel>(stream);
            }
        }

        #endregion
    }
}
