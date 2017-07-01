using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.IF
{
    public interface IGetExpression
    {
        IReadOnlyExpression GetExpression(string key, ServiceType serviceType);
        IReadOnlyExpression GetExpression(string key, string id, ServiceType serviceType);
    }
}
