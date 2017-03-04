using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.View
{
    /// <summary>
    /// マウスジェスチャー処理。
    /// </summary>
    public class MouseGestureManager: DisposeFinalizeBase
    {
        public MouseGestureManager(FrameworkElement element)
        {
            Element = element;

            Element.PreviewMouseDown += Element_PreviewMouseDown;
        }

        #region property

        public FrameworkElement Element { get; private set; }

        public MouseButton TargetMouseButton { get; } = MouseButton.Right;

        #endregion

        #region function

        /// <summary>
        /// マウスジェスチャー準備。
        /// </summary>
        void StartGesturePreparation()
        {
            Debug.WriteLine("🐈");
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(Element != null) {
                Element.PreviewMouseDown -= Element_PreviewMouseDown;
            }

            Element = null;
            base.Dispose(disposing);
        }

        #endregion

        private void Element_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            switch(TargetMouseButton) {
                case MouseButton.Left:
                    if(e.LeftButton == MouseButtonState.Pressed) {
                        StartGesturePreparation();
                    }
                    break;

                case MouseButton.Middle:
                    if(e.MiddleButton == MouseButtonState.Pressed) {
                        StartGesturePreparation();
                    }
                    break;

                case MouseButton.Right:
                    if(e.RightButton == MouseButtonState.Pressed) {
                        StartGesturePreparation();
                    }
                    break;

                case MouseButton.XButton1:
                    if(e.XButton1 == MouseButtonState.Pressed) {
                        StartGesturePreparation();
                    }
                    break;

                case MouseButton.XButton2:
                    if(e.XButton2 == MouseButtonState.Pressed) {
                        StartGesturePreparation();
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

    }
}
