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
    public class WebNavigatorNavigatingItemModel: WebNavigatorDefinedElementModelBase
    {
        #region property

        [XmlArray("conditions"), XmlArrayItem("condition")]
        public CollectionModel<WebNavigatorPageConditionModel> Conditions { get; set; } = new CollectionModel<WebNavigatorPageConditionModel>();

        #endregion
    }
}
