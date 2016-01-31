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

                    //var sock = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Raw);
                    //var stream = new NetworkStream(sock);
                    var ev = new EventWaitHandle(false, EventResetMode.AutoReset);
                    var ip = new IPEndPoint(IPAddress.Any, 12345);
                    //var sock = new Socket(SocketType.Stream, ProtocolType.IPv4);
                    var tcp = new TcpListener(ip);
                    
                    tcp.Start();
                    var mem = new MemoryStream();
                    var task = Task.Run(async () => {
                        ev.Set();
                        using(var cl = tcp.AcceptTcpClient()) {
                            cl.SendTimeout = (int)TimeSpan.MaxValue.TotalMilliseconds;
                            var downloadPath = @"z:\test.mp4";
                            var sss = cl.GetStream();

                            var aaa = new byte[8000];
                            var read = sss.Read(aaa, 0, aaa.Length);
                            var t = Encoding.UTF8.GetString(aaa.Take(read).ToArray());

                            var ss = await userAgent.GetStringAsync(VideoInformationViewModel.WatchUrl);
                            userAgent.DefaultRequestHeaders.Referrer = VideoInformationViewModel.WatchUrl;
                            using(var storageWriter = new BinaryWriter(new FileStream(downloadPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))) {

                                var rerr = new[] {
                                    "HTTP/1.1 200 OK",
                                    "Connection: close",
                                    "Content-Type: application/octet-stream",
                                    "Content-Length: " + VideoInformationViewModel.SizeHigh,
                                };
                                var res = string.Join("\r\n", rerr) + "\r\n\r\n";
                                var bbb = Encoding.UTF8.GetBytes(res);
                                sss.Write(bbb, 0, (int)bbb.Length);
                                var idx = 0;
                                using(var networkReader = new BinaryReader(await userAgent.GetStreamAsync(rawVideoGetflvModel.MovieServerUrl))) {
                                    byte[] buffer = new byte[1024 * 4];
                                    int bytesRead; while((bytesRead = networkReader.Read(buffer, 0, buffer.Length)) > 0) {
                                        storageWriter.Write(buffer, 0, bytesRead);
                                        mem.Write(buffer, 0, bytesRead);
                                        //VideoStream.Write(buffer, 0, bytesRead);
                                        //try {
                                            //sss.Write(buffer, 0, bytesRead);
                                            //sss.Flush();
                                            var a = Encoding.UTF8.GetBytes(bytesRead.ToString() + "\r\n");
                                            var b = Encoding.UTF8.GetBytes("\r\n\r\n");
                                            var m = new MemoryStream();

                                            m.Write(a, 0, a.Length);
                                            m.Write(buffer, 0, bytesRead);
                                            m.Write(b, 0, b.Length);

                                            var mb = m.GetBuffer();
                                            //sss.Write(mb, 0, (int)m.Length);

                                        Debug.WriteLine("{0}: {1}", idx++, bytesRead);

                                        //} catch(IOException ex) {
                                        //    Debug.WriteLine(ex);
                                        //}
                                    }
                                    //var oooo = Encoding.UTF8.GetBytes("0\r\n\r\n");
                                    //sss.Write(oooo, 0, oooo.Length);
                                    var aaaaaaa = mem.ToArray();
                                    sss.Write(aaaaaaa, 0, aaaaaaa.Length);
                                }

                                sss.Close();
                                cl.Close();
                            }
                        }
                    });
                    ev.WaitOne();
                    //var options = new[] {

                    //};
                    //Player.MediaPlayer.Play(new Uri("http://127.0.0.1:12345"), options);
                    Player.MediaPlayer.Play(new Uri("http://127.0.0.1:12345"));
                    //Player.MediaPlayer.Play(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));

                    /*
                    var ttt = new TcpClient("127.0.0.1", 12345);

                    var ccc = ttt.GetStream();
                    ccc.WriteByte(1);
                    var ddd = new MemoryStream();
                    var eee = new byte[8*1024];
                    int fff;
                    while((fff = ccc.Read(eee, 0, eee.Length)) > 0) {
                        ddd.Write(eee, 0, fff);
                    }
                    await task;

                    var pp = new FileStream(@"Z:\@@@.mp4", FileMode.Create);
                    ddd.Seek(0, SeekOrigin.Begin);
                    ddd.CopyTo(pp);
                    pp.Dispose();
                    //*/

                    await task;

                    //var uri = new Uri(rawVideoGetflvModel.MovieServerUrl);
                    //var param = new[] {
                    //    "--imem-cookie=" + session.GetSession(uri)
                    //};
                    //Player.MediaPlayer.Playing += MediaPlayer_Playing;
                    //Player.MediaPlayer.Playing += MediaPlayer_Playing;

                    //Player.MediaPlayer.Play(uri, param);
                    VideoLoadState = LoadState.Loaded;

                    //using(var networkReader = new BinaryReader(await userAgent.GetStreamAsync(rawVideoGetflvModel.MovieServerUrl))) {
                    //    var downloadPath = @"z:\test.mp4";
                    //    using(var storageWriter = new BinaryWriter(new FileStream(downloadPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))) {
                    //        VideoStream = new BufferedStream(new FileStream(downloadPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                    //        byte[] buffer = new byte[1024];
                    //        int bytesRead;

                    //        VideoStream = new BufferedStream(new MemoryStream());

                    //        while((bytesRead = networkReader.Read(buffer, 0, 1024)) > 0) {
                    //            storageWriter.Write(buffer, 0, bytesRead);
                    //            VideoStream.Write(buffer, 0, bytesRead);
                    //        }
                    //        VideoLoadState = LoadState.Loaded;

                    //        Player.MediaPlayer.SetMedia(new FileInfo(downloadPath));
                    //        Player.MediaPlayer.Play();
                    //    }
                    //}
                }
            }
        }

        private void MediaPlayer_Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            throw new NotImplementedException();
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
