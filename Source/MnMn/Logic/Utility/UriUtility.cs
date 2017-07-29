using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.Compatibility;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class UriUtility
    {
        #region function

        static IReadOnlyUriResult GetUriModel(IUriGetter uriGetter,string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            return uriGetter.GetUri(key, replaceMap, serviceType);
        }

        public static Uri GetConvertedUri(IUriGetter uriGetter, IUriCompatibility uriCompatibility, string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            var uriResult = GetUriModel(uriGetter, key, replaceMap, serviceType);
            var rawUri = uriCompatibility.ConvertUri(key, uriResult.Uri, serviceType);

            var uri = new Uri(rawUri);

            return uri;
        }

        public static Uri GetConvertedUri(MediatorBase mediator, string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            return GetConvertedUri(mediator, mediator, key, replaceMap, serviceType);
        }

        #endregion
    }
}
