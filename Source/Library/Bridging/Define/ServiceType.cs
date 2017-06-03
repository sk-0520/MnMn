using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.MnMn.Library.Bridging.Attribute;

namespace ContentTypeTextNet.MnMn.Library.Bridging.Define
{
    /// <summary>
    /// 対象サービス種別。
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// 全て。
        /// <para>ブラウザの橋渡し用。</para>
        /// </summary>
        [XmlEnum("*")]
        All,
        /// <summary>
        /// 共通。
        /// <para>これなんかやだなぁ。</para>
        /// </summary>
        [XmlEnum("common")]
        Common,
        /// <summary>
        /// 本体。
        /// </summary>
        [XmlEnum("app")]
        Application,
        /// <summary>
        /// ニコニコ。
        /// </summary>
        [XmlEnum("smile")]
        Smile,
        /// <summary>
        /// ニコニコ動画。
        /// </summary>
        [XmlEnum("smile-video")]
        SmileVideo,
        /// <summary>
        /// ニコニコ生放送。
        /// </summary>
        [XmlEnum("smile-live")]
        SmileLive,
        /// <summary>
        /// Twitter。
        /// </summary>
        [XmlEnum("idle_talk")]
        IdleTalk,
        /// <summary>
        /// ツイート。
        /// </summary>
        [XmlEnum("idle_talk-mutter")]
        IdleTalkMutter,
    }
}
