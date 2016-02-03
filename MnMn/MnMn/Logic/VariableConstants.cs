using System;
using System.Collections.Generic;
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

        public static bool HasOptionCacheRootDirectoryPath => CommandLine.HasValue("cache-root");
        public static string OptionValueCacheRootDirectoryPath => CommandLine.GetValue("cache-root");

        public static bool HasOptionSmileUserAccountName => CommandLine.HasValue("smile-login-name");
        public static string OptionValueSmileUserAccountName => CommandLine.GetValue("smile-login-name");

        public static bool HasOptionSmileUserAccountPassword => CommandLine.HasValue("smile-login-pass");
        public static string OptionValueSmileUserAccountPassword => CommandLine.GetValue("smile-login-pass");



        #endregion

    }
}
