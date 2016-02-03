using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn
{
    /// <summary>
    /// App.config関係。
    /// </summary>
    partial class Constants
    {
        #region niconico

        #region niconico-video

        public static TimeSpan ServiceSmileVideoDownloadingErrorWaitTime { get { return TimeSpan.Parse(ConfigurationManager.AppSettings["service-smile-smilevideo-DownloadingErrorWaitTime"]); } }
        public static int ServiceSmileVideoDownloadingErrorRetryCount { get { return int.Parse(ConfigurationManager.AppSettings["service-smile-smilevideo-DownloadingErrorRetryCount"]); } }
        public static TimeSpan ServiceSmileVideoWatchToMovieWaitTime { get { return TimeSpan.Parse(ConfigurationManager.AppSettings["service-smile-smilevideo-WatchToMovieWaitTime"]); } }
        public static long ServiceSmileVideoPlayLowestSize { get { return long.Parse(ConfigurationManager.AppSettings["service-smile-smilevideo-PlayLowestSize"]); } }
        public static int ServiceSmileVideoReceiveBuffer { get { return int.Parse(ConfigurationManager.AppSettings["service-smile-smilevideo-ReceiveBuffer"]); } }
        
        #endregion

        #endregion
    }
}
