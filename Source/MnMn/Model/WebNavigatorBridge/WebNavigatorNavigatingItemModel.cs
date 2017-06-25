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
    public class WebNavigatorNavigatingItemModel: WebNavigatorDefinedElementModelBase, IReadOnlyWebNavigatorNavigatingItem
    {
        #region property

        [XmlArray("conditions"), XmlArrayItem("condition")]
        public CollectionModel<WebNavigatorPageConditionModel> Conditions { get; set; } = new CollectionModel<WebNavigatorPageConditionModel>();
        IReadOnlyList<IReadOnlyWebNavigatorPageCondition> IReadOnlyWebNavigatorNavigatingItem.Conditions => Conditions;

        #endregion
    }
}
