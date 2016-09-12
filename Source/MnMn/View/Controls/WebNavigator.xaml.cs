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
        WebBrowser browser = null;

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
                            Navigate(inputUri);
                        }
                    },
                    o => IsEnabledUserChangeSource && !string.IsNullOrWhiteSpace(location.Text)
                );
            }
        }

        #endregion

        #region function

        void NavigateDefault(Uri uri)
        {
            this.browser.Navigate(uri);
        }

        void NavigateGecko(Uri uri)
        {
            Gecko.Navigate(uri.OriginalString);
        }

        /// <summary>
        /// 指定 URI に移動。
        /// </summary>
        /// <param name="uri"></param>
        public void Navigate(Uri uri)
        {
            switch(WebNavigatorConfiguration.Engine) {
                case Define.WebNavigatorEngine.Default:
                    NavigateDefault(uri);
                    break;

                case Define.WebNavigatorEngine.GeckoFx:
                    NavigateGecko(uri);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void LoadHtmlDefault(string htmlSource)
        {
            this.browser.NavigateToString(htmlSource);
        }

        void LoadHtmlGecko(string htmlSource)
        {
            Gecko.LoadHtml(htmlSource);
        }

        /// <summary>
        /// HTMLを直接読み込み。
        /// </summary>
        /// <param name="htmlSource"></param>
        public void LoadHtml(string htmlSource)
        {
            switch(WebNavigatorConfiguration.Engine) {
                case Define.WebNavigatorEngine.Default:
                    LoadHtmlDefault(htmlSource);
                    break;

                case Define.WebNavigatorEngine.GeckoFx:
                    LoadHtmlGecko(htmlSource);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        [SecurityCritical]
        void RefreshDefault(bool noCache)
        {
            this.browser.Refresh(noCache);
        }

        void RefreshGecko(bool noCache)
        {
            if(noCache) {
                Gecko.Reload(GeckoLoadFlags.IsRefresh);
            } else {
                Gecko.Reload(GeckoLoadFlags.BypassCache);
            }
        }

        [SecurityCritical]
        public void Refresh(bool noCache = false)
        {
            switch(WebNavigatorConfiguration.Engine) {
                case Define.WebNavigatorEngine.Default:
                    RefreshDefault(noCache);
                    break;

                case Define.WebNavigatorEngine.GeckoFx:
                    RefreshGecko(noCache);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void InitializedDefault()
        {
            this.browser = new WebBrowser();

            this.browser.Loaded += browser_Loaded;
            this.browser.Navigating += browser_Navigating;
            this.browser.Navigated += browser_Navigated;

            this.container.Content = this.browser;
        }

        void InitializedGecko()
        {
            Gecko = WebNavigatorConfiguration.CreateBrowser();
            Gecko.CreateControl();

            var host = new WindowsFormsHost();
            host.Child = Gecko;

            this.container.Content = host;
        }

        #endregion

        #region UserControl

        protected override void OnInitialized(EventArgs e)
        {
            switch(WebNavigatorConfiguration.Engine) {
                case Define.WebNavigatorEngine.Default:
                    InitializedDefault();
                    break;

                case Define.WebNavigatorEngine.GeckoFx:
                    InitializedGecko();
                    break;

                default:
                    throw new NotImplementedException();
            }

            base.OnInitialized(e);
        }

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

    }
}
