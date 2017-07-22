using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Order.AppProcessLink
{
    /// <summary>
    /// 接続可・不可状態の設定。
    /// </summary>
    public class AppProcessLinkStateChangeOrderModel: AppProcessLinkOrderModelBase
    {
        public AppProcessLinkStateChangeOrderModel(ProcessLinkState state)
        {
            ChangeState = state;
        }

        #region property

        public ProcessLinkState ChangeState { get; }

        #endregion
    }
}
