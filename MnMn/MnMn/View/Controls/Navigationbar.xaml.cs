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
    /// Navigationbar.xaml の相互作用ロジック
    /// </summary>
    public partial class Navigationbar: UserControl
    {
        public Navigationbar()
        {
            InitializeComponent();
        }

        #region VideoPositionProperty

        public static readonly DependencyProperty VideoPositionProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(VideoPositionProperty)),
            typeof(float),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(default(float), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVideoPositionChanged))
        );

        private static void OnVideoPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
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
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(default(long), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVideoTotalSizeChanged))
        );

        private static void OnVideoTotalSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
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
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(default(long), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVideoLoadedSizeChanged))
        );

        private static void OnVideoLoadedSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
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
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVolumeChanged))
        );

        private static void OnVolumeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
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
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(Brushes.Lime, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVideoLoadingForegroundChanged))
        );

        private static void OnVideoLoadingForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.VideoLoadingForeground = e.NewValue as Brush;
            }
        }

        public Brush VideoLoadingForeground
        {
            get { return GetValue(VideoLoadingForegroundProperty) as Brush; }
            set { SetValue(VideoLoadingForegroundProperty, value); }
        }

        #endregion

        #region VideoLoadingBackgroundProperty

        public static readonly DependencyProperty VideoLoadingBackgroundProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(VideoLoadingBackgroundProperty)),
            typeof(Brush),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(Brushes.Gray, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnVideoLoadingBackgroundChanged))
        );

        private static void OnVideoLoadingBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.VideoLoadingBackground = e.NewValue as Brush;
            }
        }

        public Brush VideoLoadingBackground
        {
            get { return GetValue(VideoLoadingBackgroundProperty) as Brush; }
            set { SetValue(VideoLoadingBackgroundProperty, value); }
        }

        #endregion

        #region CanSeekProperty

        public static readonly DependencyProperty CanSeekProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(CanSeekProperty)),
            typeof(bool),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnCanSeekChanged))
        );

        private static void OnCanSeekChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.CanSeek = (bool)e.NewValue;
            }
        }

        public bool CanSeek
        {
            get { return (bool)GetValue(CanSeekProperty); }
            set { SetValue(CanSeekProperty, value); }
        }

        #endregion

        #region TotalTimeProperty

        public static readonly DependencyProperty TotalTimeProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(TotalTimeProperty)),
            typeof(TimeSpan),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(default(TimeSpan), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTotalTimeChanged))
        );

        private static void OnTotalTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.TotalTime = (TimeSpan)e.NewValue;
            }
        }

        public TimeSpan TotalTime
        {
            get { return (TimeSpan)GetValue(TotalTimeProperty); }
            set { SetValue(TotalTimeProperty, value); }
        }

        #endregion

        #region PlayTimeProperty

        public static readonly DependencyProperty PlayTimeProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(PlayTimeProperty)),
            typeof(TimeSpan),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(default(TimeSpan), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnPlayTimeChanged))
        );

        private static void OnPlayTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.PlayTime = (TimeSpan)e.NewValue;
            }
        }

        public TimeSpan PlayTime
        {
            get { return (TimeSpan)GetValue(PlayTimeProperty); }
            set { SetValue(PlayTimeProperty, value); }
        }

        #endregion

        #region PlayCommand

        #region PlayCommandProperty

        public static readonly DependencyProperty PlayCommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(PlayCommandProperty)),
            typeof(ICommand),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPlayCommandChanged))
        );

        private static void OnPlayCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.PlayCommand = e.NewValue as ICommand;
            }
        }

        public ICommand PlayCommand
        {
            get { return GetValue(PlayCommandProperty) as ICommand; }
            set { SetValue(PlayCommandProperty, value); }
        }

        #endregion

        #region PlayCommandParameterProperty

        public static readonly DependencyProperty PlayCommandParameterProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(PlayCommandParameterProperty)),
            typeof(object),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPlayCommandParameterChanged))
        );

        private static void OnPlayCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.PlayCommandParameter = e.NewValue;
            }
        }

        public object PlayCommandParameter
        {
            get { return GetValue(PlayCommandParameterProperty); }
            set { SetValue(PlayCommandParameterProperty, value); }
        }

        #endregion

        #endregion

        #region StopCommand

        #region StopCommandProperty

        public static readonly DependencyProperty StopCommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(StopCommandProperty)),
            typeof(ICommand),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnStopCommandChanged))
        );

        private static void OnStopCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.StopCommand = e.NewValue as ICommand;
            }
        }

        public ICommand StopCommand
        {
            get { return GetValue(StopCommandProperty) as ICommand; }
            set { SetValue(StopCommandProperty, value); }
        }

        #endregion

        #region StopCommandParameterProperty

        public static readonly DependencyProperty StopCommandParameterProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(StopCommandParameterProperty)),
            typeof(object),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnStopCommandParameterChanged))
        );

        private static void OnStopCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.StopCommandParameter = e.NewValue;
            }
        }

        public object StopCommandParameter
        {
            get { return GetValue(StopCommandParameterProperty); }
            set { SetValue(StopCommandParameterProperty, value); }
        }

        #endregion

        #endregion

        #region MuteCommand

        #region MuteCommandProperty

        public static readonly DependencyProperty MuteCommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(MuteCommandProperty)),
            typeof(ICommand),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnMuteCommandChanged))
        );

        private static void OnMuteCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.MuteCommand = e.NewValue as ICommand;
            }
        }

        public ICommand MuteCommand
        {
            get { return GetValue(MuteCommandProperty) as ICommand; }
            set { SetValue(MuteCommandProperty, value); }
        }

        #endregion

        #region MuteCommandParameterProperty

        public static readonly DependencyProperty MuteCommandParameterProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(MuteCommandParameterProperty)),
            typeof(object),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnMuteCommandParameterChanged))
        );

        private static void OnMuteCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.MuteCommandParameter = e.NewValue;
            }
        }

        public object MuteCommandParameter
        {
            get { return GetValue(MuteCommandParameterProperty); }
            set { SetValue(MuteCommandParameterProperty, value); }
        }

        #endregion

        #endregion

    }
}
