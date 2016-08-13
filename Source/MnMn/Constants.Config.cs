﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.MnMn.MnMn
{
    /// <summary>
    /// App.config関係。
    /// </summary>
    partial class Constants
    {
        #region variable

        static ConfigurationCaching appConfig = new ConfigurationCaching();

        #endregion


        public static int TextFileSaveBuffer { get { return appConfig.Get("text-file-save-buffer", int.Parse); } }

        public static string UriUpdate { get { return ReplaceAppConfig(appConfig.Get("uri-update")); } }
        public static string UriChangelogRelease { get { return ReplaceAppConfig(appConfig.Get("uri-changelog-release")); } }
        public static string UriChangelogRc { get { return ReplaceAppConfig(appConfig.Get("uri-changelog-rc")); } }

        public static TimeSpan UpdateAppExitWaitTime { get { return appConfig.Get("update-app-exit-wait-time", TimeSpan.Parse); } }


        public static int BackupArchiveCount { get { return appConfig.Get("backup-archive", int.Parse); } }

        #region smile

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
