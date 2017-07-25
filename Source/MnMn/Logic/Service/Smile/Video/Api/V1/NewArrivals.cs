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
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    public class NewArrivals: ApiBase
    {
        public NewArrivals(Mediator mediator)
            : base(mediator)
        { }

        #region function

        async Task<FeedSmileVideoModel> LoadFeedAsync(string key)
        {
            using(var page = new PageLoader(Mediator, new HttpUserAgentHost(NetworkSetting, Mediator.Logger), key, ServiceType.SmileVideo)) {
                page.ReplaceUriParameters["lang"] = AppUtility.GetCultureName();
                var feedResult = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                if(!feedResult.IsSuccess) {
                    return null;
                } else {
                    using(var stream = StreamUtility.ToUtf8Stream(feedResult.Result)) {
                        return SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoModel>(stream);
                    }
                }
            }
        }

        public Task<FeedSmileVideoModel> LoadNewVideoAsync()
        {
            return LoadFeedAsync(SmileVideoMediationKey.newarrival);
        }
        public Task<FeedSmileVideoModel> LoadNewCommentAsync()
        {
            return LoadFeedAsync(SmileVideoMediationKey.recent);
        }

        #endregion
    }
}
