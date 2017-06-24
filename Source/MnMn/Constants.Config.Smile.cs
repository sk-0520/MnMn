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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.ReadOnly;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Define.UI.Player;

namespace ContentTypeTextNet.MnMn.MnMn
{
    partial class Constants
    {
        #region property

        #region service

        public static string ServiceSmileSessionKey => appConfig.Get("service-smile-session-key");

        /// <summary>
        /// アカウント登録先URI。
        /// </summary>
        public static string ServiceSmileAccountRegister => appConfig.Get("service-smile-account-register");
        /// <summary>
        /// パスワードリセット先URI。
        /// </summary>
        public static string ServiceSmileAccountReset => appConfig.Get("service-smile-account-reset");

        /// <summary>
        /// コンテンツサーチで使用するサービス名。UAみたいなもん。
        /// </summary>
        public static string ServiceSmileContentsSearchContext => appConfig.Get("service-smile-content_search-context");

        public static TimeSpan ServiceSmileUserCheckItLaterCheckTime => appConfig.Get("service-smile-smileuser-check_it_later-check-time", TimeSpan.Parse);

        /// <summary>
        /// マイリスト削除後再読み込みを行う際の待ち時間。
        /// </summary>
        public static TimeSpan ServiceSmileMyListReloadWaitTime => appConfig.Get("service-smile-mylist-reload-wait-time", TimeSpan.Parse);

        public static TimeSpan ServiceSmileUserDataCacheTime = appConfig.Get("service-smile-user-data-cache-time", TimeSpan.Parse);
        public static TimeSpan ServiceSmileUserImageCacheTime = appConfig.Get("service-smile-user-image-cache-time", TimeSpan.Parse);

        public static TimeSpan ServiceSmileMyListCacheTime = appConfig.Get("service-smile-mylist-cache-time", TimeSpan.Parse);

        public static TimeSpan ServiceSmileMarketImageCacheTime = appConfig.Get("service-smile-market-image-cache-time", TimeSpan.Parse);

        /// <summary>
        /// マイリストのタイトルから除外する文字列。
        /// <para>先頭。</para>
        /// </summary>
        public static string ServiceSmileMyListTitleTrimHead => appConfig.Get("service-smile-mylist-title-trim-head");
        /// <summary>
        /// マイリストのタイトルから除外する文字列。
        /// <para>末尾</para>
        /// </summary>
        public static string ServiceSmileMyListTitleTrimTail => appConfig.Get("service-smile-mylist-title-trim-tail");

        /// <summary>
        /// 動画データ取得中になんかエラー発生から再開までの待ち時間。
        /// </summary>
        public static TimeSpan ServiceSmileVideoDownloadingErrorWaitTime => appConfig.Get("service-smile-smilevideo-downloading_error-wait-time", TimeSpan.Parse);
        /// <summary>
        /// DMC形式ダウンロードタイプの重み
        /// </summary>
        public static IReadOnlyRange<int> ServiceSmileVideoDownloadDmcWeightRange => appConfig.Get("service-smile-smilevideo-donwload-dmc-weight-range", RangeModel.Parse<int>);
        public static int ServiceSmileVideoDownloadDmcWeightRangeMinimum => ServiceSmileVideoDownloadDmcWeightRange.Head;
        public static int ServiceSmileVideoDownloadDmcWeightRangeMaximum => ServiceSmileVideoDownloadDmcWeightRange.Tail;
        public static Regex ServiceSmileVideoDownloadDmcWeightVideoSort => appConfig.Get("service-smile-smilevideo-donwload-dmc-weight-video-sort", s => new Regex(s, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Singleline));
        public static Regex ServiceSmileVideoDownloadDmcWeightAudioSort => appConfig.Get("service-smile-smilevideo-donwload-dmc-weight-audio-sort", s => new Regex(s, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Singleline));
        /// <summary>
        /// DMC形式動画のポーリング待ち時間
        /// </summary>
        public static TimeSpan ServiceSmileVideoDownloadDmcPollingWaitTime => appConfig.Get("service-smile-smilevideo-donwload-dmc-polling-wait-time", TimeSpan.Parse);

        /// <summary>
        /// ダウンロード拡張子。
        /// </summary>
        public static string ServiceSmileVideoDownloadDmcExtension => appConfig.Get("service-smile-smilevideo-donwload-dmc-extension");

        /// <summary>
        /// 動画データ取得中になんかエラー発生から再開のリトライ数。
        /// </summary>
        public static int ServiceSmileVideoDownloadingErrorRetryCount => appConfig.Get("service-smile-smilevideo-downloading_error-retry-count", int.Parse);
        /// <summary>
        /// 視聴ページから動画データ取得までの待ち時間。
        /// </summary>
        public static TimeSpan ServiceSmileVideoWatchToMovieWaitTime => appConfig.Get("service-smile-smilevideo-watch-to-movie-wait-time", TimeSpan.Parse);
        /// <summary>
        /// 動画受信時のバッファサイズ。
        /// <para>確保するだけで受け取るかどうかはサーバー次第。</para>
        /// </summary>
        public static int ServiceSmileVideoReceiveBuffer => appConfig.Get("service-smile-smilevideo-receive-buffer", int.Parse);
        /// <summary>
        /// ユーザー履歴数。
        /// </summary>
        public static int ServiceSmileUserHistoryCount => appConfig.Get("service-smile-user-history-count", int.Parse);
        /// <summary>
        /// マイリスト履歴数。
        /// </summary>
        public static int ServiceSmileMyListHistoryCount => appConfig.Get("service-smile-mylist-history-count", int.Parse);
        /// <summary>
        /// 検索履歴数。
        /// </summary>
        public static int ServiceSmileVideoSearchHistoryCount => appConfig.Get("service-smile-smilevideo-search-history-count", int.Parse);
        /// <summary>
        /// 再生履歴数。
        /// <para>データ上の保持数</para>
        /// </summary>
        public static int ServiceSmileVideoPlayHistoryCount => appConfig.Get("service-smile-smilevideo-play-history-count", int.Parse);
        /// <summary>
        /// 再生履歴数。
        /// <para>View上の保持数</para>
        /// </summary>
        public static int ServiceSmileVideoPlayHistoryViewCount => appConfig.Get("service-smile-smilevideo-play-history-view-count", int.Parse);
        /// <summary>
        /// あとで見るの保持数。
        /// <para>非表示を含めた全体数。</para>
        /// </summary>
        public static int ServiceSmileVideoCheckItLaterCount => appConfig.Get("service-smile-smilevideo-check_it_later-count", int.Parse);
        /// <summary>
        /// アカウント履歴削除待ち時間。
        /// <para>複数アイテム削除時間の削除間隔。</para>
        /// </summary>
        public static TimeSpan ServiceSmileVideoHistoryRemoveWaitTime => appConfig.Get("service-smile-smilevideo-history-remove-wait-time", TimeSpan.Parse);
        /// <summary>
        /// アカウント履歴削除後再読み込みを行う際の待ち時間。
        /// </summary>
        public static TimeSpan ServiceSmileVideoHistoryReloadWaitTime => appConfig.Get("service-smile-smilevideo-history-reload-wait-time", TimeSpan.Parse);
        /// <summary>
        ///オススメ動画の次ページ取得までの待機時間。
        /// </summary>
        public static TimeSpan ServiceSmileVideoRecommendationsPageContinueWaitTime => appConfig.Get("service-smile-smilevideo-recommendations-page-continue-wait-time", TimeSpan.Parse);
        /// <summary>
        /// マイリストのフォルダ色。
        /// </summary>
        public static Color ServiceSmileVideoMyListFolderColor => appConfig.Get("service-smile-mylist-folder-color", s => (Color)ColorConverter.ConvertFromString(s));

        ///// <summary>
        ///// ファインダーのフィルタリングを使用するか。
        ///// </summary>
        //public static bool ServiceSmileVideoIsEnabledFiltering => appConfig.Get("service-smile-smilevideo-is-enabled-filtering", bool.Parse);

        /// <summary>
        /// あとで見るのチェック時間。
        /// </summary>
        public static TimeSpan ServiceSmileVideoCheckItLaterCheckTime => appConfig.Get("service-smile-smilevideo-check_it_later-check-time", TimeSpan.Parse);

        /// <summary>
        /// コメント表示を行う際の補正時間。
        /// </summary>
        public static TimeSpan ServiceSmileVideoCommentCorrectionTime => appConfig.Get("service-smile-smilevideo-comment-correction-time", TimeSpan.Parse);

        /// <summary>
        /// コメント一行表示の改行の視覚表示文字。
        /// </summary>
        public static string ServiceSmileVideoCommentSimpleNewline => appConfig.Get("service-smile-smilevideo-comment-simple-newline");
        /// <summary>
        /// コメント一行表示の連続スペースの視覚表示兼まとめ文字。
        /// </summary>
        public static string ServiceSmileVideoCommentSimpleSpace => appConfig.Get("service-smile-smilevideo-comment-simple-space");

        /// <summary>
        /// コメント表示領域実サイズ。
        /// <para>横。</para>
        /// </summary>
        public static double ServiceSmileVideoPlayerCommentWidth => appConfig.Get("service-smile-smilevideo-player-comment-width", double.Parse);
        /// <summary>
        /// コメント表示領域実サイズ。
        /// <para>縦。</para>
        /// </summary>
        public static double ServiceSmileVideoPlayerCommentHeight => appConfig.Get("service-smile-smilevideo-player-comment-height", double.Parse);

        public static double ServiceSmileVideoPlayerOfficial4x3Width => appConfig.Get("service-smile-smilevideo-player-official-4:3-width", double.Parse);
        public static double ServiceSmileVideoPlayerOfficial4x3Height => appConfig.Get("service-smile-smilevideo-player-official-4:3-height", double.Parse);
        public static double ServiceSmileVideoPlayerOfficial16x9Width => appConfig.Get("service-smile-smilevideo-player-official-16:9-width", double.Parse);
        public static double ServiceSmileVideoPlayerOfficial16x9Height => appConfig.Get("service-smile-smilevideo-player-official-16:9-height", double.Parse);
        /// <summary>
        /// 自動再生タイミング上下限値。
        /// </summary>
        public static IReadOnlyRange<long> ServiceSmileVideoPlayerAutoPlayLowestSizeRange => appConfig.Get("service-smile-smilevideo-player-auto-play-lowest-size-range", RangeModel.Parse<long>);
        public static long ServiceSmileVideoPlayerAutoPlayLowestSizeRangeMinimum => ServiceSmileVideoPlayerAutoPlayLowestSizeRange.Head;
        public static long ServiceSmileVideoPlayerAutoPlayLowestSizeRangeMaximum => ServiceSmileVideoPlayerAutoPlayLowestSizeRange.Tail;

        public static IReadOnlyRange<int> ServiceSmileVideoPlayerStepVolumeRange = appConfig.Get("service-smile-smilevideo-player-setep-volume-range", RangeModel.Parse<int>);
        public static int ServiceSmileVideoPlayerStepVolumeRangeMinimum => ServiceSmileVideoPlayerStepVolumeRange.Head;
        public static int ServiceSmileVideoPlayerStepVolumeRangeMaximum => ServiceSmileVideoPlayerStepVolumeRange.Tail;
        public static IReadOnlyRange<int> ServiceSmileVideoPlayerStepSeekRangePercent => appConfig.Get("service-smile-smilevideo-player-setep-seek-range-percent", RangeModel.Parse<int>);
        public static int ServiceSmileVideoPlayerStepSeekRangePercentMinimum => ServiceSmileVideoPlayerStepSeekRangePercent.Head;
        public static int ServiceSmileVideoPlayerStepSeekRangePercentMaximum => ServiceSmileVideoPlayerStepSeekRangePercent.Tail;
        public static IReadOnlyRange<int> ServiceSmileVideoPlayerStepSeekRangeAbsolute => appConfig.Get("service-smile-smilevideo-player-setep-seek-range-absolute", RangeModel.Parse<int>);
        public static int ServiceSmileVideoPlayerStepSeekRangeAbsoluteMinimum => ServiceSmileVideoPlayerStepSeekRangeAbsolute.Head;
        public static int ServiceSmileVideoPlayerStepSeekRangeAbsoluteMaximum => ServiceSmileVideoPlayerStepSeekRangeAbsolute.Tail;

        public static bool ServiceSmileVideoPlayerCommentEffect => appConfig.Get("service-smile-smilevideo-player-comment-effect", bool.Parse);

        public static double ServiceSmilevideoPlayerCommentBig => appConfig.Get("service-smile-smilevideo-player-comment-big", double.Parse);
        public static double ServiceSmilevideoPlayerCommentSmall => appConfig.Get("service-smile-smilevideo-player-comment-small", double.Parse);

        /// <summary>
        /// 関連動画のソート。
        /// </summary>
        public static string ServiceSmileVideoRelationVideoSort => appConfig.Get("service-smile-relation-video-sort");
        /// <summary>
        /// 関連動画の昇順。
        /// </summary>
        public static string ServiceSmileVideoRelationVideoOrderAscending => appConfig.Get("service-smile-relation-video-order-ascending");
        /// <summary>
        /// 関連動画を降順。
        /// </summary>
        public static string ServiceSmileVideoRelationVideoOrderDescending => appConfig.Get("service-smile-relation-video-order-descending");

        /// <summary>
        /// 動画検索方法。
        /// </summary>
        public static SmileVideoSearchType ServiceSmileVideoSearchType => appConfig.Get("service-smile-smilevideo-search-type", s => EnumUtility.Parse< SmileVideoSearchType>(s));

        public static TimeSpan ServiceSmileVideoThumbCacheTime = appConfig.Get("service-smile-smilevideo-thumb-cache-time", TimeSpan.Parse);
        public static TimeSpan ServiceSmileVideoMsgCacheTime = appConfig.Get("service-smile-smilevideo-msg-cache-time", TimeSpan.Parse);
        public static TimeSpan ServiceSmileVideoRelationCacheTime = appConfig.Get("service-smile-smilevideo-relation-cache-time", TimeSpan.Parse);
        public static TimeSpan ServiceSmileVideoCheckItLaterCacheTime = appConfig.Get("service-smile-smilevideo-check_it_later-cache-time", TimeSpan.Parse);

        public static int ServiceSmileVideoTagFeedItemCount => appConfig.Get("service-smile-smilevideo-tag-feed-item-count", int.Parse);
        public static bool ServiceSmileVideoTagFeedItemSearchingUpdate => appConfig.Get("service-smile-smilevideo-tag-feed-item-searching-update", bool.Parse);
        public static TimeSpan ServiceSmileVideoTagFeedWaitTime => appConfig.Get("service-smile-smilevideo-tag-feed-wait-time", TimeSpan.Parse);
        public static string ServiceSmileVideoTagFeedSort => appConfig.Get("service-smile-smilevideo-tag-feed-sort");
        public static string ServiceSmileVideoTagFeedOrder => appConfig.Get("service-smile-smilevideo-tag-feed-order");
        public static int ServiceSmileVideoTagFeedCount => appConfig.Get("service-smile-smilevideo-tag-feed-count", int.Parse);

        public static bool ServiceSmileVideoPlayerActiveTopmostPlayingRestart => appConfig.Get("service-smile-smilevideo-player-active-topmost-playing-restart", bool.Parse);

        #region live

        /// <summary>
        /// 動画データ取得中になんかエラー発生から再開のリトライ数。
        /// </summary>
        public static int ServiceSmileLiveCategoryBaseCount => appConfig.Get("service-smile-smilelive-category-base-count", int.Parse);

        public static TimeSpan ServiceSmileLiveInformationCacheTime = appConfig.Get("service-smile-live-information-cache-time", TimeSpan.Parse);

        #endregion

        #endregion

        #region app

        public static IEnumerable<string> AppSmileVideoLaboratoryPlayVideoExtensions => appConfig.Get("app-smile-labo-play-video-extensions", s => s.Split(';'));
        public static IEnumerable<string> AppSmileVideoLaboratoryPlayMsgExtensions => appConfig.Get("app-smile-labo-play-msg-extensions", s => s.Split(';'));


        #endregion

        #region setting

        public static double SettingServiceSmileUserGroupAreaStar = appConfig.Get("setting-service-smile-user-group-area-star", double.Parse);
        public static double SettingServiceSmileUserItemsAreaStar = appConfig.Get("setting-service-smile-user-items-area-star", double.Parse);

        public static double SettingServiceSmileMyListGroupAreaStar => appConfig.Get("setting-service-smile-mylist-group-area-star", double.Parse);
        public static double SettingServiceSmileMyListItemsAreaStar => appConfig.Get("setting-service-smile-mylist-items-area-star", double.Parse);
        public static bool SettingServiceSmileMyListUsingBookmarkTagFilter => appConfig.Get("setting-service-smile-mylist-using-bookmark-tag-filter", bool.Parse);


        #region video

        /// <summary>
        /// コメントのフォントサイズ。
        /// <para>システムから取得。</para>
        /// </summary>
        public static double SettingServiceSmileVideoCommentFontSize { get; } = System.Windows.SystemFonts.MessageFontSize * 1.8;
        /// <summary>
        /// コメントのフォントファミリ。
        /// <para>システムから取得。</para>
        /// </summary>
        public static string SettingServiceSmileVideoCommentFontFamily { get; } = System.Windows.SystemFonts.MessageFontFamily.FamilyNames.Values.First();
        /// <summary>
        /// コメントtの透明度。
        /// </summary>
        public static double SettingServiceSmileVideoCommentFontAlpha => appConfig.Get("setting-service-smile-smilevideo-comment-font-alpha", double.Parse);
        /// <summary>
        /// コメントを太字にするか。
        /// </summary>
        public static bool SettingServiceSmileVideoCommentFontBold => appConfig.Get("setting-service-smile-smilevideo-comment-font-bold", bool.Parse);
        /// <summary>
        /// コメントを斜体にするか。
        /// </summary>
        public static bool SettingServiceSmileVideoCommentFontItalic => appConfig.Get("setting-service-smile-smilevideo-comment-font-italic", bool.Parse);
        /// <summary>
        /// コメントの表示時間。
        /// </summary>
        public static TimeSpan SettingServiceSmileVideoCommentShowTime => appConfig.Get("setting-service-smile-smilevideo-comment-show-time", TimeSpan.Parse);
        /// <summary>
        /// コメント中の円マークがスラッシュと対になる場合はバックスラッシュに置き換えるか。
        /// </summary>
        public static bool SettingServiceSmileVideoCommentConvertPairYenSlash => appConfig.Get("setting-service-smile-smilevideo-comment-pair-yen-slash", bool.Parse);
        /// <summary>
        /// 共有NGを使用するか。
        /// </summary>
        public static bool SettingServiceSmileVideoCommentSharedNoGoodIsEnabled => appConfig.Get("setting-service-smile-smilevideo-comment-shared-no-good-is-enabled", bool.Parse);
        /// <summary>
        /// 共有NGレベル。
        /// </summary>
        public static int SettingServiceSmileVideoCommentSharedNoGoodScore => appConfig.Get("setting-service-smile-smilevideo-comment-shared-no-good-score", int.Parse);
        /// <summary>
        /// コメント投稿を匿名で行うか。
        /// </summary>
        public static bool SettingServiceSmileVideoCommentPostAnonymous => appConfig.Get("setting-service-smile-smilevideo-comment-post-anonymous", bool.Parse);
        /// <summary>
        /// 投稿者コメントもフィルタリング対象とするか。
        /// </summary>
        public static bool SettingServiceSmileVideoCommentIsEnabledOriginalPosterFilering => appConfig.Get("setting-service-smile-smilevideo-comment-is-enabled-op-filering", bool.Parse);
        /// <summary>
        /// 検索で一度に取得する数。
        /// </summary>
        [Obsolete]
        public static int SettingServiceSmileVideoSearchCount => appConfig.Get("setting-service-smile-smilevideo-search-count", int.Parse);
        public static double SettingServiceSmileVideoSearchBookmarkWidth => appConfig.Get("setting-service-smile-smilevideo-search-bookmark-width", double.Parse);
        public static double SettingServiceSmileVideoSearchFinderWidth => appConfig.Get("setting-service-smile-smilevideo-search-finder-width", double.Parse);
        public static bool SettingServiceSmileVideoSearchIsCheckUpdate => appConfig.Get("setting-service-smile-smilevideo-search-is-check-update", bool.Parse);
        /// <summary>
        /// 動画情報を取得するか。
        /// </summary>
        public static bool SettingServiceSmileVideoLoadVideoInformation => appConfig.Get("setting-service-smile-smilevideo-load-video-information", bool.Parse);
        /// <summary>
        /// 自動再生を行うか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerIsAutoPlay => appConfig.Get("setting-service-smile-smilevideo-player-is-auto-play", bool.Parse);
        /// <summary>
        /// 再生可能判定までの動画サイズ。
        /// </summary>
        public static long SettingServiceSmileVideoPlayerAutoPlayLowestSize => appConfig.Get("setting-service-smile-smilevideo-player-auto-play-lowest-size", long.Parse);
        /// <summary>
        /// リプレイを行うか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerReplay => appConfig.Get("setting-service-smile-smilevideo-player-replay", bool.Parse);
        /// <summary>
        /// 音量。
        /// </summary>
        public static int SettingServiceSmileVideoPlayerVolume => appConfig.Get("setting-service-smile-smilevideo-player-volume", int.Parse);
        /// <summary>
        /// ミュート。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerMute => appConfig.Get("setting-service-smile-smilevideo-player-mute", bool.Parse);
        /// <summary>
        /// プレイヤーウィンドウ: 左。
        /// </summary>
        public static double SettingServiceSmileVideoWindowLeft => appConfig.Get("setting-service-smile-smilevideo-player-window-left", double.Parse);
        /// <summary>
        /// プレイヤーウィンドウ: 上。
        /// </summary>
        public static double SettingServiceSmileVideoWindowTop => appConfig.Get("setting-service-smile-smilevideo-player-window-top", double.Parse);
        /// <summary>
        /// プレイヤーウィンドウ: 幅。
        /// </summary>
        public static double SettingServiceSmileVideoWindowWidth => appConfig.Get("setting-service-smile-smilevideo-player-window-width", double.Parse);
        /// <summary>
        /// プレイヤーウィンドウ: 高さ。
        /// </summary>
        public static double SettingServiceSmileVideoWindowHeight => appConfig.Get("setting-service-smile-smilevideo-player-window-height", double.Parse);
        /// <summary>
        /// ウィンドウ最前面状態。
        /// </summary>
        public static bool SettingServiceSmileVideoWindowTopmost => appConfig.Get("setting-service-smile-smilevideo-player-window-topmost", bool.Parse);

        public static TopmostKind SettingServiceSmileVideoPlayerTopmostKind => appConfig.Get("setting-service-smile-smilevideo-player-topmost-kind", s => EnumUtility.Parse<TopmostKind>(s, false));

        /// <summary>
        /// 予期せぬ停止から次の動画へ移るまでの待機時間。
        /// </summary>
        public static TimeSpan SettingServiceSmileVideoPlayerBufferingStopSkipTime => appConfig.Get("setting-service-smile-smilevideo-player-buffering-stop-skip-time", TimeSpan.Parse);
        /// <summary>
        /// コメント表示上限を使用するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerDisplayCommentLimitIsEnabled => appConfig.Get("setting-service-smile-smilevideo-player-display-comment-limit-is-enabled", bool.Parse);
        /// <summary>
        /// コメント表示上限数。
        /// </summary>
        public static int SettingServiceSmileVideoPlayerDisplayCommentLimitCount => appConfig.Get("setting-service-smile-smilevideo-player-display-comment-limit-count", int.Parse);
        /// <summary>
        /// コメント有効領域を使用するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerCommentEnableAreaIsEnabled => appConfig.Get("setting-service-smile-smilevideo-player-comment-enable-area-is-enabled", bool.Parse);
        /// <summary>
        /// コメント有効領域の割合。
        /// </summary>
        public static double SettingServiceSmileVideoPlayerCommentEnableAreaPercent => appConfig.Get("setting-service-smile-smilevideo-player-comment-enable-area-percent", double.Parse);
        /// <summary>
        /// コメント有効領域を設定可能にするか。
        /// </summary>
        public static bool SettingServiceSmileVideoCanChangeCommentEnabledArea => appConfig.Get("setting-service-smile-smilevideo-player-can-change-comment-enabled-area", bool.Parse);
        /// <summary>
        /// コメントのテキスト描画方法。
        /// </summary>
        public static TextShowMode SettingServiceSmileVideoCommentTextShowMode => appConfig.Get("setting-service-smile-smilevideo-comment-text_show_mode", s => (TextShowMode)Enum.Parse(typeof(TextShowMode), s));
        /// <summary>
        /// 投稿者のコメント背景色を塗りつぶすか。
        /// </summary>
        public static bool SettingServiceSmileVideoCommentFillBackgroundOriginalPoster  => appConfig.Get("setting-service-smile-smilevideo-comment-fill-background-op", bool.Parse);
        /// <summary>
        /// コメントを自動スクロールするか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerAutoScrollCommentList => appConfig.Get("setting-service-smile-smilevideo-player-auto-scroll-comment-list", bool.Parse);
        /// <summary>
        /// コメントリスト内にカーソルがある場合は自動スクロールを抑制するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerDisbaledAutoScrollCommentListOverCursor => appConfig.Get("setting-service-smile-smilevideo-player-disabled-auto-scroll-comment-list-over-cursor", bool.Parse);
        /// <summary>
        /// 詳細部を表示するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerShowDetailArea => appConfig.Get("setting-service-smile-smilevideo-player-show-detail-area", bool.Parse);
        /// <summary>
        /// コメントリストを表示するか。
        /// <para>通常ウィンドウ。</para>
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerShowNormalWindowCommentList => appConfig.Get("setting-service-smile-smilevideo-player-show-normal-window-comment-list", bool.Parse);
        /// <summary>
        /// コメントリストを表示するか。
        /// <para>通常ウィンドウ。</para>
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerShowFullScreenCommentList => appConfig.Get("setting-service-smile-smilevideo-player-show-fullscreen-comment-list", bool.Parse);
        /// <summary>
        /// コメント投稿時間を表示するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerShowPostTimestamp => appConfig.Get("setting-service-smile-smilevideo-player-show-post-timestamp", bool.Parse);
        /// <summary>
        /// スペースで一時停止するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerKeySpaceToPause => appConfig.Get("setting-service-smile-smilevideo-player-key-space-to-pause", bool.Parse);
        /// <summary>
        /// クリックで一時停止するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerMoseClickToPause => appConfig.Get("setting-service-smile-smilevideo-player-mouse-click-to-pause", bool.Parse);
        /// <summary>
        /// ホイール操作。
        /// </summary>
        public static WheelOperation SettingServiceSmileVideoPlayerWheelOperation => appConfig.Get("setting-service-smile-smilevideo-player-wheel-operation", s => EnumUtility.Parse<WheelOperation>(s, false));

        public static int SettingServiceSmileVideoPlayerVolumeOperationStep => appConfig.Get("setting-service-smile-smilevideo-player-volume-operation-step", int.Parse);
        public static bool SettingServiceSmileVideoPlayerSeekOperationIsPercent => appConfig.Get("setting-service-smile-smilevideo-player-seek-operation-is-percent", bool.Parse);
        public static int SettingServiceSmileVideoPlayerSeekOperationAbsoluteStep => appConfig.Get("setting-service-smile-smilevideo-player-seek-operation-absolute-step", int.Parse);
        public static int SettingServiceSmileVideoPlayerSeekOperationPercentStep => appConfig.Get("setting-service-smile-smilevideo-player-seek-operation-percent-step", int.Parse);

        public static bool SettingServiceSmileVideoPlayerInactiveIsFullScreenRestore => appConfig.Get("setting-service-smile-smilevideo-player-inactive-is-fullscreen-restore", bool.Parse);
        public static bool SettingServiceSmileVideoPlayerInactiveIsFullScreenRestorePrimaryDisplayOnly => appConfig.Get("setting-service-smile-smilevideo-player-inactive-is-fullscreen-restore-primary-display-only", bool.Parse);
        public static bool SettingServiceSmileVideoPlayerStopFullScreenRestore => appConfig.Get("setting-service-smile-smilevideo-player-stop-fullscreen-restore", bool.Parse);
        public static bool SettingServiceSmileVideoPlayerStopFullScreenRestorePrimaryDisplayOnly => appConfig.Get("setting-service-smile-smilevideo-player-stop-fullscreen-restore--primary-display-only", bool.Parse);

        public static double SettingServiceSmileVideoPlayerPlayerAreaStar => appConfig.Get("setting-service-smile-smilevideo-player-player-area-star", double.Parse);
        public static double SettingServiceSmileVideoPlayerCommentAreaStar => appConfig.Get("setting-service-smile-smilevideo-player-comment-area-star", double.Parse);
        public static double SettingServiceSmileVideoPlayerInformationAreaPixel => appConfig.Get("setting-service-smile-smilevideo-player-information-area-pixel", double.Parse);

        public static BackgroundKind SettingServiceSmileVideoPlayerBackgroundKind => appConfig.Get("setting-service-smile-smilevideo-player-background-kind", s => EnumUtility.Parse<BackgroundKind>(s, false));
        public static Color SettingServiceSmileVideoPlayerBackgroundColor => appConfig.Get("setting-service-smile-smilevideo-player-background-color", s => (Color)ColorConverter.ConvertFromString(s));

        public static double SettingServiceSmileVideoPlayerViewScale => appConfig.Get("setting-service-smile-smilevideo-player-view-scale", double.Parse);

        public static bool SettingServiceSmileVideoPlayerShowNavigatorFullScreen => appConfig.Get("setting-service-smile-smilevideo-player-show-navigator-fullscreen", bool.Parse);

        /// <summary>
        /// コメントを表示するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerVisibleComment => appConfig.Get("setting-service-smile-smilevideo-player-comment-visible", bool.Parse);
        /// <summary>
        /// 全体フィルタリングを有効にするか。
        /// </summary>
        public static bool SettingServiceSmileVideoGlobalCommentFileringIsEnabled => appConfig.Get("setting-service-smile-smilevideo-player-global-comment-filtering-is-enabled", bool.Parse);

        public static int SettingServiceSmileVideoPlayerCommentFps => appConfig.Get("setting-service-smile-smilevideo-player-comment-fps", int.Parse);

        /// <summary>
        /// 動画再生方法。
        /// </summary>
        public static ExecuteOrOpenMode SettingServiceSmileVideoExecuteOpenMode => appConfig.Get("setting-service-smile-smilevideo-execute-open-mode", s => (ExecuteOrOpenMode)Enum.Parse(typeof(ExecuteOrOpenMode), s));
        /// <summary>
        /// プレイヤーを常に新規プレイヤーで表示するか。
        /// </summary>
        public static bool SettingServiceSmileVideoExecuteOpenPlayerInNewWindow => appConfig.Get("setting-service-smile-smilevideo-execute-open-player-in-new-window", bool.Parse);

        /// <summary>
        /// 重複したコメントを無視するか。
        /// </summary>
        public static bool SettingServiceSmileVideoFilteringIgnoreOverlapWord => appConfig.Get("setting-service-smile-smilevideo-filtering-ignore-overlap-word", bool.Parse);
        /// <summary>
        /// 重複判定時間。
        /// </summary>
        public static TimeSpan SettingServiceSmileVideoFilteringIgnoreOverlapTime => appConfig.Get("setting-service-smile-smilevideo-filtering-ignore-overlap-time", TimeSpan.Parse);

        public static bool SettingServiceSmileVideoDownloadUsingDmc => appConfig.Get("setting-service-smile-smilevideo-download-dmc-using", bool.Parse);
        public static int SettingServiceSmileVideoDownloadDmcVideoWeight => appConfig.Get("setting-service-smile-smilevideo-download-dmc-video-weight", int.Parse);
        public static int SettingServiceSmileVideoDownloadDmcAudioWeight => appConfig.Get("setting-service-smile-smilevideo-download-dmc-audio-weight", int.Parse);

        public static double SettingServiceSmileVideoBookmarkGroupAreaStar => appConfig.Get("setting-service-smile-smilevideo-bookmark-group-area-star", double.Parse);
        public static double SettingServiceSmileVideoBookmarkItemsAreaStar => appConfig.Get("setting-service-smile-smilevideo-bookmark-items-area-star", double.Parse);

        #endregion

        #region live

        public static Uri ServiceSmileLiveQatchUrlBase => appConfig.Get("service-smile-smilelive-watch-url-base", s => new Uri(s));

        public static ExecuteOrOpenMode SettingServiceSmileLiveExecuteOpenMode => appConfig.Get("setting-service-smile-smilelive-execute-open-mode", s => (ExecuteOrOpenMode)Enum.Parse(typeof(ExecuteOrOpenMode), s));
        public static bool SettingServiceSmileLiveExecuteOpenPlayerInNewWindow => appConfig.Get("setting-service-smile-smilelive-execute-open-player-in-new-window", bool.Parse);

        /// <summary>
        /// プレイヤーウィンドウ: 左。
        /// </summary>
        public static double SettingServiceSmileLiveWindowLeft => appConfig.Get("setting-service-smile-smilelive-player-window-left", double.Parse);
        /// <summary>
        /// プレイヤーウィンドウ: 上。
        /// </summary>
        public static double SettingServiceSmileLiveWindowTop => appConfig.Get("setting-service-smile-smilelive-player-window-top", double.Parse);
        /// <summary>
        /// プレイヤーウィンドウ: 幅。
        /// </summary>
        public static double SettingServiceSmileLiveWindowWidth => appConfig.Get("setting-service-smile-smilelive-player-window-width", double.Parse);
        /// <summary>
        /// プレイヤーウィンドウ: 高さ。
        /// </summary>
        public static double SettingServiceSmileLiveWindowHeight => appConfig.Get("setting-service-smile-smilelive-player-window-height", double.Parse);
        /// <summary>
        /// ウィンドウ最前面状態。
        /// </summary>
        public static bool SettingServiceSmileLiveWindowTopmost => appConfig.Get("setting-service-smile-smilelive-player-window-topmost", bool.Parse);

        /// <summary>
        /// 詳細部を表示するか。
        /// </summary>
        public static bool SettingServiceSmileLivePlayerShowDetailArea => appConfig.Get("setting-service-smile-smilelive-player-show-detail-area", bool.Parse);

        public static double SettingServiceSmileLivePlayerViewScale => appConfig.Get("setting-service-smile-smilelive-player-view-scale", double.Parse);

        #endregion

        #endregion

        #endregion
    }
}
