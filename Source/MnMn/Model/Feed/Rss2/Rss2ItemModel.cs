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
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2
{
    /// <summary>
    /// チャンネルのコンテンツ情報。
    /// </summary>
    public class Rss2ItemModelBase<TRss2GuidModel>: ModelBase
        where TRss2GuidModel : Rss2GuidModel, new()
    {
        #region property

        [XmlElement("title")]
        public string Title { get; set; }
        [XmlElement("link")]
        public string Link { get; set; }
        [XmlElement("guid")]
        public TRss2GuidModel Guid { get; set; } = new TRss2GuidModel();
        [XmlIgnore, IgnoreDataMember]
        public DateTime PubDate { get; set; }
        [XmlElement("pubDate")]
        public string _PubDate
        {
            get { return PubDate.ToString(RawValueUtility.Rss2DateTimeFormat); }
            set { PubDate = RawValueUtility.ConvertDateTime(value); }
        }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("category")]
        public string Category { get; set; }

        #endregion
    }

    public sealed class Rss2ItemModel: Rss2ItemModelBase<Rss2GuidModel>
    { }

}
