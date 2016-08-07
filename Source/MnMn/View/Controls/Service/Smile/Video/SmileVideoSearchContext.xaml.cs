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

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video
{
    /// <summary>
    /// SmileVideoSearchContext.xaml の相互作用ロジック
    /// </summary>
    public partial class SmileVideoSearchContext: UserControl
    {
        public SmileVideoSearchContext()
        {
            InitializeComponent();
        }

        #region SelectedMethodProperty

        public static readonly DependencyProperty SelectedMethodProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedMethodProperty)),
            typeof(DefinedElementModel),
            typeof(SmileVideoSearchContext),
            new FrameworkPropertyMetadata(default(DefinedElementModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedMethodChanged))
        );

        private static void OnSelectedMethodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoSearchContext;
            if(control != null) {
                control.SelectedMethod = e.NewValue as DefinedElementModel;
            }
        }

        public DefinedElementModel SelectedMethod
        {
            get { return GetValue(SelectedMethodProperty) as DefinedElementModel; }
            set { SetValue(SelectedMethodProperty, value); }
        }

        #endregion

        #region SelectedSortProperty

        public static readonly DependencyProperty SelectedSortProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedSortProperty)),
            typeof(DefinedElementModel),
            typeof(SmileVideoSearchContext),
            new FrameworkPropertyMetadata(default(DefinedElementModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedSortChanged))
        );

        private static void OnSelectedSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoSearchContext;
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

        #region SortItemsProperty

        public static readonly DependencyProperty SortItemsProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SortItemsProperty)),
            typeof(ObservableCollection<DefinedElementModel>),
            typeof(SmileVideoSearchContext),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSortItemsChanged))
        );

        private static void OnSortItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileVideoSearchContext>(d, control => {
                control.SortItems = e.NewValue as ObservableCollection<DefinedElementModel>;
            });
        }

        public ObservableCollection<DefinedElementModel> SortItems
        {
            get { return GetValue(SortItemsProperty) as ObservableCollection<DefinedElementModel>; }
            set { SetValue(SortItemsProperty, value); }
        }

        #endregion

        #region MethodItemsProperty

        public static readonly DependencyProperty MethodItemsProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(MethodItemsProperty)),
            typeof(ObservableCollection<DefinedElementModel>),
            typeof(SmileVideoSearchContext),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnMethodItemsChanged))
        );

        private static void OnMethodItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileVideoSearchContext>(d, control => {
                control.MethodItems = e.NewValue as ObservableCollection<DefinedElementModel>;
            });
        }

        public ObservableCollection<DefinedElementModel> MethodItems
        {
            get { return GetValue(MethodItemsProperty) as ObservableCollection<DefinedElementModel>; }
            set { SetValue(MethodItemsProperty, value); }
        }

        #endregion

    }
}
