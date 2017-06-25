using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorGestureElement: IReadOnlyWebNavigatorDefinedElement
    {
        #region property

        IReadOnlyList<PointingGestureDirection> Directions { get; }

        #endregion
    }
}
