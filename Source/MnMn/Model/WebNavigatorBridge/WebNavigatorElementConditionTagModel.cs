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
    public class WebNavigatorElementConditionTagModel: ModelBase
    {
        #region property

        [XmlAttribute("tag")]
        public string TagNamePattern { get; set; }

        [XmlAttribute("attribute")]
        public string Attribute { get; set; }

        [XmlAttribute("value")]
        public string ValuePattern { get; set; }

        #endregion
    }
}
