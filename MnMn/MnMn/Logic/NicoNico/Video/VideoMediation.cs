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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico.Video
{
    public class VideoMediation: MediationCustomBase
    {
        public VideoMediation(Mediation mediation)
            :base(mediation, Constants.NicoNicoVideoUriListPath, Constants.NicoNicoVideoUriParametersListPath, Constants.NicoNicoVideoRequestParametersListPath)
        {
            Ranking = LoadModelFromFile<RankingModel>(Constants.NicoNicoVideoRankingPath);
        }

        #region property

        RankingModel Ranking { get; set; }

        #endregion

        #region function

        ResponseModel Request_Impl(RequestModel request)
        {
            switch(request.RequestKind) {
                case RequestKind.RankingDefine:
                    return new ResponseModel(request, Ranking);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region MediationBase

        public override ResponseModel Request(RequestModel request)
        {
            if(request.ServiceType != ServiceType.NicoNicoVideo) {
                ThrowNotSupportRequest(request);
            }

            return Request_Impl(request);
        }

        public override string GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.NicoNicoVideo) {
                ThrowNotSupportGetUri(key, replaceMap, serviceType);
            }

            return GetUri_Impl(key, replaceMap, serviceType);
        }

        public override string ConvertUri(string uri, ServiceType serviceType)
        {
            if(serviceType != ServiceType.NicoNicoVideo) {
                ThrowNotSupportConvertUri(uri, serviceType);
            }

            return uri;
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.NicoNicoVideo) {
                ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
            }

            return GetRequestParameter_Impl(key, replaceMap, serviceType);
        }

        public override IDictionary<string, string> ConvertRequestParameter(IReadOnlyDictionary<string, string> requestParams, ServiceType serviceType)
        {
            if(serviceType != ServiceType.NicoNicoVideo) {
                ThrowNotSupportConvertRequestParameter(requestParams, serviceType);
            }

            return (IDictionary<string, string>)requestParams;
        }

        public override CheckModel CheckResponseHeader(Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            return CheckModel.Success();
        }

        public override byte[] ConvertBinary(Uri uri, byte[] binary, ServiceType serviceType)
        {
            return binary;
        }

        public override Encoding GetEncoding(Uri uri, byte[] binary, ServiceType serviceType)
        {
            return Encoding.UTF8;
        }

        public override string ConvertString(Uri uri, string text, ServiceType serviceType)
        {
            return text;
        }

        #endregion
    }
}
