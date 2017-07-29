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
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
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
    public class SmileMediator: CustomMediatorBase
    {
        public SmileMediator(Mediator mediator, SmileSettingModel setting)
            : base(mediator, Constants.SmileUriListPath, Constants.SmileUriParametersListPath, Constants.SmileRequestHeadersListPath, Constants.SmileRequestParametersListPath, Constants.SmileRequestMappingsListPath, Constants.SmileExpressionsPath)
        {
            Setting = setting;

            VideoMediator = new SmileVideoMediator(Mediator, Setting.Video);
            LiveMediator = new SmileLiveMediator(Mediator, Setting.Live);
        }

        #region property

        SmileSettingModel Setting { get; set; }

        /// <summary>
        /// ニコニコ動画関係。
        /// </summary>
        internal SmileVideoMediator VideoMediator { get; private set; }
        SmileLiveMediator LiveMediator { get; }

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
                    Session = new SmileSessionViewModel(Mediator, model);
                } else {
                    Session.ChangeUserAccountAsync(model).Wait();
                }
            }

            return new ResponseModel(request, Session);
        }

        //bool ConvertValueCore(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType)
        //{
        //    switch(inputKey) {
        //        case SmileMediationKey.inputIsScrapingVideoId:
        //            outputValue = SmileIdUtility.IsScrapingVideoId((string)inputValue);
        //            return true;

        //        case SmileMediationKey.inputGetVideoId: {
        //                var s = SmileIdUtility.GetVideoId(inputValue as string, Mediation);
        //                outputValue = s;
        //                return outputValue != null;
        //            }

        //        case SmileMediationKey.inputGetMyListId: {
        //                var s = SmileIdUtility.GetMyListId(inputValue as string);
        //                outputValue = s;
        //                return outputValue != null;
        //            }

        //        case SmileMediationKey.inputGetUserId: {
        //                var s = SmileIdUtility.GetUserId(inputValue as string);
        //                outputValue = s;
        //                return outputValue != null;
        //            }



        //        default:
        //            outputValue = null;
        //            return false;

        //    }
        //}

        #endregion

        #region MediatorBase

        protected override string ScriptDirectoryPath { get; } = Path.Combine(Constants.SpaghettiDirectoryPath, Constants.ServiceName, Constants.ServiceSmileName);

        protected override IEnumerable<string> GetCustomKeys()
        {
            return GetMediatorKeys(typeof(SmileMediatorKey));
        }

        internal override void SetManager(ServiceType serviceType, ManagerPackModelBase managerPack)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    ManagerPack = (SmileManagerPackModel)managerPack;
                    break;

                case ServiceType.SmileVideo:
                    VideoMediator.SetManager(serviceType, managerPack);
                    break;

                case ServiceType.SmileLive:
                    LiveMediator.SetManager(serviceType, managerPack);
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
                    return VideoMediator.Request(request);

                case ServiceType.SmileLive:
                    return LiveMediator.Request(request);

                default:
                    ThrowNotSupportRequest(request);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyUriResult GetUri(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetUriCore(key, replaceMap, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.GetUri(key, replaceMap, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.GetUri(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetUri(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertUri(string key, string uri, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return ConvertUriCore(key, uri, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.ConvertUri(key, uri, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.ConvertUri(key, uri, serviceType);

                default:
                    ThrowNotSupportConvertUri(key, uri, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestHeader(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetRequestHeaderCore(key, replaceMap, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.GetRequestHeader(key, replaceMap, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.GetRequestHeader(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestHeader(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> ConvertRequestHeader(string key, IDictionary<string, string> requestHeaders, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return ConvertRequestHeaderCore(key, requestHeaders, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.ConvertRequestHeader(key, requestHeaders, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.ConvertRequestHeader(key, requestHeaders, serviceType);

                default:
                    ThrowNotSupportConvertRequestHeader(key, requestHeaders, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetRequestParameterCore(key, replaceMap, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.GetRequestParameter(key, replaceMap, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.GetRequestParameter(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyMappingResult GetRequestMapping(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetRequestMappingCore(key, replaceMap, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.GetRequestMapping(key, replaceMap, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.GetRequestMapping(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestMapping(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyExpression GetExpression(string key, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetExpressionCore(key, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.GetExpression(key, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.GetExpression(key, serviceType);

                default:
                    ThrowNotSupportGetExpression(key, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyExpression GetExpression(string key, string id, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetExpressionCore(key, id, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.GetExpression(key, id, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.GetExpression(key, id, serviceType);

                default:
                    ThrowNotSupportGetExpression(key, id, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyCheck CheckResponseHeader(string key, Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return CheckResponseHeaderCore(key, uri, headers, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.CheckResponseHeader(key, uri, headers, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.CheckResponseHeader(key, uri, headers, serviceType);

                default:
                    ThrowNotSupportCheckResponseHeader(key, uri, headers, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> ConvertRequestParameter(string key, IDictionary<string, string> requestParams, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return ConvertRequestParameterCore(key, requestParams, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.ConvertRequestParameter(key, requestParams, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.ConvertRequestParameter(key, requestParams, serviceType);

                default:
                    ThrowNotSupportConvertRequestParameter(key, requestParams, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertRequestMapping(string key, string mapping, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return ConvertRequestMappingCore(key, mapping, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.ConvertRequestMapping(key, mapping, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.ConvertRequestMapping(key, mapping, serviceType);

                default:
                    ThrowNotSupportConvertRequestMapping(key, mapping, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override void ConvertBinary(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    ConvertBinaryCore(key, uri, stream, serviceType);
                    break;

                case ServiceType.SmileVideo:
                    VideoMediator.ConvertBinary(key, uri, stream, serviceType);
                    break;

                case ServiceType.SmileLive:
                    LiveMediator.ConvertBinary(key, uri, stream, serviceType);
                    break;

                default:
                    ThrowNotSupportConvertBinary(key, uri, stream, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override Encoding GetEncoding(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return GetEncodingCore(key, uri, stream, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.GetEncoding(key, uri, stream, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.GetEncoding(key, uri, stream, serviceType);

                default:
                    ThrowNotSupportGetEncoding(key, uri, stream, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertString(string key, Uri uri, string text, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                    return ConvertStringCore(key, uri, text, serviceType);

                case ServiceType.SmileVideo:
                    return VideoMediator.ConvertString(key, uri, text, serviceType);

                case ServiceType.SmileLive:
                    return LiveMediator.ConvertString(key, uri, text, serviceType);

                default:
                    ThrowNotSupportConvertString(key, uri, text, serviceType);
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
                } else {
                    var channel = request.ShowRequestParameter as SmileOpenChannelIdParameterModel;
                    ManagerPack.ChannelManager.LoadFromParameterAsync(channel).ConfigureAwait(false);
                    return ManagerPack.ChannelManager;
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
                    return VideoMediator.RequestShowView(request);

                case ServiceType.SmileLive:
                    return LiveMediator.RequestShowView(request);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
