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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Attribute;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api
{
    /// <summary>
    /// NOTE: 気持ち後回し
    /// </summary>
    public class Getflv: SessionApiBase
    {
        public Getflv(Mediation mediation, SmileSessionViewModel session)
            : base(mediation, session)
        { }

        #region property
        #endregion

        #region function

        public static RawSmileVideoGetflvModel Load(string rawWwwFormData)
        {
            return RawValueUtility.ConvertNameModelFromWWWFormData<RawSmileVideoGetflvModel>(rawWwwFormData);
        }

        async Task<RawSmileVideoGetflvModel> GetScrapingAsync(Uri uri)
        {
            //var re = await GetNormalAsync(uri);
            // WEBから取得してみる
            using(var page = new PageLoader(Mediation, SessionBase, SmileVideoMediationKey.getflvScraping, ServiceType.SmileVideo)) {
                page.ForceUri = uri;
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                var document = new HtmlDocument();
                document.LoadHtml(response.Result);
                var watchApiDataElement = document.DocumentNode.SelectSingleNode("//*[@id='watchAPIDataContainer']");
                var watchApiDataText = HtmlEntity.DeEntitize(watchApiDataElement.InnerText);

                var json = JObject.Parse(watchApiDataText);
                var flashvars = json.SelectToken("flashvars");
                var flvInfo = flashvars.SelectToken("flvInfo");
                var rawFlvText = flvInfo.ToString();
                var convertedFlvText = HttpUtility.UrlDecode(rawFlvText);
                var result = Load(convertedFlvText);

                return result;
            }
        }

        async Task<RawSmileVideoGetflvModel> GetNormalAsync(Uri uri)
        {
            using(var page = new PageLoader(Mediation, SessionBase, SmileVideoMediationKey.getflvNormal, ServiceType.SmileVideo)) {
                page.ForceUri = uri;

                var response = await page.GetResponseTextAsync(Define.PageLoaderMethod.Get);
                var result = Load(response.Result);
                return result;
            }
        }

        public bool IsScrapingPattern(string videoId)
        {
            object resultIsWeb;
            if(Mediation.ConvertValue(out resultIsWeb, typeof(bool), SmileVideoMediationKey.inputScrapingVideo, videoId, typeof(string), ServiceType.SmileVideo)) {
                var result = (bool)resultIsWeb;
                return result;
            } else {
                throw new NotSupportedException(videoId);
            }
        }

        public async Task<RawSmileVideoGetflvModel> GetAsync(string videoId, Uri watchUri)
        {
            await LoginIfNotLoginAsync();

            if(IsScrapingPattern(videoId)) {
                return await GetScrapingAsync(watchUri);
            } else {
                var map = new StringsModel() {
                    { "video-id", videoId },
                };
                var srcUri = Mediation.GetUri(SmileVideoMediationKey.getflvNormal, map, Define.ServiceType.SmileVideo);
                var convertedUri = Mediation.ConvertUri(srcUri, Define.ServiceType.SmileVideo);
                var uri = new Uri(convertedUri);
                return await GetNormalAsync(uri);
            }
        }


        #endregion
    }
}
