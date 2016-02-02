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

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.NicoNico.Video
{
    /// <summary>
    /// NicoNicoVideoRankingContextElements.xaml の相互作用ロジック
    /// </summary>
    public partial class NicoNicoVideoRankingContextElements: UserControl
    {
        public NicoNicoVideoRankingContextElements()
        {
            InitializeComponent();
        }

        #region SelectedPeriodProperty

        public static readonly DependencyProperty SelectedPeriodProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedPeriodProperty)),
            typeof(NicoNicoVideoElementModel),
            typeof(NicoNicoVideoRankingContextElements),
            new FrameworkPropertyMetadata(default(NicoNicoVideoElementModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedPeriodChanged))
        );

        private static void OnSelectedPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as NicoNicoVideoRankingContextElements;
            if(control != null) {
                control.SelectedPeriod = e.NewValue as NicoNicoVideoElementModel;
            }
        }

        public NicoNicoVideoElementModel SelectedPeriod
        {
            get { return GetValue(SelectedPeriodProperty) as NicoNicoVideoElementModel; }
            set { SetValue(SelectedPeriodProperty, value); }
        }

        #endregion
        
        #region SelectedTargetProperty

        public static readonly DependencyProperty SelectedTargetProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedTargetProperty)),
            typeof(NicoNicoVideoElementModel),
            typeof(NicoNicoVideoRankingContextElements),
            new FrameworkPropertyMetadata(default(NicoNicoVideoElementModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedTargetChanged))
        );

        private static void OnSelectedTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as NicoNicoVideoRankingContextElements;
            if(control != null) {
                control.SelectedTarget = e.NewValue as NicoNicoVideoElementModel;
            }
        }

        public NicoNicoVideoElementModel SelectedTarget
        {
            get { return GetValue(SelectedTargetProperty) as NicoNicoVideoElementModel; }
            set { SetValue(SelectedTargetProperty, value); }
        }

        #endregion

        #region PeriodItemsSourceProperty

        public static readonly DependencyProperty PeriodItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(PeriodItemsSourceProperty)),
            typeof(ObservableCollection<NicoNicoVideoElementModel>),
            typeof(NicoNicoVideoRankingContextElements),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPeriodItemsSourceChanged))
        );

        private static void OnPeriodItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<NicoNicoVideoRankingContextElements>(d, control => {
                control.PeriodItemsSource = e.NewValue as ObservableCollection<NicoNicoVideoElementModel>;
            });
        }

        public ObservableCollection<NicoNicoVideoElementModel> PeriodItemsSource
        {
            get { return GetValue(PeriodItemsSourceProperty) as ObservableCollection<NicoNicoVideoElementModel>; }
            set { SetValue(PeriodItemsSourceProperty, value); }
        }

        #endregion

        #region TargetItemsSourceProperty

        public static readonly DependencyProperty TargetItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(TargetItemsSourceProperty)),
            typeof(ObservableCollection<NicoNicoVideoElementModel>),
            typeof(NicoNicoVideoRankingContextElements),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTargetItemsSourceChanged))
        );

        private static void OnTargetItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<NicoNicoVideoRankingContextElements>(d, control => {
                control.TargetItemsSource = e.NewValue as ObservableCollection<NicoNicoVideoElementModel>;
            });
        }

        public ObservableCollection<NicoNicoVideoElementModel> TargetItemsSource
        {
            get { return GetValue(TargetItemsSourceProperty) as ObservableCollection<NicoNicoVideoElementModel>; }
            set { SetValue(TargetItemsSourceProperty, value); }
        }

        #endregion

    }
}
