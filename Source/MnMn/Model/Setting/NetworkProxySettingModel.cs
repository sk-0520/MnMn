using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting
{
    [Serializable, DataContract]
    public class NetworkProxySettingModel : PasswordSettingModel, IReadOnlyNetworkProxy
    {
        #region IReadOnlyNetworkProxy

        [DataMember]
        public bool UsingCustomProxy { get; set; }
        [DataMember]
        public string ServerAddress { get; set; }
        [DataMember]
        public int ServerPort { get; set; }

        [DataMember]
        public bool UsingAuth { get; set; }
        [DataMember]
        public string UserName { get; set; }

        [IgnoreDataMember]
        public DateTime ChangedTimestamp { get; set; } = DateTime.Now;

        #endregion
    }
}
