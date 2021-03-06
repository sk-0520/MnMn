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
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
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
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video
{
    public class SmileVideoMediator: CustomMediatorBase
    {
        public SmileVideoMediator(Mediator mediator, SmileVideoSettingModel setting)
            : base(mediator, Constants.SmileVideoUriListPath, Constants.SmileVideoUriParametersListPath, Constants.SmileVideoRequestHeadersListPath, Constants.SmileVideoRequestParametersListPath, Constants.SmileVideoRequestMappingsListPath, Constants.SmileVideoExpressionsPath)
        {
            Setting = setting;

            InformationCaching = new SmileVideoInformationCacher(Mediator);

            Ranking = LoadModelFromFile<SmileVideoRankingModel>(Constants.SmileVideoRankingPath);
            Search = LoadModelFromFile<SmileVideoSearchModel>(Constants.SmileVideoSearchPath);
            AccountMyList = LoadModelFromFile<SmileVideoMyListModel>(Constants.SmileVideoMyListPath);
            Filtering = LoadModelFromFile<SmileVideoFilteringModel>(Constants.SmileVideoFilteringPath);
            Keyword = LoadModelFromFile<SmileVideoKeywordModel>(Constants.SmileVideoKeywordPath);

            GlobalFiltering = new SmileVideoFilteringViweModel(Setting.Comment.Filtering, Setting.FinderFiltering, Filtering);
        }

        #region property

        SmileVideoSettingModel Setting { get; }

        SmileVideoInformationCacher InformationCaching { get; }

        SmileVideoRankingModel Ranking { get; }
        SmileVideoSearchModel Search { get; }
        SmileVideoMyListModel AccountMyList { get; }
        internal SmileVideoFilteringModel Filtering { get; }
        IReadOnlySmileVideoKeyword Keyword { get; }

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

                case SmileVideoCustomSettingKind.Bookmark:
                    var sysNodes = ManagerPack.BookmarkManager.SystemNodes;
                    var userNodes = ManagerPack.BookmarkManager.UserNodes;
                    return new ResponseModel(request, new SmileVideoBookmarkResultModel(sysNodes, userNodes));

                default:
                    throw new NotImplementedException();
            }
        }

        ResponseModel Request_OtherDefine(SmileVideoOtherDefineRequestModel request)
        {
            switch(request.OtherDefineKind) {
                case SmileVideoOtherDefineKind.Keyword:
                    return new ResponseModel(request, Keyword);

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

                case SmileVideoInformationSource.ContentsSearch:
                    return new ResponseModel(request, InformationCaching.Get(request.ContentsSearch));

                case SmileVideoInformationSource.OfficialSearch:
                    return new ResponseModel(request, InformationCaching.Get(request.OfficialSearch));

                case SmileVideoInformationSource.Feed:
                    return new ResponseModel(request, InformationCaching.Get(request.Feed, request.InformationFlags));

                default:
                    throw new NotImplementedException();
            }
        }

        ResponseModel Request_WindowViewModels(RequestModel request)
        {
            var windowViewModels = Players
                .Select(p => (SmileVideoPlayerViewModel)p.DataContext)
                .ToEvaluatedSequence()
            ;
            return new ResponseModel(request, windowViewModels);
        }

        ResponseModel Request_Process(ProcessRequestModelBase request)
        {
            var smileProcessRequest = request as SmileVideoProcessRequestModel;
            if(smileProcessRequest != null) {
                switch(smileProcessRequest.Parameter.Process) {
                    case SmileVideoProcess.CheckItLater: {
                            var param = (SmileVideoProcessCheckItLaterParameterModel)smileProcessRequest.Parameter;
                            var result = ManagerPack.CheckItLaterManager.AddLater(param.VideoItem, param.CheckItLaterFrom, param.IsForce);
                            return new ResponseModel(request, result);
                        }

                    case SmileVideoProcess.Bookmark: {
                            var param = (SmileVideoProcessBookmarkParameterModel)smileProcessRequest.Parameter;
                            if(param.IsNewNode) {
                                var result = ManagerPack.BookmarkManager.CreateBookmark(param.ParentBookmark, param.NewNode);
                                return new ResponseModel(request, result);
                            } else {
                                if(param.AddItems) {
                                    var result = ManagerPack.BookmarkManager.AddBookmarkItems(param.ParentBookmark, param.VideoItems);
                                    return new ResponseModel(request, result);
                                } else {
                                    var result = ManagerPack.BookmarkManager.InitializeBookmarkItems(param.ParentBookmark, param.VideoItems);
                                    return new ResponseModel(request, result);
                                }
                            }
                        }

                    case SmileVideoProcess.UnorganizedBookmark: {
                            var param = (SmileVideoProcessUnorganizedBookmarkParameterModel)smileProcessRequest.Parameter;
                            var result = ManagerPack.BookmarkManager.AddUnorganizedBookmark(param.VideoItem);
                            return new ResponseModel(request, result);
                        }

                    case SmileVideoProcess.SearchBookmark: {
                            var param = (SmileVideoProcessSearchBookmarkParameterModel)smileProcessRequest.Parameter;
                            var item = new SmileVideoSearchBookmarkItemModel() {
                                Query = param.Query,
                                SearchType = param.SearchType,
                            };
                            if(param.AddBookmark) {
                                var result = ManagerPack.SearchManager.AddBookmarkAsync(item);
                                return new ResponseModel(request, result);
                            } else {
                                var result = ManagerPack.SearchManager.RemoveBookmark(item);
                                return new ResponseModel(request, result);
                            }
                        }

                    default:
                        throw new NotImplementedException();
                }
            }

            var webProcessRequest = request as WebNavigatorProcessRequestModel;
            if(webProcessRequest != null) {
                switch(webProcessRequest.Parameter.Key) {
                    case WebNavigatorContextMenuKey.smileVideoOpenPlayer:
                    case WebNavigatorContextMenuKey.smileVideoAddCheckItlater:
                    case WebNavigatorContextMenuKey.smileVideoAddUnorganizedBookmark: {
                            var videoId = webProcessRequest.Parameter.ParameterVaule;
                            var videoInformationRequest = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));
                            Mediator.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(videoInformationRequest).ContinueWith(t => {
                                var videoInformation = t.Result;

                                switch(webProcessRequest.Parameter.Key) {
                                    case WebNavigatorContextMenuKey.smileVideoOpenPlayer:
                                        videoInformation.OpenVideoDefaultAsync(false);
                                        break;

                                    case WebNavigatorContextMenuKey.smileVideoAddCheckItlater:
                                        Mediator.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessCheckItLaterParameterModel(videoInformation.ToVideoItemModel(), SmileVideoCheckItLaterFrom.ManualOperation, true)));
                                        break;

                                    case WebNavigatorContextMenuKey.smileVideoAddUnorganizedBookmark:
                                        Mediator.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessUnorganizedBookmarkParameterModel(videoInformation.ToVideoItemModel())));
                                        break;

                                    default:
                                        throw new NotImplementedException();
                                }
                            }, TaskScheduler.FromCurrentSynchronizationContext());

                            return new ResponseModel(request, videoId);
                        }


                    default:
                        throw new NotImplementedException();
                }
            }

            throw new NotImplementedException();
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

                case RequestKind.OtherDefine:
                    return Request_OtherDefine((SmileVideoOtherDefineRequestModel)request);

                case RequestKind.CacheData:
                    return Request_CacheData((SmileVideoInformationCacheRequestModel)request);

                case RequestKind.WindowViewModels:
                    return Request_WindowViewModels(request);

                case RequestKind.Process:
                    return Request_Process((ProcessRequestModelBase)request);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region MediatorBase

        protected override string ScriptDirectoryPath { get; } = Path.Combine(Constants.SpaghettiDirectoryPath, Constants.ServiceName, Constants.ServiceSmileName, Constants.ServiceSmileVideoName);

        protected override IEnumerable<string> GetCustomKeys()
        {
            return GetMediatorKeys(typeof(SmileVideoMediatorKey));
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

        public override IReadOnlyUriResult GetUri(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
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

        public override IDictionary<string, string> GetRequestHeader(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetRequestHeader(key, replaceMap, serviceType);
            }

            return GetRequestHeaderCore(key, replaceMap, serviceType);
        }

        public override IDictionary<string, string> ConvertRequestHeader(string key, IDictionary<string, string> requestHeaders, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportConvertRequestHeader(key, requestHeaders, serviceType);
            }

            return ConvertRequestHeaderCore(key, requestHeaders, serviceType);
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
            }

            return GetRequestParameterCore(key, replaceMap, serviceType);
        }

        public override IReadOnlyMappingResult GetRequestMapping(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetRequestMapping(key, replaceMap, serviceType);
            }

            return GetRequestMappingCore(key, replaceMap, serviceType);
        }

        public override IReadOnlyExpression GetExpression(string key, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetExpression(key, serviceType);
            }

            return GetExpressionCore(key, serviceType);
        }

        public override IReadOnlyExpression GetExpression(string key, string id, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportGetExpression(key, id, serviceType);
            }

            return GetExpressionCore(key, id, serviceType);
        }

        public override IDictionary<string, string> ConvertRequestParameter(string key, IDictionary<string, string> requestParams, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportConvertRequestParameter(key, requestParams, serviceType);
            }

            return ConvertRequestParameterCore(key, requestParams, serviceType);
        }

        public override string ConvertRequestMapping(string key, string mapping, ServiceType serviceType)
        {
            if(serviceType != ServiceType.SmileVideo) {
                ThrowNotSupportConvertRequestMapping(key, mapping, serviceType);
            }

            return ConvertRequestMappingCore(key, mapping, serviceType);
        }

        public override IReadOnlyCheck CheckResponseHeader(string key, Uri uri, HttpHeaders headers, ServiceType serviceType)
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
            return ConvertStringCore(key, uri, text, serviceType);
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
                    if(!Players.Where(p => !(p.DataContext is SmileVideoLaboratoryPlayerViewModel)).Any() && !(player is SmileVideoLaboratoryPlayerViewModel)) {
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
                var search = request.ShowRequestParameter as SmileVideoSearchParameterModel;
                if(search != null) {
                    ManagerPack.SearchManager.LoadSearchFromParameterAsync(search).ConfigureAwait(false);
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
                    } else {
                        var rankingName = request.ShowRequestParameter as SmileVideoRankingCategoryNameParameterModel;
                        if(rankingName != null) {
                            ManagerPack.RankingManager.LoadRankingCategoryFromParameterAsync(rankingName).ConfigureAwait(false);
                            return ManagerPack.RankingManager;
                        }
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
            var players = Players
                .Select(p => (SmileVideoPlayerViewModel)p.DataContext)
                .Where(vm => !(vm is SmileVideoLaboratoryPlayerViewModel)) // 任意再生は除外
            ;
            var nextPlayer =
                players.FirstOrDefault(p => p.IsWorkingPlayer.Value)
                ??
                players.FirstOrDefault()
            ;

            if(nextPlayer != null) {
                nextPlayer.IsWorkingPlayer.Value = true;
            }

            player.Dispose();
        }

    }
}
