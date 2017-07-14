using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    public class WatchData : SessionApiBase<SmileSessionViewModel>
    {
        public WatchData(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        RawSmileVideoWatchDataApiModel ConvertRawApi(string rawApi)
        {
            using(var stream = StreamUtility.ToUtf8Stream(rawApi)) {
                var result = SerializeUtility.LoadJsonDataFromStream<RawSmileVideoWatchDataApiModel>(stream);
                result.Raw = rawApi;
                return result;
            }
        }

        RawSmileVideoWatchDataEnvironmentModel ConvertRawEnvironment(string rawEnvironment)
        {
            using(var stream = StreamUtility.ToUtf8Stream(rawEnvironment)) {
                var result = SerializeUtility.LoadJsonDataFromStream<RawSmileVideoWatchDataEnvironmentModel>(stream);
                result.Raw = rawEnvironment;
                return result;
            }
        }

        RawSmileVideoWatchDataModel Load(HtmlAgilityPack.HtmlNode watachDataElement)
        {
            var rawApiData = watachDataElement.Attributes["data-api-data"].Value;
            var apiData = ConvertRawApi(rawApiData);

            var rawEnvironment = watachDataElement.Attributes["data-environment"].Value;
            var environment = ConvertRawEnvironment(rawEnvironment);

            return new RawSmileVideoWatchDataModel() {
                Api = apiData,
                Environment = environment,
            };
        }

        public async Task<SmileVideoWatchDataModel> LoadAsync(string videoId, Uri watchUri, SmileVideoMovieType movieType, bool usingDmc)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, SessionBase, SmileVideoMediationKey.getflvScraping, ServiceType.SmileVideo)) {
                var usingUri = movieType == SmileVideoMovieType.Swf
                    ? new Uri(watchUri.OriginalString + "?as3=1")
                    : watchUri
                ;
                page.ForceUri = usingUri;
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                if(response.IsSuccess) {
                    var htmlSource = response.Result;
                    var htmlDocument = HtmlUtility.CreateHtmlDocument(htmlSource);
                    var watachDataElement = htmlDocument.GetElementbyId("js-initial-watch-data");
                    var rawModel = Load(watachDataElement);

                    return new SmileVideoWatchDataModel() {
                        RawData = rawModel,
                        HtmlSource = htmlSource,
                    };
                }

                return null;
            }
        }
    }
}
