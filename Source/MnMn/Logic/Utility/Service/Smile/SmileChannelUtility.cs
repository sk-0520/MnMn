using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Channel.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile
{
    public class SmileChannelUtility
    {
        #region function


        public async static Task<IEnumerable<FeedSmileChannelModel>> LoadAllVideoFeedAsync(Mediator mediation, string channelId)
        {
            var channel = new Logic.Service.Smile.Api.V1.Channel(mediation);
            var result = new List<FeedSmileChannelModel>();

            var pageNumber = 1;
            while(true) {
                if(1 < pageNumber) {
                    await Task.Delay(Constants.ServiceSmileChannelVideoFeedWaitTime);
                }
                var feed = await channel.LoadVideoFeedAsync(channelId, pageNumber);
                if(feed.Channel.Items.Any()) {
                    result.Add(feed);
                    if(feed.Channel.Items.Count < Constants.ServiceSmileChannelVideoFeedItemCount) {
                        // 1 ページにおける上限未満ならそこで終わり
                        break;
                    }
                    pageNumber += 1;
                } else {
                    // そもそもない
                    break;
                }
            }

            return result;
        }

        public static FeedSmileVideoItemModel ConvertVideoFeedItemFrtomChannelFeedItem(FeedSmileChannelItemModel item)
        {
            var result = new FeedSmileVideoItemModel() {
                Category = item.Category,
                Guid = item.Guid,
                Description = item.Description,
                Link = item.Link,
                PubDate = item.PubDate,
                Title = item.Title,
            };

            return result;
        }

        #endregion
    }
}
