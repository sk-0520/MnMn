using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live.Api
{
    public class Broadcast: ApiBase
    {
        public Broadcast(Mediator mediator)
            : base(mediator)
        { }

        #region function

        public static FeedSmileLiveModel ConvertFromRssText(string rssText)
        {
            using(var stream = StreamUtility.ToUtf8Stream(rssText)) {
                return SerializeUtility.LoadXmlSerializeFromStream<FeedSmileLiveModel>(stream);
            }
        }

        public Task<FeedSmileLiveModel> LoadAsync()
        {
            var page = new PageLoader(Mediator, new HttpUserAgentHost(NetworkSetting, Mediator.Logger), SmileLiveMediatorKey.broadcast, ServiceType.SmileLive);

            return page.GetResponseTextAsync(PageLoaderMethod.Get).ContinueWith(t => {
                var response = t.Result;
                t.Dispose();
                page.Dispose();

                if(!response.IsSuccess) {
                    return default(FeedSmileLiveModel);
                } else {
                    var result = ConvertFromRssText(response.Result);
                    return result;
                }
            });
        }

        #endregion

    }
}
