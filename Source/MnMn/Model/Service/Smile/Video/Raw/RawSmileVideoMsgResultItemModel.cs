using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw
{
    [Serializable, DataContract]
    public class RawSmileVideoMsgResultItemModel : ModelBase
    {
        #region property

        [DataMember(Name = "ping")]
        public RawSmileVideoMsgPingModel Ping { get; set; }

        [DataMember(Name = "thread")]
        public RawSmileVideoMsgThreadModel Thread { get; set; }

        [DataMember(Name = "leaf")]
        public RawSmileVideoMsgLeafModel Leaf { get; set; }

        [DataMember(Name = "global_num_res")]
        public RawSmileVideoMsgGlobalNumResModel GlobalNumRes { get; set; }

        [DataMember(Name = "chat")]
        public RawSmileVideoMsgChatModel Chat { get; set; }

        [DataMember(Name = "chat_result")]
        public RawSmileVideoMsgChatResultModel ChatResult { get; set; }

        #endregion
    }
}
