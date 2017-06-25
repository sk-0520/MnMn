using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyParameter
    {
        #region property

        public IReadOnlyList<IReadOnlyParameter> Parameters { get; }

        #endregion
    }
}
