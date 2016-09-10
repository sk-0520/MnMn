using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
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

        #region IsMuteProperty

        public static readonly DependencyProperty IsMuteProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsMuteProperty)),
            typeof(bool),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsMuteChanged))
        );

        private static void OnIsMuteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.IsMute = (bool)e.NewValue;
            }
        }

        public bool IsMute
        {
            get { return (bool)GetValue(IsMuteProperty); }
            set { SetValue(IsMuteProperty, value); }
        }

        #endregion

        #region IsAutoPlayProperty

        public static readonly DependencyProperty IsAutoPlayProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsAutoPlayProperty)),
            typeof(bool),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsAutoPlayChanged))
        );

        private static void OnIsAutoPlayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.IsAutoPlay = (bool)e.NewValue;
            }
        }

        public bool IsAutoPlay
        {
            get { return (bool)GetValue(IsAutoPlayProperty); }
            set { SetValue(IsAutoPlayProperty, value); }
        }

        #endregion

        #region WaitingFirstPlayProperty

        // smile-video側都合でなんつーかやだなー。
        public static readonly DependencyProperty WaitingFirstPlayProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(WaitingFirstPlayProperty)),
            typeof(bool),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnWaitingFirstPlayChanged))
        );

        private static void OnWaitingFirstPlayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.WaitingFirstPlay = (bool)e.NewValue;
            }
        }

        public bool WaitingFirstPlay
        {
            get { return (bool)GetValue(WaitingFirstPlayProperty); }
            set { SetValue(WaitingFirstPlayProperty, value); }
        }

        #endregion

        #region IsUserOperationStopProperty

        // smile-video側都合でなんつーかやだなー。
        public static readonly DependencyProperty IsUserOperationStopProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsUserOperationStopProperty)),
            typeof(bool),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsUserOperationStopChanged))
        );

        private static void OnIsUserOperationStopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.IsUserOperationStop = (bool)e.NewValue;
            }
        }

        public bool IsUserOperationStop
        {
            get { return (bool)GetValue(IsUserOperationStopProperty); }
            set { SetValue(IsUserOperationStopProperty, value); }
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

        #region IsReplayCheckedProperty

        public static readonly DependencyProperty IsReplayCheckedProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsReplayCheckedProperty)),
            typeof(bool),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsReplayCheckedChanged))
        );

        private static void OnIsReplayCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.IsReplayChecked = (bool)e.NewValue;
            }
        }

        public bool IsReplayChecked
        {
            get { return (bool)GetValue(IsReplayCheckedProperty); }
            set { SetValue(IsReplayCheckedProperty, value); }
        }

        #endregion

        #region PlayerStateProperty

        public static readonly DependencyProperty PlayerStateProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(PlayerStateProperty)),
            typeof(PlayerState),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(default(PlayerState), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnPlayerStateChanged))
        );

        private static void OnPlayerStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.PlayerState = (PlayerState)e.NewValue;
            }
        }

        public PlayerState PlayerState
        {
            get { return (PlayerState)GetValue(PlayerStateProperty); }
            set { SetValue(PlayerStateProperty, value); }
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

        #region ReplayCommand

        #region ReplayCommandProperty

        public static readonly DependencyProperty ReplayCommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(ReplayCommandProperty)),
            typeof(ICommand),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnReplayCommandChanged))
        );

        private static void OnReplayCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.ReplayCommand = e.NewValue as ICommand;
            }
        }

        public ICommand ReplayCommand
        {
            get { return GetValue(ReplayCommandProperty) as ICommand; }
            set { SetValue(ReplayCommandProperty, value); }
        }

        #endregion

        #region ReplayCommandParameterProperty

        public static readonly DependencyProperty ReplayCommandParameterProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(ReplayCommandParameterProperty)),
            typeof(object),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnReplayCommandParameterChanged))
        );

        private static void OnReplayCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.ReplayCommandParameter = e.NewValue;
            }
        }

        public object ReplayCommandParameter
        {
            get { return GetValue(ReplayCommandParameterProperty); }
            set { SetValue(ReplayCommandParameterProperty, value); }
        }

        #endregion

        #endregion

        #region ExstendsContentProperty

        public static readonly DependencyProperty ExstendsContentProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(ExstendsContentProperty)),
            typeof(object),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnExstendsContentChanged))
        );

        private static void OnExstendsContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.ExstendsContent = e.NewValue;
            }
        }

        public object ExstendsContent
        {
            get { return GetValue(ExstendsContentProperty); }
            set { SetValue(ExstendsContentProperty, value); }
        }

        #endregion

        #region SeekbarPopupContentProperty

        public static readonly DependencyProperty SeekbarPopupContentProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SeekbarPopupContentProperty)),
            typeof(object),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSeekbarPopupContentChanged))
        );

        private static void OnSeekbarPopupContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.SeekbarPopupContent = e.NewValue;
            }
        }

        public object SeekbarPopupContent
        {
            get { return GetValue(SeekbarPopupContentProperty); }
            set { SetValue(SeekbarPopupContentProperty, value); }
        }

        #endregion

        #region SeekbarPopupIsOpenProperty

        public static readonly DependencyProperty SeekbarPopupIsOpenProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SeekbarPopupIsOpenProperty)),
            typeof(bool),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSeekbarPopupIsOpenChanged))
        );

        private static void OnSeekbarPopupIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.SeekbarPopupIsOpen = (bool)e.NewValue;
            }
        }

        public bool SeekbarPopupIsOpen
        {
            get { return (bool)GetValue(SeekbarPopupIsOpenProperty); }
            set { SetValue(SeekbarPopupIsOpenProperty, value); }
        }

        #endregion

        #region SeekbarPopupPlacementProperty

        public static readonly DependencyProperty SeekbarPopupPlacementProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SeekbarPopupPlacementProperty)),
            typeof(PlacementMode),
            typeof(Navigationbar),
            new FrameworkPropertyMetadata(PlacementMode.Center, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSeekbarPopupPlacementChanged))
        );

        private static void OnSeekbarPopupPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigationbar;
            if(control != null) {
                control.SeekbarPopupPlacement = (PlacementMode)e.NewValue;
            }
        }

        public PlacementMode SeekbarPopupPlacement
        {
            get { return (PlacementMode)GetValue(SeekbarPopupPlacementProperty); }
            set { SetValue(SeekbarPopupPlacementProperty, value); }
        }

        #endregion

        #region function
        #endregion
    }
}
