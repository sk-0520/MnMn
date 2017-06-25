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
    public class WebNavigatorElementConditionTagModel: ModelBase, IReadOnlyWebNavigatorElementConditionTag
    {
        #region IReadOnlyWebNavigatorElementConditionTag

        [XmlAttribute("tag")]
        public string TagNamePattern { get; set; }

        [XmlAttribute("attribute")]
        public string Attribute { get; set; }

        [XmlAttribute("value")]
        public string ValuePattern { get; set; }

        #endregion
    }
}
