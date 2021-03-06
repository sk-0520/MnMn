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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.ReadOnly;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn
{
    /// <summary>
    /// 各種定数。
    /// </summary>
    public partial class Constants
    {
        const string buildTypeDebug = "DEBUG";
        const string buildTypeBeta = "β";
        const string buildTypeRelease = "RELEASE";

        #region proeprty

        /// <summary>
        /// アプリケーション名。
        /// </summary>
        public const string applicationName = "MnMn";

        /// <summary>
        /// アプリケーション名。
        /// </summary>
        public static string ApplicationName { get; } = applicationName;

#if DEBUG
        /// <summary>
        /// アプリケーション使用名。
        /// </summary>
        public static string ApplicationUsingName { get; } = ApplicationName + "-debug";
#elif BETA
        /// <summary>
        /// アプリケーション使用名。
        /// </summary>
        public static string ApplicationUsingName { get; } = ApplicationName + "-beta";
#else
        /// <summary>
        /// アプリケーション使用名。
        /// </summary>
        public static string ApplicationUsingName { get; } = ApplicationName;
#endif
        /// <summary>
        /// ビルドタイプ。
        /// </summary>
        public static string BuildType
        {
            get
            {
#if DEBUG
                return buildTypeDebug;
#elif BETA
                return buildTypeBeta;
#else
                return buildTypeRelease;
#endif
            }
        }

        /// <summary>
        /// <see cref="BuildType"/>とやってること一緒だけどリリース版は何も返さない。
        /// </summary>
        public static string BuildTypeInformation
        {
            get
            {
#if DEBUG
                return BuildType;
#elif BETA
                return BuildType;
#else
                return string.Empty;
#endif
            }

        }

        /// <summary>
        /// アプリケーションパス。
        /// </summary>
        public static string AssemblyPath { get; } = Assembly.GetExecutingAssembly().Location;
        /// <summary>
        /// アプリケーション親ディレクトリパス。
        /// </summary>
        public static string AssemblyRootDirectoryPath { get; } = Path.GetDirectoryName(AssemblyPath);
        /// <summary>
        /// アプリケーションが使用するディレクトリ名。
        /// </summary>
        public static string ApplicationDirectoryName { get; } = ApplicationUsingName;

        /// <summary>
        /// バージョン番号。
        /// </summary>
        public static Version ApplicationVersionNumber { get; } = Assembly.GetExecutingAssembly().GetName().Version;
        public static string ApplicationVersionNumberText { get; } = ApplicationVersionNumber.ToString();
        /// <summary>
        /// バージョンリビジョン。
        /// </summary>
        public static string ApplicationVersionRevision { get; } = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;
        /// <summary>
        /// アプリケーションバージョン。
        /// </summary>
        public static string ApplicationVersion { get; } = ApplicationVersionNumber.ToString() + "-" + ApplicationVersionRevision;

        public static DateTime LightweightUpdateNone { get; } = DateTime.MinValue;

        public static string FormatTimestampFileName { get; } = "yyyy-MM-dd_HH-mm-ss";

        #region tag

        public static object TagTabCloseButton { get; } = "TabCloseButton";

        #endregion

        /// <summary>
        /// 最小XMLファイルサイズ。
        /// </summary>
        public static long MinimumXmlFileSize { get; } = "<x/>".Length;
        /// <summary>
        /// 最小PNGファイルサイズ。
        /// <para>http://yosiopp.net/archives/225</para>
        /// </summary>
        public static long MinimumPngFileSize { get; } = 67;
        /// <summary>
        /// 最小Jsonファイルサイズ。
        /// </summary>
        public static long MinimumJsonFileSize { get; } = "{}".Length;
        /// <summary>
        /// 最小HTMLサイズ。
        /// <para>http://qiita.com/wakaba@github/items/ec36c9d67707d8fb4395</para>
        /// </summary>
        public static long MinimumHtmlFileSize { get; } = "<!DOCTYPE html><title></title>h".Length;

        public static string ServiceSmileVideoGetVideoError = "SERVICE:SMILE-VIDEO: GET ERROR";

        /// <summary>
        /// ニコニコ動画: ユーザーデータキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileUserDataCacheSpan => new CacheSpan(DateTime.Now, ServiceSmileUserDataCacheTime);
        /// <summary>
        /// ニコニコ動画: ユーザー画像キャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileUserImageCacheSpan => new CacheSpan(DateTime.Now, ServiceSmileUserImageCacheTime);
        /// <summary>
        /// ニコニコ動画: マイリストキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileMyListCacheSpan => new CacheSpan(DateTime.Now, ServiceSmileMyListCacheTime);
        /// <summary>
        /// ニコニコ動画: 動画情報キャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileVideoThumbCacheSpan => new CacheSpan(DateTime.Now, ServiceSmileVideoThumbCacheTime);
        /// <summary>
        /// ニコニコ動画: 動画サムネイルキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileVideoImageCacheSpan => CacheSpan.InfinityCache;
        /// <summary>
        /// ニコニコ動画: コメントキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileVideoMsgCacheSpan => new CacheSpan(DateTime.Now, ServiceSmileVideoMsgCacheTime);
        /// <summary>
        /// ニコニコ動画: 関連動画情報キャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileVideoRelationCacheSpan => new CacheSpan(DateTime.Now, ServiceSmileVideoRelationCacheTime);
        /// <summary>
        /// ニコニコ動画: あとで見る の保持有効期間。
        /// </summary>
        public static CacheSpan ServiceSmileVideoCheckItLaterCacheSpan => new CacheSpan(DateTime.Now, ServiceSmileVideoCheckItLaterCacheTime);
        /// <summary>
        /// ニコニコ生放送: 動画情報キャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileLiveInformationCacheSpan => new CacheSpan(DateTime.Now, ServiceSmileLiveInformationCacheTime);
        /// <summary>
        /// ニコニコ生放送: 動画サムネイルキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileLiveImageCacheSpan => CacheSpan.InfinityCache;
        /// <summary>
        /// ニコニコ市場: 動画サムネイルキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileMarketImageCacheSpan => new CacheSpan(DateTime.Now, ServiceSmileMarketImageCacheTime);
        /// <summary>
        /// ニコニコチャンネル: ユーザーデータキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileChannelDataCacheSpan => new CacheSpan(DateTime.Now, ServiceSmileChannelDataCacheTime);
        public static CacheSpan ServiceSmileChannelImageCacheSpan => new CacheSpan(DateTime.Now, ServiceSmileChannelImageCacheTime);

        /// <summary>
        /// sbin/
        /// </summary>
        public static string SbinDirectoryPath { get { return Path.Combine(AssemblyRootDirectoryPath, "sbin"); } }

        /// <summary>
        /// 更新プログラムファイルパス。
        /// </summary>
        public static string UpdaterExecuteFilePath { get { return Path.Combine(SbinDirectoryPath, "Updater", "Updater.exe"); } }
        /// <summary>
        /// 更新アーカイブ展開プログラムファイルパス。
        /// </summary>
        public static string ExtractorExecuteFilePath { get { return Path.Combine(SbinDirectoryPath, "Extractor", "Extractor.exe"); } }

        /// <summary>
        /// etc/
        /// </summary>
        public static string ApplicationEtcDirectoryPath { get { return Path.Combine(AssemblyRootDirectoryPath, "etc"); } }
        /// <summary>
        /// doc/
        /// </summary>
        public static string ApplicationDocDirectoryPath { get { return Path.Combine(AssemblyRootDirectoryPath, "doc"); } }

        /// <summary>
        /// etc/
        /// </summary>
        public static string EtcDirectoryPath { get; } = Path.Combine(AssemblyRootDirectoryPath, "etc");

        /// <summary>
        /// lib/
        /// </summary>
        public static string LibraryDirectoryPath { get; } = Path.Combine(AssemblyRootDirectoryPath, "lib");

        public static string WebNavigatorGeckoFxLibraryDirectoryPath { get; } = Path.Combine(LibraryDirectoryPath, WebNavigatorGeckoFxLibraryDirectoryName);
        public static string WebNavigatorGeckoFxPluginsDirectoryPath { get; } = Path.Combine(WebNavigatorGeckoFxLibraryDirectoryPath, WebNavigatorGeckoFxPluginsDirectoryName);
        public static string WebNavigatorGeckoFxExtensionsDirectoryPath { get; } = Path.Combine(WebNavigatorGeckoFxLibraryDirectoryPath, WebNavigatorGeckoFxExtensionsDirectoryName);

        /// <summary>
        /// 外部ライブラリファイルパス。
        /// </summary>
        public static string ComponentListFileName { get { return Path.Combine(ApplicationDocDirectoryPath, "components.xml"); } }
        /// <summary>
        /// ヘルプファイルパス。
        /// </summary>
        public static string HelpFilePath { get { return Path.Combine(ApplicationDocDirectoryPath, "help.html"); } }
        /// <summary>
        /// スクリプトディレクトリ名。
        /// </summary>
        public static string ScriptDirectoryName { get; } = "script";
        public static string SpaghettiDirectoryName { get; } = "spaghetti";
        public static string SpaghettiDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, ScriptDirectoryName, SpaghettiDirectoryName); } }
        public static string SpaghettiScriptSplit { get; } = "@";
        public static string SpaghettiScriptSearchPattern { get; } = "*.xml";
        /// <summary>
        /// アーカイブファイル検索パターン。
        /// <para>ワイルドカード。</para>
        /// </summary>
        public static string ArchiveSearchPattern { get; } = "*.zip";

        public static string ArchiveWebNavigatorGeckFxPluginSearchPattern { get; } = "*.zip";

        /// <summary>
        /// 設定ディレクトリ名。
        /// </summary>
        public static string SettingDirectoryName { get; } = "setting";
        /// <summary>
        /// 設定ファイル名。
        /// </summary>
        public static string SettingFileName { get; } = "setting.json";
        /// <summary>
        /// セーフモードで使用する設定ファイル名。
        /// </summary>
        public static string SafeModeSettingFileName { get; }= "safe-mode_setting.json";
        /// <summary>
        /// 情報ファイル名。
        /// </summary>
        public static string InformationFileName { get; } = "information.txt";
        /// <summary>
        /// ログファイル名。
        /// </summary>
        public static string LogFileName { get; } = "log.log";
        /// <summary>
        /// アーカイブディレクトリ名。
        /// </summary>
        public static string ArchiveDirectoryName { get; } = "archive";
        public static string ArchiveWebNavigatorGeckFxPluginDirectoryName { get; } = "archive-plugin";
        public static string ArchiveLightweightUpdateDirectoryName { get; } = "archive-lightweight_update";
        public static TimeSpan ArchiveLightweightUpdateTimeout => TimeSpan.FromMinutes(10);
        public static TimeSpan ArchiveLightweightUpdateWaitTime => TimeSpan.FromSeconds(1);
        /// <summary>
        /// バックアップディレクトリ名。
        /// </summary>
        public static string BackupDirectoryName { get; } = "backup";
        /// <summary>
        /// 既存バックアップファイルローテートパターン。
        /// <para>#623でzipに変わるからその互換性保持。</para>
        /// </summary>
        public static string BackupSearchPattern_Issue623 { get; } = "*.json.gz";
        /// <summary>
        /// バックアップファイルローテートパターン。
        /// </summary>
        public static string BackupSearchPattern { get; } = "*.zip";

        public static string PublicExportFileNamePattern { get; } = "*.zip";

        /// <summary>
        /// クラッシュレポート保管ディレクトリ名。
        /// </summary>
        public static string CrashReportDirectoryName { get; } = "crash";
        public static string CrashReportFileExtension { get; } = "crash-log.xml";
        public static string CrashReportSearchPattern { get; } = "*." + CrashReportFileExtension;
        public static int CrashReportCount { get; } = 10;

        /// <summary>
        /// /サービス名。
        /// </summary>
        public static string ServiceName { get; } = "service";
        /// <summary>
        ///
        /// </summary>
        public static string ServiceCommonName { get; } = "common";
        /// <summary>
        /// サービス名: ニコニコ。
        /// </summary>
        public static string ServiceSmileName { get; } = "smile";
        /// <summary>
        /// サービス名: ニコニコ動画。
        /// </summary>
        public static string ServiceSmileVideoName { get; } = "video";
        /// <summary>
        /// サービス名: ニコニコ生放送。
        /// </summary>
        public static string ServiceSmileLiveName { get; } = "live";
        /// <summary>
        /// サービス名: Twitter。
        /// </summary>
        public static string ServiceIdleTalkName { get; } = "idle_talk";
        /// <summary>
        /// サービス名: TwitterのTweet。
        /// </summary>
        public static string ServiceIdleTalkMutterName { get; } = "mutter";
        /// <summary>
        ///
        /// </summary>
        public static string BinaryDirectoryPath { get { return Path.Combine(AssemblyRootDirectoryPath, "bin"); } }

        public static string CrashReporterApplicationPath { get; } = Path.Combine(BinaryDirectoryPath, "CrashReporter", "CrashReporter.exe");
        public static string FfmpegApplicationPath { get; } = Path.Combine(BinaryDirectoryPath, "ffmpeg", "ffmpeg.exe");

        public static string ApplicationTemplateDirectoryPath { get; } = Path.Combine(EtcDirectoryPath, "template");
        public static string ApplicationLightweightUpdateHtmlTemplatePath { get; } = Path.Combine(ApplicationTemplateDirectoryPath, "lightweight-update.html");

        public static string DefineName { get; } = "define";

        /// <summary>
        /// etc/define
        /// </summary>
        public static string DefineDirectoryPath { get; } = Path.Combine(EtcDirectoryPath, DefineName);

        public static string ApplicationThemeDefinePath { get; } = Path.Combine(DefineDirectoryPath, "theme.xml");
        public static string ApplicationWebNavigatorBridgePath { get; } = Path.Combine(DefineDirectoryPath, "web-navigator.xml");
        public static string ApplicationAcceptVersionPath { get; } = Path.Combine(DefineDirectoryPath, "accept-version.xml");

        public static string DefineCommonDirectoryPath { get; } = Path.Combine(DefineDirectoryPath, ServiceCommonName);
        public static string CommonUriListPath { get; } = Path.Combine(DefineCommonDirectoryPath, "uri-list.xml");
        public static string CommonUriParametersListPath { get; } = Path.Combine(DefineCommonDirectoryPath, "uri-params.xml");
        public static string CommonRequestHeadersListPath { get; } = Path.Combine(DefineCommonDirectoryPath, "request-headers.xml");
        public static string CommonRequestParametersListPath { get; } = Path.Combine(DefineCommonDirectoryPath, "request-params.xml");
        public static string CommonRequestMappingsListPath { get; } = Path.Combine(DefineCommonDirectoryPath, "request-mappings.xml");
        public static string CommonExpressionsPath { get; } = Path.Combine(DefineCommonDirectoryPath, "expressions.xml");

        /// <summary>
        /// etc/define/service
        /// </summary>
        public static string DefineServiceDirectoryPath { get; } = Path.Combine(DefineDirectoryPath, ServiceName);

        /// <summary>
        /// etc/define/smile
        /// </summary>
        public static string DefineSmileDirectoryPath { get; } = Path.Combine(DefineServiceDirectoryPath, ServiceSmileName);

        public static IReadOnlyRange<int> FrameworkFps { get; } = RangeModel.Create(10, 60);
        public static double FrameworkFpsMinimum { get; } = FrameworkFps.Head;
        public static double FrameworkFpsMaximum { get; } = FrameworkFps.Tail;

        public static string SmileUriListPath { get; } = Path.Combine(DefineSmileDirectoryPath, "uri-list.xml");
        public static string SmileUriParametersListPath { get; } = Path.Combine(DefineSmileDirectoryPath, "uri-params.xml");
        public static string SmileRequestHeadersListPath { get; } = Path.Combine(DefineSmileDirectoryPath, "request-headers.xml");
        public static string SmileRequestParametersListPath { get; } = Path.Combine(DefineSmileDirectoryPath, "request-params.xml");
        public static string SmileRequestMappingsListPath { get; } = Path.Combine(DefineSmileDirectoryPath, "request-mappings.xml");
        public static string SmileExpressionsPath { get; } = Path.Combine(DefineSmileDirectoryPath, "expressions.xml");
        public static string SmileUserInformationPath { get; } = Path.Combine(DefineSmileDirectoryPath, "user.xml");

        public static string DefineSmileVideoDirectoryPath { get; } = Path.Combine(DefineSmileDirectoryPath, ServiceSmileVideoName);
        public static string DefineSmileLiveDirectoryPath { get; } = Path.Combine(DefineSmileDirectoryPath, ServiceSmileLiveName);

        public static string SmileVideoUriListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "uri-list.xml");
        public static string SmileVideoUriParametersListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "uri-params.xml");
        public static string SmileVideoRequestHeadersListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "request-headers.xml");
        public static string SmileVideoRequestParametersListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "request-params.xml");
        public static string SmileVideoRequestMappingsListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "request-mappings.xml");
        public static string SmileVideoExpressionsPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "expressions.xml");

        public static string SmileVideoFinderItemName { get; } = "abc";

        public static string SmileVideoRankingPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "ranking.xml");
        public static string SmileVideoSearchPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "search.xml");
        public static string SmileVideoMyListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "mylist.xml");
        public static string SmileVideoFilteringPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "filtering.xml");
        public static string SmileVideoKeywordPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "keyword.xml");
        public static string SmileVideoMsgPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "msg.xml");

        public static string SmileUserCacheDirectoryName { get; } = "user";
        public static string SmileMyListCacheDirectoryName { get; } = "mylist";
        public static string SmileMarketCacheDirectoryName { get; } = "market";
        public static string SmileChannelCacheDirectoryName { get; } = "channel";

        public static char SmileMyListBookmarkTagTokenSplitter { get; } = ';';

        public static string SmileLiveUriListPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "uri-list.xml");
        public static string SmileLiveUriParametersListPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "uri-params.xml");
        public static string SmileLiveRequestHeadersListPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "request-headers.xml");
        public static string SmileLiveRequestParametersListPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "request-params.xml");
        public static string SmileLiveRequestMappingsListPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "request-mappings.xml");
        public static string SmileLiveExpressionsPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "expressions.xml");

        public static string SmileLiveCategoryPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "category.xml");
        //public static string SmileLivePlayerDirectoryPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "player");
        //public static string SmileLivePlayerContainerPath { get; } = Path.Combine(SmileLivePlayerDirectoryPath, "container.html");

        //public static string SmileVideoCacheVideosDirectoryName { get; } = "videos";

        public static string DefineIdleTalkDirectoryPath { get; } = Path.Combine(DefineServiceDirectoryPath, ServiceIdleTalkName);
        public static string DefineIdleTalkMutterDirectoryPath { get; } = Path.Combine(DefineIdleTalkDirectoryPath, ServiceIdleTalkMutterName);

        public static string IdleTalkUriListPath { get; } = null;
        public static string IdleTalkUriParametersListPath { get; } = null;
        public static string IdleTalkRequestHeadersListPath { get; } = null;
        public static string IdleTalkRequestParametersListPath { get; } = null;
        public static string IdleTalkRequestMappingsListPath { get; } = null;
        public static string IdleTalkExpressionsPath { get; } = null;

        public static string IdleTalkMutterUriListPath { get; } = Path.Combine(DefineIdleTalkMutterDirectoryPath, "uri-list.xml");
        public static string IdleTalkMutterUriParametersListPath { get; } = Path.Combine(DefineIdleTalkMutterDirectoryPath, "uri-params.xml");
        public static string IdleTalkMutterRequestHeadersListPath { get; } = Path.Combine(DefineIdleTalkMutterDirectoryPath, "request-headers.xml");
        public static string IdleTalkMutterRequestParametersListPath { get; } = Path.Combine(DefineIdleTalkMutterDirectoryPath, "request-params.xml");
        public static string IdleTalkMutterRequestMappingsListPath { get; } = Path.Combine(DefineIdleTalkMutterDirectoryPath, "request-mappings.xml");
        public static string IdleTalkMutterExpressionsPath { get; } = Path.Combine(DefineIdleTalkMutterDirectoryPath, "expressions.xml");

        ///// <summary>
        ///// TODO: カルチャからどうこうしたい。
        ///// </summary>
        //public static string CurrentLanguageCode => "ja-jp";

        public static string ExtensionTemporaryFile { get; } = "tmp";

        /// <summary>
        /// 設定ディレクトリパス。
        /// </summary>
        public static string UserDirectoryPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary>
        /// データ保存ディレクトリパス。
        /// </summary>
        public static string CacheDirectoryPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        #endregion

    }
}
