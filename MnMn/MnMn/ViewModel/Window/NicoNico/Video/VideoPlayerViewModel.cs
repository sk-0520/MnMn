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

        public VlcControl Player { get; private set; }


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
            // TODO: 細かな制御と外部化
            if(RawValueUtility.ConvertBoolean(rawVideoGetflvModel.Done)) {
                VideoLoadState = LoadState.Loading;

                using(var userAgent = session.CreateHttpUserAgent()) {
                    var ss = await userAgent.GetByteArrayAsync(VideoInformationViewModel.WatchUrl);
                    userAgent.DefaultRequestHeaders.Referrer = VideoInformationViewModel.WatchUrl;
                    using(var networkReader = new BinaryReader(await userAgent.GetStreamAsync(rawVideoGetflvModel.MovieServerUrl))) {
                        var downloadPath = @"z:\test.mp4";
                        using(var storageWriter = new BinaryWriter(new FileStream(downloadPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))) {
                            VideoStream = new BufferedStream(new FileStream(downloadPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                            byte[] buffer = new byte[1024];
                            int bytesRead;
                            
                            VideoStream = new BufferedStream(new MemoryStream());
                            
                            while((bytesRead = networkReader.Read(buffer, 0, 1024)) > 0) {
                                storageWriter.Write(buffer, 0, bytesRead);
                                VideoStream.Write(buffer, 0, bytesRead);
                            }
                            VideoLoadState = LoadState.Loaded;

                            Player.MediaPlayer.SetMedia(new FileInfo(downloadPath));
                            Player.MediaPlayer.Play();
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

        internal void SetPlayer(VlcControl player)
        {
            Player = player;
        }


        #endregion
    }
}
