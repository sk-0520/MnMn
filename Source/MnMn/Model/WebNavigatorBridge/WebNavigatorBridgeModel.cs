using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge
{
    [Serializable, XmlRoot("web-navigator")]
    public class WebNavigatorBridgeModel: ModelBase
    {
        #region property

        [XmlElement("context-menu")]
        public WebNavigatorContextMenuModel ContextMenu { get; set; } = new WebNavigatorContextMenuModel();

        #endregion
    }
}
