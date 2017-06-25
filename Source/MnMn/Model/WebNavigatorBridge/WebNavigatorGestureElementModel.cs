using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge
{
    /// <summary>
    /// <para>キー自体は<see cref="ContentTypeTextNet.MnMn.MnMn.Define.WebNavigatorContextMenuKey"/>で定義してるはず。</para>
    /// </summary>
    [Serializable]
    public class WebNavigatorGestureElementModel: WebNavigatorDefinedElementModelBase, IReadOnlyWebNavigatorGestureElement
    {
        #region property

        [XmlArray("directions"), XmlArrayItem("direction")]
        public CollectionModel<PointingGestureDirection> Directions { get; set; } = new CollectionModel<PointingGestureDirection>();
        IReadOnlyList<PointingGestureDirection> IReadOnlyWebNavigatorGestureElement.Directions => Directions;

        #endregion
    }
}
