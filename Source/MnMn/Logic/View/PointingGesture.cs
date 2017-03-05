using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Event;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.View
{
    public class PointingGesture
    {
        #region event

        public event EventHandler<PointingGestureChangedEventArgs> Changed;

        #endregion

        #region property

        public PointingGestureState State { get; set; }

        Size StartSize { get; set; } = new Size(10, 10);
        Size ActionSize { get; set; } = new Size(30, 30);

        Point PreparationPoint { get; set; }

        List<PointingGestureItem> Items { get; } = new List<PointingGestureItem>();

        #endregion

        #region function

        protected void OnChanged(PointingGestureChangeKind changeKind)
        {
            var changed = Changed;
            if(changed != null) {
                switch(changeKind) {
                    case PointingGestureChangeKind.Start:
                        Changed(this, PointingGestureChangedEventArgs.StartEvent);
                        break;

                    case PointingGestureChangeKind.Cancel:
                        Changed(this, PointingGestureChangedEventArgs.CancelEvent);
                        break;

                    case PointingGestureChangeKind.Add:
                    case PointingGestureChangeKind.Finish:
                        Changed(this, new PointingGestureChangedEventArgs(changeKind, Items));
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

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
                    OnChanged(PointingGestureChangeKind.Start);
                }
            } else {
                Debug.Assert(State == PointingGestureState.Action);
                var prev = Items.Last();
                var direction = GetDirection(prev.Point, point, ActionSize);
                if(prev.Direction != direction && direction != PointingGestureDirection.None) {
                    Items.Add(new PointingGestureItem(point, direction));
                    OnChanged(PointingGestureChangeKind.Add);
                }
            }
        }

        public void Finish()
        {
            if(State == PointingGestureState.Action) {
                OnChanged(PointingGestureChangeKind.Finish);
            }
            State = PointingGestureState.None;
        }

        public void Cancel()
        {
            OnChanged(PointingGestureChangeKind.Cancel);
            State = PointingGestureState.None;
        }

        #endregion
    }
}
