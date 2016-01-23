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

namespace ContentTypeTextNet.MnMn.MnMn.View.Control.NicoNico.Video
{
    /// <summary>
    /// VideoList.xaml の相互作用ロジック
    /// </summary>
    public partial class VideoList: UserControl
    {
        public VideoList()
        {
            InitializeComponent();
        }

        #region PeriodItemsSourceProperty

        public static readonly DependencyProperty VideoItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(VideoItemsSourceProperty)),
            typeof(ObservableCollection<object>),
            typeof(VideoList),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnVideoItemsSourceChanged))
        );

        private static void OnVideoItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<VideoList>(d, control => {
                control.PeriodItemsSource = e.NewValue as ObservableCollection<object>;
            });
        }

        public ObservableCollection<object> PeriodItemsSource
        {
            get { return GetValue(VideoItemsSourceProperty) as ObservableCollection<object>; }
            set { SetValue(VideoItemsSourceProperty, value); }
        }

        #endregion
    }
}
