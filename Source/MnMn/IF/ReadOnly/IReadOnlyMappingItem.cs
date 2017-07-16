using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyMappingItem: IReadOnlyKey
    {
        #region property

        MappingItemType Type { get; set; }

        string Name { get; set; }

        string Bond { get; set; }

        string Value { get; set; }

        string Failure { get; set; }

        IReadOnlyList<IReadOnlyMappingItemBracket> Brackets { get; }
        IReadOnlyList<IReadOnlyMappingItemReplace> Replace { get; }
        IReadOnlyList<IReadOnlyMappingItemCustom> Customs { get; }

        /// <summary>
        /// キーが存在するか。
        /// </summary>
        bool HasKey { get; }

        #endregion
    }
}
