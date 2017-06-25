using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting
{
    public interface IReadOnlyFilteringItem
    {
        #region property

        /// <summary>
        /// 有効無効。
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// フィルタリング方法。
        /// </summary>
        FilteringType Type { get; }

        /// <summary>
        /// 大文字小文字を無視するか。
        /// </summary>
        bool IgnoreCase { get; }

        /// <summary>
        /// フィルタリング文字列。
        /// </summary>
        string Source { get; }

        /// <summary>
        /// フィルタ名。
        /// </summary>
        string Name { get; }

        #endregion
    }
}
