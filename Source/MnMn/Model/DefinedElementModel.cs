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
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    /// <summary>
    /// 要素定義。
    /// <para>記述の正しい外部設定が用いられる前提のデータ定義を持ち運ぶ。</para>
    /// </summary>
    [Serializable]
    public class DefinedElementModel: KeyModelBase, IDeepClone, IReadOnlyDefinedElement
    {
        #region variable

        StringsModel _words;
        StringsModel _extends;

        #endregion

        #region property

        /// <summary>
        /// 要素表記文言。
        /// </summary>
        [XmlArray("words"), XmlArrayItem("word")]
        [IsDeepClone]
        public CollectionModel<DefinedKeyValuePairModel> _Words { get; set; } = new CollectionModel<DefinedKeyValuePairModel>();

        /// <summary>
        /// 要素表記文言。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public IReadOnlyDictionary<string, string> Words
        {
            get
            {
                if(this._words == null) {
                    var map = _Words.ToDictionary(
                        p => p.Key,
                        p => p.Value
                    );
                    this._words = new StringsModel(map);
                }

                return this._words;
            }
        }

        [XmlArray("extends"), XmlArrayItem("extend")]
        [IsDeepClone]
        public CollectionModel<DefinedKeyValuePairModel> _Extends { get; set; } = new CollectionModel<DefinedKeyValuePairModel>();

        /// <summary>
        /// 拡張データ。
        /// <para>使用側で責任を持つ。</para>
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public IReadOnlyDictionary<string, string> Extends
        {
            get
            {
                if(this._extends == null) {
                    var map = _Extends.ToDictionary(
                        p => p.Key,
                        p => p.Value
                    );
                    this._extends = new StringsModel(map);
                }

                return this._extends;
            }
        }

        /// <summary>
        /// 画像情報。
        /// </summary>
        [XmlElement("image")]
        [IsDeepClone]
        public DefinedImageModel Image { get; set; }

        #endregion

        #region ModelBase

        public override string DisplayText
        {
            get
            {
                string resultValue;
                if(Words.TryGetValue(AppUtility.GetCultureName(), out resultValue)) {
                    return resultValue;
                }

                var firstValue = Words.Values.FirstOrDefault();
                return string.IsNullOrEmpty(firstValue) ? Key : firstValue;
            }
        }

        #endregion

        #region IDeepClone

        public IDeepClone DeepClone()
        {
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
