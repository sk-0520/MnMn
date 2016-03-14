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

        #region 

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

        #endregion

        #region function

        public static DirectoryInfo GetSettingDirectory()
        {
            var baseDir = HasOptionSettingRootDirectoryPath
                ? Path.Combine(OptionValueSettingRootDirectoryPath, Constants.ApplicationDirectoryName)
                : null
            ;
            if(string.IsNullOrWhiteSpace(baseDir)) {
                baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.ApplicationDirectoryName);
            }

            var path = Path.Combine(baseDir, Constants.SettingDirectoryName);

            if(Directory.Exists(path)) {
                return new DirectoryInfo(path);
            } else {
                return Directory.CreateDirectory(path);
            }
        }

        #endregion

    }
}
