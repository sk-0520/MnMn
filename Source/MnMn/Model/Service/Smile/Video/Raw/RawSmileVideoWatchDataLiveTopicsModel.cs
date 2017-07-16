using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataLiveTopicsModel : ModelBase
    {
        #region property

        [DataMember(Name = "items")]
        public CollectionModel<RawSmileVideoWatchDataLiveTopicsItemModel> Items { get; set; }

        #endregion
    }
}