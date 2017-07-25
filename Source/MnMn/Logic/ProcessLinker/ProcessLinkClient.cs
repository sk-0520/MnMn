using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ProcessLink;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLink
{
    public class ProcessLinkClient : DisposeFinalizeBase
    {
        public ProcessLinkClient(Mediator mediator)
        {
            Mediator = mediator;
        }

        #region property

        Uri ServiceUri => Constants.AppServiceUri;
        Mediator Mediator { get; }

        #endregion

        #region property

        public void Execute(ServiceType serviceType, string key, string value)
        {
            var serviceUri = new Uri(ServiceUri, Constants.AppServiceProcessLinkEndpoint);
            var endpointAddr = new EndpointAddress(serviceUri);
            Binding binding;
            switch(serviceUri.Scheme) {
                case "net.pipe":
                    binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
                    break;

                default:
                    throw new NotImplementedException();
            }

            try {
                using(var channel = new ChannelFactory<IProcessLink>(binding, endpointAddr)) {
                    var service = channel.CreateChannel();

                    service.Execute(serviceType, key, value);

                    channel.Close();
                }
            } catch(Exception ex) {
                Mediator.Logger.Error(ex);
            }
        }

        #endregion
    }
}
