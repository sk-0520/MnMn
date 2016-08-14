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

namespace ContentTypeTextNet.MnMn.MnMn
{
    partial class Constants
    {
        #region property

        public static string ServiceSmileContentsSearchContext { get { return appConfig.Get("service-smile-ContentSearch-context"); } }

        #region smile-video

        public static TimeSpan ServiceSmileVideoDownloadingErrorWaitTime { get { return appConfig.Get("service-smile-smilevideo-DownloadingErrorWaitTime", TimeSpan.Parse); } }
        public static int ServiceSmileVideoDownloadingErrorRetryCount { get { return appConfig.Get("service-smile-smilevideo-DownloadingErrorRetryCount", int.Parse); } }
        public static TimeSpan ServiceSmileVideoWatchToMovieWaitTime { get { return appConfig.Get("service-smile-smilevideo-WatchToMovieWaitTime", TimeSpan.Parse); } }
        public static long ServiceSmileVideoPlayLowestSize { get { return appConfig.Get("service-smile-smilevideo-PlayLowestSize", long.Parse); } }
        public static int ServiceSmileVideoReceiveBuffer { get { return appConfig.Get("service-smile-smilevideo-ReceiveBuffer", int.Parse); } }

        #endregion

        #endregion
    }
}
