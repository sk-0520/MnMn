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
    [Serializable]
    public class NetworkSettingModel : SettingModelBase, IReadOnlyNetworkSetting
    {
        #region IReadOnlyNetworkSetting


        [DataMember]
        public NetworkUserAgentSettingModel LogicUserAgent { get; set; } = new NetworkUserAgentSettingModel() {
            UsingCustomUserAgent = Constants.SettingApplicationNetworkLogicUsingCustomUserAgent,
            CustomUserAgentFormat = Constants.SettingApplicationNetworkLogicCustomUserAgentFormat,
        };
        IReadOnlyUserAgent IReadOnlyNetworkSetting.LogicUserAgent => LogicUserAgent;

        [DataMember]
        public NetworkProxySettingModel LogicProxy { get; set; } = new NetworkProxySettingModel();
        IReadOnlyNetworkProxy IReadOnlyNetworkSetting.LogicProxy => LogicProxy;


        [DataMember]
        public NetworkUserAgentSettingModel BrowserUserAgent { get; set; } = new NetworkUserAgentSettingModel() {
            UsingCustomUserAgent = Constants.SettingApplicationNetworkBrowserUsingCustomUserAgent,
            CustomUserAgentFormat = Constants.SettingApplicationNetworkBrowserCustomUserAgentFormat,
        };
        IReadOnlyUserAgent IReadOnlyNetworkSetting.BrowserUserAgent => BrowserUserAgent;

        [DataMember]
        public NetworkProxySettingModel BrowserProxy { get; set; } = new NetworkProxySettingModel();
        IReadOnlyNetworkProxy IReadOnlyNetworkSetting.BrowserProxy => BrowserProxy;

        #endregion
    }
}
