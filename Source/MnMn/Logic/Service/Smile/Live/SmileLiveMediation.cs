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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Live;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live
{
    public class SmileLiveMediation: MediationCustomBase
    {
        public SmileLiveMediation(Mediation mediation, SmileLiveSettingModel setting)
            : base(mediation, Constants.SmileLiveUriListPath, Constants.SmileLiveUriParametersListPath, Constants.SmileVideoRequestParametersListPath, Constants.SmileVideoRequestMappingsListPath)
        {

        }

        #region MediationBase


        internal override object RequestShowView(ShowViewRequestModel reque)
        {
            throw new NotImplementedException();
        }

        internal override void SetManager(ServiceType serviceType, ManagerPackModelBase managerPack)
        {
            throw new NotImplementedException();
        }

        /*
        internal override void SetManager(ServiceType serviceType, ManagerPackModelBase managerPack)
        {
            CheckUtility.Enforce(serviceType == ServiceType.SmileVideo);

            ManagerPack = (SmileVideoManagerPackModel)managerPack;
        }

        public override ResponseModel Request(RequestModel request)
        {
            if(request.ServiceType != ServiceType.SmileVideo) {
                ThrowNotSupportRequest(request);
            }

            return RequestCore(request);
        }

        public override UriResultModel GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetUri(key, replaceMap, serviceType);
            }

            return GetUriCore(key, replaceMap, serviceType);
        }

        public override string ConvertUri(string uri, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportConvertUri(uri, serviceType);
            }

            return uri;
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
            }

            return GetRequestParameterCore(key, replaceMap, serviceType);
        }

        public override MappingResultModel GetRequestMapping(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetRequestMapping(key, replaceMap, serviceType);
            }

            return GetRequestMappingCore(key, replaceMap, serviceType);
        }

        public override IDictionary<string, string> ConvertRequestParameter(IReadOnlyDictionary<string, string> requestParams, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportConvertRequestParameter(requestParams, serviceType);
            }

            return (IDictionary<string, string>)requestParams;
        }

        public override string ConvertRequestMapping(string mapping, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportConvertRequestMapping(mapping, serviceType);
            }

            return mapping;
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

        public override bool ConvertValue(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportValueConvert(inputKey, inputValue, inputType, outputType, serviceType);
            }

            switch(serviceType) {
                case ServiceType.SmileVideo:
                    return ValueConvertCore(out outputValue, inputKey, inputValue, inputType, outputType, serviceType);

                default:
                    ThrowNotSupportValueConvert(inputKey, inputValue, inputType, outputType, serviceType);
                    throw new NotImplementedException();
            }
        }

        internal override object RequestShowView(ShowViewRequestModel request)
        {
            CheckUtility.DebugEnforce(request.ServiceType == ServiceType.SmileVideo);

            if(request.ParameterIsViewModel) {
                var player = request.ViewModel as SmileVideoPlayerViewModel;
                if(player != null) {
                    var window = new SmileVideoPlayerWindow() {
                        DataContext = player,
                    };
                    window.Closed += Player_Closed;
                    if(!Players.Any()) {
                        player.IsWorkingPlayer.Value = true;
                    }
                    Players.Add(window);
                    return window;
                }
                var information = request.ViewModel as SmileVideoInformationViewModel;
                if(information != null) {
                    var plaingItem = Players
                        .Select(w => new { View = w, ViewModel = w.DataContext as SmileVideoPlayerViewModel })
                        .Where(p => p.ViewModel != null)
                        .FirstOrDefault(p => p.ViewModel.VideoId == information.VideoId)
                    ;

                    return plaingItem.View;
                }
            } else {
                var finder = request.ShowRequestParameter as SmileVideoSearchParameterModel;
                if(finder != null) {
                    ManagerPack.SearchManager.LoadSearchFromParameterAsync(finder).ConfigureAwait(false);
                    return ManagerPack.SearchManager;
                } else {
                    var mylist = request.ShowRequestParameter as SmileVideoSearchMyListParameterModel;
                    if(mylist != null) {
                        ManagerPack.MyListManager.SearchUserMyListFromParameterAsync(mylist).ConfigureAwait(false);
                        ManagerPack.MyListManager.IsSelectedAccount = false;
                        ManagerPack.MyListManager.IsSelectedBookmark = false;
                        ManagerPack.MyListManager.IsSelectedHistory = false;
                        ManagerPack.MyListManager.IsSelectedSearch = true;
                        return ManagerPack.MyListManager;
                    }
                }
            }

            throw new NotImplementedException();
        }
        */


        #endregion

    }
}
