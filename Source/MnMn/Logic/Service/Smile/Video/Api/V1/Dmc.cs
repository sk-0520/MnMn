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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1
{
    /// <summary>
    /// 新形式ダウンロード。
    /// </summary>
    public class Dmc: SessionApiBase<SmileSessionViewModel>
    {
        public Dmc(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        #region function

        static RawSmileVideoDmcObjectModel ConvertFromRawData(string s)
        {
            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(s))) {
                var result = SerializeUtility.LoadXmlSerializeFromStream<RawSmileVideoDmcObjectModel>(stream);
                result.Raw = s;
                return result;
            }
        }

        public Task<RawSmileVideoDmcObjectModel> LoadAsync(Uri uri, string method, RawSmileVideoDmcObjectModel param)
        {
            var page = new PageLoader(Mediation, Session, SmileVideoMediationKey.dmc, ServiceType.SmileVideo);

            page.ReplaceUriParameters["api-uri"] = uri.OriginalString;
            page.ReplaceUriParameters["api-uri"] = uri.OriginalString;

            page.ReplaceRequestParameters["recipe-id"] = param.Data.Session.RecipeId;
            page.ReplaceRequestParameters["content-id"] = param.Data.Session.ContentId;
            page.ReplaceRequestParameters["protocol-name"] = param.Data.Session.Protocol.Name;

            var mux = param.Data.Session.ContentSrcIdSets.First().SrcIdToMultiplexers.First();
            page.ReplaceRequestParameters["videos"] = string.Join(string.Empty, mux.VideoSrcIds.Select(s => $"<string>{s}</string>"));
            page.ReplaceRequestParameters["audios"] = string.Join(string.Empty, mux.AudioSrcIds.Select(s => $"<string>{s}</string>"));

            page.ReplaceRequestParameters["heartbeat-lifetime"] = param.Data.Session.KeepMethod.HeartBeat.LifeTime;

            page.ReplaceRequestParameters["token-minus-signature"] = param.Data.Session.OperationAuth.BySignature.Token;
            page.ReplaceRequestParameters["token-only-signature"] = param.Data.Session.OperationAuth.BySignature.Signature;

            page.ReplaceRequestParameters["auth-type"] = param.Data.Session.ContentAuth.AuthType;
            page.ReplaceRequestParameters["service-id"] = param.Data.Session.ContentAuth.ServiceId;
            page.ReplaceRequestParameters["user-id"] = param.Data.Session.ContentAuth.ServiceUserId;
            page.ReplaceRequestParameters["content-key-timeout"] = param.Data.Session.ContentAuth.ContentKeyTimeout;
            page.ReplaceRequestParameters["player-id"] = param.Data.Session.ClientInformation.PlayerId;
            page.ReplaceRequestParameters["priority"] = param.Data.Session.Priority;

            page.ReplaceRequestParameters["file-extension"] = Constants.ServiceSmileVideoDownloadDmcExtension;


            return page.GetResponseTextAsync(PageLoaderMethod.Post).ContinueWith(t => {
                var res = t.Result;

                return ConvertFromRawData(res.Result);
            });
        }

        #endregion
    }
}
