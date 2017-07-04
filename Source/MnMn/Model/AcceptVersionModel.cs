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
    [Serializable, XmlRoot("accept-version")]
    public class AcceptVersionModel : ModelBase, IReadOnlyAcceptVersion
    {
        #region IReadOnlyAcceptVersion

        [XmlArray("versions"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Items { get; set; }
        IReadOnlyList<IReadOnlyDefinedElement> IReadOnlyAcceptVersion.Items => Items;

        #endregion
    }
}
