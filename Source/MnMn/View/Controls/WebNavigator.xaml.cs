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
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Data.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.View;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.WebNavigator;
using ContentTypeTextNet.MnMn.MnMn.Model.Response;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge;
using Gecko;
using Newtonsoft.Json.Linq;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// WebNavigator.xaml の相互作用ロジック
    /// </summary>
    public partial class WebNavigator : UserControl
    {
        #region define

        //const int WM_RBUTTONDOWN = 0x0204;
        //const int WM_NCRBUTTONDOWN = 0x00A4;
        //const int WM_RBUTTONUP = 0x0205;
        //const int WM_XBUTTONUP = 0x020C;
        //const int MK_RBUTTON = 0x0002;
        //const int WM_MOUSEMOVE = 0x0200;
        //const int XBUTTON1 = 0x10000;
        //const int XBUTTON2 = 0x20000;

        readonly string[] ignoreGeckoFxLogs = Constants.WebNavigatorGeckoFxIgnoreEngineLogs;

        #endregion

        #region variable

        ICommand _copySelectionCommand;
        ICommand _selectAllCommand;

        ICommand _showSourceCommand;
        ICommand _showPropertyCommand;

        #endregion

        public WebNavigator()
        {
            InitializeComponent();

            PointingGesture.Changed += PointingGesture_Changed;
        }

        #region DependencyProperty

        #region BridgeClickProperty

        public static readonly DependencyProperty BridgeClickProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(BridgeClickProperty)),
            typeof(bool),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnBridgeClickChanged))
        );

        private static void OnBridgeClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.BridgeClick = (bool)e.NewValue;
            }
        }

        public bool BridgeClick
        {
            get { return (bool)GetValue(BridgeClickProperty); }
            set { SetValue(BridgeClickProperty, value); }
        }

        #endregion

        #region BridgeNavigatingProperty

        public static readonly DependencyProperty BridgeNavigatingProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(BridgeNavigatingProperty)),
            typeof(bool),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnBridgeNavigatingChanged))
        );

        private static void OnBridgeNavigatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.BridgeNavigating = (bool)e.NewValue;
            }
        }

        public bool BridgeNavigating
        {
            get { return (bool)GetValue(BridgeNavigatingProperty); }
            set { SetValue(BridgeNavigatingProperty, value); }
        }

        #endregion

        #region BridgeNewWindowProperty

        public static readonly DependencyProperty BridgeNewWindowProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(BridgeNewWindowProperty)),
            typeof(bool),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnBridgeNewWindowPropertyChanged))
        );

        private static void OnBridgeNewWindowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.BridgeNewWindow = (bool)e.NewValue;
            }
        }

        public bool BridgeNewWindow
        {
            get { return (bool)GetValue(BridgeNewWindowProperty); }
            set { SetValue(BridgeNewWindowProperty, value); }
        }

        #endregion

        #region BridgeContextMenuProperty

        public static readonly DependencyProperty BridgeContextMenuProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(BridgeContextMenuProperty)),
            typeof(bool),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnBridgeContextMenuChanged))
        );

        private static void OnBridgeContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.BridgeContextMenu = (bool)e.NewValue;
            }
        }

        public bool BridgeContextMenu
        {
            get { return (bool)GetValue(BridgeContextMenuProperty); }
            set { SetValue(BridgeContextMenuProperty, value); }
        }

        #endregion

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

        #region IsNavigatingProperty

        public static readonly DependencyProperty IsNavigatingProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsNavigatingProperty)),
            typeof(bool),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsNavigatingChanged))
        );

        private static void OnIsNavigatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.IsNavigating = (bool)e.NewValue;
            }
        }

        public bool IsNavigating
        {
            get { return (bool)GetValue(IsNavigatingProperty); }
            set { SetValue(IsNavigatingProperty, value); }
        }

        #endregion

        #region IsEnabledSystemBrowserProperty

        public static readonly DependencyProperty IsEnabledSystemBrowserProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsEnabledSystemBrowserProperty)),
            typeof(bool),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsEnabledSystemBrowserChanged))
        );

        private static void OnIsEnabledSystemBrowserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.IsEnabledSystemBrowser = (bool)e.NewValue;
            }
        }

        public bool IsEnabledSystemBrowser
        {
            get { return (bool)GetValue(IsEnabledSystemBrowserProperty); }
            set { SetValue(IsEnabledSystemBrowserProperty, value); }
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

        #region IsEnabledGestureProperty

        public static readonly DependencyProperty IsEnabledGestureProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsEnabledGestureProperty)),
            typeof(bool),
            typeof(WebNavigator),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsEnabledGestureChanged))
        );

        private static void OnIsEnabledGestureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WebNavigator;
            if(control != null) {
                control.IsEnabledGesture = (bool)e.NewValue;
            }
        }

        public bool IsEnabledGesture
        {
            get { return (bool)GetValue(IsEnabledGestureProperty); }
            set { SetValue(IsEnabledGestureProperty, value); }
        }

        #endregion

        public bool IsMinimumRunning
        {
            get
            {
                return WebNavigatorCore.ForceDefaultEngine;
            }
        }

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

        WebNavigatorPointingGesture PointingGesture { get; } = new WebNavigatorPointingGesture();

        /// <summary>
        /// サービス種別。
        /// </summary>
        public ServiceType ServiceType { get; set; }

        public CollectionModel<PointingGestureItem> GestureItems { get; } = new CollectionModel<PointingGestureItem>();
        IReadOnlyList<WebNavigatorGestureElementModel> GestureDefineElements { get; set; }
        IReadOnlyDictionary<string, ICommand> GestureCommands { get; set; }

        Mediation Mediation
        {
            get
            {
                return DoFunction(
                    b => ((WebNavigatorTagModel)b.Tag).Mediation,
                    b => b.Mediation
                );
            }
        }

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

        //public ImageSource DefaultBrowserIcon
        //{
        //    get { return WebNavigatorCore.DefaultBrowserIcon; }
        //}

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

        public ICommand ReloadDocumentCommand
        {
            get
            {
                return new DelegateCommand(
                    o => Refresh(),
                    o => !IsNavigating
                );
            }
        }

        public ICommand StopDocumentCommand
        {
            get
            {
                return new DelegateCommand(
                    o => Stop(),
                    o => IsNavigating
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
                        if(Uri.TryCreate(this.location.Text, UriKind.Absolute, out inputUri)) {
                            Navigate(inputUri);
                        }
                    },
                    o => IsEnabledUserChangeSource && !string.IsNullOrWhiteSpace(this.location.Text)
                );
            }
        }

        public ICommand OpenDefaultBrowserCommand
        {
            get
            {
                return new DelegateCommand(
                    o => OpenDefaultBrowser(Source),
                    o => Source != null && WebNavigatorCore.DefaultBrowserExecuteData.HasValue
                );
            }
        }

        ICommand CopySelectionCommand
        {
            get
            {
                if(this._copySelectionCommand == null) {
                    this._copySelectionCommand = new DelegateCommand(
                        o => DoAction(
                            b => { /* not impl */ },
                            b => b.CopySelection()
                        ),
                        o => DoFunction(
                            b => true,
                            b => b.CanCopySelection
                        )
                    );
                }
                return this._copySelectionCommand;
            }
        }

        ICommand SelectAllCommand
        {
            get
            {
                if(this._selectAllCommand == null) {
                    this._selectAllCommand = new DelegateCommand(o => DoAction(
                        b => { /* not impl */ },
                        b => b.SelectAll()
                    ));
                }
                return this._selectAllCommand;
            }
        }

        ICommand ShowSourceCommand
        {
            get
            {
                if(this._showSourceCommand == null) {
                    this._showSourceCommand = new DelegateCommand(o => DoAction(
                        b => { /* not impl */ },
                        b => b.ViewSource()
                    ));
                }

                return this._showSourceCommand;
            }
        }

        ICommand ShowPropertyCommand
        {
            get
            {
                if(this._showPropertyCommand == null) {
                    this._showPropertyCommand = new DelegateCommand(o => DoAction(
                        b => { /* not impl */ },
                        b => b.ShowPageProperties()
                    ));
                }

                return this._showPropertyCommand;
            }
        }

        public ICommand OpenIssue551Command
        {
            get
            {
                return new DelegateCommand(o => {
                    ShellUtility.OpenUriInDefaultBrowser("https://bitbucket.org/sk_0520/mnmn/issues/551", Mediation.Logger);
                });
            }
        }

        public ICommand OpenIssue560Command
        {
            get
            {
                return new DelegateCommand(o => {
                    ShellUtility.OpenUriInDefaultBrowser("https://bitbucket.org/sk_0520/mnmn/issues/560", Mediation.Logger);
                });
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
        void DoAction(Action<WebBrowser> defaultAction, Action<ServiceGeckoWebBrowser> geckoFxAction)
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

            this.container.Dispatcher.Invoke(() => {
                this.container.Focus();
            }, DispatcherPriority.SystemIdle);
        }

        public void NavigateEmpty()
        {
            var uri = new Uri("about:blank");
            Navigate(uri);
            DoAction(
                b => { },
                b => Dispatcher.Invoke(() => {
                    if(b.History.Count > 0) {
                        b.History.Clear();
                    }
                })
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
                b => b.LoadHtml(htmlSource, null)
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

        public void Stop()
        {
            DoAction(
                b => b.InvokeScript("eval", "document.execCommand('Stop');"),
                b => b.Stop()
            );
            IsNavigating = false;
        }

        void InitializedDefault()
        {
            BrowserDefault = WebNavigatorCore.CreateDefaultBrowser();

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
            BrowserGeckoFx = WebNavigatorCore.CreateGeckoBrowser();
            BrowserGeckoFx.FrameEventsPropagateToMainWindow = true;
            BrowserGeckoFx.CreateControl();

            BrowserGeckoFx.SetNavigator(this);

            BrowserGeckoFx.NoDefaultContextMenu = true;

            BrowserGeckoFx.Disposed += BrowserGeckoFx_Disposed;
            BrowserGeckoFx.Navigating += BrowserGeckoFx_Navigating;
            BrowserGeckoFx.Navigated += BrowserGeckoFx_Navigated;
            BrowserGeckoFx.CreateWindow += BrowserGeckoFx_CreateWindow;
            BrowserGeckoFx.Load += BrowserGeckoFx_Load;
            BrowserGeckoFx.DOMContentLoaded += BrowserGeckoFx_DOMContentLoaded;
            BrowserGeckoFx.DomClick += BrowserGeckoFx_DomClick;
            BrowserGeckoFx.DomContextMenu += BrowserGeckoFx_DomContextMenu;
            BrowserGeckoFx.DomMouseDown += BrowserGeckoFx_DomMouseDown;
            BrowserGeckoFx.DomMouseMove += BrowserGeckoFx_DomMouseMove;
            BrowserGeckoFx.DomMouseUp += BrowserGeckoFx_DomMouseUp;

            if(Constants.WebNavigatorGeckoFxShowLog) {
                BrowserGeckoFx.ConsoleMessage += BrowserGeckoFx_ConsoleMessage;
            }

            var host = new WindowsFormsHost();
            using(Initializer.BeginInitialize(host)) {
                host.Child = BrowserGeckoFx;
                host.Loaded += Browser_Loaded;
            }

            this.container.Content = host;
        }


        void OpenDefaultBrowser(Uri uri)
        {
            var logger = new Logger();
            ShellUtility.OpenUriInDefaultBrowser(uri, logger);
        }

        void SetClickParameterGeckoFx(WebNavigatorClickParameterModel model, DomMouseEventArgs e)
        {
            switch(e.Button) {
                case GeckoMouseButton.Left:
                    model.MouseButton = MouseButton.Left;
                    break;

                case GeckoMouseButton.Middle:
                    model.MouseButton = MouseButton.Middle;
                    break;

                case GeckoMouseButton.Right:
                    model.MouseButton = MouseButton.Right;
                    break;

                default:
                    throw new NotImplementedException();
            }

            var element = e.Target.CastToGeckoElement();

            model.Element = WebNavigatorCore.ConvertSimleHtmlElementGeckoFx(element);
            var rootElements = WebNavigatorCore.GetRootElementsGeckoFx(element);
            model.RootNodes = rootElements
                .Select(elm => WebNavigatorCore.ConvertSimleHtmlElementGeckoFx(elm))
                .ToList()
            ;
        }

        Separator MakeContextMenuItemSeparator(WebNavigatorContextMenuItemViewModel contextMenuItem)
        {
            var result = new Separator() {
                DataContext = contextMenuItem,
            };

            return result;
        }

        MenuItem MakeContextMenuItemCore(WebNavigatorContextMenuItemViewModel contextMenuItem)
        {
            var result = new MenuItem() {
                DataContext = contextMenuItem,

                Header = contextMenuItem.DisplayText,
            };
            result.Command = new DelegateCommand(
                o => DoContextMenuItemCommand(result, o),
                o => CanDoContextMenuItemCommand(result, o)
            );

            return result;
        }

        Control MakeContextMenuItem(WebNavigatorContextMenuItemViewModel contextMenuItem)
        {
            if(contextMenuItem.IsSeparator) {
                return MakeContextMenuItemSeparator(contextMenuItem);
            } else {
                return MakeContextMenuItemCore(contextMenuItem);
            }
        }

        ICommand GetCommonCommand(string key)
        {
            switch(key) {
                case WebNavigatorContextMenuKey.commonBack:
                    return BackCommand;

                case WebNavigatorContextMenuKey.commonForward:
                    return ForwardCommand;

                case WebNavigatorContextMenuKey.commonCopySelection:
                    return CopySelectionCommand;

                case WebNavigatorContextMenuKey.commonSelerctAll:
                    return SelectAllCommand;

                case WebNavigatorContextMenuKey.commonSource:
                    return ShowSourceCommand;

                case WebNavigatorContextMenuKey.commonProperty:
                    return ShowPropertyCommand;

                default:
                    return null;
            }
        }

        void ExecuteCommonCommand(WebNavigatorContextMenuItemViewModel contextMenuItem, object parameter)
        {
            Debug.Assert(contextMenuItem.SendService == ServiceType.Common);

            var command = GetCommonCommand(contextMenuItem.Key);
            if(command != null) {
                command.TryExecute(parameter);
            }
        }

        void DoContextMenuItemCommand(MenuItem menuItem, object parameter)
        {
            var contextMenuItem = (WebNavigatorContextMenuItemViewModel)menuItem.DataContext;
            if(contextMenuItem.SendService == ServiceType.Common) {
                ExecuteCommonCommand(contextMenuItem, parameter);
            } else {
                var processParameter = new WebNavigatorProcessParameterModel() {
                    ParameterVaule = (string)parameter,
                    Key = ((WebNavigatorContextMenuItemViewModel)menuItem.DataContext).Key,
                };

                var processRequest = new WebNavigatorProcessRequestModel(contextMenuItem.SendService, processParameter);
                DoAction(
                    b => { ((WebNavigatorTagModel)b.Tag).Mediation.Request(processRequest); },
                    b => { (b.Mediation).Request(processRequest); }
                );
            }
        }

        bool CanDoContextMenuItemCommand(MenuItem menuItem, object parameter)
        {
            var contextMenuItem = (WebNavigatorContextMenuItemViewModel)menuItem.DataContext;
            if(contextMenuItem.SendService == ServiceType.Common) {
                // 共通処理は特殊
                var command = GetCommonCommand(contextMenuItem.Key);
                if(command != null) {
                    return command.CanExecute(parameter);
                } else {
                    return false;
                }
            }

            return true;
        }

        internal bool PreProcessMessage(IWindowMessage windowMessage, ref System.Windows.Forms.Message msg, ref bool handled)
        {
            if(msg.Msg == (int)WM.WM_XBUTTONUP) {
                //var wParam = msg.WParam.ToInt32();
                var hiWord = WindowsUtility.HIWORD(msg.WParam);

                if(hiWord == (int)XBUTTON.XBUTTON1 && CanGoBack) {
                    GoBack();
                    handled = true;
                    return true;
                } else if(hiWord == (int)XBUTTON.XBUTTON2 && CanGoForward) {
                    GoForward();
                    handled = true;
                    return true;
                }
            }

            return false;
        }

        CheckResultModel<ICommand> GetGestureCommand(Mediation mediation, IEnumerable<PointingGestureItem> items)
        {
            if(GestureDefineElements == null) {
                var parameter = new WebNavigatorParameterModel(null, EventArgs.Empty, WebNavigatorCore.Engine, WebNavigatorParameterKind.Gesture);
                var result = Mediation.GetResultFromRequest<WebNavigatorGestureResultModel>(new WebNavigatorRequestModel(RequestKind.WebNavigator, ServiceType, parameter));
                GestureDefineElements = result.GestureItems;
                GestureCommands = GestureDefineElements.ToDictionary(
                    ik => ik.Key,
                    iv => GetCommonCommand(iv.Key)
                );
            }

            var gestureItems = GestureDefineElements.Join(
                GestureCommands,
                d => d.Key,
                c => c.Key,
                (d, c) => new {
                    Text = d.DisplayText,
                    Directions = d.Directions,
                    Command = c.Value
                }
            );
            var target = gestureItems.FirstOrDefault(gi => gi.Directions.SequenceEqual(items.Select(i => i.Direction)));

            if(target != null) {
                return new CheckResultModel<ICommand>(true, target.Command, null, target.Text);
            } else {
                return CheckResultModel.Failure<ICommand>(Properties.Resources.String_App_Unknown_Gesture);
            }
        }

        public void SetScale(double scale)
        {
            DoAction(
                b => { /* not impl*/ },
                b => {
                    b.GetDocShellAttribute()
                        .GetContentViewerAttribute()
                        .SetFullZoomAttribute((float)scale);
                    ;
                }
            );
        }

        void SetLinkState(string showText)
        {
            if(this.showLinkState.Text != showText) {
                this.showLinkState.Text = showText;
            }

            var openPopup = !string.IsNullOrWhiteSpace(this.showLinkState.Text);
            this.popupLinkState.IsOpen = openPopup;
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
                b => ((WebNavigatorTagModel)b.Tag).ServiceType = ServiceType, // IEもうどうでもいいわ
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
            PointingGesture.Changed -= PointingGesture_Changed;

            BrowserDefault.Unloaded -= BrowserDefault_Unloaded;
            BrowserDefault.Loaded -= BrowserDefault_Loaded;
            BrowserDefault.Navigating -= BrowserDefault_Navigating;
            BrowserDefault.Navigated -= BrowserDefault_Navigated;
            BrowserDefault.LoadCompleted -= BrowserDefault_LoadCompleted;
        }

        private void BrowserGeckoFx_Disposed(object sender, EventArgs e)
        {
            PointingGesture.Changed -= PointingGesture_Changed;

            BrowserGeckoFx.Disposed -= BrowserGeckoFx_Disposed;
            BrowserGeckoFx.Navigating -= BrowserGeckoFx_Navigating;
            BrowserGeckoFx.Navigated -= BrowserGeckoFx_Navigated;
            BrowserGeckoFx.CreateWindow -= BrowserGeckoFx_CreateWindow;
            BrowserGeckoFx.Load -= BrowserGeckoFx_Load;
            BrowserGeckoFx.DOMContentLoaded -= BrowserGeckoFx_DOMContentLoaded;
            BrowserGeckoFx.DomClick -= BrowserGeckoFx_DomClick;
            BrowserGeckoFx.DomContextMenu -= BrowserGeckoFx_DomContextMenu;
            BrowserGeckoFx.ConsoleMessage -= BrowserGeckoFx_ConsoleMessage;
        }

        private void BrowserDefault_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SetLinkState(null);

            this.location.Text = e.Uri?.ToString() ?? string.Empty;
            IsNavigating = true;
        }

        private void BrowserGeckoFx_Navigating(object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {
            SetLinkState(null);

            if(!BridgeNavigating) {
                return;
            }

            var parameter = new WebNavigatorNavigatingParameterModel(Source, e, WebNavigatorEngine.GeckoFx) {
                NextUri = e.Uri,
            };

            var result = BrowserGeckoFx.Mediation.GetResultFromRequest<WebNavigatorResultModel>(new WebNavigatorRequestModel(RequestKind.WebNavigator, ServiceType, parameter));

            e.Cancel = result.Cancel;

            if(!e.Cancel) {
                this.location.Text = e.Uri?.ToString() ?? string.Empty;
                IsNavigating = true;
            } else {
                var navigatingResult = (WebNavigatorNavigatingResultModel)result;
                var processParameter = new WebNavigatorProcessParameterModel() {
                    Key = navigatingResult.NavigatingItem.Key,
                    ParameterVaule = navigatingResult.Parameter,
                };
                var processRequest = new WebNavigatorProcessRequestModel(navigatingResult.NavigatingItem.SendService, processParameter);
                var processResult = BrowserGeckoFx.Mediation.Request(new WebNavigatorProcessRequestModel(navigatingResult.NavigatingItem.SendService, processParameter));
            }
        }

        private void BrowserDefault_Navigated(object sender, NavigationEventArgs e)
        {
            IsNavigating = false;
            CommandManager.InvalidateRequerySuggested();
        }

        private void BrowserGeckoFx_Navigated(object sender, GeckoNavigatedEventArgs e)
        {
            IsNavigating = false;
            CommandManager.InvalidateRequerySuggested();
        }

        private void BrowserGeckoFx_CreateWindow(object sender, GeckoCreateWindowEventArgs e)
        {
            this.popupLinkState.IsOpen = false;

            if(BridgeNewWindow) {
                // 先に内部制御を試す
                Uri nextUri;
                if(Uri.TryCreate(e.Uri, UriKind.RelativeOrAbsolute, out nextUri)) {
                    var parameter = new WebNavigatorNavigatingParameterModel(Source, e, WebNavigatorEngine.GeckoFx) {
                        NextUri = nextUri,
                    };
                    var result = BrowserGeckoFx.Mediation.GetResultFromRequest<WebNavigatorResultModel>(new WebNavigatorRequestModel(RequestKind.WebNavigator, ServiceType, parameter));

                    e.Cancel = result.Cancel;
                    if(e.Cancel) {
                        var navigatingResult = (WebNavigatorNavigatingResultModel)result;
                        var processParameter = new WebNavigatorProcessParameterModel() {
                            Key = navigatingResult.NavigatingItem.Key,
                            ParameterVaule = navigatingResult.Parameter,
                        };
                        var processRequest = new WebNavigatorProcessRequestModel(navigatingResult.NavigatingItem.SendService, processParameter);
                        var processResult = BrowserGeckoFx.Mediation.Request(new WebNavigatorProcessRequestModel(navigatingResult.NavigatingItem.SendService, processParameter));
                        return;
                    }
                }
            }

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

        private void BrowserGeckoFx_DomClick(object sender, DomMouseEventArgs e)
        {
            var parameter = new WebNavigatorClickParameterModel(Source, e, WebNavigatorEngine.GeckoFx);
            SetClickParameterGeckoFx(parameter, e);

            var result = BrowserGeckoFx.Mediation.GetResultFromRequest<WebNavigatorResultModel>(new WebNavigatorRequestModel(RequestKind.WebNavigator, ServiceType, parameter));

            if(e.Cancelable) {
                e.Handled = result.Cancel;
            }
        }

        private void BrowserGeckoFx_DomContextMenu(object sender, DomMouseEventArgs e)
        {
            if(!BridgeContextMenu) {
                if(e.Cancelable) {
                    e.Handled = true;
                }

                return;
            }

            //if(this.popupGesture.IsOpen) {
            //    return;
            //}
            if(PointingGesture.State != PointingGestureState.None || PointingGesture.GeckoFxSuppressionContextMenu) {
                if(e.Cancelable) {
                    e.Handled = true;
                }
                PointingGesture.Cancel();
                return;
            }


            if(ContextMenu.ItemsSource == null) {
                //var menuItems = CreateContextMenu();
                var contextMenuDefineParameter = new WebNavigatorParameterModel(null, null, WebNavigatorEngine.GeckoFx, WebNavigatorParameterKind.ContextMenuDefine);
                var contextMenuDefineResult = BrowserGeckoFx.Mediation.GetResultFromRequest<WebNavigatorContextMenuDefineResultModel>(new WebNavigatorRequestModel(RequestKind.WebNavigator, ServiceType, contextMenuDefineParameter));
                //ContextMenu.ItemsSource = menuItems.Select(MakeContextMenuItem).ToList();
                ContextMenu.ItemsSource = contextMenuDefineResult.Items.Select(MakeContextMenuItem).ToList();
            }

            var contextMenuParameter = new WebNavigatorContextMenuParameterModel(Source, e, WebNavigatorEngine.GeckoFx);
            SetClickParameterGeckoFx(contextMenuParameter, e);

            var contextMenuResult = BrowserGeckoFx.Mediation.GetResultFromRequest<WebNavigatorResultModel>(new WebNavigatorRequestModel(RequestKind.WebNavigator, ServiceType, contextMenuParameter));
            if(e.Cancelable) {
                e.Handled = contextMenuResult.Cancel;
            }

            if(!e.Handled) {
                var contextMenuItemParameter = new WebNavigatorContextMenuItemParameterModel(Source, e, WebNavigatorEngine.GeckoFx);
                SetClickParameterGeckoFx(contextMenuItemParameter, e);

                var menuItems = ContextMenu.Items
                    .Cast<Control>()
                    .Select(c => new { View = c, Define = (WebNavigatorContextMenuItemViewModel)c.DataContext })
                    .ToList()
                ;
                foreach(var menuItem in menuItems) {
                    contextMenuItemParameter.Key = menuItem.Define.Key;
                    var contextMenuItemResult = BrowserGeckoFx.Mediation.GetResultFromRequest<WebNavigatorContextMenuItemResultModel>(new WebNavigatorRequestModel(RequestKind.WebNavigator, ServiceType, contextMenuItemParameter));

                    if(contextMenuItemResult.Cancel) {
                        // 基本的にここでキャンセルは通さないけど一応例外投げておく(必要になったら対応する)
                        throw new NotImplementedException();
                    }

                    menuItem.View.IsEnabled = contextMenuItemResult.IsEnabled;
                    menuItem.View.Visibility = contextMenuItemResult.IsVisible ? Visibility.Visible : Visibility.Collapsed;
                    if(!menuItem.Define.IsSeparator) {
                        var viewMenuItem = (MenuItem)menuItem.View;
                        viewMenuItem.CommandParameter = contextMenuItemResult.Parameter;
                    }
                }

                // #418
                if(Constants.WebNavigatorContextMenuShowHtmlSource) {
                    var showSourceMenuItem = menuItems.LastOrDefault(i => i.Define.Key == WebNavigatorContextMenuKey.commonSource);
                    if(showSourceMenuItem == null) {
                        var sourceViewModel = new WebNavigatorContextMenuItemViewModel(new WebNavigatorContextMenuItemModel() {
                            AllowService = ServiceType.All,
                            SendService = ServiceType.Common,
                            Key = WebNavigatorContextMenuKey.commonSource,
                            _Words = {
                                //TODO: ローカライズ
                                new DefinedKeyValuePairModel() {
                                    Key = "js-jp",
                                    Value = "source",
                                },
                            },
                            IsSeparator = false,
                        });
                        var menuItemControl = MakeContextMenuItem(sourceViewModel);
                        ((List<Control>)ContextMenu.ItemsSource).Add(menuItemControl);
                        showSourceMenuItem = new { View = menuItemControl, Define = (WebNavigatorContextMenuItemViewModel)menuItemControl.DataContext };
                        menuItems.Add(showSourceMenuItem);
                    }
                    showSourceMenuItem.View.IsEnabled = true;
                    showSourceMenuItem.View.Visibility = Visibility.Visible;
                }

                // 自身がセパレータで自身より前もセパレータなら前を非表示にする
                var showMenuItems = menuItems
                    .SelectValueIndex()
                    .Where(i => i.Value.View.Visibility == Visibility.Visible)
                    .ToList()
                ;
                foreach(var showMenuItem in showMenuItems.SelectValueIndex()) {
                    if(0 < showMenuItem.Index && showMenuItem.Value.Value.Define.IsSeparator) {
                        if(menuItems[showMenuItem.Index - 1].Define.IsSeparator) {
                            menuItems[showMenuItem.Index - 1].View.Visibility = Visibility.Collapsed;
                        }
                    }
                }

                ContextMenu.IsOpen = true;
            }
        }

        private void BrowserGeckoFx_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            if(!Constants.WebNavigatorGeckoFxShowEngineLog) {
                if(this.ignoreGeckoFxLogs.Any(s => e.Message.IndexOf(s) != -1)) {
                    return;
                }
            }

            var browser = (ServiceGeckoWebBrowser)sender;
            browser.Mediation.Logger.Trace(e.Message);
        }

        private void BrowserGeckoFx_DomMouseDown(object sender, DomMouseEventArgs e)
        {
            if(IsEnabledGesture) {
                if(e.Button == GeckoMouseButton.Right) {
                    var pos = new Point(e.ClientX, e.ClientY);
                    var element = e.Target.CastToGeckoElement();
                    PointingGesture.StartPreparation(pos, element);
                }
            }
        }

        private void BrowserGeckoFx_DomMouseMove(object sender, DomMouseEventArgs e)
        {
            if(PointingGesture.State != PointingGestureState.None) {
                var pos = new Point(e.ClientX, e.ClientY);

                PointingGesture.Move(pos);
            } else {
                PointingGesture.Cancel();
            }

            var browser = (GeckoWebBrowser)sender;
            var element = browser.Document.ElementFromPoint(e.ClientX, e.ClientY);
            string hrefValue = null;
            if(element != null) {
                var elements = WebNavigatorCore.GetRootElementsGeckoFx(element).ToList();
                elements.Add(element);

                var anchorElement = elements.LastOrDefault(elm => elm.TagName.ToUpper() == "A");
                if(anchorElement != null) {
                    var href = anchorElement.GetAttribute("href");
                    if(!string.IsNullOrWhiteSpace(href)) {
                        hrefValue = href;
                    }
                }
            }

            SetLinkState(hrefValue);
        }

        private void BrowserGeckoFx_DomMouseUp(object sender, DomMouseEventArgs e)
        {
            if(PointingGesture.State == PointingGestureState.Action) {
                PointingGesture.Finish();
                e.Handled = true;
            } else {
                PointingGesture.Cancel();
            }
        }

        private void PointingGesture_Changed(object sender, Define.Event.PointingGestureChangedEventArgs e)
        {
            if(e.ChangeKind == PointingGestureChangeKind.Start || e.ChangeKind == PointingGestureChangeKind.Add) {
                this.popupGesture.IsOpen = true;
                GestureItems.Add(e.Item);
                var gestureCommand = GetGestureCommand(Mediation, GestureItems);
                this.textGesture.Text = gestureCommand.Message;
            } else {
                if(e.ChangeKind == PointingGestureChangeKind.Finish) {
                    var gestureCommand = GetGestureCommand(Mediation, GestureItems);
                    if(gestureCommand.IsSuccess) {
                        gestureCommand.Result.TryExecute(null);
                    }
                }

                this.popupGesture.IsOpen = false;
                GestureItems.Clear();
            }
            ContextMenu.IsOpen = false;
        }

        private void contentLinkState_MouseEnter(object sender, MouseEventArgs e)
        {
            this.popupLinkState.IsOpen = false;
        }
    }
}
