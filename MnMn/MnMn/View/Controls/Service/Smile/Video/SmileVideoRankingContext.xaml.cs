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
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video
{
    /// <summary>
    /// SmileVideoRankingContext.xaml の相互作用ロジック
    /// </summary>
    public partial class SmileVideoRankingContext: UserControl
    {
        public SmileVideoRankingContext()
        {
            InitializeComponent();
        }

        #region SelectedPeriodProperty

        public static readonly DependencyProperty SelectedPeriodProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedPeriodProperty)),
            typeof(DefinedElementModel),
            typeof(SmileVideoRankingContext),
            new FrameworkPropertyMetadata(default(DefinedElementModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedPeriodChanged))
        );

        private static void OnSelectedPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoRankingContext;
            if(control != null) {
                control.SelectedPeriod = e.NewValue as DefinedElementModel;
            }
        }

        public DefinedElementModel SelectedPeriod
        {
            get { return GetValue(SelectedPeriodProperty) as DefinedElementModel; }
            set { SetValue(SelectedPeriodProperty, value); }
        }

        #endregion
        
        #region SelectedTargetProperty

        public static readonly DependencyProperty SelectedTargetProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SelectedTargetProperty)),
            typeof(DefinedElementModel),
            typeof(SmileVideoRankingContext),
            new FrameworkPropertyMetadata(default(DefinedElementModel), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedTargetChanged))
        );

        private static void OnSelectedTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoRankingContext;
            if(control != null) {
                control.SelectedTarget = e.NewValue as DefinedElementModel;
            }
        }

        public DefinedElementModel SelectedTarget
        {
            get { return GetValue(SelectedTargetProperty) as DefinedElementModel; }
            set { SetValue(SelectedTargetProperty, value); }
        }

        #endregion

        #region PeriodItemsSourceProperty

        public static readonly DependencyProperty PeriodItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(PeriodItemsSourceProperty)),
            typeof(ObservableCollection<DefinedElementModel>),
            typeof(SmileVideoRankingContext),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPeriodItemsSourceChanged))
        );

        private static void OnPeriodItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileVideoRankingContext>(d, control => {
                control.PeriodItemsSource = e.NewValue as ObservableCollection<DefinedElementModel>;
            });
        }

        public ObservableCollection<DefinedElementModel> PeriodItemsSource
        {
            get { return GetValue(PeriodItemsSourceProperty) as ObservableCollection<DefinedElementModel>; }
            set { SetValue(PeriodItemsSourceProperty, value); }
        }

        #endregion

        #region TargetItemsSourceProperty

        public static readonly DependencyProperty TargetItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(TargetItemsSourceProperty)),
            typeof(ObservableCollection<DefinedElementModel>),
            typeof(SmileVideoRankingContext),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTargetItemsSourceChanged))
        );

        private static void OnTargetItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<SmileVideoRankingContext>(d, control => {
                control.TargetItemsSource = e.NewValue as ObservableCollection<DefinedElementModel>;
            });
        }

        public ObservableCollection<DefinedElementModel> TargetItemsSource
        {
            get { return GetValue(TargetItemsSourceProperty) as ObservableCollection<DefinedElementModel>; }
            set { SetValue(TargetItemsSourceProperty, value); }
        }

        #endregion

    }
}
