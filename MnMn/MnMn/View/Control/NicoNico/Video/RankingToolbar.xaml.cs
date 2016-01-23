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
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video;

namespace ContentTypeTextNet.MnMn.MnMn.View.Control.NicoNico.Video
{
    /// <summary>
    /// RankingToolbar.xaml の相互作用ロジック
    /// </summary>
    public partial class RankingToolbar: UserControl
    {
        public RankingToolbar()
        {
            InitializeComponent();
        }

        #region CommandProperty

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(CommandProperty)),
            typeof(ICommand),
            typeof(RankingToolbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCommandChanged))
        );

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RankingToolbar;
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

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(CommandParameterProperty)),
            typeof(object),
            typeof(RankingToolbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCommandParameterChanged))
        );

        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RankingToolbar;
            control.CommandParameter = e.NewValue;
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        #endregion

        #region SelectedCategoryProperty

        public static readonly DependencyProperty SelectedCategoryProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedCategoryProperty)),
            typeof(ElementModel),
            typeof(RankingToolbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedCategoryChanged))
        );

        private static void OnSelectedCategoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RankingToolbar;
            if(control != null) {
                control.SelectedCategory = e.NewValue as ElementModel;
            }
        }

        public ElementModel SelectedCategory
        {
            get { return GetValue(SelectedCategoryProperty) as ElementModel; }
            set { SetValue(SelectedCategoryProperty, value); }
        }

        #endregion

        #region RankingToolbar

        #region SelectedPeriodProperty

        public static readonly DependencyProperty SelectedPeriodProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedPeriodProperty)),
            typeof(ElementModel),
            typeof(RankingToolbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedPeriodChanged))
        );

        private static void OnSelectedPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RankingToolbar;
            if(control != null) {
                control.SelectedPeriod = e.NewValue as ElementModel;
            }
        }

        public ElementModel SelectedPeriod
        {
            get { return GetValue(SelectedPeriodProperty) as ElementModel; }
            set { SetValue(SelectedPeriodProperty, value); }
        }

        #endregion

        #region SelectedTargetProperty

        public static readonly DependencyProperty SelectedTargetProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedTargetProperty)),
            typeof(ElementModel),
            typeof(RankingToolbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedTargetChanged))
        );

        private static void OnSelectedTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RankingToolbar;
            if(control != null) {
                control.SelectedTarget = e.NewValue as ElementModel;
            }
        }

        public ElementModel SelectedTarget
        {
            get { return GetValue(SelectedTargetProperty) as ElementModel; }
            set { SetValue(SelectedTargetProperty, value); }
        }

        #endregion

        #region PeriodItemsSourceProperty

        public static readonly DependencyProperty PeriodItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(PeriodItemsSourceProperty)),
            typeof(ObservableCollection<ElementModel>),
            typeof(RankingToolbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTargetItemsSourceChanged))
        );

        private static void OnPeriodItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<RankingToolbar>(d, control => {
                control.PeriodItemsSource = e.NewValue as ObservableCollection<ElementModel>;
            });
        }

        public ObservableCollection<ElementModel> PeriodItemsSource
        {
            get { return GetValue(PeriodItemsSourceProperty) as ObservableCollection<ElementModel>; }
            set { SetValue(PeriodItemsSourceProperty, value); }
        }

        #endregion

        #region TargetItemsSourceProperty

        public static readonly DependencyProperty TargetItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(TargetItemsSourceProperty)),
            typeof(ObservableCollection<ElementModel>),
            typeof(RankingToolbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTargetItemsSourceChanged))
        );

        private static void OnTargetItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<RankingToolbar>(d, control => {
                control.TargetItemsSource = e.NewValue as ObservableCollection<ElementModel>;
            });
        }

        public ObservableCollection<ElementModel> TargetItemsSource
        {
            get { return GetValue(TargetItemsSourceProperty) as ObservableCollection<ElementModel>; }
            set { SetValue(TargetItemsSourceProperty, value); }
        }

        #endregion

        #endregion
    }
}
