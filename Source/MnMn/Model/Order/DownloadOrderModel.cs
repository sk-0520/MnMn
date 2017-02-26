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
        /// <param name="downloadState"></param>
        /// <param name="canManagement">管理権限を譲渡するか。</param>
        /// <param name="serviceType"></param>
        public DownloadOrderModel(IDownloadState downloadState, bool canManagement, ServiceType serviceType) 
            : base(OrderKind.Donwload, serviceType)
        {
            DownloadState = downloadState;
            CanManagement = canManagement;
        }

        #region property

        public IDownloadState DownloadState { get; }

        public bool CanManagement { get; }

        #endregion
    }
}
