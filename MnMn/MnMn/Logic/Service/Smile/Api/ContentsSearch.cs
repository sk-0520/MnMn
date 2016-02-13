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
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api
{
    public class ContentsSearch: ApiBase
    {
        public ContentsSearch(Mediation mediation)
            : base(mediation)
        { }

        string ToContentsSearchServiceString(SmileContentsSearchService searchService)
        {
            var map = new Dictionary<SmileContentsSearchService, string>() {
                { SmileContentsSearchService.Video,          "video" },
                { SmileContentsSearchService.Live,           "live" },
                { SmileContentsSearchService.Illust,         "illust" },
                { SmileContentsSearchService.Manga,          "manga" },
                { SmileContentsSearchService.Book,           "book" },
                { SmileContentsSearchService.Channel,        "channel" },
                { SmileContentsSearchService.ChannelArticle, "channelarticle" },
                { SmileContentsSearchService.News,           "news" },
            };

            return map[searchService];
        }

        string ToSmileContentsSearchTypeString(SmileContentsSearchType searchType)
        {
            var map = new Dictionary<SmileContentsSearchType, string[]>() {
                { SmileContentsSearchType.Tag, new [] { "tagsExact", } },
                { SmileContentsSearchType.Keyword, new [] { "title", "description", "tags" } },
            };

            var typeList = map[searchType];
            var result = string.Join(",", typeList);
            return result;
        }

        string ToOrderByString(OrderBy orderBy)
        {
            var map = new Dictionary<OrderBy, string>() {
                { OrderBy.Ascending, "-" },
                { OrderBy.Descending, "+" },
            };

            return map[orderBy];
        }

        string ToSmileContentsSearchFieldString(SmileContentsSearchField searchField)
        {
            var map = new Dictionary<SmileContentsSearchField, string>() {
                { SmileContentsSearchField.ContentId,              "contentId" },
                { SmileContentsSearchField.Title,                  "title" },
                { SmileContentsSearchField.Description,            "description" },
                { SmileContentsSearchField.Tags,                   "tags" },
                { SmileContentsSearchField.CategoryTags,           "categoryTags" },
                { SmileContentsSearchField.ViewCounter,            "viewCounter" },
                { SmileContentsSearchField.MylistCounter,          "mylistCounter" },
                { SmileContentsSearchField.CommentCounter,         "commentCounter" },
                { SmileContentsSearchField.StartTime,              "startTime" },
                { SmileContentsSearchField.ThumbnailUrl,           "thumbnailUrl" },
                { SmileContentsSearchField.CommunityIcon,          "communityIcon" },
                { SmileContentsSearchField.ScoreTimeshiftReserved, "scoreTimeshiftReserved" },
                { SmileContentsSearchField.LiveStatus,             "liveStatus" },
            };

            return map[searchField];
        }

        public async Task<RawSmileContentsSearchModel> GetAsnc(string searchService, string query, string searchType, string sortField, IEnumerable<string> getFilelds, string orderBy, int fromIndex, int getCount)
        {
            CheckUtility.EnforceNotNullAndNotWhiteSpace(query);

            using(var page = new PageLoader(Mediation, HttpUserAgentHost, SmileMediationKey.contentsSearch, ServiceType.Smile)) {
                page.ReplaceUriParameters["service"] = searchService;
                page.ReplaceUriParameters["query"] = query;
                page.ReplaceUriParameters["targets"] = searchType;
                page.ReplaceUriParameters["fields"] = string.Join(",", getFilelds);
                page.ReplaceUriParameters["filters"] = string.Empty;
                page.ReplaceUriParameters["sort"] = orderBy + sortField;
                page.ReplaceUriParameters["offset"] = fromIndex.ToString();
                page.ReplaceUriParameters["limit"] = getCount.ToString();
                page.ReplaceUriParameters["context"] = "test";
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(response.Result))) {
                    return SerializeUtility.LoadJsonDataFromStream<RawSmileContentsSearchModel>(stream);
                }
            }
        }
        public Task<RawSmileContentsSearchModel> GetAsnc(SmileContentsSearchService searchService, string query, SmileContentsSearchType searchType, SmileContentsSearchField searchField, IEnumerable<SmileContentsSearchField> getFilelds, OrderBy orderBy, int fromIndex, int getCount)
        {
            CheckUtility.EnforceNotNullAndNotWhiteSpace(query);
            return GetAsnc(
                ToContentsSearchServiceString(searchService),
                query,
                ToSmileContentsSearchTypeString(searchType),
                ToSmileContentsSearchFieldString(searchField),
                getFilelds.Select(f => ToSmileContentsSearchFieldString(f)),
                ToOrderByString(orderBy),
                fromIndex,
                getCount
            );
        }
    }
}
