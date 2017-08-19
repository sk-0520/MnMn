using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
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
            var countExpression = Mediator.GetExpression(SmileVideoMediatorKey.watchPage, SmileVideoMediatorKey.Id.watchPage_count, Library.Bridging.Define.ServiceType.SmileVideo);
            var element = htmlDocument.DocumentNode.SelectSingleNode(countExpression.XPath);
            if(element == null) {
                Mediator.Logger.Warning("not fount element");
                return null;
            }

            var attributeExpression = Mediator.GetExpression(SmileVideoMediatorKey.watchPage, SmileVideoMediatorKey.Id.watchPage_attribute, Library.Bridging.Define.ServiceType.SmileVideo);
            var attribute = element.Attributes[attributeExpression.Word];
            if(attribute == null) {
                Mediator.Logger.Warning("not fount attribute");
                return null;
            }

            var numExpression = Mediator.GetExpression(SmileVideoMediatorKey.watchPage, SmileVideoMediatorKey.Id.watchPage_number, Library.Bridging.Define.ServiceType.SmileVideo);
            var match = numExpression.Regex.Match(attribute.Value);

            if(!match.Success) {
                Mediator.Logger.Warning("not match");
                return null;
            }

            var viewCount = match.Groups["VIEW"];
            var commentCount = match.Groups["COMMENT"];

            var result = new RawSmileVideoThumbModel() {
                ViewCounter = viewCount.Value,
                CommentNum = commentCount.Value,
            };

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
            using(var stream = await userAgent.GetStreamAsync(watchPageUrl).ConfigureAwait(false))
            using(var reader = new StreamReader(stream, Encoding.UTF8)) {
                var htmlDocument = HtmlUtility.CreateHtmlDocument(reader.ReadToEnd());
                var result = GetGetthumbinfo(htmlDocument);
                return result;
            }
        }



        #endregion
    }
}
