using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
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
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using Gecko;

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

        #region IsVisibleHomeProperty

        public static readonly DependencyProperty IsVisibleHomeProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsVisibleHomeProperty)),
            typeof(bool),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsVisibleHomeChanged))
        );

        private static void OnIsVisibleHomeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.IsVisibleHome = (bool)e.NewValue;
            }
        }

        public bool IsVisibleHome
        {
            get { return (bool)GetValue(IsVisibleHomeProperty); }
            set { SetValue(IsVisibleHomeProperty, value); }
        }

        #endregion

        #region HomeSourceProperty

        public static readonly DependencyProperty HomeSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(HomeSourceProperty)),
            typeof(Uri),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(default(Uri), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnHomeSourceChanged))
        );

        private static void OnHomeSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.HomeSource = e.NewValue as Uri;
            }
        }

        public Uri HomeSource
        {
            get { return GetValue(HomeSourceProperty) as Uri; }
            set { SetValue(HomeSourceProperty, value); }
        }

        #endregion

        #region IsEnabledUserChangeSourceProperty

        public static readonly DependencyProperty IsEnabledUserChangeSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsEnabledUserChangeSourceProperty)),
            typeof(bool),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsEnabledUserChangeSourceChanged))
        );

        private static void OnIsEnabledUserChangeSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.IsEnabledUserChangeSource = (bool)e.NewValue;
            }
        }

        public bool IsEnabledUserChangeSource
        {
            get { return (bool)GetValue(IsEnabledUserChangeSourceProperty); }
            set { SetValue(IsEnabledUserChangeSourceProperty, value); }
        }

        #endregion

        #region property

        public Uri Source
        {
            get { return this.browser.Source; }
            set { this.browser.Source = value; }
        }

        public GeckoWebBrowser Gecko { get; private set; }

        #endregion

        #region command

        public ICommand HomeCommand
        {
            get
            {
                return new DelegateCommand(
                    o => { Navigate(HomeSource); },
                    o => IsVisibleHome && HomeSource != null
                );
            }
        }

        public ICommand BackCommand
        {
            get
            {
                return new DelegateCommand(
                    o => { this.browser.GoBack(); },
                    o => this.browser.CanGoBack
                );
            }
        }

        public ICommand ForwardCommand
        {
            get
            {
                return new DelegateCommand(
                    o => { this.browser.GoForward(); },
                    o => this.browser.CanGoForward
                );
            }
        }

        public ICommand ChangeSourceCommand
        {
            get
            {
                return new DelegateCommand(
                    o => {
                        Uri inputUri;
                        if(Uri.TryCreate(location.Text, UriKind.Absolute, out inputUri)) {
                            Navigate(location.Text);
                        }
                    },
                    o => IsEnabledUserChangeSource && !string.IsNullOrWhiteSpace(location.Text)
                );
            }
        }

        #endregion

        #region function

        #region wrapper

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
            Gecko.Navigate(source);
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
            Gecko.Navigate(source.OriginalString);
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
        [Obsolete]
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
        [Obsolete]
        public void Navigate(Uri source, string targetFrameName, byte[] postData, string additionalHeaders)
        {
            this.browser.Navigate(source, targetFrameName, postData, additionalHeaders);
        }

        //
        // 概要:
        //     ドキュメントのコンテンツを含む System.IO.Stream へ非同期に移動します。
        //
        // パラメーター:
        //   stream:
        //     ドキュメントのコンテンツを含む System.IO.Stream。
        //
        // 例外:
        //   T:System.ObjectDisposedException:
        //     The System.Windows.Controls.WebBrowser instance is no longer valid.
        //
        //   T:System.InvalidOperationException:
        //     A reference to the underlying native WebBrowser could not be retrieved.
        [Obsolete]
        public void NavigateToStream(Stream stream)
        {
            this.browser.NavigateToStream(stream);
        }
        //
        // 概要:
        //     ドキュメントのコンテンツを含む System.String へ非同期に移動します。
        //
        // パラメーター:
        //   text:
        //     ドキュメントのコンテンツを含む System.String。
        //
        // 例外:
        //   T:System.ObjectDisposedException:
        //     The System.Windows.Controls.WebBrowser instance is no longer valid.
        //
        //   T:System.InvalidOperationException:
        //     A reference to the underlying native WebBrowser could not be retrieved.
        public void NavigateToString(string text)
        {
            this.browser.NavigateToString(text);
            Gecko.LoadHtml(text);
        }
        //
        // 概要:
        //     現在のページを再読み込みします。
        //
        // 例外:
        //   T:System.ObjectDisposedException:
        //     The System.Windows.Controls.WebBrowser instance is no longer valid.
        //
        //   T:System.InvalidOperationException:
        //     A reference to the underlying native WebBrowser could not be retrieved.
        [SecurityCritical]
        public void Refresh()
        {
            this.browser.Refresh();
        }
        //
        // 概要:
        //     オプションのキャッシュ検証を使用して現在のページを再読み込みします。
        //
        // パラメーター:
        //   noCache:
        //     キャッシュ検証を行わずに最新の情報に更新するかどうかを指定します。
        //
        // 例外:
        //   T:System.ObjectDisposedException:
        //     The System.Windows.Controls.WebBrowser instance is no longer valid.
        //
        //   T:System.InvalidOperationException:
        //     A reference to the underlying native WebBrowser could not be retrieved.
        [SecurityCritical]
        public void Refresh(bool noCache)
        {
            this.browser.Refresh(noCache);
        }

        #endregion

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

        private void browser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            this.location.Text = e.Uri?.ToString() ?? string.Empty;
        }

        private void browser_Navigated(object sender, NavigationEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            Gecko = CustomBrowser.CreateBrowser();
            Debug.WriteLine($"Gecko.Created: {Gecko.Created}");
            Debug.WriteLine($"Gecko.IsHandleCreated: {Gecko.IsHandleCreated}");
            Gecko.CreateControl();
            Debug.WriteLine($"Gecko.Created: {Gecko.Created}");
            Debug.WriteLine($"Gecko.IsHandleCreated: {Gecko.IsHandleCreated}");

            browserHost.Child = Gecko;
        }
    }
}
