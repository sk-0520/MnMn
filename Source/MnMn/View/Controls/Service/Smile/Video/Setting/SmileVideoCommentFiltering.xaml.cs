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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
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
            new FrameworkPropertyMetadata(default(MVMPairCollectionBase<SmileVideoCommentFilteringItemSettingModel, SmileVideoCommentFilteringItemEditViewModel>), new PropertyChangedCallback(OnFilteringChanged))
        );

        private static void OnFilteringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileVideoCommentFiltering>(d, control => {
                control.Filtering = e.NewValue as SmileVideoFilteringViweModel;
                //control.FilteringViewModelItemsSource = control.Filtering.CommentFilterList.ViewModelList;
                control.selectIgnoreOverlapWord.IsChecked = control.Filtering.IgnoreOverlapWord;
                control.inputIgnoreOverlapTime.Value = (int)control.Filtering.IgnoreOverlapTime.TotalSeconds;
                var defineItems = control.Filtering.CommentDefineItems
                    .Select(de => new SmileVideoCommentFilteringElementViewModel(de))
                    .ToEvaluatedSequence()
                ;
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

        #region function

        void SelectionChangedCore(object sender, SelectionChangedEventArgs e)
        {
            if(Filtering == null) {
                return;
            }

            e.Handled = true;
            OnFilteringChanged();
        }

        void CheckBoxChangedCore(object sender, RoutedEventArgs e)
        {
            if(Filtering == null) {
                return;
            }

            e.Handled = true;
            OnFilteringChanged();
        }

        void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if(Filtering == null) {
                return;
            }

            var textBox = (TextBox)sender;

            var editItem = textBox.DataContext as SmileVideoCommentFilteringItemEditViewModel;
            editItem.Source = textBox.Text;

            e.Handled = true;
            OnFilteringChanged();
        }

        void ButtonClick(object sender, RoutedEventArgs e)
        {
            if(Filtering == null) {
                return;
            }

            var button = (Button)sender;
            Filtering.RemoveCommentFilter((SmileVideoCommentFilteringItemEditViewModel)button.DataContext);

            e.Handled = true;
            OnFilteringChanged();
        }

        #endregion

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxTextChanged(sender, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick(sender, e);
        }

        private void AddFilter_Click(object sender, RoutedEventArgs e)
        {
            this.scrollFiterItems.ScrollToEnd();
        }
    }
}
