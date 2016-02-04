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
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    public class MappingItemModel: ModelBase
    {
        #region property

        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("type")]
        public MappingItemType Type { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("bond")]
        public string Bond { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("fail")]
        public string Failure { get; set; }

        /// <summary>
        /// キーが存在するか。
        /// </summary>
        public bool HasKey => !string.IsNullOrWhiteSpace(Key);

        #endregion
    }
}
