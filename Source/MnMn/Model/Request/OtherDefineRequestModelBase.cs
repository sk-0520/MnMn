using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request
{
    public abstract class OtherDefineRequestModelBase : RequestModel
    {
        public OtherDefineRequestModelBase(ServiceType serviceType)
            :base(RequestKind.OtherDefine, serviceType)
        { }
    }
}
