using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataDmcQualityModel: ModelBase
    {
        #region proeprty

        [DataMember(Name = "videos")]
        CollectionModel<RawSmileVideoWatchDataDmcQualityVideoModel> Videos { get; set; }

        [DataMember(Name = "audios")]
        CollectionModel<RawSmileVideoWatchDataDmcQualityAudioModel> Audios { get; set; }

        #endregion
    }
}