﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// SplashWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SplashWindow: Window
    {
        #region define

        //const int WM_SYSKEYDOWN = 0x0104;
        const int VK_F4 = 0x73;

        #endregion

        public SplashWindow()
        {
            InitializeComponent();

            if(VariableConstants.HasOptionExecuteMode) {
                if(VariableConstants.IsSafeModeExecute) {
                    this.textExecuteMode.Text = Properties.Resources.String_App_Splash_Execute_SafeMode;
                }
            } else {
                this.textExecuteMode.Visibility = Visibility.Collapsed;
            }
        }

        #region function

        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if((msg == (int)WM.WM_SYSKEYDOWN) && (wParam.ToInt32() == (int)VK.VK_F4)) {
                handled = true;
            }

            return IntPtr.Zero;
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source.AddHook(new HwndSourceHook(WndProc));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// MSDNコピペ！
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
    }
}
