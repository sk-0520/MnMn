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
    [Serializable, XmlRoot("web-navigator")]
    public class WebNavigatorBridgeModel: ModelBase, IReadOnlyWebNavigatorBridge
    {
        #region property

        [XmlElement("navigating")]
        public WebNavigatorNavigatingModel Navigating { get; set; } = new WebNavigatorNavigatingModel();
        IReadOnlyWebNavigatorNavigating IReadOnlyWebNavigatorBridge.Navigating => Navigating;

        [XmlElement("context-menu")]
        public WebNavigatorContextMenuModel ContextMenu { get; set; } = new WebNavigatorContextMenuModel();
        IReadOnlyWebNavigatorContextMenu IReadOnlyWebNavigatorBridge.ContextMenu => ContextMenu;

        [XmlElement("gesture")]
        public WebNavigatorGestureModel Gesture { get; set; } = new WebNavigatorGestureModel();
        IReadOnlyWebNavigatorGesture IReadOnlyWebNavigatorBridge.Gesture => Gesture;

        #endregion
    }
}
