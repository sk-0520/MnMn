using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge
{
    /// <summary>
    /// <para>キー自体は<see cref="ContentTypeTextNet.MnMn.MnMn.Define.WebNavigatorContextMenuKey"/>で定義してるはず。</para>
    /// </summary>
    [Serializable]
    public class WebNavigatorContextMenuItemModel: WebNavigatorDefinedElementBase
    {
        #region property

        [XmlAttribute("separator")]
        public bool IsSeparator { get; set; }

        /// <summary>
        /// 表示・使用条件。
        /// <para>上から順に一番最初に合致したものが使用される。</para>
        /// </summary>
        [XmlArray("conditions"), XmlArrayItem("condition")]
        public CollectionModel<WebNavigatorElementConditionItemModel> Conditions { get; set; } = new CollectionModel<WebNavigatorElementConditionItemModel>();

        #endregion
    }
}
