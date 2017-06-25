using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge
{
    public interface IReadOnlyWebNavigatorBridge
    {
        #region property

        IReadOnlyWebNavigatorNavigating Navigating { get; }

        IReadOnlyWebNavigatorContextMenu ContextMenu { get; }

        IReadOnlyWebNavigatorGesture Gesture { get; }

        #endregion
    }
}
