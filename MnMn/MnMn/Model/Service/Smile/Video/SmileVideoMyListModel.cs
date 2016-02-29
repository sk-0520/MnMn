using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    [Serializable, XmlRoot("mylist")]
    public class SmileVideoMyListModel: ModelBase
    {
        [XmlArray("folder"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Folder { get; set; } = new CollectionModel<DefinedElementModel>();

        [XmlArray("sort"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Sort { get; set; } = new CollectionModel<DefinedElementModel>();

        [XmlElement("create-group-name")]
        public DefinedElementModel CreateGroupName { get; set; } = new DefinedElementModel();
    }
}
