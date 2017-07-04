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
        #region property

        [XmlElement("force-accept")]
        public string _ForceAccept { get; set; }

        #endregion

        #region IReadOnlyAcceptVersion

        [XmlIgnore]
        public Version ForceAcceptVersion {
            get => new Version(_ForceAccept);
            set { _ForceAccept = value.ToString(); }
        }

        [XmlArray("versions"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Items { get; set; }
        IReadOnlyList<IReadOnlyDefinedElement> IReadOnlyAcceptVersion.Items => Items;

        #endregion
    }
}
