using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ProcessLink;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model.ProcessLinker;
using ContentTypeTextNet.MnMn.MnMn.Define;
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

        ProcessLinkState State { get; set; }

        #endregion

        #region function

        void StartHost()
        {

        }

        void StopHost()
        {

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
                                StopHost();
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
                                StartHost();
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
    }
}
