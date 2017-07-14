using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataViewerModel : ModelBase
    {
        #region property

        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }
        [DataMember(Name = "prefecture")]
        public string Prefecture { get; set; }
        [DataMember(Name = "sex")]
        public string Sex { get; set; }
        [DataMember(Name = "age")]
        public string Age { get; set; }
        [DataMember(Name = "isPremium")]
        public string IsPremium { get; set; }
        [DataMember(Name = "isPrivileged")]
        public string IsPrivileged { get; set; }
        [DataMember(Name = "isPostLocked")]
        public string IsPostLocked { get; set; }
        [DataMember(Name = "isHtrzm")]
        public string IsHtrzm { get; set; }
        [DataMember(Name = "isTwitterConnection")]
        public string IsTwitterConnection { get; set; }

        #endregion
    }
}