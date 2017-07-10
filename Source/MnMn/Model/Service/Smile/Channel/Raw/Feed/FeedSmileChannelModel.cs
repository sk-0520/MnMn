using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Channel.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Channel.Raw.Feed
{
    [Serializable, XmlRoot("rss")]
    public class FeedSmileChannelModel: Rss2ModelBase<FeedSmileChannelChannelModel, FeedSmileChannelItemModel, Rss2GuidModel>
    {
        #region Rss2ModelBase

        protected override IEnumerable<KeyValuePair<string, string>> GetXmlns()
        {
            yield return new KeyValuePair<string, string>("dc", "http://purl.org/dc/elements/1.1/");
            yield return new KeyValuePair<string, string>("nicoch", "http://ch.nicovideo.jp/");
        }

        #endregion
    }
}
