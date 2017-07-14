using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataDmcQualityAudioModel:ModelBase
    {
        #region property

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "available")]
        public string Available { get; set; }

        [DataMember(Name = "bitrate")]
        public string Bitrate { get; set; }

        [DataMember(Name = "sampling_rate")]
        public string SamplingRate { get; set; }

        #endregion
    }
}