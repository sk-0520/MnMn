using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class NetoworkUtility
    {
        #region function

        public static string GetUserAgentText(string userAgentFormat)
        {
            if(string.IsNullOrEmpty(userAgentFormat)) {
                return string.Empty;
            }

            var usingMap = new StringsModel() {
                ["APP"] = Constants.ApplicationName,
                ["VER"] = Constants.ApplicationVersion,
                ["VER:NUM"] = Constants.ApplicationVersionNumber.ToString(),
                ["VER:REV"] = Constants.ApplicationVersionRevision,
            };

            var result = AppUtility.ReplaceString(userAgentFormat, usingMap);
            return result;
        }

        #endregion
    }
}
