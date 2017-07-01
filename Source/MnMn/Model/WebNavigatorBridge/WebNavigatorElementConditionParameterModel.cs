using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge
{
    public class WebNavigatorElementConditionParameterModel: WebNavigatorElementConditionTagModel, IReadOnlyWebNavigatorElementConditionParameter
    {
        #region IReadOnlyWebNavigatorElementConditionParameter

        [XmlAttribute("param")]
        public string ParameterSource { get; set; }

        #endregion
    }
}
