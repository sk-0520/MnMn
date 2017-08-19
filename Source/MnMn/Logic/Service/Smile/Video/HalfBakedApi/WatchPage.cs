using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using HtmlAgilityPack;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.HalfBakedApi
{
    public class WatchPage : ApiBase
    {
        public WatchPage(Mediator mediator)
            : base(mediator)
        { }

        #region function

        RawSmileVideoThumbModel GetGetthumbinfo(HtmlDocument htmlDocument)
        {
            var result = new RawSmileVideoThumbModel();

            return result;
        }

        /// <summary>
        /// (非セッション)視聴ページから getthumbinfo 情報を取得する。
        /// <para>通常の getthumbinfo を補完する目的で全情報を持つわけではない。</para>
        /// </summary>
        /// <param name="watchPageUrl"></param>
        /// <returns></returns>
        public async Task<RawSmileVideoThumbModel> LoadGetthumbinfoAsync(string watchPageUrl)
        {
            using(var userAgent = HttpUserAgentHost.CreateHttpUserAgent())
            using(var stream = await userAgent.GetStreamAsync(watchPageUrl).ConfigureAwait(false)) {
                var htmlDocument = HtmlUtility.CreateHtmlDocument(stream);

                var result = GetGetthumbinfo(htmlDocument);
                return result;
            }
        }



        #endregion
    }
}
