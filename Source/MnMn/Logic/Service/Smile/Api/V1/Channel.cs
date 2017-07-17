using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Channel.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V1
{
    /// <summary>
    /// チャンネル系。
    /// <para>一応セッション噛ませてるけど使わない方針。</para>
    /// </summary>
    public class Channel : SessionApiBase<SmileSessionViewModel>
    {
        public Channel(Mediation mediation)
            : base(mediation, ServiceType.Smile)
        { }

        #region function

        public Task<FeedSmileChannelModel> LoadVideoFeedAsync(string channelId, int pageNumber)
        {
            var page = new PageLoader(Mediation, new HttpUserAgentHost(NetworkSetting, Mediation.Logger), SmileMediationKey.channelVideoFeed, ServiceType.Smile);
            page.ReplaceUriParameters["channel-id"] = channelId;
            page.ReplaceUriParameters["page"] = pageNumber == 1 ? string.Empty : pageNumber.ToString();

            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(t => {
                page.Dispose();
                var response = t.Result;

                if(!response.IsSuccess) {
                    return null;
                } else {
                    using(var stream = StreamUtility.ToUtf8Stream(response.Result)) {
                        return SerializeUtility.LoadXmlSerializeFromStream<FeedSmileChannelModel>(stream);
                    }
                }
            });
        }

        SmileChannelInformationModel LoadInformationCore(string channelId, string htmlSource)
        {
            var result = new SmileChannelInformationModel();

            var htmlDocument = HtmlUtility.CreateHtmlDocument(htmlSource);

            var headlineElement = htmlDocument.DocumentNode.SelectSingleNode("//h1");
            var headlineLineElemet = headlineElement.SelectSingleNode(".//a");

            result.ChannelId = channelId;
            result.ChannelCode = headlineLineElemet.Attributes["href"].Value.Split(new[] { ',' }, 2).Last(); // TODO: 正規表現で分離させた方が安全
            result.ChannelName = headlineLineElemet.InnerText;

            return result;
        }

        public Task<SmileChannelInformationModel> LoadInformationAsync(string channelId)
        {
            var page = new PageLoader(Mediation, HttpUserAgentHost, SmileMediationKey.channelPage, ServiceType.Smile);
            page.ReplaceUriParameters["channel-id"] = channelId;

            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(t => {
                page.Dispose();
                var response = t.Result;

                if(!response.IsSuccess) {
                    return null;
                } else {
                    return LoadInformationCore(channelId, response.Result);
                }
            });
        }

        #endregion
    }
}
