using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    interface IReadOnlyParameters
    {
        #region property

        IReadOnlyList<IReadOnlyParameter> Parameters { get; }

        #endregion
    }
}
