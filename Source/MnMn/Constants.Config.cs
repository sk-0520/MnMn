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

        public static string AppUriAbout => ReplaceAppConfig(appConfig.Get("app-uri-about"));
        public static string AppMailAbout => ReplaceAppConfig(appConfig.Get("app-mail-about"));
        public static string AppUriDevelopment => ReplaceAppConfig(appConfig.Get("app-uri-development"));
        public static string AppUriForum => ReplaceAppConfig(appConfig.Get("app-uri-forum"));
        public static string AppUriUpdate => ReplaceAppConfig(appConfig.Get("app-uri-update"));
        public static string AppUriChangelogRelease => ReplaceAppConfig(appConfig.Get("app-uri-changelog-release"));
        //public static string UriChangelogRc => ReplaceAppConfig(appConfig.Get("app-uri-changelog-rc"));

        public static TimeSpan UpdateAppExitWaitTime => appConfig.Get("update-app-exit-wait-time", TimeSpan.Parse);

        /// <summary>
        /// 設定ファイルの自動保存タイミング。
        /// </summary>
        public static TimeSpan BackgroundAutoSaveSettingTime => appConfig.Get("background-auto-save-setting-time", TimeSpan.Parse);
        /// <summary>
        /// アップデートチェックタイミング。
        /// </summary>
        public static TimeSpan BackgroundUpdateCheckTime => appConfig.Get("background-update-check-time", TimeSpan.Parse);


        public static int LogViewCount => appConfig.Get("log-view-count", int.Parse);
        public static int TextFileSaveBuffer => appConfig.Get("text-file-save-buffer", int.Parse);

        public static int BackupArchiveCount => appConfig.Get("backup-archive-count", int.Parse);
        public static int BackupSettingCount => appConfig.Get("backup-setting-count", int.Parse);

        /// <summary>
        /// キャッシュファイル有効期間。
        /// </summary>
        public static TimeSpan SettingApplicationCacheLifeTime => appConfig.Get("setting-application-cache-life-time", TimeSpan.Parse);
        /// <summary>
        /// ウィンドウ: 左
        /// </summary>
        public static int SettingApplicationWindowLeft => appConfig.Get("setting-application-window-top", int.Parse);
        /// <summary>
        /// ウィンドウ: 上
        /// </summary>
        public static int SettingApplicationWindowTop => appConfig.Get("setting-application-window-left", int.Parse);
        /// <summary>
        /// ウィンドウ: 幅
        /// </summary>
        public static int SettingApplicationWindowWidth => appConfig.Get("setting-application-window-width", int.Parse);
        /// <summary>
        /// ウィンドウ: 高さ
        /// </summary>
        public static int SettingApplicationWindowHeight => appConfig.Get("setting-application-window-height", int.Parse);

        #endregion
    }
}
