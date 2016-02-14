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

        public static string ApplicationName => "MnMn";

        public static string AssemblyPath => Assembly.GetExecutingAssembly().Location;
        public static string AssemblyParentDirectoryPath => Path.GetDirectoryName(AssemblyPath);

#if DEBUG
        public static string ApplicationDirectoryName => ApplicationName + "-debug";
#else
        public static string ApplicationDirectoryName => ApplicationName;
#endif

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

        public static CacheSpan ServiceSmileVideoThumbCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(12));
        public static CacheSpan ServiceSmileVideoImageCacheSpan => CacheSpan.InfinityCache;
        public static CacheSpan ServiceSmileVideoMsgCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(1));

        public static string ServiceName { get; } = "service";
        public static string ServiceSmileName { get; } = "smile";
        public static string ServiceSmileVideoName { get; } = "video";

        public static string DefineName { get; } = "define";

        public static string EtcDirectoryPath { get; } = Path.Combine(AssemblyParentDirectoryPath, "etc");

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

        public static string SmileVideoRankingPath => Path.Combine(DefineSmileVideoDirectoryPath, "ranking.xml");
        public static string SmileVideoSearchPath => Path.Combine(DefineSmileVideoDirectoryPath, "search.xml");

        public static string SmileVideoCacheVideosDirectoryName => "videos";

        public static double CommentFontSize => System.Windows.SystemFonts.MessageFontSize * 2;
        public static string CommentFontFamily => System.Windows.SystemFonts.MessageFontFamily.FamilyNames.FirstOrDefault(x => string.Compare(x.Key.IetfLanguageTag, CurrentLanguageCode, true) == 0).Value;
        public static double CommentFontAlpha => 1;
        public static TimeSpan CommentShowTime => TimeSpan.FromSeconds(3);

        public static int SmileVideoSearchCount => 100;
        public static bool SmileVideoLoadVideoTime => true;

        public static string CurrentLanguageCode => "ja-JP";

        /// <summary>
        /// 設定ディレクトリパス。
        /// </summary>
        public static string UserDirectoryPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary>
        /// データ保存ディレクトリパス。
        /// </summary>
        public static string CacheDirectoryPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

#endregion
    }
}
