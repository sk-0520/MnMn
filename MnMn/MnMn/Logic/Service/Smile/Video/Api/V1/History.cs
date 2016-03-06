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
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    public class History: SessionApiBase<SmileSessionViewModel>, ISmileVideoScrapingPageHtmlToFeedApi
    {
        public History(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        #region function

        /// <summary>
        /// API使用。
        /// </summary>
        /// <returns></returns>
        public async Task<RawSmileVideoAccountHistoryModel> LoadHistoryAsync()
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.historyApi, ServiceType.SmileVideo)) {
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                if(!response.IsSuccess) {
                    return null;
                }

                var rawJson = response.Result;
                using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawJson))) {
                    return SerializeUtility.LoadJsonDataFromStream<RawSmileVideoAccountHistoryModel>(stream);
                }
            }
        }

        public async Task RemoveVideoAsync(RawSmileVideoAccountHistoryModel model, string videoId)
        {
            await LoginIfNotLoginAsync();

            // 動画IDと実際の視聴データのIDは異なる(公式とかとか)
            var map = model.History.ToDictionary(h => h.VideoId, h => h.ItemId);

            string targetId;
            if(map.TryGetValue(videoId, out targetId)) {
                using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.historyRemove, ServiceType.SmileVideo)) {
                    page.ReplaceRequestParameters["video-id"] = targetId;
                    page.ReplaceRequestParameters["token"] = model.Token;
                    var response = await page.GetResponseTextAsync(PageLoaderMethod.Post);
                    if(!response.IsSuccess) {
                        return;
                    }
                    // TODO: 結果からなんか返さないと。
                }
            }
        }

        #endregion

        #region IScrapingPageHtmlToFeedApi

        public async Task<HtmlDocument> LoadPageHtmlDocument()
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.historyPage, ServiceType.SmileVideo)) {
                var result = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                if(!result.IsSuccess) {
                    return null;
                }

                var htmlSource = result.Result;

                var htmlDocument = new HtmlDocument() {
                    OptionAutoCloseOnEnd = true,
                };
                htmlDocument.LoadHtml(htmlSource);

                return htmlDocument;
            }
        }

        public FeedSmileVideoModel ConvertFeedModelFromPageHtml(HtmlDocument htmlDocument)
        {
            var result = new FeedSmileVideoModel();

            // 各ブロックごとに分割
            var blockElements = htmlDocument.DocumentNode.SelectNodes("//*[@id='historyList']/*");
            foreach(var element in blockElements) {
                var item = new FeedSmileVideoItemModel();

                var thumbContainerElement = element.SelectSingleNode(".//*[@class='thumbContainer']");
                var sectionElement = element.SelectSingleNode(".//*[@class='section']");

                var titleElement = sectionElement.SelectSingleNode(".//h5/a");

                item.Title = titleElement.InnerText;
                item.Link = titleElement.GetAttributeValue("href", string.Empty);

                var detailModel = new RawSmileVideoFeedDetailModel();

                var imageElement = thumbContainerElement.SelectSingleNode(".//a/img");
                detailModel.ThumbnailUrl = imageElement.GetAttributeValue("src", string.Empty);

                var lengthElement = thumbContainerElement.SelectSingleNode(".//*[@class='videoTime']");
                detailModel.Length = lengthElement.InnerText;

                var metadataElement = sectionElement.SelectSingleNode("//*[@class='metadata']");

                var viewElement = metadataElement.SelectSingleNode(".//*[@class='play']");
                detailModel.ViewCounter = viewElement.InnerText.Split(':').Last();

                var commentElement = metadataElement.SelectSingleNode(".//*[@class='comment']");
                detailModel.CommentNum = commentElement.InnerText.Split(':').Last();

                var mylistElement = metadataElement.SelectSingleNode(".//*[@class='mylist']");
                detailModel.MylistCounter = mylistElement.InnerText.Split(':').Last();

                // うーん、ださい
                var dateElement = metadataElement.SelectSingleNode(".//*[@class='posttime']");
                detailModel.FirstRetrieve = dateElement.InnerText.Trim()
                    .Replace("年", "/")
                    .Replace("月", "/")
                    .Replace("日", "")
                    .Replace("投稿", "")
                ;

                item.Description = SmileVideoFeedUtility.ConvertDescriptionFromFeedDetailModel(detailModel);

                result.Channel.Items.Add(item);

            }

            return result;
        }

        #endregion
    }
}
