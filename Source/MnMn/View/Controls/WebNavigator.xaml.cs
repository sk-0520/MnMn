﻿using System;
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
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
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

        #region NewWindowCommand

        #region NewWindowCommandProperty

        public static readonly DependencyProperty NewWindowCommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(NewWindowCommandProperty)),
            typeof(ICommand),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnNewWindowCommandChanged))
        );

        private static void OnNewWindowCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.NewWindowCommand = e.NewValue as ICommand;
            }
        }

        public ICommand NewWindowCommand
        {
            get { return GetValue(NewWindowCommandProperty) as ICommand; }
            set { SetValue(NewWindowCommandProperty, value); }
        }

        #endregion

        #region NewWindowCommandParameterProperty

        public static readonly DependencyProperty NewWindowCommandParameterProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(NewWindowCommandParameterProperty)),
            typeof(object),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnNewWindowCommandParameterChanged))
        );

        private static void OnNewWindowCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.NewWindowCommandParameter = e.NewValue;
            }
        }

        public object NewWindowCommandParameter
        {
            get { return GetValue(NewWindowCommandParameterProperty); }
            set { SetValue(NewWindowCommandParameterProperty, value); }
        }

        #endregion

        #endregion

        #region SourceLoadedCommand

        #region SourceLoadedCommandProperty

        public static readonly DependencyProperty SourceLoadedCommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SourceLoadedCommandProperty)),
            typeof(ICommand),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSourceLoadedCommandChanged))
        );

        private static void OnSourceLoadedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.SourceLoadedCommand = e.NewValue as ICommand;
            }
        }

        public ICommand SourceLoadedCommand
        {
            get { return GetValue(SourceLoadedCommandProperty) as ICommand; }
            set { SetValue(SourceLoadedCommandProperty, value); }
        }

        #endregion

        #region SourceLoadedCommandParameterProperty

        public static readonly DependencyProperty SourceLoadedCommandParameterProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SourceLoadedCommandParameterProperty)),
            typeof(object),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSourceLoadedCommandParameterChanged))
        );

        private static void OnSourceLoadedCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.SourceLoadedCommandParameter = e.NewValue;
            }
        }

        public object SourceLoadedCommandParameter
        {
            get { return GetValue(SourceLoadedCommandParameterProperty); }
            set { SetValue(SourceLoadedCommandParameterProperty, value); }
        }

        #endregion

        #endregion

        #region DomLoadedCommand

        #region DomLoadedCommandProperty

        public static readonly DependencyProperty DomLoadedCommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(DomLoadedCommandProperty)),
            typeof(ICommand),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDomLoadedCommandChanged))
        );

        private static void OnDomLoadedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.DomLoadedCommand = e.NewValue as ICommand;
            }
        }

        public ICommand DomLoadedCommand
        {
            get { return GetValue(DomLoadedCommandProperty) as ICommand; }
            set { SetValue(DomLoadedCommandProperty, value); }
        }

        #endregion

        #region DomLoadedCommandParameterProperty

        public static readonly DependencyProperty DomLoadedCommandParameterProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(DomLoadedCommandParameterProperty)),
            typeof(object),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDomLoadedCommandParameterChanged))
        );

        private static void OnDomLoadedCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.DomLoadedCommandParameter = e.NewValue;
            }
        }

        public object DomLoadedCommandParameter
        {
            get { return GetValue(DomLoadedCommandParameterProperty); }
            set { SetValue(DomLoadedCommandParameterProperty, value); }
        }

        #endregion

        #endregion

        #region property

        /// <summary>
        /// 標準ブラウザ。
        /// </summary>
        WebBrowser BrowserDefault { get; set; }
        /// <summary>
        /// Gecko版。
        /// </summary>
        ServiceGeckoWebBrowser BrowserGeckoFx { get; set; }

        /// <summary>
        /// サービス種別。
        /// </summary>
        public ServiceType ServiceType { get; set; }

        /// <summary>
        /// 現在ページ。
        /// </summary>
        public Uri Source
        {
            get
            {
                return DoFunction(
                    b => b.Source,
                    b => b.Url
                );
            }
        }

        /// <summary>
        /// 何も読み込まれていない状態か。
        /// <para>初期化した後とか</para>
        /// </summary>
        public bool IsEmptyContent { get; private set; }

        /// <summary>
        /// 「戻る」は使用可能か。
        /// </summary>
        public bool CanGoBack
        {
            get
            {
                return DoFunction(
                    b => b.CanGoBack,
                    b => b.CanGoBack
                );
            }
        }

        /// <summary>
        /// 「進む」は使用可能か。
        /// </summary>
        public bool CanGoForward
        {
            get
            {
                return DoFunction(
                    b => b.CanGoForward,
                    b => b.CanGoForward
                );
            }
        }

        #endregion

        #region command

        public ICommand HomeCommand
        {
            get
            {
                return new DelegateCommand(
                    o => Navigate(HomeSource),
                    o => IsVisibleHome && HomeSource != null
                );
            }
        }

        public ICommand BackCommand
        {
            get
            {
                return new DelegateCommand(
                    o => GoBack(),
                    o => CanGoBack
                );
            }
        }

        public ICommand ForwardCommand
        {
            get
            {
                return new DelegateCommand(
                    o => GoForward(),
                    o => CanGoForward
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
                            Navigate(inputUri);
                        }
                    },
                    o => IsEnabledUserChangeSource && !string.IsNullOrWhiteSpace(location.Text)
                );
            }
        }

        #endregion

        #region function

        /// <summary>
        /// 戻り値を有する処理の実施。
        /// </summary>
        /// <typeparam name="TResult">戻り値</typeparam>
        /// <param name="defaultFunction">標準ブラウザ使用時の処理。</param>
        /// <param name="geckoFxFunction">Gecko版使用時の処理。</param>
        /// <returns></returns>
        TResult DoFunction<TResult>(Func<WebBrowser, TResult> defaultFunction, Func<ServiceGeckoWebBrowser, TResult> geckoFxFunction)
        {
            return WebNavigatorUtility.DoFunction(
                WebNavigatorCore.Engine,
                () => defaultFunction(BrowserDefault),
                () => geckoFxFunction(BrowserGeckoFx)
            );
        }

        /// <summary>
        /// 戻り値の無い処理の実施。
        /// </summary>
        /// <param name="defaultAction">標準ブラウザ使用時の処理。</param>
        /// <param name="geckoFxAction">Gecko版使用時の処理。</param>
        void DoAction(Action<WebBrowser> defaultAction, Action<GeckoWebBrowser> geckoFxAction)
        {
            WebNavigatorUtility.DoAction(
                WebNavigatorCore.Engine,
                () => defaultAction(BrowserDefault),
                () => geckoFxAction(BrowserGeckoFx)
            );
        }

        /// <summary>
        /// 戻る。
        /// </summary>
        public void GoBack()
        {
            DoAction(
                b => b.GoBack(),
                b => b.GoBack()
            );
        }

        /// <summary>
        /// 進む。
        /// </summary>
        public void GoForward()
        {
            DoAction(
                b => b.GoForward(),
                b => b.GoForward()
            );
        }

        /// <summary>
        /// 指定 URI に移動。
        /// </summary>
        /// <param name="uri"></param>
        public void Navigate(Uri uri)
        {
            DoAction(
                b => b.Navigate(uri),
                b => b.Navigate(uri.OriginalString)
            );
            IsEmptyContent = false;
        }

        public void NavigateEmpty()
        {
            var uri = new Uri("about:blank");
            Navigate(uri);
            DoAction(
                b => { },
                b => Dispatcher.Invoke(() => b.History.Clear())
            );
            IsEmptyContent = true;
        }

        /// <summary>
        /// HTMLを直接読み込み。
        /// </summary>
        /// <param name="htmlSource"></param>
        public void LoadHtml(string htmlSource)
        {
            DoAction(
                b => b.NavigateToString(htmlSource),
                b => b.LoadHtml(htmlSource, string.Empty)
            );
            IsEmptyContent = false;
        }

        [SecurityCritical]
        public void Refresh(bool noCache = false)
        {
            DoAction(
                b => b.Refresh(noCache),
                b => {
                    if(noCache) {
                        b.Reload(GeckoLoadFlags.IsRefresh);
                    } else {
                        b.Reload(GeckoLoadFlags.BypassCache);
                    }
                }
            );
        }

        void InitializedDefault()
        {
            BrowserDefault = new WebBrowser();

            BrowserDefault.Loaded += Browser_Loaded;
            BrowserDefault.Loaded += BrowserDefault_Loaded;
            BrowserDefault.Unloaded += BrowserDefault_Unloaded;
            BrowserDefault.Navigating += BrowserDefault_Navigating;
            BrowserDefault.Navigated += BrowserDefault_Navigated;
            BrowserDefault.LoadCompleted += BrowserDefault_LoadCompleted;
            // IE側は LoadCompleted だけ？
            //BrowserGeckoFx.DOMContentLoaded += BrowserGeckoFx_DOMContentLoaded;

            this.container.Content = BrowserDefault;
        }

        void InitializedGeckoFx()
        {
            BrowserGeckoFx = WebNavigatorCore.CreateBrowser();
            BrowserGeckoFx.CreateControl();

            BrowserGeckoFx.Disposed += BrowserGeckoFx_Disposed;
            BrowserGeckoFx.Navigating += BrowserGeckoFx_Navigating;
            BrowserGeckoFx.Navigated += BrowserGeckoFx_Navigated;
            BrowserGeckoFx.CreateWindow += BrowserGeckoFx_CreateWindow;
            BrowserGeckoFx.Load += BrowserGeckoFx_Load;
            BrowserGeckoFx.DOMContentLoaded += BrowserGeckoFx_DOMContentLoaded;

            var host = new WindowsFormsHost();
            using(Initializer.BeginInitialize(host)) {
                host.Child = BrowserGeckoFx;
                host.Loaded += Browser_Loaded;
            }

            this.container.Content = host;
        }

        #endregion

        #region UserControl

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            DoAction(
                b => InitializedDefault(),
                b => InitializedGeckoFx()
            );

            IsEmptyContent = true;
        }

        #endregion

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            element.Loaded -= Browser_Loaded;
            DoAction(
                b => b.Tag = ServiceType, // IEもうどうでもいいわ
                b => {
                    var type = b.GetType();
                    var property = type.GetProperty(nameof(ServiceGeckoWebBrowser.ServiceType));
                    property.SetValue(b, ServiceType);
                }
            );
        }

        private void BrowserDefault_Loaded(object sender, RoutedEventArgs e)
        {
            // http://stackoverflow.com/questions/6138199/wpf-webbrowser-control-how-to-supress-script-errors
            dynamic activeX = BrowserDefault.GetType().InvokeMember(
                "ActiveXInstance",
                BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                BrowserDefault,
                new object[] { }
            );
            if(activeX != null) {
                activeX.Silent = true;
                BrowserDefault.Loaded -= BrowserDefault_Loaded;
            }
        }

        private void BrowserDefault_Unloaded(object sender, RoutedEventArgs e)
        {
            BrowserDefault.Unloaded -= BrowserDefault_Unloaded;
            BrowserDefault.Loaded -= BrowserDefault_Loaded;
            BrowserDefault.Navigating -= BrowserDefault_Navigating;
            BrowserDefault.Navigated -= BrowserDefault_Navigated;
            BrowserDefault.LoadCompleted -= BrowserDefault_LoadCompleted;
        }

        private void BrowserGeckoFx_Disposed(object sender, EventArgs e)
        {
            BrowserGeckoFx.Disposed -= BrowserGeckoFx_Disposed;
            BrowserGeckoFx.Navigating -= BrowserGeckoFx_Navigating;
            BrowserGeckoFx.Navigated -= BrowserGeckoFx_Navigated;
            BrowserGeckoFx.CreateWindow -= BrowserGeckoFx_CreateWindow;
            BrowserGeckoFx.Load -= BrowserGeckoFx_Load;
            BrowserGeckoFx.DOMContentLoaded -= BrowserGeckoFx_DOMContentLoaded;
        }

        private void BrowserDefault_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            this.location.Text = e.Uri?.ToString() ?? string.Empty;
        }

        private void BrowserGeckoFx_Navigating(object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {
            this.location.Text = e.Uri?.ToString() ?? string.Empty;
        }


        private void BrowserDefault_Navigated(object sender, NavigationEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private void BrowserGeckoFx_Navigated(object sender, GeckoNavigatedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private void BrowserGeckoFx_CreateWindow(object sender, GeckoCreateWindowEventArgs e)
        {
            if(NewWindowCommand != null) {
                var eventData = WebNavigatorEventData.Create(WebNavigatorEngine.GeckoFx, sender, e, NewWindowCommandParameter);
                NewWindowCommand.TryExecute(eventData);
            }
        }

        private void BrowserDefault_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if(SourceLoadedCommand != null) {
                var eventData = WebNavigatorEventData.Create(WebNavigatorEngine.Default, sender, e, SourceLoadedCommandParameter);
                SourceLoadedCommand.TryExecute(eventData);
            }
        }

        private void BrowserGeckoFx_Load(object sender, DomEventArgs e)
        {
            if(SourceLoadedCommand != null) {
                var eventData = WebNavigatorEventData.Create(WebNavigatorEngine.GeckoFx, sender, e, SourceLoadedCommandParameter);
                SourceLoadedCommand.TryExecute(eventData);
            }
        }

        private void BrowserGeckoFx_DOMContentLoaded(object sender, DomEventArgs e)
        {
            if(DomLoadedCommand != null) {
                var eventData = WebNavigatorEventData.Create(WebNavigatorEngine.GeckoFx, sender, e, DomLoadedCommandParameter);
                DomLoadedCommand.TryExecute(eventData);
            }
        }


    }
}
