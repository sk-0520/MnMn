using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Extensions
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// <see cref="HttpClient"/>にロジック用<see cref="IReadOnlyNetworkSetting"/>を設定する単一処理。
        /// <para>込み入った処理をしない単純な共通処理。</para>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="networkSetting"></param>
        public static void SetLogicUserAgentText(this HttpClient httpClient, IReadOnlyNetworkSetting networkSetting)
        {
            var userAgentText = NetworkUtility.GetLogicUserAgentText(networkSetting);
            if(!string.IsNullOrWhiteSpace(userAgentText)) {
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgentText);
            }
        }
    }
}
