using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable]
    public class ExpressionItemModel : ModelBase, IReadOnlyExpressionItem
    {
        #region IReadOnlyExpressionElement

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("kind")]
        public ExpressionItemKind Kind { get; set; }

        [XmlAttribute("regex-options")]
        public RegexOptions RegexOptions { get;set;}

        [XmlElement("data")]
        public CDataModel Data { get; set; } = new CDataModel();
        IReadOnlyCData IReadOnlyExpressionItem.Data => Data;

        #endregion
    }
}
