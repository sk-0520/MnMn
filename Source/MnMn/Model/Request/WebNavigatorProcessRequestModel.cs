using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request
{
    public class WebNavigatorProcessRequestModel: ProcessRequestModelBase<WebNavigatorProcessParameterModel>
    {
        public WebNavigatorProcessRequestModel(ServiceType serviceType, WebNavigatorProcessParameterModel parameter) 
            : base(serviceType, parameter)
        {
        }
    }
}
