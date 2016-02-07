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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Event;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    /// <summary>
    /// 指定動画ダウンロード。
    /// </summary>
    public class SmileVideoDownloadViewModel: ViewModelBase
    {
        #region define

        const long isDonwloaded = -1;

        #endregion

        #region variable

        LoadState _informationLoadState;
        LoadState _thumbnailLoadState;
        LoadState _commentLoadState;
        LoadState _videoLoadState;

        CacheState _cacheState;

        long _videoLoadedSize = 0;
        long _videoTotalSize = 1;

        #endregion

        public SmileVideoDownloadViewModel(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; set; }

        public SmileVideoInformationViewModel VideoInformationViewModel { get; set; }

        protected DirectoryInfo DownloadDirectory { get; set; }

        protected FileInfo VideoFile { get; set; }

        protected Stream VideoStream { get; private set; }

        protected long DonwloadStartPosition { get; private set; }

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
                        return VideoInformationViewModel.ThumbnailImage;

                    default:
                        return null;
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
            return VideoInformationViewModel.LoadImageAsync(imageCacheSpan).ContinueWith(task => {
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

        Task LoadGetflvAsync(SmileSessionViewModel session)
        {
            OnLoadGetflvStart();

            // こいつはキャッシュ参照しないけどキャッシュ自体は作っておく
            var getflv = new Getflv(Mediation, session);
            getflv.SessionSupport = true;
            return getflv.GetAsync(VideoInformationViewModel.VideoId).ContinueWith(task => {
                var rawVideoGetflvModel = task.Result;
                Task.Run(() => {
                    var path = Path.Combine(DownloadDirectory.FullName, Constants.SmileVideoCacheGetflvFileName);
                    SerializeUtility.SaveXmlSerializeToFile(path, rawVideoGetflvModel);
                });
                VideoInformationViewModel.SetGetflvModel(rawVideoGetflvModel);

                OnLoadGetflvEnd();
            });
        }

        protected virtual void OnLoadVideoStart()
        { }
        protected virtual void OnLoadVideoEnd()
        { }

        protected async Task LoadVideoAsync(SmileSessionViewModel session, FileInfo donwloadFile, long headPosition)
        {
            OnLoadVideoStart();

            VideoLoadState = LoadState.Preparation;
            VideoTotalSize = VideoInformationViewModel.VideoSize;
            VideoFile = donwloadFile;

            using(var downloader = new SmileVideoDownloader(VideoInformationViewModel.MovieServerUrl, session, VideoInformationViewModel.WatchUrl) {
                ReceiveBufferSize = Constants.ServiceSmileVideoReceiveBuffer,
                DownloadTotalSize = VideoTotalSize,
                RangeHeadPotision = headPosition,
            }) {
                var fileMode = downloader.UsingRangeDonwload ? FileMode.Append: FileMode.Create;
                using(VideoStream = new FileStream(VideoFile.FullName, fileMode, FileAccess.Write, FileShare.Read)) {
                    try {
                        downloader.DownloadStart += Downloader_DownloadStart;
                        downloader.DownloadingError += Downloader_DownloadingError;
                        downloader.Downloading += Downloader_Downloading;

                        await downloader.StartAsync();
                        if(downloader.Completed) {
                            VideoLoadState = LoadState.Loaded;
                            if(VideoInformationViewModel.IsEconomyMode) {
                                VideoInformationViewModel.LoadedEconomyVideo = true;
                            }else {
                                VideoInformationViewModel.LoadedNormalVideo = true;
                            }
                            VideoInformationViewModel.SaveSetting();
                        } else {
                            VideoLoadState = LoadState.Failure;
                        }
                        OnLoadVideoEnd();
                    } catch(Exception ex) {
                        Debug.WriteLine(ex);
                        VideoLoadState = LoadState.Failure;
                    } finally {
                        downloader.DownloadStart -= Downloader_DownloadStart;
                        downloader.DownloadingError -= Downloader_DownloadingError;
                        downloader.Downloading -= Downloader_Downloading;

                        OnLoadVideoEnd();
                    }
                }
            }
        }

        protected virtual void OnLoadMsgStart()
        { }
        protected virtual void OnLoadMsgEnd()
        { }


        protected async Task<RawSmileVideoMsgPacketModel> LoadMsgAsync(SmileSessionViewModel session, CacheSpan msgCacheSpan)
        {
            OnLoadMsgStart();

            CommentLoadState = LoadState.Preparation;

            var cacheFilePath = Path.Combine(DownloadDirectory.FullName, Constants.SmileVideoCacheMsgFileName);
            if(File.Exists(cacheFilePath)) {
                var fileInfo = new FileInfo(cacheFilePath);
                if(msgCacheSpan.IsCacheTime(fileInfo.LastWriteTime) && Constants.MinimumXmlFileSize <= fileInfo.Length) {
                    CommentLoadState = LoadState.Loading;
                    using(var stream = new FileStream(cacheFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        var result = Msg.Load(stream);
                        OnLoadMsgEnd();
                        return result;
                    }
                }
            }

            CommentLoadState = LoadState.Loading;

            var getThreadkey = new Getthreadkey(Mediation);
            var threadkeyModel = await getThreadkey.GetAsync(VideoInformationViewModel.ThreadId);

            var msg = new Msg(Mediation, session);
            var rawMessagePacket = await msg.GetAsync(
                VideoInformationViewModel.MessageServerUrl, 
                VideoInformationViewModel.ThreadId, 
                VideoInformationViewModel.UserId, 
                1000, 
                1, 10, 100, 
                500, 
                threadkeyModel
            );

            // キャッシュ構築
            try {
                SerializeUtility.SaveXmlSerializeToFile(cacheFilePath, rawMessagePacket);
            } catch(FileNotFoundException) {
                // BUGS: いかんのう
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
            OnLoadVideoEnd();
        }

        protected async Task LoadDataWithSessionAsync()
        {
            OnLoadDataWithSessionStart();

            var request = new RequestModel(RequestKind.Session, ServiceType.Smile);
            var response = Mediation.Request(request);
            var session = (SmileSessionViewModel)response.Result;
            var tcs = new CancellationTokenSource();
            await LoadGetflvAsync(session);

            if(VideoInformationViewModel.HasError) {
                InformationLoadState = LoadState.Failure;
                return;
            }

            var commentTask = LoadMsgAsync(session, Constants.ServiceSmileVideoMsgCacheSpan).ContinueWith(task => LoadCommentAsync(task.Result));

            // キャッシュとかエコノミー確認であれこれ分岐
            Debug.Assert(DownloadDirectory != null);
            var normalVideoFile = new FileInfo(Path.Combine(DownloadDirectory.FullName, VideoInformationViewModel.GetVideoFileName(false)));
            var normalHeadPosition = GetDownloadHeadPosition(normalVideoFile, VideoInformationViewModel.SizeHigh);
            if(normalHeadPosition == isDonwloaded) {
                // ダウンロード済み
                LoadVideoFromCache(normalVideoFile);
                return;
            }

            long headPosition = 0;
            FileInfo donwloadFile;
            // エコノミーキャッシュが存在しても非エコノミーでダウンロードできるなら新たにファイルを落とす。
            var isEconomyMode = VideoInformationViewModel.IsEconomyMode;
            if(isEconomyMode) {
                var economyVideoFile = new FileInfo(Path.Combine(DownloadDirectory.FullName, VideoInformationViewModel.GetVideoFileName(true)));
                var economyHeadPosition = GetDownloadHeadPosition(economyVideoFile, VideoInformationViewModel.SizeLow);
                if(economyHeadPosition == isDonwloaded) {
                    // エコノミーダウンロード済み
                    LoadVideoFromCache(economyVideoFile);
                    return;
                }
                headPosition = economyHeadPosition;
                donwloadFile = economyVideoFile;
            } else {
                headPosition = normalHeadPosition;
                donwloadFile = normalVideoFile;
            }

            await LoadVideoAsync(session, donwloadFile, headPosition);
        }

        protected virtual void OnLoadStart()
        { }

        protected virtual void OnLoadEnd()
        { }

        async Task<RawSmileVideoThumbResponseModel> LoadGetthumbinfoAsync(string videoId, CacheSpan thumbCacheSpan)
        {
            var cachedThumbFilePath = Path.Combine(DownloadDirectory.FullName, Constants.SmileVideoCacheGetthumbinfoFileName);
            if(File.Exists(cachedThumbFilePath)) {
                var fileInfo = new FileInfo(cachedThumbFilePath);
                if(thumbCacheSpan.IsCacheTime(fileInfo.LastWriteTime) && Constants.MinimumXmlFileSize <= fileInfo.Length) {
                    using(var stream = new FileStream(cachedThumbFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        return Getthumbinfo.Load(stream);
                    }
                }
            }

            var getthumbinfo = new Getthumbinfo(Mediation);
            var result = await getthumbinfo.GetAsync(videoId);

            // キャッシュ構築
            try {
                SerializeUtility.SaveXmlSerializeToFile(cachedThumbFilePath, result);
            } catch(FileNotFoundException) {
                // BUGS: いかんのう
            }

            return result;
        }

        protected virtual void OnLoadGetthumbinfoStart() { }
        protected virtual void OnLoadGetthumbinfoEnd() { }

        public async Task LoadAsync(string videoId, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            InformationLoadState = LoadState.Loading;

            var baseDir = RestrictUtility.Block(() => {
                var response = Mediation.Request(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
                return (DirectoryInfo)response.Result;
            });
            DownloadDirectory = Directory.CreateDirectory(Path.Combine(baseDir.FullName, videoId));

            OnLoadGetthumbinfoStart();
            var rawGetthumbinfoModel = await LoadGetthumbinfoAsync(videoId, thumbCacheSpan);
            VideoInformationViewModel = new SmileVideoInformationViewModel(Mediation, rawGetthumbinfoModel.Thumb, SmileVideoInformationViewModel.NoOrderd);
            OnLoadGetthumbinfoEnd();

            await LoadAsync(VideoInformationViewModel, thumbCacheSpan, imageCacheSpan);
        }

        public async Task LoadAsync(SmileVideoInformationViewModel videoInformation, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            OnLoadStart();

            InformationLoadState = LoadState.Loading;

            var baseDir = RestrictUtility.Block(() => {
                var response = Mediation.Request(new RequestModel(RequestKind.CacheDirectory, ServiceType.SmileVideo));
                return (DirectoryInfo)response.Result;
            });
            DownloadDirectory = Directory.CreateDirectory(Path.Combine(baseDir.FullName, videoInformation.VideoId));

            if(videoInformation.VideoInformationSource != SmileVideoVideoInformationSource.Getthumbinfo) {
                OnLoadGetthumbinfoStart();
                var rawGetthumbinfoModel = await LoadGetthumbinfoAsync(videoInformation.VideoId, thumbCacheSpan);
                VideoInformationViewModel = new SmileVideoInformationViewModel(Mediation, rawGetthumbinfoModel.Thumb, SmileVideoInformationViewModel.NoOrderd);
                OnLoadGetthumbinfoEnd();
            } else {
                VideoInformationViewModel = videoInformation;
            }
            InformationLoadState = LoadState.Loaded;

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

        protected virtual void OnDownloadingError(object sender, DownloadingErrorEventArgs e)
        { }

        #endregion

        private void Downloader_DownloadStart(object sender, DownloadStartEventArgs e)
        {
            VideoLoadState = LoadState.Loading;
            if(e.DownloadStartType == DownloadStartType.Range) {
                DonwloadStartPosition = e.RangeHeadPosition;
            }

            OnDownloadStart(sender, e);
        }

        private void Downloader_Downloading(object sender, DownloadingEventArgs e)
        {
            var downloader = (Downloader)sender;
            var buffer = e.Data;
            VideoStream.Write(buffer.Array, 0, e.Data.Count);
            VideoLoadedSize = DonwloadStartPosition + downloader.DownloadedSize;

            Debug.WriteLine($"{e.Counter}: {e.Data.Count}, {VideoLoadedSize:#,###}/{VideoTotalSize:#,###}");

            OnDownloading(sender, e);
        }

        private void Downloader_DownloadingError(object sender, DownloadingErrorEventArgs e)
        {
            int retry = Constants.ServiceSmileVideoDownloadingErrorRetryCount;

            e.Cancel = retry < e.Counter;
            Debug.WriteLine(e.Exception);

            if(e.Cancel) {
                VideoLoadState = LoadState.Failure;
            } else {
                var time = Constants.ServiceSmileVideoDownloadingErrorWaitTime;
                Thread.Sleep(time);
            }

            OnDownloadingError(sender, e);
        }
    }
}
