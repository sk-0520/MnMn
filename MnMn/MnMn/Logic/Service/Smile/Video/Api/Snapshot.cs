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
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api
{
    public class Snapshot: ApiBase
    {
        public Snapshot(Mediation mediation)
            : base(mediation)
        { }

        string ToSearchTypeString(SmileVideoSnapshotSearchType searchType)
        {
            var map = new Dictionary<SmileVideoSnapshotSearchType, string[]>() {
                { SmileVideoSnapshotSearchType.Tag, new [] { "tags_exact" } },
                { SmileVideoSnapshotSearchType.Keyword, new [] { "title", "description", "tags" } },
            };

            var result = string.Join(", ", map[searchType].Select(s => $"\"{s}\""));
            return result;
        }

        string ToSortByString(SmileVideoSnapshotSortBy sortBy)
        {
            var map = new Dictionary<SmileVideoSnapshotSortBy, string>() {
                { SmileVideoSnapshotSortBy.LastCommentTime,  "last_comment_time" },
                { SmileVideoSnapshotSortBy.ViewCounter,      "view_counter" },
                { SmileVideoSnapshotSortBy.StartTime,        "start_time" },
                { SmileVideoSnapshotSortBy.MylistCounter,    "mylist_counter" },
                { SmileVideoSnapshotSortBy.CommentCounter,   "comment_counter" },
                { SmileVideoSnapshotSortBy.LengthSeconds,    "length_seconds" },
            };

            return map[sortBy];
        }

        string ToOrderByString(OrderBy orderBy)
        {
            var map = new Dictionary<OrderBy, string>() {
                { OrderBy.Asc, "asc" },
                { OrderBy.Desc, "desc" },
            };

            return map[orderBy];
        }

        public async Task GetAsnc(SmileVideoSnapshotSearchType searchType, string query, SmileVideoSnapshotSortBy sortBy, OrderBy orderBy, int fromIndex, int getCount)
        {
            CheckUtility.EnforceNotNullAndNotWhiteSpace(query);

            using(var page = new PageScraping(Mediation, HttpUserAgentHost, SmileVideoMediationKey.snapshot, ServiceType.SmileVideo)) {
                page.ParameterType = ParameterType.Mapping;
                page.ReplaceRequestParameters["query"] = query;
                page.ReplaceRequestParameters["search"] = ToSearchTypeString(searchType);
                page.ReplaceRequestParameters["from"] = fromIndex.ToString();
                page.ReplaceRequestParameters["size"] = getCount.ToString();
                page.ReplaceRequestParameters["sort_by"] = ToSortByString(sortBy);
                page.ReplaceRequestParameters["order"] = ToOrderByString(orderBy);
                page.ReplaceRequestParameters["issuer"] = query;

                var response = await page.GetResponseTextAsync(HttpMethod.Post);
            }
        }
    }
}
