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
        public bool LogicUsingCustomUserAgent { get; set; } = Constants.SettingApplicationNetworkLogicUsingCustomUserAgent;

        [DataMember]
        public string LogicUserAgentFormat { get; set; } = Constants.SettingApplicationNetworkLogicCustomUserAgentFormat;

        #endregion
    }
}
