using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request
{
    public class AppLogingProcessRequestModel: ProcessRequestModelBase<AppLoggingParameterModel>
    {
        public AppLogingProcessRequestModel(AppLoggingParameterModel parameter) 
            : base(ServiceType.Application, parameter)
        { }

        #region property
        #endregion
    }
}
