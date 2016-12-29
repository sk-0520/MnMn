using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile
{
    [Serializable, XmlRoot("user")]
    public class SmileUserDefinedModel: ModelBase
    {
        /// <summary>
        /// バージョン一覧。
        /// </summary>
        [XmlArray("version"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> VersionItems { get; set; } = new CollectionModel<DefinedElementModel>();
    }
}
