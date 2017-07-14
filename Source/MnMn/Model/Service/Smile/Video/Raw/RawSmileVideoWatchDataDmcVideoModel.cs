using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataDmcVideoModel: ModelBase
    {
        #region property

        [DataMember(Name = "video_id")]
        public string VideoId { get; set; }

        [DataMember(Name = "length_seconds")]
        public string LengthSeconds { get; set; }

        [DataMember(Name = "deleted")]
        public string Deleted { get; set; }

        #endregion
    }
}
