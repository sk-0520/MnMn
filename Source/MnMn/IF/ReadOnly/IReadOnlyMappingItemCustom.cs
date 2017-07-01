using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyMappingItemCustom: IReadOnlyMappingItemNode
    {
        #region property

        string ReplaceFrom { get; }

        string ReplaceTo { get; }

        #endregion
    }
}
