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

        /// <summary>
        /// コンテンツサーチで使用するサービス名。UAみたいなもん。
        /// </summary>
        public static string ServiceSmileContentsSearchContext { get { return appConfig.Get("service-smile-content_search-context"); } }

        public static TimeSpan ServiceSmileVideoDownloadingErrorWaitTime { get { return appConfig.Get("service-smile-smilevideo-doenloading_error-wait-time", TimeSpan.Parse); } }
        public static int ServiceSmileVideoDownloadingErrorRetryCount { get { return appConfig.Get("service-smile-smilevideo-downloading_Error-retry-count", int.Parse); } }
        public static TimeSpan ServiceSmileVideoWatchToMovieWaitTime { get { return appConfig.Get("service-smile-smilevideo-watch-to-movie-wait-time", TimeSpan.Parse); } }
        public static long ServiceSmileVideoPlayLowestSize { get { return appConfig.Get("service-smile-smilevideo-play-lowest-size", long.Parse); } }
        public static int ServiceSmileVideoReceiveBuffer { get { return appConfig.Get("service-smile-smilevideo-receive-buffer", int.Parse); } }

        /// <summary>
        /// マイリスト履歴数。
        /// </summary>
        public static int SettingServiceSmileMyListHistoryCount { get; } = 50;
        public static double SettingServiceSmileVideoCommentFontSize { get; } = System.Windows.SystemFonts.MessageFontSize * 1.8;
        public static string SettingServiceSmileVideoCommentFontFamily { get; } = System.Windows.SystemFonts.MessageFontFamily.FamilyNames.Values.First();
        public static double SettingServiceSmileVideoCommentFontAlpha { get; } = 1;
        public static bool SettingServiceSmileVideoCommentFontBold { get; } = false;
        public static bool SettingServiceSmileVideoCommentFontItalic { get; } = false;
        public static TimeSpan SettingServiceSmileVideoCommentShowTime { get; } = TimeSpan.FromSeconds(3);
        public static bool SettingServiceSmileVideoCommentConvertPairYenSlash { get; } = true;
        public static bool SettingServiceSmileVideoCommentIsEnabledSharedNoGood { get; } = true;
        public static int SettingServiceSmileVideoCommentSharedNoGoodScore { get; } = -1500;
        public static bool SettingServiceSmileVideoCommentPostAnonymous { get; } = true;

        public static int SettingServiceSmileVideoSearchCount { get; } = 100;
        public static bool SettingServiceSmileVideoAutoPlay { get; } = true;
        public static bool SettingServiceSmileVideoLoadVideoInformation { get; } = true;
        public static int SettingServiceSmileVideoPlayerDisplayCommentLimitCount { get; } = 25;
        public static TextShowKind SettingServiceSmileVideoPlayerTextShowKind { get; } = TextShowKind.Shadow;
        public static Color SettingServiceSmileVideoMyListFolderColor { get; } = Colors.SkyBlue;
        public static bool SettingServiceSmileVideoPlayerShowDetailArea { get; } = true;
        public static bool SettingServiceSmileVideoPlayerShowCommentList { get; } = true;
        public static bool SettingServiceSmileVideoPlayerShowPostTimestamp { get; } = false;
        public static bool SettingServiceSmileVideoPlayerVisibleComment { get; } = true;
        public static bool SettingServiceSmileVideoIsEnabledGlobalCommentFilering { get; } = true;


        #endregion
    }
}
