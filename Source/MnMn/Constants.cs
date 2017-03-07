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

        /// <summary>
        /// 前回バージョンがこれ未満なら使用許諾を表示
        /// </summary>
        public static Version AcceptVersion { get; } = new Version(0, 49, 1, 0);
        
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

        /// <summary>
        /// ニコニコ動画: ユーザーデータキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileUserDataCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(12));
        /// <summary>
        /// ニコニコ動画: ユーザー画像キャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileUserImageCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromDays(3));
        /// <summary>
        /// ニコニコ動画: マイリストキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileMyListCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(12));
        /// <summary>
        /// ニコニコ動画: 動画情報キャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileVideoThumbCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(12));
        /// <summary>
        /// ニコニコ動画: 動画サムネイルキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileVideoImageCacheSpan => CacheSpan.InfinityCache;
        /// <summary>
        /// ニコニコ動画: コメントキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileVideoMsgCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(1));
        /// <summary>
        /// ニコニコ動画: 関連動画情報キャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileVideoRelationCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(1));
        /// <summary>
        /// ニコニコ動画: あとで見る の保持有効期間。
        /// </summary>
        public static CacheSpan ServiceSmileVideoCheckItLaterCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromDays(7));
        /// <summary>
        /// ニコニコ生放送: 動画情報キャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileLiveInformationCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(12));
        /// <summary>
        /// ニコニコ生放送: 動画サムネイルキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileLiveImageCacheSpan => CacheSpan.InfinityCache;
        /// <summary>
        /// ニコニコ市場: 動画サムネイルキャッシュ時間。
        /// </summary>
        public static CacheSpan ServiceSmileMarketImageCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromDays(7));
        /// <summary>
        /// sbin/
        /// </summary>
        public static string SbinDirectoryPath { get { return Path.Combine(AssemblyRootDirectoryPath, "sbin"); } }

        /// <summary>
        /// 更新プログラムファイルパス。
        /// </summary>
        public static string UpdaterExecuteFilePath { get { return Path.Combine(SbinDirectoryPath, "Updater", "Updater.exe"); } }

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
        /// <summary>
        /// バックアップディレクトリ名。
        /// </summary>
        public static string BackupDirectoryName { get; } = "backup";
        /// <summary>
        /// バックアップファイルローテートパターン。
        /// </summary>
        public static string BackupSearchPattern { get; } = "*.json.gz";

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
        ///
        /// </summary>
        public static string BinaryDirectoryPath { get { return Path.Combine(AssemblyRootDirectoryPath, "bin"); } }

        public static string CrashReporterApplicationPath { get; } = Path.Combine(BinaryDirectoryPath, "CrashReporter", "CrashReporter.exe");
        public static string FfmpegApplicationPath { get; } = Path.Combine(BinaryDirectoryPath, "ffmpeg", "ffmpeg.exe");

        public static string DefineName { get; } = "define";

        /// <summary>
        /// etc/define
        /// </summary>
        public static string DefineDirectoryPath { get; } = Path.Combine(EtcDirectoryPath, DefineName);

        public static string ApplicationThemeDefinePath { get; } = Path.Combine(DefineDirectoryPath, "theme.xml");
        public static string ApplicationWebNavigatorBridgePath { get; } = Path.Combine(DefineDirectoryPath, "web-navigator.xml");

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
        public static string SmileUserInformationPath { get; } = Path.Combine(DefineSmileDirectoryPath, "user.xml");

        public static string DefineSmileVideoDirectoryPath { get; } = Path.Combine(DefineSmileDirectoryPath, ServiceSmileVideoName);
        public static string DefineSmileLiveDirectoryPath { get; } = Path.Combine(DefineSmileDirectoryPath, ServiceSmileLiveName);

        public static string SmileVideoUriListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "uri-list.xml");
        public static string SmileVideoUriParametersListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "uri-params.xml");
        public static string SmileVideoRequestHeadersListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "request-headers.xml");
        public static string SmileVideoRequestParametersListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "request-params.xml");
        public static string SmileVideoRequestMappingsListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "request-mappings.xml");

        public static string SmileVideoFinderItemName { get; } = "abc";

        public static string SmileVideoRankingPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "ranking.xml");
        public static string SmileVideoSearchPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "search.xml");
        public static string SmileVideoMyListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "mylist.xml");
        public static string SmileVideoFilteringPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "filtering.xml");

        public static string SmileUserCacheDirectoryName { get; } = "user";
        public static string SmileMyListCacheDirectoryName { get; } = "mylist";
        public static string SmileMarketCacheDirectoryName { get; } = "market";

        public static char SmileMyListBookmarkTagTokenSplitter { get; } = ';';

        public static string SmileLiveUriListPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "uri-list.xml");
        public static string SmileLiveUriParametersListPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "uri-params.xml");
        public static string SmileLiveRequestHeadersListPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "request-headers.xml");
        public static string SmileLiveRequestParametersListPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "request-params.xml");
        public static string SmileLiveRequestMappingsListPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "request-mappings.xml");

        public static string SmileLiveCategoryPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "category.xml");
        //public static string SmileLivePlayerDirectoryPath { get; } = Path.Combine(DefineSmileLiveDirectoryPath, "player");
        //public static string SmileLivePlayerContainerPath { get; } = Path.Combine(SmileLivePlayerDirectoryPath, "container.html");

        //public static string SmileVideoCacheVideosDirectoryName { get; } = "videos";

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
