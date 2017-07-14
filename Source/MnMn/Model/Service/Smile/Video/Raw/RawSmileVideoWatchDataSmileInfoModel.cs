using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataSmileInfoModel: ModelBase
    {
        #region property

        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "isSlowLine")]
        public string IsSlowLine { get; set; }
        [DataMember(Name = "currentQualityId")]
        public string CurrentQualityId { get; set; }
        [DataMember(Name = "qualityIds")]
        public CollectionModel<string> QualityIds { get; set; }

        #endregion
    }
}