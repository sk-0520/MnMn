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
        /// 正規表現。
        /// </summary>
        [XmlEnum("regex")]
        Regex,
    }
}
