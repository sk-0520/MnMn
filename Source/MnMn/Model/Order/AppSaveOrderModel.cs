using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Order
{
    public class AppSaveOrderModel: OrderModel
    {
        public AppSaveOrderModel(bool isBackup)
            : base(Define.OrderKind.Save, ContentTypeTextNet.MnMn.Library.Bridging.Define.ServiceType.Application)
        {
            IsBackup = isBackup;
        }

        #region property

        /// <summary>
        /// バックアップを必要とするか。
        /// </summary>
        public bool IsBackup { get; }

        #endregion
    }
}
