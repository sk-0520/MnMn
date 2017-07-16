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
    public class RawSmileVideoWatchDataApiModel : RawModelBase
    {
        #region property

        [DataMember(Name = "video")]
        public RawSmileVideoWatchDataVideoModel Video { get; set; }

        [DataMember(Name = "player")]
        public RawSmileVideoWatchDataPlayerModel Player { get; set; }

        [DataMember(Name = "thread")]
        public RawSmileVideoWatchDataThreadModel Thread { get; set; }

        [DataMember(Name = "tags")]
        public CollectionModel<RawSmileVideoWatchDataTagModel> Tags { get; set; }

        [DataMember(Name = "playlist")]
        public RawSmileVideoWatchDataPlayListModel PlayList { get; set; }

        [DataMember(Name = "owner")]
        public RawSmileVideoWatchDataOwnerModel Owner { get; set; }

        [DataMember(Name = "viewer")]
        public RawSmileVideoWatchDataViewerModel Viewer { get; set; }

        //[DataMember(Name = "community")]
        //public object community { get; set; }
        //[DataMember(Name = "channel")]
        //public object channel { get; set; }

        [DataMember(Name = "ad")]
        public RawSmileVideoWatchDataAdModel Ad { get; set; }

        [DataMember(Name = "lead")]
        public RawSmileVideoWatchDataLeadModel Lead { get; set; }

        //[DataMember(Name = "maintenance")]
        //public object maintenance { get; set; }

        [DataMember(Name = "context")]
        public RawSmileVideoWatchDataContextModel Context { get; set; }

        [DataMember(Name = "liveTopics")]
        public RawSmileVideoWatchDataLiveTopicsModel LiveTopics { get;set;}

        #endregion
    }
}
