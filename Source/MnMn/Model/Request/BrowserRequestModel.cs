using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.Browser;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request
{
    public class BrowserRequestModel: RequestModel
    {
        public BrowserRequestModel(RequestKind requestKind, ServiceType serviceType, BrowserParameterModelBase parameter) 
            : base(requestKind, serviceType)
        {
            Parameter = parameter;
        }

        #region property

        public BrowserParameterModelBase Parameter { get; }

        #endregion
    }
}
