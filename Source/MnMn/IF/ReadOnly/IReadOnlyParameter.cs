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

        /// <summary>
        /// URIパラメータのキーと値。
        /// </summary>
        IReadOnlyList<IReadOnlyParameterItem> Items { get; }

        #endregion
    }
}
