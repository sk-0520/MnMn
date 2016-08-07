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
    /// <summary>
    /// <para>http://stackoverflow.com/questions/1379888/how-do-you-serialize-a-string-as-cdata-using-xmlserializer?answertab=votes#tab-top</para>
    /// </summary>
    public class MappingContentModel: ModelBase
    {
        #region property

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("trim")]
        public MappingContentTrim Trim {get;set;}

        /// <summary>
        /// 一行にまとめるか。
        /// <para>トリム処理後に実施される。</para>
        /// </summary>
        [XmlAttribute("oneline")]
        public bool Oneline { get; set; }

        [XmlIgnore]
        public string Value { get; set; }

        [XmlText]
        public XmlNode[] CDataValue
        {
            get
            {
                var dummy = new XmlDocument();
                return new XmlNode[] { dummy.CreateCDataSection(Value) };
            }
            set
            {
                if(value == null) {
                    Value = null;
                    return;
                }

                if(value.Length != 1) {
                    throw new InvalidOperationException($"Invalid array length {value.Length}");
                }

                Value = value[0].Value;
            }
        }

        #endregion
    }
}
