using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyParameterItem : IReadOnlyKey
    {
        #region property

        /// <summary>
        /// 値。
        /// </summary>
        string Value { get; }

        /// <summary>
        /// 接頭辞。
        /// <para>UriParameterType.PreSuffixes で URI の場合に使用される。</para>
        /// </summary>
        string Prefix { get; }
        /// <summary>
        /// 接尾辞。
        /// <para>UriParameterType.PreSuffixes で URI の場合に使用される。</para>
        /// </summary>
        string Suffix { get; }
        /// <summary>
        /// 連結部。
        /// <para>UriParameterType.PreSuffixes で URI の場合に使用される。</para>
        /// </summary>
        string Bond { get; }
        /// <summary>
        /// 値が無い場合にはキーも処理対象外とする。
        /// </summary>
        bool Force { get; }

        ParameterEncode Encode { get; }

        bool IsMulti { get; }

        /// <summary>
        /// キーが存在するか。
        /// </summary>
        bool HasKey { get; }

        #endregion
    }
}
