using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataDmcSessionApiUrlModel: ModelBase
    {
        #region property

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "is_well_known_port")]
        public string IsWellKnownPort { get; set; }

        [DataMember(Name = "is_ssl")]
        public string IsSsl { get; set; }

        #endregion
    }
}