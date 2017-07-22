using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.ProcessLink;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Order.AppProcessLink
{
    public class AppProcessLinkParameterOrderModel: AppProcessLinkOrderModelBase
    {
        public AppProcessLinkParameterOrderModel(IReadOnlyProcessLinkExecuteParameter parameter)
        {
            Parameter = parameter;
        }

        #region property

        public IReadOnlyProcessLinkExecuteParameter Parameter { get; }

        #endregion
    }
}
