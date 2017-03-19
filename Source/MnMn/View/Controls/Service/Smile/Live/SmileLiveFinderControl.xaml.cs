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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Live
{
    /// <summary>
    /// SmileLiveFinderControl.xaml の相互作用ロジック
    /// </summary>
    public partial class SmileLiveFinderControl: UserControl
    {
        public SmileLiveFinderControl()
        {
            InitializeComponent();
        }

        #region HeaderContentProperty

        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(HeaderContentProperty)),
            typeof(object),
            typeof(SmileLiveFinderControl),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnHeaderContentChanged))
        );

        private static void OnHeaderContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileLiveFinderControl;
            if(control != null) {
                control.HeaderContent = e.NewValue;
            }
        }

        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        #endregion

        #region FooterContentProperty

        public static readonly DependencyProperty FooterContentProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FooterContentProperty)),
            typeof(object),
            typeof(SmileLiveFinderControl),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnFooterContentChanged))
        );

        private static void OnFooterContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileLiveFinderControl;
            if(control != null) {
                control.FooterContent = e.NewValue;
            }
        }

        public object FooterContent
        {
            get { return GetValue(FooterContentProperty); }
            set { SetValue(FooterContentProperty, value); }
        }

        #endregion

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FinderUtility.ScrollDataContextChanged(this.PART_List, e.OldValue, e.NewValue);
        }

        private void PART_List_Loaded(object sender, RoutedEventArgs e)
        {
            FinderUtility.ScrollItemsControlLoaded(this.PART_List, DataContext);
        }
    }
}
