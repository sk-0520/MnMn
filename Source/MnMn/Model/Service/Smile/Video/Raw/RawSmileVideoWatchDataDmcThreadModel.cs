using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    public class RawSmileVideoWatchDataDmcThreadModel: ModelBase
    {
        #region property

        [DataMember(Name = "server_url")]
        public string ServerUrl { get; set; }

        [DataMember(Name = "sub_server_url")]
        public string SubServerUrl { get; set; }

        [DataMember(Name = "thread_id")]
        public string ThreadId { get; set; }

        [DataMember(Name = "nicos_thread_id")]
        public string NicosThreadId { get; set; }

        [DataMember(Name = "optional_thread_id")]
        public string OptionalThreadId { get; set; }

        [DataMember(Name = "thread_key_required")]
        public string ThreadKeyRequired { get; set; }

        //[DataMember(Name = "channel_ng_words")]
        //public string channel_ng_words { get; set; }

        //[DataMember(Name = "owner_ng_words")]
        //public string owner_ng_words { get; set; }

        [DataMember(Name = "maintenances_ng")]
        public string MaintenancesNg { get; set; }

        [DataMember(Name = "postkey_available")]
        public string PostkeyAvailable { get; set; }

        [DataMember(Name = "ng_revision")]
        public string NgRevision { get; set; }

        #endregion
    }
}
