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
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V2
{
    public class ContentsSearch: ApiBase
    {
        public ContentsSearch(Mediator mediation)
            : base(mediation)
        { }

        public async Task<RawSmileContentsSearchModel> SearchAsync(string searchService, string query, string searchType, string sortField, IEnumerable<string> getFilelds, string orderBy, int fromIndex, int getCount)
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
                page.ReplaceUriParameters["context"] = Constants.ServiceSmileContentsSearchContext;
                var response = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                using(var stream = StreamUtility.ToUtf8Stream(response.Result)) {
                    return SerializeUtility.LoadJsonDataFromStream<RawSmileContentsSearchModel>(stream);
                }
            }
        }
    }
}
