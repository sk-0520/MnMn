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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile
{
    public class SmileMediation: MediationCustomBase
    {
        public SmileMediation(Mediation mediation, SmileSettingModel setting)
            : base(mediation, Constants.SmileUriListPath, Constants.SmileUriParametersListPath, Constants.SmileRequestHeadersListPath, Constants.SmileRequestParametersListPath, Constants.SmileRequestMappingsListPath)
        {
            Setting = setting;

            VideoMediation = new SmileVideoMediation(Mediation, Setting.Video);
            LiveMediation = new SmileLiveMediation(Mediation, Setting.Live);
        }

        #region property

        SmileSettingModel Setting { get; set; }

        /// <summary>
        /// ニコニコ動画関係。
        /// </summary>
        internal SmileVideoMediation VideoMediation { get; private set; }
        SmileLiveMediation LiveMediation { get; }

        SmileSessionViewModel Session { get; set; }

        internal SmileManagerPackModel ManagerPack { get; private set; }

        #endregion

        #region function

        ResponseModel RequestCore(RequestModel request)
        {
            switch(request.RequestKind) {
                case RequestKind.Session:
                    return RequestSession(request);

                case RequestKind.Setting:
                    return new ResponseModel(request, Setting);

                default:
                    ThrowNotSupportRequest(request);
                    throw new NotImplementedException();
            }
        }

        ResponseModel RequestSession(RequestModel request)
        {
            if(Session == null || (Session.LoginState == LoginState.None || Session.LoginState == LoginState.Failure)) {
                SmileUserAccountModel model;

                if(VariableConstants.HasOptionSmileUserAccountName && VariableConstants.HasOptionSmileUserAccountPassword) {
                    model = new SmileUserAccountModel() {
                        Name = VariableConstants.OptionValueSmileUserAccountName,
                        Password = VariableConstants.OptionValueSmileUserAccountPassword,
                    };
                } else {
                    model = Setting.Account;
                }
                if(Session == null) {
                    Session = new SmileSessionViewModel(Mediation, model);
                } else {
                    Session.ChangeUserAccountAsync(model).Wait();
                }
            }

            return new ResponseModel(request, Session);
        }

        bool ConvertValueCore(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType)
        {
            switch(inputKey) {
                case SmileMediationKey.inputIsScrapingVideoId:
                    outputValue = SmileIdUtility.IsScrapingVideoId((string)inputValue);
                    return true;

                case SmileMediationKey.inputGetVideoId: {
                        var s = SmileIdUtility.GetVideoId(inputValue as string);
                        outputValue = s;
                        return outputValue != null;
                    }

                case SmileMediationKey.inputGetMyListId: {
                        var s = SmileIdUtility.GetMyListId(inputValue as string);
                        outputValue = s;
                        return outputValue != null;
                    }

                case SmileMediationKey.inputGetUserId: {
                        var s = SmileIdUtility.GetUserId(inputValue as string);
                        outputValue = s;
                        return outputValue != null;
                    }



                default:
                    outputValue = null;
                    return false;

            }
        }

        #endregion

        #region MediationBase

        internal override void SetManager(ServiceType serviceType, ManagerPackModelBase managerPack)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    ManagerPack = (SmileManagerPackModel)managerPack;
                    break;

                case ServiceType.SmileVideo:
                    VideoMediation.SetManager(serviceType, managerPack);
                    break;

                case ServiceType.SmileLive:
                    LiveMediation.SetManager(serviceType, managerPack);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public override ResponseModel Request(RequestModel request)
        {
            switch(request.ServiceType) {
                case ServiceType.Smile:
                    return RequestCore(request);

                case ServiceType.SmileVideo:
                    return VideoMediation.Request(request);

                case ServiceType.SmileLive:
                    return LiveMediation.Request(request);

                default:
                    ThrowNotSupportRequest(request);
                    throw new NotImplementedException();
            }
        }

        public override UriResultModel GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetUriCore(key, replaceMap, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediation.GetUri(key, replaceMap, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediation.GetUri(key, replaceMap, serviceType);

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

                case ServiceType.SmileLive:
                    return LiveMediation.ConvertUri(uri, serviceType);

                default:
                    ThrowNotSupportConvertUri(uri, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestHeader(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetRequestHeaderCore(key, replaceMap, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediation.GetRequestHeader(key, replaceMap, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediation.GetRequestHeader(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestHeader(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> ConvertRequestHeader(IReadOnlyDictionary<string, string> requestHeaders, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return (IDictionary<string, string>)requestHeaders;

                case ServiceType.SmileVideo:
                    return VideoMediation.ConvertRequestHeader(requestHeaders, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediation.ConvertRequestHeader(requestHeaders, serviceType);

                default:
                    ThrowNotSupportConvertRequestHeader(requestHeaders, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetRequestParameterCore(key, replaceMap, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediation.GetRequestParameter(key, replaceMap, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediation.GetRequestParameter(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override MappingResultModel GetRequestMapping(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetRequestMappingCore(key, replaceMap, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediation.GetRequestMapping(key, replaceMap, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediation.GetRequestMapping(key, replaceMap, serviceType);

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

                case ServiceType.SmileLive:
                    return LiveMediation.CheckResponseHeader(uri, headers, serviceType);

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

                case ServiceType.SmileLive:
                    return LiveMediation.ConvertRequestParameter(requestParams, serviceType);

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

                case ServiceType.SmileLive:
                    return LiveMediation.ConvertRequestMapping(mapping, serviceType);

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

                case ServiceType.SmileLive:
                    return LiveMediation.ConvertBinary(uri, binary, serviceType);

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

                case ServiceType.SmileLive:
                    return LiveMediation.GetEncoding(uri, binary, serviceType);

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

                case ServiceType.SmileLive:
                    return LiveMediation.ConvertString(uri, text, serviceType);

                default:
                    ThrowNotSupportConvertString(uri, text, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override bool ConvertValue(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return ConvertValueCore(out outputValue, outputType, inputKey, inputValue, inputType, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediation.ConvertValue(out outputValue, outputType, inputKey, inputValue, inputType, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediation.ConvertValue(out outputValue, outputType, inputKey, inputValue, inputType, serviceType);

                default:
                    ThrowNotSupportValueConvert(inputKey, inputValue, inputType, outputType, serviceType);
                    throw new NotImplementedException();
            }
        }

        object RequestShowViewCore(ShowViewRequestModel request)
        {
            CheckUtility.DebugEnforce(request.ServiceType == ServiceType.Smile);

            if(request.ParameterIsViewModel) {
                throw new NotImplementedException();
            } else {
                var user = request.ShowRequestParameter as SmileOpenUserIdParameterModel;
                if(user != null) {
                    ManagerPack.UsersManager.LoadFromParameterAsync(user).ConfigureAwait(false);
                    return ManagerPack.UsersManager;
                }
            }

            throw new NotImplementedException();
        }

        internal override object RequestShowView(ShowViewRequestModel request)
        {
            switch(request.ServiceType) {
                case ServiceType.Smile:
                    return RequestShowViewCore(request);

                case ServiceType.SmileVideo:
                    return VideoMediation.RequestShowView(request);

                case ServiceType.SmileLive:
                    return LiveMediation.RequestShowView(request);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
