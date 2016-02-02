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
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Event;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.NicoNico;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.NicoNico.Video
{
    /// <summary>
    /// 指定動画ダウンロード。
    /// </summary>
    public class NicoNicoVideoDownloadViewModel: ViewModelBase
    {
        #region variable

        LoadState _informationLoadState;
        LoadState _thumbnailLoadState;
        LoadState _commentLoadState;
        LoadState _videoLoadState;

        CacheState _cacheState;

        long _videoLoadedSize;
        long _videoSize = Downloader.UnknownDonwloadSize;

        #endregion

        public NicoNicoVideoDownloadViewModel(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; set; }

        public NicoNicoVideoInformationViewModel VideoInformationViewModel { get; set; }

        protected string VideoPath { get; set; }

        protected Stream VideoStream { get; private set; }

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
            private set { SetVariableValue(ref this._videoLoadedSize, value); }
        }

        public long VideoSize
        {
            get { return this._videoSize; }
            private set { SetVariableValue(ref this._videoSize, value); }
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

        protected void OnLoadDataWithoutSessionStart()
        { }
        protected void OnLoadDataWithoutSessionEnd()
        { }

        protected Task LoadDataWithoutSessionAsync()
        {
            ThumbnailLoadState = LoadState.Loading;
            OnLoadDataWithoutSessionStart();
            return VideoInformationViewModel.LoadImageAsync().ContinueWith(task => {
                ThumbnailLoadState = LoadState.Loaded;
                OnLoadDataWithoutSessionEnd();
            });
        }

        protected void OnLoadDataWithSessionStart()
        { }
        protected void OnLoadDataWithSessionEnd()
        { }

        protected async Task LoadDataWithSessionAsync()
        {
            OnLoadDataWithSessionStart();
            var request = new RequestModel(RequestKind.Session, ServiceType.NicoNico);
            var response = Mediation.Request(request);
            var session = (NicoNicoSessionViewModel)response.Result;
            var getflv = new Getflv(Mediation, session);
            getflv.SessionSupport = true;
            var rawVideoGetflvModel = await getflv.GetAsync(VideoInformationViewModel.VideoId);
            VideoInformationViewModel.SetGetflvModel(rawVideoGetflvModel);

            // TODO: 細かな制御と外部化
            if(VideoInformationViewModel.Done) {
                VideoLoadState = LoadState.Preparation;
                VideoSize = VideoInformationViewModel.VideoSize;

                using(var downloader = new NicoNicoVideoDownloader(VideoInformationViewModel.MovieServerUrl, session, VideoInformationViewModel.WatchUrl) {
                    ReceiveBufferSize = Constants.ServiceNicoNicoVideoReceiveBuffer,
                    DownloadTotalSize = VideoSize,
                }) {
                    VideoPath = @"z:\test.mp4";
                    using(VideoStream = new FileStream(VideoPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)) {
                        try {
                            downloader.DownloadStart += Downloader_DownloadStart;
                            downloader.DownloadingError += Downloader_DownloadingError;
                            downloader.Downloading += Downloader_Downloading;

                            await downloader.StartAsync();
                            if(downloader.Completed) {
                                VideoLoadState = LoadState.Loaded;
                            } else {
                                VideoLoadState = LoadState.Failure;
                            }
                        } catch(Exception ex) {
                            Debug.WriteLine(ex);
                            VideoLoadState = LoadState.Failure;
                        } finally {
                            downloader.DownloadStart -= Downloader_DownloadStart;
                            downloader.DownloadingError -= Downloader_DownloadingError;
                            downloader.Downloading -= Downloader_Downloading;
                            OnLoadDataWithSessionEnd();
                        }
                    }
                }
            }
        }

        protected void OnLoadStart()
        { }

        protected void OnLoadEnd()
        { }

        public async Task LoadAsync(string videoId)
        {
            OnLoadStart();

            InformationLoadState = LoadState.Loading;
            var getthumbinfo = new Getthumbinfo(Mediation);
            var rawGetthumbinfoModel = await getthumbinfo.GetAsync(videoId);
            VideoInformationViewModel = new NicoNicoVideoInformationViewModel(Mediation, rawGetthumbinfoModel.Thumb, NicoNicoVideoInformationViewModel.NoOrderd);
            InformationLoadState = LoadState.Loaded;

            var noSessionTask = LoadDataWithoutSessionAsync();
            var sessionTask = LoadDataWithSessionAsync();

            //Task.WaitAll(noSessionTask, sessionTask);
            await noSessionTask;
            await sessionTask;

            OnLoadEnd();
        }

        protected void OnDownloadStart(object sender, DownloadStartEventArgs e)
        { }

        protected void OnDownloading(object sender, DownloadingEventArgs e)
        { }

        protected void OnDownloadingError(object sender, DownloadingErrorEventArgs e)
        { }

        #endregion

        private void Downloader_DownloadStart(object sender, DownloadStartEventArgs e)
        {
            VideoLoadState = LoadState.Loading;
            OnDownloadStart(sender, e);
        }

        private void Downloader_Downloading(object sender, DownloadingEventArgs e)
        {
            var downloader = (Downloader)sender;
            var buffer = e.Data;
            VideoStream.Write(buffer.Array, 0, e.Data.Count);
            VideoLoadedSize = downloader.DownloadedSize;
            
            Debug.WriteLine($"{e.Counter}: {e.Data.Count}, {VideoLoadedSize:#,###}/{VideoSize:#,###}");

            OnDownloading(sender, e);
        }

        private void Downloader_DownloadingError(object sender, DownloadingErrorEventArgs e)
        {
            int retry = Constants.ServiceNicoNicoVideoDownloadingErrorRetryCount;

            e.Cancel = retry < e.Counter;
            Debug.WriteLine(e.Exception);

            if(e.Cancel) {
                VideoLoadState = LoadState.Failure;
            } else {
                var time = Constants.ServiceNicoNicoVideoDownloadingErrorWaitTime;
                Thread.Sleep(time);
            }

            OnDownloadingError(sender, e);
        }
    }
}
