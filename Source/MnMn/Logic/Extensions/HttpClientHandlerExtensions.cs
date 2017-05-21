using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Extensions
{
    public static class HttpClientHandlerExtensions
    {
        #region function

        public static void SetProxy(this HttpClientHandler httpClientHandler, IReadOnlyNetworkProxy networkProxy, ILogger logger)
        {
            var proxy = new WebProxy(networkProxy.ServerAddress, networkProxy.ServerPort);
            if(networkProxy.UsingAuth) {
                var auth = new NetworkCredential(networkProxy.UserName, networkProxy.Password);
                proxy.Credentials = auth;
            }
            if(!httpClientHandler.UseProxy) {
                httpClientHandler.UseProxy = true;
            }
            try {
                httpClientHandler.Proxy = proxy;
            } catch(InvalidOperationException ex) {
                logger.Warning(ex);
            }
        }

        #endregion
    }
}
