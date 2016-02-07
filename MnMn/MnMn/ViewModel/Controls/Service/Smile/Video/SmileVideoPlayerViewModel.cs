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
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using System.Windows.Media.Animation;
using System.Windows.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using System.Globalization;
using System.Windows.Media.Effects;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoPlayerViewModel: SmileVideoDownloadViewModel
    {
        #region

        class CommentData
        {
            public CommentData(SmileVideoCommentViewModel viewModel, AnimationTimeline animation)
            {
                ViewModel = viewModel;
                Animation = animation;
            }

            #region property

            public SmileVideoCommentViewModel ViewModel { get; }
            public AnimationTimeline Animation { get; }

            #endregion
        }

        #endregion

        #region variable

        bool _canVideoPlay;
        bool _isVideoPlayng;

        float _videoPosition;
        int _volume=100;

        TimeSpan _totalTime;
        TimeSpan _playTime;

        #endregion

        public SmileVideoPlayerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

        Mediation Mediation { get; set; }

        SmileVideoPlayerWindow View { get; set; }
        xZune.Vlc.Wpf.VlcPlayer Player { get; set; }
        //Slider VideoSilder { get; set; }
        Canvas NormalCommentArea { get; set; }
        Canvas ContributorCommentArea { get; set; }
        
        Navigationbar Navigationbar { get; set; }

        public CollectionModel<SmileVideoCommentViewModel> NormalCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
        public CollectionModel<SmileVideoCommentViewModel> ContributorCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();

        public bool ChangingVideoPosition { get; set; }
        public Point SeekbarMouseDownPosition { get; set; }
        public bool SeekbarThumbMoving { get; set; }

        bool IsDead { get; set; }
        long VideoPlayLowestSize => Constants.ServiceSmileVideoPlayLowestSize;

        List<CommentData> CommentBoxList { get; } = new List<CommentData>();

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

        public int Volume
        {
            get { return this._volume; }
            set { if(SetVariableValue(ref this._volume, value)) {
                    if(Player != null) {
                        Player.Volume = this._volume;
                    }
                }
            }
        }

        public TimeSpan PlayTime
        {
            get { return this._playTime; }
            set { SetVariableValue(ref this._playTime, value); }
        }
        public TimeSpan TotalTime
        {
            get { return this._totalTime; }
            set { SetVariableValue(ref this._totalTime, value); }
        }

        TimeSpan PrevTime { get; set; }

        #endregion

        #region function

        internal void SetView(SmileVideoPlayerWindow view)
        {
            View = view;
            Player = view.player;//.MediaPlayer;
            NormalCommentArea = view.normalCommentArea;
            ContributorCommentArea = view.contributorCommentArea;
            Navigationbar = view.seekbar;

            // 初期設定
            Player.Volume = Volume;

            // あれこれイベント
            View.Closed += View_Closed;
            Player.PositionChanged += Player_PositionChanged;
            Navigationbar.PreviewMouseDown += VideoSilder_PreviewMouseDown;
        }

        void AutoPlay(FileInfo fileInfo)
        {
            Player.LoadMedia(VideoFile.FullName);
            Player.Play();

            CanVideoPlay = true;
        }

        void MoveVideoPostion(float targetPosition)
        {
            float setPosition = targetPosition;

            if(targetPosition <= 0) {
                setPosition = 0;
            } else {
                var percentLoaded = (double)VideoLoadedSize / (double)VideoTotalSize;
                if(percentLoaded < targetPosition) {
                    setPosition = (float)percentLoaded;
                }
            }

            Player.Position = setPosition;
        }

        void FireShowComment()
        {
            Debug.WriteLine("{0} - {1}", PrevTime, PlayTime);
            
            foreach(var commentViewModel in NormalCommentList.Where(c => PrevTime <= c.ElapsedTime && c.ElapsedTime <= PlayTime).ToArray()) {
                var ft = new FormattedText(
                    commentViewModel.Content,
                    CultureInfo.GetCultureInfo(Constants.CurrentLanguageCode),
                    FlowDirection.LeftToRight,
                    new Typeface(Setting.FontFamily),
                    Setting.FontSize,
                    commentViewModel.Foreground
                );
                var geometry = ft.BuildGeometry(new Point());
                var drawing = new GeometryDrawing(null, new Pen(Brushes.Red, 2), geometry);
                var box = new Label();
                box.BeginInit();
                box.Foreground = commentViewModel.Foreground;
                box.FontFamily = new FontFamily(Setting.FontFamily);
                box.FontSize = Setting.FontSize;
                box.Opacity = Setting.FontAlpha;
                box.Content = commentViewModel.Content;
                box.Effect = new DropShadowEffect() {
                    Color = commentViewModel.GetShadowColor(commentViewModel.GetForeColor()),
                    Direction = 315,
                    BlurRadius = 2,
                    ShadowDepth = 2,
                    Opacity = 0.8,
                    RenderingBias = RenderingBias.Performance,
                };
                box.EndInit();

                NormalCommentArea.Children.Add(box);
                NormalCommentArea.UpdateLayout();

                Canvas.SetLeft(box, NormalCommentArea.ActualWidth);

                var animation = new DoubleAnimation();
                var data = new CommentData(commentViewModel, animation);
                CommentBoxList.Add(data);

                var starTime = commentViewModel.ElapsedTime.TotalMilliseconds - PrevTime.TotalMilliseconds;
                var diffPosition = starTime / NormalCommentArea.ActualWidth;

                animation.From = NormalCommentArea.ActualWidth + diffPosition;
                animation.To = - box.ActualWidth;
                animation.Duration = new Duration(Setting.ShowTime);

                EventDisposer< EventHandler> ev = null;
                animation.Completed += EventUtility.Create<EventHandler>((object sender, EventArgs e) => {
                    NormalCommentArea.Children.Remove(box);
                    CommentBoxList.Remove(data);
                    ev.Dispose();
                    box =  null;
                    ev = null;
                }, h => NormalCommentArea.Dispatcher.Invoke(() =>  animation.Completed -= h), out ev);

                box.BeginAnimation(Canvas.LeftProperty, animation);
            }
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
            var comments = rawMsgPacket.Chat.Select(c => new SmileVideoCommentViewModel(c, Setting)).OrderBy(c => c.ElapsedTime).ToArray();

            NormalCommentList.InitializeRange(comments.Where(c => !c.IsContributor));
            ContributorCommentList.InitializeRange(comments.Where(c => c.IsContributor));
            
            return base.LoadCommentAsync(rawMsgPacket);
        }

        protected override void OnLoadGetthumbinfoEnd()
        {
            TotalTime = VideoInformationViewModel.Length;
            base.OnLoadGetthumbinfoEnd();
        }

        #endregion

        private void View_Closed(object sender, EventArgs e)
        {
            Navigationbar.seekbar.PreviewMouseDown -= VideoSilder_PreviewMouseDown;
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
                PlayTime = Player.Time;
                FireShowComment();
                PrevTime = PlayTime;
            }
        }

        private void VideoSilder_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(!CanVideoPlay) {
                return;
            }

            ChangingVideoPosition = true;
            SeekbarMouseDownPosition = e.GetPosition(Navigationbar.seekbar);
            Navigationbar.seekbar.PreviewMouseUp += VideoSilder_PreviewMouseUp;
            Navigationbar.seekbar.MouseMove += Seekbar_MouseMove;
        }

        private void Seekbar_MouseMove(object sender, MouseEventArgs e)
        {
            SeekbarThumbMoving = true;
        }

        private void VideoSilder_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Debug.Assert(CanVideoPlay);

            Navigationbar.seekbar.PreviewMouseUp -= VideoSilder_PreviewMouseUp;
            Navigationbar.seekbar.MouseMove -= Seekbar_MouseMove;

            float nextPosition;

            if(!SeekbarThumbMoving) {
                var pos = e.GetPosition(Navigationbar.seekbar);
                nextPosition = (float)(pos.X / Navigationbar.seekbar.ActualWidth);
            } else {
                nextPosition = (float)Navigationbar.VideoPosition;
            }
            // TODO: 読み込んでない部分は移動不可にする
            MoveVideoPostion(nextPosition);

            ChangingVideoPosition = false;
            SeekbarThumbMoving = false;
        }
    }
}
