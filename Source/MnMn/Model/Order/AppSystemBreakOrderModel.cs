using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Order
{
    /// <summary>
    /// スクリーンセーバー・ロック指示。
    /// <para>抑制には定期的に投げる運用。</para>
    /// </summary>
    public class AppSystemBreakOrderModel : OrderModel
    {
        public AppSystemBreakOrderModel(bool suppression)
            : base(OrderKind.SystemBreak, ServiceType.Application)
        {
            Suppression = suppression;
        }

        #region property

        /// <summary>
        /// スクリーンセーバー・ロックを抑制するか。
        /// <para>現運用では true のみ。</para>
        /// </summary>
        public bool Suppression {get;}

        #endregion
    }
}
