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
    public partial class RankingContextElements: UserControl
    {
        public RankingContextElements()
        {
            InitializeComponent();
        }

        #region SelectedPeriodProperty

        public static readonly DependencyProperty SelectedPeriodProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedPeriodProperty)),
            typeof(ElementModel),
            typeof(RankingContextElements),
            new FrameworkPropertyMetadata(default(ElementModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedPeriodChanged))
        );

        private static void OnSelectedPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RankingContextElements;
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
            typeof(RankingContextElements),
            new FrameworkPropertyMetadata(default(ElementModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedTargetChanged))
        );

        private static void OnSelectedTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RankingContextElements;
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
            typeof(RankingContextElements),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPeriodItemsSourceChanged))
        );

        private static void OnPeriodItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<RankingContextElements>(d, control => {
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
            typeof(RankingContextElements),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTargetItemsSourceChanged))
        );

        private static void OnTargetItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<RankingContextElements>(d, control => {
                control.TargetItemsSource = e.NewValue as ObservableCollection<ElementModel>;
            });
        }

        public ObservableCollection<ElementModel> TargetItemsSource
        {
            get { return GetValue(TargetItemsSourceProperty) as ObservableCollection<ElementModel>; }
            set { SetValue(TargetItemsSourceProperty, value); }
        }

        #endregion

    }
}
