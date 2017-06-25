using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyMapping
    {
        #region property

        string ContentType { get; set; }

        /// <summary>
        /// マッピング項目。
        /// </summary>
        IReadOnlyList<IReadOnlyMappingItem> Items { get; }

        /// <summary>
        /// マッピング元データ。
        /// </summary>
        IReadOnlyMappingContent Content { get; }

        #endregion
    }
}
