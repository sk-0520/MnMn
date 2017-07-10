using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Channel.Raw.Feed
{
    public class FeedSmileChannelChannelModel: Rss2ChannelModelBase<FeedSmileChannelItemModel, Rss2GuidModel>
    {
        [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Creator { get; set; }
    }
}
