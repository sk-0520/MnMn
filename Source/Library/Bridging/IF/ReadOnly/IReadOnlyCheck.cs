using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly
{
    public interface IReadOnlyCheck
    {
        #region property

        /// <summary>
        /// 成功状態。
        /// <para>必須項目。</para>
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// 詳細。
        /// </summary>
        object Detail { get; }

        /// <summary>
        /// メッセージ。
        /// </summary>
        string Message { get; }

        #endregion
    }
}
