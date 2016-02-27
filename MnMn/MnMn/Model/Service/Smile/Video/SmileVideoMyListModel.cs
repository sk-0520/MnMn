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
        [XmlArray("folders"), XmlArrayItem("element")]
        public CollectionModel<SmileVideoElementModel> Folders { get; set; } = new CollectionModel<SmileVideoElementModel>();
    }
}
