using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    [Serializable]
    public class SmileVideoOfficialSearchModel: ModelBase, IReadOnlySmileVideoSearchDefine
    {
        #region ISmileVideoSearchDefine

        [XmlAttribute("max-index")]
        public int MaximumIndex { get; set; }
        [XmlAttribute("max-count")]
        public int MaximumCount { get; set; }

        [XmlArray("method"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Methods { get; set; } = new CollectionModel<DefinedElementModel>();
        IReadOnlyList<DefinedElementModel> IReadOnlySmileVideoSearchDefine.Methods => Methods;

        [XmlArray("sort"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Sort { get; set; } = new CollectionModel<DefinedElementModel>();
        IReadOnlyList<DefinedElementModel> IReadOnlySmileVideoSearchDefine.Sort => Sort;

        [XmlArray("type"), XmlArrayItem("element")]
        public CollectionModel<DefinedElementModel> Type { get; set; } = new CollectionModel<DefinedElementModel>();
        IReadOnlyList<DefinedElementModel> IReadOnlySmileVideoSearchDefine.Type => Type;

        #endregion
    }
}
