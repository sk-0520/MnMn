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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.NicoNico;
using Vlc.DotNet.Wpf;
using Vlc.DotNet.Core;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using System.Windows.Media;
using ContentTypeTextNet.MnMn.MnMn.View.Window.NicoNico.Video;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Logic.NicoNico.Video;
using System.Windows;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Window.NicoNico.Video
{
    public class VideoPlayerViewModel: ViewModelBase
    {
        #region variable

        LoadState _informationLoadState;
        LoadState _thumbnailLoadState;
        LoadState _commentLoadState;
        LoadState _videoLoadState;

        bool _canVideoPlay;
        bool _isVideoPlayng;

        CacheState _cacheState;

        long _videoLoadedSize;
        long _videoSize = 1;

        float _videoPosition;

        #endregion

        public VideoPlayerViewModel(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; set; }

        NicoNicoVideoPlayerWindow View { get; set; }
        Vlc.DotNet.Forms.VlcControl Player { get; set; }
        Slider VideoSilder { get; set; }

        public NicoNicoVideoInformationViewModel VideoInformationViewModel { get; set; }

        public bool ChangingVideoPosition { get; set; }
        bool IsDead { get; set; }
        long VideoPlayLowestSize => Constants.ServiceNicoNicoVideoPlayLowestSize;
        Stream VideoStream { get; set; }
        string VideoPath { get; set; }


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

        public bool CanVideoPlay
        {
            get { return this._canVideoPlay; }
            set { SetVariableValue(ref this._canVideoPlay, value); }
        }

        public bool IsVideoPlayng
        {
            get { return this._isVideoPlayng; }
            set { SetVariableValue(ref this._isVideoPlayng, value); }
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

        public float VideoPosition
        {
            get { return this._videoPosition; }
            set { SetVariableValue(ref this._videoPosition, value); }
        }

        #endregion

        #region function

        Task LoadDataWithoutSessionAsync()
        {
            ThumbnailLoadState = LoadState.Loading;
            return VideoInformationViewModel.LoadImageAsync().ContinueWith(task => {
                ThumbnailLoadState = LoadState.Loaded;
            });
        }

        async Task LoadDataWithSessionAsync()
        {
            var request = new RequestModel(RequestKind.Session, ServiceType.NicoNico);
            var response = Mediation.Request(request);
            var session = (NicoNicoSessionViewModel)response.Result;
            var getflv = new Getflv(Mediation, session);
            getflv.SessionSupport = true;
            var rawVideoGetflvModel = await getflv.GetAsync(VideoInformationViewModel.VideoId);
            VideoInformationViewModel.SetGetflvModel(rawVideoGetflvModel);

            // TODO: 細かな制御と外部化
            if(VideoInformationViewModel.Done) {
                VideoLoadState = LoadState.Loading;
                VideoSize = VideoInformationViewModel.VideoSize;

                using(var downloader = new NicoNicoVideoDownloader(VideoInformationViewModel.MovieServerUrl, session, VideoInformationViewModel.WatchUrl) {
                    ReceiveBufferSize = Constants.ServiceNicoNicoVideoReceiveBuffer,
                    DownloadTotalSize = VideoSize,
                }) {
                    VideoPath = @"z:\test.mp4";
                    using(VideoStream = new FileStream(VideoPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)) {
                        try {
                            downloader.DownloadingError += Downloader_DownloadingError;
                            downloader.Downloading += Downloader_Downloading;

                            await downloader.StartAsync();
                            if(downloader.Completed) {
                                VideoLoadState = LoadState.Loaded;
                                // あまりにも小さい場合は読み込み完了時にも再生できなくなっている
                                if(!CanVideoPlay) {
                                    AutoPlay(new FileInfo(VideoPath));
                                }
                            } else {
                                VideoLoadState = LoadState.Failure;
                            }
                        } catch(Exception ex) {
                            Debug.WriteLine(ex);
                            VideoLoadState = LoadState.Failure;
                        } finally {
                            downloader.DownloadingError -= Downloader_DownloadingError;
                            downloader.Downloading -= Downloader_Downloading;
                        }
                    }
                }
            }
        }

        public async Task LoadAsync(string videoId)
        {
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
        }

        internal void SetView(NicoNicoVideoPlayerWindow view)
        {
            View = view;
            Player = view.player.MediaPlayer;
            VideoSilder = view.videoSilder;

            // あれこれイベント
            View.Closed += View_Closed;
            Player.PositionChanged += Player_PositionChanged;
            VideoSilder.PreviewMouseDown += VideoSilder_PreviewMouseDown;
        }

        void AutoPlay(FileInfo fileInfo)
        {
            Player.Play(fileInfo);
            CanVideoPlay = true;
        }

        #endregion

        private void View_Closed(object sender, EventArgs e)
        {
            VideoSilder.PreviewMouseDown -= VideoSilder_PreviewMouseDown;
            Player.PositionChanged -= Player_PositionChanged;
            View.Closed -= View_Closed;

            IsDead = true;

            if(Player.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing) {
                Player.Stop();
            }
        }

        private void Player_PositionChanged(object sender, VlcMediaPlayerPositionChangedEventArgs e)
        {
            if(CanVideoPlay && !ChangingVideoPosition) {
                VideoPosition = e.NewPosition;
            }
        }

        private void VideoSilder_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(!CanVideoPlay) {
                return;
            }

            ChangingVideoPosition = true;

            VideoSilder.PreviewMouseUp += VideoSilder_PreviewMouseUp;
        }

        private void VideoSilder_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Debug.Assert(CanVideoPlay);

            VideoSilder.PreviewMouseUp -= VideoSilder_PreviewMouseUp;

            // TODO: 読み込んでない部分は移動不可にする
            var nextPosition = (float)VideoSilder.Value;
            ChangingVideoPosition = false;
            Player.Position = nextPosition;
        }

        private void Downloader_Downloading(object sender, Define.Event.DownloadingEventArgs e)
        {
            var downloader = (Downloader)sender;
            var buffer = e.Data;
            VideoStream.Write(buffer.Array, 0, e.Data.Count);
            //Application.Current.Dispatcher.Invoke(() => );
            VideoLoadedSize = downloader.DownloadedSize;

            if(!CanVideoPlay) {
                var fi = new FileInfo(VideoPath);
                if(fi.Length > VideoPlayLowestSize) {
                    AutoPlay(fi);
                }
            }
            Debug.WriteLine($"{e.Counter}: {e.Data.Count}, {VideoLoadedSize:#,###}/{VideoSize:#,###}");
            e.Cancel = IsDead;
        }

        private void Downloader_DownloadingError(object sender, Define.Event.DownloadingErrorEventArgs e)
        {
            const int retry = 5;

            e.Cancel = retry < e.Counter;
            Debug.WriteLine(e.Exception);

            if(e.Cancel) {
                VideoLoadState = LoadState.Failure;
            } else {
                var time = TimeSpan.FromMilliseconds(250);
                Thread.Sleep(time);
            }
        }





    }
}
