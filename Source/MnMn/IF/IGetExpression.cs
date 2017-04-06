using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.IF
{
    public interface IGetExpression
    {
        IExpression GetExpression(string key, ServiceType serviceType);
        IExpression GetExpression(string key, string id, ServiceType serviceType);
    }
}
