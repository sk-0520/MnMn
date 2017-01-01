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
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
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
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    public class SmileVideoMediation: MediationCustomBase
    {
        public SmileVideoMediation(Mediation mediation, SmileVideoSettingModel setting)
            : base(mediation, Constants.SmileVideoUriListPath, Constants.SmileVideoUriParametersListPath, Constants.SmileVideoRequestHeadersListPath, Constants.SmileVideoRequestParametersListPath, Constants.SmileVideoRequestMappingsListPath)
        {
            Setting = setting;

            InformationCaching = new SmileVideoInformationCaching(Mediation);

            Ranking = LoadModelFromFile<SmileVideoRankingModel>(Constants.SmileVideoRankingPath);
            Search = LoadModelFromFile<SmileVideoSearchModel>(Constants.SmileVideoSearchPath);
            AccountMyList = LoadModelFromFile<SmileVideoMyListModel>(Constants.SmileVideoMyListPath);
            Filtering = LoadModelFromFile<SmileVideoFilteringModel>(Constants.SmileVideoFilteringPath);

            GlobalFiltering = new SmileVideoFilteringViweModel(Setting.Comment.Filtering, Setting.FinderFiltering, Filtering);
        }

        #region property

        SmileVideoSettingModel Setting { get; }

        SmileVideoInformationCaching InformationCaching { get; }

        SmileVideoRankingModel Ranking { get; }
        SmileVideoSearchModel Search { get; }
        SmileVideoMyListModel AccountMyList { get; }
        internal SmileVideoFilteringModel Filtering { get; }

        internal SmileVideoManagerPackModel ManagerPack { get; private set; }

        HashSet<SmileVideoPlayerWindow> Players { get; } = new HashSet<SmileVideoPlayerWindow>();

        SmileVideoFilteringViweModel GlobalFiltering { get; }

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

                case SmileVideoCustomSettingKind.CommentFiltering:
                    var filter = GlobalFiltering;
                    return new ResponseModel(request, new SmileVideoFilteringResultModel(filter));

                default:
                    throw new NotImplementedException();
            }
        }

        ResponseModel Request_CacheData(SmileVideoInformationCacheRequestModel request)
        {
            switch(request.InformationSource) {
                case SmileVideoInformationSource.VideoId:
                    return new ResponseModel(request, InformationCaching.LoadFromVideoIdAsync(request.VideoId, request.ThumbCacheSpan));

                case SmileVideoInformationSource.Getthumbinfo:
                    return new ResponseModel(request, InformationCaching.Get(request.Thumb));

                case SmileVideoInformationSource.Search:
                    return new ResponseModel(request, InformationCaching.Get(request.ContentsSearch));

                case SmileVideoInformationSource.Feed:
                    return new ResponseModel(request, InformationCaching.Get(request.Feed, request.InformationFlags));

                default:
                    throw new NotImplementedException();
            }
        }

        ResponseModel Request_WindowViewModels(RequestModel request)
        {
            var windowViewModels = Players.Select(p => (SmileVideoPlayerViewModel)p.DataContext).ToList();
            return new ResponseModel(request, windowViewModels);
        }

        ResponseModel Request_Process(SmileVideoProcessRequestModel request)
        {
            switch(request.Parameter.Process) {
                case SmileVideoProcess.CheckItLater: {
                        var param = (SmileVideoProcessCheckItLaterParameterModel)request.Parameter;
                        var result = ManagerPack.CheckItLaterManager.AddLater(param.VideoItem, param.IsForce);
                        return new ResponseModel(request, result);
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        ResponseModel RequestCore(RequestModel request)
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

                case RequestKind.CacheData:
                    return Request_CacheData((SmileVideoInformationCacheRequestModel)request);

                case RequestKind.WindowViewModels:
                    return Request_WindowViewModels(request);

                case RequestKind.Process:
                    return Request_Process((SmileVideoProcessRequestModel)request);

                default:
                    throw new NotImplementedException();
            }
        }

        bool ValueConvertCore(out object outputValue, string inputKey, object inputValue, Type inputType, Type outputType, ServiceType serviceType)
        {
            switch(inputKey) {
                case SmileVideoMediationKey.inputEconomyMode:
                    outputValue = SmileVideoGetflvUtility.IsEconomyMode((string)inputValue);
                    return true;

                default:
                    outputValue = null;
                    return false;
            }
        }

        #endregion

        #region MediationBase

        protected override string ScriptDirectoryPath { get; } = Path.Combine(Constants.SpaghettiDirectoryPath, Constants.ServiceName, Constants.ServiceSmileName, Constants.ServiceSmileVideoName);

        protected override IEnumerable<string> GetCustomKeys()
        {
            return GetMediationKeys(typeof(SmileVideoMediationKey));
        }

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

        public override string ConvertUri(string key, string uri, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportConvertUri(key, uri, serviceType);
            }

            return ConvertUriCore(key, uri, serviceType);
        }

        public override IDictionary<string, string> GetRequestHeader(string key, IReadOnlyDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetRequestHeader(key, replaceMap, serviceType);
            }

            return GetRequestHeaderCore(key, replaceMap, serviceType);
        }

        public override IDictionary<string, string> ConvertRequestHeader(string key, IReadOnlyDictionary<string, string> requestHeaders, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportConvertRequestHeader(key, requestHeaders, serviceType);
            }

            return (IDictionary<string, string>)requestHeaders;

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

        public override IDictionary<string, string> ConvertRequestParameter(string key, IReadOnlyDictionary<string, string> requestParams, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportConvertRequestParameter(key, requestParams, serviceType);
            }

            return (IDictionary<string, string>)requestParams;
        }

        public override string ConvertRequestMapping(string key, string mapping, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
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

        #endregion

        private void Player_Closed(object sender, EventArgs e)
        {
            var window = (SmileVideoPlayerWindow)sender;

            window.Closed -= Player_Closed;

            var player = (SmileVideoPlayerViewModel)window.DataContext;
            player.IsWorkingPlayer.Value = false;
            Players.Remove(window);

            window.DataContext = null;

            // 判断基準なし
            var nextPlayer = Players
                .Select(p => (SmileVideoPlayerViewModel)p.DataContext)
                .FirstOrDefault()
            ;
            if(nextPlayer != null) {
                nextPlayer.IsWorkingPlayer.Value = true;
            }

            player.Dispose();
        }

    }
}
