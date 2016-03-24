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
    /// SourceLoadStateNavigator.xaml の相互作用ロジック
    /// </summary>
    public partial class SourceLoadStateNavigator: UserControl
    {
        public SourceLoadStateNavigator()
        {
            InitializeComponent();
        }

        #region SourceLoadStateProperty

        public static readonly DependencyProperty SourceLoadStateProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SourceLoadStateProperty)),
            typeof(SourceLoadState),
            typeof(SourceLoadStateNavigator),
            new FrameworkPropertyMetadata(SourceLoadState.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSourceLoadStateChanged))
        );

        private static void OnSourceLoadStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SourceLoadStateNavigator;
            if(control != null) {
                control.SourceLoadState = (SourceLoadState)e.NewValue;
            }
        }

        public SourceLoadState SourceLoadState
        {
            get { return (SourceLoadState)GetValue(SourceLoadStateProperty); }
            set { SetValue(SourceLoadStateProperty, value); }
        }

        #endregion

        #region SpeedProperty

        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SpeedProperty)),
            typeof(TimeSpan),
            typeof(SourceLoadStateNavigator),
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
