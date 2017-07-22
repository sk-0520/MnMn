using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.ProcessLink;

namespace ContentTypeTextNet.MnMn.MnMn.IF
{
    public interface IProcessLinkChildHost
    {
        #region function

        ProcessLinkResultModel Execute(IReadOnlyProcessLinkExecuteParameter parameter);

        #endregion
    }
}
