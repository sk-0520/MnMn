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
    public class WebNavigatorElementConditionItemModel: ModelBase
    {
        #region property

        [XmlAttribute("visible")]
        public bool IsVisible { get; set; }

        [XmlElement("base")]
        public string BaseUriPattern { get; set; }

        [XmlArray("targets"), XmlArrayItem("target")]
        public CollectionModel<WebNavigatorElementConditionTagModel> TargetItems { get; set; } = new CollectionModel<WebNavigatorElementConditionTagModel>();

        [XmlElement("parameter")]
        public WebNavigatorElementConditionParameterModel Parameter { get; set; } = new WebNavigatorElementConditionParameterModel();

        #endregion
    }
}
