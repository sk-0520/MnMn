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
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile
{
    public class SmileMediation: MediationCustomBase
    {
        public SmileMediation(Mediation mediation, SmileSettingModel setting)
            : base(mediation, Constants.SmileUriListPath, Constants.SmileUriParametersListPath, Constants.SmileRequestParametersListPath, Constants.SmileRequestMappingsListPath)
        {
            Setting = setting;

            VideoMediation = new SmileVideoMediation(Mediation, Setting.VideoSetting);
        }

        #region property

        SmileSettingModel Setting { get; set; }

        /// <summary>
        /// ニコニコ動画関係。
        /// </summary>
        SmileVideoMediation VideoMediation { get; set; }

        SmileSessionViewModel Session { get; set; }

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
                var model = new SmileUserAccountModel();
                model.User = VariableConstants.OptionValueSmileUserAccountName;
                model.Password = VariableConstants.OptionValueSmileUserAccountPassword;
                Session = new SmileSessionViewModel(Mediation, model);
            }

            return new ResponseModel(request, Session);
        }

        #endregion

        #region MediationBase

        public override ResponseModel Request(RequestModel request)
        {
            switch(request.ServiceType) {
                case ServiceType.Smile:
                    return Request_Impl(request);

                case ServiceType.SmileVideo:
                    return VideoMediation.Request(request);

                default:
                    ThrowNotSupportRequest(request);
                    throw new NotImplementedException();
            }
        }

        public override string GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetUri_Impl(key, replaceMap, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediation.GetUri(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetUri(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertUri(string uri, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return uri;

                case ServiceType.SmileVideo:
                    return VideoMediation.ConvertUri(uri, serviceType);

                default:
                    ThrowNotSupportConvertUri(uri, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetRequestParameter_Impl(key, replaceMap, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediation.GetRequestParameter(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override MappingResult GetRequestMapping(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetRequestMapping_Impl(key, replaceMap, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediation.GetRequestMapping(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override CheckModel CheckResponseHeader(Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return CheckModel.Success();

                case ServiceType.SmileVideo:
                    return VideoMediation.CheckResponseHeader(uri, headers, serviceType);

                default:
                    ThrowNotSupportCheckResponseHeader(uri, headers, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> ConvertRequestParameter(IReadOnlyDictionary<string, string> requestParams, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return (IDictionary<string, string>)requestParams;

                case ServiceType.SmileVideo:
                    return VideoMediation.ConvertRequestParameter(requestParams, serviceType);

                default:
                    ThrowNotSupportConvertRequestParameter(requestParams, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertRequestMapping(string mapping, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return mapping;

                case ServiceType.SmileVideo:
                    return VideoMediation.ConvertRequestMapping(mapping, serviceType);

                default:
                    ThrowNotSupportConvertRequestMapping(mapping, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override byte[] ConvertBinary(Uri uri, byte[] binary, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return binary;

                case ServiceType.SmileVideo:
                    return VideoMediation.ConvertBinary(uri, binary, serviceType);

                default:
                    ThrowNotSupportConvertBinary(uri, binary, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override Encoding GetEncoding(Uri uri, byte[] binary, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return Encoding.UTF8;

                case ServiceType.SmileVideo:
                    return VideoMediation.GetEncoding(uri, binary, serviceType);

                default:
                    ThrowNotSupportGetEncoding(uri, binary, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertString(Uri uri, string text, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return text;

                case ServiceType.SmileVideo:
                    return VideoMediation.ConvertString(uri, text, serviceType);

                default:
                    ThrowNotSupportConvertString(uri, text, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override bool ConvertValue(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    outputValue = null;
                    return false;

                case ServiceType.SmileVideo:
                    return VideoMediation.ConvertValue(out outputValue, outputType, inputKey, inputValue, inputType, serviceType);

                default:
                    ThrowNotSupportValueConvert(inputKey, inputValue, inputType, outputType, serviceType);
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
