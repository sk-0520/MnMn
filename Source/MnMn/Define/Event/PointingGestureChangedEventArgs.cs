using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Data;

namespace ContentTypeTextNet.MnMn.MnMn.Define.Event
{
    public class PointingGestureChangedEventArgs: EventArgs
    {
        #region define

        public static PointingGestureChangedEventArgs CancelEvent { get; } = new PointingGestureChangedEventArgs(PointingGestureChangeKind.Cancel, default(PointingGestureItem));

        #endregion

        public PointingGestureChangedEventArgs(PointingGestureChangeKind changeKind, PointingGestureItem item)
        {
            ChangeKind = changeKind;
            Item = item;
        }

        #region property

        public PointingGestureChangeKind ChangeKind { get; }

        public PointingGestureItem Item { get; }

        #endregion
    }
}
