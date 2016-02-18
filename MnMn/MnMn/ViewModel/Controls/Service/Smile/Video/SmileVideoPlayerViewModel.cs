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
using System.ComponentModel;
using System.Windows.Documents;
using HtmlAgilityPack;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoPlayerViewModel: SmileVideoDownloadViewModel
    {
        #region define

        static readonly Size BaseSize_4x3 = new Size(640, 480);
        static readonly Size BaseSize_16x9 = new Size(512, 384);

        class CommentData
        {
            public CommentData(FrameworkElement element, SmileVideoCommentViewModel viewModel, AnimationTimeline animation)
            {
                Element = element;
                ViewModel = viewModel;
                Animation = animation;
            }

            #region property

            public FrameworkElement Element { get; }
            public SmileVideoCommentViewModel ViewModel { get; }
            public AnimationTimeline Animation { get; }

            #endregion
        }

        #endregion

        #region variable

        bool _canVideoPlay;
        bool _isVideoPlayng;

        float _videoPosition;
        int _volume = 100;

        TimeSpan _totalTime;
        TimeSpan _playTime;

        SmileVideoCommentViewModel _selectedComment;

        LoadState _tagLoadState;

        double _realVideoWidth;
        double _realVideoHeight;
        double _baseWidth;
        double _baseHeight;

        #endregion

        public SmileVideoPlayerViewModel(Mediation mediation)
            : base(mediation)
        {
            CommentItems = CollectionViewSource.GetDefaultView(CommentList);
            CommentItems.Filter = FilterItems;
        }

        #region property

        Mediation Mediation { get; set; }

        SmileVideoPlayerWindow View { get; set; }
        xZune.Vlc.Wpf.VlcPlayer Player { get; set; }
        //Slider VideoSilder { get; set; }
        Canvas NormalCommentArea { get; set; }
        Canvas ContributorCommentArea { get; set; }
        ListView CommentView { get; set; }
        FlowDocumentScrollViewer DocumentDescription { get; set; }

        Navigationbar Navigationbar { get; set; }

        bool IsFirstPlay { get; set; } = true;

        public ICollectionView CommentItems { get; private set; }
        CollectionModel<SmileVideoCommentViewModel> CommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
        CollectionModel<SmileVideoCommentViewModel> NormalCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
        CollectionModel<SmileVideoCommentViewModel> ContributorCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();

        public CollectionModel<SmileVideoTagViewModel> TagItems { get; } = new CollectionModel<SmileVideoTagViewModel>();

        public bool ChangingVideoPosition { get; set; }
        public Point SeekbarMouseDownPosition { get; set; }
        public bool SeekbarThumbMoving { get; set; }

        bool IsDead { get; set; }
        long VideoPlayLowestSize => Constants.ServiceSmileVideoPlayLowestSize;

        List<CommentData> NormalCommentShowList { get; } = new List<CommentData>();

        public LoadState TagLoadState
        {
            get { return this._tagLoadState; }
            set { SetVariableValue(ref this._tagLoadState, value); }
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

        public float VideoPosition
        {
            get { return this._videoPosition; }
            set { SetVariableValue(ref this._videoPosition, value); }
        }

        public int Volume
        {
            get { return this._volume; }
            set
            {
                if(SetVariableValue(ref this._volume, value)) {
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

        public SmileVideoCommentViewModel SelectedComment
        {
            get { return this._selectedComment; }
            set { SetVariableValue(ref this._selectedComment, value); }
        }

        Size VisualVideoSize { get; set; }

        public double RealVideoWidth
        {
            get { return this._realVideoWidth; }
            set { SetVariableValue(ref this._realVideoWidth, value); }
        }
        public double RealVideoHeight
        {
            get { return this._realVideoHeight; }
            set { SetVariableValue(ref this._realVideoHeight, value); }
        }

        public double BaseWidth
        {
            get { return this._baseWidth; }
            set { SetVariableValue(ref this._baseWidth, value); }
        }
        public double BaseHeight
        {
            get { return this._baseHeight; }
            set { SetVariableValue(ref this._baseHeight, value); }
        }

        public string Description
        {
            get
            {
                if(VideoInformation?.PageHtmlLoadState == LoadState.Loaded) {
                    return VideoInformation.PageDescription;
                }

                return null;
            }
        }

        #endregion

        #region command

        public ICommand OpenLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {

                    }
                );
            }
        }

        #endregion

        #region function

        internal void SetView(SmileVideoPlayerWindow view)
        {
            View = view;
            Player = view.player;//.MediaPlayer;
            NormalCommentArea = view.normalCommentArea;
            ContributorCommentArea = view.contributorCommentArea;
            Navigationbar = view.seekbar;
            CommentView = view.commentView;
            DocumentDescription = view.documentDescription;

            // 初期設定
            Player.Volume = Volume;

            // あれこれイベント
            View.Loaded += View_Loaded;
            View.Closing += View_Closing;
            Player.PositionChanged += Player_PositionChanged;
            Player.SizeChanged += Player_SizeChanged;
            Navigationbar.PreviewMouseDown += VideoSilder_PreviewMouseDown;
        }

        void SetVideoDataInformation()
        {
            RealVideoWidth = Player.VlcMediaPlayer.PixelWidth;
            RealVideoHeight = Player.VlcMediaPlayer.PixelHeight;

            // コメントエリアのサイズ設定
            ChangeBaseSize();
        }

        void SetMedia()
        {
            Player.LoadMedia(VideoFile.FullName);
        }

        void AutoPlay(FileInfo fileInfo)
        {
            SetMedia();
            VideoPlay();
                CanVideoPlay = true;
        }

        void VideoPlay()
        {
            Player.Play();
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
            Debug.WriteLine($"{PrevTime} - {PlayTime}, {Player.ActualWidth}x{Player.ActualHeight}");

            var commentArea = new Size(
                NormalCommentArea.ActualWidth,
                NormalCommentArea.ActualHeight
            );
            var list = NormalCommentList.ToArray();
            foreach(var commentViewModel in list.Where(c => PrevTime <= c.ElapsedTime && c.ElapsedTime <= PlayTime).ToArray()) {
                var ft = new FormattedText(
                    commentViewModel.Content,
                    CultureInfo.GetCultureInfo(Constants.CurrentLanguageCode),
                    FlowDirection.LeftToRight,
                    new Typeface(Setting.FontFamily),
                    Setting.FontSize,
                    commentViewModel.Foreground
                );
                //var geometry = ft.BuildGeometry(new Point());
                //var drawing = new GeometryDrawing(null, new Pen(Brushes.Red, 2), geometry);
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

                // 今あるコメントから安全圏を走査
                var lastData = NormalCommentShowList
                    .Where(i => commentArea.Width < Canvas.GetLeft(i.Element) + i.Element.ActualWidth)
                    .OrderBy(i => Canvas.GetTop(i.Element))
                    .LastOrDefault()
                ;
                if(lastData != null) {
                    var nextY = Canvas.GetTop(lastData.Element) + lastData.Element.ActualHeight;
                    if(commentArea.Height < nextY + box.ActualHeight) {
                        Canvas.SetTop(box, 0);
                    } else {
                        Canvas.SetTop(box, nextY);
                    }
                } else {
                    Canvas.SetTop(box, 0);
                }


                var animation = new DoubleAnimation();
                var data = new CommentData(box, commentViewModel, animation);
                NormalCommentShowList.Add(data);

                var starTime = commentViewModel.ElapsedTime.TotalMilliseconds - PrevTime.TotalMilliseconds;
                var diffPosition = starTime / commentArea.Width;


                animation.From = commentArea.Width + diffPosition;
                animation.To = -box.ActualWidth;
                animation.Duration = new Duration(Setting.ShowTime);

                EventDisposer<EventHandler> ev = null;
                animation.Completed += EventUtility.Create<EventHandler>((object sender, EventArgs e) => {
                    NormalCommentArea.Children.Remove(box);
                    NormalCommentShowList.Remove(data);
                    ev.Dispose();
                    box = null;
                    ev = null;
                }, h => NormalCommentArea.Dispatcher.BeginInvoke(new Action(() => animation.Completed -= h)), out ev);

                box.BeginAnimation(Canvas.LeftProperty, animation);
            }
        }

        void ScrollComment()
        {
            var nowTimelineItem = CommentItems
                .Cast<SmileVideoCommentViewModel>()
                .FirstOrDefault(c => PrevTime <= c.ElapsedTime && c.ElapsedTime <= PlayTime)
            ;
            if(nowTimelineItem != null) {
                CommentView.ScrollIntoView(nowTimelineItem);
            }
        }

        bool FilterItems(object o)
        {
            var item = (SmileVideoCommentViewModel)o;

            return true;
        }

        Task LoadTagsAsync()
        {
            TagLoadState = LoadState.Preparation;

            TagItems.InitializeRange(VideoInformation.TagList);

            return Task.CompletedTask;
        }

        Inline CreateDescriptionInline(HtmlNode node)
        {
            switch(node.NodeType) {
                case HtmlNodeType.Text: {
                        var text = new Run(node.InnerText);
                        return text;
                    }

                case HtmlNodeType.Element:
                    if(node.Name == "br") {
                        return new LineBreak();
                    } else if(node.Name == "a") {
                        var text = new Run(node.InnerText);
                        var link = new Hyperlink(text);
                        link.Command = OpenLinkCommand;
                        link.CommandParameter = node.GetAttributeValue("href", string.Empty);
                        return link;
                    } else if(node.Name == "font") {
                        var text = new Run(node.InnerText);
                        var colorCode = node.GetAttributeValue("color", "#000000");
                        var color = (Color)ColorConverter.ConvertFromString(colorCode);
                        text.Foreground = new SolidColorBrush() {
                            Color = color,
                        };
                        return text;
                    } else {
                        var text = new Run("*" + node.OuterHtml + "*");
                        return text;
                    }
                    throw new NotImplementedException();

                default:
                    return new Run(string.Empty);
            }
        }

        IEnumerable<HtmlNode> ChompBreak(IEnumerable<HtmlNode> ndoes)
        {
            return ndoes
                .SkipWhile(n => n.NodeType == HtmlNodeType.Element && n.Name == "br")
                .Reverse()
                .SkipWhile(n => n.NodeType == HtmlNodeType.Element && n.Name == "br")
                .Reverse()
            ;
        }

        Paragraph CreateDescriptionParagraph(IEnumerable<HtmlNode> paragraphNodes)
        {
            var p = new Paragraph();

            foreach(var node in ChompBreak(paragraphNodes)) {
                var inline = CreateDescriptionInline(node);
                p.Inlines.Add(inline);
            }

            return p;
        }

        void MakeDescription()
        {
            DocumentDescription.Dispatcher.Invoke(() => {
                //var document = new FlowDocument();
                var document = DocumentDescription.Document;

                var html = new HtmlDocument() {
                    OptionAutoCloseOnEnd = true,
                };
                html.LoadHtml(VideoInformation.PageDescription);

                var nodeIndexList = ChompBreak(html.DocumentNode.ChildNodes.Cast<HtmlNode>()).Select((n,i) => new { Node = n, Index = i }).ToArray();
                var breakIndexList = nodeIndexList.Where(ni => ni.Node.NodeType == HtmlNodeType.Element && ni.Node.Name == "br").Select((n,i) => new { Node = n.Node, Index = n.Index, BreakIndex = i}).ToArray();
                var paragraphPointList = breakIndexList.Where(bi => bi.BreakIndex < breakIndexList.Length - 1 && bi.Node.NextSibling == breakIndexList[bi.BreakIndex + 1].Node).ToArray();
                if(paragraphPointList.Length > 1) {
                    var head = 0;
                    foreach(var point in paragraphPointList.Take(paragraphPointList.Length - 1)) {
                        var tail = point.Index;
                        var nodes = nodeIndexList.Skip(head).Take(tail- head);
                        var p = CreateDescriptionParagraph(nodes.Select(ni => ni.Node));
                        document.Blocks.Add(p);
                        head = tail + 1;
                    }
                } else {
                    var p = CreateDescriptionParagraph(nodeIndexList.Select(ni => ni.Node));
                    document.Blocks.Add(p);
                }

                document.FontSize = DocumentDescription.FontSize;
                document.FontFamily = DocumentDescription.FontFamily;
                document.FontStretch = DocumentDescription.FontStretch;
                
                //DocumentDescription.Document = document;
            });
        }

        void ChangeBaseSize()
        {
            if(RealVideoHeight <= 0 || RealVideoWidth <= 0) {
                BaseWidth = Player.ActualHeight;
                BaseHeight = Player.ActualWidth;
                return;
            } else if(IsFirstPlay) {
                var desktopScale = UIUtility.GetDpiScale(Player);
                VisualVideoSize = new Size(
                    RealVideoWidth * desktopScale.X,
                    RealVideoHeight * desktopScale.Y
                );
            }

            var scale = new Point(
                Player.ActualWidth / VisualVideoSize.Width,
                Player.ActualHeight / VisualVideoSize.Height
            );
            var baseScale = Math.Min(scale.X, scale.Y);

            BaseWidth = VisualVideoSize.Width * baseScale;
            BaseHeight = VisualVideoSize.Height * baseScale;
        }

        #endregion

        #region SmileVideoDownloadViewModel

        protected override void OnDownloadStart(object sender, DownloadStartEventArgs e)
        {
            MakeDescription();

            base.OnDownloadStart(sender, e);
        }

        protected override void OnDownloading(object sender, DownloadingEventArgs e)
        {
            if(!CanVideoPlay) {
                // とりあえず待って
                var fi = new FileInfo(VideoFile.FullName);
                CanVideoPlay = fi.Length > VideoPlayLowestSize;
                if(CanVideoPlay) {
                    AutoPlay(fi);
                }
            }
            e.Cancel = IsDead;

            base.OnDownloading(sender, e);
        }

        protected override void OnLoadVideoEnd()
        {
            if(VideoInformation.PageHtmlLoadState == LoadState.Loaded) {
                MakeDescription();
            }

            // あまりにも小さい場合は読み込み完了時にも再生できなくなっている
            if(!CanVideoPlay) {
                AutoPlay(VideoFile);
            }

            base.OnLoadVideoEnd();
        }

        protected override Task LoadCommentAsync(RawSmileVideoMsgPacketModel rawMsgPacket)
        {
            var comments = rawMsgPacket.Chat
                .GroupBy(c => c.No)
                .Select(c => new SmileVideoCommentViewModel(c.First(), Setting))
                .OrderBy(c => c.ElapsedTime)
            ;
            CommentList.InitializeRange(comments);

            NormalCommentList.InitializeRange(CommentList.Where(c => !c.IsContributor));
            ContributorCommentList.InitializeRange(CommentList.Where(c => c.IsContributor));

            return base.LoadCommentAsync(rawMsgPacket);
        }

        protected override void OnLoadGetthumbinfoEnd()
        {
            CallOnPropertyChange(nameof(Description));

            TotalTime = VideoInformation.Length;
            LoadTagsAsync();

            base.OnLoadGetthumbinfoEnd();
        }

        #endregion

        void View_Loaded(object sender, RoutedEventArgs e)
        {
            View.Loaded -= View_Loaded;
        }

        private void View_Closing(object sender, CancelEventArgs e)
        {
            View.Loaded -= View_Loaded;
            View.Closing -= View_Closing;
            Player.PositionChanged -= Player_PositionChanged;
            Player.SizeChanged -= Player_SizeChanged;
            Navigationbar.seekbar.PreviewMouseDown -= VideoSilder_PreviewMouseDown;

            IsDead = true;

            if(this.Player.State == xZune.Vlc.Interop.Media.MediaState.Playing) {
                this.Player.BeginStop();
            }

            //this.Player.Dispose();
        }

        private void Player_PositionChanged(object sender, EventArgs e)
        {
            if(CanVideoPlay && !ChangingVideoPosition) {
                if(IsFirstPlay) {
                    SetVideoDataInformation();
                    IsFirstPlay = false;
                }
                VideoPosition = Player.Position;
                PlayTime = Player.Time;
                FireShowComment();
                ScrollComment();
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

        private void Player_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeBaseSize();
        }
    }
}
