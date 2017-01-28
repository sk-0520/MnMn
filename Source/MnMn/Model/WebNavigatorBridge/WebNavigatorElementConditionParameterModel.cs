using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge
{
    public class WebNavigatorElementConditionParameterModel: WebNavigatorElementConditionTagModel
    {
        #region property

        [XmlAttribute("param")]
        public string ParameterSource { get; set; }

        #endregion
    }
}
