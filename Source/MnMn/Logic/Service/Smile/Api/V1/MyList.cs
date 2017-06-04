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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V1
{
    public class MyList: SessionApiBase<SmileSessionViewModel>
    {
        public MyList(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        /// <summary>
        /// とりあえずマイリスト内のアイテムを取得する。
        /// </summary>
        /// <returns></returns>
        public async Task<RawSmileAccountMyListDefaultModel> LoadAccountDefaultAsync()
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, SessionBase, SmileMediationKey.mylistDefault, ServiceType.Smile)) {
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                var rawJson = response.Result;
                using(var stream = StreamUtility.ToUtf8Stream(rawJson)) {
                    return SerializeUtility.LoadJsonDataFromStream<RawSmileAccountMyListDefaultModel>(stream);
                }
            }
        }

        /// <summary>
        /// 自身のマイリスト一覧を取得する。
        /// </summary>
        /// <returns></returns>
        public async Task<RawSmileAccountMyListGroupModel> LoadAccountGroupAsync()
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, SessionBase, SmileMediationKey.mylistGroup, ServiceType.Smile)) {
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                var rawJson = response.Result;
                using(var stream = StreamUtility.ToUtf8Stream(rawJson)) {
                    return SerializeUtility.LoadJsonDataFromStream<RawSmileAccountMyListGroupModel>(stream);
                }
            }
        }

        /// <summary>
        /// 指定マイリストIDからがさーっと取得する。
        /// </summary>
        /// <param name="myListId"></param>
        /// <returns></returns>
        public async Task<FeedSmileVideoModel> LoadGroupAsync(string myListId)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, SessionBase, SmileMediationKey.mylist, ServiceType.Smile)) {
                page.ReplaceUriParameters["mylist-id"] = myListId;

                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);

                var rawJson = response.Result;
                if(rawJson == null) {
                    return null;
                }
                using(var stream = StreamUtility.ToUtf8Stream(rawJson)) {
                    var result = SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoModel>(stream);

                    result.Channel.Title = SmileMyListUtility.TrimTitle(result.Channel.Title);

                    return result;
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

                var htmlDocument = new HtmlDocument() {
                    OptionAutoCloseOnEnd = true,
                };
                htmlDocument.LoadHtml(response.Result);

                var totalItemCountElement = htmlDocument.DocumentNode.SelectSingleNode("//*[@class='search_total']");
                if(totalItemCountElement == null) {
                    return null;
                }
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
                var paremtElement = htmlDocument.DocumentNode.SelectSingleNode("//*[@class='content_672']");
                var baseElements = paremtElement.SelectNodes(".//div/div").Where(be => be.InnerText.Length > 0);
                foreach(var baseElement in baseElements) {
                    var paragraphElements = baseElement.SelectNodes(".//p")?.ToEvalSequence();
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

                    var finder = new SmileVideoSearchMyListFinderViewModel(Mediation, myListId, myListName, myListItemCount, query, totalItemCountValue, showCount);
                    result.Add(finder);
                }

                return result;
            }
        }

        async Task<SmileJsonResultModel> RequestPost(PageLoader page)
        {
            var response = await page.GetResponseTextAsync(PageLoaderMethod.Post);
            var resultJson = response.Result;
            var json = JObject.Parse(resultJson);
            var result = new SmileJsonResultModel(json);
            Mediation.Logger.Debug(result.Status.ToString());
            return result;
        }

        public async Task<SmileJsonResultModel> AdditionAccountDefaultMyListFromVideo(string videoId, string token)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, Session, SmileMediationKey.mylistDefaultVideoAdd, ServiceType.Smile)) {
                page.ReplaceRequestParameters["video-id"] = videoId;
                page.ReplaceRequestParameters["token"] = token;

                return await RequestPost(page);
            }
        }

        public async Task<SmileJsonResultModel> AdditionAccountMyListFromVideo(string myListId, string threadId, string token)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, Session, SmileMediationKey.mylistVideoAdd, ServiceType.Smile)) {
                page.ReplaceRequestParameters["mylist-id"] = myListId;
                page.ReplaceRequestParameters["thread-id"] = threadId;
                page.ReplaceRequestParameters["token"] = token;

                return await RequestPost(page);
            }
        }

        public async Task<SmileJsonResultModel> RemoveAccountDefaultMyListFromVideo(IEnumerable<string> itemIdList)
        {
            await LoginIfNotLoginAsync();

            var token = await LoadAccountGroupToken(null);
            if(token == null) {
                return SmileJsonResultModel.FailureLoadToken();
            }

            using(var page = new PageLoader(Mediation, Session, SmileMediationKey.mylistDefaultVideoDelete, ServiceType.Smile)) {
                var ms = new MultiStrings(itemIdList);
                page.ReplaceRequestParameters["item-id-list"] = ms.ToString();
                page.ReplaceRequestParameters["token"] = token;

                return await RequestPost(page);
            }
        }

        public async Task<SmileJsonResultModel> RemoveAccountMyListFromVideo(string myListId, IEnumerable<string> itemIdList)
        {
            await LoginIfNotLoginAsync();

            var token = await LoadAccountGroupToken(myListId);
            if(token == null) {
                return SmileJsonResultModel.FailureLoadToken();
            }

            using(var page = new PageLoader(Mediation, Session, SmileMediationKey.mylistVideoDelete, ServiceType.Smile)) {
                var ms = new MultiStrings(itemIdList);
                page.ReplaceRequestParameters["mylist-id"] = myListId;
                page.ReplaceRequestParameters["item-id-list"] = ms.ToString();
                page.ReplaceRequestParameters["token"] = token;

                return await RequestPost(page);
            }
        }

        async Task<string> LoadAccountGroupToken(string myListId)
        {
            await LoginIfNotLoginAsync();

            using(var page = new PageLoader(Mediation, Session, SmileMediationKey.mylistGroupToken, ServiceType.Smile)) {
                page.ReplaceUriParameters["mylist-id"] = myListId;
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                var htmlSource = response.Result;

                var htmlDocument = new HtmlDocument() {
                    OptionAutoCloseOnEnd = true,
                };
                htmlDocument.LoadHtml(htmlSource);

                //var regToken = new Regex(
                //    @"
                //    NicoAPI
                //    \s*
                //    \.
                //    \s*
                //    token
                //    \s*
                //    =
                //    \s*
                //    (\""|')
                //    (?<TOKEN>
                //        (.+)
                //    )
                //    (\""|')
                //    ",
                //    RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace
                //);

                var mylistGroupToken = Mediation.GetExpression(SmileMediationKey.mylistGroupToken, ServiceType.Smile);
                var regToken = mylistGroupToken.Regex;

                var tokenElement2 = htmlDocument.DocumentNode.Descendants()
                    .Where(n => n.Name == "script")
                    .Select(e => e.InnerText)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToEvalSequence()
                    //.FirstOrDefault(s => regToken.IsMatch(s))
                ;

                var tokenElement = htmlDocument.DocumentNode.Descendants()
                    .Where(n => n.Name == "script")
                    .Select(e => e.InnerText)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .FirstOrDefault(s => regToken.IsMatch(s))
                ;

                if(tokenElement == null) {
                    return null;
                }

                var match = regToken.Match(tokenElement);
                var token = match.Groups["TOKEN"];

                return token.Value;
            }
        }

        public async Task<SmileJsonResultModel> CreateAccountGroupAsync(string myListName)
        {
            await LoginIfNotLoginAsync();

            var token = await LoadAccountGroupToken(null);
            if(token == null) {
                return SmileJsonResultModel.FailureLoadToken();
            }

            using(var page = new PageLoader(Mediation, Session, SmileMediationKey.mylistGroupCreate, ServiceType.Smile)) {
                page.ReplaceRequestParameters["name"] = myListName;
                page.ReplaceRequestParameters["token"] = token;

                return await RequestPost(page);
            }
        }

        public async Task<SmileJsonResultModel> DeleteAccountGroupAsync(string myListId)
        {
            await LoginIfNotLoginAsync();

            var token = await LoadAccountGroupToken(myListId);
            if(token == null) {
                return SmileJsonResultModel.FailureLoadToken();
            }
            using(var page = new PageLoader(Mediation, Session, SmileMediationKey.mylistGroupDelete, ServiceType.Smile)) {
                page.ReplaceRequestParameters["mylist-id"] = myListId;
                page.ReplaceRequestParameters["token"] = token;

                return await RequestPost(page);
            }
        }

        public async Task<SmileJsonResultModel> UpdateAccountGroupAsync(string myListId, string myListFolderId, string myListName, string myListSort, string myListDescription, bool isPublic)
        {
            await LoginIfNotLoginAsync();

            var token = await LoadAccountGroupToken(myListId);
            if(token == null) {
                return SmileJsonResultModel.FailureLoadToken();
            }

            using(var page = new PageLoader(Mediation, Session, SmileMediationKey.mylistGroupUpdate, ServiceType.Smile)) {
                page.ReplaceRequestParameters["mylist-id"] = myListId;
                page.ReplaceRequestParameters["token"] = token;
                page.ReplaceRequestParameters["name"] = myListName;
                page.ReplaceRequestParameters["description"] = myListDescription;
                page.ReplaceRequestParameters["public"] = isPublic ? "1" : "0";
                page.ReplaceRequestParameters["sort"] = myListSort;
                page.ReplaceRequestParameters["folder-id"] = myListFolderId;

                return await RequestPost(page);
            }
        }

        public async Task<SmileJsonResultModel> SortAccountGroupAsync(IReadOnlyList<string> myListIds)
        {
            await LoginIfNotLoginAsync();

            var token = await LoadAccountGroupToken(string.Empty);
            if(token == null) {
                return SmileJsonResultModel.FailureLoadToken();
            }

            var multiIds = new MultiStrings(myListIds);

            using(var page = new PageLoader(Mediation, Session, SmileMediationKey.mylistGroupSort, ServiceType.Smile)) {
                page.ReplaceRequestParameters["token"] = token;
                page.ReplaceRequestParameters["group-id-list"] = multiIds.ToString();

                return await RequestPost(page);
            }
        }
    }
}
