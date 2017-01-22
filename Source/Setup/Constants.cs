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

        public static TimeSpan HttpWaitTime { get; } = TimeSpan.FromMinutes(25);

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

        public static Uri ProjectUri { get; } = new Uri("https://bitbucket.org/sk_0520/mnmn");
        public static Uri HelpUri { get; } = new Uri("https://bitbucket.org/sk_0520/mnmn/wiki/%E8%87%AA%E5%8B%95%E3%82%BB%E3%83%83%E3%83%88%E3%82%A2%E3%83%83%E3%83%97");
        public static Uri LicenseUri { get; } = new Uri("http://www.gnu.org/licenses/gpl-3.0.html");
        

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
