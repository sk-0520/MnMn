using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Setting
{
    [Serializable]
    public class NetworkSettingModel : SettingModelBase, IReadOnlyNetworkSetting
    {
        #region IReadOnlyNetworkSetting

        [DataMember]
        public bool UsingCustomUserAgent { get; set; } = Constants.SettingApplicationNetworkUsingCustomUserAgent;

        [DataMember]
        public string UserAgentFormat { get; set; } = Constants.SettingApplicationNetworkCustomUserAgentFormat;

        #endregion
    }
}
