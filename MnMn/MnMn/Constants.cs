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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
        #region proeprty

        public static string ApplicationName { get; } = "MnMn";

        public static string AssemblyPath { get; } = Assembly.GetExecutingAssembly().Location;
        public static string AssemblyRootDirectoryPath { get; } = Path.GetDirectoryName(AssemblyPath);

#if DEBUG
        public static string ApplicationDirectoryName { get; } = ApplicationName + "-debug";
#else
        public static string ApplicationDirectoryName  { get; } =  ApplicationName;
#endif

        /// <summary>
        /// バージョン番号。
        /// </summary>
        public static readonly Version ApplicationVersionNumber = Assembly.GetExecutingAssembly().GetName().Version;


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

        public static CacheSpan ServiceSmileUserDataCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(12));
        public static CacheSpan ServiceSmileUserImageCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromDays(3));

        public static CacheSpan ServiceSmileMyListCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(12));

        public static CacheSpan ServiceSmileVideoThumbCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(12));
        public static CacheSpan ServiceSmileVideoImageCacheSpan => CacheSpan.InfinityCache;
        public static CacheSpan ServiceSmileVideoMsgCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(1));
        public static CacheSpan ServiceSmileVideoRelationCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(1));
        public static CacheSpan ServiceSmileVideoCheckItLaterCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromDays(7));

        public static string SbinDirectoryName { get; } = "sbin";
        public static string UpdateProgramDirectoryName { get; } = "Updater";
        public static string UpdateProgramName { get; } = PathUtility.AppendExtension(UpdateProgramDirectoryName, "exe");

#if DEBUG
        public static readonly TimeSpan updateWaitTime = TimeSpan.FromSeconds(1);
#else
		public static readonly TimeSpan updateWaitTime = TimeSpan.FromSeconds(30);
#endif

        /// <summary>
        /// etc/
        /// </summary>
        public static string ApplicationEtcDirectoryPath { get { return Path.Combine(AssemblyRootDirectoryPath, "etc"); } }

        public static string ScriptDirectoryName { get; } = "script";

        public static string ArchiveSearchPattern { get; } = "*.zip";


        public static string SettingDirectoryName { get; } = "setting";
        public static string SettingFileName { get; } = "setting.json";

        public static string ArchiveDirectoryName { get; } = "archive";

        public static string ServiceName { get; } = "service";
        public static string ServiceSmileName { get; } = "smile";
        public static string ServiceSmileVideoName { get; } = "video";

        public static string BinaryDirectoryName { get; } = "bin";

        public static string FfmpegApplicationPath { get; } = Path.Combine(AssemblyRootDirectoryPath, BinaryDirectoryName, "ffmpeg", "ffmpeg.exe");

        public static string DefineName { get; } = "define";

        public static string EtcDirectoryPath { get; } = Path.Combine(AssemblyRootDirectoryPath, "etc");

        public static string DefineDirectoryPath { get; } = Path.Combine(EtcDirectoryPath, DefineName);

        public static string DefineServiceDirectoryPath { get; } = Path.Combine(DefineDirectoryPath, ServiceName);

        public static string DefineSmileDirectoryPath { get; } = Path.Combine(DefineServiceDirectoryPath, ServiceSmileName);

        public static string SmileUriListPath { get; } = Path.Combine(DefineSmileDirectoryPath, "uri-list.xml");
        public static string SmileUriParametersListPath { get; } = Path.Combine(DefineSmileDirectoryPath, "uri-params.xml");
        public static string SmileRequestParametersListPath { get; } = Path.Combine(DefineSmileDirectoryPath, "request-params.xml");
        public static string SmileRequestMappingsListPath { get; } = Path.Combine(DefineSmileDirectoryPath, "request-mappings.xml");

        public static string DefineSmileVideoDirectoryPath { get; } = Path.Combine(DefineSmileDirectoryPath, ServiceSmileVideoName);

        public static string SmileVideoUriListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "uri-list.xml");
        public static string SmileVideoUriParametersListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "uri-params.xml");
        public static string SmileVideoRequestParametersListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "request-params.xml");
        public static string SmileVideoRequestMappingsListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "request-mappings.xml");

        public static string SmileVideoRankingPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "ranking.xml");
        public static string SmileVideoSearchPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "search.xml");
        public static string SmileVideoMyListPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "mylist.xml");
        public static string SmileVideoFilteringPath { get; } = Path.Combine(DefineSmileVideoDirectoryPath, "filtering.xml");

        public static TimeSpan ApplicationCacheLifeTime { get; } = TimeSpan.FromDays(3);

        public static string SmileUserCacheDirectoryName { get; } = "user";
        public static string SmileMyListCacheDirectoryName { get; } = "mylist";
        //public static string SmileVideoCacheVideosDirectoryName { get; } = "videos";

        public static double SmileVideoCommentFontSize { get; } = System.Windows.SystemFonts.MessageFontSize * 1.8;
        public static string SmileVideoCommentFontFamily { get; } = System.Windows.SystemFonts.MessageFontFamily.FamilyNames.Values.First();
        public static double SmileVideoCommentFontAlpha { get; } = 1;
        public static bool SmileVideoCommentFontBold { get; } = false;
        public static bool SmileVideoCommentFontItalic { get; } = false;
        public static TimeSpan SmileVideoCommentShowTime { get; } = TimeSpan.FromSeconds(3);
        public static bool SmileVideoCommentConvertPairYenSlash { get; } = true;
        public static bool SmileVideoCommentIsEnabledSharedNoGood { get; } = true;
        public static int SmileVideoCommentSharedNoGoodScore { get; } = -1500;
        public static bool SmileVideoCommentPostAnonymous { get; } = true;

        public static int SmileMyListHistoryCount { get; } = 50;

        public static int SmileVideoSearchCount { get; } = 100;
        public static bool SmileVideoAutoPlay { get; } = true;
        public static bool SmileVideoLoadVideoInformation { get; } = true;
        public static int SmileVideoPlayerDisplayCommentLimitCount { get; } = 25;
        public static TextShowKind SmileVideoPlayerTextShowKind { get; } = TextShowKind.Shadow;
        public static Color SmileVideoMyListFolderColor { get; } = Colors.SkyBlue;
        public static bool SmileVideoPlayerShowDetailArea { get; } = true;
        public static bool SmileVideoPlayerShowCommentList { get; } = true;
        public static bool SmileVideoPlayerShowPostTimestamp { get; } = false;
        public static bool PlayerVisibleComment { get; } = true;

        public static string CurrentLanguageCode => "ja-jp";

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

        #region function

        /// <summary>
        /// 文字列リテラルを書式で変換。
        ///
        /// {...} を置き換える。
        /// * TIMESTAMP: そんとき
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private static string ReplaceAppConfig(string src)
        {
            var map = new Dictionary<string, string>() {
                { "TIMESTAMP", DateTime.Now.ToBinary().ToString() },
            };
            var replacedText = src.ReplaceRangeFromDictionary("{", "}", map);

            return replacedText;
        }

        #endregion
    }
}
