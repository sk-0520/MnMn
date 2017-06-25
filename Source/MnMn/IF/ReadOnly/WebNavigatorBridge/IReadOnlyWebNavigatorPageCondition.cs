using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorPageCondition: IModel
    {
        #region property

        string UriPattern { get; }

        string ParameterSource { get; }

        #endregion
    }
}
