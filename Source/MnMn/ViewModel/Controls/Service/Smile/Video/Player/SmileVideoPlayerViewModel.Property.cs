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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Define.UI.Player;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using MahApps.Metro.Controls;
using Meta.Vlc.Wpf;
using OxyPlot;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    // プロパティ。
    partial class SmileVideoPlayerViewModel
    {
        #region property

        protected virtual SmileVideoPlayerSettingModel PlayerSetting { get { return Setting.Player; } }

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
        protected Canvas NormalCommentArea { get; set; }
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
        Flyout DetailComment { get; set; }
        ///// <summary>
        ///// 動画紹介文書表示要素。
        ///// </summary>
        //FlowDocumentScrollViewer DocumentDescription { get; set; }
        /// <summary>
        /// コメント有効位置抑制スライダー要素。
        /// </summary>
        Control EnabledCommentControl { get; set; }

        public int VolumeMinimum { get { return Constants.NavigatorVolumeRange.Head; } }
        public int VolumeMaximum { get { return Constants.NavigatorVolumeRange.Tail; } }

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

        public FewViewModel<bool> CanPlayNextVieo { get; } = new FewViewModel<bool>(false);

        public FewViewModel<bool> IsWorkingPlayer { get; } = new FewViewModel<bool>(false);

        #region window

        /// <summary>
        /// ウィンドウ X 座標。
        /// </summary>
        public double Left
        {
            get { return this._left; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetVariableValue(ref this._left, value);
                }
            }
        }
        /// <summary>
        /// ウィンドウ Y 座標。
        /// </summary>
        public double Top
        {
            get { return this._top; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetVariableValue(ref this._top, value);
                }
            }
        }
        /// <summary>
        /// ウィンドウ横幅。
        /// </summary>
        public double Width
        {
            get { return this._width; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetVariableValue(ref this._width, value);
                }
            }
        }
        /// <summary>
        /// ウィンドウ高さ。
        /// </summary>
        public double Height
        {
            get { return this._height; }
            set
            {
                if(IsNormalWindow && State == WindowState.Normal) {
                    SetVariableValue(ref this._height, value);
                }
            }
        }
        /// <summary>
        /// 最前面表示状態。
        /// </summary>
        public bool Topmost
        {
            get { return this._topmost; }
            set { SetVariableValue(ref _topmost, value); }
        }

        #endregion

        /// <summary>
        /// 詳細部分を表示状態。
        /// </summary>
        public bool PlayerShowDetailArea
        {
            get { return this._playerShowDetailArea; }
            set { SetVariableValue(ref this._playerShowDetailArea, value); }
        }
        /// <summary>
        /// コメント一覧表示状態。
        /// </summary>
        public bool PlayerShowCommentArea
        {
            get
            {
                return IsNormalWindow
                    ? this._showNormalWindowCommentList
                    : this._showFullScreenCommentList
                ;
            }
            set
            {
                if(IsNormalWindow) {
                    SetVariableValue(ref this._showNormalWindowCommentList, value);
                } else {
                    SetVariableValue(ref this._showFullScreenCommentList, value);
                }
            }
        }

        /// <summary>
        /// コメント表示レイヤー(投稿者・視聴者供に)表示状態。
        /// </summary>
        public bool PlayerVisibleComment
        {
            get { return this._playerVisibleComment; }
            set { SetVariableValue(ref this._playerVisibleComment, value); }
        }

        /// <summary>
        /// コメント一覧自動スクロール状態。
        /// </summary>
        public bool IsAutoScroll
        {
            get { return this._isAutoScroll; }
            set { SetVariableValue(ref this._isAutoScroll, value); }
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
                    if(Player != null) {
                        Player.Volume = Volume;
                    }
                }
            }
        }
        /// <summary>
        /// ミュート状態。
        /// </summary>
        public bool IsMute
        {
            get { return this._isMute; }
            set
            {
                if(SetVariableValue(ref this._isMute, value)) {
                    if(Player != null) {
                        Player.IsMute = IsMute;
                    }
                }
            }
        }


        /// <summary>
        /// 共有NG有効状態。
        /// </summary>
        public bool IsEnabledSharedNoGood
        {
            get { return this._isEnabledSharedNoGood; }
            set
            {
                if(SetVariableValue(ref this._isEnabledSharedNoGood, value)) {
                    if(LocalCommentFilering != null) {
                        ApprovalComment();
                    }
                }
            }
        }
        /// <summary>
        /// 共有NG閾値。
        /// </summary>
        public int SharedNoGoodScore
        {
            get { return this._sharedNoGoodScore; }
            set
            {
                if(SetVariableValue(ref this._sharedNoGoodScore, value)) {
                    if(LocalCommentFilering != null) {
                        ApprovalComment();
                    }
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
        /// 自動再生か。
        /// </summary>
        public bool IsAutoPlay
        {
            get { return Setting.Player.IsAutoPlay; }
        }
        /// <summary>
        /// 初回再生待ちか。
        /// </summary>
        public FewViewModel<bool> WaitingFirstPlay { get; } = new FewViewModel<bool>(false);
        /// <summary>
        /// ユーザー操作で停止されたか。
        /// </summary>
        public FewViewModel<bool> UserOperationStop { get; } = new FewViewModel<bool>(false);

        ///// <summary>
        ///// 投降者コメントが構築されたか。
        ///// </summary>
        //bool IsMadeDescription { get; set; } = false;
        /// <summary>
        ///
        /// </summary>
        protected bool IsCheckedTagPedia { get; set; } = false;

        public ICollectionView CommentItems { get; private set; }
        /// <summary>
        /// コメント全データ。
        /// </summary>
        protected CollectionModel<SmileVideoCommentViewModel> CommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
        /// <summary>
        /// 視聴者コメントデータ。
        /// <para><see cref="CommentList"/>から構築。</para>
        /// </summary>
        protected CollectionModel<SmileVideoCommentViewModel> NormalCommentList { get; } = new CollectionModel<SmileVideoCommentViewModel>();
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
        public SmileVideoFilteringViweModel GlobalCommentFilering { get; protected set; }

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
        /// プレイリスト。
        /// </summary>
        public PlayListManager<SmileVideoInformationViewModel> PlayListItems { get; } = new PlayListManager<SmileVideoInformationViewModel>();
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
        protected TimeSpan PrevPlayedTime { get; set; }

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
                    IsOpenCommentDetail = SelectedComment != null;
                    if(IsOpenCommentDetail) {
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
        /// 動画描画の実描画領域の Viewbox の横幅。
        /// </summary>
        public double BaseWidth
        {
            get { return this._baseWidth; }
            set { SetVariableValue(ref this._baseWidth, value); }
        }
        /// <summary>
        /// 動画描画の実描画領域の Viewbox の高さ。
        /// </summary>
        public double BaseHeight
        {
            get { return this._baseHeight; }
            set { SetVariableValue(ref this._baseHeight, value); }
        }
        /// <summary>
        /// コメント表示領域の横幅。
        /// </summary>
        public double CommentAreaWidth
        {
            get { return this._commentAreaWidth; }
            private set { SetVariableValue(ref this._commentAreaWidth, value); }
        }
        /// <summary>
        /// コメント表示領域の高さ。
        /// </summary>
        public double CommentAreaHeight
        {
            get { return this._commentAreaHeight; }
            private set { SetVariableValue(ref this._commentAreaHeight, value); }
        }

        public FewViewModel<GridLength> PlayerAreaLength { get; } = new FewViewModel<GridLength>();
        public FewViewModel<GridLength> CommentAreaLength { get; } = new FewViewModel<GridLength>();
        public FewViewModel<GridLength> InformationAreaLength { get; } = new FewViewModel<GridLength>();

        public PlayerState PlayerState
        {
            get { return this._playerState; }
            set { SetVariableValue(ref this._playerState, value); }
        }

        /// <summary>
        /// バッファ開始時点での動画位置。
        /// </summary>
        TimeSpan BufferingVideoTime { get; set; }
        TimeSpan SafeShowTime { get; set; }
        long SafeDownloadedSize { get; set; }

        /// <summary>
        /// リプレイ状態。
        /// </summary>
        public bool ReplayVideo
        {
            get { return this._replayVideo; }
            set { SetVariableValue(ref this._replayVideo, value); }
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
        public virtual IReadOnlyList<SmileVideoMyListFinderViewModelBase> AccountMyListItems
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
        public virtual IReadOnlyList<SmileVideoBookmarkNodeViewModel> BookmarkItems
        {
            get
            {
                if(this._bookmarkItems == null) {
                    var treeItems = Mediation.ManagerPack.SmileManager.VideoManager.BookmarkManager.UserNodes;
                    this._bookmarkItems = SmileVideoBookmarkUtility.ConvertFlatBookmarkItems(treeItems);
                }

                return this._bookmarkItems;
            }
        }

        /// <summary>
        /// コメントが選択されているか。
        /// </summary>
        public bool IsOpenCommentDetail
        {
            get { return this._isOpenCommentDetail; }
            set
            {
                if(SetVariableValue(ref this._isOpenCommentDetail, value)) {
                    if(!IsOpenCommentDetail) {
                        SelectedComment = null;
                    }
                }
            }
        }
        /// <summary>
        /// コメント表示制限数の有効状態。
        /// </summary>
        public bool IsEnabledDisplayCommentLimit
        {
            get { return this._isEnabledDisplayCommentLimit; }
            set { SetVariableValue(ref this._isEnabledDisplayCommentLimit, value); }
        }
        /// <summary>
        /// コメント表示制限数。
        /// </summary>
        public int DisplayCommentLimitCount
        {
            get { return this._displayCommentLimitCount; }
            set { SetVariableValue(ref this._displayCommentLimitCount, value); }
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

        protected SmileVideoCommentStyleSettingModel CommentStyleSetting { get; set; }

        /// <summary>
        /// コメントのフォント名。
        /// </summary>
        public FontFamily CommentFontFamily
        {
            get { return FontUtility.MakeFontFamily(CommentStyleSetting.FontFamily, SystemFonts.MessageFontFamily); }
            set
            {
                if(SetPropertyValue(CommentStyleSetting, FontUtility.GetOriginalFontFamilyName(value), nameof(CommentStyleSetting.FontFamily))) {
                    ChangedCommentFont();
                }
            }
        }

        /// <summary>
        /// コメントのフォントは太字か。
        /// </summary>
        public bool CommentFontBold
        {
            get { return CommentStyleSetting.FontBold; }
            set
            {
                if(SetPropertyValue(CommentStyleSetting, value, nameof(CommentStyleSetting.FontBold))) {
                    ChangedCommentFont();
                }
            }
        }

        /// <summary>
        /// コメントのフォントは斜体か。
        /// </summary>
        public bool CommentFontItalic
        {
            get { return CommentStyleSetting.FontItalic; }
            set
            {
                if(SetPropertyValue(CommentStyleSetting, value, nameof(CommentStyleSetting.FontItalic))) {
                    ChangedCommentFont();
                }
            }
        }

        /// <summary>
        /// コメントのフォントサイズ。
        /// </summary>
        public double CommentFontSize
        {
            get { return CommentStyleSetting.FontSize; }
            set
            {
                if(SetPropertyValue(CommentStyleSetting, value, nameof(CommentStyleSetting.FontSize))) {
                    ChangedCommentFont();
                }
            }
        }

        /// <summary>
        /// コメントの透明度。
        /// </summary>
        public double CommentFontAlpha
        {
            get { return CommentStyleSetting.FontAlpha; }
            set
            {
                if(SetPropertyValue(CommentStyleSetting, value, nameof(CommentStyleSetting.FontAlpha))) {
                    ChangedCommentFont();
                }
            }
        }

        public int CommentFps
        {
            get { return CommentStyleSetting.Fps; }
            set
            {
                if(SetPropertyValue(CommentStyleSetting, (int)value, nameof(CommentStyleSetting.Fps))) {
                    ChangedCommentFps();
                }
            }
        }

        /// <summary>
        /// コメント表示時間。
        /// </summary>
        public TimeSpan CommentShowTime
        {
            get { return CommentStyleSetting.ShowTime; }
            set
            {
                var prevTime = CommentShowTime;
                if(SetPropertyValue(CommentStyleSetting, value, nameof(CommentStyleSetting.ShowTime))) {
                    ChangedCommentShowTime(prevTime);
                }
            }
        }

        /// <summary>
        /// コメント中の円マーク置き換え状態。
        /// </summary>
        public bool CommentConvertPairYenSlash
        {
            get { return CommentStyleSetting.ConvertPairYenSlash; }
            set
            {
                if(SetPropertyValue(CommentStyleSetting, value, nameof(CommentStyleSetting.ConvertPairYenSlash))) {
                    ChangedCommentContent();
                }
            }
        }

        /// <summary>
        /// コメントのテキスト描画方法。
        /// </summary>
        public TextShowMode PlayerTextShowMode
        {
            get { return CommentStyleSetting.TextShowMode; }
            set
            {
                if(SetPropertyValue(CommentStyleSetting, value, nameof(CommentStyleSetting.TextShowMode))) {
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
                    CallOnPropertyChange(nameof(PlayerShowCommentArea));
                }
            }
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

        public FewViewModel<Color> VideoBackgroundColor { get; } = new FewViewModel<Color>();

        public BackgroundKind BackgroundKind
        {
            get { return Setting.Player.BackgroundKind; }
        }

        public Color BackgroundColor
        {
            get { return Setting.Player.BackgroundColor; }
        }

        public bool IsEnabledOriginalPosterFilering
        {
            get { return this._isEnabledOriginalPosterFilering; }
            set
            {
                if(SetVariableValue(ref this._isEnabledOriginalPosterFilering, value)) {
                    ApprovalComment();
                }
            }
        }

        public DescriptionBase DescriptionProcessor => new SmileDescription(Mediation);

        #endregion
    }
}
