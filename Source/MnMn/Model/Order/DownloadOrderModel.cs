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
        public DownloadOrderModel(IDownloadState downloadState, ServiceType serviceType) 
            : base(OrderKind.Donwload, serviceType)
        {
            DownloadState = downloadState;
        }

        #region property

        public IDownloadState DownloadState { get; }

        #endregion
    }
}
