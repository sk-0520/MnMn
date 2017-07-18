using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataVideoModel : ModelBase
    {
        #region property

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "originalTitle")]
        public string OriginalTitle { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "originalDescription")]
        public string OriginalDescription { get; set; }

        [DataMember(Name = "thumbnailURL")]
        public string ThumbnailURL { get; set; }

        [DataMember(Name = "postedDateTime")]
        public string PostedDateTime { get; set; }

        [DataMember(Name = "originalPostedDateTime")]
        public string OriginalPostedDateTime { get; set; }

        [DataMember(Name = "width")]
        public string Width { get; set; }

        [DataMember(Name = "height")]
        public string Height { get; set; }

        [DataMember(Name = "duration")]
        public string Duration { get; set; }

        [DataMember(Name = "viewCount")]
        public string ViewCount { get; set; }

        [DataMember(Name = "mylistCount")]
        public string MylistCount { get; set; }

        [DataMember(Name = "translation")]
        public string Translation { get; set; }

        [DataMember(Name = "translator")]
        public string Translator { get; set; }

        [DataMember(Name = "movieType")]
        public string MovieType { get; set; }

        [DataMember(Name = "badges")]
        public CollectionModel<RawSmileVideoWatchDataVideoBadgeModel> Badges { get; set; }

        [DataMember(Name = "introducedNicoliveDJInfo")]
        public object IntroducedNicoliveDJInfo { get; set; }

        [DataMember(Name = "dmcInfo")]
        public RawSmileVideoWatchDataDmcInfoModel DmcInfo { get; set; } = new RawSmileVideoWatchDataDmcInfoModel();

        [DataMember(Name = "backCommentType")]
        public string BackCommentType { get; set; }
        [DataMember(Name = "isCommentExpired")]
        public string IsCommentExpired { get; set; }
        [DataMember(Name = "isWide")]
        public string IsWide { get; set; }
        [DataMember(Name = "isOfficialAnime")]
        public string IsOfficialAnime { get; set; }
        [DataMember(Name = "isNoBanner")]
        public string IsNoBanner { get; set; }
        [DataMember(Name = "isDeleted")]
        public string IsDeleted { get; set; }
        [DataMember(Name = "isTranslated")]
        public string IsTranslated { get; set; }
        [DataMember(Name = "isR18")]
        public string IsR18 { get; set; }
        [DataMember(Name = "isAdult")]
        public string IsAdult { get; set; }
        [DataMember(Name = "isNicowari")]
        public string IsNicowari { get; set; }
        [DataMember(Name = "isPublic")]
        public string IsPublic { get; set; }
        [DataMember(Name = "isPublishedNicoscript")]
        public string IsPublishedNicoscript { get; set; }
        [DataMember(Name = "isNoNGS")]
        public string IsNoNGS { get; set; }
        [DataMember(Name = "isCommunityMemberOnly")]
        public string IsCommunityMemberOnly { get; set; }
        [DataMember(Name = "isCommonsTreeExists")]
        public string IsCommonsTreeExists { get; set; }
        [DataMember(Name = "isNoIchiba")]
        public string IsNoIchiba { get; set; }
        [DataMember(Name = "isOfficial")]
        public string IsOfficial { get; set; }
        [DataMember(Name = "isMonetized")]
        public string IsMonetized { get; set; }

        [DataMember(Name = "smileInfo")]
        public RawSmileVideoWatchDataSmileInfoModel SmileInfo { get; set; }

        #endregion
    }
}
