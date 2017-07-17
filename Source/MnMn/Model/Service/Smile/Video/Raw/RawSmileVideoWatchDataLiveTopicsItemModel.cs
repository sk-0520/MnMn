using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataLiveTopicsItemModel : ModelBase
    {
        #region property
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "thumbnailURL")]
        public string ThumbnailURL { get; set; }
        [DataMember(Name = "point")]
        public string Point { get; set; }
        [DataMember(Name = "isHigh")]
        public string IsHigh { get; set; }
        [DataMember(Name = "elapsedTimeM")]
        public string ElapsedTimeM { get; set; }
        [DataMember(Name = "communityId")]
        public string CommunityId { get; set; }
        [DataMember(Name = "communityName")]
        public string CommunityName { get; set; }
        #endregion
    }
}