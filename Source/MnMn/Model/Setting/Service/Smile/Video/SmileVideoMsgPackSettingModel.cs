using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video
{
    [Serializable, DataContract]
    public class SmileVideoMsgPackSettingModel : SettingModelBase
    {
        #region property

        [DataMember]
        public Dictionary<SmileVideoMsgPacketId, int> PacketId { get; set; } = new Dictionary<SmileVideoMsgPacketId, int>();

        [DataMember]
        public CollectionModel<RawSmileVideoMsgResultItemModel> Items { get; set; } = new CollectionModel<RawSmileVideoMsgResultItemModel>();

        #endregion
    }
}
