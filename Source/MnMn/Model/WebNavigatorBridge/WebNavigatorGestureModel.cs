using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge
{
    [Serializable]
    public class WebNavigatorGestureModel: ModelBase, IReadOnlyWebNavigatorGesture
    {
        #region property

        [XmlElement("element")]
        public CollectionModel<WebNavigatorGestureElementModel> Items { get; set; } = new CollectionModel<WebNavigatorGestureElementModel>();
        IReadOnlyList<IReadOnlyWebNavigatorGestureElement> IReadOnlyWebNavigatorGesture.Items => Items;

        #endregion
    }
}
