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

        [DataMember(Name = "ping", EmitDefaultValue =false)]
        public RawSmileVideoMsgPingModel Ping { get; set; }

        [DataMember(Name = "thread", EmitDefaultValue = false)]
        public RawSmileVideoMsgThreadModel Thread { get; set; }

        [DataMember(Name = "leaf", EmitDefaultValue = false)]
        public RawSmileVideoMsgLeafModel Leaf { get; set; }

        [DataMember(Name = "global_num_res", EmitDefaultValue = false)]
        public RawSmileVideoMsgGlobalNumResModel GlobalNumRes { get; set; }

        [DataMember(Name = "chat", EmitDefaultValue = false)]
        public RawSmileVideoMsgChatModel Chat { get; set; }

        [DataMember(Name = "chat_result", EmitDefaultValue = false)]
        public RawSmileVideoMsgChatResultModel ChatResult { get; set; }

        #endregion
    }
}
