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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live.Player;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live
{
    public class SmileLiveMediation: MediationCustomBase
    {
        public SmileLiveMediation(Mediation mediation, SmileLiveSettingModel setting)
            : base(mediation, Constants.SmileLiveUriListPath, Constants.SmileLiveUriParametersListPath, Constants.SmileLiveRequestHeadersListPath, Constants.SmileVideoRequestParametersListPath, Constants.SmileVideoRequestMappingsListPath)
        {
            Setting = setting;
            Category = SerializeUtility.LoadXmlSerializeFromFile<SmileLiveCategoryModel>(Constants.SmileLiveCategoryPath);
        }

        #region property

        SmileLiveSettingModel Setting { get; }
        SmileLiveCategoryModel Category { get; }

        HashSet<SmileLivePlayerWindow> Players { get; } = new HashSet<SmileLivePlayerWindow>();

        #endregion

        #region function

        ResponseModel Request_WindowViewModels(RequestModel request)
        {
            var windowViewModels = Players.Select(p => (SmileLivePlayerViewModel)p.DataContext).ToList();
            return new ResponseModel(request, windowViewModels);
        }

        ResponseModel RequestCore(RequestModel request)
        {
            switch(request.RequestKind) {
                case RequestKind.CategoryDefine:
                    return new ResponseModel(request, Category);

                case RequestKind.Setting:
                    return new ResponseModel(request, Setting);

                //case RequestKind.CacheData:
                //    return Request_CacheData((SmileVideoInformationCacheRequestModel)request);

                case RequestKind.WindowViewModels:
                    return Request_WindowViewModels(request);

                default:
                    throw new NotImplementedException();
            }
        }

        bool ValueConvertCore(out object outputValue, string inputKey, object inputValue, Type inputType, Type outputType, ServiceType serviceType)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region MediationBase

        protected override string ScriptDirectoryPath { get; } = Path.Combine(Constants.SpaghettiDirectoryPath, Constants.ServiceName, Constants.ServiceSmileName, Constants.ServiceSmileLiveName);

        protected override IEnumerable<string> GetCustomKeys()
        {
            return GetMediationKeys(typeof(SmileLiveMediationKey));
        }

        internal override object RequestShowView(ShowViewRequestModel request)
        {
            CheckUtility.DebugEnforce(request.ServiceType == ServiceType.SmileLive);

            if(request.ParameterIsViewModel) {
                var player = request.ViewModel as SmileLivePlayerViewModel;
                if(player != null) {
                    var window = new SmileLivePlayerWindow() {
                        DataContext = player,
                    };
                    window.Closed += Player_Closed;
                    if(!Players.Any()) {
                        player.IsWorkingPlayer.Value = true;
                    }
                    Players.Add(window);
                    return window;
                }

                throw new NotImplementedException();
            } else {
                throw new NotImplementedException();
            }
        }

        internal override void SetManager(ServiceType serviceType, ManagerPackModelBase managerPack)
        {
            throw new NotImplementedException();
        }

        public override ResponseModel Request(RequestModel request)
        {
            if(request.ServiceType != ServiceType.SmileLive) {
                ThrowNotSupportRequest(request);
            }

            return RequestCore(request);
        }

        public override UriResultModel GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportGetUri(key, replaceMap, serviceType);
            }

            return GetUriCore(key, replaceMap, serviceType);
        }

        public override string ConvertUri(string key, string uri, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportConvertUri(key, uri, serviceType);
            }

            return ConvertUriCore(key, uri, serviceType);
        }

        public override IDictionary<string, string> GetRequestHeader(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportGetRequestHeader(key, replaceMap, serviceType);
            }

            return GetRequestHeaderCore(key, replaceMap, serviceType);
        }

        public override IDictionary<string, string> ConvertRequestHeader(string key, IReadOnlyDictionary<string, string> requestHeaders, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportConvertRequestHeader(key, requestHeaders, serviceType);
            }

            return (IDictionary<string, string>)requestHeaders;
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
            }

            return GetRequestParameterCore(key, replaceMap, serviceType);
        }

        public override MappingResultModel GetRequestMapping(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportGetRequestMapping(key, replaceMap, serviceType);
            }

            return GetRequestMappingCore(key, replaceMap, serviceType);
        }

        public override IDictionary<string, string> ConvertRequestParameter(string key, IReadOnlyDictionary<string, string> requestParams, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportConvertRequestParameter(key, requestParams, serviceType);
            }

            return (IDictionary<string, string>)requestParams;
        }

        public override string ConvertRequestMapping(string key, string mapping, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportConvertRequestMapping(key, mapping, serviceType);
            }

            return mapping;
        }

        public override CheckModel CheckResponseHeader(string key, Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            return CheckResponseHeaderCore(key, uri, headers, serviceType);
        }

        public override void ConvertBinary(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            ConvertBinaryCore(key, uri, stream, serviceType);
        }

        public override Encoding GetEncoding(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            return GetEncodingCore(key, uri, stream, serviceType);
        }

        public override string ConvertString(string key, Uri uri, string text, ServiceType serviceType)
        {
            return text;
        }

        public override bool ConvertValue(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileLive) {
                ThrowNotSupportValueConvert(inputKey, inputValue, inputType, outputType, serviceType);
            }

            return ValueConvertCore(out outputValue, inputKey, inputValue, inputType, outputType, serviceType);
        }

        #endregion

        void Player_Closed(object sender, EventArgs e)
        {
            var window = (SmileLivePlayerWindow)sender;

            window.Closed -= Player_Closed;

            var player = (SmileLivePlayerViewModel)window.DataContext;
            player.IsWorkingPlayer.Value = false;
            Players.Remove(window);

            // 判断基準なし
            var nextPlayer = Players
                .Select(p => (SmileLivePlayerViewModel)p.DataContext)
                .FirstOrDefault()
            ;
            if(nextPlayer != null) {
                nextPlayer.IsWorkingPlayer.Value = true;
            }
        }


    }
}
