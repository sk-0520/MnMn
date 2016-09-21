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
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Live
{
    /// <summary>
    /// SmileLiveCategoryContext.xaml の相互作用ロジック
    /// </summary>
    public partial class SmileLiveCategoryContext: UserControl
    {
        public SmileLiveCategoryContext()
        {
            InitializeComponent();
        }

        #region SelectedSortProperty

        public static readonly DependencyProperty SelectedSortProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedSortProperty)),
            typeof(DefinedElementModel),
            typeof(SmileLiveCategoryContext),
            new FrameworkPropertyMetadata(default(DefinedElementModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedSortChanged))
        );

        private static void OnSelectedSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileLiveCategoryContext;
            if(control != null) {
                control.SelectedSort = e.NewValue as DefinedElementModel;
            }
        }

        public DefinedElementModel SelectedSort
        {
            get { return GetValue(SelectedSortProperty) as DefinedElementModel; }
            set { SetValue(SelectedSortProperty, value); }
        }

        #endregion

        #region SelectedOrderProperty

        public static readonly DependencyProperty SelectedOrderProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedOrderProperty)),
            typeof(DefinedElementModel),
            typeof(SmileLiveCategoryContext),
            new FrameworkPropertyMetadata(default(DefinedElementModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedOrderChanged))
        );

        private static void OnSelectedOrderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileLiveCategoryContext;
            if(control != null) {
                control.SelectedOrder = e.NewValue as DefinedElementModel;
            }
        }

        public DefinedElementModel SelectedOrder
        {
            get { return GetValue(SelectedOrderProperty) as DefinedElementModel; }
            set { SetValue(SelectedOrderProperty, value); }
        }

        #endregion

        #region SortItemsSourceProperty

        public static readonly DependencyProperty SortItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SortItemsSourceProperty)),
            typeof(ObservableCollection<DefinedElementModel>),
            typeof(SmileLiveCategoryContext),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSortItemsSourceChanged))
        );

        private static void OnSortItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileLiveCategoryContext>(d, control => {
                control.SortItemsSource = e.NewValue as ObservableCollection<DefinedElementModel>;
            });
        }

        public ObservableCollection<DefinedElementModel> SortItemsSource
        {
            get { return GetValue(SortItemsSourceProperty) as ObservableCollection<DefinedElementModel>; }
            set { SetValue(SortItemsSourceProperty, value); }
        }

        #endregion

        #region OrderItemsSourceProperty

        public static readonly DependencyProperty OrderItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(OrderItemsSourceProperty)),
            typeof(ObservableCollection<DefinedElementModel>),
            typeof(SmileLiveCategoryContext),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnOrderItemsSourceChanged))
        );

        private static void OnOrderItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileLiveCategoryContext>(d, control => {
                control.OrderItemsSource = e.NewValue as ObservableCollection<DefinedElementModel>;
            });
        }

        public ObservableCollection<DefinedElementModel> OrderItemsSource
        {
            get { return GetValue(OrderItemsSourceProperty) as ObservableCollection<DefinedElementModel>; }
            set { SetValue(OrderItemsSourceProperty, value); }
        }

        #endregion

    }
}
