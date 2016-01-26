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

        public static string NicoNicoUserAccountName => CommandLine.GetValue("niconico-login-name");
        public static string NicoNicoUserAccountPassword => CommandLine.GetValue("niconico-login-pass");

        #endregion

    }
}
