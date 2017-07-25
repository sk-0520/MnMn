using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V1
{
    public class Market: ApiBase
    {
        public Market(Mediator mediation)
            : base(mediation)
        { }

        #region function

        public async Task<RawSmileMarketVideoRelationModel> LoadVideoRelationAsync(string videoId)
        {
            using(var page = new PageLoader(Mediation, new HttpUserAgentHost(NetworkSetting, Mediation.Logger), SmileMediationKey.marketVideoRelation, ServiceType.Smile)) {
                page.ReplaceUriParameters["video-id"] = videoId;
                var feedResult = await page.GetResponseTextAsync(PageLoaderMethod.Get);
                if(!feedResult.IsSuccess) {
                    return null;
                } else {
                    using(var stream = StreamUtility.ToUtf8Stream(feedResult.Result)) {
                        return SerializeUtility.LoadJsonDataFromStream<RawSmileMarketVideoRelationModel>(stream);
                    }
                }
            }
        }

        #endregion
    }
}
