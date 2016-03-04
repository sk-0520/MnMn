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
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    public class SmileVideoMediation: MediationCustomBase
    {
        public SmileVideoMediation(Mediation mediation, SmileVideoSettingModel  setting)
            :base(mediation, Constants.SmileVideoUriListPath, Constants.SmileVideoUriParametersListPath, Constants.SmileVideoRequestParametersListPath, Constants.SmileVideoRequestMappingsListPath)
        {
            Setting = setting;

            Ranking = LoadModelFromFile<SmileVideoRankingModel>(Constants.SmileVideoRankingPath);
            Search = LoadModelFromFile<SmileVideoSearchModel>(Constants.SmileVideoSearchPath);
            AccountMyList = LoadModelFromFile<SmileVideoMyListModel>(Constants.SmileVideoMyListPath);
        }

        #region property

        SmileVideoSettingModel Setting { get; }

        SmileVideoRankingModel Ranking { get; }
        SmileVideoSearchModel Search { get; }
        SmileVideoMyListModel AccountMyList { get; }

        internal SmileVideoManagerPackModel ManagerPack { get; private set; }

        #endregion

        #region function

        ResponseModel Request_CustomSetting(SmileVideoCustomSettingRequestModel request)
        {
            switch(request.CustomSettingKind) {
                case SmileVideoCustomSettingKind.Search:
                    return new ResponseModel(request, new SmileVideoSearchSettingResultModel(ManagerPack.SearchManager.SelectedMethod, ManagerPack.SearchManager.SelectedSort));

                case SmileVideoCustomSettingKind.MyList:
                    var list = ManagerPack.MyListManager.AccountMyListViewer;
                    return new ResponseModel(request, new SmileVideoAccountMyListSettingResultModel(list));

                default:
                    throw new NotImplementedException();
            }
        }

        ResponseModel Request_Impl(RequestModel request)
        {
            switch(request.RequestKind) {
                case RequestKind.RankingDefine:
                    return new ResponseModel(request, Ranking);

                case RequestKind.SearchDefine:
                    return new ResponseModel(request, Search);

                case RequestKind.PlayListDefine:
                    return new ResponseModel(request, AccountMyList);

                case RequestKind.Setting:
                    return new ResponseModel(request, Setting);

                case RequestKind.CustomSetting:
                    return Request_CustomSetting((SmileVideoCustomSettingRequestModel)request);

                default:
                    throw new NotImplementedException();
            }
        }

        bool ValueConvert_Impl(out object outputValue, string inputKey, object inputValue, Type inputType, Type outputType, ServiceType serviceType)
        {
            switch(inputKey) {
                case SmileVideoMediationKey.inputEconomyMode:
                    outputValue = SmileVideoGetflvUtility.isEconomyMode((string)inputValue);
                    return true;

                default:
                    outputValue = null;
                    return false;
            }
        }

        #endregion

        #region MediationBase

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

            return Request_Impl(request);
        }

        public override UriResultModel GetUri(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetUri(key, replaceMap, serviceType);
            }

            return GetUri_Impl(key, replaceMap, serviceType);
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

            return GetRequestParameter_Impl(key, replaceMap, serviceType);
        }

        public override MappingResultModel GetRequestMapping(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetRequestMapping(key, replaceMap, serviceType);
            }

            return GetRequestMapping_Impl(key, replaceMap, serviceType);
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
                    return ValueConvert_Impl(out outputValue, inputKey, inputValue, inputType, outputType, serviceType);

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
                    return window;
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
                        ManagerPack.MyListManager.IsSelectedLocal = false;
                        ManagerPack.MyListManager.IsSelectedHistory = false;
                        ManagerPack.MyListManager.IsSelectedSearch = true;
                        return ManagerPack.MyListManager;
                    }
                }
            }

            throw new NotImplementedException();
        }

        #endregion
    }
}
