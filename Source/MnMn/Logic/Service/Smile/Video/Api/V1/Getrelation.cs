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
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    public class Getrelation: ApiBase
    {
        public Getrelation(Mediation mediation)
            : base(mediation)
        { }

        #region function

        public static RawSmileVideoRelatedVideoModel Load(string s)
        {
            using(var stream = StreamUtility.ToUtf8Stream(s)) {
                return SerializeUtility.LoadXmlSerializeFromStream<RawSmileVideoRelatedVideoModel>(stream);
            }
        }

        public Task<RawSmileVideoRelatedVideoModel> LoadAsync(string videoId, int pageNumber, string sort, OrderBy orderBy)
        {
            var page = new PageLoader(Mediation, HttpUserAgentHost, SmileVideoMediationKey.getrelation, ServiceType.SmileVideo);
            page.ReplaceUriParameters["video-id"] = videoId;
            page.ReplaceUriParameters["page"] = pageNumber.ToString();
            page.ReplaceUriParameters["sort"] = sort;
            page.ReplaceUriParameters["order"] = orderBy == OrderBy.Ascending ? Constants.ServiceSmileVideoRelationVideoOrderAscending : Constants.ServiceSmileVideoRelationVideoOrderDescending;

            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(task => {
                page.Dispose();
                var response = task.Result;
                return Load(response.Result);
            });
        }

        #endregion
    }
}
