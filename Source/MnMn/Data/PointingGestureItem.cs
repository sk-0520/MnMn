using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Data
{
    public struct PointingGestureItem
    {
        public PointingGestureItem(Point point, PointingGestureDirection direction)
        {
            Point = point;
            Direction = direction;
        }

        #region property

        public Point Point { get; }
        public PointingGestureDirection Direction { get; }

        #endregion

        #region Object

        public override string ToString()
        {
            return $"{Direction}, {(int)Point.X} x {(int)Point.Y}";
        }

        #endregion
    }
}
