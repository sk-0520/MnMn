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
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.NicoNico;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.NicoNico;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico
{
    public class NicoNicoMediation: MediationCustomBase
    {
        public NicoNicoMediation(Mediation mediation)
            : base(mediation, Constants.NicoNicoUriListPath, Constants.NicoNicoUriParametersListPath, Constants.NicoNicoRequestParametersListPath)
        {
            VideoMediation = new NicoNicoVideoMediation(Mediation);
        }

        #region property

        /// <summary>
        /// ニコニコ動画関係。
        /// </summary>
        NicoNicoVideoMediation VideoMediation { get; set; }

        NicoNicoSessionViewModel Session { get; set; }

        #endregion

        #region function

        ResponseModel Request_Impl(RequestModel request)
        {
            switch(request.RequestKind) {
                case RequestKind.Session:
                    return RequestSession(request);

                default:
                    ThrowNotSupportRequest(request);
                    throw new NotImplementedException();
            }
        }

        ResponseModel RequestSession(RequestModel request)
        {
            if(Session == null) {
                var model = new NicoNicoUserAccountModel();
                model.User = VariableConstants.NicoNicoUserAccountName;
                model.Password = VariableConstants.NicoNicoUserAccountPassword;
                Session = new NicoNicoSessionViewModel(Mediation, model);
            }

            return new ResponseModel(request, Session);
        }

        #endregion

        #region MediationBase

        public override ResponseModel Request(RequestModel request)
        {
            switch(request.ServiceType) {
                case ServiceType.NicoNico:
                    return Request_Impl(request);

                case ServiceType.NicoNicoVideo:
                    return VideoMediation.Request(request);

                default:
                    ThrowNotSupportRequest(request);
                    throw new NotImplementedException();
            }
        }

        public override string GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                    return GetUri_Impl(key, replaceMap, serviceType);

                case ServiceType.NicoNicoVideo:
                    return VideoMediation.GetUri(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetUri(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertUri(string uri, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                    return uri;

                case ServiceType.NicoNicoVideo:
                    return VideoMediation.ConvertUri(uri, serviceType);

                default:
                    ThrowNotSupportConvertUri(uri, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                    return GetRequestParameter_Impl(key, replaceMap, serviceType);

                case ServiceType.NicoNicoVideo:
                    return VideoMediation.GetRequestParameter(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override CheckModel CheckResponseHeader(Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                    return CheckModel.Success();

                case ServiceType.NicoNicoVideo:
                    return VideoMediation.CheckResponseHeader(uri, headers, serviceType);

                default:
                    ThrowNotSupportCheckResponseHeader(uri, headers, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> ConvertRequestParameter(IReadOnlyDictionary<string, string> requestParams, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                    return (IDictionary<string, string>)requestParams;

                case ServiceType.NicoNicoVideo:
                    return VideoMediation.ConvertRequestParameter(requestParams, serviceType);

                default:
                    ThrowNotSupportConvertRequestParameter(requestParams, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override byte[] ConvertBinary(Uri uri, byte[] binary, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                    return binary;

                case ServiceType.NicoNicoVideo:
                    return VideoMediation.ConvertBinary(uri, binary, serviceType);

                default:
                    ThrowNotSupportConvertBinary(uri, binary, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override Encoding GetEncoding(Uri uri, byte[] binary, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                    return Encoding.UTF8;

                case ServiceType.NicoNicoVideo:
                    return VideoMediation.GetEncoding(uri, binary, serviceType);

                default:
                    ThrowNotSupportGetEncoding(uri, binary, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertString(Uri uri, string text, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                    return text;

                case ServiceType.NicoNicoVideo:
                    return VideoMediation.ConvertString(uri, text, serviceType);

                default:
                    ThrowNotSupportConvertString(uri, text, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override bool ConvertValue(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.NicoNico:
                    outputValue = null;
                    return false;

                case ServiceType.NicoNicoVideo:
                    return VideoMediation.ConvertValue(out outputValue, outputType, inputKey, inputValue, inputType, serviceType);

                default:
                    ThrowNotSupportValueConvert(inputKey, inputValue, inputType, outputType, serviceType);
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}