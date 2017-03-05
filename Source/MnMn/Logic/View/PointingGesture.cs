using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.View
{
    public class PointingGesture
    {
        #region property

        public PointingGestureState State { get; set; }

        Size StartSize { get; set; } = new Size(10, 10);
        Size ActionSize { get; set; } = new Size(10, 10);

        Point PreparationPoint { get; set; }

        List<PointingGestureItem> Items { get; } = new List<PointingGestureItem>();

        #endregion

        #region function

        PointingGestureDirection GetDirection(Point prev, Point now, Size size)
        {
            var diffX = now.X - prev.X;
            if(size.Width < Math.Abs(diffX)) {
                if(0 < diffX) {
                    return PointingGestureDirection.Right;
                } else {
                    return PointingGestureDirection.Left;
                }
            }

            var diffY = prev.Y - now.Y;
            if(size.Height < Math.Abs(diffY)) {
                if(0 < diffY) {
                    return PointingGestureDirection.Up;
                } else {
                    return PointingGestureDirection.Down;
                }
            }

            return PointingGestureDirection.None;
        }

        public void StartPreparation(Point point)
        {
            Items.Clear();
            State = PointingGestureState.Preparation;
            PreparationPoint = point;
            Debug.WriteLine(PreparationPoint);
        }

        public void Move(Point point)
        {
            if(State == PointingGestureState.Preparation) {
                var direction = GetDirection(PreparationPoint, point, StartSize);
                if(direction != PointingGestureDirection.None) {
                    State = PointingGestureState.Action;
                    Items.Add(new PointingGestureItem(point, direction));
                    Debug.WriteLine(Items.Last());
                }
            } else {
                Debug.Assert(State == PointingGestureState.Action);
                var prev = Items.Last();
                var direction = GetDirection(prev.Point, point, ActionSize);
                if(direction != PointingGestureDirection.None) {
                    Items.Add(new PointingGestureItem(point, direction));
                    Debug.WriteLine(Items.Last());
                }
            }
        }

        #endregion
    }
}
