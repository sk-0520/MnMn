using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    [Serializable, XmlRoot("msg")]
    public class SmileVideoMsgModel: ModelBase
    {
        #region property

        [XmlArray("colors"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Colors { get; set; } = new CollectionModel<DefinedElementModel>();

        #endregion
    }
}
