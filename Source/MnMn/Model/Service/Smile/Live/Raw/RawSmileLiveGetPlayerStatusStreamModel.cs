using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live.Raw
{
    [Serializable]
    public class RawSmileLiveGetPlayerStatusStreamModel: ModelBase
    {
        #region property

        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("provider_type")]
        public string ProviderType { get; set; }

        [XmlElement("default_community")]
        public string DefaultCommunity { get; set; }

        [XmlElement("owner_id")]
        public string OwnerId { get; set; }

        [XmlElement("owner_name")]
        public string OwnerName { get; set; }

        /// <summary>
        /// 0: 通常, 1: 予約
        /// </summary>
        [XmlElement("is_reserved")]
        public string IsReserved { get; set; }

        [XmlElement("watch_count")]
        public string WatchCount { get; set; }

        [XmlElement("comment_count")]
        public string CommentCount { get; set; }

        [XmlElement("danjo_comment_mode")]
        public string DanjoCommentMode { get; set; }
        
        [XmlElement("relay_comment")]
        public string RelayComment { get; set; }

        [XmlElement("bourbon_url")]
        public string BourbonUrl { get; set; }

        [XmlElement("full_video")]
        public string FullVideo { get; set; }

        [XmlElement("after_video")]
        public string AfterVideo { get; set; }

        [XmlElement("before_video")]
        public string BeforeVideo { get; set; }

        [XmlElement("kickout_video")]
        public string KickoutVideo { get; set; }

        /// <summary>
        /// 0: 生放送, 1: タイムシフト。
        /// </summary>
        [XmlElement("archive")]
        public string Archive { get; set; }

        [XmlElement("ichiba_notice_enable")]
        public string IchibaNoticeEnable { get; set; }

        [XmlElement("comment_lock")]
        public string CommentLock { get; set; }

        [XmlElement("background_comment")]
        public string BackgroundComment { get; set; }

        [XmlElement("telop")]
        public RawSmileLiveGetPlayerStatusStreamTelopModel Telop { get; set; } = new RawSmileLiveGetPlayerStatusStreamTelopModel();

        [XmlElement("base_time")]
        public string BaseTime { get; set; }
        [XmlElement("open_time")]
        public string OpenTime { get; set; }
        [XmlElement("start_time")]
        public string StartTime { get; set; }
        [XmlElement("end_time")]
        public string EndTime { get; set; }

        [XmlArray("contents_list"), XmlArrayItem("contents")]
        public CollectionModel<RawSmileLiveGetPlayerStatusStreamContentModel> Content { get; set; } = new CollectionModel<RawSmileLiveGetPlayerStatusStreamContentModel>();

        [XmlElement("picture_url")]
        public string PictureUrl { get; set; }
        [XmlElement("thumb_url")]
        public string ThumbUrl { get; set; }

        [XmlElement("is_priority_prefecture")]
        public string IsPriorityPrefecture { get; set; }

        #endregion
    }
}
