using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public static class VariableConstants
    {
        public static CommandLine CommandLine => new CommandLine();

        #region property

        const string optionLogDirectoryPath = "log";
        public static bool HasOptionLogDirectoryPath => CommandLine.HasValue(optionLogDirectoryPath);
        public static string OptionLogDirectoryPath => CommandLine.GetValue(optionLogDirectoryPath);

        const string optionSettingRootDirectoryPath = "setting-root";
        public static bool HasOptionSettingRootDirectoryPath => CommandLine.HasValue(optionSettingRootDirectoryPath);
        public static string OptionValueSettingRootDirectoryPath => CommandLine.GetValue(optionSettingRootDirectoryPath);

        const string optionCacheRootDirectoryPath = "cache-root";
        public static bool HasOptionCacheRootDirectoryPath => CommandLine.HasValue(optionCacheRootDirectoryPath);
        public static string OptionValueCacheRootDirectoryPath => CommandLine.GetValue(optionCacheRootDirectoryPath);

        const string optionSmileUserAccountName = "smile-login-name";
        public static bool HasOptionSmileUserAccountName => CommandLine.HasValue(optionSmileUserAccountName);
        public static string OptionValueSmileUserAccountName => CommandLine.GetValue(optionSmileUserAccountName);

        const string optionSmileUserAccountPassword = "smile-login-pass";
        public static bool HasOptionSmileUserAccountPassword => CommandLine.HasValue(optionSmileUserAccountPassword);
        public static string OptionValueSmileUserAccountPassword => CommandLine.GetValue(optionSmileUserAccountPassword);

        const string optionExecuteBetaVersion = "execute-beta";
        public static bool HasOptionExecuteBetaVersion => CommandLine.HasOption(optionExecuteBetaVersion);

        #endregion

        #region function

        static string GetSettingBaseDirectory()
        {
            var baseDir = HasOptionSettingRootDirectoryPath
                ? Path.Combine(Environment.ExpandEnvironmentVariables(OptionValueSettingRootDirectoryPath), Constants.ApplicationDirectoryName)
                : null
            ;
            if(string.IsNullOrWhiteSpace(baseDir)) {
                baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.ApplicationDirectoryName);
            }

            return baseDir;
        }

        public static DirectoryInfo GetSettingDirectory()
        {
            var baseDir = GetSettingBaseDirectory();
            var path = Path.Combine(baseDir, Constants.SettingDirectoryName);

            return Directory.CreateDirectory(path);
        }

        public static DirectoryInfo GetArchiveDirectory()
        {
            var baseDir = GetSettingBaseDirectory();
            var path = Path.Combine(baseDir, Constants.ArchiveDirectoryName);

            return Directory.CreateDirectory(path);
        }

        public static DirectoryInfo GetLightweightUpdateDirectory()
        {
            var baseDir = GetSettingBaseDirectory();
            var path = Path.Combine(baseDir, Constants.ArchiveLightweightUpdateDirectoryName);

            return Directory.CreateDirectory(path);
        }

        public static DirectoryInfo GetWebNavigatorGeckFxPluginDirectory()
        {
            var baseDir = GetSettingBaseDirectory();
            var path = Path.Combine(baseDir, Constants.ArchiveWebNavigatorGeckFxPluginDirectoryName);

            return Directory.CreateDirectory(path);
        }

        public static DirectoryInfo GetBackupDirectory()
        {
            var baseDir = GetSettingBaseDirectory();
            var path = Path.Combine(baseDir, Constants.BackupDirectoryName);

            return Directory.CreateDirectory(path);
        }

        public static DirectoryInfo GetCrashReportDirectory()
        {
            var baseDir = GetSettingBaseDirectory();
            var path = Path.Combine(baseDir, Constants.CrashReportDirectoryName);

            return Directory.CreateDirectory(path);
        }


        #endregion

    }
}
