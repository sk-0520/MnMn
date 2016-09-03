﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// WebNavigator.xaml の相互作用ロジック
    /// </summary>
    public partial class WebNavigator: UserControl
    {
        public WebNavigator()
        {
            InitializeComponent();
        }

        #region IsVisibleToolbarProperty

        public static readonly DependencyProperty IsVisibleToolbarProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsVisibleToolbarProperty)),
            typeof(bool),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsVisibleToolbarChanged))
        );

        private static void OnIsVisibleToolbarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.IsVisibleToolbar = (bool)e.NewValue;
            }
        }

        public bool IsVisibleToolbar
        {
            get { return (bool)GetValue(IsVisibleToolbarProperty); }
            set { SetValue(IsVisibleToolbarProperty, value); }
        }

        #endregion

        #region property

        public Uri Source
        {
            get { return this.browser.Source; }
            set { this.browser.Source = value; }
        }

        public Uri HomeSource { get; set; }

        #endregion

        #region function

        //
        // 概要:
        //     指定された URL にあるドキュメントに非同期に移動します。
        //
        // パラメーター:
        //   source:
        //     移動先の URL。
        public void Navigate(string source)
        {
            this.browser.Navigate(source);
        }
        //
        // 概要:
        //     指定された System.Uri にあるドキュメントに非同期に移動します。
        //
        // パラメーター:
        //   source:
        //     移動先の System.Uri。
        //
        // 例外:
        //   T:System.ObjectDisposedException:
        //     The System.Windows.Controls.WebBrowser instance is no longer valid.
        //
        //   T:System.InvalidOperationException:
        //     A reference to the underlying native WebBrowser could not be retrieved.
        //
        //   T:System.Security.SecurityException:
        //     Navigation from an application that is running in partial trust to a System.Uri
        //     that is not located at the site of origin.
        public void Navigate(Uri source)
        {
            this.browser.Navigate(source);
        }

        //
        // 概要:
        //     指定された URL にあるドキュメントに非同期に移動し、ドキュメントのコンテンツを読み込むターゲット フレームを指定します。追加の HTTP POST データおよび
        //     HTTP ヘッダーを、ナビゲーション要求の一部としてサーバーに送信できます。
        //
        // パラメーター:
        //   source:
        //     移動先の URL。
        //
        //   targetFrameName:
        //     ドキュメントのコンテンツを表示するフレームの名前。
        //
        //   postData:
        //     ソースが要求されたときにサーバーに送信する HTTP POST データ。
        //
        //   additionalHeaders:
        //     ソースが要求されたときにサーバーに送信する HTTP ヘッダー。
        public void Navigate(string source, string targetFrameName, byte[] postData, string additionalHeaders)
        {
            this.browser.Navigate(source, targetFrameName, postData, additionalHeaders);
        }

        //
        // 概要:
        //     指定された System.Uri にあるドキュメントに非同期に移動し、ドキュメントのコンテンツを読み込むターゲット フレームを指定します。追加の HTTP
        //     POST データおよび HTTP ヘッダーを、ナビゲーション要求の一部としてサーバーに送信できます。
        //
        // パラメーター:
        //   source:
        //     移動先の System.Uri。
        //
        //   targetFrameName:
        //     ドキュメントのコンテンツを表示するフレームの名前。
        //
        //   postData:
        //     ソースが要求されたときにサーバーに送信する HTTP POST データ。
        //
        //   additionalHeaders:
        //     ソースが要求されたときにサーバーに送信する HTTP ヘッダー。
        //
        // 例外:
        //   T:System.ObjectDisposedException:
        //     The System.Windows.Controls.WebBrowser instance is no longer valid.
        //
        //   T:System.InvalidOperationException:
        //     A reference to the underlying native WebBrowser could not be retrieved.
        //
        //   T:System.Security.SecurityException:
        //     Navigation from an application that is running in partial trust:To a System.Uri
        //     that is not located at the site of origin, or targetFrameName name is not null
        //     or empty.
        public void Navigate(Uri source, string targetFrameName, byte[] postData, string additionalHeaders)
        {
            this.browser.Navigate(source, targetFrameName, postData, additionalHeaders);
        }

        #endregion

        #region UserControl

        #endregion

        private void browser_Loaded(object sender, RoutedEventArgs e)
        {
            // http://stackoverflow.com/questions/6138199/wpf-webbrowser-control-how-to-supress-script-errors
            dynamic activeX = this.browser.GetType().InvokeMember(
                "ActiveXInstance",
                BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                this.browser,
                new object[] { }
            );
            if(activeX != null) {
                activeX.Silent = true;
                this.browser.Loaded -= browser_Loaded;
            }
        }
    }
}
