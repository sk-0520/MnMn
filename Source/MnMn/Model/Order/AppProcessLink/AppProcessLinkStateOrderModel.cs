using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Order.AppProcessLink
{
    /// <summary>
    /// 接続可・不可状態の設定。
    /// </summary>
    public class AppProcessLinkStateOrderModel: AppProcessLinkOrderModelBase
    {
        public AppProcessLinkStateOrderModel(bool canConnect)
        {
            CanConnect = canConnect;
        }

        #region property

        public bool CanConnect { get; }

        #endregion
    }
}
