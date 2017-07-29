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

            var editItem = textBox.DataContext as SmileVideoFinderFilteringItemEditViewModel;
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
            Filtering.RemoveFinderFilter((SmileVideoFinderFilteringItemEditViewModel)button.DataContext);

            e.Handled = true;
            OnFilteringChanged();
        }


        #endregion

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

        private void RemoveFilter_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick(sender, e);
        }

        private void AddFilter_Click(object sender, RoutedEventArgs e)
        {
            this.scrollFiterItems.ScrollToEnd();
        }
    }
}
