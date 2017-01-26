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
        /// 共通。
        /// <para>これなんかやだなぁ。</para>
        /// </summary>
        Common,
        /// <summary>
        /// 本体。
        /// </summary>
        Application,
        /// <summary>
        /// ニコニコ。
        /// </summary>
        Smile,
        /// <summary>
        /// ニコニコ動画。
        /// </summary>
        SmileVideo,
        /// <summary>
        /// ニコニコ生放送。
        /// </summary>
        SmileLive,
    }
}
