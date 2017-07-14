using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoWatchDataDmcHirobaModel : ModelBase
    {
        #region property

        [DataMember(Name = "fms_token")]
        public string FmsToken { get; set; }
        [DataMember(Name = "server_url")]
        public string ServerUrl { get; set; }
        [DataMember(Name = "server_port")]
        public string ServerPort { get; set; }
        [DataMember(Name = "thread_id")]
        public string ThreadId { get; set; }
        [DataMember(Name = "thread_key")]
        public string ThreadKey { get; set; }

        #endregion
    }
}