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

namespace ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2
{
    /// <summary>
    /// RSS 2
    /// <para>現状ニコニコのフィード取れりゃそれでいい。</para>
    /// </summary>
    [Serializable, XmlRoot("rss")]
    public class Rss2ModelBase<TRss2ChannelModel, TRss2ItemModel, TRss2GuidModel>: FeedModelBase
        where TRss2ChannelModel : Rss2ChannelModelBase<TRss2ItemModel, TRss2GuidModel>, new()
        where TRss2ItemModel : Rss2ItemModelBase<TRss2GuidModel>, new()
        where TRss2GuidModel : Rss2GuidModel, new()
    {
        #region variable

        XmlSerializerNamespaces _xmlns;

        #endregion

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns
        {
            get
            {
                if(this._xmlns == null) {
                    this._xmlns = new XmlSerializerNamespaces();
                    foreach(var pair in GetXmlns()) {
                        this._xmlns.Add(pair.Key, pair.Value);
                    }
                }

                return _xmlns;
            }
            set { _xmlns = value; }
        }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlElement("channel")]
        public TRss2ChannelModel Channel { get; set; } = new TRss2ChannelModel();

        #region function

        virtual protected IEnumerable<KeyValuePair<string, string>> GetXmlns()
        {
            return Enumerable.Empty<KeyValuePair<string, string>>();
        }

        #endregion
    }

    [Serializable, XmlRoot("rss")]
    public sealed class Rss2Model: Rss2ModelBase<Rss2ChannelModelBase<Rss2ItemModelBase<Rss2GuidModel>, Rss2GuidModel>, Rss2ItemModelBase<Rss2GuidModel>, Rss2GuidModel>
    { }
}

