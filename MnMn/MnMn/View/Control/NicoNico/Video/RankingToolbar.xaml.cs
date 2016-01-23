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

namespace ContentTypeTextNet.MnMn.MnMn.View.Control.NicoNico.Video
{
    /// <summary>
    /// RankingToolbar.xaml の相互作用ロジック
    /// </summary>
    public partial class RankingToolbar: UserControl
    {
        public RankingToolbar()
        {
            InitializeComponent();
        }

        #region CommandProperty

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(CommandProperty)),
            typeof(ICommand),
            typeof(RankingToolbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCommandChanged))
        );

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RankingToolbar;
            if(control != null) {
                control.Command = e.NewValue as ICommand;
            }
        }

        public ICommand Command
        {
            get { return GetValue(CommandProperty) as ICommand; }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(CommandParameterProperty)),
            typeof(object),
            typeof(RankingToolbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCommandParameterChanged))
        );

        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RankingToolbar;
            control.CommandParameter = e.NewValue;
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        #endregion

    }
}
