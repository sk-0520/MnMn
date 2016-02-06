using System;
using System.Collections.Generic;
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
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// Seekbar.xaml の相互作用ロジック
    /// </summary>
    public partial class Seekbar: UserControl
    {
        public Seekbar()
        {
            InitializeComponent();
        }

        #region VideoPositionProperty

        public static readonly DependencyProperty VideoPositionProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(VideoPositionProperty)),
            typeof(float),
            typeof(Seekbar),
            new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVideoPositionChanged))
        );

        private static void OnVideoPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Seekbar;
            if(control != null) {
                control.VideoPosition = (float)e.NewValue;
            }
        }

        public float VideoPosition
        {
            get { return (float)GetValue(VideoPositionProperty); }
            set { SetValue(VideoPositionProperty, value); }
        }

        #endregion

        #region VideoTotalSizeProperty

        public static readonly DependencyProperty VideoTotalSizeProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(VideoTotalSizeProperty)),
            typeof(long),
            typeof(Seekbar),
            new FrameworkPropertyMetadata(default(long), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVideoTotalSizeChanged))
        );

        private static void OnVideoTotalSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Seekbar;
            if(control != null) {
                control.VideoTotalSize = (long)e.NewValue;
            }
        }

        public long VideoTotalSize
        {
            get { return (long)GetValue(VideoTotalSizeProperty); }
            set { SetValue(VideoTotalSizeProperty, value); }
        }

        #endregion

        #region VideoLoadedSizeProperty

        public static readonly DependencyProperty VideoLoadedSizeProperty = DependencyProperty.Register(
            nameof(VideoLoadedSize),
            typeof(long),
            typeof(Seekbar),
            new FrameworkPropertyMetadata(default(long), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVideoLoadedSizeChanged))
        );

        private static void OnVideoLoadedSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Seekbar;
            if(control != null) {
                control.VideoLoadedSize = (long)e.NewValue;
            }
        }

        public long VideoLoadedSize
        {
            get { return (long)GetValue(VideoLoadedSizeProperty); }
            set { SetValue(VideoLoadedSizeProperty, value); }
        }

        #endregion

        #region VolumeProperty

        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(VolumeProperty)),
            typeof(int),
            typeof(Seekbar),
            new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVolumeChanged))
        );

        private static void OnVolumeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Seekbar;
            if(control != null) {
                control.Volume = (int)e.NewValue;
            }
        }

        public int Volume
        {
            get { return (int)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        #endregion

        #region VideoLoadingForegroundProperty

        public static readonly DependencyProperty VideoLoadingForegroundProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(VideoLoadingForegroundProperty)),
            typeof(Brush),
            typeof(Seekbar),
            new FrameworkPropertyMetadata(Brushes.Lime, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVideoLoadingForegroundChanged))
        );

        private static void OnVideoLoadingForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Seekbar;
            if(control != null) {
                control.VideoLoadingForeground = e.NewValue as Brush;
            }
        }

        public Brush VideoLoadingForeground
        {
            get { return (Brush)GetValue(VideoLoadingForegroundProperty); }
            set { SetValue(VideoLoadingForegroundProperty, value); }
        }

        #endregion

        #region VideoLoadingBackgroundProperty

        public static readonly DependencyProperty VideoLoadingBackgroundProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(VideoLoadingBackgroundProperty)),
            typeof(Brush),
            typeof(Seekbar),
            new FrameworkPropertyMetadata(Brushes.Gray, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVideoLoadingBackgroundChanged))
        );

        private static void OnVideoLoadingBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Seekbar;
            if(control != null) {
                control.VideoLoadingBackground = e.NewValue as Brush;
            }
        }

        public Brush VideoLoadingBackground
        {
            get { return (Brush)GetValue(VideoLoadingBackgroundProperty); }
            set { SetValue(VideoLoadingBackgroundProperty, value); }
        }

        #endregion
    }
}
