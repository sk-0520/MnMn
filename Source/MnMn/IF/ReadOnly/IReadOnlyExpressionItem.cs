using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyExpressionItem
    {
        #region property

        string Id { get; }

        ExpressionItemKind Kind { get; }

        RegexOptions RegexOptions { get; }

        IReadOnlyCData Data { get; }

        #endregion
    }
}
