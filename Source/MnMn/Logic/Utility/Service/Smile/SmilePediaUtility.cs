using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile
{
    public static class SmilePediaUtility
    {
        #region function

        public static Uri GetArticleUriFromWord(Mediator mediator, string word)
        {
            var serviceType = ServiceType.Smile;
            var key = SmileMediationKey.pediaWordArticle;

            var map = new StringsModel() {
                ["word"] = word,
            };

            var rawUri = mediator.GetUri(key, map, serviceType);
            var convertedUri = mediator.ConvertUri(key, rawUri.Uri, serviceType);

            return new Uri(convertedUri);
        }

        #endregion
    }
}
