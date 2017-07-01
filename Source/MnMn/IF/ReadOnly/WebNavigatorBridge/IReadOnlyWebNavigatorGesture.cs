using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorGesture
    {
        #region property

        IReadOnlyList<IReadOnlyWebNavigatorGestureElement> Items { get; }

        #endregion
    }
}
