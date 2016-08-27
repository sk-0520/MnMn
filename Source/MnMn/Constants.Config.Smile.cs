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
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn
{
    partial class Constants
    {
        #region property

        #region service

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

        /// <summary>
        /// マイリスト削除後再読み込みを行う際の待ち時間。
        /// </summary>
        public static TimeSpan ServiceSmileMyListReloadWaitTime => appConfig.Get("service-smile-mylist-reload-wait-time", TimeSpan.Parse);


        /// <summary>
        /// 動画データ取得中になんかエラー発生から再開までの待ち時間。
        /// </summary>
        public static TimeSpan ServiceSmileVideoDownloadingErrorWaitTime => appConfig.Get("service-smile-smilevideo-downloading_error-wait-time", TimeSpan.Parse);
        /// <summary>
        /// 動画データ取得中になんかエラー発生から再開のリトライ数。
        /// </summary>
        public static int ServiceSmileVideoDownloadingErrorRetryCount => appConfig.Get("service-smile-smilevideo-downloading_error-retry-count", int.Parse);
        /// <summary>
        /// 視聴ページから動画データ取得までの待ち時間。
        /// </summary>
        public static TimeSpan ServiceSmileVideoWatchToMovieWaitTime => appConfig.Get("service-smile-smilevideo-watch-to-movie-wait-time", TimeSpan.Parse);
        /// <summary>
        /// 再生可能判定までの動画サイズ。
        /// </summary>
        public static long ServiceSmileVideoPlayLowestSize => appConfig.Get("service-smile-smilevideo-play-lowest-size", long.Parse);
        /// <summary>
        /// 動画受信時のバッファサイズ。
        /// <para>確保するだけで受け取るかどうかはサーバー次第。</para>
        /// </summary>
        public static int ServiceSmileVideoReceiveBuffer => appConfig.Get("service-smile-smilevideo-receive-buffer", int.Parse);
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
        /// </summary>
        public static int ServiceSmileVideoPlayHistoryCount => appConfig.Get("service-smile-smilevideo-play-history-count", int.Parse);
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

        /// <summary>
        /// ファインダーのフィルタリングを使用するか。
        /// </summary>
        public static bool ServiceSmileVideoIsEnabledFiltering => appConfig.Get("service-smile-smilevideo-is-enabled-filtering", bool.Parse);

        #endregion

        #region setting

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
        /// 検索で一度に取得する数。
        /// </summary>
        public static int SettingServiceSmileVideoSearchCount => appConfig.Get("setting-service-smile-smilevideo-search-count", int.Parse);
        /// <summary>
        /// 動画情報を取得するか。
        /// </summary>
        public static bool SettingServiceSmileVideoLoadVideoInformation => appConfig.Get("setting-service-smile-smilevideo-load-video-information", bool.Parse);
        /// <summary>
        /// 自動再生を行うか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerAutoPlay => appConfig.Get("setting-service-smile-smilevideo-player-auto-play", bool.Parse);
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
        /// コメントのテキスト描画方法。
        /// </summary>
        public static TextShowKind SettingServiceSmileVideoPlayerTextShowKind => appConfig.Get("setting-service-smile-smilevideo-player-TextShowKind", s => (TextShowKind)Enum.Parse(typeof(TextShowKind), s));
        /// <summary>
        /// コメントを自動スクロールするか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerAutoScrollCommentList => appConfig.Get("setting-service-smile-smilevideo-player-auto-scroll-comment-list", bool.Parse);
        /// <summary>
        /// 詳細部を表示するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerShowDetailArea => appConfig.Get("setting-service-smile-smilevideo-player-show-detail-area", bool.Parse);
        /// <summary>
        /// コメントリストを表示するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerShowCommentList => appConfig.Get("setting-service-smile-smilevideo-player-show-comment-list", bool.Parse);
        /// <summary>
        /// コメント投稿時間を表示するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerShowPostTimestamp => appConfig.Get("setting-service-smile-smilevideo-player-show-post-timestamp", bool.Parse);
        /// <summary>
        /// コメントを表示するか。
        /// </summary>
        public static bool SettingServiceSmileVideoPlayerVisibleComment => appConfig.Get("setting-service-smile-smilevideo-player-comment-visible", bool.Parse);
        /// <summary>
        /// 全体フィルタリングを有効にするか。
        /// </summary>
        public static bool SettingServiceSmileVideoGlobalCommentFileringIsEnabled => appConfig.Get("setting-service-smile-smilevideo-player-global-comment-filtering-is-enabled", bool.Parse);
        /// <summary>
        /// 重複したコメントを無視するか。
        /// </summary>
        public static bool SettingServiceSmileVideoFilteringIgnoreOverlapWord => appConfig.Get("setting-service-smile-smilevideo-filtering-ignore-overlap-word", bool.Parse);
        /// <summary>
        /// 重複判定時間。
        /// </summary>
        public static TimeSpan SettingServiceSmileVideoFilteringIgnoreOverlapTime => appConfig.Get("setting-service-smile-smilevideo-filtering-ignore-overlap-time", TimeSpan.Parse);

        #endregion

        #endregion
    }
}
