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
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.HalfBakedApi
{
    public class HotList: ApiBase, ISmileVideoScrapingPageHtmlToFeedApi
    {
        public HotList(Mediation mediation)
            : base(mediation)
        { }

        #region property



        #endregion

        #region IScrapingPageHtmlToFeedApi

        public async Task<HtmlDocument> LoadPageHtmlDocument()
        {
            using(var page = new PageLoader(Mediation, new HttpUserAgentHost(NetworkSetting, Mediation.Logger), SmileVideoMediationKey.hotlist, ServiceType.SmileVideo)) {
                page.ReplaceUriParameters["lang"] = AppUtility.GetCultureName();

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
            var blockElements = htmlDocument.DocumentNode.SelectNodes("//*[@class='thumb_col_1']");
            foreach(var element in blockElements) {
                var item = new FeedSmileVideoItemModel();

                var parentElement = element.SelectSingleNode(".//tr");

                var watchElement = parentElement.SelectSingleNode(".//a[@class='watch']");
                item.Title = watchElement.InnerText;
                item.Link = watchElement.GetAttributeValue("href", string.Empty);

                var detailModel = new RawSmileVideoFeedDetailModel();
                var imageElement = parentElement.SelectSingleNode(".//img[@class='img_std96']");
                detailModel.ThumbnailUrl = imageElement.GetAttributeValue("src", string.Empty);

                var lengthElement = parentElement.SelectSingleNode(".//*[@class='vinfo_length']");
                detailModel.Length = lengthElement.InnerText;

                var viewElement = parentElement.SelectSingleNode(".//*[@class='vinfo_view']");
                detailModel.ViewCounter = viewElement.InnerText;

                var commentElement = parentElement.SelectSingleNode(".//*[@class='vinfo_res']");
                detailModel.CommentNum = commentElement.InnerText;

                var descriptionElement = parentElement.SelectSingleNode(".//*[@class='vinfo_description']");
                detailModel.Description = descriptionElement.InnerText;

                var dateElement = parentElement.SelectNodes(".//p")
                    .FirstOrDefault(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("thumb_num"))
                    ?.SelectSingleNode("./strong")
                ;
                detailModel.FirstRetrieve = dateElement.InnerText;

                var mylistElement = parentElement.SelectSingleNode(".//*[@class='vinfo_mylist']");
                detailModel.MylistCounter = mylistElement.InnerText;

                item.Description = SmileVideoFeedUtility.ConvertDescriptionFromFeedDetailModel(detailModel);

                result.Channel.Items.Add(item);
            }

            return result;
        }

        #endregion
    }
}
