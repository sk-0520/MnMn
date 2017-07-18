using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataLeadModel : ModelBase
    {
        #region property

        [DataMember(Name = "tagRelatedMarquee")]
        public object TagRelatedMarquee { get; set; }
        [DataMember(Name = "tagRelatedBanner")]
        public object TagRelatedBanner { get; set; }
        [DataMember(Name = "nicosdkApplicationBanner")]
        public object NicosdkApplicationBanner { get; set; }
        [DataMember(Name = "videoEndBannerIn")]
        public object VideoEndBannerIn { get; set; }
        [DataMember(Name = "videoEndOverlay")]
        public object VideoEndOverlay { get; set; }

        #endregion
    }
}