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

        #region property

        public static int TextFileSaveBuffer { get { return appConfig.Get("text-file-save-buffer", int.Parse); } }

        public static string UriUpdate { get { return ReplaceAppConfig(appConfig.Get("uri-update")); } }
        public static string UriChangelogRelease { get { return ReplaceAppConfig(appConfig.Get("uri-changelog-release")); } }
        public static string UriChangelogRc { get { return ReplaceAppConfig(appConfig.Get("uri-changelog-rc")); } }

        public static TimeSpan UpdateAppExitWaitTime { get { return appConfig.Get("update-app-exit-wait-time", TimeSpan.Parse); } }


        public static int BackupArchiveCount { get { return appConfig.Get("backup-archive", int.Parse); } }

        #endregion
    }
}
