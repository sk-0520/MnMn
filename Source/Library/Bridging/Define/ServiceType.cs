using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Attribute;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Define
{
    /// <summary>
    /// 対象サービス種別。
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// 本体。
        /// </summary>
        //[TextDisplay(Constants.applicationName)]
        Application,
        /// <summary>
        /// ニコニコ。
        /// </summary>
        //[EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_ServiceType_Smile))]
        Smile,
        /// <summary>
        /// ニコニコ動画。
        /// </summary>
        //[EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_ServiceType_SmileVideo))]
        SmileVideo,
        /// <summary>
        /// ニコニコ生放送。
        /// </summary>
        //[EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_ServiceType_SmileLive))]
        SmileLive,
    }
}
