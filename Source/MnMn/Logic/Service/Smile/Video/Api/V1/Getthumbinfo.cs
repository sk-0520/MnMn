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
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    public class Getthumbinfo: ApiBase
    {
        public Getthumbinfo(Mediation mediation)
            : base(mediation)
        { }

        #region function

        public static RawSmileVideoThumbResponseModel ConvertFromRawData(Stream stream)
        {
            return SerializeUtility.LoadXmlSerializeFromStream<RawSmileVideoThumbResponseModel>(stream);
        }

        async Task<RawSmileVideoThumbResponseModel> LoadAsyncCore(PageLoader page, string videoId)
        {
            page.ReplaceUriParameters["video-id"] = videoId;
            var plainXml = await page.GetResponseTextAsync(Define.PageLoaderMethod.Get);
            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(plainXml.Result))) {
                var result = ConvertFromRawData(stream);
                result.Raw = plainXml.Result;
                return result;
            }
        }

        public async Task<RawSmileVideoThumbResponseModel> LoadAsync(string videoId)
        {
            using(var page = new PageLoader(Mediation, HttpUserAgentHost, SmileVideoMediationKey.getthumbinfo, Define.ServiceType.SmileVideo)) {
                return await LoadAsyncCore(page, videoId);
            }
        }

        #endregion
    }
}
