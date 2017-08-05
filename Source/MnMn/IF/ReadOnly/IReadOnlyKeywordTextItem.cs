using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyKeywordTextItem
    {
        #region property

        string RawValue { get; }
        string Value { get; }
        string Tooltip { get; }

        #endregion
    }
}
