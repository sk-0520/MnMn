using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class NetworkUtility
    {
        #region property

        /// <summary>
        /// ネットワーク接続が有効か確認。
        /// <para>インターネットじゃなくてネットワーク接続である点に留意。繋がってればインターネットいけると楽観視する。</para>
        /// <para><see cref="Constants.ApplicationCheckNetworkAvailable"/>で実際にチェックするか分岐する。</para>
        /// </summary>
        public static bool IsNetworkAvailable
        {
            get
            {
                if(!Constants.ApplicationCheckNetworkAvailable) {
                    return true;
                }

                var available = NetworkInterface.GetIsNetworkAvailable();
                return available;
            }
        }
        
        #endregion
        #region function

        public static string GetUserAgentText(string userAgentFormat)
        {
            if(string.IsNullOrEmpty(userAgentFormat)) {
                return string.Empty;
            }

            var usingMap = new StringsModel() {
                ["APP"] = Constants.ApplicationName,
                ["VER"] = Constants.ApplicationVersion,
                ["VER:NUM"] = Constants.ApplicationVersionNumber.ToString(),
                ["VER:REV"] = Constants.ApplicationVersionRevision,
            };

            var result = AppUtility.ReplaceString(userAgentFormat, usingMap);
            return result;
        }

        public static string GetLogicUserAgentText(IReadOnlyNetworkSetting networkSetting)
        {
            Debug.Assert(networkSetting != null);

            string userAgentText = null;
            if(networkSetting.LogicUserAgent.UsingCustomUserAgent) {
                userAgentText = GetUserAgentText(networkSetting.LogicUserAgent.CustomUserAgentFormat);
            }
            if(string.IsNullOrEmpty(userAgentText)) {
                userAgentText = GetUserAgentText(networkSetting.LogicUserAgent.CustomUserAgentFormat);
            }

            return userAgentText;
        }

        public static bool CanSetProxy(DateTime judgeTime, IReadOnlyNetworkProxy networkProxy)
        {
            if(networkProxy.UsingCustomProxy) {
                if(judgeTime < networkProxy.ChangedTimestamp) {
                    return true;
                }
            }

            return false;
        }

        public static bool CanSetProxy(IHttpUserAgentCreator httpUserAgentCreator, IReadOnlyNetworkProxy networkProxy)
        {
            return CanSetProxy(httpUserAgentCreator.LastProxyChangedTimestamp, networkProxy);
        }

        #endregion
    }
}
