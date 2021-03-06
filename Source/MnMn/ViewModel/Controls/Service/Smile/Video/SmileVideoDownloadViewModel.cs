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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Event;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    /// <summary>
    /// 指定動画ダウンロード。
    /// </summary>
    public class SmileVideoDownloadViewModel : ViewModelBase, IDownloadItem
    {
        #region define

        const long isDownloaded = -1;
        const long initVideoLoadSize = 0;
        const long initVideoTotalSize = 1;

        #endregion

        #region variable

        LoadState _informationLoadState;
        LoadState _thumbnailLoadState;
        LoadState _commentLoadState;
        LoadState _videoLoadState;

        CacheState _cacheState;

        long _videoLoadedSize = initVideoLoadSize;
        long _videoTotalSize = initVideoTotalSize;

        bool _isEconomyMode;

        //bool _isProcessCancel;
        Uri _downloadUri;
        DownloadState _downloadState;

        #endregion

        public SmileVideoDownloadViewModel(Mediator mediator)
        {
            Mediator = mediator;

            var response = Mediator.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)response.Result;

            NetworkSetting = Mediator.GetNetworkSetting();

            Session = Mediator.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
        }

        #region property

        protected Mediator Mediator { get; }
        public SmileSessionViewModel Session { get; }

        protected SmileVideoSettingModel Setting { get; }
        protected IReadOnlyNetworkSetting NetworkSetting { get; }

        public SmileVideoInformationViewModel Information { get; set; }

        public FewViewModel<bool> UsingDmc { get; } = new FewViewModel<bool>();
        public string DmcVideoSrc { get { return DmcLoader?.VideoSource; } }
        public string DmcAudioSrc { get { return DmcLoader?.AudioSource; } }
        protected SmileVideoDmcLoader DmcLoader { get; set; }

        public Uri DownloadUri
        {
            get { return this._downloadUri; }
            private set { SetVariableValue(ref this._downloadUri, value); }
        }

        protected FileInfo VideoFile { get; set; }

        protected Stream VideoStream { get; private set; }

        protected long DownloadStartPosition { get; private set; }

        //protected RawSmileVideoMsgThreadModel CommentThread { get; private set; }

        public string VideoId
        {
            get { return Information?.VideoId; }
        }

        public LoadState InformationLoadState
        {
            get { return this._informationLoadState; }
            set { SetVariableValue(ref this._informationLoadState, value); }
        }

        public LoadState ThumbnailLoadState
        {
            get { return this._thumbnailLoadState; }
            set
            {
                if(SetVariableValue(ref this._thumbnailLoadState, value)) {
                    var propertyNames = new[] {
                        nameof(ThumbnailImage),
                        nameof(Image),
                    };
                    CallOnPropertyChange(propertyNames);
                }
            }
        }

        public LoadState CommentLoadState
        {
            get { return this._commentLoadState; }
            set { SetVariableValue(ref this._commentLoadState, value); }
        }

        public LoadState VideoLoadState
        {
            get { return this._videoLoadState; }
            set { SetVariableValue(ref this._videoLoadState, value); }
        }

        public CacheState CacheState
        {
            get { return this._cacheState; }
            set { SetVariableValue(ref this._cacheState, value); }
        }

        public long VideoLoadedSize
        {
            get { return this._videoLoadedSize; }
            // TowWay用
            set
            {
                if(SetVariableValue(ref this._videoLoadedSize, value)) {
                    CallOnPropertyChange(nameof(DownloadedSize));
                }
            }
        }

        public long VideoTotalSize
        {
            get { return this._videoTotalSize; }
            // TowWay用
            set
            {
                if(SetVariableValue(ref this._videoTotalSize, value)) {
                    CallOnPropertyChange(nameof(DownloadTotalSize));
                }
            }
        }

        public ImageSource ThumbnailImage
        {
            get
            {
                switch(ThumbnailLoadState) {
                    case LoadState.Loaded:
                        return Information.ThumbnailImage;

                    default:
                        return null;
                }
            }
        }

        public bool IsEconomyMode
        {
            get { return this._isEconomyMode; }
            set { SetVariableValue(ref this._isEconomyMode, value); }
        }

        protected CancellationTokenSource DownloadCancel { get; set; }

        protected virtual FileInfo PlayFile
        {
            get
            {
                if(Information.InformationSource != SmileVideoInformationSource.Getthumbinfo) {
                    Mediator.Logger.Error($"{VideoId}: {nameof(Information)}.{nameof(Information.InformationSource)} is {Information.InformationSource}");
                    return null;
                }

                if(Information.MovieType == SmileVideoMovieType.Swf) {
                    if(VideoFile == null) {
                        return null;
                    }

                    var filePath = PathUtility.AppendExtension(VideoFile.FullName, SmileVideoInformationUtility.flashConvertedExtension);
                    var result = new FileInfo(filePath);
                    return result;
                } else {
                    return VideoFile;
                }
            }
        }

        public virtual bool IsPremiumAccount
        {
            get { return Session.IsPremium; }
        }


        #endregion

        #region function

        protected virtual void OnLoadDataWithoutSessionStart()
        { }
        protected virtual void OnLoadDataWithoutSessionEnd()
        { }

        protected virtual Task LoadDataWithoutSessionAsync(CacheSpan imageCacheSpan)
        {
            ThumbnailLoadState = LoadState.Loading;
            OnLoadDataWithoutSessionStart();
            return Information.LoadThumbnaiImageDefaultAsync(imageCacheSpan).ContinueWith(task => {
                ThumbnailLoadState = LoadState.Loaded;
                OnLoadDataWithoutSessionEnd();
            });
        }

        protected virtual void OnLoadDataWithSessionStart()
        { }
        protected virtual void OnLoadDataWithSessionEnd()
        { }

        protected virtual void OnLoadGetflvStart()
        { }
        protected virtual void OnLoadGetflvEnd()
        { }

        protected virtual void OnLoadWatchDataStart()
        { }
        protected virtual void OnLoadWatchDataEnd()
        { }

        async Task LoadGetflvAsync()
        {
            OnLoadGetflvStart();

            var result = await Information.LoadGetflvAsync(true, Setting.Download.UsingDmc);

            OnLoadGetflvEnd();
        }

        async Task LoadWatchDataAsync()
        {
            OnLoadWatchDataStart();

            var result = await Information.LoadWatchDataAsync(true, Setting.Download.UsingDmc);

            OnLoadWatchDataEnd();
        }

        protected virtual void OnLoadVideoStart()
        { }
        protected virtual void OnLoadVideoEnd()
        { }

        protected async Task LoadVideoAsync(Uri downloadUri, FileInfo downloadFile, long headPosition)
        {
            OnLoadVideoStart();

            VideoLoadState = LoadState.Preparation;
            VideoFile = downloadFile;
            DownloadUri = downloadUri;

            DownloadCancel = new CancellationTokenSource();
            using(var downloader = new SmileVideoDownloader(downloadUri, Session, Information.WatchUrl, DownloadCancel.Token) {
                ReceiveBufferSize = Constants.ServiceSmileVideoReceiveBuffer,
                DownloadTotalSize = VideoTotalSize,
                RangeHeadPotision = headPosition,
            }) {
                Mediator.Logger.Information($"{VideoId}: download start: uri: {downloadUri}, head: {headPosition}, size: {VideoTotalSize}");

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var fileMode = downloader.UsingRangeDownload ? FileMode.Append : FileMode.Create;
                using(VideoStream = new FileStream(VideoFile.FullName, fileMode, FileAccess.Write, FileShare.Read)) {
                    try {
                        downloader.DownloadStart += Downloader_DownloadStart;
                        downloader.DownloadingError += Downloader_DownloadingError;
                        downloader.Downloading += Downloader_Downloading;
                        downloader.Downloaded += Downloader_Downloaded;

                        await downloader.StartAsync();
                        if(downloader.Completed) {
                            if(UsingDmc.Value) {
                                //var mux = DmcMultiplexer;
                                var role = SmileVideoInformationUtility.GetDmcRoleKey(DmcLoader.VideoSource, DmcLoader.AudioSource);
                                VideoStream.Flush();
                                VideoFile.Refresh();
                                // 理屈で言えばここは絶対に存在する
                                // #715 で消極的に読み込み済み印を設定
                                Information.SetDmcLoaded(DmcLoader.VideoSource, DmcLoader.AudioSource, Information.DmcItems[role].Length <= VideoFile.Length);
                                if(Information.DmcItems[role].IsLoaded) {
                                    VideoLoadState = LoadState.Loaded;
                                    DownloadState = DownloadState.Completed;
                                } else {
                                    Mediator.Logger.Debug($"{VideoId}: LoadState.Failure, {DmcLoader.VideoSource}, {DmcLoader.AudioSource}, {Information.DmcItems[role].Length <= VideoFile.Length}, {role}");
                                    VideoLoadState = LoadState.Failure;
                                    DownloadState = DownloadState.Failure;
                                }
                            } else {
                                VideoLoadState = LoadState.Loaded;
                                DownloadState = DownloadState.Completed;
                                if(Information.IsEconomyMode) {
                                    Information.LoadedEconomyVideo = true;
                                } else {
                                    Information.LoadedNormalVideo = true;
                                }
                            }
                            Information.SaveSetting(false);
                        } else {
                            DownloadState = DownloadState.Failure;
                            VideoLoadState = LoadState.Failure;
                            Mediator.Logger.Debug($"{VideoId}: LoadState.Failure");
                        }
                        if(!downloader.Canceled) {
                            OnLoadVideoEnd();
                        }
                    } catch(Exception ex) {
                        Mediator.Logger.Error(ex);
                        DownloadState = DownloadState.Failure;
                        VideoLoadState = LoadState.Failure;
                        Mediator.Logger.Debug($"{VideoId}: LoadState.Failure");
                    } finally {
                        downloader.DownloadStart -= Downloader_DownloadStart;
                        downloader.DownloadingError -= Downloader_DownloadingError;
                        downloader.Downloading -= Downloader_Downloading;
                        downloader.Downloaded -= Downloader_Downloaded;

                        stopWatch.Stop();
                        Mediator.Logger.Information($"{VideoId}: download end: {stopWatch.Elapsed}");
                        // 保険
                        if(Information.IsDownloading) {
                            Information.IsDownloading = false;
                        }
                    }
                }
            }
        }

        protected Tuple<string, RawSmileVideoDmcObjectModel> GetDmcObject_Issue665NA()
        {
            var info = Information.DmcInfo_Issue665NA;

            var model = new RawSmileVideoDmcObjectModel();
            {
                var with = model.Data.Session;
                var session = info.SelectToken("session_api");

                //var uri = session["api_urls"].First().Value<string>();
                var uri = session["urls"].First()["url"].Value<string>();

                with.RecipeId = session["recipe_id"].Value<string>();
                with.ContentId = session["content_id"].Value<string>();
                with.Protocol.Name = session["protocols"].First().Value<string>();
                with.Priority = session["priority"].Value<string>();

                var mux = new RawSmileVideoDmcSrcIdToMultiplexerModel();
                mux.SrcId.VideoSrcIds.InitializeRange(session["videos"].Values<string>());
                mux.SrcId.AudioSrcIds.InitializeRange(session["audios"].Values<string>());
                var idSet = new RawSmileVideoDmcContentSrcIdSetModel();
                idSet.SrcIdToMultiplexers.Add(mux.SrcId);
                with.ContentSrcIdSets.Add(idSet);

                with.OperationAuth.BySignature.Token = session["token"].Value<string>();
                with.OperationAuth.BySignature.Signature = session["signature"].Value<string>();
                with.KeepMethod.HeartBeat.LifeTime = session["heartbeat_lifetime"].Value<string>();
                IDictionary<string, JToken> authTypes = (JObject)session["auth_types"];
                JToken authType;
                if(authTypes.TryGetValue(with.Protocol.Name, out authType)) {
                    with.ContentAuth.AuthType = authType.Value<string>();
                } else {
                    with.ContentAuth.AuthType = authType.Value<string>();
                }
                //with.ContentAuth.ServiceId = session["service_id"].Value<string>();
                with.ContentAuth.ServiceUserId = session["service_user_id"].Value<string>();
                with.ContentAuth.ContentKeyTimeout = session["content_key_timeout"].Value<string>();
                with.ClientInformation.PlayerId = session["player_id"].Value<string>();

                return Tuple.Create(uri, model);
            }
        }

        protected Tuple<string, RawSmileVideoDmcObjectModel> GetDmcObject2()
        {
            var model = new RawSmileVideoDmcObjectModel();
            {
                var with = model.Data.Session;
                var session = Information.DmcInfo2.SessionApi;

                var uri = session.Urls.First().Url;

                with.RecipeId = session.RecipeId;
                with.ContentId = session.ContentId;
                with.Protocol.Name = session.Protocols.First();
                with.Priority = session.Priority;

                var mux = new RawSmileVideoDmcSrcIdToMultiplexerModel();
                mux.SrcId.VideoSrcIds.InitializeRange(session.Videos);
                mux.SrcId.AudioSrcIds.InitializeRange(session.Audios);
                var idSet = new RawSmileVideoDmcContentSrcIdSetModel();
                idSet.SrcIdToMultiplexers.Add(mux.SrcId);
                with.ContentSrcIdSets.Add(idSet);

                with.OperationAuth.BySignature.Token = session.Token;
                with.OperationAuth.BySignature.Signature = session.Signature;
                with.KeepMethod.HeartBeat.LifeTime = session.HeartbeatLifetime;
                string authType;
                if(session.AuthTypes.TryGetValue(with.Protocol.Name, out authType)) {
                    with.ContentAuth.AuthType = authType;
                } else {
                    with.ContentAuth.AuthType = authType;
                }
                with.ContentAuth.ServiceUserId = session.ServiceUserId;
                with.ContentAuth.ContentKeyTimeout = session.ContentKeyTimeout;
                with.ClientInformation.PlayerId = session.PlayerId;

                return Tuple.Create(uri, model);
            }
        }

        /// <summary>
        /// DMC形式でダウンロードする。
        /// </summary>
        /// <returns>真: DMC形式で処理できた(ダウンロードが成功したかどうかではない)、偽: 新形式では処理できない。</returns>
        protected async Task<bool> LoadDmcVideoAsync()
        {
            if(!Information.IsDmc) {
                return false;
            }

            VideoLoadState = LoadState.Preparation;

            var tuple = Information.IsCompatibleIssue665NA
                ? GetDmcObject_Issue665NA()
                : GetDmcObject2()
            ;
            //var dmc = new Dmc(Mediation);
            //var tuple = GetDmcObject2();
            //DmcApiUri = new Uri(tuple.Item1);
            var model = tuple.Item2;

            // 動画ソースを選りすぐる
            if(Information.IsCompatibleIssue665NA) {
                var sendMux = model.Data.Session.ContentSrcIdSets.First().SrcIdToMultiplexers.First();
                var sendVideoWeights = SmileVideoDmcObjectUtility.GetVideoWeights(sendMux.VideoSrcIds, Setting.Download.VideoWeight);
                sendMux.VideoSrcIds.InitializeRange(sendVideoWeights.ToEvaluatedSequence());
                var sendAudioWeights = SmileVideoDmcObjectUtility.GetAudioWeights(sendMux.AudioSrcIds, Setting.Download.AudioWeight);
                sendMux.AudioSrcIds.InitializeRange(sendAudioWeights.ToEvaluatedSequence());
            } else {
                var dmcInfo2 = Information.DmcInfo2;

                var sendMux = model.Data.Session.ContentSrcIdSets.First().SrcIdToMultiplexers.First();
                var sendVideoWeights = SmileVideoDmcObjectUtility.GetVideoWeights(dmcInfo2.SessionApi.Videos, Setting.Download.VideoWeight);
                sendMux.VideoSrcIds.InitializeRange(sendVideoWeights.ToEvaluatedSequence());
                var sendAudioWeights = SmileVideoDmcObjectUtility.GetAudioWeights(dmcInfo2.SessionApi.Audios, Setting.Download.AudioWeight);
                sendMux.AudioSrcIds.InitializeRange(sendAudioWeights.ToEvaluatedSequence());
            }

            DmcLoader = new SmileVideoDmcLoader(VideoId, new Uri(tuple.Item1), Mediator);
            var downloadUri = await DmcLoader.StartAsync(model, Information.DmcFile);

            if(downloadUri == null) {
                return false;
            }
            if(DmcLoader == null) {
                return false;
            }

            var ext = DmcLoader.FileExtension;
            var video = DmcLoader.VideoSource;
            var audio = DmcLoader.AudioSource;
            var downloadPath = Information.GetDmcVideoFileName(video, audio, ext);
            var downloadFile = new FileInfo(downloadPath);
            downloadFile.Refresh();
            var headPosition = 0L;
            if(downloadFile.Exists) {
                headPosition = downloadFile.Length;
            }

            UsingDmc.Value = true;

            // ソース品質でキャッシュ判定
            if(Information.DmcItems.Any()) {
                var cachedItems = Information.DmcItems
                    .Where(i => i.Value.IsLoaded)
                    .Select(i => i.Value)
                ;
                if(cachedItems.Any()) {
                    Mediator.Logger.Information("cache check");
                    var highQualityItems = SmileVideoDmcObjectUtility.GetHighQualityItems(cachedItems);

                    var topQualityItem = highQualityItems.First();
                    var cmpVideo = SmileVideoDmcObjectUtility.CompareVideo(video, topQualityItem.Video);
                    if(0 <= cmpVideo) {
                        // キャッシュ動画ソースは提供ソースより高品質
                        Mediator.Logger.Information("service video <= cache video");

                        var cmpAudio = SmileVideoDmcObjectUtility.CompareAudio(audio, topQualityItem.Audio);
                        if(0 <= cmpAudio) {
                            // キャッシュ音声ソースは提供ソースより高品質
                            Mediator.Logger.Information("service audio <= cache audio");

                            var cacheRole = SmileVideoInformationUtility.GetDmcRoleKey(topQualityItem.Video, topQualityItem.Audio);

                            var cachePath = Information.GetDmcVideoFileName(topQualityItem.Video, topQualityItem.Audio, ext);
                            var cacheFile = new FileInfo(cachePath);
                            cacheFile.Refresh();
                            if(cacheFile.Exists && cacheFile.Length <= topQualityItem.Length) {
                                Mediator.Logger.Information("enabled cache: high quality");
                                LoadVideoFromCache(cacheFile);
                                var task = DmcLoader.StopAsync();
                                return true;
                            }
                        }
                    }
                }
            }

            var role = SmileVideoInformationUtility.GetDmcRoleKey(video, audio);
            if(Information.DmcItems.TryGetValue(role, out var dmcItem)) {
                if(dmcItem.IsLoaded && downloadFile.Exists) {
                    if(dmcItem.Length <= downloadFile.Length) {
                        Mediator.Logger.Information("simple cache: high quality");
                        LoadVideoFromCache(downloadFile);
                        var task = DmcLoader.StopAsync();
                        return true;
                    }
                }
            }


            await LoadVideoAsync(downloadUri, downloadFile, headPosition);
            return true;
        }

        protected virtual void OnLoadMsgStart()
        { }
        protected virtual void OnLoadMsgEnd()
        { }

        protected async Task<RawSmileVideoMsgPacket_Issue665NA_Model> LoadMsg_Issue665NA_CoreAsync(int getCount, int rangeHeadMinutes, int rangeTailMinutes, int rangeGetCount, int rangeGetAllCount)
        {
            await Information.LoadGetthreadkeyAsync();

            var msg = new Msg(Mediator);
            return await msg.Load_Issue665NA_Async(
                Information.MessageServerUrl,
                Information.ThreadId,
                Information.UserId,
                getCount,
                rangeHeadMinutes, rangeTailMinutes, rangeGetCount,
                rangeGetAllCount,
                Information.Getthreadkey
            );
        }

        protected async Task<SmileVideoMsgPackSettingModel> LoadMsgCoreAsync(int getCount, SmileVideoMsgRangeModel range)
        {
            await Information.LoadGetthreadkeyAsync();

            var msg = new Msg(Mediator);
            return await msg.LoadAsync(
                Information.MessageServerUrl,
                Information.ThreadId,
                getCount,
                range,
                Session.UserId,
                Information.UserKey,
                Information.HasOriginalPostedComment,
                Information.IsChannelVideo,
                Information.CommunityThreadId,
                Information.Getthreadkey
            );
        }

        IDictionary<SmileVideoMsgPacketId, IList<RawSmileVideoMsgResultItemModel>> GetMsgMap(SmileVideoMsgPackSettingModel msg)
        {
            var result = new Dictionary<SmileVideoMsgPacketId, IList<RawSmileVideoMsgResultItemModel>>();

            var members = EnumUtility.GetMembers<SmileVideoMsgPacketId>();
            var items = msg.Items.ToEvaluatedSequence();
            foreach(var member in members) {
                if(msg.PacketId.TryGetValue(member, out var index)) {
                    var head = $"ps:{index}";
                    var tail = $"pf:{index}";

                    var headSequence = items.SkipWhile(i => i.Ping == null || (i.Ping != null && i.Ping.Content != head)).Skip(1);
                    var sequence = headSequence.TakeWhile(i => i.Ping == null || (i.Ping != null && i.Ping.Content != tail));

                    var list = sequence.Where(i => i.Chat != null).ToEvaluatedSequence();
                    result.Add(member, list);
                } else {
                    // null なんか返そうもんなら死ぬ
                    result.Add(member, new List<RawSmileVideoMsgResultItemModel>());
                }
            }

            return result;
        }

        protected SmileVideoMsgPackSettingModel MergeMsg(SmileVideoMsgPackSettingModel newMsg, SmileVideoMsgPackSettingModel oldMsg)
        {
            var newMap = GetMsgMap(newMsg);
            var oldMap = GetMsgMap(oldMsg);

            var msg = new SmileVideoMsgPackSettingModel();
            var members = EnumUtility.GetMembers<SmileVideoMsgPacketId>();
            foreach(var item in members.SelectValueIndex()) {
                var newItems = newMap[item.Value];
                var oldItems = oldMap[item.Value];

                var mergeItems = newItems
                    .Concat(oldItems)
                    .Where(i => i.Chat != null)
                    .GroupBy(i => i.Chat.No)
                    .Select(g => g.First())
                ;

                msg.PacketId[item.Value] = item.Index;
                msg.Items.Add(new RawSmileVideoMsgResultItemModel() {
                    Ping = new RawSmileVideoMsgPingModel() {
                        Content = $"ps:{item.Index}"
                    }
                });
                msg.Items.AddRange(mergeItems);
                msg.Items.Add(new RawSmileVideoMsgResultItemModel() {
                    Ping = new RawSmileVideoMsgPingModel() {
                        Content = $"pf:{item.Index}"
                    }
                });
            }

            return msg;
        }

        protected async Task<RawSmileVideoMsgPacket_Issue665NA_Model> LoadMsg_Issue665NA_Async(CacheSpan msgCacheSpan)
        {
            OnLoadMsgStart();

            CommentLoadState = LoadState.Preparation;

            //TODO; キャッシュチェック時のファイル処理関係は共通化可能
            var cacheFilePath = Information.MsgFile_Issue665NA.FullName;
            if(File.Exists(cacheFilePath)) {
                var fileInfo = new FileInfo(cacheFilePath);
                if(msgCacheSpan.IsCacheTime(fileInfo.LastWriteTime) && Constants.MinimumXmlFileSize <= fileInfo.Length) {
                    CommentLoadState = LoadState.Loading;
                    using(var stream = new FileStream(cacheFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        var result = Msg.ConvertFromRawPacketData_Issue665NA(stream);
                        OnLoadMsgEnd();
                        return result;
                    }
                }
            }

            CommentLoadState = LoadState.Loading;
            var rawMessagePacket = await LoadMsg_Issue665NA_CoreAsync(1000, 0, (int)Information.Length.TotalMinutes, 100, 1000);

            // キャッシュ構築
            if(rawMessagePacket.Chat.Any()) {
                try {
                    SerializeUtility.SaveXmlSerializeToFile(cacheFilePath, rawMessagePacket);
                } catch(FileNotFoundException) {
                    // BUGS: いかんのう
                }
            }

            OnLoadMsgEnd();
            return rawMessagePacket;
        }

        protected async Task<SmileVideoMsgPackSettingModel> LoadMsgAsync(CacheSpan msgCacheSpan)
        {
            OnLoadMsgStart();

            CommentLoadState = LoadState.Preparation;

            //TODO; キャッシュチェック時のファイル処理関係は共通化可能
            var cacheFilePath = Information.MsgFile.FullName;
            // #665 除外
            var fileInfo = new FileInfo(cacheFilePath);
            fileInfo.Refresh();
            if(fileInfo.Exists) {
                if(msgCacheSpan.IsCacheTime(fileInfo.LastWriteTime) && Constants.MinimumXmlFileSize <= fileInfo.Length) {
                    CommentLoadState = LoadState.Loading;
                    var result = Msg.ConvertMsgSettingModel(fileInfo);
                    OnLoadMsgEnd();
                    return result;
                }
            }

            CommentLoadState = LoadState.Loading;
            var rawMessagePacket = await LoadMsgCoreAsync(1000, new SmileVideoMsgRangeModel(0, (int)Information.Length.TotalMinutes, 100, 1000));
            // #665 除外
            //ImportCommentThread(rawMessagePacket);
            if(Setting.Download.StackComments) {
                Mediator.Logger.Debug(nameof(Setting.Download.StackComments));

                if(fileInfo.Exists && Constants.MinimumJsonFileSize <= fileInfo.Length) {
                    Mediator.Logger.Debug("start merge");

                    var cacheComment = Msg.ConvertMsgSettingModel(fileInfo);
                    rawMessagePacket = MergeMsg(rawMessagePacket, cacheComment);

                    Mediator.Logger.Debug("end merge");
                }
            }

            // キャッシュ構築
            if(rawMessagePacket.Items.Any(i => i.Chat != null)) {
                try {
                    // #665
                    //SerializeUtility.SaveXmlSerializeToFile(cacheFilePath, rawMessagePacket);
                    SerializeUtility.SaveJsonDataToFile(cacheFilePath, rawMessagePacket);
                } catch(FileNotFoundException ex) {
                    // BUGS: いかんのう
                    Mediator.Logger.Error(ex);
                }
            }

            OnLoadMsgEnd();
            return rawMessagePacket;
        }

        protected virtual Task LoadComment_Issue665NA_Async(RawSmileVideoMsgPacket_Issue665NA_Model rawMsgPacket)
        {
            CommentLoadState = LoadState.Loaded;
            return Task.CompletedTask;
        }

        protected virtual Task LoadCommentAsync(SmileVideoMsgPackSettingModel rawMsgPacket)
        {
            CommentLoadState = LoadState.Loaded;
            return Task.CompletedTask;
        }

        protected static long GetDownloadHeadPosition(FileInfo fileInfo, long completeSize)
        {
            if(fileInfo.Exists) {
                if(completeSize <= fileInfo.Length) {
                    return isDownloaded;
                } else {
                    // 途中からダウンロード
                    return fileInfo.Length;
                }
            }

            return 0;
        }

        protected void LoadVideoFromCache(FileInfo videoFile)
        {
            VideoFile = videoFile;
            VideoLoadedSize = VideoTotalSize = VideoFile.Length;
            DownloadState = DownloadState.Completed;
            VideoLoadState = LoadState.Loaded;

            //// TODO: HTMLがあること前提
            if(Information.PageHtmlLoadState != LoadState.Loaded && string.IsNullOrEmpty(Information.DescriptionHtmlSource)) {
                Information.LoadLocalPageHtml();
            }

            OnLoadVideoEnd();
        }

        protected virtual async Task LoadDataWithSessionAsync()
        {
            OnLoadDataWithSessionStart();

            var tcs = new CancellationTokenSource();

            RETRY_Issue665NA:
            if(Information.IsCompatibleIssue665NA) {
                await LoadGetflvAsync();
                if(Information.InformationLoadState == LoadState.Failure || Information.HasGetflvError) {
                    InformationLoadState = LoadState.Failure;
                    return;
                }
            } else {
                await LoadWatchDataAsync();
                var force_Issue665NA_forceFlashPage = false;
                try {
                    if(Information.InformationLoadState == LoadState.Failure || Information.HasWatchDataError) {
                        InformationLoadState = LoadState.Failure;
                        return;
                    }

                    if(Information.DmcInfo2 != null) {
                        var result = SmileVideoDmcObjectUtility.IsSuccess(Information.DmcInfo2);
                        if(!result) {
                            // DMC配信でもなくDMC実装後なのにHTML5版に対応してないわけわからん動画用
                            force_Issue665NA_forceFlashPage = true;
                            throw new InvalidOperationException(Constants.ServiceSmileVideoGetVideoError);
                        }
                    }

                } catch(InvalidOperationException ex) {
                    // TODO: when(ex.Message == Constants.ServiceSmileVideoGetVideoError) への置き換え
                    if(ex.Message == Constants.ServiceSmileVideoGetVideoError) {
                        // #665 対応がどうにもダメな場合はなけなしの力で#665非対応処理を強制実施
                        Information.Force_Issue665NA(force_Issue665NA_forceFlashPage);
                        goto RETRY_Issue665NA;
                    }
                }
            }

            var commentTask = Information.IsCompatibleIssue665NA
                ? LoadMsg_Issue665NA_Async(Constants.ServiceSmileVideoMsgCacheSpan).ContinueWith(task => LoadComment_Issue665NA_Async(task.Result), TaskScheduler.FromCurrentSynchronizationContext())
                : LoadMsgAsync(Constants.ServiceSmileVideoMsgCacheSpan).ContinueWith(task => LoadCommentAsync(task.Result), TaskScheduler.FromCurrentSynchronizationContext())
            ;

            // キャッシュとかエコノミー確認であれこれ分岐
            Debug.Assert(Information != null);

            if(Information.MovieType == SmileVideoMovieType.Swf && Information.ConvertedSwf) {
                var f = new FileInfo(Path.Combine(Information.CacheDirectory.FullName, Information.GetVideoFileName(false)));
                LoadVideoFromCache(f);
                return;
            }

            if(Setting.Download.UsingDmc) {
                if(await LoadDmcVideoAsync()) {
                    // DMC形式で何かしら処理できた
                    return;
                }
            }

            var normalVideoFile = new FileInfo(Path.Combine(Information.CacheDirectory.FullName, Information.GetVideoFileName(false)));
            var normalHeadPosition = GetDownloadHeadPosition(normalVideoFile, Information.SizeHigh);
            if(normalHeadPosition == isDownloaded) {
                // ダウンロード済み
                IsEconomyMode = false;
                LoadVideoFromCache(normalVideoFile);
                return;
            }

            long headPosition = 0;
            FileInfo downloadFile;
            // エコノミーキャッシュが存在しても非エコノミーでダウンロードできるなら新たにファイルを落とす。
            IsEconomyMode = Information.IsEconomyMode;
            if(IsEconomyMode) {
                var economyVideoFile = new FileInfo(Path.Combine(Information.CacheDirectory.FullName, Information.GetVideoFileName(true)));
                var economyHeadPosition = GetDownloadHeadPosition(economyVideoFile, Information.SizeLow);
                if(economyHeadPosition == isDownloaded) {
                    // エコノミーダウンロード済み
                    IsEconomyMode = true;
                    LoadVideoFromCache(economyVideoFile);
                    return;
                }
                headPosition = economyHeadPosition;
                downloadFile = economyVideoFile;
            } else {
                headPosition = normalHeadPosition;
                downloadFile = normalVideoFile;
            }

            VideoTotalSize = Information.VideoSize;

            await LoadVideoAsync(Information.MovieServerUrl, downloadFile, headPosition);
        }

        protected virtual void OnLoadStart()
        { }

        protected virtual void OnLoadEnd()
        { }

        protected virtual void OnLoadGetthumbinfoStart() { }
        protected virtual void OnLoadGetthumbinfoEnd() { }

        [Obsolete]
        public async Task LoadAsync(string videoId, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            InformationLoadState = LoadState.Loading;

            OnLoadGetthumbinfoStart();

            var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));
            Information = await Mediator.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);

            OnLoadGetthumbinfoEnd();

            await LoadAsync(Information, false, thumbCacheSpan, imageCacheSpan);
        }

        protected virtual void InitializeStatus()
        {
            // ダウンロード終了と強制で判断。ここじゃダメなんだろうなぁ、やだなぁ
            if(Information != null) {
                Information.IsDownloading = false;
            }

            VideoLoadedSize = initVideoLoadSize;
            VideoTotalSize = initVideoTotalSize;
            DownloadState = DownloadState.None;
            VideoLoadState = LoadState.None;
            InformationLoadState = LoadState.None;
            ThumbnailLoadState = LoadState.None;
            CommentLoadState = LoadState.None;
            UsingDmc.Value = false;
            //DmcObject = null;
            //DmcApiUri = null;
            //DmcPollingTask = null;
            //DmcPollingWait = null;
            //DmcPollingCancel = null;
            if(DmcLoader != null) {
                DmcLoader.Dispose();
            }
            DmcLoader = null;
            DownloadUri = null;
            DownloadCancel = null;
        }

        protected virtual Task StopPrevProcessAsync()
        {
            if(DownloadCancel != null) {
                Mediator.Logger.Trace($"{VideoId}: download cancel!");
                DownloadCancel.Cancel();
                DownloadCancel.Dispose();
                DownloadCancel = null;
            }

            if(UsingDmc.Value) {
                return StopDmcDownloadAsync().ContinueWith(t => {
                    InitializeStatus();
                });
            }

            InitializeStatus();

            return Task.CompletedTask;
        }


        public virtual async Task LoadAsync(SmileVideoInformationViewModel videoInformation, bool forceEconomy, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            await StopPrevProcessAsync();

            OnLoadStart();

            DownloadState = DownloadState.Preparation;
            InformationLoadState = LoadState.Loading;

            OnLoadGetthumbinfoStart();
            Information = videoInformation;
            if(Information.InformationSource != SmileVideoInformationSource.Getthumbinfo) {
                await Information.LoadInformationDefaultAsync(thumbCacheSpan);
            }
            InformationLoadState = LoadState.Loaded;
            OnLoadGetthumbinfoEnd();

            var noSessionTask = LoadDataWithoutSessionAsync(imageCacheSpan);
            var sessionTask = LoadDataWithSessionAsync();

            await noSessionTask;
            await sessionTask;

            OnLoadEnd();
        }

        protected virtual void OnDownloadStart(object sender, DownloadStartEventArgs e)
        { }

        protected virtual void OnDownloading(object sender, DownloadingEventArgs e)
        { }

        protected virtual void OnDownloaded(object sender, DownloaderEventArgs e)
        { }

        protected virtual void OnDownloadingError(object sender, DownloadingErrorEventArgs e)
        { }

        protected Task StopDmcDownloadAsync()
        {
            Debug.Assert(UsingDmc.Value);
            if(Information.IsDownloading) {
                return DmcLoader.StopAsync();
            }

            return Task.CompletedTask;
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IDownloadItem

        Uri IDownloadItem.DownloadUri => DownloadUri;

        public string DownloadTitle => $"[{Information.VideoId}] {Information.Title}";

        public DownloadState DownloadState
        {
            get { return this._downloadState; }
            internal set { SetVariableValue(ref this._downloadState, value); }
        }

        public DownloadUnit DownloadUnit { get; } = DownloadUnit.Size;

        public bool EnabledTotalSize => true;

        public long DownloadTotalSize => VideoTotalSize;

        public long DownloadedSize => VideoLoadedSize;

        public IProgress<double> DownloadingProgress { get; set; }

        public ImageSource Image => Information.ThumbnailImage;

        public bool CanRestart => true;

        public virtual object DownloadUniqueItem => Information;

        public ICommand OpenDirectoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(Information.CacheDirectory.Exists) {
                            ShellUtility.OpenDirectory(Information.CacheDirectory, Mediator.Logger);
                        }
                    },
                    o => Information?.CacheDirectory.Exists ?? false
                );
            }
        }

        public ICommand ExecuteTargetCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var forceEconomy = false;
                        Information.OpenVideoDefaultAsync(forceEconomy);
                    },
                    o => Information != null && SmileVideoInformationUtility.CheckCanPlay(Information, Mediator.Logger) && DownloadState != DownloadState.Preparation
                );
            }
        }

        public ICommand AutoExecuteTargetCommand => CreateCommand(o => { }, o => false);

        public Task StartAsync()
        {
            DownloadState = DownloadState.Preparation;

            if(Information == null) {
                DownloadState = DownloadState.Failure;
                throw new InvalidOperationException($"{nameof(Information)} is null");
            }
            if(!SmileVideoInformationUtility.CheckCanPlay(Information, Mediator.Logger)) {
                // 別んとこで使われてる
                DownloadState = DownloadState.Failure;
                Mediator.Logger.Warning($"{Information.VideoId}: can not download");
                return Task.CompletedTask;
            }

            var forceEconomy = false;
            return LoadAsync(Information, forceEconomy, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ContinueWith(t => {
                if(t.IsFaulted) {
                    Mediator.Logger.Warning($"{Information.VideoId}: {t.Exception.Message}", t.Exception);
                    DownloadState = DownloadState.Failure;
                }
            });
        }


        public void Cancel()
        {
            StopPrevProcessAsync().ContinueWith(_ => {
                DownloadState = DownloadState.Failure;
            });
        }

        #endregion

        private void Downloader_DownloadStart(object sender, DownloadStartEventArgs e)
        {
            DownloadState = DownloadState.Downloading;
            VideoLoadState = LoadState.Loading;

            if(e.DownloadStartType == DownloadStartType.Range) {
                DownloadStartPosition = e.RangeHeadPosition;
            }
            var downloader = (SmileVideoDownloader)sender;
            //Information.SetPageHtmlAsync(downloader.PageHtml, true).Wait();

            if(UsingDmc.Value) {
                if(downloader.ResponseHeaders.ContentLength.HasValue) {
                    Information.IsDownloading = true;

                    VideoFile.Refresh();

                    VideoTotalSize = VideoFile.Length + downloader.ResponseHeaders.ContentLength.Value;

                    Mediator.Logger.Information($"file:{VideoFile.Length}, response:{downloader.ResponseHeaders.ContentLength.Value}, total:{VideoTotalSize}");

                    var video = DmcLoader.VideoSource;
                    var audio = DmcLoader.AudioSource;
                    var role = SmileVideoInformationUtility.GetDmcRoleKey(video, audio);
                    if(e.DownloadStartType == DownloadStartType.Begin) {
                        var dmcItem = new SmileVideoDmcItemModel() {
                            Video = video,
                            Audio = audio,
                            Length = downloader.ResponseHeaders.ContentLength.Value,
                        };
                        Information.DmcItems[role] = dmcItem;
                    } else {
                        Debug.Assert(e.DownloadStartType == DownloadStartType.Range);
                        if(!Information.DmcItems.ContainsKey(role)) {
                            // キャッシュは存在するのにデータがないのはこれいかに
                            var dmcItem = new SmileVideoDmcItemModel() {
                                Video = video,
                                Audio = audio,
                                Length = VideoFile.Length + downloader.ResponseHeaders.ContentLength.Value,
                            };
                            Information.DmcItems[role] = dmcItem;
                        }
                    }
                }

                DmcLoader.StartPolling();
            }

            OnDownloadStart(sender, e);
        }

        private void Downloader_Downloading(object sender, DownloadingEventArgs e)
        {
            var downloader = (Downloader)sender;
            var buffer = e.Data;
            VideoStream.Write(buffer.Array, 0, e.Data.Count);
            VideoLoadedSize = DownloadStartPosition + downloader.DownloadedSize;

            if(e.Cancel || downloader.Canceled) {
                return;
            }

            DownloadingProgress?.Report(VideoLoadedSize / (double)VideoTotalSize);

            Information.IsDownloading = true;

            OnDownloading(sender, e);
            if(!e.Cancel && DownloadCancel != null) {
                e.Cancel = DownloadCancel.IsCancellationRequested;
            }
        }

        private void Downloader_Downloaded(object sender, DownloaderEventArgs e)
        {
            if(UsingDmc.Value) {
                StopDmcDownloadAsync();
            }
            Information.IsDownloading = false;

            OnDownloaded(sender, e);
        }

        private void Downloader_DownloadingError(object sender, DownloadingErrorEventArgs e)
        {
            int retry = Constants.ServiceSmileVideoDownloadingErrorRetryCount;

            e.Cancel = retry < e.Counter;
            Mediator.Logger.Error(e.Exception);

            if(e.Cancel) {
                if(VideoLoadState != LoadState.Loaded) {
                    // スレッド間のあれな動作であれになる抑制
                    DownloadState = DownloadState.Failure;
                    VideoLoadState = LoadState.Failure;
                    Mediator.Logger.Debug($"{VideoId}: LoadState.Failure");
                }
                if(UsingDmc.Value) {
                    StopDmcDownloadAsync();
                }
                Information.IsDownloading = false;
            } else {
                var time = Constants.ServiceSmileVideoDownloadingErrorWaitTime;
                Thread.Sleep(time);
            }

            OnDownloadingError(sender, e);
        }
    }
}
