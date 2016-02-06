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

        /// <summary>
        /// 最小XMLファイルサイズ。
        /// </summary>
        public static long MinimumXmlFileSize => "<x/>".Length;
        /// <summary>
        /// 最小PNGファイルサイズ。
        /// <para>http://yosiopp.net/archives/225</para>
        /// </summary>
        public static long MinimumPngFileSize => 67;

        public static CacheSpan ServiceSmileVideoThumbCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(12));
        public static CacheSpan ServiceSmileVideoImageCacheSpan => CacheSpan.InfinityCache;
        public static CacheSpan ServiceSmileVideoMsgCacheSpan => new CacheSpan(DateTime.Now, TimeSpan.FromHours(1));

        public static string ServiceName => "service";
        public static string ServiceSmileName => "smile";
        public static string ServiceSmileVideoName => "video";

        public static string DefineName => "define";

        public static string EtcDirectoryPath => Path.Combine(AssemblyParentDirectoryPath, "etc");

        public static string DefineDirectoryPath => Path.Combine(EtcDirectoryPath, DefineName);

        public static string DefineServiceDirectoryPath => Path.Combine(DefineDirectoryPath, ServiceName);

        public static string DefineSmileDirectoryPath => Path.Combine(DefineServiceDirectoryPath, ServiceSmileName);

        public static string SmileUriListPath => Path.Combine(DefineSmileDirectoryPath, "uri-list.xml");
        public static string SmileUriParametersListPath => Path.Combine(DefineSmileDirectoryPath, "uri-params.xml");
        public static string SmileRequestParametersListPath => Path.Combine(DefineSmileDirectoryPath, "request-params.xml");

        public static string DefineSmileVideoDirectoryPath => Path.Combine(DefineSmileDirectoryPath, ServiceSmileVideoName);

        public static string SmileVideoUriListPath => Path.Combine(DefineSmileVideoDirectoryPath, "uri-list.xml");
        public static string SmileVideoUriParametersListPath => Path.Combine(DefineSmileVideoDirectoryPath, "uri-params.xml");
        public static string SmileVideoRequestParametersListPath => Path.Combine(DefineSmileVideoDirectoryPath, "request-params.xml");
        public static string SmileVideoRequestMappingsListPath => Path.Combine(DefineSmileVideoDirectoryPath, "request-mapping.xml");

        public static string SmileVideoRankingPath => Path.Combine(DefineSmileVideoDirectoryPath, "ranking.xml");

        public static string SmileVideoCacheGetthumbinfoFileName => "thumb.xml";
        public static string SmileVideoCacheGetflvFileName => "getflv.xml";
        public static string SmileVideoCacheMsgFileName => "msg.xml";

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
