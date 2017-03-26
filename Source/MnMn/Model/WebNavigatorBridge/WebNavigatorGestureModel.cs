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
    public class WebNavigatorGestureModel: ModelBase
    {
        #region property

        [XmlElement("element")]
        public CollectionModel<WebNavigatorGestureElementModel> Items { get; set; } = new CollectionModel<WebNavigatorGestureElementModel>();

        #endregion
    }
}
