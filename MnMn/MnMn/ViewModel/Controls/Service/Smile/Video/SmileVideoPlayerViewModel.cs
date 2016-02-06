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
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using System.Windows.Media;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using System.Windows;
using ContentTypeTextNet.MnMn.MnMn.Define.Event;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoPlayerViewModel: SmileVideoDownloadViewModel
    {
        #region variable

        bool _canVideoPlay;
        bool _isVideoPlayng;

        float _videoPosition;

        #endregion

        public SmileVideoPlayerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

        Mediation Mediation { get; set; }

        SmileVideoPlayerWindow View { get; set; }
        xZune.Vlc.Wpf.VlcPlayer Player { get; set; }
        Slider VideoSilder { get; set; }
        Canvas CommentArea { get; set; }

        public CollectionModel<SmileVideoCommentViewModel> NormalCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
        public CollectionModel<SmileVideoCommentViewModel> ContributorCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();

        public bool ChangingVideoPosition { get; set; }
        bool IsDead { get; set; }
        long VideoPlayLowestSize => Constants.ServiceSmileVideoPlayLowestSize;

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
        
        public float VideoPosition
        {
            get { return this._videoPosition; }
            set { SetVariableValue(ref this._videoPosition, value); }
        }

        #endregion

        #region function

        internal void SetView(SmileVideoPlayerWindow view)
        {
            View = view;
            Player = view.player;//.MediaPlayer;
            VideoSilder = view.videoSilder;
            CommentArea = view.commentArea;

            // あれこれイベント
            View.Closed += View_Closed;
            Player.PositionChanged += Player_PositionChanged;
            VideoSilder.PreviewMouseDown += VideoSilder_PreviewMouseDown;
        }

        void AutoPlay(FileInfo fileInfo)
        {
            Player.LoadMedia(VideoFile.FullName);
            Player.Play();

            CanVideoPlay = true;
        }

        #endregion

        #region SmileVideoDownloadViewModel

        protected override void OnDownloading(object sender, DownloadingEventArgs e)
        {
            if(!CanVideoPlay) {
                // とりあえず待って
                var fi = new FileInfo(VideoFile.FullName);
                if(fi.Length > VideoPlayLowestSize) {
                    AutoPlay(fi);
                }
            }
            e.Cancel = IsDead;

            base.OnDownloading(sender, e);
        }

        protected override void OnLoadVideoEnd()
        {
            // あまりにも小さい場合は読み込み完了時にも再生できなくなっている
            if(!CanVideoPlay) {
                AutoPlay(VideoFile);
            }

            base.OnLoadVideoEnd();
        }

        protected override Task LoadCommentAsync(RawSmileVideoMsgPacketModel rawMsgPacket)
        {
            var comments = rawMsgPacket.Chat.Select(c => new SmileVideoCommentViewModel(c)).OrderBy(c => c.ElapsedTime).ToArray();

            NormalCommentList.InitializeRange(comments.Where(c => !c.IsContributor));
            ContributorCommentList.InitializeRange(comments.Where(c => c.IsContributor));
            
            return base.LoadCommentAsync(rawMsgPacket);
        }

        #endregion

        private void View_Closed(object sender, EventArgs e)
        {
            VideoSilder.PreviewMouseDown -= VideoSilder_PreviewMouseDown;
            Player.PositionChanged -= Player_PositionChanged;
            View.Closed -= View_Closed;

            IsDead = true;

            if(Player.State == xZune.Vlc.Interop.Media.MediaState.Playing) {
                Player.BeginStop();
            }
        }

        private void Player_PositionChanged(object sender, EventArgs e)
        {
            if(CanVideoPlay && !ChangingVideoPosition) {
                VideoPosition = Player.Position;
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
