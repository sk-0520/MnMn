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

        /// <summary>
        /// コメントを有効にするか。
        /// <para>書式: $[//] 一行コメント</para>
        /// <para>書式: $[/*] $[*/] 複数行コメント</para>
        /// </summary>
        bool IsEnabledComment { get; set; }
        /// <summary>
        /// インラインでのトリムを行うか。
        /// <para>書式: $[n1:n2]</para>
        /// <para>n1: 左側の残すホワイトスペース。未設定ですべて。</para>
        /// <para>n2: 右側の残すホワイトスペース。未設定ですべて。</para>
        /// </summary>
        bool IsEnabledInlineTrime { get; set; }

        /// <summary>
        /// 条件フィルタを有効にするか。
        /// <para>書式: $[filter:真偽値:/*] $[filter:真偽値:*/] 複数行フィルタ</para>
        /// <para>一行のみはかったるいんでいらない</para>
        /// </summary>
        bool IsEnabledFilter { get; set; }

        string Value { get; }

        XmlNode[] CDataValue { get; }

        #endregion
    }
}
