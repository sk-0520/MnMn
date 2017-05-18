using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting
{
    public class NetworkProxySettingModel : PasswordSettingModel, IReadOnlyNetworkProxy
    {
        #region IReadOnlyNetworkProxy

        [DataMember]
        public bool UsingCustomProxy { get; set; }
        [DataMember]
        public string ServerAddress { get; set; }

        [DataMember]
        public bool UsingAuth { get; set; }
        [DataMember]
        public string UserName { get; set; }

        #endregion
    }
}
