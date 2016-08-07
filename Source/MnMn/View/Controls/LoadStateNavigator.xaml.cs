using System;
using System.Collections.Generic;
using System.Linq;
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
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// LoadStateNavigator.xaml の相互作用ロジック
    /// </summary>
    public partial class LoadStateNavigator: UserControl
    {
        public LoadStateNavigator()
        {
            InitializeComponent();
        }

        #region LoadStateProperty

        public static readonly DependencyProperty LoadStateProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(LoadStateProperty)),
            typeof(LoadState),
            typeof(LoadStateNavigator),
            new FrameworkPropertyMetadata(Define.LoadState.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnLoadStateChanged))
        );

        private static void OnLoadStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LoadStateNavigator;
            if(control != null) {
                control.LoadState = (LoadState)e.NewValue;
            }
        }

        public LoadState LoadState
        {
            get { return (LoadState)GetValue(LoadStateProperty); }
            set { SetValue(LoadStateProperty, value); }
        }

        #endregion

        #region LoadedToVisibilityProperty

        public static readonly DependencyProperty LoadedToVisibilityProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(LoadedToVisibilityProperty)),
            typeof(Visibility),
            typeof(LoadStateNavigator),
            new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnLoadedToVisibilityChanged))
        );

        private static void OnLoadedToVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LoadStateNavigator;
            if(control != null) {
                control.LoadedToVisibility = (Visibility)e.NewValue;
            }
        }

        public Visibility LoadedToVisibility
        {
            get { return (Visibility)GetValue(LoadedToVisibilityProperty); }
            set { SetValue(LoadedToVisibilityProperty, value); }
        }

        #endregion

        #region ShowStateTextProperty

        public static readonly DependencyProperty ShowStateTextProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(ShowStateTextProperty)),
            typeof(bool),
            typeof(LoadStateNavigator),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnShowStateTextChanged))
        );

        private static void OnShowStateTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LoadStateNavigator;
            if(control != null) {
                control.ShowStateText = (bool)e.NewValue;
            }
        }

        public bool ShowStateText
        {
            get { return (bool)GetValue(ShowStateTextProperty); }
            set { SetValue(ShowStateTextProperty, value); }
        }

        #endregion

        #region SpeedProperty

        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SpeedProperty)),
            typeof(TimeSpan),
            typeof(LoadStateNavigator),
            new FrameworkPropertyMetadata(TimeSpan.FromSeconds(1), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSpeedChanged))
        );

        private static void OnSpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LoadStateNavigator;
            if(control != null) {
                control.Speed = (TimeSpan)e.NewValue;
            }
        }

        public TimeSpan Speed
        {
            get { return (TimeSpan)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        #endregion
    }
}
