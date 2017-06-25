using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyWebNavigatorTag
    {
        #region property

        ServiceType ServiceType { get; }
        Mediation Mediation { get; }

        #endregion
    }
}
