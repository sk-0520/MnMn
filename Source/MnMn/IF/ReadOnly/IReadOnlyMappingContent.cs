using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyMappingContent
    {
        #region property

        /// <summary>
        ///
        /// </summary>
        MappingContentTrim Trim { get; set; }

        /// <summary>
        /// 一行にまとめるか。
        /// <para>トリム処理後に実施される。</para>
        /// </summary>
        bool Oneline { get; set; }

        string Value { get; }

        XmlNode[] CDataValue { get; }

        #endregion
    }
}
