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
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// データ連携等々の橋渡し。
    /// </summary>
    public class Mediation: MediationBase
    {
        public Mediation()
            : base()
        {
            NicoNico = new NicoNicoMediation(this);
        }

        #region property

        /// <summary>
        /// ニコニコ関係。
        /// </summary>
        NicoNicoMediation NicoNico { get; set; }

        #endregion

        #region function

        #endregion

        #region MediationBase

        public override ResponseModel Request(RequestModel request)
        {
            CheckUtility.DebugEnforceNotNull(request);

            switch(request.ServiceType) {
                case ServiceType.NicoNico:
                case ServiceType.NicoNicoVideo:
                    return NicoNico.Request(request);

                default:
                    ThrowNotSupportRequest(request);
                    throw new NotImplementedException();
            }
        }


        public override string GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                case ServiceType.NicoNicoVideo:
                    return NicoNico.GetUri(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetUri(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertUri(string uri, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                case ServiceType.NicoNicoVideo:
                    return NicoNico.ConvertUri(uri, serviceType);

                default:
                    ThrowNotSupportConvertUri(uri, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                case ServiceType.NicoNicoVideo:
                    return NicoNico.GetRequestParameter(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> ConvertRequestParameter(IReadOnlyDictionary<string, string> requestParams, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                case ServiceType.NicoNicoVideo:
                    return NicoNico.ConvertRequestParameter(requestParams, serviceType);

                default:
                    ThrowNotSupportConvertRequestParameter(requestParams, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override CheckModel CheckResponseHeader(Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                case ServiceType.NicoNicoVideo:
                    return NicoNico.CheckResponseHeader(uri, headers, serviceType);

                default:
                    ThrowNotSupportCheckResponseHeader(uri, headers, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override byte[] ConvertBinary(Uri uri, byte[] binary, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                case ServiceType.NicoNicoVideo:
                    return NicoNico.ConvertBinary(uri, binary, serviceType);

                default:
                    ThrowNotSupportConvertBinary(uri, binary, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override Encoding GetEncoding(Uri uri, byte[] binary, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                case ServiceType.NicoNicoVideo:
                    return NicoNico.GetEncoding(uri, binary, serviceType);

                default:
                    ThrowNotSupportGetEncoding(uri, binary, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertString(Uri uri, string text, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                case ServiceType.NicoNicoVideo:
                    return NicoNico.ConvertString(uri, text, serviceType);

                default:
                    ThrowNotSupportConvertString(uri, text, serviceType);
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
