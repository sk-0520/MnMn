using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoDmcContentSrcIdModel: ModelBase
    {
        #region property

        [XmlArray("video_src_ids"), XmlArrayItem("string"), DataMember(Name = "video_src_ids")]
        public CollectionModel<string> VideoSrcIds { get; set; } = new CollectionModel<string>();

        [XmlArray("audio_src_ids"), XmlArrayItem("string"), DataMember(Name = "audio_src_ids")]
        public CollectionModel<string> AudioSrcIds { get; set; } = new CollectionModel<string>();

        #endregion
    }
}
