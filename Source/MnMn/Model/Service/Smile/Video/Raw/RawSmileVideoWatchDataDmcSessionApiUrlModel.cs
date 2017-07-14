using System.Runtime.Serialization;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataDmcSessionApiUrlModel
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