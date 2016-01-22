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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video
{
    /// <summary>
    /// ランキング要素。
    /// </summary>
    [Serializable]
    public class ElementModel: ModelBase
    {
        #region property

        /// <summary>
        /// ランキング要素の検索キー。
        /// </summary>
        [XmlAttribute("key")]
        public string Key { get; set; }
        /// <summary>
        /// ランキング要素タイトル。
        /// </summary>
        [XmlArray("words"), XmlArrayItem("word")]
        public CollectionModel<WordModel> Words { get; set; } = new CollectionModel<WordModel>();

        public string CurrentWord {
            get
            {
                var word = Words.FirstOrDefault(w => w.Language == Constants.CurrentLanguageCode);
                if(word != null) {
                    return word.Value;
                }

                return Key;
            }
        }

        #endregion
    }
}
