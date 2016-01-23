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
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico.Video;

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

        #region VideoInformationItemsSourceProperty

        public static readonly DependencyProperty VideoInformationItemsSourceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(VideoInformationItemsSourceProperty)),
            typeof(ObservableCollection<VideoInformationViewModel>),
            typeof(VideoList),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnVideoInformationItemsSourceChanged))
        );

        private static void OnVideoInformationItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CastUtility.AsAction<VideoList>(d, control => {
                control.VideoInformationItemsSource = e.NewValue as ObservableCollection<VideoInformationViewModel>;
            });
        }

        public ObservableCollection<VideoInformationViewModel> VideoInformationItemsSource
        {
            get { return GetValue(VideoInformationItemsSourceProperty) as ObservableCollection<VideoInformationViewModel>; }
            set { SetValue(VideoInformationItemsSourceProperty, value); }
        }

        #endregion
    }
}
