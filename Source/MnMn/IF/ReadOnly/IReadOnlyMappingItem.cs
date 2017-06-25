using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    interface IReadOnlyMappingItem
    {
        #region property

        MappingItemType Type { get; set; }

        string Name { get; set; }

        string Bond { get; set; }

        string Value { get; set; }

        string Failure { get; set; }

        IReadOnlyList<IReadOnlyMappingItemNode> Brackets { get; }
        IReadOnlyList<IReadOnlyMappingItemNode> Customs { get; }

        /// <summary>
        /// キーが存在するか。
        /// </summary>
        bool HasKey { get; }

        #endregion
    }
}
