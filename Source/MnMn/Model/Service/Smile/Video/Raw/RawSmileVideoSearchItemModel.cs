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
    public class RawSmileVideoSearchItemModel : ModelBase
    {
        #region property

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "first_retrieve")]
        public string FirstRetrieve { get; set; }

        [DataMember(Name = "view_counter")]
        public string ViewCounter { get; set; }

        [DataMember(Name = "mylist_counter")]
        public string MylistCounter { get; set; }

        [DataMember(Name = "thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [DataMember(Name = "num_res")]
        public string NumRes { get; set; }

        [DataMember(Name = "last_res_body")]
        public string last_res_body { get; set; }

        [DataMember(Name = "length")]
        public string Length { get; set; }

        [DataMember(Name = "title_short")]
        public string TitleShort { get; set; }

        [DataMember(Name = "description_short")]
        public string DescriptionShort { get; set; }

        [DataMember(Name = "thumbnail_style")]
        public string ThumbnailStyle { get; set; }

        [DataMember(Name = "is_middle_thumbnail")]
        public string IsMiddleThumbnail { get; set; }

        #endregion
    }
}
