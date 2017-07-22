using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ProcessLink;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLinker;
using ContentTypeTextNet.MnMn.MnMn.Model.Order.AppProcessLink;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class ProcessLinkerHost : IProcessLink
    {
        public ProcessLinkerHost(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; }

        bool CanConnect { get; set; }

        #endregion

        #region function

        internal bool ReceiveOrder(AppProcessLinkOrderModelBase order)
        {
            var stateOrder = order as AppProcessLinkStateOrderModel;
            if(stateOrder != null) {
                CanConnect = stateOrder.CanConnect;
                return true;
            }

            return false;
        }

        #endregion

        #region IProcessLink

        public IReadOnlyProcessLinkSession Connect(string clientName)
        {
            if(!CanConnect) {
                return null;
            }

            return new ProcessLinkSessionModel() {
                ClientName = clientName,
                ClientId = clientName, // TODO: 急ぎはしないけどまぁあれよね
            };
        }

        #endregion
    }
}
