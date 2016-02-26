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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api
{
    public class MyList: SessionApiBase<SmileSessionViewModel>
    {
        public MyList(Mediation mediation, SmileSessionViewModel sessionViewModel)
            :base(mediation, sessionViewModel)
        { }

        /// <summary>
        /// とりあえずマイリスト内のアイテムを取得する。
        /// </summary>
        /// <returns></returns>
        public async Task<RawSmileAccountMyListDefaultModel> GetAccountDefaultAsync()
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, SessionBase, SmileMediationKey.mylistDefault, ServiceType.Smile)) {
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                var rawJson = response.Result;
                using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawJson))) {
                    return SerializeUtility.LoadJsonDataFromStream<RawSmileAccountMyListDefaultModel>(stream);
                }
            }
        }

        /// <summary>
        /// 自身のマイリスト一覧を取得する。
        /// </summary>
        /// <returns></returns>
        public async Task<RawSmileAccountMyListGroupModel> GetAccountGroupAsync()
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, SessionBase, SmileMediationKey.mylistGroup, ServiceType.Smile)) {
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                var rawJson = response.Result;
                using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawJson))) {
                    return SerializeUtility.LoadJsonDataFromStream<RawSmileAccountMyListGroupModel>(stream);
                }
            }
        }

        /// <summary>
        /// 指定未リストIDからがさーっと取得する。
        /// </summary>
        /// <param name="myListId"></param>
        /// <returns></returns>
        public async Task<FeedSmileVideoModel> GetGroupAsync(string myListId)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, SessionBase, SmileMediationKey.mylist, ServiceType.Smile)) {
                page.ReplaceUriParameters["mylist-id"] = myListId;

                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                var rawJson = response.Result;
                using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawJson))) {
                    return SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoModel>(stream);
                }
            }
        }

        public async Task<IList<SmileVideoSearchMyListFinderViewModel>> SearchPage(string query, int pageNumber)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, SessionBase, SmileMediationKey.mylistSearch, ServiceType.Smile)) {
                page.ReplaceUriParameters["query"] = query;
                page.ReplaceUriParameters["page"] = pageNumber.ToString();
                
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                var html = new HtmlDocument();
                html.LoadHtml(response.Result);

                var totalItemCountElement = html.DocumentNode.SelectSingleNode("//*[@class='search_total']");
                var totalItemCountValue = RawValueUtility.ConvertInteger(totalItemCountElement.InnerText);

                var siblingElements = totalItemCountElement.ParentNode.SelectNodes(".//strong");
                var showCountRangeElement = siblingElements.Last();

                var showCountRangeValue = showCountRangeElement.InnerText;
                var showCountRangeValues = showCountRangeValue
                    .Split('-')
                    .Select(s => s.Trim())
                    .Select(s => RawValueUtility.ConvertInteger(s))
                ;
                var head = showCountRangeValues.FirstOrDefault();
                var tail = showCountRangeValues.LastOrDefault();
                var showCount = tail - head + 1;

                var result = new List<SmileVideoSearchMyListFinderViewModel>(showCount);

                // 親となる要素
                var paremtElement = html.DocumentNode.SelectSingleNode("//*[@class='content_672']");
                var baseElements = paremtElement.SelectNodes(".//div/div").Where(be => be.InnerText.Length > 0);
                foreach(var baseElement in baseElements) {
                    var paragraphElements = baseElement.SelectNodes(".//p")?.ToArray();
                    if(paragraphElements == null) {
                        continue;
                    }
                    var headerElement = paragraphElements[0];
                    var mylistElement = headerElement.SelectSingleNode(".//a");
                    var myListName = mylistElement.InnerText;
                    var myListId = mylistElement.Attributes["href"].Value.Split('/').Last();
                    var myListItemCountElement = headerElement.SelectSingleNode(".//strong");
                    var myListItemCountSource = myListItemCountElement.InnerText;
                    var myListItemCountText = string.Concat(myListItemCountSource.TakeWhile(c => char.IsDigit(c)));
                    var myListItemCount = RawValueUtility.ConvertInteger(myListItemCountText);
                    //var descriptionElement = paragraphElements[1];

                    var finder = new SmileVideoSearchMyListFinderViewModel(Mediation, myListId, myListName, myListItemCount, query, totalItemCountValue, showCount);
                    result.Add(finder);
                }

                return result;
                //SmileVideoSearchMyListFinderViewModel finder;
                //if(pageNumber == 0) {
                //    finder = new SmileVideoSearchMyListFinderViewModel(Mediation);
                //} else {
                //    finder = new SmileVideoSearchMyListFinderViewModel(Mediation);
                //}
            }
        }
    }
}
