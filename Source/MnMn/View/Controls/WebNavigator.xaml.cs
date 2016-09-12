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

        WebBrowser BrowserDefault { get; set; }
        GeckoWebBrowser BrowserGeckoFx { get; set; }

        public Uri Source
        {
            get
            {
                switch(WebNavigatorCore.Engine) {
                    case Define.WebNavigatorEngine.Default:
                        return BrowserDefault.Source;

                    case Define.WebNavigatorEngine.GeckoFx:
                        return BrowserGeckoFx.Url;

                    default:
                        throw new NotImplementedException();
                }
            }
            //set { browser.Source = value; }
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
                switch(WebNavigatorCore.Engine) {
                    case Define.WebNavigatorEngine.Default:
                        return BrowserDefault.CanGoBack;

                    case Define.WebNavigatorEngine.GeckoFx:
                        return BrowserGeckoFx.CanGoBack;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// 「進む」は使用可能か。
        /// </summary>
        public bool CanGoForward
        {
            get
            {
                switch(WebNavigatorCore.Engine) {
                    case Define.WebNavigatorEngine.Default:
                        return BrowserDefault.CanGoForward;

                    case Define.WebNavigatorEngine.GeckoFx:
                        return BrowserGeckoFx.CanGoForward;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

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
                    o => { GoBack(); },
                    o => CanGoBack
                );
            }
        }

        public ICommand ForwardCommand
        {
            get
            {
                return new DelegateCommand(
                    o => { GoForward(); },
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

        TResult DoFunction<TResult>(Func<WebBrowser, TResult> defaultFunction, Func<GeckoWebBrowser, TResult> geckoFxFunction)
        {
            CheckUtility.DebugEnforceNotNull(defaultFunction);
            CheckUtility.DebugEnforceNotNull(geckoFxFunction);

            switch(WebNavigatorCore.Engine) {
                case Define.WebNavigatorEngine.Default:
                    return defaultFunction(BrowserDefault);

                case Define.WebNavigatorEngine.GeckoFx:
                    return geckoFxFunction(BrowserGeckoFx);

                default:
                    throw new NotImplementedException();
            }
        }

        void DoAction(Action<WebBrowser> defaultAction, Action<GeckoWebBrowser> geckoFxAction)
        {
            CheckUtility.DebugEnforceNotNull(defaultAction);
            CheckUtility.DebugEnforceNotNull(geckoFxAction);

            var dmy = -1;

            DoFunction(
                b => { defaultAction(b); return dmy; },
                b => { geckoFxAction(b); return dmy; }
            );
        }

        //void GoBackDefault()
        //{
        //    BrowserDefault.GoBack();
        //}

        //void GoBackGeckoFx()
        //{
        //    BrowserGeckoFx.GoBack();
        //}

        /// <summary>
        /// 戻る。
        /// </summary>
        public void GoBack()
        {
            //switch(WebNavigatorCore.Engine) {
            //    case Define.WebNavigatorEngine.Default:
            //        GoBackDefault();
            //        break;

            //    case Define.WebNavigatorEngine.GeckoFx:
            //        GoBackGeckoFx();
            //        break;

            //    default:
            //        throw new NotImplementedException();
            //}
            DoAction(
                b => b.GoBack(),
                b => b.GoBack()
            );
        }

        //void GoForwardDefault()
        //{
        //    BrowserDefault.GoForward();
        //}

        //void GoForwardGeckoFx()
        //{
        //    BrowserGeckoFx.GoForward();
        //}

        /// <summary>
        /// 進む。
        /// </summary>
        public void GoForward()
        {
            //switch(WebNavigatorCore.Engine) {
            //    case Define.WebNavigatorEngine.Default:
            //        GoForwardDefault();
            //        break;

            //    case Define.WebNavigatorEngine.GeckoFx:
            //        GoForwardGeckoFx();
            //        break;

            //    default:
            //        throw new NotImplementedException();
            //}
            DoAction(
                b => b.GoForward(),
                b => b.GoForward()
            );
        }

        //void NavigateDefault(Uri uri)
        //{
        //    BrowserDefault.Navigate(uri);
        //}

        //void NavigateGeckoFx(Uri uri)
        //{
        //    BrowserGeckoFx.Navigate(uri.OriginalString);
        //}

        /// <summary>
        /// 指定 URI に移動。
        /// </summary>
        /// <param name="uri"></param>
        public void Navigate(Uri uri)
        {
            //switch(WebNavigatorCore.Engine) {
            //    case Define.WebNavigatorEngine.Default:
            //        NavigateDefault(uri);
            //        break;

            //    case Define.WebNavigatorEngine.GeckoFx:
            //        NavigateGeckoFx(uri);
            //        break;

            //    default:
            //        throw new NotImplementedException();
            //}
            DoAction(
                b => b.Navigate(uri),
                b => b.Navigate(uri.OriginalString)
            );
            IsEmptyContent = false;
        }

        //void LoadHtmlDefault(string htmlSource)
        //{
        //    BrowserDefault.NavigateToString(htmlSource);
        //}

        //void LoadHtmlGeckoFx(string htmlSource)
        //{
        //    BrowserGeckoFx.LoadHtml(htmlSource);
        //}

        /// <summary>
        /// HTMLを直接読み込み。
        /// </summary>
        /// <param name="htmlSource"></param>
        public void LoadHtml(string htmlSource)
        {
            //switch(WebNavigatorCore.Engine) {
            //    case Define.WebNavigatorEngine.Default:
            //        LoadHtmlDefault(htmlSource);
            //        break;

            //    case Define.WebNavigatorEngine.GeckoFx:
            //        LoadHtmlGeckoFx(htmlSource);
            //        break;

            //    default:
            //        throw new NotImplementedException();
            //}
            DoAction(
                b => b.NavigateToString(htmlSource),
                b => b.LoadHtml(htmlSource)
            );
            IsEmptyContent = false;
        }

        //[SecurityCritical]
        //void RefreshDefault(bool noCache)
        //{
        //    BrowserDefault.Refresh(noCache);
        //}

        //void RefreshGeckoFx(bool noCache)
        //{
        //    if(noCache) {
        //        BrowserGeckoFx.Reload(GeckoLoadFlags.IsRefresh);
        //    } else {
        //        BrowserGeckoFx.Reload(GeckoLoadFlags.BypassCache);
        //    }
        //}

        [SecurityCritical]
        public void Refresh(bool noCache = false)
        {
            //switch(WebNavigatorCore.Engine) {
            //    case Define.WebNavigatorEngine.Default:
            //        RefreshDefault(noCache);
            //        break;

            //    case Define.WebNavigatorEngine.GeckoFx:
            //        RefreshGeckoFx(noCache);
            //        break;

            //    default:
            //        throw new NotImplementedException();
            //}
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

            BrowserDefault.Unloaded += BrowserDefault_Unloaded;
            BrowserDefault.Loaded += BrowserDefault_Loaded;
            BrowserDefault.Navigating += BrowserDefault_Navigating;
            BrowserDefault.Navigated += BrowserDefault_Navigated;

            this.container.Content = BrowserDefault;
        }

        void InitializedGeckoFx()
        {
            BrowserGeckoFx = WebNavigatorCore.CreateBrowser();
            BrowserGeckoFx.CreateControl();

            BrowserGeckoFx.Disposed += BrowserGeckoFx_Disposed;
            BrowserGeckoFx.Navigating += BrowserGeckoFx_Navigating;
            BrowserGeckoFx.Navigated += BrowserGeckoFx_Navigated;

            var host = new WindowsFormsHost();
            using(Initializer.BeginInitialize(host)) {
                host.Child = BrowserGeckoFx;
            }

            this.container.Content = host;
        }

        #endregion

        #region UserControl

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //switch(WebNavigatorCore.Engine) {
            //    case Define.WebNavigatorEngine.Default:
            //        InitializedDefault();
            //        break;

            //    case Define.WebNavigatorEngine.GeckoFx:
            //        InitializedGeckoFx();
            //        break;

            //    default:
            //        throw new NotImplementedException();
            //}
            DoAction(
                b => InitializedDefault(),
                b => InitializedGeckoFx()
            );

            IsEmptyContent = true;
        }

        #endregion

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
        }

        private void BrowserGeckoFx_Disposed(object sender, EventArgs e)
        {
            BrowserGeckoFx.Disposed -= BrowserGeckoFx_Disposed;
            BrowserGeckoFx.Navigating -= BrowserGeckoFx_Navigating;
            BrowserGeckoFx.Navigated -= BrowserGeckoFx_Navigated;
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

    }
}
