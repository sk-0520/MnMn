using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoSearchThumbnailStyleModel : ModelBase
    {
        #region property

        [DataMember(Name = "offset_x")]
        public string OffsetX { get; set; }

        [DataMember(Name = "offset_y")]
        public string OffsetY { get; set; }

        [DataMember(Name = "width")]
        public string Width { get; set; }

        #endregion
    }
}
