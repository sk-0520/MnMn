using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Order.AppProcessLink
{
    public abstract class AppProcessLinkOrderModelBase : OrderModel
    {
        public AppProcessLinkOrderModelBase()
            : base(OrderKind.ProcessLink, ServiceType.Application)
        { }

        #region property
        #endregion
    }
}
