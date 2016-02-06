﻿/*
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

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api
{
    public class Getthumbinfo: ApiBase
    {
        public Getthumbinfo(Mediation mediation)
            :base(mediation)
        { }

        #region function

        public static RawSmileVideoThumbResponseModel Load(Stream stream)
        {
            return SerializeUtility.LoadXmlSerializeFromStream<RawSmileVideoThumbResponseModel>(stream);
        }

        async Task<RawSmileVideoThumbResponseModel> GetAsync_Impl(PageScraping page, string videoId)
        {
            page.ReplaceUriParameters["video-id"] = videoId;
            var plainXml = await page.GetResponseTextAsync(Define.HttpMethod.Get);
            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(plainXml.Result))) {
                var result = Load(stream);
                return result;
            }
        }

        public async Task<RawSmileVideoThumbResponseModel> GetAsync(string videoId)
        {
            using(var page = new PageScraping(Mediation, HttpUserAgentHost, SmileVideoMediationKey.getthumbinfo, Define.ServiceType.SmileVideo)) {
                return await GetAsync_Impl(page, videoId);
            }
        }

        #endregion
    }
}
