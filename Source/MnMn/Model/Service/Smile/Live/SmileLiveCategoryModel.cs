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

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live
{
    [Serializable, XmlRoot("category")]
    public class SmileLiveCategoryModel: ModelBase
    {
        /// <summary>
        /// 一番最初のページ番号。
        /// </summary>
        [XmlAttribute("first-page")]
        public int FirstPage { get; set; }

        /// <summary>
        /// 位置検索で取得できるアイテム数。
        /// </summary>
        [XmlAttribute("max-count")]
        public int MaxCount { get; set; }

        [XmlArray("sort"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> SortItems { get; set; } = new CollectionModel<DefinedElementModel>();

        [XmlArray("order"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> OrderItems { get; set; } = new CollectionModel<DefinedElementModel>();

        [XmlArray("categories"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> CategoryItems { get; set; } = new CollectionModel<DefinedElementModel>();
    }
}
