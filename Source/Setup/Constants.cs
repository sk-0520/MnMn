using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Setup
{
    public static class Constants
    {
        #region variable

        const string projectName = "MnMn";

        const string mutexName = "MnMn";

        #endregion

        #region property

        public static string ApplicationFileName { get; } = "MnMn.exe";

        public static string ProjectName
        {
            get
            {
#if DEBUG
                return projectName + "-debug";
#else
                return projectName;
#endif
            }
        }

        public static string MutexName
        {
            get
            {
#if DEBUG
                return mutexName + "-debug";
#else
                return mutexName;
#endif
            }
        }

        public static TimeSpan MutexWaitTime { get; } = TimeSpan.FromSeconds(3);

        public static Uri UpdateUri
        {
            get
            {
#if DEBUG
                return new Uri("http://localhost/test/mnmn/update.xml");
#else
                return new Uri("https://bitbucket.org/sk_0520/mnmn/downloads/update.xml");
#endif
            }
        }

        public static string SettingBaseDirectoryPath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#if RAM
                basePath = @"%RAM%\mnmn-setup-root";
#endif
                return Path.Combine(basePath, ProjectName);
            }
        }

        public static string InstallDirectoryPath { get; } = Path.Combine(SettingBaseDirectoryPath, "application");
        public static string ArchiveDirectoryPath { get; } = Path.Combine(SettingBaseDirectoryPath, "archive");

        public static string BaseRegistryPath { get; } = @"SOFTWARE\ContentTypeTextNet\" + ProjectName;
        public static string ApplicationPathName { get; } = "application";

        public static int DownloadBufferSize { get; } = 1024;

#endregion
    }
}
