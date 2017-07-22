using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.ProcessLink
{
    public interface IReadOnlyProcessLinkExecuteParameter
    {
        #region property

        ServiceType ServiceType { get; }
        string Key { get; }
        string Value { get; }

        #endregion
    }
}
