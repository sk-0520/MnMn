using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.View
{
    public abstract class TaskbarThumbnailCreator<TView> : DisposeFinalizeBase
        where TView: FrameworkElement
    {
        public TaskbarThumbnailCreator(TView view, Action<Thickness> receiveMethod)
        {
            View = view;
            ReceiveMethod = receiveMethod;

            AttacheEvent();
        }

        #region property

        protected TView View { get; private set; }
        Action<Thickness> ReceiveMethod { get; set; }

        #endregion

        #region function

        protected void AttacheEvent()
        {
            View.SizeChanged += View_SizeChanged;
            View.MouseLeave += View_MouseLeave;
            View.MouseMove += View_MouseMove;
        }

        void DetachEvent()
        {
            View.SizeChanged -= View_SizeChanged;
            View.MouseLeave -= View_MouseLeave;
            View.MouseMove -= View_MouseMove;
        }

        protected abstract Thickness GetThickness();

        protected void ChangedThickness()
        {
            var thickness = GetThickness();
            ReceiveMethod(thickness);
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(View != null) {
                    DetachEvent();
                    View = null;
                }
                if(ReceiveMethod != null) {
                    ReceiveMethod = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void View_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangedThickness();
        }

        private void View_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ChangedThickness();
        }

        private void View_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ChangedThickness();
        }

    }
}
