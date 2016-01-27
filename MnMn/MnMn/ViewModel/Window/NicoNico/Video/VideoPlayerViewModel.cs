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

        #endregion

        public VideoPlayerViewModel(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; set; }

        public LoadState InformationLoadState
        {
            get { return this._informationLoadState; }
            set { SetVariableValue(ref this._informationLoadState, value); }
        }
        public LoadState ThumbnailLoadState
        {
            get { return this._thumbnailLoadState; }
            set { SetVariableValue(ref this._thumbnailLoadState, value); }
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

        public VideoInformationViewModel VideoInformationViewModel { get; set; }

        public Stream VideoStream
        {
            get { return this._videoStream; }
            set { SetVariableValue(ref this._videoStream, value); }
        }

        public MediaElement Player { get; private set; }


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
            // TODO: 細かな制御
            if(RawValueUtility.ConvertBoolean(rawVideoGetflvModel.Done)) {
                VideoLoadState = LoadState.Loading;

                using(var userAgent = session.CreateHttpUserAgent()) {
                    var ss = await userAgent.GetStringAsync(VideoInformationViewModel.WatchUrl);
                    //VideoStream = await userAgent.GetStreamAsync(rawVideoGetflvModel.MovieServerUrl);
                    //VlcContext.LibVlcDllsPath = CommonStrings.LIBVLC_DLLS_PATH_DEFAULT_VALUE_AMD64;

                    //  Player.MediaPlayer.Play(new Uri( rawVideoGetflvModel.MovieServerUrl));
                    //Player.MediaPlayer.Play(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));
                    //Player.Source = new Uri(rawVideoGetflvModel.MovieServerUrl);
                    userAgent.DefaultRequestHeaders.Referrer = VideoInformationViewModel.WatchUrl;
                    //var buffer = await userAgent.GetByteArrayAsync(rawVideoGetflvModel.MovieServerUrl);
                    using(var s = await userAgent.GetStreamAsync(rawVideoGetflvModel.MovieServerUrl)) {
                        var r = new BinaryReader(s);
                        using(var w = new BinaryWriter(new FileStream(@"z:\test.mp4", FileMode.Create))) {
                            byte[] buffer = new Byte[1024];
                            int bytesRead;

                            while((bytesRead = r.Read(buffer, 0, 1024)) > 0) {
                                w.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
            }
        }

        public async Task InitializeAsync(string videoId)
        {
            InformationLoadState = LoadState.Loading;
            var getthumbinfo = new Getthumbinfo(Mediation);
            var rawGetthumbinfoModel = await getthumbinfo.GetAsync(videoId);
            VideoInformationViewModel = new VideoInformationViewModel(Mediation, rawGetthumbinfoModel.Thumb, VideoInformationViewModel.NoOrderd);
            InformationLoadState = LoadState.Loaded;

            var noSessionTask = LoadNoSessionDataAsync();
            var sessionTask = LoadSessionDataAsync();

            //Task.WaitAll(noSessionTask, sessionTask);
            await noSessionTask;
            await sessionTask;
        }

        internal void SetPlayer(MediaElement player)
        {
            Player = player;
        }


        #endregion
    }
}
