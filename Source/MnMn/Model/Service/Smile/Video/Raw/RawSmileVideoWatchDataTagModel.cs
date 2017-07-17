using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataTagModel : ModelBase
    {
        #region property

        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "isCategory")]
        public string IsCategory { get; set; }
        [DataMember(Name = "isCategoryCandidate")]
        public string IsCategoryCandidate { get; set; }
        [DataMember(Name = "isDictionaryExists")]
        public string IsDictionaryExists { get; set; }
        [DataMember(Name = "isLocked")]
        public string IsLocked { get; set; }

        #endregion
    }
}