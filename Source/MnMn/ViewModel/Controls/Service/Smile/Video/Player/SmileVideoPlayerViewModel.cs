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
using System.Windows.Threading;
using System.Runtime.ExceptionServices;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;
using System.Windows.Controls.Primitives;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using OxyPlot;
using OxyPlot.Wpf;
using ContentTypeTextNet.MnMn.MnMn.Model.Order;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    public class SmileVideoPlayerViewModel: SmileVideoDownloadViewModel, ISetView, ISmileVideoDescription, ICloseView, ICaptionCommand
    {
        #region define

        //static readonly Size BaseSize_4x3 = new Size(640, 480);
        //static readonly Size BaseSize_16x9 = new Size(512, 384);

        const float initPrevStateChangedPosition = -1;

        static readonly Thickness enabledResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
        static readonly Thickness maximumWindowBorderThickness = SystemParameters.WindowResizeBorderThickness;
        static readonly Thickness normalWindowBorderThickness = new Thickness(1);

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

        double _commentEnabledPercent = 100;
        double _commentEnabledHeight;
        bool _showEnabledCommentPreviewArea = false;
        bool _isEnabledOriginalPosterCommentArea = false;

        [Obsolete]
        GridLength _commentListLength = new GridLength(3, GridUnitType.Star);
        [Obsolete]
        GridLength _visualPlayerWidth = new GridLength(7, GridUnitType.Star);
        [Obsolete]
        GridLength _visualPlayerHeight = new GridLength(1, GridUnitType.Star);

        PlayerState _playerState;
        bool _isBufferingStop;

        IReadOnlyList<SmileVideoAccountMyListFinderViewModel> _accountMyListItems;
        IReadOnlyList<SmileVideoBookmarkNodeViewModel> _bookmarkItems;

        SmileVideoFilteringCommentType _filteringCommentType;
        string _filteringUserId;
        int _commentListCount;
        int _originalPosterCommentListCount;

        bool _isSelectedComment;
        bool _isSelectedInformation;
        bool _isSettedMedia;

        long _secondsDownloadingSize;

        SmileVideoCommentVertical _postCommandVertical = SmileVideoCommentVertical.Normal;
        SmileVideoCommentSize _postCommandSize = SmileVideoCommentSize.Medium;
        Color _postCommandColor = Colors.White;
        string _postBeforeCommand;
        string _postAfterCommand;
        string _postCommentBody;

        string _commentInformation;

        bool _isNormalWindow = true;
        Thickness _resizeBorderThickness = enabledResizeBorderThickness;
        Thickness _windowBorderThickness = normalWindowBorderThickness;

        bool _showCommentChart;

        #endregion

        public SmileVideoPlayerViewModel(Mediation mediation)
            : base(mediation)
        {
            CommentItems = CollectionViewSource.GetDefaultView(CommentList);
            CommentItems.Filter = FilterCommentItems;

            var filteringResult = Mediation.GetResultFromRequest<SmileVideoFilteringResultModel>(new SmileVideoCustomSettingRequestModel(SmileVideoCustomSettingKind.CommentFiltering));
            GlobalCommentFilering = filteringResult.Filtering;
        }

        #region property

        /// <summary>
        /// ウィンドウ要素。
        /// </summary>
        SmileVideoPlayerWindow View { get; set; }
        /// <summary>
        /// プレイヤー要素。
        /// </summary>
        VlcPlayer Player { get; set; }
        /// <summary>
        /// ナビゲーションバー要素。
        /// </summary>
        Navigationbar Navigationbar { get; set; }
        /// <summary>
        /// 視聴者コメント表示レイヤー要素。
        /// </summary>
        Canvas NormalCommentArea { get; set; }
        /// <summary>
        /// 投稿者コメント表示レイヤー要素。
        /// </summary>
        Canvas OriginalPosterCommentArea { get; set; }
        /// <summary>
        /// コメント一覧要素。
        /// </summary>
        ListBox CommentView { get; set; }
        /// <summary>
        /// コメント詳細部分要素。
        /// <para>使ってないんよねぇ。</para>
        /// </summary>
        Border DetailComment { get; set; }
        /// <summary>
        /// 動画紹介文書表示要素。
        /// </summary>
        FlowDocumentScrollViewer DocumentDescription { get; set; }
        /// <summary>
        /// コメント有効位置抑制スライダー要素。
        /// </summary>
        Slider EnabledCommentSlider { get; set; }

        /// <summary>
        /// タイトル。
        /// </summary>
        public string Title
        {
            get { return $"[{VideoId}]: {Information.Title}"; }
            set { /*dmy*/ }
        }

        /// <summary>
        /// コマンドで使用する色一覧。
        /// </summary>
        public CollectionModel<Color> CommandColorItems { get; } = new CollectionModel<Color>(SmileVideoMsgUtility.normalCommentColors);

        /// <summary>
        /// プレイリストはランダム再生か。
        /// </summary>
        public bool IsRandomPlay
        {
            get { return PlayListItems.IsRandom; }
            set { SetPropertyValue(PlayListItems, value, nameof(PlayListItems.IsRandom)); }
        }

        #region window

        /// <summary>
        /// ウィンドウ X 座標。
        /// </summary>
        public double Left
        {
            get { return Setting.Player.Window.Left; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetPropertyValue(Setting.Player.Window, value, nameof(Setting.Player.Window.Left));
                }
            }
        }
        /// <summary>
        /// ウィンドウ Y 座標。
        /// </summary>
        public double Top
        {
            get { return Setting.Player.Window.Top; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetPropertyValue(Setting.Player.Window, value, nameof(Setting.Player.Window.Top));
                }
            }
        }
        /// <summary>
        /// ウィンドウ横幅。
        /// </summary>
        public double Width
        {
            get { return Setting.Player.Window.Width; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetPropertyValue(Setting.Player.Window, value, nameof(Setting.Player.Window.Width));
                }
            }
        }
        /// <summary>
        /// ウィンドウ高さ。
        /// </summary>
        public double Height
        {
            get { return Setting.Player.Window.Height; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetPropertyValue(Setting.Player.Window, value, nameof(Setting.Player.Window.Height));
                }
            }
        }
        /// <summary>
        /// 最前面表示状態。
        /// </summary>
        public bool Topmost
        {
            get { return Setting.Player.Window.Tommost; }
            set { SetPropertyValue(Setting.Player.Window, value, nameof(Setting.Player.Window.Height)); }
        }

        #endregion

        /// <summary>
        /// 詳細部分を表示状態。
        /// </summary>
        public bool PlayerShowDetailArea
        {
            get { return Setting.Player.ShowDetailArea; }
            set { SetPropertyValue(Setting.Player, value, nameof(Setting.Player.ShowDetailArea)); }
        }
        /// <summary>
        /// コメント一覧表示状態。
        /// </summary>
        public bool PlayerShowCommentArea
        {
            get
            {
                return IsNormalWindow ? Setting.Player.ShowNormalWindowCommentList : Setting.Player.ShowFullScreenCommentList;
            }
            set
            {
                var propertyName = IsNormalWindow ? nameof(Setting.Player.ShowNormalWindowCommentList) : nameof(Setting.Player.ShowFullScreenCommentList);

                SetPropertyValue(Setting.Player, value, propertyName);
            }
        }

        /// <summary>
        /// コメント表示レイヤー(投稿者・視聴者供に)表示状態。
        /// </summary>
        public bool PlayerVisibleComment
        {
            get { return Setting.Player.VisibleComment; }
            set { SetPropertyValue(Setting.Player, value, nameof(Setting.Player.VisibleComment)); }
        }

        /// <summary>
        /// コメント一覧自動スクロール状態。
        /// </summary>
        public bool IsAutoScroll
        {
            get { return Setting.Player.AutoScrollCommentList; }
            set { SetPropertyValue(Setting.Player, value, nameof(Setting.Player.AutoScrollCommentList)); }
        }

        /// <summary>
        /// 共有NG有効状態。
        /// </summary>
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
        /// <summary>
        /// 共有NG閾値。
        /// </summary>
        public int SharedNoGoodScore
        {
            get { return Setting.Comment.SharedNoGoodScore; }
            set
            {
                if(SetPropertyValue(Setting.Comment, value, nameof(Setting.Comment.SharedNoGoodScore))) {
                    ApprovalComment();
                }
            }
        }
        /// <summary>
        /// コメント一覧フィルタリング状態。
        /// <para>表示コメントではない。</para>
        /// </summary>
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
        /// <summary>
        /// コメント一覧でフィルタリングするユーザーID。
        /// <para>視聴者選択時のみに有効。</para>
        /// </summary>
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
        /// <summary>
        /// コメント一覧フィルタリングで用いるユーザー一覧。
        /// </summary>
        public CollectionModel<SmileVideoFilteringUserViewModel> FilteringUserList { get; } = new CollectionModel<SmileVideoFilteringUserViewModel>();
        /// <summary>
        /// コメント一覧の件数。
        ///
        /// TODO: べつにいらんのちゃうかなぁ。
        /// </summary>
        public int CommentListCount
        {
            get { return this._commentListCount; }
            private set { SetVariableValue(ref this._commentListCount, value); }
        }
        /// <summary>
        /// コメント一覧の件数。
        ///
        /// TODO: <see cref="CommentListCount"/>と同じ。
        /// </summary>
        public int OriginalPosterCommentListCount
        {
            get { return this._originalPosterCommentListCount; }
            private set { SetVariableValue(ref this._originalPosterCommentListCount, value); }
        }
        /// <summary>
        /// 匿名投稿が可能か。
        /// </summary>
        public bool IsEnabledPostAnonymous { get { return Information?.IsOfficialVideo ?? false; } }

        /// <summary>
        /// 初回再生か。
        /// </summary>
        bool IsFirstPlay { get; set; } = true;
        /// <summary>
        /// ユーザー操作で停止されたか。
        /// </summary>
        bool UserOperationStop { get; set; } = false;

        /// <summary>
        /// 投降者コメントが構築されたか。
        /// </summary>
        bool IsMadeDescription { get; set; } = false;
        /// <summary>
        ///
        /// </summary>
        bool IsCheckedTagPedia { get; set; } = false;

        public ICollectionView CommentItems { get; private set; }
        /// <summary>
        /// コメント全データ。
        /// </summary>
        CollectionModel<SmileVideoCommentViewModel> CommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
        /// <summary>
        /// 視聴者コメントデータ。
        /// <para><see cref="CommentList"/>から構築。</para>
        /// </summary>
        CollectionModel<SmileVideoCommentViewModel> NormalCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
        /// <summary>
        /// 投稿者コメントデータ。
        /// <para><see cref="CommentList"/>から構築。</para>
        /// </summary>
        CollectionModel<SmileVideoCommentViewModel> OriginalPosterCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();

        /// <summary>
        /// 現在表示中コメント。
        /// </summary>
        public CollectionModel<SmileVideoCommentDataModel> ShowingCommentList { get; } = new CollectionModel<SmileVideoCommentDataModel>();

        /// <summary>
        /// タグ。
        /// </summary>
        public CollectionModel<SmileVideoTagViewModel> TagItems { get; } = new CollectionModel<SmileVideoTagViewModel>();
        /// <summary>
        /// 関連動画。
        /// </summary>
        public CollectionModel<SmileVideoInformationViewModel> RelationVideoItems { get; } = new CollectionModel<SmileVideoInformationViewModel>();
        /// <summary>
        /// 市場。
        /// <para>未実装</para>
        /// </summary>
        public CollectionModel<object> MarketItems { get; } = new CollectionModel<object>();

        /// <summary>
        /// 動画に対するフィルタ設定。
        /// </summary>
        public SmileVideoFilteringViweModel LocalCommentFilering { get; set; }
        /// <summary>
        /// 全体に対するフィルタ設定。
        /// </summary>
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
        /// <summary>
        /// 自動再生可能サイズ。
        /// </summary>
        long VideoPlayLowestSize => Constants.ServiceSmileVideoPlayLowestSize;
        /// <summary>
        /// プレイリスト。
        /// </summary>
        public PlayListModel<SmileVideoInformationViewModel> PlayListItems { get; } = new PlayListModel<SmileVideoInformationViewModel>();
        /// <summary>
        /// 関連動画読込状態。
        /// </summary>
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
        /// <summary>
        /// ミュート状態。
        /// </summary>
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

        /// <summary>
        /// 選択中コメント。
        /// </summary>
        public SmileVideoCommentViewModel SelectedComment
        {
            get { return this._selectedComment; }
            set
            {
                var prevSelectedComment = this._selectedComment;
                if(SetVariableValue(ref this._selectedComment, value)) {
                    IsSelectedComment = SelectedComment != null;
                    if(IsSelectedComment) {
                        SelectedComment.IsSelected = true;
                    }
                    if(prevSelectedComment != null) {
                        prevSelectedComment.IsSelected = false;
                    }
                }
            }
        }

        /// <summary>
        /// 動画を実際に表示するサイズ。
        /// <para><see cref="RealVideoWidth"/>, <see cref="RealVideoHeight"/>に対してスケーリングしたサイズ。</para>
        /// </summary>
        Size VisualVideoSize { get; set; }

        /// <summary>
        /// 動画の物理サイズ(横幅)。
        /// </summary>
        public double RealVideoWidth
        {
            get { return this._realVideoWidth; }
            set { SetVariableValue(ref this._realVideoWidth, value); }
        }
        /// <summary>
        /// 動画の物理サイズ(高さ)。
        /// </summary>
        public double RealVideoHeight
        {
            get { return this._realVideoHeight; }
            set { SetVariableValue(ref this._realVideoHeight, value); }
        }

        /// <summary>
        /// なんかの横幅。
        /// </summary>
        public double BaseWidth
        {
            get { return this._baseWidth; }
            set { SetVariableValue(ref this._baseWidth, value); }
        }
        /// <summary>
        /// なんかの高さ。
        /// </summary>
        public double BaseHeight
        {
            get { return this._baseHeight; }
            set { SetVariableValue(ref this._baseHeight, value); }
        }
        /// <summary>
        /// コメント表示領域の幅。
        /// </summary>
        public double CommentAreaWidth
        {
            get { return this._commentAreaWidth; }
            set { SetVariableValue(ref this._commentAreaWidth, value); }
        }
        /// <summary>
        /// コメント表示領域の高さ。
        /// </summary>
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
        /// <summary>
        /// バッファ開始時点での動画位置。
        /// </summary>
        float BufferingVideoPosition { get; set; }
        /// <summary>
        /// リプレイ状態。
        /// </summary>
        public bool ReplayVideo
        {
            get { return Setting.Player.ReplayVideo; }
            set { SetPropertyValue(Setting.Player, value, nameof(Setting.Player.ReplayVideo)); }
        }

        /// <summary>
        /// チャンネル動画か。
        /// </summary>
        public bool IsChannelVideo { get { return Information.IsChannelVideo; } }
        /// <summary>
        /// 投稿者ユーザー名。
        /// </summary>
        public string UserName { get { return Information.UserName; } }
        /// <summary>
        /// 投稿者ユーザーID。
        /// </summary>
        public string UserId { get { return Information.UserId; } }
        /// <summary>
        /// 投稿者チャンネル名。
        /// </summary>
        public string ChannelName { get { return Information.ChannelName; } }
        /// <summary>
        /// 投稿者チャンネルID。
        /// </summary>
        public string ChannelId { get { return Information.ChannelId; } }

        /// <summary>
        /// 自身のアカウントに紐付くマイリストの一覧。
        /// </summary>
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

        /// <summary>
        /// ブックマーク一覧。
        /// </summary>
        public IReadOnlyList<SmileVideoBookmarkNodeViewModel> BookmarkItems
        {
            get
            {
                if(this._bookmarkItems == null) {
                    var treeItems = Mediation.ManagerPack.SmileManager.VideoManager.BookmarkManager.NodeItems;
                    this._bookmarkItems = SmileVideoBookmarkUtility.ConvertFlatBookmarkItems(treeItems);
                }

                return this._bookmarkItems;
            }
        }

        /// <summary>
        /// コメントが選択されているか。
        /// </summary>
        public bool IsSelectedComment
        {
            get { return this._isSelectedComment; }
            set { SetVariableValue(ref this._isSelectedComment, value); }
        }
        /// <summary>
        /// コメント表示制限数の有効状態。
        /// </summary>
        public bool IsEnabledDisplayCommentLimit
        {
            get { return Setting.Player.IsEnabledDisplayCommentLimit; }
            set { SetPropertyValue(Setting.Player, value); }
        }
        /// <summary>
        /// コメント表示制限数。
        /// </summary>
        public int DisplayCommentLimitCount
        {
            get { return Setting.Player.DisplayCommentLimitCount; }
            set { SetPropertyValue(Setting.Player, value); }
        }
        /// <summary>
        /// 動画情報欄選択状態。
        /// </summary>
        public bool IsSelectedInformation
        {
            get { return this._isSelectedInformation; }
            set { SetVariableValue(ref this._isSelectedInformation, value); }
        }
        /// <summary>
        /// メディア(動画ファイルとか)がプレイヤーに設定されているか。
        /// </summary>
        public bool IsSettedMedia
        {
            get { return this._isSettedMedia; }
            set { SetVariableValue(ref this._isSettedMedia, value); }
        }

        /// <summary>
        /// コメント有効表示領域の比率。
        /// </summary>
        public double CommentEnabledPercent
        {
            get { return this._commentEnabledPercent; }
            set
            {
                if(SetVariableValue(ref this._commentEnabledPercent, value)) {
                    ChangedEnabledCommentPercent();
                }
            }
        }

        /// <summary>
        /// コメントのテキスト描画方法。
        /// </summary>
        public TextShowKind PlayerTextShowKind
        {
            get { return Setting.Player.TextShowKind; }
            set
            {
                if(SetPropertyValue(Setting.Player, value, nameof(Setting.Player.TextShowKind))) {
                    ChangedCommentContent();
                }

            }
        }

        /// <summary>
        /// コメント有効表示領域の高さ。
        /// </summary>
        public double CommentEnabledHeight
        {
            get { return this._commentEnabledHeight; }
            set { SetVariableValue(ref this._commentEnabledHeight, value); }
        }

        /// <summary>
        /// コメント有効表示領域のプレビューを表示するか。
        /// </summary>
        public bool ShowEnabledCommentPreviewArea
        {
            get { return this._showEnabledCommentPreviewArea; }
            set { SetVariableValue(ref this._showEnabledCommentPreviewArea, value); }
        }

        /// <summary>
        /// 動画投稿者もコメント有効領域に従うか。
        /// </summary>
        public bool IsEnabledOriginalPosterCommentArea
        {
            get { return this._isEnabledOriginalPosterCommentArea; }
            set { SetVariableValue(ref this._isEnabledOriginalPosterCommentArea, value); }
        }

        /// <summary>
        /// コメントのフォント名。
        /// </summary>
        public FontFamily CommentFontFamily
        {
            get { return FontUtility.MakeFontFamily(Setting.Comment.FontFamily, SystemFonts.MessageFontFamily); }
            set
            {
                if(SetPropertyValue(Setting.Comment, FontUtility.GetOriginalFontFamilyName(value), nameof(Setting.Comment.FontFamily))) {
                    ChangedCommentFont();
                }
            }
        }

        /// <summary>
        /// コメントのフォントは太字か。
        /// </summary>
        public bool CommentFontBold
        {
            get { return Setting.Comment.FontBold; }
            set
            {
                if(SetPropertyValue(Setting.Comment, value, nameof(Setting.Comment.FontBold))) {
                    ChangedCommentFont();
                }
            }
        }

        /// <summary>
        /// コメントのフォントは斜体か。
        /// </summary>
        public bool CommentFontItalic
        {
            get { return Setting.Comment.FontItalic; }
            set
            {
                if(SetPropertyValue(Setting.Comment, value, nameof(Setting.Comment.FontItalic))) {
                    ChangedCommentFont();
                }
            }
        }

        /// <summary>
        /// コメントのフォントサイズ。
        /// </summary>
        public double CommentFontSize
        {
            get { return Setting.Comment.FontSize; }
            set
            {
                if(SetPropertyValue(Setting.Comment, value, nameof(Setting.Comment.FontSize))) {
                    ChangedCommentFont();
                }
            }
        }

        /// <summary>
        /// コメントの透明度。
        /// </summary>
        public double CommentFontAlpha
        {
            get { return Setting.Comment.FontAlpha; }
            set
            {
                if(SetPropertyValue(Setting.Comment, value, nameof(Setting.Comment.FontAlpha))) {
                    ChangedCommentFont();
                }
            }
        }

        /// <summary>
        /// コメント表示時間。
        /// </summary>
        public TimeSpan CommentShowTime
        {
            get { return Setting.Comment.ShowTime; }
            set
            {
                var prevTime = CommentShowTime;
                if(SetPropertyValue(Setting.Comment, value, nameof(Setting.Comment.ShowTime))) {
                    ChangedCommentShowTime(prevTime);
                }
            }
        }

        /// <summary>
        /// コメント中の円マーク置き換え状態。
        /// </summary>
        public bool CommentConvertPairYenSlash
        {
            get { return Setting.Comment.ConvertPairYenSlash; }
            set
            {
                if(SetPropertyValue(Setting.Comment, value, nameof(Setting.Comment.ConvertPairYenSlash))) {
                    ChangedCommentContent();
                }
            }
        }

        #region post

        /// <summary>
        /// 投稿コメント縦位置。
        /// </summary>
        public SmileVideoCommentVertical PostCommandVertical
        {
            get { return this._postCommandVertical; }
            set
            {
                if(SetVariableValue(ref this._postCommandVertical, value)) {
                    ChangedPostCommands();
                }
            }
        }
        /// <summary>
        /// 投稿コメントサイズ。
        /// </summary>
        public SmileVideoCommentSize PostCommandSize
        {
            get { return this._postCommandSize; }
            set
            {
                if(SetVariableValue(ref this._postCommandSize, value)) {
                    ChangedPostCommands();
                }
            }
        }
        /// <summary>
        /// 投稿コメント色。
        /// </summary>
        public Color PostCommandColor
        {
            get { return this._postCommandColor; }
            set
            {
                if(SetVariableValue(ref this._postCommandColor, value)) {
                    ChangedPostCommands();
                }
            }
        }
        /// <summary>
        /// 投稿コメントは匿名か。
        /// </summary>
        public bool IsPostAnonymous
        {
            get { return Setting.Comment.PostAnonymous; }
            set
            {
                if(SetPropertyValue(Setting.Comment, value, nameof(Setting.Comment.PostAnonymous))) {
                    ChangedPostCommands();
                }
            }
        }
        /// <summary>
        /// 投稿コメントの前につけるコマンド。
        /// </summary>
        public string PostBeforeCommand
        {
            get { return this._postBeforeCommand; }
            set
            {
                if(SetVariableValue(ref this._postBeforeCommand, value)) {
                    ChangedPostCommands();
                }
            }
        }
        /// <summary>
        /// 投稿コメントの後につけるコマンド。
        /// </summary>
        public string PostAfterCommand
        {
            get { return this._postAfterCommand; }
            set
            {
                if(SetVariableValue(ref this._postAfterCommand, value)) {
                    ChangedPostCommands();
                }
            }
        }
        /// <summary>
        /// 投稿コメントのコマンド一覧。
        /// </summary>
        public CollectionModel<string> PostCommandItems { get; } = new CollectionModel<string>();
        /// <summary>
        /// 投稿コメント本文。
        /// </summary>
        public string PostCommentBody
        {
            get { return this._postCommentBody; }
            set { SetVariableValue(ref this._postCommentBody, value); }
        }

        #endregion

        /// <summary>
        /// コメント一覧に投稿時間を表示するか。
        /// </summary>
        public bool PlayerShowPostTimestamp
        {
            get { return Setting.Player.ShowPostTimestamp; }
            set { SetPropertyValue(Setting.Player, value, nameof(Setting.Player.ShowPostTimestamp)); }
        }
        /// <summary>
        /// 全体フィルタ使用状態。
        /// </summary>
        public bool IsEnabledGlobalCommentFilering
        {
            get { return Information?.IsEnabledGlobalCommentFilering ?? Constants.SettingServiceSmileVideoGlobalCommentFileringIsEnabled; }
            set
            {
                if(Information != null) {
                    if(Information.IsEnabledGlobalCommentFilering != value) {
                        Information.IsEnabledGlobalCommentFilering = value;
                        ApprovalComment();
                    }
                }
            }
        }
        /// <summary>
        /// 一秒間にダウンロードできたデータサイズ。
        /// </summary>
        public long SecondsDownloadingSize
        {
            get { return this._secondsDownloadingSize; }
            set { SetVariableValue(ref this._secondsDownloadingSize, value); }
        }
        /// <summary>
        /// コメント投稿に関する情報。
        /// </summary>
        public string CommentInformation
        {
            get { return this._commentInformation; }
            set { SetVariableValue(ref this._commentInformation, value); }
        }
        /// <summary>
        /// 通常ウィンドウ状態か。
        /// <para>これだけ変えても変な挙動になるだけ。<see cref="SetWindowMode(bool)"/で切り替える前提。></para>
        /// </summary>
        public bool IsNormalWindow
        {
            get { return this._isNormalWindow; }
            set
            {
                if(SetVariableValue(ref this._isNormalWindow, value)) {
                    if(IsNormalWindow) {
                        ResizeBorderThickness = enabledResizeBorderThickness;
                        //重複
                        if(State == WindowState.Maximized) {
                            WindowBorderThickness = maximumWindowBorderThickness;
                            State = WindowState.Normal;
                        } else {
                            WindowBorderThickness = normalWindowBorderThickness;
                        }
                    } else {
                        ResizeBorderThickness = new Thickness(0);
                        WindowBorderThickness = new Thickness(0);
                    }
                    CallOnPropertyChange(nameof(PlayerShowCommentArea));
                }
            }
        }
        /// <summary>
        /// リサイズ幅。
        /// </summary>
        public Thickness ResizeBorderThickness
        {
            get { return this._resizeBorderThickness; }
            set { SetVariableValue(ref this._resizeBorderThickness, value); }
        }
        /// <summary>
        /// ウィンドウ枠幅。
        /// </summary>
        public Thickness WindowBorderThickness
        {
            get { return this._windowBorderThickness; }
            set { SetVariableValue(ref this._windowBorderThickness, value); }
        }
        /// <summary>
        /// コメントグラフ表示状態。
        /// </summary>
        public bool ShowCommentChart
        {
            get { return this._showCommentChart; }
            set { SetVariableValue(ref this._showCommentChart, value); }
        }
        /// <summary>
        /// コメントグラフデータ。
        /// </summary>
        public CollectionModel<DataPoint> CommentChartList { get; } = new CollectionModel<DataPoint>();

        #endregion

        #region command

        public ICommand OpenLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        Mediation.Logger.Warning($"{o}");
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
                        LoadAsync(videoInformation, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ConfigureAwait(false);
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
                                        StopMovie(true);
                                        SetMedia();
                                        PlayMovie();
                                        break;

                                    default:
                                        //Player.BeginStop(() => {
                                        //    Player.Play();
                                        //});
                                        StopMovie(true);
                                        PlayMovie();
                                        break;
                                }
                                return;

                            case PlayerState.Playing:
                                Player.PauseOrResume();
                                return;

                            case PlayerState.Pause:
                                if(IsBufferingStop) {
                                    Mediation.Logger.Debug("restart");
                                    SetMedia();
                                    PlayMovie().Task.ContinueWith(task => {
                                        Player.Position = VideoPosition;
                                    });
                                } else {
                                    Mediation.Logger.Debug("resume");
                                    View.Dispatcher.BeginInvoke(new Action(() => {
                                        Player.PauseOrResume();
                                    }));
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
                        StopMovie(true);
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

        public ICommand OpenUserOrChannelIdCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(IsChannelVideo) {
                            Mediation.Logger.Debug("チャンネルを開く処理は未実装");
                        } else {
                            OpenUserId(UserId);
                        }
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
                        if(Information.CacheDirectory.Exists) {
                            Process.Start(Information.CacheDirectory.FullName);
                        }
                    }
                );
            }
        }

        public ICommand AddMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var myListFinder = o as SmileVideoMyListFinderViewModelBase;
                        AddMyListAsync(myListFinder).ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand AddBookmarkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var bookmarkNode = o as SmileVideoBookmarkNodeViewModel;
                        AddBookmark(bookmarkNode);
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
                        LoadAsync(selectVideoInformation, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ConfigureAwait(false);
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
                        if(PlayListItems.CanItemChange) {
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
                        if(PlayListItems.CanItemChange) {
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
                    o => {
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

        public ICommand ResetCommentSettingCommand
        {
            get { return CreateCommand(o => ResetCommentSetting()); }
        }

        public ICommand PostCommentCommand
        {
            get { return CreateCommand(o => PostCommentAsync(PlayTime).ConfigureAwait(false)); }
        }

        public ICommand CloseCommentInformationCommand
        {
            get
            {
                return CreateCommand(o => {
                    CommentInformation = string.Empty;
                });
            }
        }

        public ICommand SwitchFullScreenCommand
        {
            get { return CreateCommand(o => SwitchFullScreen()); }
        }

        public ICommand FullScreenCancelCommand
        {
            get
            {
                return CreateCommand(o => {
                    if(IsNormalWindow) {
                        return;
                    }
                    // フルスクリーン時は元に戻してあげる
                    SetWindowMode(true);
                });
            }
        }

        #endregion

        #region function

        public Task LoadAsync(IEnumerable<SmileVideoInformationViewModel> videoInformations, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            PlayListItems.AddRange(videoInformations);
            return LoadAsync(PlayListItems.GetFirstItem(), false, thumbCacheSpan, imageCacheSpan);
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
                var realScale = RealVideoHeight / CommentAreaHeight;
                CommentAreaWidth *= realScale;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        void SetMedia()
        {
            if(!IsSettedMedia && !IsViewClosed) {
                Player.LoadMedia(PlayFile.FullName);
                IsSettedMedia = true;
            }
        }

        void StartIfAutoPlay()
        {
            if(Setting.Player.AutoPlay && !IsViewClosed) {
                SetMedia();
                PlayMovie();
            }
        }

        DispatcherOperation PlayMovie()
        {
            Player.IsMute = IsMute;
            Player.Volume = Volume;
            return View.Dispatcher.BeginInvoke(new Action(() => {
                ClearComment();
                if(!IsViewClosed) {
                    Player.Play();
                }
            }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        void StopMovie(bool isStopComment)
        {
            Mediation.Logger.Debug("stop");
            if(IsSettedMedia && !IsViewClosed) {
                Player.Stop();
            }
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
            if(isStopComment) {
                ClearComment();
            }
            PrevPlayedTime = TimeSpan.Zero;
        }

        void ClearComment()
        {
            foreach(var data in ShowingCommentList.ToArray()) {
                data.Clock.Controller.SkipToFill();
                data.Clock.Controller.Remove();
            }
            ShowingCommentList.Clear();
        }

        /// <summary>
        /// 動画再生位置を移動。
        /// </summary>
        /// <param name="targetPosition">動画再生位置。</param>
        void MoveVideoPosition(float targetPosition)
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


        void FireShowComments()
        {
            //Mediation.Logger.Trace($"{VideoId}: {PrevPlayedTime} - {PlayTime}, {Player.ActualWidth}x{Player.ActualHeight}");

            SmileVideoCommentUtility.FireShowCommentsCore(NormalCommentArea, GetCommentArea(false), PrevPlayedTime, PlayTime, NormalCommentList, ShowingCommentList, Setting);
            SmileVideoCommentUtility.FireShowCommentsCore(OriginalPosterCommentArea, GetCommentArea(true), PrevPlayedTime, PlayTime, OriginalPosterCommentList, ShowingCommentList, Setting);
        }

        Size GetCommentArea(bool OriginalPoster)
        {
            if(OriginalPoster && !IsEnabledOriginalPosterCommentArea) {
                return new Size(OriginalPosterCommentArea.ActualWidth, OriginalPosterCommentArea.ActualHeight);
            }

            return new Size(NormalCommentArea.ActualWidth, CommentEnabledHeight);
        }

        void ScrollCommentList()
        {
            if(!IsAutoScroll) {
                return;
            }
            var nowTimelineItem = CommentItems
                .Cast<SmileVideoCommentViewModel>()
                .FirstOrDefault(c => SmileVideoCommentUtility.InShowTime(c, PrevPlayedTime, PlayTime))
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
            return Information.LoadRelationVideosAsync(Constants.ServiceSmileVideoRelationCacheSpan).ContinueWith(task => {
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
            Debug.Assert(Information.PageHtmlLoadState == LoadState.Loaded);

            IsCheckedTagPedia = true;

            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            htmlDocument.LoadHtml(Information.WatchPageHtmlSource);
            var json = SmileVideoWatchAPIUtility.ConvertJsonFromWatchPage(Information.WatchPageHtmlSource);
            var videoDetail = json?.SelectToken("videoDetail");
            var tagList = videoDetail?.SelectToken("tagList");
            if(tagList != null) {
                var map = TagItems.ToDictionary(tk => tk.TagName, tv => tv);
                foreach(var tagItem in tagList) {
                    var tagName = tagItem.Value<string>("tag");
                    var hasDic = tagItem.Value<string>("dic");
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
            IsMadeDescription = true;

            var flowDocumentSource = SmileVideoDescriptionUtility.ConvertFlowDocumentFromHtml(Mediation, Information.DescriptionHtmlSource);
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

            ChangedEnabledCommentPercent();
        }

        void AddBookmark(SmileVideoBookmarkNodeViewModel bookmarkNode)
        {
            var videoItem = Information.ToVideoItemModel();
            bookmarkNode.VideoItems.Add(videoItem);
        }

        Task<SmileJsonResultModel> AddMyListAsync(SmileVideoMyListFinderViewModelBase myListFinder)
        {
            var defaultMyListFinder = myListFinder as SmileVideoAccountMyListDefaultFinderViewModel;
            if(defaultMyListFinder != null) {
                return AddAccountDefaultMyListAsync(defaultMyListFinder);
            } else {
                return AddAccountMyListAsync(myListFinder);
            }
        }

        Task<SmileJsonResultModel> AddAccountDefaultMyListAsync(SmileVideoAccountMyListDefaultFinderViewModel defaultMyListFinder)
        {
            var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            var myList = new Logic.Service.Smile.Api.V1.MyList(Mediation);
            return myList.AdditionAccountDefaultMyListFromVideo(VideoId, Information.PageVideoToken);
        }

        Task<SmileJsonResultModel> AddAccountMyListAsync(SmileVideoMyListFinderViewModelBase myListFinder)
        {
            var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            var myList = new Logic.Service.Smile.Api.V1.MyList(Mediation);
            return myList.AdditionAccountMyListFromVideo(myListFinder.MyListId, Information.ThreadId, Information.PageVideoToken);
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
            var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));
            return Mediation.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request).ContinueWith(t => {
                var videoInformation = t.Result;
                return LoadAsync(videoInformation, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
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

        void ApprovalCommentCustomOverlap(bool ignoreOverlapWord, TimeSpan ignoreOverlapTime)
        {
            if(!ignoreOverlapWord) {
                return;
            }

            var groupingComments = CommentList
                .Where(c => c.Approval)
                .OrderBy(c => c.ElapsedTime)
                .GroupBy(c => c.Content.Trim())
            ;
            foreach(var groupingComment in groupingComments) {
                var length = groupingComment.Count();
                for(var i = 1; i < length; i++) {
                    var prevItem = groupingComment.ElementAt(i - 1);
                    var nowItem = groupingComment.ElementAt(i);
                    if(nowItem.ElapsedTime - prevItem.ElapsedTime <= ignoreOverlapTime) {
                        nowItem.Approval = false;
                    }
                }
            }
        }

        void ApprovalCommentCustomDefineKeys(IReadOnlyList<string> defineKeys)
        {
            if(!defineKeys.Any()) {
                return;
            }

            var filterList = defineKeys
                .Join(Mediation.Smile.VideoMediation.Filtering.Elements, d => d, e => e.Key, (d, e) => e)
                .Select(e => new SmileVideoCommentFilteringItemSettingModel() {
                    Target = SmileVideoCommentFilteringTarget.Comment,
                    IgnoreCase = RawValueUtility.ConvertBoolean(e.Extends["ignore-case"]),
                    Type = FilteringType.Regex,
                    Source = e.Extends["pattern"],
                })
                .ToList()
            ;

            ApprovalCommentCustomFilterItems(filterList);
        }

        void ApprovalCommentCustomFilterItems(IReadOnlyList<SmileVideoCommentFilteringItemSettingModel> filterList)
        {
            if(!filterList.Any()) {
                return;
            }

            var filters = filterList.Select(f => new SmileVideoCommentFiltering(f));
            foreach(var filter in filters.AsParallel()) {
                foreach(var item in CommentList.AsParallel().Where(c => c.Approval)) {
                    item.Approval = !filter.Check(item.Content, item.UserId, item.Commands);
                }
            }
        }

        private void ApprovalCommentCustom(SmileVideoFilteringViweModel filter)
        {
            ApprovalCommentCustomOverlap(filter.IgnoreOverlapWord, filter.IgnoreOverlapTime);
            ApprovalCommentCustomDefineKeys(filter.DefineKeys);
            ApprovalCommentCustomFilterItems(filter.CommentFilterList.ModelList);
        }

        void ApprovalComment()
        {
            ApprovalCommentSet(CommentList, true);

            ApprovalCommentSharedNoGood();

            ApprovalCommentCustom(LocalCommentFilering);
            if(IsEnabledGlobalCommentFilering) {
                ApprovalCommentCustom(GlobalCommentFilering);
            }
        }

        void ClearSelectedComment()
        {
            SelectedComment = null;
        }

        Task LoadNextPlayListItemAsync()
        {
            var targetViewModel = PlayListItems.ChangeNextItem();
            return LoadAsync(targetViewModel, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        Task LoadPrevPlayListItemAsync()
        {
            var targetViewModel = PlayListItems.ChangePrevItem();
            return LoadAsync(targetViewModel, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        void AddHistory(SmileVideoPlayHistoryModel historyModel)
        {
            Setting.History.Insert(0, historyModel);
        }

        double GetEnabledCommentHeight(double baseHeight, double percent)
        {
            return baseHeight / 100.0 * percent;
        }

        void ChangedEnabledCommentPercent()
        {
            CommentEnabledHeight = GetEnabledCommentHeight(NormalCommentArea.ActualHeight, CommentEnabledPercent);
        }

        void ChangedCommentFont()
        {
            foreach(var comment in CommentList) {
                comment.ChangeFontStyle();
            }
        }

        void ChangedCommentContent()
        {
            foreach(var comment in ShowingCommentList.ToArray()) {
                comment.ViewModel.ChangeActualContent();
                comment.ViewModel.ChangeTextShow();
            }
        }

        void ChangedCommentShowTime(TimeSpan prevTime)
        {
#if false // ややっこしいんじゃふざけんな
            foreach(var comment in ShowingCommentList.ToArray()) {
                var time = comment.Clock.CurrentTime.Value;
                var nowElapsedTime = prevTime - time;
                var percent = nowElapsedTime.TotalMilliseconds / prevTime.TotalMilliseconds;
                var animation = new DoubleAnimation();

                if(comment.ViewModel.Vertical == SmileVideoCommentVertical.Normal) {
                    var nowLeft = Canvas.GetLeft(comment.Element);
                    animation.From = nowLeft;
                    animation.To = -comment.Element.ActualWidth;
                    var useTime = TimeSpan.FromMilliseconds(CommentShowTime.TotalMilliseconds * percent);
                    animation.Duration = new Duration(useTime);

                    //comment.Clock.Controller.Stop();
                    comment.Animation.Duration = CommentShowTime;
                    comment.Element.ApplyAnimationClock(Canvas.LeftProperty, animation.CreateClock());
                }
            }
#endif
        }

        void ResetCommentSetting()
        {
            Setting.Comment.FontFamily = Constants.SettingServiceSmileVideoCommentFontFamily;
            Setting.Comment.FontSize = Constants.SettingServiceSmileVideoCommentFontSize;
            Setting.Comment.FontBold = Constants.SettingServiceSmileVideoCommentFontBold;
            Setting.Comment.FontItalic = Constants.SettingServiceSmileVideoCommentFontItalic;
            Setting.Comment.FontAlpha = Constants.SettingServiceSmileVideoCommentFontAlpha;
            Setting.Comment.ShowTime = Constants.SettingServiceSmileVideoCommentShowTime;
            Setting.Comment.ConvertPairYenSlash = Constants.SettingServiceSmileVideoCommentConvertPairYenSlash;
            Setting.Player.TextShowKind = Constants.SettingServiceSmileVideoPlayerTextShowKind;

            ChangedCommentFont();
            ChangedCommentContent();
            ResetCommentInformation();

            var propertyNames = new[] {
                nameof(CommentFontFamily),
                nameof(CommentFontSize),
                nameof(CommentFontBold),
                nameof(CommentFontItalic),
                nameof(CommentFontAlpha),
                nameof(CommentShowTime),
                nameof(CommentConvertPairYenSlash),
                nameof(PlayerTextShowKind),
                nameof(CommentInformation),
            };
            CallOnPropertyChange(propertyNames);
        }

        IEnumerable<string> CreatePostCommandsCore()
        {
            yield return PostBeforeCommand;

            if(!IsEnabledPostAnonymous) {
                yield return SmileVideoMsgUtility.ConvertRawIsAnonymous(IsPostAnonymous);
            }
            if(PostCommandVertical != SmileVideoCommentVertical.Normal) {
                yield return SmileVideoMsgUtility.ConvertRawVerticalAlign(PostCommandVertical);
            }
            if(PostCommandSize != SmileVideoCommentSize.Medium) {
                yield return SmileVideoMsgUtility.ConvertRawFontSize(PostCommandSize);
            }
            if(PostCommandColor != Colors.White) {
                yield return SmileVideoMsgUtility.ConvertRawForeColor(PostCommandColor);
            }

            yield return PostAfterCommand;
        }

        IEnumerable<string> CreatePostCommands()
        {
            return CreatePostCommandsCore()
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
            ;
        }

        void ChangedPostCommands()
        {
            var commands = CreatePostCommands();
            PostCommandItems.InitializeRange(commands);
        }

        async Task PostCommentAsync(TimeSpan videoPosition)
        {
            if(CommentThread == null) {
                var rawMessagePacket = await LoadMsgCoreAsync(0, 0, 0, 0, 0);
                ImportCommentThread(rawMessagePacket);
            }

            var commentCount = RawValueUtility.ConvertInteger(CommentThread.LastRes ?? "0");
            Debug.Assert(CommentThread.Thread == Information.ThreadId);

            var msg = new Msg(Mediation);

            var postKey = await msg.LoadPostKeyAsync(Information.ThreadId, commentCount);
            if(postKey == null) {
                SetCommentInformation(Properties.Resources.String_App_Define_Service_Smile_Video_Comment_PostKeyError);
                return;
            } else {
                Mediation.Logger.Information($"postkey = {postKey}");
            }

            var resultPost = await msg.PostAsync(
                Information.MessageServerUrl,
                Information.ThreadId,
                videoPosition,
                CommentThread.Ticket,
                postKey.PostKey,
                PostCommandItems,
                PostCommentBody
            );
            if(resultPost == null) {
                SetCommentInformation($"fail: {nameof(msg.PostAsync)}");
                return;
            }
            var status = SmileVideoMsgUtility.ConvertResultStatus(resultPost.ChatResult.Status);
            if(status != SmileVideoCommentResultStatus.Success) {
                //TODO: ユーザー側に通知
                var message = DisplayTextUtility.GetDisplayText(status);
                SetCommentInformation($"fail: {message}, {status}");
                return;
            }

            //投稿コメントを画面上に流す
            //TODO: 投稿者判定なし

            var commentModel = new RawSmileVideoMsgChatModel() {
                //Anonymity = SmileVideoCommentUtility.GetIsAnonymous(PostCommandItems)
                Mail = string.Join(" ", PostCommandItems),
                Content = PostCommentBody,
                Date = RawValueUtility.ConvertRawUnixTime(DateTime.Now).ToString(),
                No = resultPost.ChatResult.No,
                VPos = SmileVideoMsgUtility.ConvertRawElapsedTime(videoPosition),
                Thread = resultPost.ChatResult.Thread,
            };
            var commentViewModel = new SmileVideoCommentViewModel(commentModel, Setting) {
                IsMyPost = true,
                Approval = true,
            };
            SmileVideoCommentUtility.FireShowSingleComment(commentViewModel, NormalCommentArea, GetCommentArea(false), PrevPlayedTime, ShowingCommentList, Setting);

            NormalCommentList.Add(commentViewModel);
            CommentList.Add(commentViewModel);
            CommentItems.Refresh();

            ResetCommentInformation();

            // コメント再取得
            await LoadMsgAsync(CacheSpan.NoCache);
        }

        void SetCommentInformation(string text)
        {
            CommentInformation = text;
        }

        void ResetCommentInformation()
        {
            CommentInformation = string.Empty;
            PostCommentBody = string.Empty;
        }

        void SetLocalFiltering()
        {
            if(View != null) {
                View.localFilter.Filtering = LocalCommentFilering;
            }
        }

        void SetNavigationbarBaseEvent(Navigationbar navigationbar)
        {
            navigationbar.seekbar.PreviewMouseDown += VideoSilder_PreviewMouseDown;
            navigationbar.seekbar.MouseEnter += Seekbar_MouseEnter;
            navigationbar.seekbar.MouseLeave += Seekbar_MouseLeave;
        }

        void UnsetNavigationbarBaseEvent(Navigationbar navigationbar)
        {
            navigationbar.seekbar.PreviewMouseDown -= VideoSilder_PreviewMouseDown;
            navigationbar.seekbar.MouseEnter -= Seekbar_MouseEnter;
            navigationbar.seekbar.MouseLeave -= Seekbar_MouseLeave;
        }

        void SwitchFullScreen()
        {
            SetWindowMode(!IsNormalWindow);
        }

        void SetWindowMode(bool toNormalWindow)
        {
            IsNormalWindow = toNormalWindow;
            var hWnd = HandleUtility.GetWindowHandle(View);
            if(toNormalWindow) {
                View.Deactivated -= View_Deactivated;

                var logicalViewArea = new Rect(Left, Top, Width, Height);
                var deviceViewArea = UIUtility.ToLogicalPixel(View, logicalViewArea);
                var podRect = PodStructUtility.Convert(deviceViewArea);
                NativeMethods.MoveWindow(hWnd, podRect.Left, podRect.Top, podRect.Width, podRect.Height, true);
            } else {
                View.Deactivated += View_Deactivated;

                var podRect = PodStructUtility.Convert(Screen.PrimaryScreen.DeviceBounds);
                NativeMethods.MoveWindow(hWnd, podRect.Left, podRect.Top, podRect.Width, podRect.Height, true);
            }
        }

        #endregion

        #region SmileVideoDownloadViewModel

        public override Task LoadAsync(SmileVideoInformationViewModel videoInformation, bool forceEconomy, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            // TODO:forceEconomyは今のところ無効

            foreach(var item in PlayListItems.Where(i => i != videoInformation)) {
                item.IsPlaying = false;
            }
            videoInformation.IsPlaying = true;
            IsSelectedInformation = true;

            // プレイヤー立ち上げ時は null
            if(Information != null) {
                Information.SaveSetting(false);
                // 軽めにGC
                Mediation.Order(new AppCleanMemoryOrderModel(false));
            }

            var historyModel = Setting.History.FirstOrDefault(f => f.VideoId == videoInformation.VideoId);
            if(historyModel == null) {
                historyModel = new SmileVideoPlayHistoryModel() {
                    VideoId = videoInformation.VideoId,
                    VideoTitle = videoInformation.Title,
                    Length = videoInformation.Length,
                    FirstRetrieve = videoInformation.FirstRetrieve,
                };
            } else {
                Setting.History.Remove(historyModel);
            }
            historyModel.WatchUrl = videoInformation.WatchUrl;
            historyModel.LastTimestamp = DateTime.Now;
            historyModel.Count = RangeUtility.Increment(historyModel.Count);

            AddHistory(historyModel);

            var later = Setting.CheckItLater
                .Where(c => !c.IsChecked)
                .FirstOrDefault(c => c.VideoId == videoInformation.VideoId)
            ;
            if(later != null) {
                later.IsChecked = true;
                later.CheckTimestamp = DateTime.Now;
            }

            return base.LoadAsync(videoInformation, false, thumbCacheSpan, imageCacheSpan);
        }

        protected override void OnDownloadStart(object sender, DownloadStartEventArgs e)
        {
            if(!IsMadeDescription) {
                MakeDescription();
            }
            if(!IsCheckedTagPedia) {
                CheckTagPedia();
            }

            base.OnDownloadStart(sender, e);
        }

        protected override void OnDownloading(object sender, DownloadingEventArgs e)
        {
            if(Information.MovieType != SmileVideoMovieType.Swf) {
                if(!CanVideoPlay) {
                    // とりあえず待って
                    VideoFile.Refresh();
                    CanVideoPlay = VideoPlayLowestSize < VideoFile.Length;
                    if(CanVideoPlay) {
                        StartIfAutoPlay();
                    }
                }
            }
            e.Cancel = IsViewClosed || IsProcessCancel;
            if(e.Cancel) {
                StopMovie(true);
            }
            SecondsDownloadingSize = e.SecondsDownlodingSize;

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
                if(Information.PageHtmlLoadState == LoadState.Loaded) {
                    if(!IsMadeDescription) {
                        MakeDescription();
                    }
                    if(!IsCheckedTagPedia) {
                        CheckTagPedia();
                    }
                }

                if(Information.MovieType == SmileVideoMovieType.Swf && !Information.ConvertedSwf) {
                    // 変換が必要
                    var ffmpeg = new Ffmpeg();
                    var s = $"-i \"{VideoFile.FullName}\" -vcodec flv \"{PlayFile.FullName}\"";
                    ffmpeg.ExecuteAsync(s).ContinueWith(task => {
                        if(!IsViewClosed) {
                            Information.ConvertedSwf = true;
                            Information.SaveSetting(true);
                            ResetSwf();
                            CanVideoPlay = true;
                            StartIfAutoPlay();
                        }
                    });
                } else {
                    if(Information.MovieType == SmileVideoMovieType.Swf) {
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
            var comments = SmileVideoCommentUtility.CreateCommentViewModels(rawMsgPacket, Setting);
            CommentList.InitializeRange(comments);
            CommentListCount = CommentList.Count;
            ApprovalComment();

            var chartItems = SmileVideoCommentUtility.CreateCommentChartItems(CommentList, TotalTime);
            CommentChartList.InitializeRange(chartItems);

            NormalCommentList.InitializeRange(CommentList.Where(c => !c.IsOriginalPoster));
            var userSequence = SmileVideoCommentUtility.CreateFilteringUserItems(NormalCommentList);
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
            ChangedPostCommands();

            if(!PlayListItems.Any()) {
                PlayListItems.Add(Information);
            }
            if(Session.IsPremium && CommandColorItems.Count == SmileVideoMsgUtility.normalCommentColors.Length) {
                CommandColorItems.AddRange(SmileVideoMsgUtility.premiumCommentColors);
            }

            TotalTime = Information.Length;

            TagItems.InitializeRange(Information.TagList);

            LoadRelationVideoAsync();

            var propertyNames = new[] {
                nameof(VideoId),
                nameof(Title),
                nameof(IsEnabledPostAnonymous),
                nameof(Information),
                nameof(UserName),
                nameof(ChannelName),
                nameof(IsChannelVideo),
            };
            CallOnPropertyChange(propertyNames);

            LocalCommentFilering = new SmileVideoFilteringViweModel(Information.Filtering, null, Mediation.Smile.VideoMediation.Filtering);
            SetLocalFiltering();

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
            IsMadeDescription = false;
            IsCheckedTagPedia = false;
            ChangingVideoPosition = false;
            MovingSeekbarThumb = false;
            CanVideoPlay = false;
            IsVideoPlayng = false;
            PlayTime = TimeSpan.Zero;
            TotalTime = TimeSpan.Zero;
            SelectedComment = null;
            IsSettedMedia = false;
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
            if(Player != null) {
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
            Player = View.player;//.MediaPlayer;
            NormalCommentArea = View.normalCommentArea;
            OriginalPosterCommentArea = View.originalPosterCommentArea;
            Navigationbar = View.seekbar;
            CommentView = View.commentView;
            DetailComment = View.detailComment;
            DocumentDescription = View.documentDescription;

            // 初期設定
            Player.Volume = Volume;
            SetLocalFiltering();

            // あれこれイベント
            var content = Navigationbar.ExstendsContent as Panel;
            EnabledCommentSlider = UIUtility.FindLogicalChildren<Slider>(content).First();

            EnabledCommentSlider.MouseEnter += EnabledCommentSlider_MouseEnter;
            EnabledCommentSlider.MouseLeave += EnabledCommentSlider_MouseLeave;

            View.Loaded += View_Loaded;
            View.Closing += View_Closing;
            Player.PositionChanged += Player_PositionChanged;
            Player.SizeChanged += Player_SizeChanged;
            Player.StateChanged += Player_StateChanged;
            SetNavigationbarBaseEvent(Navigationbar);
            DetailComment.LostFocus += DetailComment_LostFocus;
        }

        #endregion

        #region ICloseView

        public void Close()
        {
            if(!IsViewClosed) {
                View.Close();
            }
        }

        #endregion

        #region ICaptionCommand

        public WindowState State
        {
            get { return this._state; }
            set
            {
                if(SetVariableValue(ref this._state, value)) {
                    if(State == WindowState.Maximized) {
                        WindowBorderThickness = maximumWindowBorderThickness;
                    } else {
                        //if(!IsNormalWindow) {
                        //    SetWindowMode(false);
                        //}
                        WindowBorderThickness = normalWindowBorderThickness;
                    }
                }
            }
        }

        public ICommand CaptionMinimumCommand { get { return CreateCommand(o => State = WindowState.Minimized); } }
        public ICommand CaptionMaximumCommand { get { return CreateCommand(o => State = WindowState.Maximized); } }
        public ICommand CaptionRestoreCommand { get { return CreateCommand(o => State = WindowState.Normal); } }
        public ICommand CaptionCloseCommand { get { return CreateCommand(o => View.Close()); } }

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
            //TODO: closingはまずくね…?

            View.Loaded -= View_Loaded;
            View.Closing -= View_Closing;
            Player.PositionChanged -= Player_PositionChanged;
            Player.SizeChanged -= Player_SizeChanged;
            Player.StateChanged -= Player_StateChanged;
            UnsetNavigationbarBaseEvent(Navigationbar);

            if(EnabledCommentSlider != null) {
                EnabledCommentSlider.MouseEnter -= EnabledCommentSlider_MouseEnter;
                EnabledCommentSlider.MouseLeave -= EnabledCommentSlider_MouseLeave;
            }

            View.Deactivated -= View_Deactivated;

            IsViewClosed = true;

            Information.IsPlaying = false;

            Information.SaveSetting(true);

            if(Player.State == Meta.Vlc.Interop.Media.MediaState.Playing) {
                //Player.BeginStop();
                StopMovie(true);
            }

            try {
                Player.Dispose();
            } catch(Exception ex) {
                Mediation.Logger.Error(ex);
            }
            Mediation.Order(new AppCleanMemoryOrderModel(true));
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
                FireShowComments();
                ScrollCommentList();
                PrevPlayedTime = PlayTime;
            }
        }

        private void VideoSilder_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(!CanVideoPlay) {
                return;
            }

            var seekbar = (Slider)sender;
            seekbar.CaptureMouse();

            ChangingVideoPosition = true;
            SeekbarMouseDownPosition = e.GetPosition(seekbar);
            seekbar.PreviewMouseUp += VideoSilder_PreviewMouseUp;
            seekbar.MouseMove += Seekbar_MouseMove;

            var tempPosition = SeekbarMouseDownPosition.X / seekbar.ActualWidth;
            seekbar.Value = tempPosition;
        }

        private void Seekbar_MouseMove(object sender, MouseEventArgs e)
        {
            MovingSeekbarThumb = true;

            Debug.Assert(ChangingVideoPosition);

            var seekbar = (Slider)sender;
            var movingPosition = e.GetPosition(seekbar);

            var tempPosition = movingPosition.X / seekbar.ActualWidth;
            seekbar.Value = tempPosition;
        }

        private void VideoSilder_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Debug.Assert(CanVideoPlay);

            var seekbar = (Slider)sender;

            seekbar.PreviewMouseUp -= VideoSilder_PreviewMouseUp;
            seekbar.MouseMove -= Seekbar_MouseMove;


            //#82: これなんの処理だ、わからん
            //float nextPosition;
            //if(!MovingSeekbarThumb) {
            //    var pos = e.GetPosition(seekbar);
            //    nextPosition = (float)(pos.X / seekbar.ActualWidth);
            //} else {
            //    nextPosition = (float)((Navigationbar)seekbar.Parent).VideoPosition;
            //}
            var pos = e.GetPosition(seekbar);
            var nextPosition = (float)(pos.X / seekbar.ActualWidth);
            // TODO: 読み込んでない部分は移動不可にする
            MoveVideoPosition(nextPosition);

            ChangingVideoPosition = false;
            MovingSeekbarThumb = false;

            seekbar.ReleaseMouseCapture();
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

            Mediation.Logger.Debug($"{VideoId}: {e.Value}, pos: {VideoPosition}, time: {PlayTime} / {Player.Length}");
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
                        //Player.Stop();
                        //Player.Play();
                        StopMovie(true);
                        PlayMovie();
                    } else {
                        //PlayerState = PlayerState.Stop;
                        //VideoPosition = 0;
                        //ClearComment();
                        //PrevPlayedTime = TimeSpan.Zero;
                        StopMovie(false);
                    }
                    break;

                default:
                    break;
            }
        }

        //private void EnabledCommentPopup_Opened(object sender, EventArgs e)
        //{
        //    ShowEnabledCommentPreviewArea = true;
        //}

        //private void EnabledCommentPopup_Closed(object sender, EventArgs e)
        //{
        //    ShowEnabledCommentPreviewArea = false;
        //}

        private void EnabledCommentSlider_MouseEnter(object sender, MouseEventArgs e)
        {
            ShowEnabledCommentPreviewArea = true;
        }

        private void EnabledCommentSlider_MouseLeave(object sender, MouseEventArgs e)
        {
            ShowEnabledCommentPreviewArea = false;
        }

        private void DetailComment_LostFocus(object sender, RoutedEventArgs e)
        {
            //TODO: コメント詳細部の挙動はまぁどうでもいいや、あとまわし
            //ClearSelectedComment();
        }

        private void View_Deactivated(object sender, EventArgs e)
        {
            SwitchFullScreen();
        }

        private void Seekbar_MouseEnter(object sender, MouseEventArgs e)
        {
            ShowCommentChart = true;
        }

        private void Seekbar_MouseLeave(object sender, MouseEventArgs e)
        {
            ShowCommentChart = false;
        }

        #endregion
    }
}
