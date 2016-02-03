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

        public static string SmileUserAccountName => CommandLine.GetValue("smile-login-name");
        public static string SmileUserAccountPassword => CommandLine.GetValue("smile-login-pass");

        #endregion

    }
}
