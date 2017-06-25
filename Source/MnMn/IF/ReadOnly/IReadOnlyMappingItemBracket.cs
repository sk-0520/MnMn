using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    interface IReadOnlyMappingItemBracket: IReadOnlyMappingItemNode
    {
        #region property

        string Open { get; }

        string Close { get; }

        #endregion
    }
}
