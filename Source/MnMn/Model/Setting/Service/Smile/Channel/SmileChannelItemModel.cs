using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Channel
{
    [Serializable, DataContract]
    public class SmileChannelItemModel: SettingModelBase
    {
        #region property

        [DataMember]
        public string ChannelId { get; set; }

        [DataMember]
        public string ChannelName { get; set; }

        /// <summary>
        /// 更新日時(作成日時)。
        /// </summary>
        [DataMember]
        public DateTime UpdateTimestamp { get; set; }

        #endregion
    }
}
