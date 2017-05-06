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
    public class RawSmileVideoSearchModel : ModelBase
    {
        #region property

        [DataMember(Name = "ss_id")]
        public string SessionId { get; set; }

        [DataMember(Name = "list")]
        public CollectionModel<RawSmileVideoSearchItemModel> List { get; set; }

        [DataMember(Name = "count")]
        public string Count { get; set; }

        [DataMember(Name = "has_ng_video_for_adsense_on_listing")]
        public string HasNgVideoForAdsenseOnListing { get; set; }

        [DataMember(Name = "related_tags")]
        public CollectionModel<string> RelatedTags { get; set; }

        [DataMember(Name = "page")]
        public string Page { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        #endregion
    }
}
