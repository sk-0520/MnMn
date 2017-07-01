using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyUriItem
    {
        #region property
        /// <summary>
        /// URIの検索キー。
        /// </summary>
        string Key { get; set; }
        /// <summary>
        /// 加工前URI。
        /// <para>完全なURIではない。</para>
        /// </summary>
        string Uri { get; set; }

        /// <summary>
        /// 廃止されているか。
        /// </summary>
        bool IsObsolete { get; set; }

        /// <summary>
        /// URI加工タイプ。
        /// </summary>
        UriParameterType UriParameterType { get; set; }

        /// <summary>
        /// 要求時のパラメータ種別。
        /// </summary>
        ParameterType RequestParameterType { get; set; }

        /// <summary>
        /// URIはセキュリティを考慮するか。
        /// <para>ログ出力で用いられる。</para>
        /// </summary>
        bool SafetyUri { get; set; }

        /// <summary>
        /// リクエストヘッダーはセキュリティを考慮するか。
        /// <para>ログ出力で用いられる。</para>
        /// </summary>
        bool SafetyHeader { get; set; }

        /// <summary>
        /// パラメータはセキュリティを考慮するか。
        /// <para>ログ出力で用いられる。</para>
        /// </summary>
        bool SafetyParameter { get; set; }

        #endregion
    }
}
