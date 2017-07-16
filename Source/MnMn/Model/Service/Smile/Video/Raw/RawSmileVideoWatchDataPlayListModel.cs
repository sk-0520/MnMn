using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataPlayListModel : ModelBase
    {
        #region property

        [DataMember(Name = "items")]
        public CollectionModel<RawSmileVideoWatchDataPlayListItemModel> Items { get; set; }

        [DataMember(Name = "type")]
        public string type { get; set; }
        [DataMember(Name = "ref")]
        public string Ref { get; set; }

        //[DataMember(Name = "option")]
        //public CollectionModel<object> Option { get; set; }
        #endregion
    }
}