using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    interface IReadOnlyThemeDefine
    {
        #region property

        IReadOnlyList<IReadOnlyDefinedElement> ApplicationItems { get; }

        IReadOnlyList<IReadOnlyDefinedElement> BaseItems { get; }

        IReadOnlyList<IReadOnlyDefinedElement> AccentItems { get; }

        #endregion
    }
}
