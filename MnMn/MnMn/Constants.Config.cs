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
        public static TimeSpan ServiceNicoNicoVideoDownloadingErrorWaitTime { get { return TimeSpan.Parse(ConfigurationManager.AppSettings["service-niconico-niconicovideo-DownloadingErrorWaitTime"]); } }
        public static int ServiceNicoNicoVideoDownloadingErrorRetryCount { get { return int.Parse(ConfigurationManager.AppSettings["service-niconico-niconicovideo-DownloadingErrorRetryCount"]); } }
        public static TimeSpan ServiceNicoNicoVideoWatchToMovieWaitTime { get { return TimeSpan.Parse(ConfigurationManager.AppSettings["service-niconico-niconicovideo-WatchToMovieWaitTime"]); } }
        public static long ServiceNicoNicoVideoPlayLowestSize { get { return long.Parse(ConfigurationManager.AppSettings["service-niconico-niconicovideo-PlayLowestSize"]); } }
        public static int ServiceNicoNicoVideoReceiveBuffer { get { return int.Parse(ConfigurationManager.AppSettings["service-niconico-niconicovideo-ReceiveBuffer"]); } }
        
        #endregion

        #endregion
    }
}
