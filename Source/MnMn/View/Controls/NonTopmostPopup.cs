using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// Popup with code to not be the topmost control
    /// <para>https://stackoverflow.com/a/18509629</para>
    /// </summary>
    public class NonTopmostPopup : Popup
    {
        /// <summary>
        /// Is Topmost dependency property
        /// </summary>
        public static readonly DependencyProperty IsTopmostProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsTopmostProperty)),
            typeof(bool),
            typeof(NonTopmostPopup),
            new FrameworkPropertyMetadata(false, OnIsTopmostChanged)
        );

        private bool? _appliedTopMost;
        private bool _alreadyLoaded;
        private Window _parentWindow;

        /// <summary>
        /// Get/Set IsTopmost
        /// </summary>
        public bool IsTopmost
        {
            get { return (bool)GetValue(IsTopmostProperty); }
            set { SetValue(IsTopmostProperty, value); }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public NonTopmostPopup()
        {
            Loaded += OnPopupLoaded;
            Unloaded += OnPopupUnloaded;
        }


        void OnPopupLoaded(object sender, RoutedEventArgs e)
        {
            if(this._alreadyLoaded) {
                return;
            }

            this._alreadyLoaded = true;

            if(Child != null) {
                Child.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(OnChildPreviewMouseLeftButtonDown), true);
            }

            this._parentWindow = Window.GetWindow(this);

            if(this._parentWindow == null) {
                return;
            }

            this._parentWindow.Activated += OnParentWindowActivated;
            this._parentWindow.Deactivated += OnParentWindowDeactivated;
        }

        private void OnPopupUnloaded(object sender, RoutedEventArgs e)
        {
            if(this._parentWindow == null) {
                return;
            }

            this._parentWindow.Activated -= OnParentWindowActivated;
            this._parentWindow.Deactivated -= OnParentWindowDeactivated;
        }

        void OnParentWindowActivated(object sender, EventArgs e)
        {
            Debug.WriteLine("Parent Window Activated");
            SetTopmostState(true);
        }

        void OnParentWindowDeactivated(object sender, EventArgs e)
        {
            Debug.WriteLine("Parent Window Deactivated");

            if(IsTopmost == false) {
                SetTopmostState(IsTopmost);
            }
        }

        void OnChildPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Child Mouse Left Button Down");

            SetTopmostState(true);

            if(!this._parentWindow.IsActive && IsTopmost == false) {
                this._parentWindow.Activate();
                Debug.WriteLine("Activating Parent from child Left Button Down");
            }
        }

        private static void OnIsTopmostChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var thisobj = (NonTopmostPopup)obj;

            thisobj.SetTopmostState(thisobj.IsTopmost);
        }

        protected override void OnOpened(EventArgs e)
        {
            SetTopmostState(IsTopmost);
            base.OnOpened(e);
        }

        private void SetTopmostState(bool isTop)
        {
            // Don’t apply state if it’s the same as incoming state
            if(this._appliedTopMost.HasValue && this._appliedTopMost == isTop) {
                return;
            }

            if(Child == null) {
                return;
            }

            var hwndSource = (PresentationSource.FromVisual(Child)) as HwndSource;

            if(hwndSource == null) {
                return;
            }

            var hwnd = hwndSource.Handle;

            RECT rect;

            if(!NativeMethods.GetWindowRect(hwnd, out rect)) {
                return;
            }

            Debug.WriteLine("setting z-order " + isTop);

            if(isTop) {
                NativeMethods.SetWindowPos(hwnd, HWND_TOPMOST, rect.Left, rect.Top, (int)Width, (int)Height, TOPMOST_FLAGS );
            } else {
                // Z-Order would only get refreshed/reflected if clicking the
                // the titlebar (as opposed to other parts of the external
                // window) unless I first set the popup to HWND_BOTTOM
                // then HWND_TOP before HWND_NOTOPMOST
                NativeMethods.SetWindowPos(hwnd, HWND_BOTTOM, rect.Left, rect.Top, (int)Width, (int)Height, TOPMOST_FLAGS);
                NativeMethods.SetWindowPos(hwnd, HWND_TOP, rect.Left, rect.Top, (int)Width, (int)Height, TOPMOST_FLAGS);
                NativeMethods.SetWindowPos(hwnd, HWND_NOTOPMOST, rect.Left, rect.Top, (int)Width, (int)Height, TOPMOST_FLAGS);
            }

            this._appliedTopMost = isTop;
        }

        #region P/Invoke imports & definitions

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        const SWP TOPMOST_FLAGS =
            SWP.SWP_NOACTIVATE | SWP.SWP_NOOWNERZORDER | SWP.SWP_NOSIZE | SWP.SWP_NOMOVE | SWP.SWP_NOREDRAW | SWP.SWP_NOSENDCHANGING;

        #endregion
    }
}
