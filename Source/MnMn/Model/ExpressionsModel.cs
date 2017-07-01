using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable, XmlRoot("expressions")]
    public class ExpressionsModel: ModelBase, IReadOnlyExpressions
    {
        #region property

        [XmlElement("element")]
        public CollectionModel<ExpressionElementModel> Elements { get; set; } = new CollectionModel<ExpressionElementModel>();
        IReadOnlyList<IReadOnlyExpressionElement> IReadOnlyExpressions.Elements => Elements;

        #endregion
    }
}
