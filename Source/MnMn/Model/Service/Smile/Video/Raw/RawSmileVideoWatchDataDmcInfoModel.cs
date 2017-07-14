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
    public class RawSmileVideoWatchDataDmcInfoModel : ModelBase
    {
        #region property

        [DataMember(Name = "time")]
        public string Time { get; set; }

        [DataMember(Name = "time_ms")]
        public string TimeMs { get; set; }

        [DataMember(Name = "video")]
        public RawSmileVideoWatchDataDmcVideoModel Video { get; set; }

        [DataMember(Name = "thread")]
        public RawSmileVideoWatchDataDmcThreadModel Thread { get; set; }

        [DataMember(Name = "user")]
        public RawSmileVideoWatchDataDmcUserModel User { get; set; }

        [DataMember(Name = "hiroba")]
        public RawSmileVideoWatchDataDmcHirobaModel Hiroba { get; set; }

        //[DataMember(Name = "error")]
        //public object error { get; set; }

        [DataMember(Name = "session_api")]
        public RawSmileVideoWatchDataDmcSessionApiModel SessionApi {get;set;}

        //[DataMember(Name = "storyboard_session_api")]
        //public object storyboard_session_api { get; set; }

        [DataMember(Name = "quality")]
        public RawSmileVideoWatchDataDmcQualityModel Quality { get; set; }

        #endregion
    }
}
