using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataVideoBadgeModel: RawModelBase
    {
        #region proerpty

        [DataMember(Name = "badge_id")]
        public string BadgeId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "icon_url")]
        public string IconUrl { get; set; }
        [DataMember(Name = "small_icon_url")]
        public string SmallIconUrl { get; set; }
        //[DataMember(Name = "link")]
        //public object Link { get; set; }
        [DataMember(Name = "sequence")]
        public string Sequence { get; set; }

        #endregion
    }
}