using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ProcessLink;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLink;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLinker;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.IdleTalk;
using ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLinker.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Order.AppProcessLink;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProcessLinkHost : DisposeFinalizeBase, IProcessLink
    {
        public ProcessLinkHost(Mediation mediation)
        {
            Mediation = mediation;

            Smile = new SmileProcessLinkChildHost(Mediation);
            IdleTalk = new IdleTalkProcessLinkChildHost(Mediation);
        }

        #region property

        Mediation Mediation { get; }

        ProcessLinkState State { get; set; }

        ServiceHost Service { get; set; }

        IProcessLinkChildHost Smile { get; }
        IProcessLinkChildHost IdleTalk { get; }

        #endregion

        #region function

        void StartService()
        {
            Mediation.Logger.Trace("[process-link] start");

            var serviceUri = Constants.AppServiceUri;
            Service = new ServiceHost(this, serviceUri);

            Binding mexBinding;
            Binding processLinkBinding;
            switch(serviceUri.Scheme) {
                case "net.pipe":
                    mexBinding = MetadataExchangeBindings.CreateMexNamedPipeBinding();
                    processLinkBinding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
                    break;

                default:
                    throw new NotImplementedException();
            }

            var serviceMetadata = Service.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if(serviceMetadata == null) {
                serviceMetadata = new ServiceMetadataBehavior();
                Service.Description.Behaviors.Add(serviceMetadata);
            }

            Service.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, mexBinding, "mex");
            Service.AddServiceEndpoint(typeof(IProcessLink), processLinkBinding, Constants.AppServiceProcessLinkEndpoint);

            Mediation.Logger.Trace($"[process-link] address: {Service.BaseAddresses.Count}", string.Join(Environment.NewLine, Service.BaseAddresses));
            Mediation.Logger.Trace($"[process-link] behavior: {Service.Description.Behaviors.Count}", string.Join(Environment.NewLine, Service.Description.Behaviors));
            Mediation.Logger.Trace($"[process-link] endpoint: {Service.Description.Endpoints.Count}", string.Join(Environment.NewLine, Service.Description.Endpoints.Select(i => i.ListenUri)));

            Mediation.Logger.Trace("[process-link] open");
            Service.Open();
        }

        void StopService()
        {
            Mediation.Logger.Trace("[process-link] closing");

            Service.Close();

            Mediation.Logger.Trace("[process-link] closed");
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

        ProcessLinkResultModel ExecuteCore(ServiceType serviceType, string key, string value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IProcessLink

        public ProcessLinkSessionModel Connect(string clientName)
        {
            if(State != ProcessLinkState.Listening) {
                return null;
            }

            return new ProcessLinkSessionModel() {
                ClientName = clientName,
                ClientId = clientName, // TODO: 急ぎはしないけどまぁあれよね
            };
        }

        public ProcessLinkResultModel Execute(ServiceType serviceType, string key, string value)
        {
            switch(serviceType) {
                case ServiceType.Application:
                    return ExecuteCore(serviceType, key, value);

                case ServiceType.Smile:
                case ServiceType.SmileLive:
                case ServiceType.SmileVideo:
                    return Smile.Execute(serviceType, key, value);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.Execute(serviceType, key, value);

                default:
                    throw new ArgumentException($"{nameof(serviceType)}: {serviceType}, {nameof(key)}: {key}, {nameof(value)}: {value}");
            }
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
