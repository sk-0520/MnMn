﻿/*
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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.ReadOnly;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn
{
    /// <summary>
    /// App.config関係。
    /// </summary>
    partial class Constants
    {
        #region variable

        static ConfigurationCacher appConfig = new ConfigurationCacher();

        #endregion

        #region property

        public static string AppUriAbout => ReplaceAppConfig(appConfig.Get("app-uri-about"));
        public static string AppMailAbout => ReplaceAppConfig(appConfig.Get("app-mail-about"));
        public static string AppUriDevelopment => ReplaceAppConfig(appConfig.Get("app-uri-development"));
        public static Uri AppUriForum => new Uri(ReplaceAppConfig(appConfig.Get("app-uri-forum")));
        public static string AppUriUpdate => ReplaceAppConfig(appConfig.Get("app-uri-update"));
        public static string AppUriChangelogRelease => ReplaceAppConfig(appConfig.Get("app-uri-changelog-release"));
        //public static string UriChangelogRc => ReplaceAppConfig(appConfig.Get("app-uri-changelog-rc"));
        public static Uri AppUriSituation => new Uri(ReplaceAppConfig(appConfig.Get("app-uri-situation")));
        public static Uri AppUriIssueResolved => new Uri(ReplaceAppConfig(appConfig.Get("app-uri-issue-resolved")));
        public static Uri AppUriQuestionnaire => new Uri(ReplaceAppConfig(appConfig.Get("app-uri-questionnaire")));
        public static string AppUriWebNavigatorGeckoFxPlugins => ReplaceAppConfig(appConfig.Get("app-uri-web_navigator-plugins"));
        public static Uri AppUriFlashPlayerVersion => new Uri(ReplaceAppConfig(appConfig.Get("app-uri-flashplayer-version")));
        public static Uri AppUriGeneralBrowserHome => new Uri(appConfig.Get("app-uri-general-browser"));
        public static Uri AppUriLightweightUpdate => new Uri(ReplaceAppConfig(appConfig.Get("app-uri-lightweight-update")));
        public static Uri AppUriCrashReceiveProgram => new Uri(ReplaceAppConfig(appConfig.Get("app-uri-crash-receive-program")));

        public static Uri AppServiceUri => new Uri(ReplaceAppConfig(appConfig.Get("app-service-uri")));
        public static string AppServiceProcessLinkEndpoint => appConfig.Get("app-service-process-link-endpoint");
        public static TimeSpan AppServiceProcessLinkWaitTime => appConfig.Get("app-service-process-link-wait-time", TimeSpan.Parse);

        public static TimeSpan AppSplashCloseTime => appConfig.Get("app-splash-close-time", TimeSpan.Parse);
        public static bool AppUnhandledExceptionHandled => appConfig.Get("app-un_handled-exception-handled", bool.Parse);
        public static bool AppSendCrashReport => appConfig.Get("app-send-crash-report", bool.Parse);
        public static bool AppCrashReportIsDebug => appConfig.Get("app-crash-report-is-debug", bool.Parse);

        public static TimeSpan AppSystemBreakTime => appConfig.Get("app-system-break-time", TimeSpan.Parse);
        public static bool ApplicationCheckNetworkAvailable => appConfig.Get("app-check-network-available", bool.Parse);

        public static TimeSpan NetworkProxySettingRefreshTime => appConfig.Get("network-proxy-setting-refresh-time", TimeSpan.Parse);

        public static WebNavigatorEngine WebNavigatorEngine => appConfig.Get("web_navigator-engine", s => EnumUtility.Parse<WebNavigatorEngine>(s, false));
        public static string WebNavigatorGeckoFxLibraryDirectoryName => appConfig.Get("web_navigator-geckofx-lib-dir-name");
        public static string WebNavigatorGeckoFxProfileDirectoryName => appConfig.Get("web_navigator-geckofx-profile-dir-name");
        public static string WebNavigatorGeckoFxPreferencesFileName => appConfig.Get("web_navigator-geckofx-preferences-name");
        public static string WebNavigatorGeckoFxPluginsDirectoryName => appConfig.Get("web_navigator-geckofx-plugin-dir-name");
        public static string WebNavigatorGeckoFxExtensionsDirectoryName => appConfig.Get("web_navigator-geckofx-extension-dir-name");
        public static bool WebNavigatorGeckoFxShowLog => appConfig.Get("web_navigator-geckofx-show-log", bool.Parse);
        public static bool WebNavigatorGeckoFxShowEngineLog => appConfig.Get("web_navigator-geckofx-show-engine-log", bool.Parse);
        public static TimeSpan WebNavigatorGeckoFxGestureWaitTime  => appConfig.Get("web_navigator-geckofx-gesture-wait-time", TimeSpan.Parse);
        public static string[] WebNavigatorGeckoFxIgnoreEngineLogs => appConfig.Get("web_navigator-geckofx-ignore-engine-logs", s => s.Split('\t'));

        public static bool WebNavigatorContextMenuShowHtmlSource => appConfig.Get("web_navigator-context_menu-show-html_source", bool.Parse);

        public static TimeSpan UpdateAppExitWaitTime => appConfig.Get("update-app-exit-wait-time", TimeSpan.Parse);

        public static double ViewScaleReset => appConfig.Get("view-scale-reset", double.Parse);
        public static IReadOnlyRange<double> ViewScaleRange => appConfig.Get("view-scale-range", RangeModel.Parse<double>);
        public static IReadOnlyRange<double> ViewScaleChangeRange => appConfig.Get("view-scale-change-range", RangeModel.Parse<double>);
        public static double[] ViewScaleList => appConfig.Get("view-scale-list", s => s.Split(';').Select(i => double.Parse(i)).ToArray());

        /// <summary>
        /// システム未操作の監視間隔。
        /// </summary>
        public static TimeSpan AutoRebootWatchTime => appConfig.Get("auto-reboot-watch-time", TimeSpan.Parse);
        /// <summary>
        /// システム未操作が一定時間続いた場合に再起動するか。
        /// </summary>
        public static bool AutoRebootIsEnabled => appConfig.Get("auto-reboot-is-enabled", bool.Parse);
        /// <summary>
        /// システム未操作が一定時間続いた場合に再起動する判定時間。
        /// <para><see cref="AutoRebootWatchTime"/>以上の値じゃないとダメぷー。</para>
        /// </summary>
        public static TimeSpan AutoRebootJudgeTime => appConfig.Get("auto-reboot-judge-time", TimeSpan.Parse);

        public static TimeSpan UninitializeAsyncWaitTime => appConfig.Get("uninitialize_async-wait-time", TimeSpan.Parse);

        public static string ApplicationLogicUserAgentFormat => appConfig.Get("application-logic-ua-format");

        public static bool ForceMoreOptionsShow => appConfig.Get("force-more-options-show", bool.Parse);

        /// <summary>
        /// 設定ファイルの自動保存タイミング。
        /// </summary>
        public static TimeSpan BackgroundAutoSaveSettingTime => appConfig.Get("background-auto-save-setting-time", TimeSpan.Parse);
        /// <summary>
        /// アップデートチェックタイミング。
        /// </summary>
        public static TimeSpan BackgroundUpdateCheckTime => appConfig.Get("background-update-check-time", TimeSpan.Parse);
        /// <summary>
        /// 起動時にGCを行うか。
        /// <para>デバッグ時に抑制する目的なのでリリース時は行う運用。</para>
        /// </summary>
        public static bool BackgroundGarbageCollectionIsEnabledStartup => appConfig.Get("background-garbage-collection-is-enabled-startup", bool.Parse);
        /// <summary>
        /// ゴミ処理タイミング。
        /// </summary>
        public static TimeSpan BackgroundGarbageCollectionTime => appConfig.Get("background-garbage-collection-time", TimeSpan.Parse);
        /// <summary>
        /// ゴミ処理規模。
        /// </summary>
        public static GarbageCollectionLevel BackgroundGarbageCollectionLevel => appConfig.Get("background-garbage-collection-level", s => (GarbageCollectionLevel)Enum.Parse(typeof(GarbageCollectionLevel), s));

        public static int GarbageCollectionCallEmptyworkingsetCount => appConfig.Get("garbage-collection-call-emptyworkingset-count", int.Parse);

        public static TimeSpan MutexWaitTime => appConfig.Get("mutex-wait-time", TimeSpan.Parse);
        public static TimeSpan MutexProcessLinkWaitTime => appConfig.Get("mutex-process-link-wait-time", TimeSpan.Parse);
        public static int LogViewCount => appConfig.Get("log-view-count", int.Parse);
        public static int TextFileSaveBuffer => appConfig.Get("text-file-save-buffer", int.Parse);
        public static int TextStreamWriteBuffer => appConfig.Get("text-stream-write-buffer", int.Parse);

        public static int BackupArchiveCount => appConfig.Get("backup-archive-count", int.Parse);
        public static int BackupSettingCount => appConfig.Get("backup-setting-count", int.Parse);
        public static int BackupWebNavigatorGeckoFxPluginCount => appConfig.Get("backup-web_navigator-geckofx-plugin-count", int.Parse);

        /// <summary>
        /// プレイヤー部分のカーソルを隠すまでの時間。
        /// </summary>
        public static TimeSpan PlayerCursorHideTime => appConfig.Get("player-cursor-hide-time", TimeSpan.Parse);
        /// <summary>
        /// キャッシュファイル有効期間。
        /// </summary>
        public static TimeSpan SettingApplicationCacheLifeTime => appConfig.Get("setting-application-cache-life-time", TimeSpan.Parse);

        /// <summary>
        /// about:config
        ///<para>plugin.scan.plid.all</para>
        /// </summary>
        public static bool SettingApplicationWebNavigatorGeckoFxScanPlugin => appConfig.Get("setting-application-web_navigator-geckofx-scan-plugin", bool.Parse);

        public static bool SettingApplicationNetworkLogicUsingCustomUserAgent => appConfig.Get("setting-application-network-logic-using-custom-ua", bool.Parse);
        public static string SettingApplicationNetworkLogicCustomUserAgentFormat => appConfig.Get("setting-application-network-logic-custom-ua-format");
        public static bool SettingApplicationNetworkBrowserUsingCustomUserAgent => appConfig.Get("setting-application-network-browser-using-custom-ua", bool.Parse);
        public static string SettingApplicationNetworkBrowserCustomUserAgentFormat => appConfig.Get("setting-application-network-browser-custom-ua-format");


        /// <summary>
        /// ウィンドウ: 左
        /// </summary>
        public static int SettingApplicationWindowLeft => appConfig.Get("setting-application-window-top", int.Parse);
        /// <summary>
        /// ウィンドウ: 上
        /// </summary>
        public static int SettingApplicationWindowTop => appConfig.Get("setting-application-window-left", int.Parse);
        /// <summary>
        /// ウィンドウ: 幅
        /// </summary>
        public static int SettingApplicationWindowWidth => appConfig.Get("setting-application-window-width", int.Parse);
        /// <summary>
        /// ウィンドウ: 高さ
        /// </summary>
        public static int SettingApplicationWindowHeight => appConfig.Get("setting-application-window-height", int.Parse);

        /// <summary>
        /// ビューの拡大率。
        /// </summary>
        public static double SettingApplicationViewScale => appConfig.Get("setting-application-view-scale", double.Parse);

        /// <summary>
        /// スクリーンセーバー・ロックの抑制。
        /// </summary>
        public static bool SettingApplicationSystemBreakSuppression => appConfig.Get("setting-application-system-break-suppression", bool.Parse);

        /// <summary>
        /// テーマ: ランダム。
        /// </summary>
        public static bool SettingApplicationThemeIsRandom => appConfig.Get("setting-application-theme-is-random", bool.Parse);
        public static string SettingApplicationThemeApplicationTheme => appConfig.Get("setting-application-theme-application-theme");
        public static string SettingApplicationThemeBaseTheme => appConfig.Get("setting-application-theme-base-theme");
        public static string SettingApplicationThemeAccent => appConfig.Get("setting-application-theme-accent");

        public static IReadOnlyRange<int> NavigatorVolumeRange => appConfig.Get("navigator-volume-range", RangeModel.Parse<int>);

        /// <summary>
        /// サービスへのログインを自動的に行うか。
        /// </summary>
        public static bool SettingApplicationServiceEnabledStartupAutoLogin => appConfig.Get("setting-application-service-enabled-startup-auto-login", bool.Parse);

        /// <summary>
        /// タブヘッダ部をマウスホイールでスクロールするか。
        /// <para>暫定対応: https://groups.google.com/d/topic/mnmn-forum/jm663Y8Wnn4/discussion </para>
        /// </summary>
        public static bool FinderTabTabHeaderEnabledMouseWheel => appConfig.Get("finder_tab-tab_header-enabled-mouse-wheel", bool.Parse);

        public static string HttpRequestResponseHeaderAccept => appConfig.Get("http-request_response-header-accept");
        public static string HttpRequestResponseHeaderAcceptEncoding => appConfig.Get("http-request_response-header-accept-encoding");

        public static bool ComboBoxInputIme549Enabled => appConfig.Get("combobox-input-ime-549-enabled", bool.Parse);

        #endregion
    }
}
