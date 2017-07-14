using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
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

        TModel ConvertTModel<TModel>(string json)
            where TModel: RawModelBase, new()
        {
            using(var stream = StreamUtility.ToUtf8Stream(json)) {
                var result = SerializeUtility.LoadJsonDataFromStream<TModel>(stream);
                result.Raw = json;
                return result;
            }
        }

        RawSmileVideoWatchDataModel Load(HtmlAgilityPack.HtmlNode watachDataElement)
        {
            var rawApiData = watachDataElement.Attributes["data-api-data"].Value;
            var apiData = ConvertTModel<RawSmileVideoWatchDataApiModel>(rawApiData);

            var rawEnvironment = watachDataElement.Attributes["data-environment"].Value;
            var environment = ConvertTModel<RawSmileVideoWatchDataEnvironmentModel>(rawEnvironment);

            return new RawSmileVideoWatchDataModel() {
                Api = apiData,
                Environment = environment,
            };
        }

        public SmileVideoWatchDataModel GetWatchData(string htmlSource)
        {
            var htmlDocument = HtmlUtility.CreateHtmlDocument(htmlSource);
            var watachDataElement = htmlDocument.GetElementbyId("js-initial-watch-data");
            var rawModel = Load(watachDataElement);

            return new SmileVideoWatchDataModel() {
                RawData = rawModel,
                HtmlSource = htmlSource,
            };
        }

        public Task<SmileVideoWatchDataModel> LoadWatchDataAsync(Uri watchUri, SmileVideoMovieType movieType)
        {
            return LoadWatchPageHtmlSourceAsync(watchUri, movieType, true).ContinueWith(t => {
                var response = t.Result;
                if(response.IsSuccess) {
                    var htmlSource = response.Result;
                    return GetWatchData(htmlSource);
                }
                return null;
            });
        }

        public async Task<IReadOnlyCheckResult<string>> LoadWatchPageHtmlSourceAsync(Uri watchUri, SmileVideoMovieType movieType, IHttpUserAgentCreator userAgentCreator)
        {
            using(var page = new PageLoader(Mediation, userAgentCreator, SmileVideoMediationKey.watchDataPage, ServiceType.SmileVideo)) {
                page.ReplaceUriParameters["uri"] = watchUri.OriginalString;
                page.ReplaceUriParameters["as3"] = movieType == SmileVideoMovieType.Swf
                    ? "1"
                    : string.Empty
                ;

                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                return response;
            }
        }
        public Task<IReadOnlyCheckResult<string>> LoadWatchPageHtmlSourceAsync(Uri watchUri, SmileVideoMovieType movieType, bool usingSession)
        {
            if(usingSession) {
                return LoginIfNotLoginAsync().ContinueWith(_ => {
                    return LoadWatchPageHtmlSourceAsync(watchUri, movieType, SessionBase);
                }).Unwrap();
            } else {
                return LoadWatchPageHtmlSourceAsync(watchUri, movieType, HttpUserAgentHost);
            }
        }
    }
}
