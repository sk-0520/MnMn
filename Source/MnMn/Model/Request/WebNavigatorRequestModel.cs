using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.WebNavigator;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request
{
    public class WebNavigatorRequestModel: RequestModel
    {
        public WebNavigatorRequestModel(RequestKind requestKind, ServiceType serviceType, WebNavigatorParameterModelBase parameter) 
            : base(requestKind, serviceType)
        {
            Parameter = parameter;
        }

        #region property

        public WebNavigatorParameterModelBase Parameter { get; }

        #endregion
    }
}
