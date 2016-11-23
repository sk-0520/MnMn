/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using System.Windows.Media;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using System.Windows;
using ContentTypeTextNet.MnMn.MnMn.Define.Event;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using System.Windows.Media.Animation;
using System.Windows.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using System.Globalization;
using System.Windows.Media.Effects;
using System.ComponentModel;
using System.Windows.Documents;
using HtmlAgilityPack;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using Meta.Vlc.Wpf;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using HTMLConverter;
using System.Windows.Markup;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video.Parameter;
using Package.stackoverflow.com;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;
using System.Xml;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Logic.Wrapper;
using System.Windows.Threading;
using System.Runtime.ExceptionServices;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;
using System.Windows.Controls.Primitives;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using OxyPlot;
using OxyPlot.Wpf;
using ContentTypeTextNet.MnMn.MnMn.Model.Order;
using ContentTypeTextNet.MnMn.MnMn.Model.MultiCommandParameter.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    /// <summary>
    /// プレイヤー管理。
    /// </summary>
    public partial class SmileVideoPlayerViewModel: SmileVideoDownloadViewModel, ISetView, ISmileDescription, ICloseView
    {
        #region define

        //static readonly Size BaseSize_4x3 = new Size(640, 480);
        //static readonly Size BaseSize_16x9 = new Size(512, 384);

        const float initPrevStateChangedPosition = -1;

        //static readonly Thickness enabledResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
        //static readonly Thickness maximumWindowBorderThickness = SystemParameters.WindowResizeBorderThickness;
        //static readonly Thickness normalWindowBorderThickness = new Thickness(1);

        #endregion

        public SmileVideoPlayerViewModel(Mediation mediation)
            : base(mediation)
        {
            CommentItems = CollectionViewSource.GetDefaultView(CommentList);
            CommentItems.Filter = FilterCommentItems;

            var filteringResult = Mediation.GetResultFromRequest<SmileVideoFilteringResultModel>(new SmileVideoCustomSettingRequestModel(SmileVideoCustomSettingKind.CommentFiltering));
            GlobalCommentFilering = filteringResult.Filtering;

            ImportSetting();
        }

        #region command

        public ICommand OpenLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        Mediation.Logger.Warning($"{o}");
                    }
                );
            }
        }

        public ICommand OpenRelationVideo
        {
            get
            {
                return CreateCommand(
                    o => {
                        var videoInformation = (SmileVideoInformationViewModel)o;
                        LoadAsync(videoInformation, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand PlayCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        switch(PlayerState) {
                            case PlayerState.Stop:
                                switch(Player.State) {
                                    case Meta.Vlc.Interop.Media.MediaState.NothingSpecial:
                                    case Meta.Vlc.Interop.Media.MediaState.Stopped:
                                        StopMovie(true);
                                        SetMedia();
                                        PlayMovie();
                                        break;

                                    default:
                                        //Player.BeginStop(() => {
                                        //    Player.Play();
                                        //});
                                        StopMovie(true);
                                        PlayMovie();
                                        break;
                                }
                                return;

                            case PlayerState.Playing:
                                Player.PauseOrResume();
                                return;

                            case PlayerState.Pause:
                                if(IsBufferingStop) {
                                    Mediation.Logger.Debug("restart");
                                    //SetMedia();
                                    //PlayMovie().Task.ContinueWith(task => {
                                    //    Player.Position = VideoPosition;
                                    //});
                                    PlayMovie();
                                    Player.Position = VideoPosition;
                                } else {
                                    Mediation.Logger.Debug("resume");
                                    View.Dispatcher.BeginInvoke(new Action(() => {
                                        Player.PauseOrResume();
                                    }));
                                }
                                return;

                            default:
                                break;
                        }
                    }
                );
            }
        }

        public ICommand StopCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        UserOperationStop.Value = true;
                        StopMovie(true);
                        //UserOperationStop = false;
                    }
                );
            }
        }

        public ICommand SearchTagCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var tagViewModel = (SmileVideoTagViewModel)o;

                        SearchTag(tagViewModel);
                    }
                );
            }
        }

        public ICommand OpenPediaCommand
        {
            get
            {
                return CreateCommand(o => {
                    var viewModel = (SmileVideoTagViewModel)o;
                    // TODO: 百科事典までサポートすんの？ だりぃぞー
                    //       今のところブラウザオープン(それも完全固定値)だけにする
                    var baseUri = new Uri("http://dic.nicovideo.jp/a/");
                    var uri = new Uri(baseUri, viewModel.TagName);
                    try {
                        Process.Start(uri.OriginalString);
                    } catch(Exception ex) {
                        Mediation.Logger.Warning(ex);
                    }
                });
            }
        }

        public ICommand OpenUserOrChannelIdCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(IsChannelVideo) {
                            Mediation.Logger.Debug("チャンネルを開く処理は未実装");
                        } else {
                            OpenUserId(UserId);
                        }
                    }
                );
            }
        }

        public ICommand OpenCacheDirectoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(Information.CacheDirectory.Exists) {
                            Process.Start(Information.CacheDirectory.FullName);
                        }
                    }
                );
            }
        }

        public ICommand AddMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var myListFinder = o as SmileVideoMyListFinderViewModelBase;
                        AddMyListAsync(myListFinder).ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand AddBookmarkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var bookmarkNode = o as SmileVideoBookmarkNodeViewModel;
                        AddBookmark(bookmarkNode);
                    }
                );
            }
        }

        public ICommand LoadSelectPlayListItemCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var selectVideoInformation = o as SmileVideoInformationViewModel;
                        LoadAsync(selectVideoInformation, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand LoadPrevPlayListItemCommand
        {
            get
            {
                return CreateCommand(
                    o => LoadPrevPlayListItemAsync().ConfigureAwait(false),
                    o => !IsRandomPlay && PlayListItems.CanItemChange
                );
            }
        }
        public ICommand LoadNextPlayListItemCommand
        {
            get
            {
                return CreateCommand(
                    o => LoadNextPlayListItemAsync().ConfigureAwait(false),
                    o => PlayListItems.CanItemChange
                );
            }
        }

        public ICommand ChangePlayerSizeFromPercentCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var percent = int.Parse((string)o);
                        ChangePlayerSizeFromPercent(percent);
                    },
                    o => !WaitingFirstPlay.Value
                );
            }
        }

        public ICommand ChangePlayerSizeFromOfficial4x3Command
        {
            get { return CreateCommand(o => ChangePlayerSizeFromOfficial4x3()); }
        }
        public ICommand ChangePlayerSizeFromOfficial16x9Command
        {
            get { return CreateCommand(o => ChangePlayerSizeFromOfficial16x9()); }
        }

        public ICommand ChangeFullScreenCommand
        {
            get { return CreateCommand(o => SetWindowMode(false)); }
        }

        public ICommand ResetPlayerAreaCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        SetAreaLength(
                            Constants.SettingServiceSmileVideoPlayerPlayerAreaStar,
                            Constants.SettingServiceSmileVideoPlayerCommentAreaStar,
                            Constants.SettingServiceSmileVideoPlayerInformationAreaPixel
                        );
                    }
                );
            }
        }

        public ICommand ChangedFilteringCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        ApprovalComment();
                    }
                );
            }
        }

        public ICommand ClearSelectedCommentCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        ClearSelectedComment();
                    }
                );
            }
        }

        public ICommand ResetCommentSettingCommand
        {
            get { return CreateCommand(o => ResetCommentSetting()); }
        }

        public ICommand PostCommentCommand
        {
            get { return CreateCommand(o => PostCommentAsync(PlayTime).ConfigureAwait(false)); }
        }

        public ICommand CloseCommentInformationCommand
        {
            get
            {
                return CreateCommand(o => {
                    CommentInformation = string.Empty;
                });
            }
        }

        public ICommand SwitchFullScreenCommand
        {
            get { return CreateCommand(o => SwitchFullScreen()); }
        }

        public ICommand PlayerMouseClickCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(Setting.Player.MoseClickToPause) {
                            PlayCommand.TryExecute(null);
                        }
                    }
                );
            }
        }

        public ICommand PlayerKeySpaceCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(Setting.Player.KeySpaceToPause) {
                            PlayCommand.TryExecute(null);
                        }
                    }
                );
            }
        }

        public ICommand FullScreenCancelCommand
        {
            get
            {
                return CreateCommand(o => {
                    if(IsNormalWindow) {
                        return;
                    }
                    // フルスクリーン時は元に戻してあげる
                    SetWindowMode(true);
                });
            }
        }

        public ICommand ShowSystemMenuCommand
        {
            get
            {
                return CreateCommand(o => {
                    var hWnd = HandleUtility.GetWindowHandle(View);
                    var _WM_SYSTEM_MENU = 0x313;
                    var devicePoint = MouseUtility.GetDevicePosition();
                    var desktopPoint = PodStructUtility.Convert(devicePoint);
                    var lParam = new IntPtr(desktopPoint.X | desktopPoint.Y << 16);
                    NativeMethods.PostMessage(hWnd, (WM)_WM_SYSTEM_MENU, IntPtr.Zero, lParam);
                });
            }
        }

        public ICommand ChangeSeekVideoPositionCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var param = (SmileVideoChangeVideoPositionCommandParameterModel)o;
                        Mediation.Logger.Information($"{param.PositionType}: {Setting.Player.SeekOperationAbsoluteStep}");
                        switch(param.PositionType) {
                            case SmileVideoChangeVideoPositionType.Setting:
                                ChangeSeekVideoPosition(param.IsNext, Setting.Player.SeekOperationIsPercent, Setting.Player.SeekOperationIsPercent ? Setting.Player.SeekOperationPercentStep : Setting.Player.SeekOperationAbsoluteStep);
                                break;

                            case SmileVideoChangeVideoPositionType.Percent:
                                ChangeSeekVideoPosition(param.IsNext, true, Setting.Player.SeekOperationPercentStep);
                                break;

                            case SmileVideoChangeVideoPositionType.Absolute:
                                ChangeSeekVideoPosition(param.IsNext, false, Setting.Player.SeekOperationAbsoluteStep);
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                    }
                );
            }
        }

        public ICommand ChangeVolumeCommand
        {
            get { return CreateCommand(o => ChangeVolume((bool)o)); }
        }

        public ICommand SetFilteringUserIdCommand
        {
            get
            {
                return CreateCommand(
                    o => AddCommentFilter(SmileVideoCommentFilteringTarget.UserId, SelectedComment.UserId, IsGlobalSettingFromKeyPushed()),
                    o => SelectedComment != null && !string.IsNullOrEmpty(SelectedComment.UserId)
                );
            }
        }

        public ICommand SetFilteringCommandCommand
        {
            get
            {
                return CreateCommand(
                    o => AddCommentFilter(SmileVideoCommentFilteringTarget.Command, (string)o, IsGlobalSettingFromKeyPushed()),
                    o => SelectedComment != null && !string.IsNullOrEmpty(o as string)
                );
            }
        }

        public ICommand SetFilteringCommentCommand
        {
            get
            {
                return CreateCommand(
                    o => AddCommentFilter(SmileVideoCommentFilteringTarget.Comment, SelectedComment.Content, IsGlobalSettingFromKeyPushed()),
                    o => SelectedComment != null && !string.IsNullOrEmpty(SelectedComment.Content)
                );
            }
        }

        #endregion

        #region function

        static bool IsGlobalSettingFromKeyPushed()
        {
            return Keyboard.Modifiers == ModifierKeys.Shift;
        }

        void SetAreaLength(double playerArea, double commentArea, double informationArea)
        {
            PlayerAreaLength.Value = new GridLength(playerArea, GridUnitType.Star);
            CommentAreaLength.Value = new GridLength(commentArea, GridUnitType.Star);
            InformationAreaLength.Value = new GridLength(informationArea, GridUnitType.Pixel);
        }

        void ImportSetting()
        {
            Left = Setting.Player.Window.Left;
            Top = Setting.Player.Window.Top;
            Width = Setting.Player.Window.Width;
            Height = Setting.Player.Window.Height;
            Topmost = Setting.Player.Window.Topmost;

            SetAreaLength(Setting.Player.PlayerArea, Setting.Player.CommentArea, Setting.Player.InformationArea);

            PlayerShowDetailArea = Setting.Player.ShowDetailArea;
            this._showNormalWindowCommentList = Setting.Player.ShowNormalWindowCommentList;
            this._showFullScreenCommentList = Setting.Player.ShowFullScreenCommentList;
            PlayerVisibleComment = Setting.Player.VisibleComment;
            IsAutoScroll = Setting.Player.AutoScrollCommentList;
            Volume = Setting.Player.Volume;
            IsMute = Setting.Player.IsMute;
            ReplayVideo = Setting.Player.ReplayVideo;
            IsEnabledDisplayCommentLimit = Setting.Player.IsEnabledDisplayCommentLimit;
            DisplayCommentLimitCount = Setting.Player.DisplayCommentLimitCount;

            IsEnabledSharedNoGood = Setting.Comment.IsEnabledSharedNoGood;
            SharedNoGoodScore = Setting.Comment.SharedNoGoodScore;
            CommentStyleSetting = (SmileVideoCommentStyleSettingModel)Setting.Comment.StyleSetting.DeepClone();
            IsEnabledOriginalPosterFilering = Setting.Comment.IsEnabledOriginalPosterFilering;
        }

        void ExportSetting()
        {
            Setting.Player.Window.Left = Left;
            Setting.Player.Window.Top = Top;
            Setting.Player.Window.Width = Width;
            Setting.Player.Window.Height = Height;
            Setting.Player.Window.Topmost = Topmost;

            Setting.Player.PlayerArea = PlayerAreaLength.Value.Value;
            Setting.Player.CommentArea = CommentAreaLength.Value.Value;
            Setting.Player.InformationArea = InformationAreaLength.Value.Value;

            Setting.Player.ShowDetailArea = PlayerShowDetailArea;
            Setting.Player.ShowNormalWindowCommentList = this._showNormalWindowCommentList;
            Setting.Player.ShowFullScreenCommentList = this._showFullScreenCommentList;
            Setting.Player.VisibleComment = PlayerVisibleComment;
            Setting.Player.AutoScrollCommentList = IsAutoScroll;
            Setting.Player.Volume = Volume;
            Setting.Player.IsMute = IsMute;
            Setting.Player.ReplayVideo = ReplayVideo;
            Setting.Player.IsEnabledDisplayCommentLimit = IsEnabledDisplayCommentLimit;
            Setting.Player.DisplayCommentLimitCount = DisplayCommentLimitCount;

            Setting.Comment.IsEnabledSharedNoGood = IsEnabledSharedNoGood;
            Setting.Comment.SharedNoGoodScore = SharedNoGoodScore;
            Setting.Comment.StyleSetting = (SmileVideoCommentStyleSettingModel)CommentStyleSetting.DeepClone();
            Setting.Comment.IsEnabledOriginalPosterFilering = IsEnabledOriginalPosterFilering;
        }

        public Task LoadAsync(IEnumerable<SmileVideoInformationViewModel> videoInformations, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            PlayListItems.AddRange(videoInformations);
            CanPlayNextVieo.Value = true;
            return LoadAsync(PlayListItems.GetFirstItem(), false, thumbCacheSpan, imageCacheSpan);
        }

        void ChangePlayerSizeFromPercent(int percent)
        {
            if(VisualVideoSize.IsEmpty) {
                return;
            }
            Debug.Assert(!WaitingFirstPlay.Value, "到達不可のはず");

            ChangePlayerSize(
                VisualVideoSize.Width / 100 * percent,
                VisualVideoSize.Height / 100 * percent
            );
        }

        void ChangePlayerSizeFromOfficial4x3()
        {
            ChangePlayerSize(
                Constants.ServiceSmileVideoPlayerOfficial4x3Width,
                Constants.ServiceSmileVideoPlayerOfficial4x3Height
            );
        }

        void ChangePlayerSizeFromOfficial16x9()
        {
            ChangePlayerSize(
                Constants.ServiceSmileVideoPlayerOfficial16x9Width,
                Constants.ServiceSmileVideoPlayerOfficial16x9Height
            );
        }

        void ChangePlayerSize(double width, double height)
        {
            var leaveSize = new Size(
                View.ActualWidth - Player.ActualWidth,
                View.ActualHeight - Player.ActualHeight
            );
            var videoSize = new Size(width, height);

            Width = leaveSize.Width + videoSize.Width;
            Height = leaveSize.Height + videoSize.Height;

            if(PlayerShowCommentArea) {
                // リスト部は比率レイアウトなので補正が必要

                // TODO: うーん、ださい
                var defaultGridSplitterLength = (double)Application.Current.Resources["GridSplitterLength"];

                leaveSize.Width = videoSize.Width / PlayerAreaLength.Value.Value * CommentAreaLength.Value.Value;
            }

            Width = leaveSize.Width + videoSize.Width;
            Height = leaveSize.Height + videoSize.Height;

        }

        void ChangeBaseSize()
        {
            if(RealVideoHeight <= 0 || RealVideoWidth <= 0) {
                BaseWidth = Player.ActualHeight;
                BaseHeight = Player.ActualWidth;
                return;
            } else if(WaitingFirstPlay.Value) {
                var desktopScale = UIUtility.GetDpiScale(Player);
                VisualVideoSize = new Size(
                    RealVideoWidth * desktopScale.X,
                    RealVideoHeight * desktopScale.Y
                );
            }

            var scale = new Point(
                Player.ActualWidth / VisualVideoSize.Width,
                Player.ActualHeight / VisualVideoSize.Height
            );
            var baseScale = Math.Min(scale.X, scale.Y);

            BaseWidth = VisualVideoSize.Width * baseScale;
            BaseHeight = VisualVideoSize.Height * baseScale;

            if(BaseWidth < BaseHeight) {
                double widthScale;
                if(CommentArea.Height <= RealVideoHeight && CommentArea.Width <= RealVideoWidth) {
                    // #207: sm29825902
                    widthScale = RealVideoWidth / CommentArea.Width;
                } else {
                    // #207: sm29681139
                    widthScale = (BaseHeight / BaseWidth) + (RealVideoWidth / CommentArea.Width);
                }

                BaseWidth *= widthScale;
            }

            ChangedEnabledCommentPercent();
        }

        void SetVideoDataInformation()
        {
            RealVideoWidth = Player.VlcMediaPlayer.PixelWidth;
            RealVideoHeight = Player.VlcMediaPlayer.PixelHeight;

            // コメントエリアのサイズ設定
            ChangeBaseSize();
        }

        [HandleProcessCorruptedStateExceptions]
        void SetMedia()
        {
            if(!IsSettedMedia && !IsViewClosed) {
                Mediation.Logger.Debug($"{VideoId}: {nameof(Player.RebuildPlayer)}");

                //Player.RebuildPlayer();

                //Player.Dispatcher.Invoke(() => {
                Mediation.Logger.Debug($"{VideoId}: set media {PlayFile.FullName}");
                Player.LoadMedia(PlayFile.FullName);

                //});
                IsSettedMedia = true;
            }
        }

        void StartIfAutoPlay()
        {
            if(IsAutoPlay && !UserOperationStop.Value && !IsViewClosed) {
                SetMedia();
                PlayMovie();
            }
        }

        void PlayMovie()
        {
            Player.IsMute = IsMute;
            Player.Volume = Volume;

            //return View.Dispatcher.BeginInvoke(new Action(() => {
            ClearComment();
            if(!IsViewClosed) {
                Player.Play();
                CanVideoPlay = true;
            }
            //}), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        void StopMovie(bool isStopComment)
        {
            Mediation.Logger.Debug("stop");
            if(IsSettedMedia && !IsViewClosed) {
                Player.Stop();
            }
            //Player.BeginStop(() => {
            //    Mediation.Logger.Debug("stoped");
            //    PlayerState = PlayerState.Stop;
            //    VideoPosition = 0;
            //    ClearComment();
            //    PrevPlayedTime = TimeSpan.Zero;
            //});
            Mediation.Logger.Debug("stoped");
            PlayerState = PlayerState.Stop;
            VideoPosition = 0;
            if(isStopComment) {
                ClearComment();
            }
            PrevPlayedTime = TimeSpan.Zero;
        }

        void ClearComment()
        {
            foreach(var data in ShowingCommentList.ToArray()) {
                data.Clock.Controller.SkipToFill();
                data.Clock.Controller.Remove();
            }
            ShowingCommentList.Clear();
        }

        /// <summary>
        /// 動画再生位置を移動。
        /// </summary>
        /// <param name="targetPosition">動画再生位置。</param>
        void MoveVideoPosition(float targetPosition)
        {
            Mediation.Logger.Debug(targetPosition.ToString());

            float setPosition = targetPosition;

            if(targetPosition <= 0) {
                setPosition = 0;
            } else {
                var percentLoaded = (double)VideoLoadedSize / (double)VideoTotalSize;
                if(percentLoaded < targetPosition) {
                    setPosition = (float)percentLoaded;
                }
            }
            ClearComment();

            Mediation.Logger.Debug(setPosition.ToString());
            Player.Position = setPosition;
            Mediation.Logger.Debug(Player.Position.ToString());
            PrevPlayedTime = Player.Time;
        }

        void FireShowComments()
        {
            //Mediation.Logger.Trace($"{VideoId}: {PrevPlayedTime} - {PlayTime}, {Player.ActualWidth}x{Player.ActualHeight}");

            SmileVideoCommentUtility.FireShowCommentsCore(NormalCommentArea, GetCommentArea(false), PrevPlayedTime, PlayTime, NormalCommentList, ShowingCommentList, IsEnabledDisplayCommentLimit, DisplayCommentLimitCount, CommentStyleSetting);
            SmileVideoCommentUtility.FireShowCommentsCore(OriginalPosterCommentArea, GetCommentArea(true), PrevPlayedTime, PlayTime, OriginalPosterCommentList, ShowingCommentList, IsEnabledDisplayCommentLimit, DisplayCommentLimitCount, CommentStyleSetting);
        }

        Size GetCommentArea(bool OriginalPoster)
        {
            if(OriginalPoster && !IsEnabledOriginalPosterCommentArea) {
                return new Size(OriginalPosterCommentArea.ActualWidth, OriginalPosterCommentArea.ActualHeight);
            }

            return new Size(NormalCommentArea.ActualWidth, CommentEnabledHeight);
        }

        void ScrollCommentList()
        {
            if(!IsAutoScroll) {
                return;
            }
            var nowTimelineItem = CommentItems
                .Cast<SmileVideoCommentViewModel>()
                .FirstOrDefault(c => SmileVideoCommentUtility.InShowTime(c, PrevPlayedTime, PlayTime))
            ;
            if(nowTimelineItem != null) {
                CommentView.ScrollToCenterOfView(nowTimelineItem, true, false);
            }
        }

        bool FilterCommentItems(object o)
        {
            if(FilteringCommentType == SmileVideoFilteringCommentType.All) {
                return true;
            }

            var item = (SmileVideoCommentViewModel)o;

            return item.FilteringView;
        }

        Task LoadRelationVideoAsync()
        {
            RelationVideoLoadState = LoadState.Preparation;

            RelationVideoLoadState = LoadState.Loading;
            return Information.LoadRelationVideosAsync(Constants.ServiceSmileVideoRelationCacheSpan).ContinueWith(task => {
                var items = task.Result;
                if(items == null) {
                    RelationVideoLoadState = LoadState.Failure;
                    return Task.CompletedTask;
                } else {
                    RelationVideoItems.InitializeRange(items);
                    var loader = new SmileVideoInformationLoader(items);
                    return loader.LoadThumbnaiImageAsync(Constants.ServiceSmileVideoImageCacheSpan).ContinueWith(_ => {
                        RelationVideoLoadState = LoadState.Loaded;
                    });
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void CheckTagPedia()
        {
            Debug.Assert(Information.PageHtmlLoadState == LoadState.Loaded);

            IsCheckedTagPedia = true;

            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            htmlDocument.LoadHtml(Information.WatchPageHtmlSource);
            var json = SmileVideoWatchAPIUtility.ConvertJsonFromWatchPage(Information.WatchPageHtmlSource);
            var videoDetail = json?.SelectToken("videoDetail");
            var tagList = videoDetail?.SelectToken("tagList");
            if(tagList != null) {
                var map = TagItems.ToDictionary(tk => tk.TagName, tv => tv);
                foreach(var tagItem in tagList) {
                    var tagName = tagItem.Value<string>("tag");
                    var hasDic = tagItem.Value<string>("dic");
                    if(RawValueUtility.ConvertBoolean(hasDic)) {
                        SmileVideoTagViewModel tag;
                        if(map.TryGetValue(tagName, out tag)) {
                            tag.ExistPedia = true;
                        }
                    }
                }
            }
        }

        void MakeDescription()
        {
            IsMadeDescription = true;

            //var flowDocumentSource = SmileDescriptionUtility.ConvertFlowDocumentFromHtml(Mediation, Information.DescriptionHtmlSource);
            var description = new SmileDescription(Mediation);
            var flowDocumentSource = description.ConvertFlowDocumentFromHtml(Information.DescriptionHtmlSource);
#if false
#if DEBUG
            var h = Path.Combine(DownloadDirectory.FullName, $"description.html");
            using(var s = File.CreateText(h)) {
                s.Write(VideoInformation.DescriptionHtml);
            }
            foreach(var ext in new[] { "xml", "xaml" }) {
                var x = Path.Combine(DownloadDirectory.FullName, $"description.{ext}");
                using(var s = File.CreateText(x)) {
                    s.Write(flowDocumentSource);
                }
            }
#endif
#endif
            DocumentDescription.Dispatcher.Invoke(() => {
                var document = DocumentDescription.Document;

                document.Blocks.Clear();

                using(var stringReader = new StringReader(flowDocumentSource))
                using(var xmlReader = System.Xml.XmlReader.Create(stringReader)) {
                    try {
                        var flowDocument = XamlReader.Load(xmlReader) as FlowDocument;
                        document.Blocks.AddRange(flowDocument.Blocks.ToArray());
                    } catch(XamlParseException ex) {
                        Mediation.Logger.Error(ex);
                        var error = new Paragraph();
                        error.Inlines.Add(ex.ToString());

                        var raw = new Paragraph();
                        raw.Inlines.Add(flowDocumentSource);

                        document.Blocks.Add(error);
                        document.Blocks.Add(raw);
                    }
                }

                document.FontSize = DocumentDescription.FontSize;
                document.FontFamily = DocumentDescription.FontFamily;
                document.FontStretch = DocumentDescription.FontStretch;
            });
        }

        void AddBookmark(SmileVideoBookmarkNodeViewModel bookmarkNode)
        {
            var videoItem = Information.ToVideoItemModel();
            bookmarkNode.VideoItems.Add(videoItem);
        }

        Task<SmileJsonResultModel> AddMyListAsync(SmileVideoMyListFinderViewModelBase myListFinder)
        {
            var defaultMyListFinder = myListFinder as SmileVideoAccountMyListDefaultFinderViewModel;
            if(defaultMyListFinder != null) {
                return AddAccountDefaultMyListAsync(defaultMyListFinder);
            } else {
                return AddAccountMyListAsync(myListFinder);
            }
        }

        Task<SmileJsonResultModel> AddAccountDefaultMyListAsync(SmileVideoAccountMyListDefaultFinderViewModel defaultMyListFinder)
        {
            //var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            var myList = new Logic.Service.Smile.Api.V1.MyList(Mediation);
            return myList.AdditionAccountDefaultMyListFromVideo(VideoId, Information.PageVideoToken);
        }

        Task<SmileJsonResultModel> AddAccountMyListAsync(SmileVideoMyListFinderViewModelBase myListFinder)
        {
            //var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            var myList = new Logic.Service.Smile.Api.V1.MyList(Mediation);
            return myList.AdditionAccountMyListFromVideo(myListFinder.MyListId, Information.ThreadId, Information.PageVideoToken);
        }

        void SearchTag(SmileVideoTagViewModel tagViewModel)
        {
            var parameter = new SmileVideoSearchParameterModel() {
                SearchType = SearchType.Tag,
                Query = tagViewModel.TagName,
            };

            Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, parameter, ShowViewState.Foreground));
        }

        void OpenUserId(string userId)
        {
            //    var parameter = new SmileOpenUserIdParameterModel() {
            //        UserId = userId,
            //    };

            //    Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.Smile, parameter, ShowViewState.Foreground));
            SmileDescriptionUtility.OpenUserId(userId, Mediation);
        }

        void OpenWebLink(string link)
        {
            //try {
            //    Process.Start(link);
            //} catch(Exception ex) {
            //    Mediation.Logger.Warning(ex);
            //}
            SmileDescriptionUtility.OpenWebLink(link, Mediation.Logger);
        }

        Task OpenVideoLinkAsync(string videoId)
        {
            var cancel = new CancellationTokenSource();
            var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));
            return Mediation.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request).ContinueWith(t => {
                var videoInformation = t.Result;
                return LoadAsync(videoInformation, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
            }, cancel.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void OpenMyListLink(string myListId)
        {
            //var parameter = new SmileVideoSearchMyListParameterModel() {
            //    Query = myListId,
            //    QueryType = SmileVideoSearchMyListQueryType.MyListId,
            //};

            //Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, parameter, ShowViewState.Foreground));
            SmileDescriptionUtility.OpenMyListId(myListId, Mediation);
        }

        void RefreshFilteringComment()
        {
            Debug.Assert(FilteringCommentType != SmileVideoFilteringCommentType.All);

            foreach(var item in CommentList) {
                item.FilteringView = false;
            }

            switch(FilteringCommentType) {
                case SmileVideoFilteringCommentType.All:
                    break;

                case SmileVideoFilteringCommentType.OriginalPoster:
                    foreach(var item in OriginalPosterCommentList) {
                        item.FilteringView = true;
                    }
                    break;

                case SmileVideoFilteringCommentType.UserId:
                    if(!string.IsNullOrWhiteSpace(FilteringUserId)) {
                        foreach(var item in CommentList.Where(i => i.UserId != null && i.UserId.IndexOf(FilteringUserId, StringComparison.OrdinalIgnoreCase) != -1)) {
                            item.FilteringView = true;
                        }
                    } else {
                        foreach(var item in CommentList) {
                            item.FilteringView = true;
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            CommentItems.Refresh();
        }

        static void ApprovalCommentSet(IEnumerable<SmileVideoCommentViewModel> list, bool value, string noApprovalRemark, string noApprovalDetail)
        {
            foreach(var item in list) {
                item.Approval = value;
                if(item.Approval) {
                    item.NoApprovalRemark.Value = null;
                    item.NoApprovalDetail.Value = null;
                } else {
                    item.NoApprovalRemark.Value = noApprovalRemark;
                    item.NoApprovalDetail.Value = noApprovalDetail;
                }
            }
        }

        void ApprovalCommentSharedNoGood(IEnumerable<SmileVideoCommentViewModel> comments)
        {
            // 共有NG
            if(IsEnabledSharedNoGood) {
                var targetComments = comments.Where(c => c.Score <= SharedNoGoodScore);
                ApprovalCommentSet(targetComments, false, global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_Comment_NoApproval_SharedNoGood, $"score < {SharedNoGoodScore}");
            }
        }

        void ApprovalCommentCustomOverlap(IEnumerable<SmileVideoCommentViewModel> comments, bool isGlobalFilter, bool ignoreOverlapWord, TimeSpan ignoreOverlapTime)
        {
            if(!ignoreOverlapWord) {
                return;
            }

            var groupingComments = comments
                .Where(c => c.Approval)
                .OrderBy(c => c.ElapsedTime)
                .GroupBy(c => c.Content.Trim())
            ;
            foreach(var groupingComment in groupingComments) {
                var length = groupingComment.Count();
                for(var i = 1; i < length; i++) {
                    var prevItem = groupingComment.ElementAt(i - 1);
                    var nowItem = groupingComment.ElementAt(i);
                    if(nowItem.ElapsedTime - prevItem.ElapsedTime <= ignoreOverlapTime) {
                        nowItem.Approval = false;
                        if(!nowItem.Approval) {
                            nowItem.NoApprovalRemark.Value = isGlobalFilter
                                ? global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_Comment_NoApproval_GlobalFilter
                                : global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_Comment_NoApproval_LocalFilter
                            ;
                            nowItem.NoApprovalDetail.Value = global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_Comment_NoApproval_Detail_Overlap;
                        }
                    }
                }
            }
        }

        void ApprovalCommentCustomDefineKeys(IEnumerable<SmileVideoCommentViewModel> comments, bool isGlobalFilter, IReadOnlyList<string> defineKeys)
        {
            if(!defineKeys.Any()) {
                return;
            }

            var filterList = defineKeys
                .Join(Mediation.Smile.VideoMediation.Filtering.Elements, d => d, e => e.Key, (d, e) => e)
                .Select(e => SmileVideoCommentUtility.ConvertFromDefined(e))
                .ToList()
            ;

            ApprovalCommentCustomFilterItems(comments, isGlobalFilter, filterList);
        }

        void ApprovalCommentCustomFilterItems(IEnumerable<SmileVideoCommentViewModel> comments, bool isGlobalFilter, IReadOnlyList<SmileVideoCommentFilteringItemSettingModel> filterList)
        {
            if(!filterList.Any()) {
                return;
            }

            var filters = filterList.Select(f => new SmileVideoCommentFiltering(f));
            foreach(var filter in filters.AsParallel()) {
                foreach(var item in comments.AsParallel().Where(c => c.Approval)) {
                    item.Approval = !filter.IsHit(item.Content, item.UserId, item.Commands);
                    if(!item.Approval) {
                        item.NoApprovalRemark.Value = isGlobalFilter
                            ? global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_Comment_NoApproval_GlobalFilter
                            : global::ContentTypeTextNet.MnMn.MnMn.Properties.Resources.String_Service_Smile_SmileVideo_Comment_NoApproval_LocalFilter
                        ;

                        item.NoApprovalDetail.Value = filter.Name;
                    }
                }
            }
        }

        private void ApprovalCommentCustom(IEnumerable<SmileVideoCommentViewModel> comments, bool isGlobalFilter, SmileVideoFilteringViweModel filter)
        {
            ApprovalCommentCustomOverlap(comments, isGlobalFilter, filter.IgnoreOverlapWord, filter.IgnoreOverlapTime);
            ApprovalCommentCustomDefineKeys(comments, isGlobalFilter, filter.DefineKeys);
            ApprovalCommentCustomFilterItems(comments, isGlobalFilter, filter.CommentFilterList.ModelList);
        }

        void ApprovalComment()
        {
            if(!CommentList.Any()) {
                return;
            }

            var comments = IsEnabledOriginalPosterFilering ? CommentList : NormalCommentList;

            ApprovalCommentSet(CommentList, true, string.Empty, string.Empty);

            ApprovalCommentSharedNoGood(comments);

            ApprovalCommentCustom(comments, false, LocalCommentFilering);
            if(IsEnabledGlobalCommentFilering) {
                ApprovalCommentCustom(comments, true, GlobalCommentFilering);
            }
        }

        void ClearSelectedComment()
        {
            SelectedComment = null;
        }

        Task LoadNextPlayListItemAsync()
        {
            var targetViewModel = PlayListItems.ChangeNextItem();
            return LoadAsync(targetViewModel, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        Task LoadPrevPlayListItemAsync()
        {
            var targetViewModel = PlayListItems.ChangePrevItem();
            return LoadAsync(targetViewModel, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        void AddHistory(SmileVideoPlayHistoryModel historyModel)
        {
            Setting.History.Insert(0, historyModel);
        }

        double GetEnabledCommentHeight(double baseHeight, double percent)
        {
            return baseHeight / 100.0 * percent;
        }

        void ChangedEnabledCommentPercent()
        {
            CommentEnabledHeight = GetEnabledCommentHeight(NormalCommentArea.ActualHeight, CommentEnabledPercent);
        }

        void ChangedCommentFont()
        {
            foreach(var comment in CommentList) {
                comment.ChangeFontStyle();
            }
        }

        void ChangedCommentContent()
        {
            foreach(var comment in ShowingCommentList.ToArray()) {
                comment.ViewModel.ChangeActualContent();
                comment.ViewModel.ChangeTextShow();
            }
        }

        void ChangedCommentShowTime(TimeSpan prevTime)
        {
#if false // ややっこしいんじゃふざけんな
            foreach(var comment in ShowingCommentList.ToArray()) {
                var time = comment.Clock.CurrentTime.Value;
                var nowElapsedTime = prevTime - time;
                var percent = nowElapsedTime.TotalMilliseconds / prevTime.TotalMilliseconds;
                var animation = new DoubleAnimation();

                if(comment.ViewModel.Vertical == SmileVideoCommentVertical.Normal) {
                    var nowLeft = Canvas.GetLeft(comment.Element);
                    animation.From = nowLeft;
                    animation.To = -comment.Element.ActualWidth;
                    var useTime = TimeSpan.FromMilliseconds(CommentShowTime.TotalMilliseconds * percent);
                    animation.Duration = new Duration(useTime);

                    //comment.Clock.Controller.Stop();
                    comment.Animation.Duration = CommentShowTime;
                    comment.Element.ApplyAnimationClock(Canvas.LeftProperty, animation.CreateClock());
                }
            }
#endif
        }

        void ResetCommentSetting()
        {
            CommentStyleSetting.FontFamily = Constants.SettingServiceSmileVideoCommentFontFamily;
            CommentStyleSetting.FontSize = Constants.SettingServiceSmileVideoCommentFontSize;
            CommentStyleSetting.FontBold = Constants.SettingServiceSmileVideoCommentFontBold;
            CommentStyleSetting.FontItalic = Constants.SettingServiceSmileVideoCommentFontItalic;
            CommentStyleSetting.FontAlpha = Constants.SettingServiceSmileVideoCommentFontAlpha;
            CommentStyleSetting.ShowTime = Constants.SettingServiceSmileVideoCommentShowTime;
            CommentStyleSetting.ConvertPairYenSlash = Constants.SettingServiceSmileVideoCommentConvertPairYenSlash;
            CommentStyleSetting.TextShowKind = Constants.SettingServiceSmileVideoCommentTextShowKind;

            ChangedCommentFont();
            ChangedCommentContent();
            ResetCommentInformation();

            var propertyNames = new[] {
                nameof(CommentFontFamily),
                nameof(CommentFontSize),
                nameof(CommentFontBold),
                nameof(CommentFontItalic),
                nameof(CommentFontAlpha),
                nameof(CommentShowTime),
                nameof(CommentConvertPairYenSlash),
                nameof(PlayerTextShowKind),
                nameof(CommentInformation),
            };
            CallOnPropertyChange(propertyNames);
        }

        IEnumerable<string> CreatePostCommandsCore()
        {
            yield return PostBeforeCommand;

            if(!IsEnabledPostAnonymous) {
                yield return SmileVideoMsgUtility.ConvertRawIsAnonymous(IsPostAnonymous);
            }
            if(PostCommandVertical != SmileVideoCommentVertical.Normal) {
                yield return SmileVideoMsgUtility.ConvertRawVerticalAlign(PostCommandVertical);
            }
            if(PostCommandSize != SmileVideoCommentSize.Medium) {
                yield return SmileVideoMsgUtility.ConvertRawFontSize(PostCommandSize);
            }
            if(PostCommandColor != Colors.White) {
                yield return SmileVideoMsgUtility.ConvertRawForeColor(PostCommandColor);
            }

            yield return PostAfterCommand;
        }

        IEnumerable<string> CreatePostCommands()
        {
            return CreatePostCommandsCore()
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
            ;
        }

        void ChangedPostCommands()
        {
            var commands = CreatePostCommands();
            PostCommandItems.InitializeRange(commands);
        }

        async Task PostCommentAsync(TimeSpan videoPosition)
        {
            if(CommentThread == null) {
                var rawMessagePacket = await LoadMsgCoreAsync(0, 0, 0, 0, 0);
                ImportCommentThread(rawMessagePacket);
            }

            var commentCount = RawValueUtility.ConvertInteger(CommentThread.LastRes ?? "0");
            Debug.Assert(CommentThread.Thread == Information.ThreadId);

            var msg = new Msg(Mediation);

            var postKey = await msg.LoadPostKeyAsync(Information.ThreadId, commentCount);
            if(postKey == null) {
                SetCommentInformation(Properties.Resources.String_App_Define_Service_Smile_Video_Comment_PostKeyError);
                return;
            } else {
                Mediation.Logger.Information($"postkey = {postKey}");
            }

            var resultPost = await msg.PostAsync(
                Information.MessageServerUrl,
                Information.ThreadId,
                videoPosition,
                CommentThread.Ticket,
                postKey.PostKey,
                PostCommandItems,
                PostCommentBody
            );
            if(resultPost == null) {
                SetCommentInformation($"fail: {nameof(msg.PostAsync)}");
                return;
            }
            var status = SmileVideoMsgUtility.ConvertResultStatus(resultPost.ChatResult.Status);
            if(status != SmileVideoCommentResultStatus.Success) {
                //TODO: ユーザー側に通知
                var message = DisplayTextUtility.GetDisplayText(status);
                SetCommentInformation($"fail: {message}, {status}");
                return;
            }

            //投稿コメントを画面上に流す
            //TODO: 投稿者判定なし

            var commentModel = new RawSmileVideoMsgChatModel() {
                //Anonymity = SmileVideoCommentUtility.GetIsAnonymous(PostCommandItems)
                Mail = string.Join(" ", PostCommandItems),
                Content = PostCommentBody,
                Date = RawValueUtility.ConvertRawUnixTime(DateTime.Now).ToString(),
                No = resultPost.ChatResult.No,
                VPos = SmileVideoMsgUtility.ConvertRawElapsedTime(videoPosition),
                Thread = resultPost.ChatResult.Thread,
            };
            var commentViewModel = new SmileVideoCommentViewModel(commentModel, CommentStyleSetting) {
                IsMyPost = true,
                Approval = true,
            };
            SmileVideoCommentUtility.FireShowSingleComment(commentViewModel, NormalCommentArea, GetCommentArea(false), PrevPlayedTime, ShowingCommentList, CommentStyleSetting);

            NormalCommentList.Add(commentViewModel);
            CommentList.Add(commentViewModel);
            CommentItems.Refresh();

            ResetCommentInformation();

            // コメント再取得
            await LoadMsgAsync(CacheSpan.NoCache);
        }

        void SetCommentInformation(string text)
        {
            CommentInformation = text;
        }

        void ResetCommentInformation()
        {
            CommentInformation = string.Empty;
            PostCommentBody = string.Empty;
        }

        void SetLocalFiltering()
        {
            if(View != null) {
                View.localFilter.Filtering = LocalCommentFilering;
            }
        }

        void SetNavigationbarBaseEvent(Navigationbar navigationbar)
        {
            navigationbar.seekbar.PreviewMouseDown += VideoSilder_PreviewMouseDown;
            //navigationbar.seekbar.MouseEnter += Seekbar_MouseEnter;
            //navigationbar.seekbar.MouseLeave += Seekbar_MouseLeave;
        }

        void UnsetNavigationbarBaseEvent(Navigationbar navigationbar)
        {
            navigationbar.seekbar.PreviewMouseDown -= VideoSilder_PreviewMouseDown;
            //navigationbar.seekbar.MouseEnter -= Seekbar_MouseEnter;
            //navigationbar.seekbar.MouseLeave -= Seekbar_MouseLeave;
        }

        void SwitchFullScreen()
        {
            SetWindowMode(!IsNormalWindow);
        }

        void SetWindowMode(bool toNormalWindow)
        {
            IsNormalWindow = toNormalWindow;

            var hWnd = HandleUtility.GetWindowHandle(View);
            if(toNormalWindow) {
                //ResizeBorderThickness = enabledResizeBorderThickness;
                ////重複
                //if(State == WindowState.Maximized) {
                //    WindowBorderThickness = maximumWindowBorderThickness;
                //    State = WindowState.Normal;
                //} else {
                //    WindowBorderThickness = normalWindowBorderThickness;
                //}

                View.Deactivated -= View_Deactivated;

                var logicalViewArea = new Rect(Left, Top, Width, Height);
                var deviceViewArea = UIUtility.ToLogicalPixel(View, logicalViewArea);
                var podRect = PodStructUtility.Convert(deviceViewArea);
                NativeMethods.MoveWindow(hWnd, podRect.Left, podRect.Top, podRect.Width, podRect.Height, true);
            } else {
                View.Deactivated += View_Deactivated;

                //ResizeBorderThickness = new Thickness(0);
                //WindowBorderThickness = new Thickness(0);

                var podRect = PodStructUtility.Convert(Screen.PrimaryScreen.DeviceBounds);
                NativeMethods.MoveWindow(hWnd, podRect.Left, podRect.Top, podRect.Width, podRect.Height, true);

                // #164: http://stackoverflow.com/questions/2052389/wpf-reset-focus-on-button-click
                var scope = FocusManager.GetFocusScope(View);
                FocusManager.SetFocusedElement(scope, null);
                Keyboard.ClearFocus();
                Keyboard.Focus(View);
            }
        }

        public void MoveForeground()
        {
            // 経験則上これが一番確実という悲しさ
            if(!Topmost) {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    Topmost = true;
                }), DispatcherPriority.SystemIdle).Task.ContinueWith(t => {
                    t.Dispose();
                    Topmost = false;
                    View.Activate();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        void ChangeVolume(bool isUp)
        {
            var step = isUp ? Setting.Player.VolumeOperationStep : -Setting.Player.VolumeOperationStep;
            var setVolume = Volume + step;
            Volume = RangeUtility.Clamp(setVolume, Constants.NavigatorVolumeRange);
        }

        void ChangeSeekVideoPosition(bool isNext, bool isPercent, int stepValue)
        {
            if(isPercent) {
                var step = (float)(stepValue * 0.01);
                if(!isNext) {
                    step *= -1;
                }
                Player.Position += step;
            } else {
                var nextTime = Player.Time.Add(TimeSpan.FromSeconds(isNext ? stepValue : -stepValue));
                if(nextTime.Ticks <= 0) {
                    nextTime = TimeSpan.Zero;
                }
                Player.Time = nextTime;
            }

            ClearComment();
            VideoPosition = Player.Position;
            PlayTime = PrevPlayedTime = Player.Time;
        }

        void AddCommentFilter(SmileVideoCommentFilteringTarget target, string source, bool setGlobalSetting)
        {
            Mediation.Logger.Debug($"{target}: {source}, global: {setGlobalSetting}");

            var usingFilter = setGlobalSetting
                ? GlobalCommentFilering
                : LocalCommentFilering
            ;

            //同一っぽいデータがある場合は無視する
            if(usingFilter.CommentFilterList.Any(i => i.Model.Target == target && i.Model.Source == source)) {
                return;
            }

            var model = new SmileVideoCommentFilteringItemSettingModel() {
                Source = source,
                Target = target,
                Type = FilteringType.PerfectMatch,
                IgnoreCase = true,
                IsEnabled = true,
            };

            usingFilter.AddCommentFilter(model);
            ApprovalComment();
        }

        /// <summary>
        /// あとで見るから外す処理。
        /// </summary>
        /// <param name="id">動画IDかスレッドIDを指定。</param>
        void SetCheckItLater(string id)
        {
            var later = Setting.CheckItLater
                .Where(c => !c.IsChecked)
                .FirstOrDefault(c => c.VideoId == id)
            ;
            if(later != null) {
                later.IsChecked = true;
                later.CheckTimestamp = DateTime.Now;
            }
        }

        #endregion

        #region SmileVideoDownloadViewModel

        public override Task LoadAsync(SmileVideoInformationViewModel videoInformation, bool forceEconomy, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            // TODO:forceEconomyは今のところ無効

            foreach(var item in PlayListItems.Where(i => i != videoInformation)) {
                item.IsPlaying = false;
            }

            if(PlayListItems.All(i => i != videoInformation)) {
                // プレイリストに存在しない動画は追加する
                PlayListItems.Add(videoInformation);
            }

            videoInformation.IsPlaying = true;
            videoInformation.LastShowTimestamp = DateTime.Now;
            IsSelectedInformation = true;

            // プレイヤー立ち上げ時は null
            if(Information != null) {
                Information.IsPlaying = false;
                Information.SaveSetting(false);
                // 軽めにGC
                Mediation.Order(new AppCleanMemoryOrderModel(false));
            }

            var historyModel = Setting.History.FirstOrDefault(f => f.VideoId == videoInformation.VideoId);
            if(historyModel == null) {
                historyModel = new SmileVideoPlayHistoryModel() {
                    VideoId = videoInformation.VideoId,
                    VideoTitle = videoInformation.Title,
                    Length = videoInformation.Length,
                    FirstRetrieve = videoInformation.FirstRetrieve,
                };
            } else {
                Setting.History.Remove(historyModel);
            }
            historyModel.WatchUrl = videoInformation.WatchUrl;
            historyModel.LastTimestamp = DateTime.Now;
            historyModel.Count = RangeUtility.Increment(historyModel.Count);

            AddHistory(historyModel);

            SetCheckItLater(videoInformation.VideoId);

            return base.LoadAsync(videoInformation, false, thumbCacheSpan, imageCacheSpan);
        }

        protected override void OnDownloadStart(object sender, DownloadStartEventArgs e)
        {
            if(!IsMadeDescription) {
                MakeDescription();
            }
            if(!IsCheckedTagPedia) {
                CheckTagPedia();
            }

            base.OnDownloadStart(sender, e);
        }

        protected override void OnDownloading(object sender, DownloadingEventArgs e)
        {
            if(Information.MovieType != SmileVideoMovieType.Swf) {
                if(!CanVideoPlay) {
                    // とりあえず待って、
                    VideoFile.Refresh();
                    // チェック。
                    CanVideoPlay = Setting.Player.AutoPlayLowestSize < VideoFile.Length;
                    if(CanVideoPlay) {
                        StartIfAutoPlay();
                    }
                }
            }

            e.Cancel |= IsViewClosed || (DownloadCancel != null && DownloadCancel.IsCancellationRequested);
            if(e.Cancel) {
                //if(UsingDmc.Value) {
                //    StopDmcDownloadAsync();
                //}
                StopMovie(true);

                Information.IsDownloading = false;
            }
            SecondsDownloadingSize = e.SecondsDownlodingSize;

            base.OnDownloading(sender, e);
        }

        void ResetSwf()
        {
            PlayFile.Refresh();
            //VideoLoadedSize = VideoTotalSize = PlayFile.Length;
        }

        protected override void OnLoadVideoEnd()
        {
            if(IsViewClosed) {
                return;
            }

            if(DownloadCancel == null || !DownloadCancel.IsCancellationRequested) {
                if(Information.PageHtmlLoadState == LoadState.Loaded) {
                    if(!IsMadeDescription) {
                        MakeDescription();
                    }
                    if(!IsCheckedTagPedia) {
                        CheckTagPedia();
                    }
                }

                if(Information.MovieType == SmileVideoMovieType.Swf && !Information.ConvertedSwf) {
                    // 変換が必要
                    var ffmpeg = new Ffmpeg();
                    var s = $"-i \"{VideoFile.FullName}\" -vcodec flv \"{PlayFile.FullName}\"";
                    ffmpeg.ExecuteAsync(s).ContinueWith(task => {
                        if(!IsViewClosed) {
                            Information.ConvertedSwf = true;
                            Information.SaveSetting(true);
                            ResetSwf();
                            CanVideoPlay = true;
                            StartIfAutoPlay();
                        }
                    });
                } else {
                    if(Information.MovieType == SmileVideoMovieType.Swf) {
                        ResetSwf();
                    }
                    // あまりにも小さい場合は読み込み完了時にも再生できなくなっている
                    if(!CanVideoPlay && !UserOperationStop.Value) {
                        CanVideoPlay = true;
                        StartIfAutoPlay();
                    }
                }
            }

            base.OnLoadVideoEnd();
        }


        protected override Task LoadCommentAsync(RawSmileVideoMsgPacketModel rawMsgPacket)
        {
            var comments = SmileVideoCommentUtility.CreateCommentViewModels(rawMsgPacket, CommentStyleSetting);
            CommentList.InitializeRange(comments);
            CommentListCount = CommentList.Count;

            var chartItems = SmileVideoCommentUtility.CreateCommentChartItems(CommentList, TotalTime);
            CommentChartList.InitializeRange(chartItems);
            ShowCommentChart = CommentChartList.Any(c => 0 < c.Y);

            NormalCommentList.InitializeRange(CommentList.Where(c => !c.IsOriginalPoster));
            var userSequence = SmileVideoCommentUtility.CreateFilteringUserItems(NormalCommentList);
            FilteringUserList.InitializeRange(userSequence);
            OriginalPosterCommentList.InitializeRange(CommentList.Where(c => c.IsOriginalPoster));
            OriginalPosterCommentListCount = OriginalPosterCommentList.Count;

            ApprovalComment();

            if(FilteringCommentType != SmileVideoFilteringCommentType.All) {
                RefreshFilteringComment();
            }

            return base.LoadCommentAsync(rawMsgPacket);
        }

        protected override void OnLoadGetthumbinfoEnd()
        {
            ChangedPostCommands();

            if(!PlayListItems.Any()) {
                PlayListItems.Add(Information);
            }
            if(Session.IsPremium && CommandColorItems.Count == SmileVideoMsgUtility.normalCommentColors.Length) {
                CommandColorItems.AddRange(SmileVideoMsgUtility.premiumCommentColors);
            }

            TotalTime = Information.Length;

            TagItems.InitializeRange(Information.TagList);

            LoadRelationVideoAsync();

            var propertyNames = new[] {
                nameof(VideoId),
                nameof(Title),
                nameof(IsEnabledPostAnonymous),
                nameof(Information),
                nameof(UserName),
                nameof(ChannelName),
                nameof(IsChannelVideo),
                nameof(IsEnabledGlobalCommentFilering),
            };
            CallOnPropertyChange(propertyNames);

            LocalCommentFilering = new SmileVideoFilteringViweModel(Information.Filtering, null, Mediation.Smile.VideoMediation.Filtering);
            SetLocalFiltering();

            base.OnLoadGetthumbinfoEnd();
        }

        protected override void InitializeStatus()
        {
            base.InitializeStatus();
            WaitingFirstPlay.Value = true;
            VideoPosition = 0;
            PrevPlayedTime = TimeSpan.Zero;
            _prevStateChangedPosition = initPrevStateChangedPosition;
            IsBufferingStop = false;
            UserOperationStop.Value = false;
            IsMadeDescription = false;
            IsCheckedTagPedia = false;
            ChangingVideoPosition = false;
            MovingSeekbarThumb = false;
            CanVideoPlay = false;
            IsVideoPlayng = false;
            PlayTime = TimeSpan.Zero;
            TotalTime = TimeSpan.Zero;
            SelectedComment = null;
            IsSettedMedia = false;
            ShowCommentChart = false;
            if(View != null) {
                if(Player != null && Player.State != Meta.Vlc.Interop.Media.MediaState.Playing && Player.State != Meta.Vlc.Interop.Media.MediaState.Paused) {
                    // https://github.com/higankanshi/Meta.Vlc/issues/80
                    Player.VlcMediaPlayer.Media = null;
                }
                View.Dispatcher.Invoke(() => {
                    CommentList.Clear();
                    NormalCommentList.Clear();
                    OriginalPosterCommentList.Clear();
                    ClearComment();
                });
            }
        }

        protected override Task StopPrevProcessAsync()
        {
            //var processTask = base.StopPrevProcessAsync();
            //var playerTask = Task.CompletedTask;
            if(Player != null) {
                if(Player.State != Meta.Vlc.Interop.Media.MediaState.Stopped) {
                    //playerTask = Task.Run(() => {
                    //var sleepTime = TimeSpan.FromMilliseconds(500);
                    //Thread.Sleep(sleepTime);
                    StopMovie(true);
                    //InitializeStatus();
                    //});
                }
                //Mediation.Logger.Debug($"{nameof(Player.RebuildPlayer)}");
                //Player.RebuildPlayer();
            }

            //return Task.WhenAll(processTask, playerTask);
            return base.StopPrevProcessAsync();
        }

        #endregion

        #region ISetView

        public void SetView(FrameworkElement view)
        {
            var playerView = (SmileVideoPlayerWindow)view;

            View = playerView;
            Player = View.player;//.MediaPlayer;
            NormalCommentArea = View.normalCommentArea;
            OriginalPosterCommentArea = View.originalPosterCommentArea;
            Navigationbar = View.seekbar;
            CommentView = View.commentView;
            DetailComment = View.detailComment;
            DocumentDescription = View.documentDescription;

            // 初期設定
            Player.Volume = Volume;
            SetLocalFiltering();

            // あれこれイベント, すっごいキモいことになってる
            var content = Navigationbar.ExstendsContent as Panel;
            EnabledCommentControl = UIUtility.FindLogicalChildren<Control>(content).ElementAt(1);

            EnabledCommentControl.MouseEnter += EnabledCommentControl_MouseEnter;
            EnabledCommentControl.MouseLeave += EnabledCommentControl_MouseLeave;

            View.Loaded += View_Loaded;
            View.Closing += View_Closing;
            Player.PositionChanged += Player_PositionChanged;
            Player.SizeChanged += Player_SizeChanged;
            Player.StateChanged += Player_StateChanged;
            Player.MouseWheel += Player_MouseWheel;
            SetNavigationbarBaseEvent(Navigationbar);
            DetailComment.LostFocus += DetailComment_LostFocus;
        }

        #endregion

        #region ICloseView

        public void Close()
        {
            if(!IsViewClosed) {
                View.Close();
            }
        }

        #endregion

        #region ICaptionCommand

        public WindowState State
        {
            get { return this._state; }
            //set
            //{
            //    if(SetVariableValue(ref this._state, value)) {
            //        if(State == WindowState.Maximized) {
            //            WindowBorderThickness = maximumWindowBorderThickness;
            //        } else {
            //            //if(!IsNormalWindow) {
            //            //    SetWindowMode(false);
            //            //}
            //            WindowBorderThickness = normalWindowBorderThickness;
            //        }
            //    }
            //}
            set { SetVariableValue(ref this._state, value); }
        }

        //public ICommand CaptionMinimumCommand { get { return CreateCommand(o => State = WindowState.Minimized); } }
        //public ICommand CaptionMaximumCommand { get { return CreateCommand(o => State = WindowState.Maximized); } }
        //public ICommand CaptionRestoreCommand { get { return CreateCommand(o => State = WindowState.Normal); } }
        //public ICommand CaptionCloseCommand { get { return CreateCommand(o => View.Close()); } }

        #endregion

        #region ISmileVideoDescription

        public ICommand OpenWebLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var link = o as string;
                        OpenWebLink(link);
                    }
                );
            }
        }

        public ICommand OpenVideoLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var videoId = o as string;
                        OpenVideoLinkAsync(videoId).ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand OpenMyListLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var myListId = o as string;
                        OpenMyListLink(myListId);
                    }
                );
            }
        }

        public ICommand OpenUserLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var userId = o as string;
                        OpenUserId(userId);
                    }
                );
            }
        }

        #endregion

        #region event

        void View_Loaded(object sender, RoutedEventArgs e)
        {
            View.Loaded -= View_Loaded;
        }

        private void View_Closing(object sender, CancelEventArgs e)
        {
            //TODO: closingはまずくね…?

            View.Loaded -= View_Loaded;
            View.Closing -= View_Closing;
            Player.PositionChanged -= Player_PositionChanged;
            Player.SizeChanged -= Player_SizeChanged;
            Player.StateChanged -= Player_StateChanged;
            Player.MouseWheel -= Player_MouseWheel;
            UnsetNavigationbarBaseEvent(Navigationbar);

            if(EnabledCommentControl != null) {
                EnabledCommentControl.MouseEnter -= EnabledCommentControl_MouseEnter;
                EnabledCommentControl.MouseLeave -= EnabledCommentControl_MouseLeave;
            }

            View.Deactivated -= View_Deactivated;

            IsViewClosed = true;

            if(Player.State == Meta.Vlc.Interop.Media.MediaState.Playing) {
                //Player.BeginStop();
                if(UsingDmc.Value) {
                    StopDmcDownloadAsync();
                }
                StopMovie(true);
            }

            ExportSetting();
            Information.SaveSetting(true);

            Information.IsPlaying = false;

            try {
                Player.Dispose();
            } catch(Exception ex) {
                Mediation.Logger.Error(ex);
            }
            Mediation.Order(new AppCleanMemoryOrderModel(true));
        }

        private void Player_PositionChanged(object sender, EventArgs e)
        {
            if(CanVideoPlay && !ChangingVideoPosition) {
                if(WaitingFirstPlay.Value) {
                    SetVideoDataInformation();
                    WaitingFirstPlay.Value = false;
                }
                VideoPosition = Player.Position;
                PlayTime = Player.Time;
                FireShowComments();
                ScrollCommentList();
                PrevPlayedTime = PlayTime;
            }
        }

        private void VideoSilder_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(!CanVideoPlay) {
                return;
            }

            var seekbar = (Slider)sender;
            seekbar.CaptureMouse();

            ChangingVideoPosition = true;
            SeekbarMouseDownPosition = e.GetPosition(seekbar);
            seekbar.PreviewMouseUp += VideoSilder_PreviewMouseUp;
            seekbar.MouseMove += Seekbar_MouseMove;

            var tempPosition = SeekbarMouseDownPosition.X / seekbar.ActualWidth;
            seekbar.Value = tempPosition;
        }

        private void Seekbar_MouseMove(object sender, MouseEventArgs e)
        {
            MovingSeekbarThumb = true;

            Debug.Assert(ChangingVideoPosition);

            var seekbar = (Slider)sender;
            var movingPosition = e.GetPosition(seekbar);

            var tempPosition = movingPosition.X / seekbar.ActualWidth;
            seekbar.Value = tempPosition;
        }

        private void VideoSilder_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Debug.Assert(CanVideoPlay);

            var seekbar = (Slider)sender;

            seekbar.PreviewMouseUp -= VideoSilder_PreviewMouseUp;
            seekbar.MouseMove -= Seekbar_MouseMove;


            //#82: これなんの処理だ、わからん
            //float nextPosition;
            //if(!MovingSeekbarThumb) {
            //    var pos = e.GetPosition(seekbar);
            //    nextPosition = (float)(pos.X / seekbar.ActualWidth);
            //} else {
            //    nextPosition = (float)((Navigationbar)seekbar.Parent).VideoPosition;
            //}
            var pos = e.GetPosition(seekbar);
            var nextPosition = (float)(pos.X / seekbar.ActualWidth);
            // TODO: 読み込んでない部分は移動不可にする
            MoveVideoPosition(nextPosition);

            ChangingVideoPosition = false;
            MovingSeekbarThumb = false;

            seekbar.ReleaseMouseCapture();
        }

        private void Player_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeBaseSize();
        }

        private void Player_StateChanged(object sender, Meta.Vlc.ObjectEventArgs<Meta.Vlc.Interop.Media.MediaState> e)
        {
            // 開いてる最中は気にしない
            if(e.Value == Meta.Vlc.Interop.Media.MediaState.Opening) {
                return;
            }
            //if(PlayerState != PlayerState.Pause && this._prevStateChangedPosition == VideoPosition && this._prevStateChangedPosition != initPrevStateChangedPosition) {
            //    return;
            //}
            this._prevStateChangedPosition = VideoPosition;

            Mediation.Logger.Debug($"{VideoId}: {e.Value}, pos: {VideoPosition}, time: {PlayTime} / {Player.Length}");
            switch(e.Value) {
                case Meta.Vlc.Interop.Media.MediaState.Playing:
                    var prevState = PlayerState;
                    PlayerState = PlayerState.Playing;
                    if(prevState == PlayerState.Pause) {
                        foreach(var data in ShowingCommentList) {
                            data.Clock.Controller.Resume();
                        }
                    }
                    break;

                case Meta.Vlc.Interop.Media.MediaState.Paused:
                    PlayerState = PlayerState.Pause;
                    foreach(var data in ShowingCommentList) {
                        data.Clock.Controller.Pause();
                    }
                    break;

                case Meta.Vlc.Interop.Media.MediaState.Ended:
                    if(VideoLoadState == LoadState.Loading) {
                        Mediation.Logger.Debug("buffering stop");
                        // 終わってない
                        IsBufferingStop = true;
                        BufferingVideoPosition = VideoPosition;
                    } else if(IsBufferingStop) {
                        Mediation.Logger.Debug("buffering wait");
                        PlayerState = PlayerState.Pause;
                        Player.Position = BufferingVideoPosition;
                        foreach(var data in ShowingCommentList) {
                            data.Clock.Controller.Pause();
                        }
                    } else if(CanPlayNextVieo.Value && PlayListItems.Skip(1).Any() && !UserOperationStop.Value) {
                        Mediation.Logger.Debug("next playlist item");
                        LoadNextPlayListItemAsync();
                    } else if(ReplayVideo && !UserOperationStop.Value) {
                        Mediation.Logger.Debug("replay");
                        //Player.BeginStop(() => {
                        //    Player.Dispatcher.Invoke(() => {
                        //        Player.Play();
                        //    });
                        //});
                        //Player.Stop();
                        //Player.Play();
                        StopMovie(true);
                        PlayMovie();
                    } else {
                        //PlayerState = PlayerState.Stop;
                        //VideoPosition = 0;
                        //ClearComment();
                        //PrevPlayedTime = TimeSpan.Zero;
                        StopMovie(false);
                    }
                    break;

                default:
                    break;
            }
        }

        private void Player_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var isUp = 0 < e.Delta;
            switch(Setting.Player.WheelOperation) {
                case Define.UI.Player.WheelOperation.None:
                    break;

                case Define.UI.Player.WheelOperation.Volume:
                    ChangeVolume(isUp);
                    break;

                case Define.UI.Player.WheelOperation.Seek:
                    ChangeSeekVideoPosition(isUp, Setting.Player.SeekOperationIsPercent, Setting.Player.SeekOperationIsPercent ? Setting.Player.SeekOperationPercentStep : Setting.Player.SeekOperationAbsoluteStep);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        //private void EnabledCommentPopup_Opened(object sender, EventArgs e)
        //{
        //    ShowEnabledCommentPreviewArea = true;
        //}

        //private void EnabledCommentPopup_Closed(object sender, EventArgs e)
        //{
        //    ShowEnabledCommentPreviewArea = false;
        //}

        private void EnabledCommentControl_MouseEnter(object sender, MouseEventArgs e)
        {
            ShowEnabledCommentPreviewArea = true;
        }

        private void EnabledCommentControl_MouseLeave(object sender, MouseEventArgs e)
        {
            ShowEnabledCommentPreviewArea = false;
        }

        private void DetailComment_LostFocus(object sender, RoutedEventArgs e)
        {
            //TODO: コメント詳細部の挙動はまぁどうでもいいや、あとまわし
            //ClearSelectedComment();
        }

        private void View_Deactivated(object sender, EventArgs e)
        {
            if(Setting.Player.InactiveIsFullScreenRestore) {
                SetWindowMode(true);
            }
        }

        //private void Seekbar_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    //ShowCommentChart = CommentChartList.Any(c => 0 < c.Y)
        //    //    ? Visibility.Visible
        //    //    : Visibility.Collapsed
        //    //;
        //}

        //private void Seekbar_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    //ShowCommentChart = Visibility.Collapsed;
        //}

        #endregion
    }
}
