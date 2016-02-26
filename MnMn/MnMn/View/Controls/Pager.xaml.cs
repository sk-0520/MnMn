using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// Pasing.xaml の相互作用ロジック
    /// </summary>
    public partial class Pager: UserControl
    {
        public Pager()
        {
            InitializeComponent();
        }

        #region PageItemsProperty

        public static readonly DependencyProperty PageItemsProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(PageItemsProperty)),
            typeof(object),
            typeof(Pager),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnVideoLoadingForegroundChanged))
        );

        private static void OnVideoLoadingForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Pager;
            if(control != null) {
                control.PageItems = e.NewValue;
            }
        }

        public object PageItems
        {
            get { return GetValue(PageItemsProperty); }
            set { SetValue(PageItemsProperty, value); }
        }

        #endregion

        #region Command

        #region CommandProperty

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(CommandProperty)),
            typeof(ICommand),
            typeof(Pager),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCommandChanged))
        );

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Pager;
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

        #endregion
    }
}
