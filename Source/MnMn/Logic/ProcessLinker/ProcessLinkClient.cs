using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ProcessLink;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class ProcessLinkClient : DisposeFinalizeBase
    {
        public ProcessLinkClient(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        Uri ServiceUri => Constants.AppServiceUri;
        Mediation Mediation { get; }

        #endregion

        #region property

        public void Execute(ServiceType serviceType, string key, string value)
        {
            var uri = new Uri(ServiceUri, Constants.AppServiceProcessLinkEndpoint);
            var endpointAddr = new EndpointAddress(uri);
        }

        #endregion
    }
}
