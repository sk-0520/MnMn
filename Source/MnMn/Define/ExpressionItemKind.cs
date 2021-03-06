using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ContentTypeTextNet.MnMn.MnMn.Define
{
    public enum ExpressionItemKind
    {
        /// <summary>
        /// 単語。
        /// </summary>
        [XmlEnum("word")]
        Word,
        /// <summary>
        /// 正規表現。
        /// </summary>
        [XmlEnum("regex")]
        Regex,
        /// <summary>
        /// XPath。
        /// </summary>
        [XmlEnum("xpath")]
        XPath,
    }
}
