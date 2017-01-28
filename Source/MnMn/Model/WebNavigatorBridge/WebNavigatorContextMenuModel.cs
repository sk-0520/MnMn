using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge
{
    [Serializable]
    public class WebNavigatorContextMenuModel: ModelBase
    {
        #region property

        [XmlElement("element")]
        public CollectionModel<WebNavigatorContextMenuItemModel> Items { get; set; } = new CollectionModel<WebNavigatorContextMenuItemModel>();

        #endregion
    }
}
