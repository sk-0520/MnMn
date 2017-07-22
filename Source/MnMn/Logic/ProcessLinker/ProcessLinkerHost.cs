using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ProcessLink;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLinker;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.Order.AppProcessLink;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProcessLinkerHost : DisposeFinalizeBase, IProcessLink
    {
        public ProcessLinkerHost(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; }

        ProcessLinkState State { get; set; }

        ServiceHost Service { get; set; }

        #endregion

        #region function

        void StartService()
        {
            var serviceUri = Constants.AppServiceUri;
            Service = new ServiceHost(this, serviceUri);

            Binding binding;
            switch(serviceUri.Scheme) {
                case "net.pipe":
                    binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
                    break;

                default:
                    throw new NotImplementedException();
            }

            Service.AddServiceEndpoint(typeof(IProcessLink), binding, Constants.AppServiceProcessLinkEndpoint);
            Service.Open();
        }

        void StopService()
        {
            Service.Close();
        }

        bool ChangeState(ProcessLinkState changeState)
        {
            if(State == changeState) {
                return false;
            }

            switch(changeState) {
                case ProcessLinkState.Shutdown: {
                        switch(State) {
                            case ProcessLinkState.Shutdown:
                                Mediation.Logger.Trace("skip: now shutdown");
                                return false;

                            case ProcessLinkState.Listening:
                            case ProcessLinkState.Pause:
                                StopService();
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                        State = changeState;
                        return true;
                    }

                case ProcessLinkState.Listening: {
                        switch(State) {
                            case ProcessLinkState.Shutdown:
                                StartService();
                                break;

                            case ProcessLinkState.Listening:
                                Mediation.Logger.Trace("skip: now listening");
                                return false;

                            case ProcessLinkState.Pause:
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                        State = changeState;
                        return true;
                    }

                case ProcessLinkState.Pause: {
                        switch(State) {
                            case ProcessLinkState.Shutdown:
                                Mediation.Logger.Warning("skip: now shutdown");
                                return false;

                            case ProcessLinkState.Listening:
                                State = ProcessLinkState.Pause;
                                return true;

                            case ProcessLinkState.Pause:
                                Mediation.Logger.Trace("skip: now pause");
                                return false;

                            default:
                                throw new NotImplementedException();
                        }
                    }
            }

            throw new NotImplementedException();
        }

        internal bool ReceiveOrder(AppProcessLinkOrderModelBase order)
        {
            var stateOrder = order as AppProcessLinkStateChangeOrderModel;
            if(stateOrder != null) {
                return ChangeState(stateOrder.ChangeState);
            }

            return false;
        }

        #endregion

        #region IProcessLink

        public IReadOnlyProcessLinkSession Connect(string clientName)
        {
            if(State != ProcessLinkState.Shutdown) {
                return null;
            }

            return new ProcessLinkSessionModel() {
                ClientName = clientName,
                ClientId = clientName, // TODO: 急ぎはしないけどまぁあれよね
            };
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Service != null) {
                    ((IDisposable)Service).Dispose();
                    Service = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
