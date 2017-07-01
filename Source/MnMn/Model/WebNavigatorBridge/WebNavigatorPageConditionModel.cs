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
    public class WebNavigatorPageConditionModel: ModelBase, IReadOnlyWebNavigatorPageCondition
    {
        #region property

        [XmlAttribute("uri")]
        public string UriPattern { get; set; }

        [XmlAttribute("param")]
        public string ParameterSource { get; set; }

        #endregion
    }
}
