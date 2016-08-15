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
using System.Drawing;
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

        public static string ServiceSmileContentsSearchContext { get { return appConfig.Get("service-smile-ContentSearch-context"); } }

        #region smile-video
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
        public static bool SmileVideoIsEnabledGlobalCommentFilering { get; } = true;

        public static TimeSpan ServiceSmileVideoDownloadingErrorWaitTime { get { return appConfig.Get("service-smile-smilevideo-DownloadingErrorWaitTime", TimeSpan.Parse); } }
        public static int ServiceSmileVideoDownloadingErrorRetryCount { get { return appConfig.Get("service-smile-smilevideo-DownloadingErrorRetryCount", int.Parse); } }
        public static TimeSpan ServiceSmileVideoWatchToMovieWaitTime { get { return appConfig.Get("service-smile-smilevideo-WatchToMovieWaitTime", TimeSpan.Parse); } }
        public static long ServiceSmileVideoPlayLowestSize { get { return appConfig.Get("service-smile-smilevideo-PlayLowestSize", long.Parse); } }
        public static int ServiceSmileVideoReceiveBuffer { get { return appConfig.Get("service-smile-smilevideo-ReceiveBuffer", int.Parse); } }

        #endregion

        #endregion
    }
}
