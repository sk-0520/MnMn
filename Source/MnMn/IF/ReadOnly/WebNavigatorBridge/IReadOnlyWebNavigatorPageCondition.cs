using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorPageCondition
    {
        #region property

        string UriPattern { get; }

        string ParameterSource { get; }

        #endregion
    }
}
