/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live.Raw.Feed;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed
{
    public class FeedSmileVideoItemModel: Rss2ItemModelBase<FeedSmileVideoGuidModel>
    {
        #region property

        [XmlElement(ElementName = "thumbnail", Namespace = "http://search.yahoo.com/mrss/")]
        public FeedSmileLiveThumbnailModel Thumbnail { get; set; } = new FeedSmileLiveThumbnailModel();

        [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Creator { get; set; }

        [XmlElement(ElementName = "community_name", Namespace = "http://live.nicovideo.jp/")]
        public string CommunityName { get; set; }

        [XmlElement(ElementName = "community_id", Namespace = "http://live.nicovideo.jp/")]
        public string CommunityId { get; set; }

        [XmlElement(ElementName = "num_res", Namespace = "http://live.nicovideo.jp/")]
        public string NumRes { get; set; }

        [XmlElement(ElementName = "view", Namespace = "http://live.nicovideo.jp/")]
        public string View { get; set; }

        [XmlElement(ElementName = "member_only", Namespace = "http://live.nicovideo.jp/")]
        public string MemberOnly { get; set; }

        [XmlElement(ElementName = "type", Namespace = "http://live.nicovideo.jp/")]
        public string Type { get; set; }

        [XmlElement(ElementName = "owner_name", Namespace = "http://live.nicovideo.jp/")]
        public string OwnerName { get; set; }

        #endregion
    }
}
