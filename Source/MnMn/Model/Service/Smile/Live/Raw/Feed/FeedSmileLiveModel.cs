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
    [Serializable, XmlRoot("rss")]
    public class FeedSmileLiveModel: Rss2ModelBase<FeedSmileLiveChannelModel, FeedSmileLiveItemModel, Rss2GuidModel>
    {
        #region Rss2ModelBase

        protected override IEnumerable<KeyValuePair<string, string>> GetXmlns()
        {
            yield return new KeyValuePair<string, string>("dc", "http://purl.org/dc/elements/1.1/");
            yield return new KeyValuePair<string, string>("nicolive", "http://live.nicovideo.jp/");
            yield return new KeyValuePair<string, string>("media", "http://search.yahoo.com/mrss/");
        }

        #endregion
    }
}
