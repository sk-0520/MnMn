using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable]
    public abstract class KeyModelBase: ModelBase, IReadOnlyKey
    {
        #region IReadOnlyKey

        /// <summary>
        /// キー。
        /// <para>検索キーに使用する</para>
        /// </summary>
        [DataMember]
        [XmlAttribute("key")]
        [IsDeepClone]
        public string Key { get; set; }

        #endregion
    }
}
