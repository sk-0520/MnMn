using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Extensions
{
    public static class HttpClientHandlerExtensions
    {
        #region function

        public static void SetProxy(this HttpClientHandler httpClientHandler, IReadOnlyNetworkProxy networkProcy)
        {
            var proxy = new WebProxy(networkProcy.ServerAddress);
            if(networkProcy.UsingAuth) {
                var auth = new NetworkCredential(networkProcy.UserName, networkProcy.Password);
                proxy.Credentials = auth;
            }
            httpClientHandler.UseProxy = true;
            httpClientHandler.Proxy = proxy;
        }

        #endregion
    }
}
