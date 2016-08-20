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
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video.Setting
{
    /// <summary>
    /// SmileVideoFinderFiltering.xaml の相互作用ロジック
    /// </summary>
    public partial class SmileVideoFinderFiltering: UserControl
    {
        #region event

        public event EventHandler FilteringChanged;

        #endregion

        public SmileVideoFinderFiltering()
        {
            InitializeComponent();
        }

        #region function

        protected void OnFilteringChanged()
        {
            if(FilteringChanged != null) {
                FilteringChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        static readonly DependencyProperty FilteringTypeItemsProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FilteringTypeItemsProperty)),
            typeof(IEnumerable<FilteringType>),
            typeof(SmileVideoFinderFiltering),
            new FrameworkPropertyMetadata(EnumUtility.GetMembers<FilteringType>())
        );

        static readonly DependencyProperty FilteringTargetItemsProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FilteringTargetItemsProperty)),
            typeof(IEnumerable<SmileVideoFinderFilteringTarget>),
            typeof(SmileVideoFinderFiltering),
            new FrameworkPropertyMetadata(EnumUtility.GetMembers<SmileVideoFinderFilteringTarget>())
        );

        #region FilteringItemSourceProperty

        public static readonly DependencyProperty FilteringProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FilteringProperty)),
            typeof(SmileVideoFilteringViweModel),
            typeof(SmileVideoFinderFiltering),
            new FrameworkPropertyMetadata(default(MVMPairCollectionBase<SmileVideoFinderFilteringItemSettingModel, SmileVideoFinderFilteringItemEditViewModel>), new PropertyChangedCallback(OnFilteringChanged))
        );

        private static void OnFilteringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileVideoFinderFiltering>(d, control => {
                control.Filtering = e.NewValue as SmileVideoFilteringViweModel;
                control.FilteringViewModelItemsSource = control.Filtering.FinderFilterList.ViewModelList;
                //control.selectIgnoreOverlapWord.IsChecked = control.Filtering.IgnoreOverlapWord;
                //control.inputIgnoreOverlapTime.Value = (int)control.Filtering.IgnoreOverlapTime.TotalSeconds;
                //var defineItems = control.Filtering.CommentDefineItems.Select(de => new SmileVideoFinderFilteringElementViewModel(de)).ToArray();
                //foreach(var item in defineItems) {
                //    item.IsChecked = control.Filtering.DefineKeys.Any(k => k == item.Key);
                //}
                //control.filteringList.ItemsSource = defineItems;
            });
        }


        public SmileVideoFilteringViweModel Filtering
        {
            get { return GetValue(FilteringProperty) as SmileVideoFilteringViweModel; }
            set { SetValue(FilteringProperty, value); }
        }

        #endregion

        #region FilteringViewModelItemsSourceProperty

        static readonly DependencyProperty FilteringViewModelItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FilteringViewModelItemsSourceProperty)),
            typeof(ObservableCollection<SmileVideoFinderFilteringItemEditViewModel>),
            typeof(SmileVideoFinderFiltering),
            new FrameworkPropertyMetadata(default(ObservableCollection<SmileVideoFinderFilteringItemEditViewModel>), new PropertyChangedCallback(OnFilteringViewModelItemsSourceChanged))
        );

        private static void OnFilteringViewModelItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileVideoFinderFiltering>(d, control => {
                control.FilteringViewModelItemsSource = e.NewValue as ObservableCollection<SmileVideoFinderFilteringItemEditViewModel>;
            });
        }


        ObservableCollection<SmileVideoFinderFilteringItemEditViewModel> FilteringViewModelItemsSource
        {
            get { return GetValue(FilteringViewModelItemsSourceProperty) as ObservableCollection<SmileVideoFinderFilteringItemEditViewModel>; }
            set { SetValue(FilteringViewModelItemsSourceProperty, value); }
        }

        #endregion

        #region SelectedFilteringEditItemProperty

        public static readonly DependencyProperty SelectedFilteringEditItemProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedFilteringEditItemProperty)),
            typeof(SmileVideoFinderFilteringItemEditViewModel),
            typeof(SmileVideoFinderFiltering),
            new FrameworkPropertyMetadata(default(SmileVideoFinderFilteringItemEditViewModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedFilteringEditItemChanged))
        );

        static void OnSelectedFilteringEditItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoFinderFiltering;
            if(control != null) {
                var oldItem = control.SelectedFilteringEditItem;
                control.SelectedFilteringEditItem = e.NewValue as SmileVideoFinderFilteringItemEditViewModel;
                if(oldItem != null) {
                    oldItem.Reset();
                }
                //if(control.SelectedFilteringEditItem == null) {
                //    control.selectType.SelectedIndex = 0;
                //    control.selectTarget.SelectedIndex = 0;
                //    control.selectIgnoreCase.IsChecked = true;
                //}
            }
        }

        public SmileVideoFinderFilteringItemEditViewModel SelectedFilteringEditItem
        {
            get { return GetValue(SelectedFilteringEditItemProperty) as SmileVideoFinderFilteringItemEditViewModel; }
            set { SetValue(SelectedFilteringEditItemProperty, value); }
        }

        #endregion

        #region RemoveCommandProperty

        static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(RemoveCommandProperty)),
            typeof(ICommand),
            typeof(SmileVideoFinderFiltering),
            new FrameworkPropertyMetadata(default(SmileVideoFinderFilteringItemEditViewModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnRemoveCommandChanged))
        );

        static void OnRemoveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoFinderFiltering;
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
                this.selectType.SelectedIndex = 0;
                this.selectTarget.SelectedIndex = 0;
                this.selectIgnoreCase.IsChecked = false;
                return;
            }
            var selectedItem = e.AddedItems[0];
            if(selectedItem == null) {
                return;
            }

            var filterItem = (SmileVideoFinderFilteringItemEditViewModel)selectedItem;
            SelectedFilteringEditItem = filterItem;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedFilteringEditItem == null) {
                return;
            }
            SelectedFilteringEditItem.Update();
            OnFilteringChanged();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var model = new SmileVideoFinderFilteringItemSettingModel();
            if(SelectedFilteringEditItem != null) {
                var src = SelectedFilteringEditItem;
                model.Type = src.EditingType;
                model.Target = src.EditingTarget;
                model.Source = src.EditingSource;
                model.IgnoreCase = src.EditingIgnoreCase;
            } else {
                model.Type = selectType.SelectedItem != null ? (FilteringType)selectType.SelectedItem : default(FilteringType);
                model.Target = selectTarget.SelectedItem != null ? (SmileVideoFinderFilteringTarget)selectTarget.SelectedItem : default(SmileVideoFinderFilteringTarget);
                model.Source = inputSource.Text;
                model.IgnoreCase = selectIgnoreCase.IsChecked.GetValueOrDefault();
            }
            var pair = Filtering.FinderFilterList.Add(model, null);
            SelectedFilteringEditItem = pair.ViewModel;
            OnFilteringChanged();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedFilteringEditItem == null) {
                return;
            }
            var a = Filtering.FinderFilterList.Remove(SelectedFilteringEditItem);
            SelectedFilteringEditItem = null;
            OnFilteringChanged();
        }


    }
}
