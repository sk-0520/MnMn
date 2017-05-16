using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyNetworkSetting
    {
        #region property

        bool LogicUsingCustomUserAgent { get; }
        string LogicUserAgentFormat { get; }

        #endregion
    }
}
