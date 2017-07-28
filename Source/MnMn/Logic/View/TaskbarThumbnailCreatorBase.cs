using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.View
{
    public abstract class TaskbarThumbnailCreatorBase<TElement> : DisposeFinalizeBase
        where TElement: FrameworkElement
    {
        public TaskbarThumbnailCreatorBase(TElement element, Action<Thickness> receiveMethod)
        {
            Element = element;
            ReceiveMethod = receiveMethod;

            AttacheEvent();
        }

        #region property

        protected TElement Element { get; private set; }
        Action<Thickness> ReceiveMethod { get; set; }

        #endregion

        #region function

        protected void AttacheEvent()
        {
            Element.SizeChanged += View_SizeChanged;
            Element.MouseLeave += View_MouseLeave;
            Element.MouseMove += View_MouseMove;
        }

        void DetachEvent()
        {
            Element.SizeChanged -= View_SizeChanged;
            Element.MouseLeave -= View_MouseLeave;
            Element.MouseMove -= View_MouseMove;
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
                if(Element != null) {
                    DetachEvent();
                    Element = null;
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
