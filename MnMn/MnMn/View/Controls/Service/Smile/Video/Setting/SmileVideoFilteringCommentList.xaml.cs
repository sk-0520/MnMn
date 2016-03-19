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
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video.Setting
{
    /// <summary>
    /// SmileVideoFilteringCommentList.xaml の相互作用ロジック
    /// <para>Formsのにほい。</para>
    /// </summary>
    public partial class SmileVideoFilteringCommentList: UserControl
    {
        public SmileVideoFilteringCommentList()
        {
            InitializeComponent();
        }

        static readonly DependencyProperty FilteringTypeItemsProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FilteringTypeItemsProperty)),
            typeof(IEnumerable<FilteringType>),
            typeof(SmileVideoFilteringCommentList),
            new FrameworkPropertyMetadata(EnumUtility.GetMembers<FilteringType>())
        );

        static readonly DependencyProperty FilteringTargetItemsProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FilteringTargetItemsProperty)),
            typeof(IEnumerable<SmileVideoFilteringTarget>),
            typeof(SmileVideoFilteringCommentList),
            new FrameworkPropertyMetadata(EnumUtility.GetMembers<SmileVideoFilteringTarget>())
        );

        #region FilteringItemSourceProperty

        public static readonly DependencyProperty FilteringItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FilteringItemsSourceProperty)),
            typeof(MVMPairCollectionBase<SmileVideoFilteringSettingModel, SmileVideoFilteringEditItemViewModel>),
            typeof(SmileVideoFilteringCommentList),
            new FrameworkPropertyMetadata(default(MVMPairCollectionBase<SmileVideoFilteringSettingModel, SmileVideoFilteringEditItemViewModel>), new PropertyChangedCallback(OnFilteringItemsSourceChanged))
        );

        private static void OnFilteringItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileVideoFilteringCommentList>(d, control => {
                control.FilteringItemsSource = e.NewValue as MVMPairCollectionBase<SmileVideoFilteringSettingModel, SmileVideoFilteringEditItemViewModel>;
                control.FilteringViewModelItemsSource = control.FilteringItemsSource.ViewModelList;
            });
        }


        public MVMPairCollectionBase<SmileVideoFilteringSettingModel, SmileVideoFilteringEditItemViewModel> FilteringItemsSource
        {
            get { return GetValue(FilteringItemsSourceProperty) as MVMPairCollectionBase<SmileVideoFilteringSettingModel, SmileVideoFilteringEditItemViewModel>; }
            set { SetValue(FilteringItemsSourceProperty, value); }
        }

        #endregion

        #region FilteringViewModelItemsSourceProperty

        static readonly DependencyProperty FilteringViewModelItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FilteringViewModelItemsSourceProperty)),
            typeof(ObservableCollection<SmileVideoFilteringEditItemViewModel>),
            typeof(SmileVideoFilteringCommentList),
            new FrameworkPropertyMetadata(default(ObservableCollection<SmileVideoFilteringEditItemViewModel>), new PropertyChangedCallback(OnFilteringViewModelItemsSourceChanged))
        );

        private static void OnFilteringViewModelItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileVideoFilteringCommentList>(d, control => {
                control.FilteringViewModelItemsSource = e.NewValue as ObservableCollection<SmileVideoFilteringEditItemViewModel>;
            });
        }


        ObservableCollection<SmileVideoFilteringEditItemViewModel> FilteringViewModelItemsSource
        {
            get { return GetValue(FilteringViewModelItemsSourceProperty) as ObservableCollection<SmileVideoFilteringEditItemViewModel>; }
            set { SetValue(FilteringViewModelItemsSourceProperty, value); }
        }

        #endregion

        #region SelectedFilteringEditItemProperty

        public static readonly DependencyProperty SelectedFilteringEditItemProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedFilteringEditItemProperty)),
            typeof(SmileVideoFilteringEditItemViewModel),
            typeof(SmileVideoFilteringCommentList),
            new FrameworkPropertyMetadata(default(SmileVideoFilteringEditItemViewModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedFilteringEditItemChanged))
        );

        static void OnSelectedFilteringEditItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

        #region RemoveCommandProperty

        static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(RemoveCommandProperty)),
            typeof(ICommand),
            typeof(SmileVideoFilteringCommentList),
            new FrameworkPropertyMetadata(default(SmileVideoFilteringEditItemViewModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnRemoveCommandChanged))
        );

        static void OnRemoveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoFilteringCommentList;
            if(control != null) {
                control.RemoveCommand = e.NewValue as ICommand;
            }
        }

        public ICommand RemoveCommand
        {
            get { return GetValue(RemoveCommandProperty) as ICommand; }
            set { SetValue(RemoveCommandProperty, value); }
        }

        #endregion

        private void filterItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count != 1) {
                return;
            }
            var selectedItem = e.AddedItems[0];
            if(selectedItem == null) {
                return;
            }

            var filterItem = (SmileVideoFilteringEditItemViewModel)selectedItem;
            SelectedFilteringEditItem = filterItem;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedFilteringEditItem == null) {
                return;
            }
            SelectedFilteringEditItem.Update();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var model = new SmileVideoFilteringSettingModel();
            if(SelectedFilteringEditItem != null) {
                var src = SelectedFilteringEditItem;
                model.Type = src.EditingType;
                model.Target = src.EditingTarget;
                model.Source = src.EditingSource;
                model.IgnoreCase = src.EditingIgnoreCase;
            } else {
                model.Type = selectType.SelectedItem != null ? (FilteringType)selectType.SelectedItem: default(FilteringType);
                model.Target = selectTarget.SelectedItem != null ? (SmileVideoFilteringTarget)selectTarget.SelectedItem : default(SmileVideoFilteringTarget);
                model.Source = inputSource.Text;
                model.IgnoreCase = selectIgnoreCase.IsChecked.GetValueOrDefault();
            }
            var pair = FilteringItemsSource.Add(model, null);
            SelectedFilteringEditItem = pair.ViewModel;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedFilteringEditItem == null) {
                return;
            }
            var a = FilteringItemsSource.Remove(SelectedFilteringEditItem);
            SelectedFilteringEditItem = null;
        }

    }
}
