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

        public Task<FeedSmileChannelModel> LoadVideoFeedAsyn(string channelId, int pageNumber)
        {
            using(var page = new PageLoader(Mediation, new HttpUserAgentHost(NetworkSetting, Mediation.Logger), SmileMediationKey.channelVideoFeed, ServiceType.Smile)) {
                page.ReplaceUriParameters["channel-id"] = channelId;
                page.ReplaceUriParameters["page"] = pageNumber == 0 ? string.Empty : pageNumber.ToString();

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
        }
            #endregion
        }
}
