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

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Window.NicoNico.Video
{
    public class VideoPlayerViewModel: ViewModelBase
    {
        #region variable

        LoadState _informationLoadState;
        LoadState _thumbnailLoadState;
        LoadState _commentLoadState;
        LoadState _videoLoadState;

        Stream _videoStream;

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

        public bool ChangingVideoPosition { get; set; }

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

        public NicoNicoVideoInformationViewModel VideoInformationViewModel { get; set; }

        public Stream VideoStream
        {
            get { return this._videoStream; }
            set { SetVariableValue(ref this._videoStream, value); }
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

        Task LoadNoSessionDataAsync()
        {
            ThumbnailLoadState = LoadState.Loading;
            return VideoInformationViewModel.LoadImageAsync().ContinueWith(task => {
                ThumbnailLoadState = LoadState.Loaded;
            });
        }

        async Task LoadSessionDataAsync()
        {
            var request = new RequestModel(RequestKind.Session, ServiceType.NicoNico);
            var response = Mediation.Request(request);
            var session = (NicoNicoSessionViewModel)response.Result;
            var getflv = new Getflv(Mediation, session);
            getflv.SessionSupport = true;
            var rawVideoGetflvModel = await getflv.GetAsync(VideoInformationViewModel.VideoId);

            long videoPlayLowestSize = Constants.ServiceNicoNicoVideoVideoPlayLowestSize;

            // TODO: 細かな制御と外部化
            if(RawValueUtility.ConvertBoolean(rawVideoGetflvModel.Done)) {
                VideoLoadState = LoadState.Loading;

                using(var userAgent = session.CreateHttpUserAgent()) {
                    var mem = new MemoryStream();
                    var task = Task.Run(async () => {
                        var downloadPath = @"z:\test.mp4";

                        CanVideoPlay = false;
                        var ss = await userAgent.GetStringAsync(VideoInformationViewModel.WatchUrl);
                        userAgent.DefaultRequestHeaders.Referrer = VideoInformationViewModel.WatchUrl;

                        using(var storageWriter = new BinaryWriter(new FileStream(downloadPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))) {
                            // TODO:エコノミー判定
                            VideoSize = VideoInformationViewModel.SizeLow;

                            var idx = 0;

                            using(var networkReader = new BinaryReader(await userAgent.GetStreamAsync(rawVideoGetflvModel.MovieServerUrl))) {
                                byte[] buffer = new byte[Constants.ServiceNicoNicoVideoVideoReceiveBuffer];

                                int bytesRead = 0;
                                while(true) {
                                    int n = 0;
                                    int max = 5;
                                    while(true) {
                                        try {
                                            bytesRead = networkReader.Read(buffer, 0, buffer.Length);
                                            break;
                                        } catch(IOException ex) {
                                            Debug.Write(ex);
                                            if(n++ < max) {
                                                continue;
                                            }
                                            VideoLoadState = LoadState.Failure;
                                            return;
                                        }
                                    }
                                    if(bytesRead <= 0) {
                                        break;
                                    }
                                    storageWriter.Write(buffer, 0, bytesRead);
                                    mem.Write(buffer, 0, bytesRead);
                                    if(!CanVideoPlay) {
                                        var fi = new FileInfo(downloadPath);
                                        if(fi.Length > videoPlayLowestSize) {
                                            await Task.Run(() => {
                                                Player.Play(new Uri(downloadPath));
                                            });
                                            CanVideoPlay = true;
                                        }
                                    }
                                    VideoLoadedSize += bytesRead;
                                    Debug.WriteLine("{0}: {1:#,###}  [{2:#,###}/{3:#,###}]", idx++, bytesRead, VideoLoadedSize, VideoSize);
                                }
                            }
                        }
                        VideoLoadState = LoadState.Loaded;
                        await Task.CompletedTask;
                    });
                    await task;
                }
            }
        }

        public async Task InitializeAsync(string videoId)
        {
            InformationLoadState = LoadState.Loading;
            var getthumbinfo = new Getthumbinfo(Mediation);
            var rawGetthumbinfoModel = await getthumbinfo.GetAsync(videoId);
            VideoInformationViewModel = new NicoNicoVideoInformationViewModel(Mediation, rawGetthumbinfoModel.Thumb, NicoNicoVideoInformationViewModel.NoOrderd);
            InformationLoadState = LoadState.Loaded;

            var noSessionTask = LoadNoSessionDataAsync();
            var sessionTask = LoadSessionDataAsync();

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

        #endregion

        private void View_Closed(object sender, EventArgs e)
        {
            VideoSilder.PreviewMouseDown -= VideoSilder_PreviewMouseDown;
            Player.PositionChanged -= Player_PositionChanged;
            View.Closed -= View_Closed;

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
    }
}
