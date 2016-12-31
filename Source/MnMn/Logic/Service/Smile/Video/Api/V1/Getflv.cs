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
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Attribute;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    /// <summary>
    /// NOTE: 気持ち後回し
    /// </summary>
    public class Getflv: SessionApiBase<SmileSessionViewModel>
    {
        public Getflv(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        #region property
        #endregion

        #region function

        static RawSmileVideoGetflvModel ConvertFromRawData(string rawWwwFormData)
        {
            var result = RawValueUtility.ConvertNameModelFromWWWFormData<RawSmileVideoGetflvModel>(rawWwwFormData);
            result.Raw = rawWwwFormData;

            return result;
        }

        async Task<RawSmileVideoGetflvModel> LoadScrapingAsync(Uri uri, SmileVideoMovieType movieType, bool usingDmc)
        {
            //var re = await GetNormalAsync(uri);
            // WEBから取得してみる
            using(var page = new PageLoader(Mediation, SessionBase, SmileVideoMediationKey.getflvScraping, ServiceType.SmileVideo)) {
                var usingUri = movieType == SmileVideoMovieType.Swf
                    ? new Uri(uri.OriginalString + "?as3=1")
                    : uri
                ;
                page.ForceUri = usingUri;
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                //var document = new HtmlDocument();
                //document.LoadHtml(response.Result);
                //var watchApiDataElement = document.DocumentNode.SelectSingleNode("//*[@id='watchAPIDataContainer']");
                //var watchApiDataText = HtmlEntity.DeEntitize(watchApiDataElement.InnerText);

                //var json = JObject.Parse(watchApiDataText);
                var json = SmileVideoWatchAPIUtility.ConvertJsonFromWatchPage(response.Result);

                var flashvars = json.SelectToken("flashvars");
                var flvInfo = flashvars.SelectToken("flvInfo");
                var rawFlvText = flvInfo.ToString();

                var convertedFlvText = HttpUtility.UrlDecode(rawFlvText);
                var result = ConvertFromRawData(convertedFlvText);

                if(usingDmc) {
                    result.IsDmc = flashvars.SelectToken("isDmc")?.ToString();
                    if(result.IsDmc != null && RawValueUtility.ConvertBoolean(result.IsDmc)) {
                        var dmcInfo = flashvars.SelectToken("dmcInfo");
                        var dmcInfoText = dmcInfo.ToString();
                        var convertedDmcInfo = HttpUtility.UrlDecode(dmcInfoText);
                        result.DmcInfo = convertedDmcInfo;
                    }
                }

                return result;
            }
        }

        async Task<RawSmileVideoGetflvModel> LoadNormalAsync(Uri uri, SmileVideoMovieType movieType)
        {
            using(var page = new PageLoader(Mediation, SessionBase, SmileVideoMediationKey.getflvNormal, ServiceType.SmileVideo)) {
                var usingUri = movieType == SmileVideoMovieType.Swf
                    ? new Uri(uri.OriginalString + "?as3=1")
                    : uri
                ;
                page.ForceUri = usingUri;

                var response = await page.GetResponseTextAsync(Define.PageLoaderMethod.Get);
                var result = ConvertFromRawData(response.Result);
                return result;
            }
        }

        bool IsScrapingPattern(string videoId)
        {
            object resultIsWeb;
            if(Mediation.ConvertValue(out resultIsWeb, typeof(bool), SmileMediationKey.inputIsScrapingVideoId, videoId, typeof(string), ServiceType.Smile)) {
                var result = (bool)resultIsWeb;
                return result;
            } else {
                throw new NotSupportedException(videoId);
            }
        }

        public async Task<RawSmileVideoGetflvModel> LoadAsync(string videoId, Uri watchUri, SmileVideoMovieType movieType, bool usingDmc)
        {
            await LoginIfNotLoginAsync();

            //if(IsScrapingPattern(videoId)) {
            //    return await LoadScrapingAsync(watchUri, movieType);
            //} else {
            //    var map = new StringsModel() {
            //        { "video-id", videoId },
            //    };
            //    var srcUri = Mediation.GetUri(SmileVideoMediationKey.getflvNormal, map, Define.ServiceType.SmileVideo);
            //    var convertedUri = Mediation.ConvertUri(srcUri.Uri, Define.ServiceType.SmileVideo);
            //    var uri = new Uri(convertedUri);
            //    return await LoadNormalAsync(uri, movieType);
            //}
            var usingUri = movieType == SmileVideoMovieType.Swf
                ? new Uri(watchUri.OriginalString + "?as3=1")
                : watchUri
            ;
            return await LoadScrapingAsync(usingUri, movieType, usingDmc);
        }


        #endregion
    }
}
