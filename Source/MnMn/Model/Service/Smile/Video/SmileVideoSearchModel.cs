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
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    [Serializable, XmlRoot("search")]
    public class SmileVideoSearchModel: ModelBase
    {
        #region property

        [XmlAttribute("service")]
        public string Service { get; set; }

        [XmlArray("method"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Methods { get; set; } = new CollectionModel<DefinedElementModel>();
        [XmlArray("sort"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Sort { get; set; } = new CollectionModel<DefinedElementModel>();
        [XmlArray("type"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Type { get; set; } = new CollectionModel<DefinedElementModel>();
        [XmlArray("result"), XmlArrayItem("field")]
        public CollectionModel<string> Results { get; set; } = new CollectionModel<string>();

        [XmlAttribute("max-index")]
        public int MaximumIndex { get; set; }
        [XmlAttribute("max-count")]
        public int MaximumCount { get; set; }

        #endregion
    }
}

