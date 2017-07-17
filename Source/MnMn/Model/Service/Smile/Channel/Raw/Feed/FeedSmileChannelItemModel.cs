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
    [Serializable]
    public class FeedSmileChannelItemModel: Rss2ItemModelBase<Rss2GuidModel>
    {
        #region property

        [XmlElement(ElementName = "isPremium", Namespace = "http://ch.nicovideo.jp/")]
        public string IsPremium { get; set; }

        #endregion
    }
}
