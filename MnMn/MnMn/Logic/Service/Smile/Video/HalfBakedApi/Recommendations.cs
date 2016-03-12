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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.HalfBakedApi
{
    public class Recommendations: SessionApiBase<SmileSessionViewModel>
    {
        #region define

        const string paramsKey = "Nico_RecommendationsParams";

        #endregion

        public Recommendations(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        #region property

        public string Seed { get; private set; }
        public string UserTags { get; private set; }

        #endregion

        #region function

        public static RawSmileVideoRecommendModel Load(string s)
        {
            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(s))) {
                var result = SerializeUtility.LoadJsonDataFromStream<RawSmileVideoRecommendModel>(stream);
                return result;
            }
        }

        string GetParamsText(string targetElementText)
        {
            var headIndex = targetElementText.IndexOf(paramsKey);
            var target = targetElementText.Substring(headIndex);
            var dataIndex = target.IndexOf('{');
            var data = target.Substring(dataIndex);

            // TODO: javascript 側の構文を上手いことパース出来るライブラリないんかね
            var tailIndex = data.IndexOf("jQuery");

            var result = data.Substring(0, tailIndex);

            return result;
        }

        RawSmileVideoRecommendModel ConvertRecommend(string htmlSource)
        {
            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            htmlDocument.LoadHtml(htmlSource);


            var regParam = new Regex(
                $@"
                var
                \s+
                {paramsKey}
                \s*
                =
                \s*
                \{{
                ",
                RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
            );

            var paramElement = htmlDocument.DocumentNode.Descendants()
                .Where(n => n.Name == "script")
                .Select(e => e.InnerText)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .FirstOrDefault(s => regParam.IsMatch(s))
            ;

            if(paramElement == null) {
                return null;
            }

            var paramsText = GetParamsText(paramElement);
            var json = JObject.Parse(paramsText);

            var firstData = json["first_data"].ToString();
            var result = Load(firstData);

            Seed = result.Information.Seed;
            UserTags = json["user_tag_param"].ToString();

            return result;
        }

        public async Task<RawSmileVideoRecommendModel> LoadAsync()
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.recommendationPage, ServiceType.SmileVideo)) {
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                var result = ConvertRecommend(response.Result);

                return result;
            }
        }

        public async Task<RawSmileVideoRecommendModel> LoadNextAsync(string pageNumber)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.recommendationApi, ServiceType.SmileVideo)) {
                page.ReplaceUriParameters["user-tags"] = UserTags;
                page.ReplaceUriParameters["seed"] = Seed;
                page.ReplaceUriParameters["page"] = pageNumber;
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                return Load(response.Result);
            }
        }
        #endregion
    }
}
