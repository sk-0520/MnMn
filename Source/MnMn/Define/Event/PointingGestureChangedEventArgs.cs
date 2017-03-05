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

        public static PointingGestureChangedEventArgs StartEvent { get; } = new PointingGestureChangedEventArgs(PointingGestureChangeKind.Start, null);
        public static PointingGestureChangedEventArgs CancelEvent { get; } = new PointingGestureChangedEventArgs(PointingGestureChangeKind.Cancel, null);

        #endregion

        public PointingGestureChangedEventArgs(PointingGestureChangeKind changeKind, IReadOnlyList<PointingGestureItem> items)
        {
            ChangeKind = changeKind;
            Items = items;
        }

        #region property

        public PointingGestureChangeKind ChangeKind { get; }

        public IReadOnlyList<PointingGestureItem> Items { get; }

        #endregion
    }
}
