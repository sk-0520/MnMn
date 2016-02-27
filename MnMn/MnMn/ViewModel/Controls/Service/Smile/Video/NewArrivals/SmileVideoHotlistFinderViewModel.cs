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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.NewArrivals
{
    /// <summary>
    /// TODO: RSS一切関係なし！
    /// </summary>
    public class SmileVideoHotlistFinderViewModel: SmileVideoNewArrivalsFinderViewModel
    {
        public SmileVideoHotlistFinderViewModel(Mediation mediation, string key)
            : base(mediation, key)
        { }

        #region function

        FeedSmileVideoModel ConvertHotlistHtmlToFeedModel(string rawHtml)
        {
            var result = new FeedSmileVideoModel();

            var html = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            html.LoadHtml(rawHtml);

            // 各ブロックごとに分割
            var blockElements = html.DocumentNode.SelectNodes("//*[@class='thumb_col_1']");
            foreach(var element in blockElements) {
                var item = new FeedSmileVideoItemModel();

                var parentElement = element.SelectSingleNode(".//tr");

                var watchElement = parentElement.SelectSingleNode(".//a[@class='watch']");
                item.Title = watchElement.InnerText;
                item.Link =  watchElement.GetAttributeValue("href", string.Empty);
                Debug.WriteLine(item.Title);

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

                //var dateElement = parentElement.SelectSingleNode(".//*[contains(@class,'thumb_num']");
                var dateElement = parentElement.SelectNodes(".//p")
                    .FirstOrDefault(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("thumb_num"))
                    ?.SelectSingleNode("//strong")
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

        #region SmileVideoNewArrivalsFinderViewModel

        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.All;

        protected override async Task LoadAsync_Impl(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            FinderLoadState = SmileVideoFinderLoadState.VideoSourceLoading;
            NowLoading = true;

            string rawHtml;
            using(var page = CreatePageLoader()) {
                var pageResult = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                if(!pageResult.IsSuccess) {
                    FinderLoadState = SmileVideoFinderLoadState.Failure;
                    return;
                }
                rawHtml = pageResult.Result;
            }

            FinderLoadState = SmileVideoFinderLoadState.VideoSourceChecking;
            var feedModel = ConvertHotlistHtmlToFeedModel(rawHtml);

            await Task.Run(() => {
                return feedModel.Channel.Items
                    .AsParallel()
                    .Select((item, index) => new SmileVideoInformationViewModel(Mediation, item, index + 1, SmileVideoInformationFlags.All))
                ;
            }).ContinueWith(task => {
                var cancel = CancelLoading = new CancellationTokenSource();

                VideoInformationList.InitializeRange(task.Result);
                VideoInformationItems.Refresh();

                Task.Run(() => {
                    FinderLoadState = SmileVideoFinderLoadState.InformationLoading;
                    var loader = new SmileVideoInformationLoader(VideoInformationList);
                    return loader.LoadThumbnaiImageAsync(imageCacheSpan);
                }).ContinueWith(t => {
                    //VideoInformationItems.Refresh();
                    FinderLoadState = SmileVideoFinderLoadState.Completed;
                    NowLoading = false;
                    // return Task.CompletedTask;
                }, cancel.Token, TaskContinuationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        #endregion
    }
}
