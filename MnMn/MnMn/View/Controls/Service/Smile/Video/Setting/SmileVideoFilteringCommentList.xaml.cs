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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video.Setting
{
    /// <summary>
    /// SmileVideoFilteringCommentList.xaml の相互作用ロジック
    /// </summary>
    public partial class SmileVideoFilteringCommentList: UserControl
    {
        public SmileVideoFilteringCommentList()
        {
            InitializeComponent();
        }

        #region FilteringItemSourceProperty

        public static readonly DependencyProperty FilteringItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FilteringItemsSourceProperty)),
            typeof(object),
            typeof(SmileVideoFilteringCommentList),
            new FrameworkPropertyMetadata(default(object), new PropertyChangedCallback(OnFilteringItemsSourceChanged))
        );

        private static void OnFilteringItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileVideoFilteringCommentList>(d, control => {
                control.FilteringItemsSource = e.NewValue as object;
            });
        }

        public object FilteringItemsSource
        {
            get { return GetValue(FilteringItemsSourceProperty) as object; }
            set { SetValue(FilteringItemsSourceProperty, value); }
        }

        #endregion

        #region SelectedFilteringEditItemProperty

        public static readonly DependencyProperty SelectedFilteringEditItemProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedFilteringEditItemProperty)),
            typeof(SmileVideoFilteringEditItemViewModel),
            typeof(SmileVideoFilteringCommentList),
            new FrameworkPropertyMetadata(default(SmileVideoFilteringEditItemViewModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedFilteringEditItemChanged))
        );

        private static void OnSelectedFilteringEditItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoFilteringCommentList;
            if(control != null) {
                control.SelectedFilteringEditItem = e.NewValue as SmileVideoFilteringEditItemViewModel;
            }
        }

        public SmileVideoFilteringEditItemViewModel SelectedFilteringEditItem
        {
            get { return GetValue(SelectedFilteringEditItemProperty) as SmileVideoFilteringEditItemViewModel; }
            set { SetValue(SelectedFilteringEditItemProperty, value); }
        }

        #endregion

    }
}
