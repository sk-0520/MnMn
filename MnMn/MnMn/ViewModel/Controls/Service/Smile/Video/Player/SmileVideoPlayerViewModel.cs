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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
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
using Meta.Vlc.Wpf;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using HTMLConverter;
using System.Windows.Markup;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video.Parameter;
using Package.stackoverflow.com;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;
using System.Xml;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Logic.Wrapper;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    public class SmileVideoPlayerViewModel: SmileVideoDownloadViewModel, ISetView, ISmileVideoDescription
    {
        #region define

        static readonly Size BaseSize_4x3 = new Size(640, 480);
        static readonly Size BaseSize_16x9 = new Size(512, 384);

        const float initPrevStateChangedPosition = -1;

        static readonly TimeSpan correctionTime = TimeSpan.FromSeconds(1);

        #endregion

        #region variable

        bool _canVideoPlay;
        bool _isVideoPlayng;

        float _videoPosition;
        float _prevStateChangedPosition;

        WindowState _state = WindowState.Normal;

        TimeSpan _totalTime;
        TimeSpan _playTime;

        SmileVideoCommentViewModel _selectedComment;

        LoadState _relationVideoLoadState;

        double _realVideoWidth;
        double _realVideoHeight;
        double _baseWidth;
        double _baseHeight;
        double _commentAreaWidth = 640;
        double _commentAreaHeight = 386;

        [Obsolete]
        GridLength _commentListLength = new GridLength(3, GridUnitType.Star);
        [Obsolete]
        GridLength _visualPlayerWidth = new GridLength(7, GridUnitType.Star);
        [Obsolete]
        GridLength _visualPlayerHeight = new GridLength(1, GridUnitType.Star);

        PlayerState _playerState;
        bool _isBufferingStop;

        IReadOnlyList<SmileVideoAccountMyListFinderViewModel> _accountMyListItems;

        SmileVideoFilteringCommentType _filteringCommentType;
        string _filteringUserId;
        int _commentListCount;
        int _originalPosterCommentListCount;

        bool _isSelectedComment;

        bool _isSelectedVideoInformation;

        #endregion

        public SmileVideoPlayerViewModel(Mediation mediation)
            : base(mediation)
        {
            CommentItems = CollectionViewSource.GetDefaultView(CommentList);
            CommentItems.Filter = FilterCommentItems;

            var filteringResult = Mediation.GetResultFromRequest<SmileVideoCommentFilteringResultModel>(new SmileVideoCustomSettingRequestModel(SmileVideoCustomSettingKind.CommentFiltering));
            GlobalCommentFilering = filteringResult.Filtering;

        }

        #region property

        SmileVideoPlayerWindow View { get; set; }
        VlcPlayer Player { get; set; }
        Navigationbar Navigationbar { get; set; }
        Canvas NormalCommentArea { get; set; }
        Canvas OriginalPosterCommentArea { get; set; }
        ListView CommentView { get; set; }
        FlowDocumentScrollViewer DocumentDescription { get; set; }

        public string Title { get { return $"SmileVideo[{VideoId}]: {VideoInformation.Title}"; } }

        public WindowState State
        {
            get { return this._state; }
            set { SetVariableValue(ref this._state, value); }
        }
        public double Left
        {
            get { return Setting.Player.Window.Left; }
            set
            {
                if(State == WindowState.Normal) {
                    SetPropertyValue(Setting.Player.Window, value, nameof(Setting.Player.Window.Left));
                }
            }
        }
        public double Top
        {
            get { return Setting.Player.Window.Top; }
            set
            {
                if(State == System.Windows.WindowState.Normal) {
                    SetPropertyValue(Setting.Player.Window, value, nameof(Setting.Player.Window.Top));
                }
            }
        }
        public double Width
        {
            get { return Setting.Player.Window.Width; }
            set
            {
                if(State == WindowState.Normal) {
                    SetPropertyValue(Setting.Player.Window, value, nameof(Setting.Player.Window.Width));
                }
            }
        }
        public double Height
        {
            get { return Setting.Player.Window.Height; }
            set
            {
                if(State == System.Windows.WindowState.Normal) {
                    SetPropertyValue(Setting.Player.Window, value, nameof(Setting.Player.Window.Height));
                }
            }
        }

        public bool PlayerShowDetailArea
        {
            get { return Setting.Player.ShowDetailArea; }
            set { SetPropertyValue(Setting.Player, value, nameof(Setting.Player.ShowDetailArea)); }
        }

        public bool PlayerShowCommentArea
        {
            get { return Setting.Player.ShowCommentList; }
            set { SetPropertyValue(Setting.Player, value, nameof(Setting.Player.ShowCommentList)); }
        }

        public bool PlayerVisibleComment
        {
            get { return Setting.Player.VisibleComment; }
            set { SetPropertyValue(Setting.Player, value, nameof(Setting.Player.VisibleComment)); }
        }

        public bool IsAutoScroll
        {
            get { return Setting.Player.AutoScrollCommentList; }
            set { SetPropertyValue(Setting.Player, value, nameof(Setting.Player.AutoScrollCommentList)); }
        }

        public bool IsEnabledSharedNoGood
        {
            get { return Setting.Comment.IsEnabledSharedNoGood; }
            set
            {
                if(SetPropertyValue(Setting.Comment, value, nameof(Setting.Comment.IsEnabledSharedNoGood))) {
                    ApprovalComment();
                }
            }
        }
        public int SharedNoGoodScore
        {
            get { return Setting.Comment.SharedNoGoodScore; }
            set {
                if(SetPropertyValue(Setting.Comment, value, nameof(Setting.Comment.SharedNoGoodScore))) {
                    ApprovalComment();
                }
            }
        }

        public SmileVideoFilteringCommentType FilteringCommentType
        {
            get { return this._filteringCommentType; }
            set
            {
                if(SetVariableValue(ref this._filteringCommentType, value)) {
                    if(this._filteringCommentType == SmileVideoFilteringCommentType.All) {
                        CommentItems.Refresh();
                    } else {
                        RefreshFilteringComment();
                    }
                }
            }
        }

        public string FilteringUserId
        {
            get { return this._filteringUserId; }
            set
            {
                if(SetVariableValue(ref this._filteringUserId, value)) {
                    if(FilteringCommentType == SmileVideoFilteringCommentType.UserId) {
                        RefreshFilteringComment();
                    }
                }
            }
        }

        public CollectionModel<FilteringUserViewModel> FilteringUserList { get; } = new CollectionModel<FilteringUserViewModel>();

        public int CommentListCount
        {
            get { return this._commentListCount; }
            private set { SetVariableValue(ref this._commentListCount, value); }
        }
        public int OriginalPosterCommentListCount
        {
            get { return this._originalPosterCommentListCount; }
            private set { SetVariableValue(ref this._originalPosterCommentListCount, value); }
        }



        public string VideoId
        {
            get { return VideoInformation.VideoId; }
        }

        /// <summary>
        /// 初回再生か。
        /// </summary>
        bool IsFirstPlay { get; set; } = true;

        bool UserOperationStop { get; set; } = false;

        /// <summary>
        /// 投降者コメントが構築されたか。
        /// </summary>
        bool IsMakedDescription { get; set; } = false;
        /// <summary>
        ///
        /// </summary>
        bool IsCheckedTagPedia { get; set; } = false;

        public ICollectionView CommentItems { get; private set; }
        CollectionModel<SmileVideoCommentViewModel> CommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
        CollectionModel<SmileVideoCommentViewModel> NormalCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
        CollectionModel<SmileVideoCommentViewModel> OriginalPosterCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();

        public CollectionModel<SmileVideoCommentDataModel> ShowingCommentList { get; } = new CollectionModel<SmileVideoCommentDataModel>();

        public CollectionModel<SmileVideoTagViewModel> TagItems { get; } = new CollectionModel<SmileVideoTagViewModel>();
        public CollectionModel<SmileVideoInformationViewModel> RelationVideoItems { get; } = new CollectionModel<SmileVideoInformationViewModel>();

        public CollectionModel<object> MarketItems { get; } = new CollectionModel<object>();

        public SmileVideoFilteringViweModel LocalCommentFilering { get; set; }
        public SmileVideoFilteringViweModel GlobalCommentFilering { get; }

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

        public CollectionModel<SmileVideoInformationViewModel> PlayListItems { get; } = new CollectionModel<SmileVideoInformationViewModel>();

        public LoadState RelationVideoLoadState
        {
            get { return this._relationVideoLoadState; }
            set { SetVariableValue(ref this._relationVideoLoadState, value); }
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
            get { return Setting.Player.Volume; }
            set
            {
                if(SetPropertyValue(Setting.Player, value, nameof(Setting.Player.Volume))) {
                    Player.Volume = Setting.Player.Volume;
                }
            }
        }

        public bool IsMute
        {
            get { return Setting.Player.IsMute; }
            set
            {
                if(SetPropertyValue(Setting.Player, value, nameof(Setting.Player.IsMute))) {
                    Player.IsMute = Setting.Player.IsMute;
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
            set
            {
                var prevSelectedComment = this._selectedComment;
                if(SetVariableValue(ref this._selectedComment, value)) {
                    IsSelectedComment = this._selectedComment != null;
                    if(IsSelectedComment) {
                        this._selectedComment.IsSelected = true;
                    }
                    if(prevSelectedComment != null) {
                        prevSelectedComment.IsSelected = false;
                    }
                }
            }
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

        public double CommentAreaWidth
        {
            get { return this._commentAreaWidth; }
            set { SetVariableValue(ref this._commentAreaWidth, value); }
        }
        public double CommentAreaHeight
        {
            get { return this._commentAreaHeight; }
            set { SetVariableValue(ref this._commentAreaHeight, value); }
        }

        [Obsolete]
        public GridLength VisualPlayerWidth
        {
            get { return this._visualPlayerWidth; }
            set { SetVariableValue(ref this._visualPlayerWidth, value); }
        }
        [Obsolete]
        public GridLength VisualPlayerHeight
        {
            get { return this._visualPlayerHeight; }
            set { SetVariableValue(ref this._visualPlayerHeight, value); }
        }
        [Obsolete]
        public GridLength CommentListLength
        {
            get { return this._commentListLength; }
            set { SetVariableValue(ref this._commentListLength, value); }
        }
        public PlayerState PlayerState
        {
            get { return this._playerState; }
            set { SetVariableValue(ref this._playerState, value); }
        }

        /// <summary>
        /// 読込中で停止しているか。
        /// </summary>
        public bool IsBufferingStop
        {
            get { return this._isBufferingStop; }
            set { SetVariableValue(ref this._isBufferingStop, value); }
        }
        float BufferingVideoPosition { get; set; }

        public bool ReplayVideo
        {
            get { return Setting.Player.ReplayVideo; }
            set { SetPropertyValue(Setting.Player, value, nameof(Setting.Player.ReplayVideo)); }
        }

        public string UserNickname
        {
            get { return VideoInformation.UserNickname; }
        }
        public string UserId
        {
            get { return VideoInformation.UserId; }
        }

        public IReadOnlyList<SmileVideoMyListFinderViewModelBase> AccountMyListItems
        {
            get
            {
                if(this._accountMyListItems == null) {
                    var response = Mediation.Request(new SmileVideoCustomSettingRequestModel(SmileVideoCustomSettingKind.MyList));
                    var result = (SmileVideoAccountMyListSettingResultModel)response.Result;
                    this._accountMyListItems = result.AccountMyList;
                }

                return this._accountMyListItems;
            }
        }

        public bool IsSelectedComment
        {
            get { return this._isSelectedComment; }
            set { SetVariableValue(ref this._isSelectedComment, value); }
        }

        public bool IsEnabledDisplayCommentLimit
        {
            get { return Setting.Player.IsEnabledDisplayCommentLimit; }
            set { SetPropertyValue(Setting.Player, value); }
        }
        public int DisplayCommentLimitCount
        {
            get { return Setting.Player.DisplayCommentLimitCount; }
            set { SetPropertyValue(Setting.Player, value); }
        }

        public bool IsSelectedVideoInformation
        {
            get { return this._isSelectedVideoInformation; }
            set { SetVariableValue(ref this._isSelectedVideoInformation, value); }
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

        public ICommand OpenRelationVideo
        {
            get
            {
                return CreateCommand(
                    o => {
                        var videoInformation = (SmileVideoInformationViewModel)o;
                        LoadAsync(videoInformation, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ConfigureAwait(false);
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
                                switch(Player.State) {
                                    case Meta.Vlc.Interop.Media.MediaState.NothingSpecial:
                                    case Meta.Vlc.Interop.Media.MediaState.Stopped:
                                        SetMedia();
                                        VideoPlay();
                                        break;

                                    default:
                                        //Player.BeginStop(() => {
                                        //    Player.Play();
                                        //});
                                        Player.Stop();
                                        Player.Play();
                                        break;
                                }
                                return;

                            case PlayerState.Playing:
                                Player.PauseOrResume();
                                return;

                            case PlayerState.Pause:
                                if(IsBufferingStop) {
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
                        StopMovie();
                        UserOperationStop = false;
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

                        SearchTag(tagViewModel);
                    }
                );
            }
        }

        public ICommand OpenPediaCommand
        {
            get
            {
                return CreateCommand(o => {
                    var viewModel = (SmileVideoTagViewModel)o;
                    // TODO: 百科事典までサポートすんの？ だりぃぞー
                    //       今のところブラウザオープン(それも完全固定値)だけにする
                    var baseUri = new Uri("http://dic.nicovideo.jp/a/");
                    var uri = new Uri(baseUri, viewModel.TagName);
                    try {
                        Process.Start(uri.ToHttpEncodeString());
                    } catch(Exception ex) {
                        Mediation.Logger.Warning(ex);
                    }
                });
            }
        }

        public ICommand OpenUserIdCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        OpenUserId(UserId);
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

        public ICommand AdditionMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var myListFinder = o as SmileVideoMyListFinderViewModelBase;
                        AdditionMyListAsync(myListFinder).ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand LoadSelectPlayListItemCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var selectVideoInformation = o as SmileVideoInformationViewModel;
                        LoadAsync(selectVideoInformation, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand LoadPrevPlayListItemCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(1 < PlayListItems.Count) {
                            LoadPrevPlayListItemAsync().ConfigureAwait(false);
                        }
                    }
                );
            }
        }
        public ICommand LoadNextPlayListItemCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(1 < PlayListItems.Count) {
                            LoadNextPlayListItemAsync().ConfigureAwait(false);
                        }
                    }
                );
            }
        }

        [Obsolete]
        public ICommand ChangePlayerSizeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var percent = int.Parse((string)o);
                        ChangePlayerSizeFromPercent(percent);
                    }
                );
            }
        }

        public ICommand ChangedFilteringCommand
        {
            get
            {
                return CreateCommand(
                    o=> {
                        ApprovalComment();
                    }
                );
            }
        }

        public ICommand ClearSelectedCommentCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        ClearSelectedComment();
                    }
                );
            }
        }

        #endregion


        #region function

        public Task LoadAsync(IEnumerable<SmileVideoInformationViewModel> videoInformations, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            PlayListItems.AddRange(videoInformations);
            return LoadAsync(PlayListItems.First(), thumbCacheSpan, imageCacheSpan);
        }


        [Obsolete]
        void ChangePlayerSizeFromPercent(int percent)
        {
            if(VisualVideoSize.IsEmpty) {
                return;
            }
            var baseLength = 100.0;
            var scale = percent / baseLength;
            var videoWidth = VisualVideoSize.Width * scale;
            VisualPlayerWidth = new GridLength(videoWidth);
            CommentListLength = new GridLength(Width - videoWidth);
            //VisualPlayerHeight = new GridLength(VisualVideoSize.Height * scale);

            //View.SizeToContent = SizeToContent.Width;
        }

        void SetVideoDataInformation()
        {
            RealVideoWidth = Player.VlcMediaPlayer.PixelWidth;
            RealVideoHeight = Player.VlcMediaPlayer.PixelHeight;

            // コメントエリアのサイズ設定
            ChangeBaseSize();

            if(RealVideoHeight < CommentAreaHeight) {
                var realScale = RealVideoHeight/CommentAreaHeight;
                CommentAreaWidth *= realScale;
            }
        }

        void SetMedia()
        {
            Player.LoadMedia(PlayFile.FullName);
        }

        void StartIfAutoPlay()
        {
            if(Setting.Player.AutoPlay) {
                SetMedia();
                VideoPlay();
            }
        }

        void VideoPlay()
        {
            Player.IsMute = IsMute;
            Player.Volume = Volume;
            View.Dispatcher.BeginInvoke(new Action(() => {
                Player.Play();
            }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        void ClearComment()
        {
            foreach(var data in ShowingCommentList.ToArray()) {
                data.Clock.Controller.SkipToFill();
                data.Clock.Controller.Remove();
            }
            ShowingCommentList.Clear();
        }

        static bool InShowTime(SmileVideoCommentViewModel comment, TimeSpan prevTime, TimeSpan nowTime)
        {
            return prevTime <= (comment.ElapsedTime - correctionTime) && (comment.ElapsedTime - correctionTime) <= nowTime;
        }

        void MoveVideoPostion(float targetPosition)
        {
            Mediation.Logger.Debug(targetPosition.ToString());

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

            Mediation.Logger.Debug(setPosition.ToString());
            Player.Position = setPosition;
            Mediation.Logger.Debug(Player.Position.ToString());
            PrevPlayedTime = Player.Time;
        }

        static SmileVideoCommentElement CreateCommentElement(SmileVideoCommentViewModel commentViewModel, Size commentArea, SmileVideoSettingModel setting)
        {
            var element = new SmileVideoCommentElement();
            using(Initializer.BeginInitialize(element)) {
                element.DataContext = commentViewModel;
            }

            if(commentViewModel.Vertical != SmileVideoCommentVertical.Normal) {
                element.Width = commentArea.Width;
            }

            return element;
        }

        static int GetSafeYPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, OrderBy orderBy, bool calculationWidth, SmileVideoSettingModel setting)
        {
            var isAsc = orderBy == OrderBy.Ascending;
            // 空いている部分に放り込む
            var lineList = showingCommentList
                .Where(i => i.ViewModel.Vertical == commentViewModel.Vertical)
                .GroupBy(i => (int)Canvas.GetTop(i.Element))
                .IfOrderByAsc(g => g.Key, isAsc)
                .ToArray()
            ;

            var myHeight = (int)element.ActualHeight;
            var start = isAsc ? 0 : (int)commentArea.Height - myHeight;
            var last = isAsc ? (int)commentArea.Height - myHeight : 0;

            if(lineList.Length == 0) {
                // コメントない
                return start;
            }

            for(var y = start; isAsc ? y < last: last < y;) {
                var dupLine = lineList.FirstOrDefault(ls => ls.Key <= y && y + myHeight <= ls.Key + ls.Max(l => l.Element.ActualHeight));
                if(dupLine == null) {
                    // 誰もいなければ入れる
                    return y;
                } else {
                    // 横方向の重なりを考慮
                    if(calculationWidth) {
                        // 誰かいる場合はその末尾に入れられるか
                        var lastComment = dupLine.FirstOrDefault(l => commentArea.Width < Canvas.GetLeft(l.Element) + l.Element.ActualWidth);
                        if(lastComment == null) {
                            return y;
                        }
                    }

                    // 現在コメント行の最大の高さを加算して次行を検索
                    var plusValue = (int)dupLine.Max(l => l.Element.ActualHeight);
                    if(isAsc) {
                        y += plusValue;
                    } else {
                        y -= plusValue;
                    }
                }
            }

            // 全走査して安全そうなところがなければ一番少なそうなところに設定する
            var compromiseY = lineList
                .OrderBy(line => line.Count())
                .FirstOrDefault()
                ?.Key ?? 0
            ;
            return compromiseY;
        }

        static void SetMarqueeCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, SmileVideoSettingModel setting)
        {
            var y = GetSafeYPosition(commentViewModel, element, commentArea, showingCommentList, OrderBy.Ascending, true, setting);
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, commentArea.Width);
        }

        static void SetStaticCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, OrderBy orderBy, SmileVideoSettingModel setting)
        {
            var y = GetSafeYPosition(commentViewModel, element, commentArea, showingCommentList, orderBy, false, setting);
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, 0);
            element.Width = commentArea.Width;
        }

        static void SetCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, SmileVideoSettingModel setting)
        {
            switch(commentViewModel.Vertical) {
                case SmileVideoCommentVertical.Normal:
                    SetMarqueeCommentPosition(commentViewModel, element, commentArea, showingCommentList, setting);
                    break;

                case SmileVideoCommentVertical.Top:
                    SetStaticCommentPosition(commentViewModel, element, commentArea, showingCommentList, OrderBy.Ascending, setting);
                    break;

                case SmileVideoCommentVertical.Bottom:
                    SetStaticCommentPosition(commentViewModel, element, commentArea, showingCommentList, OrderBy.Descending, setting);
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
            if(double.IsInfinity(diffPosition)) {
                diffPosition = 0;
            }

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

        static void FireShowCommentCore(Canvas commentParentElement, TimeSpan prevTime, TimeSpan nowTime, IList<SmileVideoCommentViewModel> commentViewModelList, IList<SmileVideoCommentDataModel> showingCommentList, SmileVideoSettingModel setting)
        {
            var commentArea = new Size(
               commentParentElement.ActualWidth,
               commentParentElement.ActualHeight
            );

            var list = commentViewModelList.ToArray();
            // 現在時間から-1秒したものを表示対象とする
            var newComments = list
                .Where(c => c.Approval)
                .Where(c => !c.NowShowing)
                .Where(c => InShowTime(c, prevTime ,nowTime))
                .ToArray()
            ;
            if(newComments.Any()) {
                foreach(var commentViewModel in newComments) {
                    var element = CreateCommentElement(commentViewModel, commentArea, setting);

                    commentViewModel.NowShowing = true;

                    commentParentElement.Children.Add(element);
                    commentParentElement.UpdateLayout();

                    SetCommentPosition(commentViewModel, element, commentArea, showingCommentList, setting);

                    // アニメーション設定
                    var animation = CreateCommentAnimeation(commentViewModel, element, commentArea, prevTime - correctionTime, setting.Comment.ShowTime + correctionTime);

                    var data = new SmileVideoCommentDataModel(element, commentViewModel, animation);
                    showingCommentList.Add(data);

                    EventDisposer<EventHandler> ev = null;
                    data.Clock.Completed += EventUtility.Create<EventHandler>((object sender, EventArgs e) => {
                        if(element != null) {
                            commentParentElement.Children.Remove(element);
                        }
                        element = null;

                        if(ev != null) {
                            ev.Dispose();
                        }
                        ev = null;

                        showingCommentList.Remove(data);
                        data.ViewModel.NowShowing = false;

                    }, h => commentParentElement.Dispatcher.BeginInvoke(new Action(() => animation.Completed -= h)), out ev);

                    element.ApplyAnimationClock(Canvas.LeftProperty, data.Clock);
                }
                // 超過分のコメントを破棄
                if(setting.Player.IsEnabledDisplayCommentLimit && 0 < setting.Player.DisplayCommentLimitCount) {
                    var removeList = showingCommentList
                        .OrderBy(i => i.ViewModel.ElapsedTime)
                        .ThenBy(i => i.ViewModel.Number)
                        .Take(showingCommentList.Count - setting.Player.DisplayCommentLimitCount)
                        .ToArray()
                    ;
                    foreach(var item in removeList) {
                        item.Clock.Controller.SkipToFill();
                    }
                }
            }
        }

        void FireShowComment()
        {
            Mediation.Logger.Trace($"{PrevPlayedTime} - {PlayTime}, {Player.ActualWidth}x{Player.ActualHeight}");

            FireShowCommentCore(NormalCommentArea, PrevPlayedTime, PlayTime, NormalCommentList, ShowingCommentList, Setting);
            FireShowCommentCore(OriginalPosterCommentArea, PrevPlayedTime, PlayTime, OriginalPosterCommentList, ShowingCommentList, Setting);
        }

        void ScrollCommentList()
        {
            if(!IsAutoScroll) {
                return;
            }
            var nowTimelineItem = CommentItems
                .Cast<SmileVideoCommentViewModel>()
                .FirstOrDefault(c => InShowTime(c, PrevPlayedTime, PlayTime))
            ;
            if(nowTimelineItem != null) {
                CommentView.ScrollToCenterOfView(nowTimelineItem, true, false);
            }
        }

        bool FilterCommentItems(object o)
        {
            if(FilteringCommentType == SmileVideoFilteringCommentType.All) {
                return true;
            }

            var item = (SmileVideoCommentViewModel)o;

            return item.FilteringView;
        }

        Task LoadRelationVideoAsync()
        {
            RelationVideoLoadState = LoadState.Preparation;

            RelationVideoLoadState = LoadState.Loading;
            return VideoInformation.LoadRelationVideosAsync(Constants.ServiceSmileVideoRelationCacheSpan).ContinueWith(task => {
                var items = task.Result;
                if(items == null) {
                    RelationVideoLoadState = LoadState.Failure;
                    return Task.CompletedTask;
                } else {
                    RelationVideoItems.InitializeRange(items);
                    var loader = new SmileVideoInformationLoader(items);
                    return loader.LoadThumbnaiImageAsync(Constants.ServiceSmileVideoImageCacheSpan).ContinueWith(_ => {
                        RelationVideoLoadState = LoadState.Loaded;
                    });
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void CheckTagPedia()
        {
            Debug.Assert(VideoInformation.PageHtmlLoadState == LoadState.Loaded);

            IsCheckedTagPedia = true;

            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            htmlDocument.LoadHtml(VideoInformation.PageHtml);
            var json = SmileVideoWatchAPIUtility.ConvertJsonFromWatchPage(VideoInformation.PageHtml);
            var videoDetail = json?.SelectToken("videoDetail");
            var tagList = videoDetail?.SelectToken("tagList");
            if(tagList != null) {
                var map = TagItems.ToDictionary(tk => tk.TagName, tv => tv);
                foreach(var tagItem in tagList) {
                    var tagName = tagItem.Value<string>("tag");
                    var hasDic  = tagItem.Value<string>("dic");
                    if(RawValueUtility.ConvertBoolean(hasDic)) {
                        SmileVideoTagViewModel tag;
                        if(map.TryGetValue(tagName, out tag)) {
                            tag.ExistPedia = true;
                        }
                    }
                }
            }
        }

        void MakeDescription()
        {
            IsMakedDescription = true;

            var flowDocumentSource = SmileVideoDescriptionUtility.ConvertFlowDocumentFromHtml(Mediation, VideoInformation.DescriptionHtml);
#if false
#if DEBUG
            var h = Path.Combine(DownloadDirectory.FullName, $"description.html");
            using(var s = File.CreateText(h)) {
                s.Write(VideoInformation.DescriptionHtml);
            }
            foreach(var ext in new[] { "xml", "xaml" }) {
                var x = Path.Combine(DownloadDirectory.FullName, $"description.{ext}");
                using(var s = File.CreateText(x)) {
                    s.Write(flowDocumentSource);
                }
            }
#endif
#endif

            DocumentDescription.Dispatcher.Invoke(() => {
                var document = DocumentDescription.Document;

                document.Blocks.Clear();

                using(var stringReader = new StringReader(flowDocumentSource))
                using(var xmlReader = System.Xml.XmlReader.Create(stringReader)) {
                    try {
                        var flowDocument = XamlReader.Load(xmlReader) as FlowDocument;
                        document.Blocks.AddRange(flowDocument.Blocks.ToArray());
                    } catch(XamlParseException ex) {
                        Mediation.Logger.Error(ex);
                        var error = new Paragraph();
                        error.Inlines.Add(ex.ToString());

                        var raw = new Paragraph();
                        raw.Inlines.Add(flowDocumentSource);

                        document.Blocks.Add(error);
                        document.Blocks.Add(raw);
                    }
                }

                document.FontSize = DocumentDescription.FontSize;
                document.FontFamily = DocumentDescription.FontFamily;
                document.FontStretch = DocumentDescription.FontStretch;
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

        Task<SmileJsonResultModel> AdditionMyListAsync(SmileVideoMyListFinderViewModelBase myListFinder)
        {
            var defaultMyListFinder = myListFinder as SmileVideoAccountMyListDefaultFinderViewModel;
            if(defaultMyListFinder != null) {
                return AdditionAccountDefaultMyListAsync(defaultMyListFinder);
            } else {
                return AdditionAccountMyListAsync(myListFinder);
            }
        }

        Task<SmileJsonResultModel> AdditionAccountDefaultMyListAsync(SmileVideoAccountMyListDefaultFinderViewModel defaultMyListFinder)
        {
            var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            var myList = new Logic.Service.Smile.Api.V1.MyList(Mediation);
            return myList.AdditionAccountDefaultMyListFromVideo(VideoId, VideoInformation.PageVideoToken);
        }

        Task<SmileJsonResultModel> AdditionAccountMyListAsync(SmileVideoMyListFinderViewModelBase myListFinder)
        {
            var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            var myList = new Logic.Service.Smile.Api.V1.MyList(Mediation);
            return myList.AdditionAccountMyListFromVideo(myListFinder.MyListId, VideoInformation.ThreadId, VideoInformation.PageVideoToken);
        }

        void SearchTag(SmileVideoTagViewModel tagViewModel)
        {
            var parameter = new SmileVideoSearchParameterModel() {
                SearchType = SearchType.Tag,
                Query = tagViewModel.TagName,
            };

            Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, parameter, ShowViewState.Foreground));
        }

        void OpenUserId(string userId)
        {
            var parameter = new SmileOpenUserIdParameterModel() {
                UserId = userId,
            };

            Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.Smile, parameter, ShowViewState.Foreground));
        }

        void OpenWebLink(string link)
        {
            try {
                Process.Start(link);
            } catch(Exception ex) {
                Mediation.Logger.Warning(ex);
            }
        }

        Task OpenVideoLinkAsync(string videoId)
        {
            var cancel = new CancellationTokenSource();
            return SmileVideoInformationViewModel.CreateFromVideoIdAsync(Mediation, videoId, Constants.ServiceSmileVideoThumbCacheSpan).ContinueWith(task => {
                var videoInformation = task.Result;
                return LoadAsync(videoInformation, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
            }, cancel.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void OpenMyListLink(string myListId)
        {
            var parameter = new SmileVideoSearchMyListParameterModel() {
                Query = myListId,
                QueryType = SmileVideoSearchMyListQueryType.MyListId,
            };

            Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, parameter, ShowViewState.Foreground));
        }

        void StopMovie()
        {
            Mediation.Logger.Debug("stop");
            Player.Stop();
            //Player.BeginStop(() => {
            //    Mediation.Logger.Debug("stoped");
            //    PlayerState = PlayerState.Stop;
            //    VideoPosition = 0;
            //    ClearComment();
            //    PrevPlayedTime = TimeSpan.Zero;
            //});
            Mediation.Logger.Debug("stoped");
            PlayerState = PlayerState.Stop;
            VideoPosition = 0;
            ClearComment();
            PrevPlayedTime = TimeSpan.Zero;
        }

        void RefreshFilteringComment()
        {
            Debug.Assert(FilteringCommentType != SmileVideoFilteringCommentType.All);

            foreach(var item in CommentList) {
                item.FilteringView = false;
            }

            switch(FilteringCommentType) {
                case SmileVideoFilteringCommentType.All:
                    break;

                case SmileVideoFilteringCommentType.OriginalPoster:
                    foreach(var item in OriginalPosterCommentList) {
                        item.FilteringView = true;
                    }
                    break;

                case SmileVideoFilteringCommentType.UserId:
                    if(!string.IsNullOrWhiteSpace(FilteringUserId)) {
                        foreach(var item in CommentList.Where(i => i.UserId != null && i.UserId.IndexOf(FilteringUserId, StringComparison.OrdinalIgnoreCase) != -1)) {
                            item.FilteringView = true;
                        }
                    } else {
                        foreach(var item in CommentList) {
                            item.FilteringView = true;
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            CommentItems.Refresh();
        }

        static void ApprovalCommentSet(IEnumerable<SmileVideoCommentViewModel> list, bool value)
        {
            foreach(var item in list) {
                item.Approval = value;
            }
        }

        void ApprovalCommentSharedNoGood()
        {
            // 共有NG
            if(IsEnabledSharedNoGood) {
                var targetComments = CommentList.Where(c => c.Score <= Setting.Comment.SharedNoGoodScore);
                ApprovalCommentSet(targetComments, false);
            }
        }

        private void ApprovalCommentCustom(IReadOnlyList<SmileVideoFilteringItemSettingModel> filterList)
        {
            if(filterList.Any()) {
                var filters = filterList.Select(f => new SmileVideoFiltering(f));
                foreach(var filter in filters.AsParallel()) {
                    foreach(var item in CommentList.AsParallel().Where(c => c.Approval)) {
                        item.Approval = !filter.Check(item.Content, item.UserId, item.Commands);
                    }
                }
            }
        }

        void ApprovalComment()
        {
            ApprovalCommentSet(CommentList, true);

            ApprovalCommentSharedNoGood();
            ApprovalCommentCustom(LocalCommentFilering.CommentFilterList.ModelList);
            ApprovalCommentCustom(GlobalCommentFilering.CommentFilterList.ModelList);
        }

        void ClearSelectedComment()
        {
            SelectedComment = null;
        }

        Task LoadNextPlayListItemAsync()
        {
            // TODO: ランダム再生の考慮
            Debug.Assert(1 < PlayListItems.Count);

            var index = PlayListItems.FindIndex(i => i == VideoInformation);
            if(index == PlayListItems.Count - 1) {
                index = 0;
            } else if(index >= 0) {
                index += 1;
            } else {
                index = 0;
            }
            var targetViewModel = PlayListItems[index];
            return LoadAsync(targetViewModel, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        Task LoadPrevPlayListItemAsync()
        {
            // TODO: ランダム再生の考慮と*Next*時にちゃんと戻れるようにする
            Debug.Assert(1 < PlayListItems.Count);

            var index = PlayListItems.FindIndex(i => i == VideoInformation);
            if(index == 0) {
                index = PlayListItems.Count - 1;
            } else if(0 < index) {
                index -= 1;
            } else {
                index = 0;
            }
            var targetViewModel = PlayListItems[index];
            return LoadAsync(targetViewModel, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        #endregion

        #region SmileVideoDownloadViewModel

        public override Task LoadAsync(SmileVideoInformationViewModel videoInformation, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            foreach(var item in PlayListItems.Where(i => i != videoInformation)) {
                item.IsPlaying = false;
            }
            videoInformation.IsPlaying = true;
            IsSelectedVideoInformation = true;

            return base.LoadAsync(videoInformation, thumbCacheSpan, imageCacheSpan);
        }

        protected override void OnDownloadStart(object sender, DownloadStartEventArgs e)
        {
            if(!IsMakedDescription) {
                MakeDescription();
            }
            if(!IsCheckedTagPedia) {
                CheckTagPedia();
            }

            base.OnDownloadStart(sender, e);
        }

        protected override void OnDownloading(object sender, DownloadingEventArgs e)
        {
            if(VideoInformation.MovieType != SmileVideoMovieType.Swf) {
                if(!CanVideoPlay) {
                    // とりあえず待って
                    VideoFile.Refresh();
                    CanVideoPlay = VideoPlayLowestSize < VideoFile.Length;
                    if(CanVideoPlay) {
                        StartIfAutoPlay();
                    }
                }
            }
            e.Cancel = IsViewClosed || IsProcessCancel ;
            if(e.Cancel) {
                StopMovie();
            }

            base.OnDownloading(sender, e);
        }

        void ResetSwf()
        {
            PlayFile.Refresh();
            //VideoLoadedSize = VideoTotalSize = PlayFile.Length;
        }

        protected override void OnLoadVideoEnd()
        {
            if(!IsProcessCancel) {
                if(VideoInformation.PageHtmlLoadState == LoadState.Loaded) {
                    if(!IsMakedDescription) {
                        MakeDescription();
                    }
                    if(!IsCheckedTagPedia) {
                        CheckTagPedia();
                    }
                }

                if(VideoInformation.MovieType == SmileVideoMovieType.Swf && !VideoInformation.ConvertedSwf) {
                    // 変換が必要
                    var ffmpeg = new Ffmpeg();
                    var s = $"-i \"{VideoFile.FullName}\" -vcodec flv \"{PlayFile.FullName}\"";
                    ffmpeg.ExecuteAsync(s).ContinueWith(task => {
                        if(!IsViewClosed) {
                            VideoInformation.ConvertedSwf = true;
                            VideoInformation.SaveSetting(true);
                            ResetSwf();
                            CanVideoPlay = true;
                            StartIfAutoPlay();
                        }
                    });
                } else {
                    if(VideoInformation.MovieType == SmileVideoMovieType.Swf) {
                        ResetSwf();
                    }
                    // あまりにも小さい場合は読み込み完了時にも再生できなくなっている
                    if(!CanVideoPlay) {
                        CanVideoPlay = true;
                        StartIfAutoPlay();
                    }
                }
            }

            base.OnLoadVideoEnd();
        }

        protected override Task LoadCommentAsync(RawSmileVideoMsgPacketModel rawMsgPacket)
        {
            var comments = rawMsgPacket.Chat
                .Where(c => !string.IsNullOrEmpty(c.Content))
                .GroupBy(c => new { c.No, c.Fork })
                .Select(c => new SmileVideoCommentViewModel(c.First(), Setting))
                .OrderBy(c => c.ElapsedTime)
            ;
            CommentList.InitializeRange(comments);
            CommentListCount = CommentList.Count;
            ApprovalComment();

            NormalCommentList.InitializeRange(CommentList.Where(c => !c.IsOriginalPoster));
            var userSequence = NormalCommentList
                .Where(c => !string.IsNullOrWhiteSpace(c.UserId))
                .GroupBy(c => c.UserId)
                .Select(g => new { Count = g.Count(), Comment = g.First() })
                .Select(cc => new FilteringUserViewModel(cc.Comment.UserId, cc.Comment.UserKind, cc.Count))
                .OrderByDescending(fu => fu.Count)
                .ThenBy(fu => fu.UserId)
            ;
            FilteringUserList.InitializeRange(userSequence);
            OriginalPosterCommentList.InitializeRange(CommentList.Where(c => c.IsOriginalPoster));
            OriginalPosterCommentListCount = OriginalPosterCommentList.Count;

            if(FilteringCommentType != SmileVideoFilteringCommentType.All) {
                RefreshFilteringComment();
            }

            return base.LoadCommentAsync(rawMsgPacket);
        }

        protected override void OnLoadGetthumbinfoEnd()
        {
            if(!PlayListItems.Any()) {
                PlayListItems.Add(VideoInformation);
            }

            TotalTime = VideoInformation.Length;

            TagItems.InitializeRange(VideoInformation.TagList);

            LoadRelationVideoAsync();

            LocalCommentFilering = new SmileVideoFilteringViweModel(VideoInformation.IndividualVideoSetting.Filtering);
            var propertyNames = new[] {
                nameof(LocalCommentFilering),
                nameof(VideoId),
                nameof(Title),
                nameof(VideoInformation),
            };
            CallOnPropertyChange(propertyNames);

            base.OnLoadGetthumbinfoEnd();
        }

        protected override void InitializeStatus()
        {
            base.InitializeStatus();
            IsFirstPlay = true;
            VideoPosition = 0;
            PrevPlayedTime = TimeSpan.Zero;
            _prevStateChangedPosition = initPrevStateChangedPosition;
            IsBufferingStop = false;
            UserOperationStop = false;
            IsMakedDescription = false;
            IsCheckedTagPedia = false;
            ChangingVideoPosition = false;
            MovingSeekbarThumb = false;
            CanVideoPlay = false;
            IsVideoPlayng = false;
            PlayTime = TimeSpan.Zero;
            TotalTime = TimeSpan.Zero;
            SelectedComment = null;
            if(View != null) {
                if(Player != null && Player.State != Meta.Vlc.Interop.Media.MediaState.Playing && Player.State != Meta.Vlc.Interop.Media.MediaState.Paused) {
                    // https://github.com/higankanshi/Meta.Vlc/issues/80
                    Player.VlcMediaPlayer.Media = null;
                }
                View.Dispatcher.Invoke(() => {
                    CommentList.Clear();
                    NormalCommentList.Clear();
                    OriginalPosterCommentList.Clear();
                    ClearComment();
                });
            }
        }

        protected override Task StopPrevProcessAsync()
        {
            var processTask = base.StopPrevProcessAsync();
            var playerTask = Task.CompletedTask;
            if(Player!= null) {
                if(Player.State != Meta.Vlc.Interop.Media.MediaState.Stopped) {
                    playerTask = Task.Run(() => {
                        var sleepTime = TimeSpan.FromMilliseconds(500);
                        Thread.Sleep(sleepTime);
                        Player.Stop();
                        InitializeStatus();
                    });
                }
            }

            return Task.WhenAll(processTask, playerTask);
        }

        #endregion

        #region ISetView

        public void SetView(FrameworkElement view)
        {
            var playerView = (SmileVideoPlayerWindow)view;

            View = playerView;
            Player = playerView.player;//.MediaPlayer;
            NormalCommentArea = playerView.normalCommentArea;
            OriginalPosterCommentArea = playerView.originalPosterCommentArea;
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

        #region ISmileVideoDescription

        public ICommand OpenWebLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var link = o as string;
                        OpenWebLink(link);
                    }
                );
            }
        }

        public ICommand OpenVideoLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var videoId = o as string;
                        OpenVideoLinkAsync(videoId).ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand OpenMyListLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var myListId = o as string;
                        OpenMyListLink(myListId);
                    }
                );
            }
        }

        public ICommand OpenUserLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var userId = o as string;
                        OpenUserId(userId);
                    }
                );
            }
        }

        #endregion

        #region event

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

            VideoInformation.SaveSetting(true);

            if(Player.State == Meta.Vlc.Interop.Media.MediaState.Playing) {
                //Player.BeginStop();
                Player.Stop();
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

        private void Player_StateChanged(object sender, Meta.Vlc.ObjectEventArgs<Meta.Vlc.Interop.Media.MediaState> e)
        {
            // 開いてる最中は気にしない
            if(e.Value == Meta.Vlc.Interop.Media.MediaState.Opening) {
                return;
            }
            //if(PlayerState != PlayerState.Pause && this._prevStateChangedPosition == VideoPosition && this._prevStateChangedPosition != initPrevStateChangedPosition) {
            //    return;
            //}
            this._prevStateChangedPosition = VideoPosition;

            Mediation.Logger.Debug($"{e.Value}, pos: {VideoPosition}, time: {PlayTime} / {Player.Length}");
            switch(e.Value) {
                case Meta.Vlc.Interop.Media.MediaState.Playing:
                    var prevState = PlayerState;
                    PlayerState = PlayerState.Playing;
                    if(prevState == PlayerState.Pause) {
                        foreach(var data in ShowingCommentList) {
                            data.Clock.Controller.Resume();
                        }
                    }
                    break;

                case Meta.Vlc.Interop.Media.MediaState.Paused:
                    PlayerState = PlayerState.Pause;
                    foreach(var data in ShowingCommentList) {
                        data.Clock.Controller.Pause();
                    }
                    break;

                case Meta.Vlc.Interop.Media.MediaState.Ended:
                    if(VideoLoadState == LoadState.Loading) {
                        Mediation.Logger.Debug("buffering stop");
                        // 終わってない
                        IsBufferingStop = true;
                        BufferingVideoPosition = VideoPosition;
                    } else if(IsBufferingStop) {
                        Mediation.Logger.Debug("buffering wait");
                        PlayerState = PlayerState.Pause;
                        Player.Position = BufferingVideoPosition;
                        foreach(var data in ShowingCommentList) {
                            data.Clock.Controller.Pause();
                        }
                    } else if(PlayListItems.Skip(1).Any() && !UserOperationStop) {
                        Mediation.Logger.Debug("next playlist item");
                        LoadNextPlayListItemAsync();
                    } else if(ReplayVideo && !UserOperationStop) {
                        Mediation.Logger.Debug("replay");
                        //Player.BeginStop(() => {
                        //    Player.Dispatcher.Invoke(() => {
                        //        Player.Play();
                        //    });
                        //});
                        Player.Stop();
                        Player.Play();
                    } else {
                        //PlayerState = PlayerState.Stop;
                        //VideoPosition = 0;
                        //ClearComment();
                        //PrevPlayedTime = TimeSpan.Zero;
                        StopMovie();
                    }
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}
