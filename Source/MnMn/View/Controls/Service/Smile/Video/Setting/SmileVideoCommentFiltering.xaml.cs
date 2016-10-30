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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video.Setting
{
    /// <summary>
    /// SmileVideoCommentFiltering.xaml の相互作用ロジック
    /// <para>Formsのにほい。</para>
    /// </summary>
    public partial class SmileVideoCommentFiltering: UserControl
    {
        #region event

        public event EventHandler FilteringChanged;

        #endregion

        public SmileVideoCommentFiltering()
        {
            InitializeComponent();
        }

        #region property

        #endregion

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
            typeof(SmileVideoCommentFiltering),
            new FrameworkPropertyMetadata(EnumUtility.GetMembers<FilteringType>())
        );

        static readonly DependencyProperty FilteringTargetItemsProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FilteringTargetItemsProperty)),
            typeof(IEnumerable<SmileVideoCommentFilteringTarget>),
            typeof(SmileVideoCommentFiltering),
            new FrameworkPropertyMetadata(EnumUtility.GetMembers<SmileVideoCommentFilteringTarget>())
        );

        #region FilteringItemSourceProperty

        public static readonly DependencyProperty FilteringProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FilteringProperty)),
            typeof(SmileVideoFilteringViweModel),
            typeof(SmileVideoCommentFiltering),
            new FrameworkPropertyMetadata(default(MVMPairCollectionBase<SmileVideoCommentFilteringItemEditViewMode, SmileVideoCommentFilteringItemEditViewModel>), new PropertyChangedCallback(OnFilteringChanged))
        );

        private static void OnFilteringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileVideoCommentFiltering>(d, control => {
                control.Filtering = e.NewValue as SmileVideoFilteringViweModel;
                //control.FilteringViewModelItemsSource = control.Filtering.CommentFilterList.ViewModelList;
                control.selectIgnoreOverlapWord.IsChecked = control.Filtering.IgnoreOverlapWord;
                control.inputIgnoreOverlapTime.Value = (int)control.Filtering.IgnoreOverlapTime.TotalSeconds;
                var defineItems = control.Filtering.CommentDefineItems.Select(de => new SmileVideoCommentFilteringElementViewModel(de)).ToArray();
                foreach(var item in defineItems) {
                    item.IsChecked = control.Filtering.DefineKeys.Any(k => k == item.Key);
                }
                control.filteringList.ItemsSource = defineItems;
            });
        }


        public SmileVideoFilteringViweModel Filtering
        {
            get { return GetValue(FilteringProperty) as SmileVideoFilteringViweModel; }
            set { SetValue(FilteringProperty, value); }
        }

        #endregion

        //#region FilteringViewModelItemsSourceProperty

        //static readonly DependencyProperty FilteringViewModelItemsSourceProperty = DependencyProperty.Register(
        //    DependencyPropertyUtility.GetName(nameof(FilteringViewModelItemsSourceProperty)),
        //    typeof(ObservableCollection<SmileVideoCommentFilteringItemEditViewModel>),
        //    typeof(SmileVideoCommentFiltering),
        //    new FrameworkPropertyMetadata(default(ObservableCollection<SmileVideoCommentFilteringItemEditViewModel>), new PropertyChangedCallback(OnFilteringViewModelItemsSourceChanged))
        //);

        //private static void OnFilteringViewModelItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    CastUtility.AsAction<SmileVideoCommentFiltering>(d, control => {
        //        control.FilteringViewModelItemsSource = e.NewValue as ObservableCollection<SmileVideoCommentFilteringItemEditViewModel>;
        //    });
        //}


        //ObservableCollection<SmileVideoCommentFilteringItemEditViewModel> FilteringViewModelItemsSource
        //{
        //    get { return GetValue(FilteringViewModelItemsSourceProperty) as ObservableCollection<SmileVideoCommentFilteringItemEditViewModel>; }
        //    set { SetValue(FilteringViewModelItemsSourceProperty, value); }
        //}

        //#endregion

        //#region SelectedFilteringEditItemProperty

        //public static readonly DependencyProperty SelectedFilteringEditItemProperty = DependencyProperty.Register(
        //    DependencyPropertyUtility.GetName(nameof(SelectedFilteringEditItemProperty)),
        //    typeof(SmileVideoCommentFilteringItemEditViewModel),
        //    typeof(SmileVideoCommentFiltering),
        //    new FrameworkPropertyMetadata(default(SmileVideoCommentFilteringItemEditViewModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedFilteringEditItemChanged))
        //);

        //static void OnSelectedFilteringEditItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var control = d as SmileVideoCommentFiltering;
        //    if(control != null) {
        //        var oldItem = control.SelectedFilteringEditItem;
        //        control.SelectedFilteringEditItem = e.NewValue as SmileVideoCommentFilteringItemEditViewModel;
        //        if(oldItem != null) {
        //            oldItem.Reset();
        //        }
        //        //if(control.SelectedFilteringEditItem == null) {
        //        //    control.selectType.SelectedIndex = 0;
        //        //    control.selectTarget.SelectedIndex = 0;
        //        //    control.selectIgnoreCase.IsChecked = true;
        //        //}
        //    }
        //}

        //public SmileVideoCommentFilteringItemEditViewModel SelectedFilteringEditItem
        //{
        //    get { return GetValue(SelectedFilteringEditItemProperty) as SmileVideoCommentFilteringItemEditViewModel; }
        //    set { SetValue(SelectedFilteringEditItemProperty, value); }
        //}

        //#endregion

        //#region RemoveCommandProperty

        //static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register(
        //    DependencyPropertyUtility.GetName(nameof(RemoveCommandProperty)),
        //    typeof(ICommand),
        //    typeof(SmileVideoCommentFiltering),
        //    new FrameworkPropertyMetadata(default(SmileVideoCommentFilteringItemEditViewModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnRemoveCommandChanged))
        //);

        //static void OnRemoveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var control = d as SmileVideoCommentFiltering;
        //    if(control != null) {
        //        control.RemoveCommand = e.NewValue as ICommand;
        //    }
        //}

        //public ICommand RemoveCommand
        //{
        //    get { return GetValue(RemoveCommandProperty) as ICommand; }
        //    set { SetValue(RemoveCommandProperty, value); }
        //}

        //#endregion

        #region function

        void SelectionChangedCore(object sender, SelectionChangedEventArgs e)
        {
            if(Filtering == null) {
                return;
            }

            e.Handled = true;
            OnFilteringChanged();
        }

        private void CheckBoxChangedCore(object sender, RoutedEventArgs e)
        {
            if(Filtering == null) {
                return;
            }

            e.Handled = true;
            OnFilteringChanged();
        }


        #endregion

        //private void filterItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if(e.AddedItems.Count != 1) {
        //        this.selectType.SelectedIndex = 0;
        //        this.selectTarget.SelectedIndex = 0;
        //        this.selectIgnoreCase.IsChecked = false;
        //        return;
        //    }
        //    var selectedItem = e.AddedItems[0];
        //    if(selectedItem == null) {
        //        return;
        //    }

        //    var filterItem = (SmileVideoCommentFilteringItemEditViewModel)selectedItem;
        //    SelectedFilteringEditItem = filterItem;
        //}

        //private void UpdateButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if(SelectedFilteringEditItem == null) {
        //        return;
        //    }
        //    SelectedFilteringEditItem.Update();
        //    OnFilteringChanged();
        //}

        //private void CreateButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var model = new SmileVideoCommentFilteringItemSettingModel();
        //    if(SelectedFilteringEditItem != null) {
        //        var src = SelectedFilteringEditItem;
        //        model.Type = src.EditingType;
        //        model.Target = src.EditingTarget;
        //        model.Source = src.EditingSource;
        //        model.IgnoreCase = src.EditingIgnoreCase;
        //    } else {
        //        model.Type = selectType.SelectedItem != null ? (FilteringType)selectType.SelectedItem : default(FilteringType);
        //        model.Target = selectTarget.SelectedItem != null ? (SmileVideoCommentFilteringTarget)selectTarget.SelectedItem : default(SmileVideoCommentFilteringTarget);
        //        model.Source = inputSource.Text;
        //        model.IgnoreCase = selectIgnoreCase.IsChecked.GetValueOrDefault();
        //    }
        //    var pair = Filtering.CommentFilterList.Add(model, null);
        //    SelectedFilteringEditItem = pair.ViewModel;
        //    OnFilteringChanged();
        //}

        //private void RemoveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if(SelectedFilteringEditItem == null) {
        //        return;
        //    }
        //    var a = Filtering.CommentFilterList.Remove(SelectedFilteringEditItem);
        //    SelectedFilteringEditItem = null;
        //    OnFilteringChanged();
        //}

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(Filtering == null) {
                return;
            }

            var checkbox = (CheckBox)sender;
            Filtering.IgnoreOverlapWord = checkbox.IsChecked.GetValueOrDefault();
            e.Handled = true;

            OnFilteringChanged();
        }

        private void selectIgnoreOverlapWord_Checked_Unchecked(object sender, RoutedEventArgs e)
        {
            if(Filtering == null) {
                return;
            }

            Filtering.IgnoreOverlapWord = this.selectIgnoreOverlapWord.IsChecked.GetValueOrDefault();
            e.Handled = true;

            OnFilteringChanged();
        }

        private void inputIgnoreOverlapTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if(Filtering == null) {
                return;
            }

            Filtering.IgnoreOverlapTime = TimeSpan.FromSeconds(this.inputIgnoreOverlapTime.Value.GetValueOrDefault());
            e.Handled = true;

            OnFilteringChanged();
        }

        private void DefinedItemCheckBox_Checked_Unchecked(object sender, RoutedEventArgs e)
        {
            if(Filtering == null) {
                return;
            }

            var checkbox = (CheckBox)sender;
            var isChecked = checkbox.IsChecked.GetValueOrDefault();
            var element = checkbox.DataContext as SmileVideoCommentFilteringElementViewModel;
            if(element == null) {
                return;
            }

            var settingIndex = Filtering.DefineKeys.IndexOf(element.Key);
            if(isChecked) {
                if(settingIndex == -1) {
                    Filtering.DefineKeys.Add(element.Key);
                }
            } else {
                if(settingIndex != -1) {
                    Filtering.DefineKeys.RemoveAt(settingIndex);
                }
            }

            e.Handled = true;
            OnFilteringChanged();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChangedCore(sender, e);
        }

        private void ListBoxCheckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChangedCore(sender, e);
        }

        private void CheckBox_Checked_UnChecked(object sender, RoutedEventArgs e)
        {
            CheckBoxChangedCore(sender, e);
        }
    }
}
