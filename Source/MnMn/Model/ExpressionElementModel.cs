using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    [Serializable]
    public class ExpressionElementModel: DefinedElementModel
    {
        #region property

        [XmlElement("item")]
        public CollectionModel<ExpressionItemModel> Items { get; set; } = new CollectionModel<ExpressionItemModel>();

        #endregion
    }
}
