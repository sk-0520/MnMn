using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.MnMn.MnMn.IF;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class DescriptionUtility
    {
        #region function

        /// <summary>
        /// <see cref="IDescription.OpenWebLinkCommand"/>
        /// </summary>
        /// <param name="link"></param>
        /// <param name="logger"></param>
        static void OpenWebLinkCore(string link, ILogger logger)
        {
            try {
                Process.Start(link);
            } catch(Exception ex) {
                logger.Warning(ex);
            }
        }

        public static void OpenWebLink(object parameter, ILogger logger)
        {
            OpenWebLinkCore((string)parameter, logger);
        }

        #endregion
    }
}
