using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using Gecko.WebIDL;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Order
{
    public class DownloadOrderModel: OrderModel
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="downloadItem"></param>
        /// <param name="canManagement">管理権限を譲渡するか。</param>
        /// <param name="serviceType"></param>
        public DownloadOrderModel(IDownloadItem downloadItem, bool canManagement, ServiceType serviceType)
            : base(OrderKind.Download, serviceType)
        {
            DownloadItem = downloadItem;
            CanManagement = canManagement;
        }

        #region property

        public IDownloadItem DownloadItem { get; }

        public bool CanManagement { get; }

        #endregion
    }
}
