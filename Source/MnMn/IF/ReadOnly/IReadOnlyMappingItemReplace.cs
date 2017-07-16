using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyMappingItemReplace: IReadOnlyMappingItemNode
    {
        #region property

        RegexOptions RegexOptions { get; set; }

        IReadOnlyCData Pattern { get; }
        Regex Regex { get; }

        IReadOnlyList<IReadOnlyMappingItemReplacePair> Pairs { get; }

        #endregion
    }
}
