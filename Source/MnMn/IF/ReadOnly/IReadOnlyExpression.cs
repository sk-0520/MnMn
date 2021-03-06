using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyExpression: IReadOnlyKey
    {
        #region property

        string Id { get; }

        ExpressionItemKind Kind { get; }

        string Word { get; }
        Regex Regex { get; }
        string XPath { get; }

        #endregion
    }
}
