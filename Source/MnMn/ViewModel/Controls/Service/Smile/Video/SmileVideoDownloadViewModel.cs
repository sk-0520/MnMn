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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Event;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
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
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    /// <summary>
    /// 指定動画ダウンロード。
    /// </summary>
    public class SmileVideoDownloadViewModel: ViewModelBase
    {
        #region define

        const long isDonwloaded = -1;
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

        #endregion

        public SmileVideoDownloadViewModel(Mediation mediation)
        {
            Mediation = mediation;

            var response = Mediation.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)response.Result;

            Session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
        }

        #region property

        protected Mediation Mediation { get; }
        public SmileSessionViewModel Session { get; }

        protected SmileVideoSettingModel Setting { get; }

        public SmileVideoInformationViewModel Information { get; set; }

        public FewViewModel<bool> UsingDmc { get; } = new FewViewModel<bool>();
        protected RawSmileVideoDmcObjectModel DmcObject { get; private set; }
        protected Uri DmcApiUri { get; private set; }
        protected RawSmileVideoDmcSrcIdToMultiplexerModel DmcMultiplexer { get { return DmcObject?.Data.Session.ContentSrcIdSets.First().SrcIdToMultiplexers.First(); } }
        public string DmcVideoSrc { get { return DmcMultiplexer?.VideoSrcIds.First(); } }
        public string DmcAudioSrc { get { return DmcMultiplexer?.AudioSrcIds.First(); } }
        protected string DmcFileExtension { get { return DmcObject.Data.Session.Protocol.HttpParameters.First().Parameters.First().FileExtension; } }
        Task DmcPollingTask { get; set; }
        AutoResetEvent DmcPollingWait { get; set; }
        CancellationTokenSource DmcPollingCancel { get; set; }

        protected Uri DownloadUri { get; private set; }

        protected FileInfo VideoFile { get; set; }

        protected Stream VideoStream { get; private set; }

        protected long DonwloadStartPosition { get; private set; }

        protected RawSmileVideoMsgThreadModel CommentThread { get; private set; }

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
                    CallOnPropertyChange(nameof(ThumbnailImage));
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
            set { SetVariableValue(ref this._videoLoadedSize, value); }
        }

        public long VideoTotalSize
        {
            get { return this._videoTotalSize; }
            // TowWay用
            set { SetVariableValue(ref this._videoTotalSize, value); }
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

        //public bool IsProcessCancel
        //{
        //    get { return this._isProcessCancel; }
        //    set { SetVariableValue(ref this._isProcessCancel, value); }
        //}

        protected CancellationTokenSource DownloadCancel { get; set; }

        protected FileInfo PlayFile
        {
            get
            {
                if(Information.MovieType == SmileVideoMovieType.Swf) {
                    var filePath = PathUtility.AppendExtension(VideoFile.FullName, SmileVideoInformationUtility.flashConvertedExtension);
                    var result = new FileInfo(filePath);
                    return result;
                } else {
                    return VideoFile;
                }
            }
        }

        #endregion

        #region function

        protected virtual void OnLoadDataWithoutSessionStart()
        { }
        protected virtual void OnLoadDataWithoutSessionEnd()
        { }

        protected Task LoadDataWithoutSessionAsync(CacheSpan imageCacheSpan)
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

        //Task<RawSmileVideoGetflvModel> LoadGetflvFromServiceAsync()
        //{
        //    if(Information.InformationLoadState == LoadState.Failure) {
        //        return Task.FromResult(default(RawSmileVideoGetflvModel));
        //    }

        //    var getflv = new Getflv(Mediation);
        //    return getflv.LoadAsync(VideoId, Information.WatchUrl, Information.MovieType);
        //}

        async Task LoadGetflvAsync()
        {
            OnLoadGetflvStart();

            //var rawVideoGetflvModel = await LoadGetflvFromServiceAsync();
            //if(rawVideoGetflvModel != null) {
            //    Information.SetGetflvModel(rawVideoGetflvModel);

            //    var path = Path.Combine(Information.CacheDirectory.FullName, PathUtility.CreateFileName(VideoId, "getflv", "xml"));
            //    SerializeUtility.SaveXmlSerializeToFile(path, rawVideoGetflvModel);
            //}
            var result = await Information.LoadGetflvAsync(true, Setting.Download.UsingDmc);

            OnLoadGetflvEnd();
        }

        protected virtual void OnLoadVideoStart()
        { }
        protected virtual void OnLoadVideoEnd()
        { }

        protected async Task LoadVideoAsync(Uri downloadUri, FileInfo donwloadFile, long headPosition)
        {
            OnLoadVideoStart();

            VideoLoadState = LoadState.Preparation;
            VideoFile = donwloadFile;
            DownloadUri = downloadUri;

            DownloadCancel = new CancellationTokenSource();
            using(var downloader = new SmileVideoDownloader(downloadUri, Session, Information.WatchUrl, DownloadCancel.Token) {
                ReceiveBufferSize = Constants.ServiceSmileVideoReceiveBuffer,
                DownloadTotalSize = VideoTotalSize,
                RangeHeadPotision = headPosition,
            }) {
                Mediation.Logger.Information($"{VideoId}: download start: uri: {downloadUri}, head: {headPosition}, size: {VideoTotalSize}");

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var fileMode = downloader.UsingRangeDonwload ? FileMode.Append : FileMode.Create;
                using(VideoStream = new FileStream(VideoFile.FullName, fileMode, FileAccess.Write, FileShare.Read)) {
                    try {
                        downloader.DownloadStart += Downloader_DownloadStart;
                        downloader.DownloadingError += Downloader_DownloadingError;
                        downloader.Downloading += Downloader_Downloading;
                        downloader.Downloaded += Downloader_Downloaded;

                        await downloader.StartAsync();
                        if(downloader.Completed) {
                            if(UsingDmc.Value) {
                                var mux = DmcMultiplexer;
                                var role = SmileVideoInformationUtility.GetDmcRoleKey(mux.VideoSrcIds.First(), mux.AudioSrcIds.First());
                                //if(!Information.DmcItems.ContainsKey(role)) {
                                //    var dmcItem = new SmileVideoDmcItemModel() {
                                //        Video = mux.VideoSrcIds.First(),
                                //        Audio = mux.AudioSrcIds.First(),
                                //    };
                                //    Information.DmcItems[role] = dmcItem;
                                //}
                                VideoFile.Refresh();
                                //Information.DmcItems[role].IsLoaded = VideoFile.Length == Information.DmcItems[role].Length;
                                // 理屈で言えばここは絶対に存在する
                                Information.SetDmcLoaded(mux.VideoSrcIds.First(), mux.AudioSrcIds.First(), VideoFile.Length == Information.DmcItems[role].Length);
                                if(Information.DmcItems[role].IsLoaded) {
                                    VideoLoadState = LoadState.Loaded;
                                } else {
                                    VideoLoadState = LoadState.Failure;
                                }
                            } else {
                                VideoLoadState = LoadState.Loaded;
                                if(Information.IsEconomyMode) {
                                    Information.LoadedEconomyVideo = true;
                                } else {
                                    Information.LoadedNormalVideo = true;
                                }
                            }
                            Information.SaveSetting(false);
                        } else {
                            VideoLoadState = LoadState.Failure;
                        }
                        if(!downloader.Canceled) {
                            OnLoadVideoEnd();
                        }
                    } catch(Exception ex) {
                        Mediation.Logger.Error(ex);
                        VideoLoadState = LoadState.Failure;
                    } finally {
                        downloader.DownloadStart -= Downloader_DownloadStart;
                        downloader.DownloadingError -= Downloader_DownloadingError;
                        downloader.Downloading -= Downloader_Downloading;
                        downloader.Downloaded -= Downloader_Downloaded;

                        //OnLoadVideoEnd();
                        stopWatch.Stop();
                        Mediation.Logger.Information($"{VideoId}: download end: {stopWatch.Elapsed}");
                    }
                }
            }
        }

        protected Tuple<string, RawSmileVideoDmcObjectModel> GetDmcObject()
        {
            var info = Information.DmcInfo;

            var model = new RawSmileVideoDmcObjectModel();
            {
                var with = model.Data.Session;
                var session = info.SelectToken("session_api");

                var uri = session["api_urls"].First().Value<string>();

                with.RecipeId = session["recipe_id"].Value<string>();
                with.ContentId = session["content_id"].Value<string>();
                with.Protocol.Name = session["protocols"].First().Value<string>();
                with.Priority = session["priority"].Value<string>();

                var mux = new RawSmileVideoDmcSrcIdToMultiplexerModel();
                mux.VideoSrcIds.InitializeRange(session["videos"].Values<string>());
                mux.AudioSrcIds.InitializeRange(session["audios"].Values<string>());
                var idSet = new RawSmileVideoDmcContentSrcIdSetModel();
                idSet.SrcIdToMultiplexers.Add(mux);
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

        /// <summary>
        /// 新形式でダウンロードする。
        /// </summary>
        /// <returns>真: 新形式で処理できた(ダウンロードが成功したかどうかではない)、偽: 新形式では処理できない。</returns>
        protected async Task<bool> LoadDmcVideoAsync()
        {
            if(!Information.IsDmc) {
                return false;
            }

            VideoLoadState = LoadState.Preparation;

            var dmc = new Dmc(Mediation);
            var tuple = GetDmcObject();
            DmcApiUri = new Uri(tuple.Item1);
            var model = tuple.Item2;

            // 動画ソースを選りすぐる
            var sendMux = model.Data.Session.ContentSrcIdSets.First().SrcIdToMultiplexers.First();
            var sendVideoWeights = SmileVideoDmcObjectUtility.GetVideoWeights(sendMux, Setting.Download.VideoWeight);
            sendMux.VideoSrcIds.InitializeRange(sendVideoWeights.ToArray());
            var sendAudioWeights = SmileVideoDmcObjectUtility.GetAudioWeights(sendMux, Setting.Download.AudioWeight);
            sendMux.AudioSrcIds.InitializeRange(sendAudioWeights.ToArray());

            var result = await dmc.LoadAsync(DmcApiUri, model);

            SerializeUtility.SaveXmlSerializeToFile(Information.DmcFile.FullName, result);
            if(!SmileVideoDmcObjectUtility.IsSuccessResponse(result)) {
                Mediation.Logger.Warning($"{VideoId}: {result.Meta.Status}, {result.Meta.Message}");
                return false;
            }

            DmcObject = result;

            var downloadUri = new Uri(result.Data.Session.ContentUri);

            var mux = DmcMultiplexer;
            var ext = DmcFileExtension;
            var video = mux.VideoSrcIds.First();
            var audio = mux.AudioSrcIds.First();
            var downloadPath = Information.GetDmcVideoFileName(video, audio, ext);
            var downloadFile = new FileInfo(downloadPath);
            downloadFile.Refresh();
            var headPosition = 0L;
            if(downloadFile.Exists) {
                headPosition = downloadFile.Length;
            }

            UsingDmc.Value = true;

            var role = SmileVideoInformationUtility.GetDmcRoleKey(video, audio);
            SmileVideoDmcItemModel dmcItem;
            if(Information.DmcItems.TryGetValue(role, out dmcItem)) {
                if(dmcItem.IsLoaded && downloadFile.Exists) {
                    if(dmcItem.Length == downloadFile.Length) {
                        LoadVideoFromCache(downloadFile);
                        var task = dmc.CloseAsync(DmcApiUri, DmcObject);
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

        protected void ImportCommentThread(RawSmileVideoMsgPacketModel rawMessagePacket)
        {
            CommentThread = rawMessagePacket.Thread.First(t => string.IsNullOrWhiteSpace(t.Fork));
        }

        protected async Task<RawSmileVideoMsgPacketModel> LoadMsgCoreAsync(int getCount, int rangeHeadMinutes, int rangeTailMinutes, int rangeGetCount, int rangeGetAllCount)
        {
            //var getThreadkey = new Getthreadkey(Mediation);
            //var threadkeyModel = await getThreadkey.LoadAsync(
            //    Information.ThreadId
            //);
            await Information.LoadGetthreadkeyAsync();

            var msg = new Msg(Mediation);
            return await msg.LoadAsync(
                Information.MessageServerUrl,
                //string.IsNullOrWhiteSpace(VideoInformationViewModel.OptionalThreadId) ? VideoInformationViewModel.ThreadId: VideoInformationViewModel.OptionalThreadId,
                Information.ThreadId,
                Information.UserId,
                getCount,
                rangeHeadMinutes, rangeTailMinutes, rangeGetCount,
                rangeGetAllCount,
                Information.Getthreadkey
            );
        }

        protected async Task<RawSmileVideoMsgPacketModel> LoadMsgAsync(CacheSpan msgCacheSpan)
        {
            OnLoadMsgStart();

            CommentLoadState = LoadState.Preparation;

            //TODO; キャッシュチェック時のファイル処理関係は共通化可能
            var cacheFilePath = Information.MsgFile.FullName;
            if(File.Exists(cacheFilePath)) {
                var fileInfo = new FileInfo(cacheFilePath);
                if(msgCacheSpan.IsCacheTime(fileInfo.LastWriteTime) && Constants.MinimumXmlFileSize <= fileInfo.Length) {
                    CommentLoadState = LoadState.Loading;
                    using(var stream = new FileStream(cacheFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        var result = Msg.ConvertFromRawPacketData(stream);
                        OnLoadMsgEnd();
                        return result;
                    }
                }
            }

            CommentLoadState = LoadState.Loading;

            //var getThreadkey = new Getthreadkey(Mediation);
            //var threadkeyModel = await getThreadkey.LoadAsync(
            //    VideoInformation.ThreadId
            //);

            //var msg = new Msg(Mediation);
            //var rawMessagePacket = await msg.LoadAsync(
            //    VideoInformation.MessageServerUrl,
            //    //string.IsNullOrWhiteSpace(VideoInformationViewModel.OptionalThreadId) ? VideoInformationViewModel.ThreadId: VideoInformationViewModel.OptionalThreadId,
            //    VideoInformation.ThreadId,
            //    VideoInformation.UserId,
            //    1000,
            //    1, (int)VideoInformation.Length.TotalMinutes, 100,
            //    500,
            //    threadkeyModel
            //);
            var rawMessagePacket = await LoadMsgCoreAsync(1000, 1, (int)Information.Length.TotalMinutes, 100, 500);
            ImportCommentThread(rawMessagePacket);

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

        protected virtual Task LoadCommentAsync(RawSmileVideoMsgPacketModel rawMsgPacket)
        {
            CommentLoadState = LoadState.Loaded;
            return Task.CompletedTask;
        }

        protected static long GetDownloadHeadPosition(FileInfo fileInfo, long completeSize)
        {
            if(fileInfo.Exists) {
                if(fileInfo.Length == completeSize) {
                    return isDonwloaded;
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
            VideoLoadState = LoadState.Loaded;

            // TODO: HTMLがあること前提
            Information.LoadLocalPageHtmlAsync().Wait();

            OnLoadVideoEnd();
        }

        protected async Task LoadDataWithSessionAsync()
        {
            OnLoadDataWithSessionStart();

            var tcs = new CancellationTokenSource();
            await LoadGetflvAsync();

            if(Information.InformationLoadState == LoadState.Failure || Information.HasGetflvError) {
                InformationLoadState = LoadState.Failure;
                return;
            }

            var commentTask = LoadMsgAsync(Constants.ServiceSmileVideoMsgCacheSpan).ContinueWith(task => LoadCommentAsync(task.Result), TaskScheduler.FromCurrentSynchronizationContext());

            // キャッシュとかエコノミー確認であれこれ分岐
            Debug.Assert(Information != null);

            if(Information.MovieType == SmileVideoMovieType.Swf && Information.ConvertedSwf) {
                var f = new FileInfo(Path.Combine(Information.CacheDirectory.FullName, Information.GetVideoFileName(false)));
                LoadVideoFromCache(f);
                return;
            }

            if(Setting.Download.UsingDmc) {
                if(await LoadDmcVideoAsync()) {
                    // 新形式で何かしら処理できた
                    return;
                }
            }

            var normalVideoFile = new FileInfo(Path.Combine(Information.CacheDirectory.FullName, Information.GetVideoFileName(false)));
            var normalHeadPosition = GetDownloadHeadPosition(normalVideoFile, Information.SizeHigh);
            if(normalHeadPosition == isDonwloaded) {
                // ダウンロード済み
                IsEconomyMode = false;
                LoadVideoFromCache(normalVideoFile);
                return;
            }

            long headPosition = 0;
            FileInfo donwloadFile;
            // エコノミーキャッシュが存在しても非エコノミーでダウンロードできるなら新たにファイルを落とす。
            IsEconomyMode = Information.IsEconomyMode;
            if(IsEconomyMode) {
                var economyVideoFile = new FileInfo(Path.Combine(Information.CacheDirectory.FullName, Information.GetVideoFileName(true)));
                var economyHeadPosition = GetDownloadHeadPosition(economyVideoFile, Information.SizeLow);
                if(economyHeadPosition == isDonwloaded) {
                    // エコノミーダウンロード済み
                    IsEconomyMode = true;
                    LoadVideoFromCache(economyVideoFile);
                    return;
                }
                headPosition = economyHeadPosition;
                donwloadFile = economyVideoFile;
            } else {
                headPosition = normalHeadPosition;
                donwloadFile = normalVideoFile;
            }

            VideoTotalSize = Information.VideoSize;

            await LoadVideoAsync(Information.MovieServerUrl, donwloadFile, headPosition);
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

            //var baseDir = RestrictUtility.Block(() => {
            //    var response = Mediation.Request(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
            //    return (DirectoryInfo)response.Result;
            //});
            ////TODO: キャッシュディレクトリ処理重複
            ////DownloadDirectory = Directory.CreateDirectory(Path.Combine(baseDir.FullName, Constants.SmileVideoCacheVideosDirectoryName, videoId));
            //DownloadDirectory = Directory.CreateDirectory(Path.Combine(baseDir.FullName, videoId));

            OnLoadGetthumbinfoStart();
            //var rawGetthumbinfoModel = await LoadGetthumbinfoAsync(videoId, thumbCacheSpan);
            //VideoInformationViewModel = new SmileVideoInformationViewModel(Mediation, rawGetthumbinfoModel.Thumb, SmileVideoInformationViewModel.NoOrderd);
            //VideoInformation = await SmileVideoInformationViewModel.CreateFromVideoIdAsync(Mediation, videoId, thumbCacheSpan);
            var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));
            Information = await Mediation.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);

            OnLoadGetthumbinfoEnd();

            await LoadAsync(Information, false, thumbCacheSpan, imageCacheSpan);
        }

        protected virtual void InitializeStatus()
        {
            VideoLoadedSize = initVideoLoadSize;
            VideoTotalSize = initVideoTotalSize;
            VideoLoadState = LoadState.None;
            InformationLoadState = LoadState.None;
            ThumbnailLoadState = LoadState.None;
            CommentLoadState = LoadState.None;
            //IsProcessCancel = false;
            UsingDmc.Value = false;
            DmcObject = null;
            DmcApiUri = null;
            DmcPollingTask = null;
            DmcPollingWait = null;
            DmcPollingCancel = null;
            DownloadUri = null;
            DownloadCancel = null;
        }

        protected virtual Task StopPrevProcessAsync()
        {
            //// ぐっちゃぐちゃ
            //var wait = new EventWaitHandle(false, EventResetMode.AutoReset);
            //PropertyChangedEventHandler changedEvent = null;

            //changedEvent = (sender, e) => {
            //    if(e.PropertyName == nameof(VideoLoadState) && VideoLoadState == LoadState.None) {
            //        PropertyChanged -= changedEvent;
            //        wait.Set();
            //    }
            //};
            //PropertyChanged += changedEvent;

            //var prevState = VideoLoadState;
            //IsProcessCancel = true;

            if(DownloadCancel != null) {
                Mediation.Logger.Trace($"{VideoId}: Cancel!");
                DownloadCancel.Cancel();
            }

            if(UsingDmc.Value) {
                return StopDmcDownloadAsync().ContinueWith(t => {
                    InitializeStatus();
                });
            }

            InitializeStatus();

            //if(prevState == LoadState.Preparation || prevState == LoadState.Loading) {
            //    return Task.Run(() => {
            //        wait.WaitOne();
            //        wait.Dispose();
            //        PropertyChanged -= changedEvent;
            //        InitializeStatus();
            //    });
            //} else {
            //    wait.Dispose();
            //    PropertyChanged -= changedEvent;
            //    InitializeStatus();
            //    return Task.CompletedTask;
            //}
            return Task.CompletedTask;
        }


        public virtual async Task LoadAsync(SmileVideoInformationViewModel videoInformation, bool forceEconomy, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            await StopPrevProcessAsync();

            OnLoadStart();

            InformationLoadState = LoadState.Loading;

            //var baseDir = RestrictUtility.Block(() => {
            //    var response = Mediation.Request(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
            //    return (DirectoryInfo)response.Result;
            //});
            ////DownloadDirectory = Directory.CreateDirectory(Path.Combine(baseDir.FullName, Constants.SmileVideoCacheVideosDirectoryName, videoInformation.VideoId));
            //DownloadDirectory = Directory.CreateDirectory(Path.Combine(baseDir.FullName, videoInformation.VideoId));

            OnLoadGetthumbinfoStart();
            Information = videoInformation;
            if(Information.InformationSource != SmileVideoInformationSource.Getthumbinfo) {
                await Information.LoadInformationDefaultAsync(thumbCacheSpan);
            }
            InformationLoadState = LoadState.Loaded;
            OnLoadGetthumbinfoEnd();

            var noSessionTask = LoadDataWithoutSessionAsync(imageCacheSpan);
            var sessionTask = LoadDataWithSessionAsync();

            //Task.WaitAll(noSessionTask, sessionTask);
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
            if(DmcApiUri != null && Information != null && Information.IsDownloading) {
                if(DmcPollingWait != null) {
                    if(!DmcPollingWait.SafeWaitHandle.IsClosed) {
                        DmcPollingWait.Set();
                        DmcPollingWait.Dispose();
                    }
                }

                if(DmcPollingCancel != null) {
                    Mediation.Logger.Debug($"{nameof(DmcPollingCancel)}: cancel!");
                    DmcPollingCancel.Cancel();
                }

                var dmc = new Dmc(Mediation);
                return dmc.CloseAsync(DmcApiUri, DmcObject);
            }

            return Task.CompletedTask;
        }

        Task PollingDmcDownloadAsync(string videoId, AutoResetEvent resetEvent, CancellationToken cancelToken)
        {
            return Task.Run(async () => {
                var pollingCount = 1;
                while(true) {
                    Mediation.Logger.Information($"{videoId}: polling wait... {pollingCount++}");
                    var isReset = resetEvent.WaitOne(Constants.ServiceSmileVideoDownloadDmcPollingWaitTime);
                    if(isReset) {
                        // 強制中止
                        Mediation.Logger.Information($"{videoId}: polling stop event is set!");
                        return;
                    }
                    if(cancelToken.IsCancellationRequested) {
                        Mediation.Logger.Information($"{videoId}: polling cancel!");
                        return;
                    }

                    if(DmcObject == null) {
                        Mediation.Logger.Debug($"{videoId}: {nameof(DmcObject)} is null!!");
                        return;
                    }
                    DmcObject.Data.Session.ModifiedTime = SmileVideoDmcObjectUtility.ConvertRawSessionTime(DateTime.Now);

                    var dmc = new Dmc(Mediation);
                    var model = await dmc.ReloadAsync(DmcApiUri, DmcObject);

                    if(cancelToken.IsCancellationRequested) {
                        Mediation.Logger.Information($"{videoId}: polling cancel reload");
                        return;
                    }

                    DmcObject = model;
                }
            }, cancelToken);
        }

        #endregion

        private void Downloader_DownloadStart(object sender, DownloadStartEventArgs e)
        {
            VideoLoadState = LoadState.Loading;

            if(e.DownloadStartType == DownloadStartType.Range) {
                DonwloadStartPosition = e.RangeHeadPosition;
            }
            var downloader = (SmileVideoDownloader)sender;
            Information.SetPageHtmlAsync(downloader.PageHtml, true).Wait();

            if(UsingDmc.Value) {
                if(downloader.ResponseHeaders.ContentLength.HasValue) {
                    Information.IsDownloading = true;

                    VideoFile.Refresh();

                    VideoTotalSize = VideoFile.Length + downloader.ResponseHeaders.ContentLength.Value;

                    Mediation.Logger.Information($"file:{VideoFile.Length}, response:{downloader.ResponseHeaders.ContentLength.Value}, total:{VideoTotalSize}");

                    var mux = DmcMultiplexer;
                    //var ext = DmcFileExtension;
                    var video = mux.VideoSrcIds.First();
                    var audio = mux.AudioSrcIds.First();
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
                    //bool isDmcLoaded;
                    //if(Information.DmcItems.TryGetValue(SmileVideoInformationUtility.GetDmcRoleKey(video, audio), out isDmcLoaded)) {
                    //    if(isDmcLoaded) {
                    //        // TODO: 根本的におかしいのかなんなのか、とりあえずの妥協
                    //        if(VideoTotalSize - 1 <= VideoFile.Length) {
                    //            VideoLoadedSize = VideoFile.Length;
                    //            e.Cancel = true;
                    //            e.Completed = true;
                    //            return;
                    //        }
                    //    }
                    //}
                }

                DmcPollingWait = new AutoResetEvent(false);
                DmcPollingCancel = new CancellationTokenSource();
                DmcPollingTask = PollingDmcDownloadAsync(VideoId, DmcPollingWait, DmcPollingCancel.Token);
            }

            OnDownloadStart(sender, e);
        }

        private void Downloader_Downloading(object sender, DownloadingEventArgs e)
        {
            var downloader = (Downloader)sender;
            var buffer = e.Data;
            VideoStream.Write(buffer.Array, 0, e.Data.Count);
            VideoLoadedSize = DonwloadStartPosition + downloader.DownloadedSize;

            if(e.Cancel || downloader.Canceled) {
                return;
            }
            //Mediation.Logger.Trace($"{VideoId}-{e.Counter}: {e.Data.Count}, {VideoLoadedSize:#,###}/{VideoTotalSize:#,###}, {e.SecondsDownlodingSize}");

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
            Mediation.Logger.Error(e.Exception);

            if(e.Cancel) {
                VideoLoadState = LoadState.Failure;
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
