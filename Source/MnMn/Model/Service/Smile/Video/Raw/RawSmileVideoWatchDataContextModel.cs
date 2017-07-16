using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataContextModel : ModelBase
    {
        #region property

        [DataMember(Name = "playFrom")]
        public string PlayFrom { get; set; }
        [DataMember(Name = "initialPlaybackPosition")]
        public string InitialPlaybackPosition { get; set; }
        [DataMember(Name = "initialPlaybackType")]
        public string InitialPlaybackType { get; set; }
        [DataMember(Name = "playLength")]
        public string PlayLength { get; set; }
        [DataMember(Name = "returnId")]
        public string ReturnId { get; set; }
        [DataMember(Name = "returnTo")]
        public string ReturnTo { get; set; }
        [DataMember(Name = "returnMsg")]
        public string ReturnMsg { get; set; }
        [DataMember(Name = "watchId")]
        public string WatchId { get; set; }
        [DataMember(Name = "isNoMovie")]
        public string IsNoMovie { get; set; }
        [DataMember(Name = "isNoRelatedVideo")]
        public string IsNoRelatedVideo { get; set; }
        [DataMember(Name = "isDownloadCompleteWait")]
        public string IsDownloadCompleteWait { get; set; }
        [DataMember(Name = "isNoNicotic")]
        public string IsNoNicotic { get; set; }
        [DataMember(Name = "isNeedPayment")]
        public string IsNeedPayment { get; set; }
        [DataMember(Name = "isAdultRatingNG")]
        public string IsAdultRatingNG { get; set; }
        [DataMember(Name = "isPlayable")]
        public string IsPlayable { get; set; }
        [DataMember(Name = "isTranslatable")]
        public string IsTranslatable { get; set; }
        [DataMember(Name = "isTagUneditable")]
        public string IsTagUneditable { get; set; }
        [DataMember(Name = "isVideoOwner")]
        public string IsVideoOwner { get; set; }
        [DataMember(Name = "isThreadOwner")]
        public string IsThreadOwner { get; set; }
        [DataMember(Name = "isOwnerThreadEditable")]
        public string IsOwnerThreadEditable { get; set; }
        [DataMember(Name = "useChecklistCache")]
        public string UseChecklistCache { get; set; }
        [DataMember(Name = "isDisabledMarquee")]
        public string IsDisabledMarquee { get; set; }
        [DataMember(Name = "isDictionaryDisplayable")]
        public string IsDictionaryDisplayable { get; set; }
        [DataMember(Name = "isDefaultCommentInvisible")]
        public string IsDefaultCommentInvisible { get; set; }
        [DataMember(Name = "accessFrom")]
        public string AccessFrom { get; set; }
        [DataMember(Name = "csrfToken")]
        public string CsrfToken { get; set; }
        [DataMember(Name = "translationVersionJsonUpdateTime")]
        public string TranslationVersionJsonUpdateTime { get; set; }
        [DataMember(Name = "userkey")]
        public string Userkey { get; set; }
        [DataMember(Name = "watchAuthKey")]
        public string WatchAuthKey { get; set; }
        [DataMember(Name = "watchTrackId")]
        public string WatchTrackId { get; set; }
        [DataMember(Name = "watchPageServerTime")]
        public string WatchPageServerTime { get; set; }
        [DataMember(Name = "isAuthenticationRequired")]
        public string IsAuthenticationRequired { get; set; }
        [DataMember(Name = "isPeakTime")]
        public string IsPeakTime { get; set; }
        [DataMember(Name = "ngRevision")]
        public string NgRevision { get; set; }
        [DataMember(Name = "categoryName")]
        public string CategoryName { get; set; }
        [DataMember(Name = "categoryKey")]
        public string CategoryKey { get; set; }
        [DataMember(Name = "categoryGroupName")]
        public string CategoryGroupName { get; set; }
        [DataMember(Name = "categoryGroupKey")]
        public string CategoryGroupKey { get; set; }
        [DataMember(Name = "yesterdayRank")]
        public string YesterdayRank { get; set; }
        [DataMember(Name = "highestRank")]
        public string HighestRank { get; set; }
        [DataMember(Name = "isMyMemory")]
        public string IsMyMemory { get; set; }

        [DataMember(Name = "ownerNGList")]
        public CollectionModel<RawSmileVideoWatchDataContextOwnerNgItemModel> ownerNGList { get; set; }

        #endregion
    }
}