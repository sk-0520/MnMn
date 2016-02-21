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
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using xZune.Vlc.Wpf;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoPlayerViewModel: SmileVideoDownloadViewModel, ISetView
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
                Clock = Animation.CreateClock();
            }

            #region property

            public FrameworkElement Element { get; }
            public SmileVideoCommentViewModel ViewModel { get; }
            public AnimationTimeline Animation { get; }
            public AnimationClock Clock { get; }

            #endregion
        }

        #endregion

        #region variable

        bool _canVideoPlay;
        bool _isVideoPlayng;

        float _videoPosition;
        int _volume = 100;
        bool _isMute = false;

        TimeSpan _totalTime;
        TimeSpan _playTime;

        SmileVideoCommentViewModel _selectedComment;

        LoadState _tagLoadState;

        double _realVideoWidth;
        double _realVideoHeight;
        double _baseWidth;
        double _baseHeight;

        PlayerState _playerState;
        bool _isBuffering;

        bool _replayVideo;

        #endregion

        public SmileVideoPlayerViewModel(Mediation mediation)
            : base(mediation)
        {
            CommentItems = CollectionViewSource.GetDefaultView(CommentList);
            CommentItems.Filter = FilterItems;
        }

        #region property

        SmileVideoPlayerWindow View { get; set; }
        VlcPlayer Player { get; set; }
        Navigationbar Navigationbar { get; set; }
        Canvas NormalCommentArea { get; set; }
        Canvas ContributorCommentArea { get; set; }
        ListView CommentView { get; set; }
        FlowDocumentScrollViewer DocumentDescription { get; set; }

        /// <summary>
        /// 初回再生か。
        /// </summary>
        bool IsFirstPlay { get; set; } = true;

        bool UserOperationStop { get; set; } = false;

        /// <summary>
        /// 投降者コメントが構築されたか。
        /// </summary>
        bool IsMakedDescription { get; set; } = false;

        public ICollectionView CommentItems { get; private set; }
        CollectionModel<SmileVideoCommentViewModel> CommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
        CollectionModel<SmileVideoCommentViewModel> NormalCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
        CollectionModel<SmileVideoCommentViewModel> ContributorCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();

        List<CommentData> ShowingCommentList { get; } = new List<CommentData>();

        public CollectionModel<SmileVideoTagViewModel> TagItems { get; } = new CollectionModel<SmileVideoTagViewModel>();

        /// <summary>
        /// 動画再生位置を変更中か。
        /// </summary>
        public bool ChangingVideoPosition { get; set; }
        /// <summary>
        /// シークバー押下時のカーソル位置。
        /// </summary>
        public Point SeekbarMouseDownPosition { get; set; }
        /// <summary>
        /// シークバーの現在地が移動中か。
        /// </summary>
        public bool MovingSeekbarThumb { get; set; }
        /// <summary>
        /// ビューが閉じられたか。
        /// </summary>
        bool IsViewClosed { get; set; }
        long VideoPlayLowestSize => Constants.ServiceSmileVideoPlayLowestSize;


        public LoadState TagLoadState
        {
            get { return this._tagLoadState; }
            set { SetVariableValue(ref this._tagLoadState, value); }
        }

        /// <summary>
        /// 再生可能なサイズまでデータを読み込んだか。
        /// </summary>
        public bool CanVideoPlay
        {
            get { return this._canVideoPlay; }
            set { SetVariableValue(ref this._canVideoPlay, value); }
        }

        /// <summary>
        /// 動画再生中か。
        /// </summary>
        public bool IsVideoPlayng
        {
            get { return this._isVideoPlayng; }
            set { SetVariableValue(ref this._isVideoPlayng, value); }
        }

        /// <summary>
        /// 動画再生位置。
        /// </summary>
        public float VideoPosition
        {
            get { return this._videoPosition; }
            set { SetVariableValue(ref this._videoPosition, value); }
        }

        /// <summary>
        /// 動画音声。
        /// </summary>
        public int Volume
        {
            get { return this._volume; }
            set
            {
                if(SetVariableValue(ref this._volume, value)) {
                    Player.Volume = this._volume;
                }
            }
        }

        public bool IsMute
        {
            get { return this._isMute; }
            set
            {
                if(SetVariableValue(ref this._isMute, value)) {
                    Player.IsMute = this._isMute;
                }
            }
        }

        /// <summary>
        /// 動画の再生中時間。
        /// </summary>
        public TimeSpan PlayTime
        {
            get { return this._playTime; }
            set { SetVariableValue(ref this._playTime, value); }
        }

        /// <summary>
        /// 動画の長さ。
        /// </summary>
        public TimeSpan TotalTime
        {
            get { return this._totalTime; }
            set { SetVariableValue(ref this._totalTime, value); }
        }

        /// <summary>
        /// 前回再生時間。
        /// </summary>
        TimeSpan PrevPlayedTime { get; set; }

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

        //public string Description
        //{
        //    get
        //    {
        //        if(VideoInformation?.PageHtmlLoadState == LoadState.Loaded) {
        //            return VideoInformation.PageDescription;
        //        }

        //        return null;
        //    }
        //}

        public PlayerState PlayerState
        {
            get { return this._playerState; }
            set { SetVariableValue(ref this._playerState, value); }
        }

        public bool IsBuffering
        {
            get { return this._isBuffering; }
            set { SetVariableValue(ref this._isBuffering, value); }
        }
        float BufferingVideoPosition { get; set; }

        public bool ReplayVideo
        {
            get { return this._replayVideo; }
            set { SetVariableValue(ref this._replayVideo, value); }
        }

        public string UserNickname
        {
            get { return VideoInformation.UserNickname; }
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

        public ICommand PlayCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        switch(PlayerState) {
                            case PlayerState.Stop:
                                Player.Play();
                                return;

                            case PlayerState.Playing:
                                Player.PauseOrResume();
                                return;

                            case PlayerState.Pause:
                                if(IsBuffering) {
                                    SetMedia();
                                    Player.Position = VideoPosition;
                                    VideoPlay();
                                } else {
                                    Player.PauseOrResume();
                                }
                                return;

                            default:
                                break;
                        }
                    }
                );
            }
        }

        public ICommand StopCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        UserOperationStop = true;
                        Player.BeginStop(new Action(() => {
                            UserOperationStop = false;
                        }));
                    }
                );
            }
        }

        public ICommand SearchTagCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var tagViewModel = (SmileVideoTagViewModel)o;

                        //var searchItemViewModel = new SmileVideoSearchGroupViewModel(Mediation, )
                        //tagViewModel.TagName
                        var searchSettingResponce = Mediation.Request(new RequestModel(RequestKind.SearchSetting, ServiceType.SmileVideo));
                        var searchSettingResult = (SmileVideoSearchSettingResultModel)searchSettingResponce.Result;

                        var searchDefineResponce = Mediation.Request(new RequestModel(RequestKind.SearchDefine, ServiceType.SmileVideo));
                        var searchDefineResult = (SmileVideoSearchModel)searchDefineResponce.Result;

                        var serchViewModel = new SmileVideoSearchGroupViewModel(Mediation, searchDefineResult, searchSettingResult.Method, searchSettingResult.Sort, searchDefineResult.GetTagTypeElement(), tagViewModel.TagName);
                        Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, serchViewModel, ShowViewState.Foreground));
                    }
                );
            }
        }

        public ICommand OpenCacheDirectoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(VideoInformation.CacheDirectory.Exists) {
                            Process.Start(VideoInformation.CacheDirectory.FullName);
                        }
                    }
                );
            }
        }

        #endregion

        #region function

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

        void StartIfAutoPlay()
        {
            if(Setting.AutoPlay) {
                SetMedia();
                VideoPlay();
            }
        }

        void VideoPlay()
        {
            Player.Play();
        }

        void ClearComment()
        {
            foreach(var data in ShowingCommentList) {
                data.Clock.Controller.SkipToFill();
                data.Clock.Controller.Remove();
            }
            ShowingCommentList.Clear();
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
            ClearComment();

            Player.Position = setPosition;
            PrevPlayedTime = Player.Time;
        }

        static FrameworkElement CreateCommentElement(SmileVideoCommentViewModel commentViewModel, Size commentArea, SmileVideoSettingModel setting)
        {
            var element = new Label();
            using(Initializer.BeginInitialize(element)) {
                element.Foreground = commentViewModel.Foreground;
                element.FontFamily = new FontFamily(setting.FontFamily);
                element.FontSize = commentViewModel.FontSize;
                element.Opacity = setting.FontAlpha;
                element.Content = commentViewModel.Content;
                element.Effect = new DropShadowEffect() {
                    Color = commentViewModel.Shadow,
                    Direction = 315,
                    BlurRadius = 2,
                    ShadowDepth = 2,
                    Opacity = 0.8,
                    RenderingBias = RenderingBias.Performance,
                };
            }

            if(commentViewModel.Vertical != SmileVideoCommentVertical.Normal) {
                element.Width = commentArea.Width;
                element.HorizontalContentAlignment = HorizontalAlignment.Center;
            }

            return element;
        }

        static void SetMarqueeCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, List<CommentData> showingCommentList, SmileVideoSettingModel setting)
        {
            // 今あるコメントから安全圏を走査
            var nowData = showingCommentList
                .Where(i => i.ViewModel.Vertical == SmileVideoCommentVertical.Normal)
                .Where(i => commentArea.Width < Canvas.GetLeft(i.Element) + i.Element.ActualWidth)
                .OrderBy(i => Canvas.GetTop(i.Element))
                .ToArray()
            ;
            var lastData = nowData.LastOrDefault();
            if(lastData != null) {
                var nextY = Canvas.GetTop(lastData.Element) + lastData.Element.ActualHeight;
                if(commentArea.Height < nextY + element.ActualHeight) {
                    double usingY = -1;
                    // これ以上下げられない場合は現在の表示されている中で一番使用されてなさそうな部分に放り込む
                    var lineData = nowData
                        .GroupBy(i => Canvas.GetTop(i.Element))
                        .OrderBy(line => line.Key)
                    ;
                    // 自身の高さの倍数で見ていく
                    var myHeight = element.ActualHeight;
                    for(var y = 0.0; y < commentArea.Height - myHeight; y += myHeight) {
                        var line = lineData.FirstOrDefault(ls => ls.Key == y);
                        if(line == null) {
                            // 他のデータなさそうなので入れる。
                            usingY = y;
                            break;
                        }
                    }
                    if(usingY == -1) {
                        usingY = lineData
                            .OrderBy(line => line.Count())
                            .First()
                            .Key
                        ;
                    }

                    Debug.WriteLine(usingY);
                    Canvas.SetTop(element, usingY);
                } else {
                    Canvas.SetTop(element, nextY);
                }
            } else {
                Canvas.SetTop(element, 0);
            }
            Canvas.SetLeft(element, commentArea.Width);
        }

        /// <summary>
        /// TODO: top,bottomは共存できる。はず。
        /// </summary>
        /// <param name="commentViewModel"></param>
        /// <param name="element"></param>
        /// <param name="commentArea"></param>
        /// <param name="showingCommentList"></param>
        /// <param name="setting"></param>
        static void SetTopCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, List<CommentData> showingCommentList, SmileVideoSettingModel setting)
        {
            // 今あるコメントから安全圏を走査
            var nowData = showingCommentList
                .Where(i => i.ViewModel.Vertical == commentViewModel.Vertical)
                        .GroupBy(i => Canvas.GetTop(i.Element))
                        .OrderBy(line => line.Key)
                .ToArray()
            ;
            double usingY = -1;
            var myHeight = element.ActualHeight;
            for(var y = 0.0; y < commentArea.Height - myHeight; y += myHeight) {
                var line = nowData.FirstOrDefault(ls => ls.Key == y);
                if(line == null) {
                    // 他のデータなさそうなので入れる。
                    usingY = y;
                    break;
                }
            }
            if(usingY == -1) {
                usingY = nowData
                    .OrderBy(line => line.Count())
                    .First()
                    .Key
                ;
            }

            Canvas.SetTop(element, usingY);
            Canvas.SetLeft(element, 0);

            CastUtility.AsAction<Label>(element, label => {
                label.Width = commentArea.Width;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentViewModel"></param>
        /// <param name="element"></param>
        /// <param name="commentArea"></param>
        /// <param name="showingCommentList"></param>
        /// <param name="setting"></param>
        static void SetBottomCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, List<CommentData> showingCommentList, SmileVideoSettingModel setting)
        {
            // 今あるコメントから安全圏を走査
            var nowData = showingCommentList
                .Where(i => i.ViewModel.Vertical == commentViewModel.Vertical)
                .GroupBy(i => Canvas.GetTop(i.Element))
                .OrderByDescending(i => i.Key)
                .ToArray()
            ;
            double usingY = -1;

            var myHeight = element.ActualHeight;
            for(var y = commentArea.Height - myHeight; 0 < y; y -= myHeight) {
                var line = nowData.FirstOrDefault(ls => ls.Key == y);
                if(line == null) {
                    // 他のデータなさそうなので入れる。
                    usingY = y;
                    break;
                }
            }
            if(usingY == -1) {
                usingY = nowData
                    .OrderByDescending(line => line.Count())
                    .First()
                    .Key
                ;
            }

            Canvas.SetTop(element, usingY);
            Canvas.SetLeft(element, 0);

            CastUtility.AsAction<Label>(element, label => {
                label.Width = commentArea.Width;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
            });

        }

        static void SetCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, List<CommentData> showingCommentList, SmileVideoSettingModel setting)
        {
            switch(commentViewModel.Vertical) {
                case SmileVideoCommentVertical.Normal:
                    SetMarqueeCommentPosition(commentViewModel, element, commentArea, showingCommentList, setting);
                    break;

                case SmileVideoCommentVertical.Top:
                    SetTopCommentPosition(commentViewModel, element, commentArea, showingCommentList, setting);
                    break;

                case SmileVideoCommentVertical.Bottom:
                    SetBottomCommentPosition(commentViewModel, element, commentArea, showingCommentList, setting);
                    break;

                default:
                    break;
            }
        }

        static AnimationTimeline CreateMarqueeCommentAnimeation(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, TimeSpan prevTime, TimeSpan showTime)
        {
            var animation = new DoubleAnimation();
            var starTime = commentViewModel.ElapsedTime.TotalMilliseconds - prevTime.TotalMilliseconds;
            var diffPosition = starTime / commentArea.Width;

            animation.From = commentArea.Width + diffPosition;
            animation.To = -element.ActualWidth;
            animation.Duration = new Duration(showTime);

            return animation;
        }

        static AnimationTimeline CreateTopBottomCommentAnimeation(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, TimeSpan prevTime, TimeSpan showTime)
        {
            var animation = new DoubleAnimation();
            //var starTime = commentViewModel.ElapsedTime.TotalMilliseconds - prevTime.TotalMilliseconds;
            //var diffPosition = starTime / commentArea.Width;

            // アニメーションさせる必要ないけど停止や移動なんかを考えるとIFとしてアニメーションの方が楽
            animation.From = animation.To = 0;
            animation.Duration = new Duration(showTime);

            return animation;
        }


        static AnimationTimeline CreateCommentAnimeation(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, TimeSpan prevTime, TimeSpan showTime)
        {
            switch(commentViewModel.Vertical) {
                case SmileVideoCommentVertical.Normal:
                    return CreateMarqueeCommentAnimeation(commentViewModel, element, commentArea, prevTime, showTime);

                case SmileVideoCommentVertical.Top:
                case SmileVideoCommentVertical.Bottom:
                    return CreateTopBottomCommentAnimeation(commentViewModel, element, commentArea, prevTime, showTime);

                default:
                    throw new NotImplementedException();
            }
        }

        static void FireShowComment_Impl(Canvas commentParentElement, TimeSpan prevTime, TimeSpan nowTime, IList<SmileVideoCommentViewModel> commentViewModelList, List<CommentData> showingCommentList, SmileVideoSettingModel setting)
        {
            var commentArea = new Size(
               commentParentElement.ActualWidth,
               commentParentElement.ActualHeight
            );

            var list = commentViewModelList.ToArray();
            // 現在時間から-1秒したものを表示対象とする
            var correctionTime = TimeSpan.FromSeconds(1);
            foreach(var commentViewModel in list.Where(c => prevTime <= (c.ElapsedTime - correctionTime) && (c.ElapsedTime - correctionTime) <= nowTime).ToArray()) {

                var element = CreateCommentElement(commentViewModel, commentArea, setting);

                commentViewModel.NowShowing = true;

                commentParentElement.Children.Add(element);
                commentParentElement.UpdateLayout();

                SetCommentPosition(commentViewModel, element, commentArea, showingCommentList, setting);

                // アニメーション設定
                var animation = CreateCommentAnimeation(commentViewModel, element, commentArea, prevTime - correctionTime, setting.ShowTime + correctionTime);

                var data = new CommentData(element, commentViewModel, animation);
                showingCommentList.Add(data);

                EventDisposer<EventHandler> ev = null;
                data.Clock.Completed += EventUtility.Create<EventHandler>((object sender, EventArgs e) => {
                    commentParentElement.Children.Remove(element);
                    showingCommentList.Remove(data);
                    data.ViewModel.NowShowing = false;
                    ev.Dispose();
                    element = null;
                    ev = null;
                }, h => commentParentElement.Dispatcher.BeginInvoke(new Action(() => animation.Completed -= h)), out ev);

                element.ApplyAnimationClock(Canvas.LeftProperty, data.Clock);
            }
        }

        void FireShowComment()
        {
            Debug.WriteLine($"{PrevPlayedTime} - {PlayTime}, {Player.ActualWidth}x{Player.ActualHeight}");

            FireShowComment_Impl(NormalCommentArea, PrevPlayedTime, PlayTime, NormalCommentList, ShowingCommentList, Setting);
            FireShowComment_Impl(ContributorCommentArea, PrevPlayedTime, PlayTime, ContributorCommentList, ShowingCommentList, Setting);
        }

        void ScrollCommentList()
        {
            var nowTimelineItem = CommentItems
                .Cast<SmileVideoCommentViewModel>()
                .FirstOrDefault(c => PrevPlayedTime <= c.ElapsedTime && c.ElapsedTime <= PlayTime)
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

            TagLoadState = LoadState.Loaded;

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
            IsMakedDescription = true;

            DocumentDescription.Dispatcher.Invoke(() => {
                //var document = new FlowDocument();
                var document = DocumentDescription.Document;

                var html = new HtmlDocument() {
                    OptionAutoCloseOnEnd = true,
                };
                html.LoadHtml(VideoInformation.PageDescription);

                var nodeIndexList = ChompBreak(html.DocumentNode.ChildNodes.Cast<HtmlNode>()).Select((n, i) => new { Node = n, Index = i }).ToArray();
                var breakIndexList = nodeIndexList.Where(ni => ni.Node.NodeType == HtmlNodeType.Element && ni.Node.Name == "br").Select((n, i) => new { Node = n.Node, Index = n.Index, BreakIndex = i }).ToArray();
                var paragraphPointList = breakIndexList.Where(bi => bi.BreakIndex < breakIndexList.Length - 1 && bi.Node.NextSibling == breakIndexList[bi.BreakIndex + 1].Node).ToArray();
                if(paragraphPointList.Length > 1) {
                    var head = 0;
                    foreach(var point in paragraphPointList.Take(paragraphPointList.Length - 1)) {
                        var tail = point.Index;
                        var nodes = nodeIndexList.Skip(head).Take(tail - head);
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
            if(!IsMakedDescription) {
                MakeDescription();
            }

            base.OnDownloadStart(sender, e);
        }

        protected override void OnDownloading(object sender, DownloadingEventArgs e)
        {
            if(!CanVideoPlay) {
                // とりあえず待って
                VideoFile.Refresh();
                CanVideoPlay = VideoFile.Length > VideoPlayLowestSize;
                if(CanVideoPlay) {
                    StartIfAutoPlay();
                }
            }
            e.Cancel = IsViewClosed;

            base.OnDownloading(sender, e);
        }

        protected override void OnLoadVideoEnd()
        {
            if(!IsMakedDescription && VideoInformation.PageHtmlLoadState == LoadState.Loaded) {
                MakeDescription();
            }

            // あまりにも小さい場合は読み込み完了時にも再生できなくなっている
            if(!CanVideoPlay) {
                CanVideoPlay = true;
                StartIfAutoPlay();
            }

            base.OnLoadVideoEnd();
        }

        protected override Task LoadCommentAsync(RawSmileVideoMsgPacketModel rawMsgPacket)
        {
            var comments = rawMsgPacket.Chat
                .GroupBy(c => new { c.No, c.Fork })
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
            TotalTime = VideoInformation.Length;
            LoadTagsAsync();

            base.OnLoadGetthumbinfoEnd();
        }

        #endregion

        #region ISetView

        public void SetView(FrameworkElement view)
        {
            var playerView = (SmileVideoPlayerWindow)view;

            View = playerView;
            Player = playerView.player;//.MediaPlayer;
            NormalCommentArea = playerView.normalCommentArea;
            ContributorCommentArea = playerView.contributorCommentArea;
            Navigationbar = playerView.seekbar;
            CommentView = playerView.commentView;
            DocumentDescription = playerView.documentDescription;

            // 初期設定
            Player.Volume = Volume;

            // あれこれイベント
            View.Loaded += View_Loaded;
            View.Closing += View_Closing;
            Player.PositionChanged += Player_PositionChanged;
            Player.SizeChanged += Player_SizeChanged;
            Player.StateChanged += Player_StateChanged;
            Navigationbar.seekbar.PreviewMouseDown += VideoSilder_PreviewMouseDown;
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
            Player.StateChanged -= Player_StateChanged;
            Navigationbar.seekbar.PreviewMouseDown -= VideoSilder_PreviewMouseDown;

            IsViewClosed = true;

            if(Player.State == xZune.Vlc.Interop.Media.MediaState.Playing) {
                Player.BeginStop();
            }
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
                ScrollCommentList();
                PrevPlayedTime = PlayTime;
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
            MovingSeekbarThumb = true;
        }

        private void VideoSilder_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Debug.Assert(CanVideoPlay);

            Navigationbar.seekbar.PreviewMouseUp -= VideoSilder_PreviewMouseUp;
            Navigationbar.seekbar.MouseMove -= Seekbar_MouseMove;

            float nextPosition;

            if(!MovingSeekbarThumb) {
                var pos = e.GetPosition(Navigationbar.seekbar);
                nextPosition = (float)(pos.X / Navigationbar.seekbar.ActualWidth);
            } else {
                nextPosition = (float)Navigationbar.VideoPosition;
            }
            // TODO: 読み込んでない部分は移動不可にする
            MoveVideoPostion(nextPosition);

            ChangingVideoPosition = false;
            MovingSeekbarThumb = false;
        }

        private void Player_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeBaseSize();
        }

        private void Player_StateChanged(object sender, xZune.Vlc.ObjectEventArgs<xZune.Vlc.Interop.Media.MediaState> e)
        {
            // 開いてる最中は気にしない
            if(e.Value == xZune.Vlc.Interop.Media.MediaState.Opening) {
                return;
            }

            Debug.WriteLine(e.Value);
            switch(e.Value) {
                case xZune.Vlc.Interop.Media.MediaState.Playing:
                    var prevState = PlayerState;
                    PlayerState = PlayerState.Playing;
                    if(prevState == PlayerState.Pause) {
                        foreach(var data in ShowingCommentList) {
                            data.Clock.Controller.Resume();
                        }
                    }
                    break;

                case xZune.Vlc.Interop.Media.MediaState.Stopped:
                    if(IsBuffering) {
                        Debug.WriteLine("buffering wait");
                        PlayerState = PlayerState.Pause;
                        Player.Position = BufferingVideoPosition;
                        foreach(var data in ShowingCommentList) {
                            data.Clock.Controller.Pause();
                        }
                    } else {
                        if(ReplayVideo && !UserOperationStop) {
                            Debug.WriteLine("replay");
                            VideoPosition = 0;
                            PrevPlayedTime = TimeSpan.Zero;
                            ClearComment();
                            Player.Play();
                        } else {
                            Debug.WriteLine("stop");
                            PlayerState = PlayerState.Stop;
                            VideoPosition = 0;
                            PrevPlayedTime = TimeSpan.Zero;
                        }
                    }
                    break;

                case xZune.Vlc.Interop.Media.MediaState.Paused:
                    PlayerState = PlayerState.Pause;
                    foreach(var data in ShowingCommentList) {
                        data.Clock.Controller.Pause();
                    }
                    break;

                case xZune.Vlc.Interop.Media.MediaState.Ended:
                    if(PlayTime != PrevPlayedTime) {
                        // 終わってない
                        IsBuffering = true;
                        BufferingVideoPosition = VideoPosition;
                    }
                    break;

                default:
                    break;
            }

        }

    }
}
