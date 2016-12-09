using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class DescriptionUtility
    {
        #region function

        /// <summary>
        /// <see cref="IDescription.OpenUriCommand"/>
        /// </summary>
        /// <param name="link"></param>
        /// <param name="logger"></param>
        static void OpenUriCore(string link, ILogger logger)
        {
            try {
                Process.Start(link);
            } catch(Exception ex) {
                logger.Warning(ex);
            }
        }

        public static void OpenUri(object parameter, ILogger logger)
        {
            OpenUriCore((string)parameter, logger);
        }

        static void OpenUriInAppBrowserCore(Uri uri, ICommunication communication)
        {
            var parameter = new AppBrowserParameterModel() {
                Uri = uri,
            };
            //communication.GetResultFromRequest();
            communication.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.Application, parameter, ShowViewState.Foreground));
        }

        public static void OpenUriInAppBrowser(object parameter, ICommunication communication)
        {
            Uri uri;
            if(Uri.TryCreate((string)parameter, UriKind.RelativeOrAbsolute, out uri)) {
                OpenUriInAppBrowserCore(uri, communication);
            }
        }

        #endregion
    }
}
