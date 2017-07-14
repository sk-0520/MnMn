using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataPlayListItemModel : ModelBase
    {
        #region property
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "requestId")]
        public string RequestId { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "thumbnailURL")]
        public string ThumbnailURL { get; set; }
        [DataMember(Name = "viewCounter")]
        public string ViewCounter { get; set; }
        [DataMember(Name = "numRes")]
        public string NumRes { get; set; }
        [DataMember(Name = "mylistCounter")]
        public string MylistCounter { get; set; }
        [DataMember(Name = "firstRetrieve")]
        public string FirstRetrieve { get; set; }
        [DataMember(Name = "lengthSeconds")]
        public string LengthSeconds { get; set; }
        [DataMember(Name = "threadUpdateTime")]
        public string ThreadUpdateTime { get; set; }
        [DataMember(Name = "createTime")]
        public string CreateTime { get; set; }
        [DataMember(Name = "width")]
        public string Width { get; set; }
        [DataMember(Name = "height")]
        public string Height { get; set; }
        [DataMember(Name = "isTranslated")]
        public string IsTranslated { get; set; }
        [DataMember(Name = "mylistComment")]
        public string MylistComment { get; set; }
        [DataMember(Name = "tkasType")]
        public string TkasType { get; set; }
        [DataMember(Name = "hasData")]
        public string HasData { get; set; }
        #endregion
    }
}