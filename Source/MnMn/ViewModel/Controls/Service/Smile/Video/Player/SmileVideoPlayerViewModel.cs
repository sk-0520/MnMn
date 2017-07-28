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
using ContentTypeTextNet.MnMn.MnMn.Define.Exceptions.Service.Smile.Video;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Market;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.IdleTalk.Mutter;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Channel;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    /// <summary>
    /// プレイヤー管理。
    /// </summary>
    public partial class SmileVideoPlayerViewModel : SmileVideoDownloadViewModel, ISetView, ISmileDescription, ICloseView
    {
        #region define

        //static readonly Size BaseSize_4x3 = new Size(640, 480);
        //static readonly Size BaseSize_16x9 = new Size(512, 384);

        const float initPrevStateChangedPosition = -1;

        //static readonly Thickness enabledResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
        //static readonly Thickness maximumWindowBorderThickness = SystemParameters.WindowResizeBorderThickness;
        //static readonly Thickness normalWindowBorderThickness = new Thickness(1);

        #endregion

        public SmileVideoPlayerViewModel(Mediator mediator)
            : base(mediator)
        {
            CommentItems = CollectionViewSource.GetDefaultView(CommentList);
            CommentItems.Filter = FilterCommentItems;

            var filteringResult = Mediator.GetResultFromRequest<SmileVideoFilteringResultModel>(new SmileVideoCustomSettingRequestModel(SmileVideoCustomSettingKind.CommentFiltering));
            GlobalCommentFilering = filteringResult.Filtering;

            ImportSetting();
        }

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
            Left = PlayerSetting.Window.Left;
            Top = PlayerSetting.Window.Top;
            Width = PlayerSetting.Window.Width;
            Height = PlayerSetting.Window.Height;
            //Topmost = PlayerSetting.Window.Topmost;

            TopmostKind = PlayerSetting.TopmostKind;

            ViewScale = PlayerSetting.ViewScale;

            SetAreaLength(PlayerSetting.PlayerArea, PlayerSetting.CommentArea, PlayerSetting.InformationArea);

            PlayerShowDetailArea = PlayerSetting.ShowDetailArea;
            this._showNormalWindowCommentList = PlayerSetting.ShowNormalWindowCommentList;
            this._showFullScreenCommentList = PlayerSetting.ShowFullScreenCommentList;
            PlayerVisibleComment = PlayerSetting.VisibleComment;
            IsAutoScroll = PlayerSetting.AutoScrollCommentList;
            Volume = PlayerSetting.Volume;
            IsMute = PlayerSetting.IsMute;
            ReplayVideo = PlayerSetting.ReplayVideo;
            IsEnabledDisplayCommentLimit = PlayerSetting.IsEnabledDisplayCommentLimit;
            DisplayCommentLimitCount = PlayerSetting.DisplayCommentLimitCount;

            IsEnabledSharedNoGood = Setting.Comment.IsEnabledSharedNoGood;
            SharedNoGoodScore = Setting.Comment.SharedNoGoodScore;
            CommentStyleSetting = (SmileVideoCommentStyleSettingModel)Setting.Comment.StyleSetting.DeepClone();
            IsEnabledOriginalPosterFilering = Setting.Comment.IsEnabledOriginalPosterFilering;
            FillBackgroundOriginalPoster = Setting.Comment.FillBackgroundOriginalPoster;

            CanChangeCommentEnabledArea = Setting.Player.CanChangeCommentEnabledArea;
        }

        void ExportSetting()
        {
            PlayerSetting.Window.Left = Left;
            PlayerSetting.Window.Top = Top;
            PlayerSetting.Window.Width = Width;
            PlayerSetting.Window.Height = Height;
            //PlayerSetting.Window.Topmost = Topmost;

            PlayerSetting.TopmostKind = TopmostKind;

            PlayerSetting.ViewScale = ViewScale;

            PlayerSetting.PlayerArea = PlayerAreaLength.Value.Value;
            PlayerSetting.CommentArea = CommentAreaLength.Value.Value;
            PlayerSetting.InformationArea = InformationAreaLength.Value.Value;

            PlayerSetting.ShowDetailArea = PlayerShowDetailArea;
            PlayerSetting.ShowNormalWindowCommentList = this._showNormalWindowCommentList;
            PlayerSetting.ShowFullScreenCommentList = this._showFullScreenCommentList;
            PlayerSetting.VisibleComment = PlayerVisibleComment;
            PlayerSetting.AutoScrollCommentList = IsAutoScroll;
            PlayerSetting.Volume = Volume;
            PlayerSetting.IsMute = IsMute;
            PlayerSetting.ReplayVideo = ReplayVideo;
            PlayerSetting.IsEnabledDisplayCommentLimit = IsEnabledDisplayCommentLimit;
            PlayerSetting.DisplayCommentLimitCount = DisplayCommentLimitCount;

            Setting.Comment.IsEnabledSharedNoGood = IsEnabledSharedNoGood;
            Setting.Comment.SharedNoGoodScore = SharedNoGoodScore;
            Setting.Comment.StyleSetting = (SmileVideoCommentStyleSettingModel)CommentStyleSetting.DeepClone();
            Setting.Comment.IsEnabledOriginalPosterFilering = IsEnabledOriginalPosterFilering;
            Setting.Comment.FillBackgroundOriginalPoster = FillBackgroundOriginalPoster;
        }

        public Task LoadAsync(IEnumerable<SmileVideoInformationViewModel> videoInformations, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            PlayListItems.AddRange(videoInformations);
            CanPlayNextVideo.Value = true;

            // 再生(DL)可能か
            var firstVideo = PlayListItems.GetFirstItem();
            if(!SmileVideoInformationUtility.CheckCanPlay(firstVideo, Mediator.Logger)) {
                if(PlayListItems.CanItemChange) {
                    firstVideo = GetSafeChangeItem(firstVideo, PlayListItems.ChangeNextItem);
                } else {
                    firstVideo = null;
                }
                if(firstVideo == null) {
                    // もうどうしようもねーな
                    throw new SmileVideoCanNotPlayItemInPlayListException(videoInformations);
                }
            }

            return LoadAsync(firstVideo, false, thumbCacheSpan, imageCacheSpan);
        }

        void ChangePlayerSizeFromPercent(int percent)
        {
            if(VisualVideoSize.Width <= 0 && VisualVideoSize.Height <= 0) {
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
            var w = View.ActualWidth - Player.ActualWidth;
            var h = View.ActualHeight - Player.ActualHeight;

            if(w < 0 || h < 0) {
                Mediator.Logger.Warning($"{nameof(w)}: {w}, {nameof(h)}: {h}");
                return;
            }

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
                var defaultGridSplitterLength = (double)Application.Current.Resources[Constants.xamlGridSplitterLength];

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

            // 高さの方が大きいので動画描画横幅を補正する
            if(BaseWidth < BaseHeight) {
                double widthScale;
                if(Constants.ServiceSmileVideoPlayerCommentHeight <= RealVideoHeight && Constants.ServiceSmileVideoPlayerCommentWidth <= RealVideoWidth) {
                    // #207: sm29825902
                    widthScale = VisualVideoSize.Width / Constants.ServiceSmileVideoPlayerCommentWidth;
                } else {
                    // #207: sm29681139
                    widthScale = (BaseHeight / BaseWidth) + (VisualVideoSize.Width / Constants.ServiceSmileVideoPlayerCommentWidth);
                }

                BaseWidth *= widthScale;
            }

            // コメント描画領域の横幅を動画描画に合わせて補正する
            var resizePercent = Constants.ServiceSmileVideoPlayerCommentHeight / BaseHeight;
            CommentAreaWidth = resizePercent * BaseWidth;

            PlayerTaskbarThumbnailCreator.SetSize(new Size(BaseWidth, BaseHeight));
            PlayerTaskbarThumbnailCreator.Refresh();

            ChangedEnabledCommentPercent();
        }

        void SetVideoDataInformation()
        {
            RealVideoWidth = Player.VlcMediaPlayer.PixelWidth;
            RealVideoHeight = Player.VlcMediaPlayer.PixelHeight;

            // 描画領域のサイズ設定
            ChangeBaseSize();
        }

        [HandleProcessCorruptedStateExceptions]
        void SetMedia()
        {
            if(!IsSettedMedia && !IsViewClosed) {
                if(PlayFile == null) {
                    Mediator.Logger.Warning($"{VideoId}: {nameof(PlayFile)} is null", Session.LoginState);
                    return;
                }

                Player.Dispatcher.Invoke(new Action(() => {
                    Mediator.Logger.Debug($"{VideoId}: {nameof(Player.RebuildPlayer)}");
                    Player.RebuildPlayer();

                    Mediator.Logger.Debug($"{VideoId}: set media {PlayFile.FullName}");
                    Player.LoadMedia(PlayFile.FullName);
                }));

                IsSettedMedia = true;
            }
        }

        void SetMediaAndPlay()
        {
            SetMedia();
            PlayMovie();
        }

        void StartIfAutoPlay()
        {
            if(IsAutoPlay && !UserOperationStop.Value && !IsViewClosed) {
                Player.Dispatcher.Invoke(() => {
                    if(Player.IsLoaded) {
                        SetMediaAndPlay();
                    } else {
                        Player.Loaded += Player_LoadedAutoPlay;
                        Mediator.Logger.Information($"{VideoId}: player -> not loaded, event wait!!");
                    }
                }, DispatcherPriority.ApplicationIdle);
            }
        }

        void PlayMovie()
        {
            ClearComment();
            if(!IsViewClosed && IsSettedMedia) {
                var sw = new Stopwatch();
                sw.Start();
                Mediator.Logger.Debug($"{VideoId}: play invoke...");
                Player.Dispatcher.Invoke(new Action(() => {
                    if(Player == null) {
                        Mediator.Logger.Debug($"{VideoId}: play is null");
                        return;
                    }

                    Player.IsMute = IsMute;
                    Player.Volume = Volume;

                    var prePlayTime = sw.Elapsed;
                    StopMovie(false);
                    Player.Play();

                    if(TotalTime == TimeSpan.Zero) {
                        TotalTime = Player.Length;
                    }

                    var playedTime = sw.Elapsed;
                    sw.Stop();

                    CanVideoPlay = true;
                    Mediator.Logger.Debug($"{VideoId}: play! {prePlayTime} - {playedTime}");
                }), DispatcherPriority.SystemIdle);
            }
        }

        void StopMovie(bool isClearComment)
        {
            Mediator.Logger.Debug($"{VideoId}: stop");
            if(IsSettedMedia && !IsViewClosed) {
                Player.Stop();
            }

            Mediator.Logger.Debug($"{VideoId}: stopped");
            PlayerState = PlayerState.Stop;
            VideoPosition = 0;
            if(isClearComment) {
                ClearComment();
            }
            PrevPlayedTime = TimeSpan.Zero;
        }

        void ClearComment()
        {
            foreach(var data in ShowingCommentList.ToEvaluatedSequence()) {
                data.Clock.Controller.SkipToFill();
                data.Clock.Controller.Remove();
            }
            ShowingCommentList.Clear();

            Application.Current?.Dispatcher.Invoke(() => {
                NormalCommentArea?.Children.Clear();
                OriginalPosterCommentArea?.Children.Clear();
            });
        }

        void ClearResidualComments()
        {
            var commentItems = NormalCommentArea.Children.Cast<SmileVideoCommentElement>()
                .Select(c => new { Area = NormalCommentArea, View = c, ViewModel = (SmileVideoCommentViewModel)c.DataContext })
                .Concat(
                    OriginalPosterCommentArea.Children.Cast<SmileVideoCommentElement>()
                    .Select(c => new { Area = OriginalPosterCommentArea, View = c, ViewModel = (SmileVideoCommentViewModel)c.DataContext })
                )
                .Where(i => i.ViewModel.NowShowing)
                .Where(i => i.ViewModel.ElapsedTime + SmileVideoCommentUtility.correctionTime + CommentStyleSetting.ShowTime < PlayTime)
                .ToEvaluatedSequence()
            ;

            foreach(var commentItem in commentItems) {
                var showComment = ShowingCommentList.FirstOrDefault(c => c.ViewModel == commentItem.ViewModel);
                if(showComment != null) {
                    showComment.Clock.Controller.SkipToFill();
                    showComment.Clock.Controller.Remove();
                }
                commentItem.Area.Children.Remove(commentItem.View);
            }
        }

        /// <summary>
        /// 動画再生位置を移動。
        /// </summary>
        /// <param name="targetPosition">動画再生位置。</param>
        void MoveVideoPosition(float targetPosition)
        {
            float setPosition = targetPosition;

            if(targetPosition <= 0) {
                setPosition = 0;
            } else {
                var percentLoaded = (double)VideoLoadedSize / (double)VideoTotalSize;
                if(percentLoaded < targetPosition) {
                    setPosition = (float)percentLoaded;
                }

                if(PlayerState == PlayerState.Buffering) {
                    var changePosition = TotalTime.TotalMilliseconds / SafeShowTime.TotalMilliseconds;
                    if(setPosition <= changePosition) {
                        var changeTime = TimeSpan.FromMilliseconds(TotalTime.TotalMilliseconds * setPosition);
                        BufferingVideoTime = changeTime;
                        ResumeBufferingStop();
                    }
                    return;
                }
            }

            ClearComment();

            var prevPosition = Player.Position;
            Player.Position = setPosition;
            Mediator.Logger.Debug($"[{VideoId}]: {prevPosition} -> {setPosition} / {Player.Position}");
            PrevPlayedTime = Player.Time;
        }

        void ShowComments()
        {
            if(CommentScriptDefault != null) {
                // 先に最大かどうか見ておかないとオーバーフローする
                var isInfinity = CommentScriptDefault.CommentScript.IsEnabledTime == TimeSpan.MaxValue;
                if(isInfinity) {
                    if(PlayTime + SmileVideoCommentUtility.correctionTime < CommentScriptDefault.ElapsedTime) {
                        CommentScriptDefault = null;
                    }
                } else {
                    if(CommentScriptDefault.ElapsedTime + CommentScriptDefault.ElapsedTime + CommentScriptDefault.CommentScript.IsEnabledTime < PlayTime) {
                        CommentScriptDefault = null;
                    }
                }
            }

            var normalComments = SmileVideoCommentUtility.ShowComments(NormalCommentArea, GetCommentArea(false), PrevPlayedTime, PlayTime, NormalCommentList, false, ShowingCommentList, IsEnabledDisplayCommentLimit, DisplayCommentLimitCount, CommentStyleSetting, CommentScriptDefault?.CommentScript);

            var opComments = SmileVideoCommentUtility.ShowComments(OriginalPosterCommentArea, GetCommentArea(true), PrevPlayedTime, PlayTime, OriginalPosterCommentList, true, ShowingCommentList, IsEnabledDisplayCommentLimit, DisplayCommentLimitCount, CommentStyleSetting, null);
            if(opComments.Any()) {
                var commentScriptDefault = opComments.LastOrDefault(c => c.HasCommentScript && c.CommentScript.ScriptType == SmileVideoCommentScriptType.Default);
                if(commentScriptDefault != null) {
                    CommentScriptDefault = commentScriptDefault;
                }
            }
        }

        protected Size GetCommentArea(bool isOriginalPoster)
        {
            if(isOriginalPoster && !IsEnabledOriginalPosterCommentArea) {
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
                if(Setting.Player.DisbaledAutoScrollCommentListOverCursor && CommentView.IsMouseOver) {
                    return;
                }

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

        protected virtual Task LoadRelationVideoAsync()
        {
            RelationVideoLoadState = LoadState.Preparation;

            RelationVideoLoadState = LoadState.Loading;
            return Information.LoadRelationVideosAsync(Constants.ServiceSmileVideoRelationCacheSpan).ContinueWith(task => {
                var items = task.Result;
                if(items == null) {
                    RelationVideoLoadState = LoadState.Failure;
                    return Task.CompletedTask;
                } else {
                    SetRelationVideoItems(items);
                    var loader = new SmileVideoInformationLoader(items, NetworkSetting, Mediator.Logger);
                    return loader.LoadThumbnaiImageAsync(Constants.ServiceSmileVideoImageCacheSpan).ContinueWith(_ => {
                        RelationVideoLoadState = LoadState.Loaded;
                    });
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void SetRelationVideoItems(IEnumerable<SmileVideoInformationViewModel> items)
        {
            foreach(var prevItem in RelationVideoItems) {
                prevItem.DecrementReference();
            }
            if(!IsViewClosed) {
                Application.Current.Dispatcher.Invoke(() => {
                    RelationVideoItems.InitializeRange(items);
                });
            }
        }

        Task LoadMarketItemsImageAsync()
        {
            // 市場専用のマネージャ系がないのでここでいい感じに頑張る。
            var dirInfo = Mediator.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Smile));
            var cachedDirPath = Path.Combine(dirInfo.FullName, Constants.SmileMarketCacheDirectoryName);
            var marketDir = Directory.CreateDirectory(cachedDirPath);

            Setting.Bookmark.Correction();

            var userAgentHost = new HttpUserAgentHost(NetworkSetting, Mediator.Logger);
            var client = userAgentHost.CreateHttpUserAgent();
            var tasks = MarketItems
                .Where(i => !i.IsStandby)
                .Select(i => i.LoadThumbnaiImageAsync(Constants.ServiceSmileMarketImageCacheSpan, client));

            return Task.WhenAll(tasks).ContinueWith(t => {
                t.Dispose();
                client.Dispose();
                userAgentHost.Dispose();
            });

        }

        protected virtual Task LoadMarketItemsAsync()
        {
            MarketLoadState = LoadState.Preparation;

            MarketLoadState = LoadState.Loading;
            return Information.LoadMarketItemsAsync().ContinueWith(t => {
                var items = t.Result;
                if(items.Any()) {
                    SetMarketItems(items);
                    return LoadMarketItemsImageAsync().ContinueWith(_ => {
                        MarketLoadState = LoadState.Loaded;
                    });
                } else {
                    MarketLoadState = LoadState.Loaded;
                    return Task.CompletedTask;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void SetMarketItems(IEnumerable<SmileMarketVideoRelationItemViewModel> items)
        {
            if(!IsViewClosed) {
                Application.Current.Dispatcher.Invoke(() => {
                    MarketItems.InitializeRange(items);
                });
            }
        }

        void CheckTagPedia_Issue665NA()
        {
            Debug.Assert(Information.PageHtmlLoadState == LoadState.Loaded);

            IsCheckedTagPedia = true;

            var htmlDocument = new HtmlDocument() {
                OptionAutoCloseOnEnd = true,
            };
            htmlDocument.LoadHtml(Information.WatchPageHtmlSource_Issue665NA);
            var json = SmileVideoWatchAPI_Issue665NA_Utility.ConvertJsonFromWatchPage(Information.WatchPageHtmlSource_Issue665NA);
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
                Application.Current?.Dispatcher.BeginInvoke(new Action(() => {
                    CommandManager.InvalidateRequerySuggested();
                }), DispatcherPriority.ApplicationIdle);
            }
        }

        protected virtual void CheckTagPedia()
        {
            Debug.Assert(Information.PageHtmlLoadState == LoadState.Loaded);

            IsCheckedTagPedia = true;

            if(Information.IsCompatibleIssue665NA) {
                CheckTagPedia_Issue665NA();
                return;
            }

            if(Information.WatchTagItems.Any()) {
                var map = TagItems.ToDictionary(tk => tk.TagName, tv => tv);
                foreach(var tagItem in Information.WatchTagItems) {
                    SmileVideoTagViewModel tag;
                    if(map.TryGetValue(tagItem.Name, out tag)) {
                        tag.ExistPedia = RawValueUtility.ConvertBoolean(tagItem.IsDictionaryExists);
                    }
                }
                Application.Current?.Dispatcher.BeginInvoke(new Action(() => {
                    CommandManager.InvalidateRequerySuggested();
                }), DispatcherPriority.ApplicationIdle);
            }
        }

        void AddBookmark(SmileVideoBookmarkNodeViewModel bookmarkNode)
        {
            var singleVideoItems = new[] { Information.ToVideoItemModel() };
            Mediator.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessBookmarkParameterModel(bookmarkNode, singleVideoItems, true)));
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
            var myList = new Logic.Service.Smile.Api.V1.MyList(Mediator);
            return myList.AdditionAccountDefaultMyListFromVideo(VideoId, Information.PageVideoToken);
        }

        Task<SmileJsonResultModel> AddAccountMyListAsync(SmileVideoMyListFinderViewModelBase myListFinder)
        {
            var myList = new Logic.Service.Smile.Api.V1.MyList(Mediator);
            return myList.AdditionAccountMyListFromVideo(myListFinder.MyListId, Information.ThreadId, Information.PageVideoToken);
        }

        void SearchTag(SmileVideoTagViewModel tagViewModel)
        {
            var parameter = new SmileVideoSearchParameterModel() {
                SearchType = SearchType.Tag,
                Query = tagViewModel.TagName,
            };

            Mediator.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, parameter, ShowViewState.Foreground));
        }

        void OpenUserId(string userId)
        {
            SmileDescriptionUtility.OpenUserId(userId, Mediator);
        }

        void OpenChannelId(string channelId)
        {
            SmileDescriptionUtility.OpenChannelId(channelId, Mediator, Mediator);
        }

        Task OpenVideoLinkAsync(string videoId)
        {
            var cancel = new CancellationTokenSource();
            var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));
            return Mediator.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request).ContinueWith(t => {
                if(t.Status == TaskStatus.Faulted) {
                    Mediator.Logger.Error(t.Exception);
                    return Task.CompletedTask;
                }
                var videoInformation = t.Result;
                if(SmileVideoInformationUtility.CheckCanPlay(videoInformation, Mediator.Logger)) {
                    return LoadAsync(videoInformation, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
                } else {
                    return Task.CompletedTask;
                }
            }, cancel.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task AddPlayListAync(string videoId)
        {
            var information = await SmileDescriptionUtility.GetVideoInformationAsync(videoId, Mediator);
            if(information != null) {
                AddPlayList(information);
            }
        }

        public void AddPlayList(SmileVideoInformationViewModel information)
        {
            if(information == null) {
                throw new ArgumentNullException(nameof(information));
            }

            if(PlayListItems.All(i => !SmileVideoInformationUtility.IsEquals(information, i))) {
                PlayListItems.Add(information);
                CanPlayNextVideo.Value = true;
            }
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
                .Join(Mediator.Smile.VideoMediator.Filtering.Elements, d => d, e => e.Key, (d, e) => e)
                .Select(e => SmileVideoCommentUtility.ConvertFromDefined(e))
                .ToEvaluatedSequence()
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

        /// <summary>
        /// 次(前)の動画を安全に取得する。
        /// </summary>
        /// <param name=""></param>
        /// <returns>動画。null で元動画。</returns>
        SmileVideoInformationViewModel GetSafeChangeItem(SmileVideoInformationViewModel current, Func<SmileVideoInformationViewModel> getNextItem)
        {
            var targetViewModel = getNextItem();

            while(!SmileVideoInformationUtility.CheckCanPlay(targetViewModel, Mediator.Logger)) {
                targetViewModel = getNextItem();
                if(targetViewModel == current) {
                    // 一周したならもう何もしない
                    return null;
                }
            }

            return targetViewModel;
        }

        Task LoadNextPlayListItemAsync()
        {
            var targetViewModel = GetSafeChangeItem(Information, PlayListItems.ChangeNextItem);

            if(targetViewModel == null) {
                return Task.CompletedTask;
            }

            return LoadAsync(targetViewModel, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        Task LoadPrevPlayListItemAsync()
        {
            var targetViewModel = GetSafeChangeItem(Information, PlayListItems.ChangePrevItem);

            if(targetViewModel == null) {
                return Task.CompletedTask;
            }

            return LoadAsync(targetViewModel, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        void AddHistoryCore(SmileVideoPlayHistoryModel historyModel)
        {
            //Setting.History.Insert(0, historyModel);
            AppUtility.AddHistoryItem(Setting.History, historyModel);
        }

        protected virtual void AddHistory(SmileVideoInformationViewModel information)
        {
            var historyModel = Setting.History.FirstOrDefault(f => f.VideoId == information.VideoId);
            if(historyModel == null) {
                historyModel = new SmileVideoPlayHistoryModel() {
                    VideoId = information.VideoId,
                    VideoTitle = information.Title,
                    Length = information.Length,
                    FirstRetrieve = information.FirstRetrieve,
                };
            } else {
                Setting.History.Remove(historyModel);
            }
            historyModel.WatchUrl = information.WatchUrl;
            historyModel.LastTimestamp = DateTime.Now;
            historyModel.Count = RangeUtility.Increment(historyModel.Count);

            AddHistoryCore(historyModel);
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
            foreach(var comment in ShowingCommentList.ToEvaluatedSequence()) {
                comment.ViewModel.ChangeActualContent();
                comment.ViewModel.ChangeTextShow();
            }
        }

        void ChangedCommentFillBackground()
        {
            foreach(var comment in OriginalPosterCommentList) {
                comment.FillBackground = FillBackgroundOriginalPoster;
            }
        }

        void ChangedCommentFps()
        {
            //NOTE: 実行中アニメーションは特に意味なさげ
            //foreach(var comment in ShowingCommentList.ToArray()) {
            //    Timeline.SetDesiredFrameRate(comment.Animation, CommentFps);
            //}
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
            CommentStyleSetting.Fps = Constants.SettingServiceSmileVideoPlayerCommentFps;
            CommentStyleSetting.ShowTime = Constants.SettingServiceSmileVideoCommentShowTime;
            CommentStyleSetting.ConvertPairYenSlash = Constants.SettingServiceSmileVideoCommentConvertPairYenSlash;
            CommentStyleSetting.TextShowMode = Constants.SettingServiceSmileVideoCommentTextShowMode;
            FillBackgroundOriginalPoster = Constants.SettingServiceSmileVideoCommentFillBackgroundOriginalPoster;

            ChangedCommentFont();
            ChangedCommentContent();
            ChangedCommentFillBackground();
            ResetCommentInformation();

            var propertyNames = new[] {
                nameof(CommentFontFamily),
                nameof(CommentFontSize),
                nameof(CommentFontBold),
                nameof(CommentFontItalic),
                nameof(CommentFontAlpha),
                nameof(CommentFps),
                nameof(CommentShowTime),
                nameof(CommentConvertPairYenSlash),
                nameof(PlayerTextShowMode),
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

        protected SmileVideoCommentViewModel CreateSingleComment(RawSmileVideoMsgChatResultModel msgChat, TimeSpan videoPosition)
        {
            var commentModel = new RawSmileVideoMsgChatModel() {
                Mail = string.Join(" ", PostCommandItems),
                Content = PostCommentBody,
                Date = RawValueUtility.ConvertRawUnixTime(DateTime.Now).ToString(),
                No = msgChat.No,
                VPos = SmileVideoMsgUtility.ConvertRawElapsedTime(videoPosition),
                Thread = msgChat.Thread,
            };
            var commentViewModel = new SmileVideoCommentViewModel(commentModel, CommentStyleSetting) {
                IsMyPost = true,
                Approval = true,
            };

            return commentViewModel;
        }

        protected void AppendComment(SmileVideoCommentViewModel comment, bool isShow)
        {
            if(isShow) {
                SmileVideoCommentUtility.ShowSingleComment(comment, NormalCommentArea, GetCommentArea(false), PrevPlayedTime, ShowingCommentList, CommentStyleSetting, null);
            }

            NormalCommentList.Add(comment);
            CommentList.Add(comment);
            CommentItems.Refresh();

            ResetCommentInformation();
        }

        protected async Task PostComment_Issue665NA_Async(TimeSpan videoPosition)
        {
            // #548
            //if(CommentThread == null) {
            //    var rawMessagePacket = await LoadMsgCoreAsync(0, 0, 0, 0, 0);
            //    ImportCommentThread(rawMessagePacket);
            //}
            var rawMessagePacket = await LoadMsg_Issue665NA_CoreAsync(0, 0, 0, 0, 0);
            var CommentThread = rawMessagePacket.Thread.First(t => string.IsNullOrWhiteSpace(t.Fork));

            var commentCount = RawValueUtility.ConvertInteger(CommentThread.LastRes ?? "0");
            Debug.Assert(CommentThread.Thread == Information.ThreadId);

            var msg = new Msg(Mediator);

            var postKey = await msg.LoadPostKeyAsync(Information.ThreadId, commentCount);
            if(postKey == null) {
                SetCommentInformation(Properties.Resources.String_App_Define_Service_Smile_Video_Comment_PostKeyError);
                return;
            } else {
                Mediator.Logger.Information($"postkey = {postKey}");
            }

            var resultPost = await msg.Post_Issue665NA_Async(
                Information.MessageServerUrl,
                Information.ThreadId,
                videoPosition,
                CommentThread.Ticket,
                postKey.PostKey,
                PostCommandItems,
                PostCommentBody
            );
            if(resultPost == null) {
                SetCommentInformation($"fail: {nameof(msg.Post_Issue665NA_Async)}");
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
            var commentViewModel = CreateSingleComment(resultPost.ChatResult, videoPosition);

            AppendComment(commentViewModel, true);

            // 次回読み込み時にコメントを更新できるようにタイムスタンプを過去にしておく
            MarkCommentCacheTimestamp_Issue665NA();
        }

        protected virtual async Task PostCommentAsync(TimeSpan videoPosition)
        {
            // #548 対策
            var rawMessagePacket = await LoadMsgCoreAsync(0, new SmileVideoMsgRangeModel(0, 0, 0, 0));
            var packetIdKey = Information.IsChannelVideo
                ? SmileVideoMsgPacketId.Community
                : SmileVideoMsgPacketId.Normal
            ;
            int packetId;
            if(!rawMessagePacket.PacketId.TryGetValue(packetIdKey, out packetId)) {
                // TODO: ユーザ表示なし
                SetCommentInformation($"packet id not fond: {nameof(packetIdKey)} = {packetIdKey}");
                return;
            }

            var tagetItem = rawMessagePacket.Items
                .Where(i => i.Leaf == null || i.GlobalNumRes == null)
                .SkipWhile(i => i.Ping == null || (i.Ping != null && i.Ping.Content != $"ps:{packetId}"))
                .FirstOrDefault(i => i.Thread != null)
            ;
            if(tagetItem == null) {
                // TODO: ユーザ表示なし
                SetCommentInformation($"not found `ps:{packetId}` thread");
                return;
            }

            var commentCount = RawValueUtility.ConvertInteger(tagetItem.Thread.LastRes ?? "0");

            var msg = new Msg(Mediator);

            var postKey = await msg.LoadPostKeyAsync(tagetItem.Thread.Thread, commentCount);
            if(postKey == null) {
                SetCommentInformation(Properties.Resources.String_App_Define_Service_Smile_Video_Comment_PostKeyError);
                return;
            } else {
                Mediator.Logger.Information($"postkey = {postKey}");
            }

            var resultPost = await msg.PostAsync(
                Information.MessageServerUrl,
                tagetItem.Thread.Thread,
                videoPosition,
                tagetItem.Thread.Ticket,
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
            var commentViewModel = CreateSingleComment(resultPost.ChatResult, videoPosition);

            AppendComment(commentViewModel, true);

            // 次回読み込み時にコメントを更新できるようにタイムスタンプを過去にしておく
            MarkCommentCacheTimestamp();
        }

        void SetCommentInformation(string text)
        {
            CommentInformation = text;
            Mediator.Logger.Trace(text);
        }

        protected void ResetCommentInformation()
        {
            CommentInformation = string.Empty;
            PostCommentBody = string.Empty;

            CommandManager.InvalidateRequerySuggested();
        }

        void SetLocalFiltering()
        {
            if(View != null) {
                View.localFilter.Filtering = LocalCommentFilering;
            }
        }

        void SwitchFullScreen()
        {
            SetWindowMode(!IsNormalWindow);
        }

        void SetWindowMode(bool toNormalWindow)
        {
            IsNormalWindow = toNormalWindow;

            if(IsViewClosed) {
                Mediator.Logger.Debug("view: closed");
                return;
            }

            var hWnd = HandleUtility.GetWindowHandle(View);
            if(toNormalWindow) {
                View.Deactivated -= View_Deactivated;

                State = WindowState.Normal;

                View.UseNoneWindowStyle = false;
                View.ShowTitleBar = true; // <-- this must be set to true
                View.IgnoreTaskbarOnMaximize = false;

                if(PrevFullScreenState == WindowState.Maximized) {
                    View.Dispatcher.Invoke(() => {
                        State = PrevFullScreenState;
                    }, DispatcherPriority.SystemIdle);
                }

                ResetFocus();

            } else {
                PrevFullScreenState = State;

                View.IgnoreTaskbarOnMaximize = true;
                State = WindowState.Maximized;
                View.UseNoneWindowStyle = true;

                View.Dispatcher.BeginInvoke(new Action(() => {
                    View.Deactivated += View_Deactivated;
                    ResetFocus();
                }), DispatcherPriority.SystemIdle);
            }
        }

        void ResetFocus()
        {
            // #164: http://stackoverflow.com/questions/2052389/wpf-reset-focus-on-button-click
            var scope = FocusManager.GetFocusScope(View);
            FocusManager.SetFocusedElement(scope, null);
            Keyboard.ClearFocus();
            Keyboard.Focus(View);
        }

        public void MoveForeground()
        {
            if(View == null) {
                return;
            }

            // 経験則上これが一番確実という悲しさ
            if(!View.Topmost) {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    View.Topmost = true;
                }), DispatcherPriority.SystemIdle).Task.ContinueWith(t => {
                    t.Dispose();
                    ChangeTopmostState();
                    View.Activate();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        void ChangeTopmostState()
        {
            if(View == null) {
                return;
            }

            if(IsNormalWindow) {
                switch(TopmostKind) {
                    case Define.UI.Player.TopmostKind.Default:
                        View.Topmost = false;
                        break;

                    case Define.UI.Player.TopmostKind.Playing:
                        View.Topmost = PlayerState == PlayerState.Playing;
                        if(!View.Topmost && !View.IsActive) {
                            var hWnd = NativeMethods.GetForegroundWindow();
                            WindowsUtility.ShowActive(hWnd);
                        } else if(View.Topmost && !View.IsActive) {
                            if(Constants.ServiceSmileVideoPlayerActiveTopmostPlayingRestart) {
                                View.Activate();
                            }
                        }
                        break;

                    case Define.UI.Player.TopmostKind.Always:
                        View.Topmost = true;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            } else {
                View.Topmost = true;
            }
        }

        void ChangeVolume(bool isUp)
        {
            var step = isUp ? PlayerSetting.VolumeOperationStep : -PlayerSetting.VolumeOperationStep;
            var setVolume = Volume + step;
            Volume = RangeUtility.Clamp(setVolume, Constants.NavigatorVolumeRange);
        }

        void SwitchMute()
        {
            IsMute = !IsMute;
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

            ResetFocus();
        }

        void AddCommentFilter(SmileVideoCommentFilteringTarget target, string source, bool setGlobalSetting)
        {
            Mediator.Logger.Debug($"{target}: {source}, global: {setGlobalSetting}");

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
        /// <para>スレッドIDかなーと思ったけどそんなこともなかったぞ！IDの扱いがわけわからん！</para>
        /// </summary>
        /// <param name="videoId">動画ID。</param>
        protected virtual void SetCheckedCheckItLater(string videoId, Uri watchUrl)
        {
            var later = Setting.CheckItLater
                .Where(c => !c.IsChecked)
                .FirstOrDefault(c => c.VideoId == videoId || c.WatchUrl?.OriginalString == watchUrl?.OriginalString)
            ;
            if(later != null) {
                later.IsChecked = true;
                later.CheckTimestamp = DateTime.Now;
            }
        }

        void SeekHead()
        {
            if(Player != null && Player.IsSeekable) {
                ClearComment();
                Player.Position = 0;

                ResetFocus();
            }
        }

        void AttachEvent()
        {
            EnabledCommentControl.MouseEnter += EnabledCommentControl_MouseEnter;
            EnabledCommentControl.MouseLeave += EnabledCommentControl_MouseLeave;

            View.Loaded += View_Loaded;
            View.Activated += View_Activated;
            View.KeyDown += View_KeyDown;
            View.KeyUp += View_KeyUp;
            View.Closing += View_Closing;
            Player.MouseDown += Player_MouseDown;
            Player.PositionChanged += Player_PositionChanged;
            Player.SizeChanged += Player_SizeChanged;
            Player.StateChanged += Player_StateChanged;
            Player.MouseWheel += Player_MouseWheel;
            //AttachmentNavigationbarBaseEvent(Navigationbar);
            DetailComment.LostFocus += DetailComment_LostFocus;
        }

        void DetachEvent()
        {
            if(View != null) {
                View.Loaded -= View_Loaded;
                View.Activated -= View_Activated;
                View.KeyDown -= View_KeyDown;
                View.KeyUp -= View_KeyUp;
                View.Closing -= View_Closing;
                View.Deactivated -= View_Deactivated;
            }

            if(Player != null) {
                Player.MouseDown -= Player_MouseDown;
                Player.PositionChanged -= Player_PositionChanged;
                Player.SizeChanged -= Player_SizeChanged;
                Player.StateChanged -= Player_StateChanged;
                Player.MouseWheel -= Player_MouseWheel;
                Player.Loaded -= Player_LoadedAutoPlay;
            }
            //if(Navigationbar != null) {
            //    DetachmentNavigationbarBaseEvent(Navigationbar);
            //}

            if(EnabledCommentControl != null) {
                EnabledCommentControl.MouseEnter -= EnabledCommentControl_MouseEnter;
                EnabledCommentControl.MouseLeave -= EnabledCommentControl_MouseLeave;
            }

            if(DetailComment != null) {
                DetailComment.LostFocus += DetailComment_LostFocus;
            }
        }

        bool IsPrimaryDisplayInView()
        {
            var hWnd = HandleUtility.GetWindowHandle(View);
            var screenModel = Screen.FromHandle(hWnd);

            return screenModel.Primary;
        }

        void ResumeBufferingStop()
        {
            if(!UserOperationStop.Value && !IsViewClosed) {
                PrevPlayedTime = BufferingVideoTime;
                Player.Stop();
                PlayMovie();
                Player.Time = BufferingVideoTime;
            }
        }

        void ChangedPlayerStateToStop(Meta.Vlc.ObjectEventArgs<Meta.Vlc.Interop.Media.MediaState> e)
        {
            if(VideoLoadState == LoadState.Loading) {
                // ダウンロードが完了していない
                Mediator.Logger.Debug("buffering stop");

                SafeShowTime = Player.Time;
                SafeDownloadedSize = VideoLoadedSize;

                var position = TotalTime.TotalMilliseconds * VideoPosition;
                BufferingVideoTime = TimeSpan.FromMilliseconds(position);
                PlayerState = PlayerState.Buffering;

                return;
            }

            if(CanPlayNextVideo.Value && PlayListItems.Skip(1).Any()) {
                // 次のプレイリストへ遷移
                Mediator.Logger.Debug("next playlist item");
                LoadNextPlayListItemAsync();
                return;
            }

            if(ReplayVideo) {
                // リプレイ
                Mediator.Logger.Debug("replay");
                StopMovie(true);
                PlayMovie();

                return;
            }

            // 普通の停止
            StopMovie(false);

            if(!IsNormalWindow && PlayerSetting.StopFullScreenRestore) {
                //フルスクリーン状態の制御
                var restoreNormalWindow = true;
                if(PlayerSetting.StopFullScreenRestorePrimaryDisplayOnly) {
                    restoreNormalWindow = IsPrimaryDisplayInView();
                }
                if(restoreNormalWindow) {
                    SetWindowMode(true);
                }
            }
        }

        void ChangeSelectedComment(bool isUp)
        {
            var comments = CommentItems
                .Cast<SmileVideoCommentViewModel>()
                .ToEvaluatedSequence()
            ;
            var currentIndex = comments.IndexOf(SelectedComment);
            if(currentIndex == -1) {
                Mediator.Logger.Debug("current comment is null");
            }

            if(isUp && 0 < currentIndex) {
                SelectedComment = comments[currentIndex - 1];
            } else if(!isUp && currentIndex < comments.Count - 1) {
                SelectedComment = comments[currentIndex + 1];
            }
        }

        void OpenIdleTalkMutter(bool openDefaultBrowser)
        {
            var smileSetting = Mediator.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));

            var serviceType = ServiceType.IdleTalkMutter;
            var key = IdleTalkMutterMediatorKey.postPage;

            var map = new StringsModel() {
                ["url"] = smileSetting.IdleTalkMutter.AutoInputWatchPageUri ? Information.WatchUrl.OriginalString : string.Empty,
                ["text"] = smileSetting.IdleTalkMutter.AutoInputVideoTitle ? Information.Title : string.Empty,
                ["via"] = string.Empty,
                ["in_reply_to"] = string.Empty,
                ["related"] = string.Empty,
                ["original_referer"] = string.Empty,
                ["hashtags"] = smileSetting.IdleTalkMutter.AutoInputHashTags,
                ["lang"] = string.Empty,
            };

            var rawUri = Mediator.GetUri(key, map, serviceType);
            var convertedUri = Mediator.ConvertUri(key, rawUri.Uri, serviceType);
            Uri uri;
            if(Uri.TryCreate(convertedUri, UriKind.RelativeOrAbsolute, out uri)) {
                if(openDefaultBrowser) {
                    ShellUtility.OpenUriInDefaultBrowser(uri, Mediator.Logger);
                } else {
                    DescriptionUtility.OpenUriInAppBrowser(uri, Mediator);
                }
            }

        }

        void SavePlayListToBookmark()
        {
            var items = PlayListItems.Select(i => i.ToVideoItemModel());

            if(IsNewBookmark) {
                var newBookmark = new SmileVideoBookmarkItemSettingModel() {
                    Name = NewBookmarkName,
                };
                newBookmark.Items.AddRange(items);
                var nodeViewModel = Mediator.GetResultFromRequest<SmileVideoBookmarkNodeViewModel>(new SmileVideoProcessRequestModel(new SmileVideoProcessBookmarkParameterModel(null, newBookmark)));
                SelectedBookmark = nodeViewModel;

                // View 側初期化
                NewBookmarkName = string.Empty;
                this._bookmarkItems = null;
                CallOnPropertyChange(nameof(BookmarkItems));
            } else {
                Mediator.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessBookmarkParameterModel(SelectedBookmark, items, false)));
            }
        }

        void ScrollActiveVideoInPlayList(SmileVideoInformationViewModel videoInformation)
        {
            ListPlaylist?.ScrollToCenterOfView(videoInformation, true, false);
        }

        IEnumerable<SmileVideoPlayerViewModel> GetEnabledPlayers()
        {
            var players = Mediator.GetResultFromRequest<IEnumerable<SmileVideoPlayerViewModel>>(new RequestModel(RequestKind.WindowViewModels, ServiceType.SmileVideo))
                .Where(p => !(p is SmileVideoLaboratoryPlayerViewModel))
            ;

            return players;
        }

        void MarkCommentCacheTimestampCore(FileInfo file)
        {
            if(file == null) {
                return;
            }
            file.Refresh();
            if(!file.Exists) {
                return;
            }
            var backTime = DateTime.Now - Constants.ServiceSmileVideoMsgCacheTime;
            try {
                var createDiffTime = file.LastWriteTime - file.CreationTime;
                file.CreationTime = backTime - createDiffTime;
                file.LastWriteTime = backTime;
            } catch(Exception ex) {
                Mediator.Logger.Error(ex);
            }
        }

        void MarkCommentCacheTimestamp_Issue665NA()
        {
            MarkCommentCacheTimestampCore(Information.MsgFile_Issue665NA);
        }
        void MarkCommentCacheTimestamp()
        {
            MarkCommentCacheTimestampCore(Information.MsgFile);
        }

        protected virtual Task LoadPosterAsync()
        {
            CacheSpan dataSpan;
            CacheSpan imageSpan;
            if(Information.IsChannelVideo) {
                var channelId = Information.ChannelId;
                PosterInformation = new SmileChannelInformationViewModel(Mediator, channelId);
                dataSpan = Constants.ServiceSmileChannelDataCacheSpan;
                imageSpan = Constants.ServiceSmileChannelImageCacheSpan;
            } else {
                PosterInformation = new SmileUserInformationViewModel(Mediator, Information.UserId, false);
                dataSpan = Constants.ServiceSmileUserDataCacheSpan;
                imageSpan = Constants.ServiceSmileUserImageCacheSpan;
            }

            return PosterInformation.LoadInformationDefaultAsync(dataSpan).ContinueWith(_ => {
                return PosterInformation.LoadThumbnaiImageDefaultAsync(imageSpan);
            }).Unwrap().ContinueWith(_ => {
                if(PosterInformation.ThumbnailLoadState == LoadState.Loaded) {
                    CallOnPropertyChange(nameof(PosterThumbnailImage));
                }
            });
        }

        void ReceiveTaskbarThumbnailThickness(Thickness thickness)
        {
            ThumbnailClipMargin = thickness;
        }

        #endregion

        #region SmileVideoDownloadViewModel

        public override Task LoadAsync(SmileVideoInformationViewModel videoInformation, bool forceEconomy, CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            // TODO:forceEconomyは今のところ無効

            PlayerTaskbarThumbnailCreator?.Reset();

            if(PlayListItems.All(i => i != videoInformation)) {
                // プレイリストに存在しない動画は追加する
                PlayListItems.Add(videoInformation);
                //#371
                CanPlayNextVideo.Value = false;
            } else {
                // プレイリストに存在するのであればカレントを設定
                PlayListItems.ChangeCurrentItem(videoInformation);
            }

            videoInformation.IsPlaying = true;
            videoInformation.LastShowTimestamp = DateTime.Now;
            IsSelectedInformation = true;

            // プレイヤー立ち上げ時は null
            if(Information != null) {
                Information.IsPlaying = false;
                Information.SaveSetting(false);
                // LOHも含めてGC
                Mediator.Order(new AppCleanMemoryOrderModel(true, false));
            }

            AddHistory(videoInformation);

            SetCheckedCheckItLater(videoInformation.VideoId, videoInformation.WatchUrl);

            ScrollActiveVideoInPlayList(videoInformation);

            return base.LoadAsync(videoInformation, false, thumbCacheSpan, imageCacheSpan);
        }

        protected override void OnDownloadStart(object sender, DownloadStartEventArgs e)
        {
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
                    CanVideoPlay = PlayerSetting.AutoPlayLowestSize < VideoFile.Length;
                    if(CanVideoPlay) {
                        StartIfAutoPlay();
                    }
                } else if(PlayerState == PlayerState.Buffering) {
                    VideoFile.Refresh();
                    var canVideoPlay = (SafeDownloadedSize + PlayerSetting.AutoPlayLowestSize) < VideoFile.Length;
                    if(canVideoPlay) {
                        ResumeBufferingStop();
                    }
                }

            }

            e.Cancel |= IsViewClosed || (DownloadCancel != null && DownloadCancel.IsCancellationRequested);
            if(e.Cancel) {
                StopMovie(true);

                Information.IsDownloading = false;
            }
            SecondsDownloadingSize = e.SecondsDownlodingSize;

            base.OnDownloading(sender, e);
        }

        void ResetSwf()
        {
            PlayFile?.Refresh();
        }

        protected override void OnLoadVideoEnd()
        {
            if(IsViewClosed) {
                return;
            }

            if(DownloadCancel == null || !DownloadCancel.IsCancellationRequested) {
                if(Information.PageHtmlLoadState == LoadState.Loaded) {
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

                    if(PlayerState == PlayerState.Buffering) {
                        ResumeBufferingStop();
                    }
                }
            }

            base.OnLoadVideoEnd();
        }


        protected override Task LoadComment_Issue665NA_Async(RawSmileVideoMsgPacket_Issue665NA_Model rawMsgPacket)
        {
            var comments = SmileVideoCommentUtility.CreateCommentViewModels_Issue665NA(rawMsgPacket, CommentStyleSetting);
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

            ChangedCommentFillBackground();
            ApprovalComment();

            if(FilteringCommentType != SmileVideoFilteringCommentType.All) {
                RefreshFilteringComment();
            }

            return base.LoadComment_Issue665NA_Async(rawMsgPacket);
        }

        protected override Task LoadCommentAsync(SmileVideoMsgPackSettingModel rawMsgPacket)
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

            ChangedCommentFillBackground();
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
            if(Session.IsLoggedIn && Session.IsPremium && CommandColorItems.Count == SmileVideoMsgUtility.normalCommentColors.Count) {
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
                nameof(PosterInformation),
                nameof(PosterThumbnailImage),
            };
            CallOnPropertyChange(propertyNames);

            LocalCommentFilering = new SmileVideoFilteringViweModel(Information.Filtering, null, Mediator.Smile.VideoMediator.Filtering);
            SetLocalFiltering();

            base.OnLoadGetthumbinfoEnd();
        }

        protected override void OnLoadDataWithoutSessionEnd()
        {
            LoadPosterAsync().ConfigureAwait(false);
        }

        protected override void InitializeStatus()
        {
            base.InitializeStatus();
            WaitingFirstPlay.Value = true;
            VideoPosition = 0;
            PrevPlayedTime = TimeSpan.Zero;
            this._prevStateChangedPosition = initPrevStateChangedPosition;
            UserOperationStop.Value = false;
            IsCheckedTagPedia = false;
            //ChangingVideoPosition = false;
            //MovingSeekbarThumb = false;
            Navigationbar?.ResetStatus();
            CanVideoPlay = false;
            IsVideoPlayng = false;
            PlayTime = TimeSpan.Zero;
            TotalTime = TimeSpan.Zero;
            SelectedComment = null;
            IsSettedMedia = false;
            ShowCommentChart = false;
            BufferingVideoTime = TimeSpan.Zero;
            SafeShowTime = TimeSpan.Zero;
            SafeDownloadedSize = 0;
            CommentScriptDefault = null;
            MarketLoadState = LoadState.None;
            ForceNavigatorbarOperation = false;
            PosterInformation = null;

            CommentAreaWidth = Constants.ServiceSmileVideoPlayerCommentWidth;
            CommentAreaHeight = Constants.ServiceSmileVideoPlayerCommentHeight;
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
            SetRelationVideoItems(Enumerable.Empty<SmileVideoInformationViewModel>());
            SetMarketItems(Enumerable.Empty<SmileMarketVideoRelationItemViewModel>());
            //TagItems.Clear();
            PlayerTaskbarThumbnailCreator?.Reset();
        }

        protected override Task StopPrevProcessAsync()
        {
            if(Player != null) {
                if(Player.State != Meta.Vlc.Interop.Media.MediaState.Stopped) {
                    StopMovie(true);
                }
            }

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
            ListPlaylist = View.listPlayList;

            PlayerCursorHider = new Logic.View.CursorHider(Player);
            PlayerTaskbarThumbnailCreator = new SmileVideoTaskbarThumbnailCreator(View, ReceiveTaskbarThumbnailThickness, ViewScale);

            // 初期設定
            Player.Volume = Volume;
            SetLocalFiltering();

            // あれこれイベント, すっごいキモいことになってる
            var content = Navigationbar.ExstendsContent as Panel;
            EnabledCommentControl = UIUtility.FindLogicalChildren<Control>(content).ElementAt(1);

            AttachEvent();

            Debug.WriteLine(View.IsInitialized);
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

        #region ISmileDescription

        public ImageSource DefaultBrowserIcon { get; } = WebNavigatorCore.DefaultBrowserIcon;

        public ICommand OpenUriCommand
        {
            get { return CreateCommand(o => DescriptionUtility.OpenUri(o, Mediator.Logger)); }
        }
        public ICommand MenuOpenUriCommand => OpenUriCommand;
        public ICommand MenuOpenUriInAppBrowserCmmand
        {
            get { return CreateCommand(o => DescriptionUtility.OpenUriInAppBrowser(o, Mediator)); }
        }
        public ICommand MenuCopyUriCmmand { get { return CreateCommand(o => DescriptionUtility.CopyUri(o, Mediator.Logger)); } }

        public ICommand OpenVideoIdLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var videoId = o as string;
                        OpenVideoLinkAsync(videoId).ConfigureAwait(false);
                    }
                    , o => !string.IsNullOrWhiteSpace((string)o)
                );
            }
        }
        public ICommand MenuOpenVideoIdLinkCommand => OpenVideoIdLinkCommand;
        public ICommand MenuOpenVideoIdLinkInNewWindowCommand { get { return CreateCommand(o => SmileDescriptionUtility.MenuOpenVideoLinkInNewWindowAsync(o, Mediator).ConfigureAwait(false), o => !string.IsNullOrWhiteSpace((string)o)); } }
        public ICommand MenuCopyVideoIdCommand { get { return CreateCommand(o => SmileDescriptionUtility.CopyVideoId(o, Mediator.Logger)); } }
        public ICommand MenuAddPlayListVideoIdLinkCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var videoId = (string)o;
                        if(string.IsNullOrWhiteSpace(videoId)) {
                            return;
                        }

                        AddPlayListAync(videoId).ConfigureAwait(false);
                    }
                );
            }
        }
        public ICommand MenuAddCheckItLaterVideoIdCommand
        {
            get { return CreateCommand(o => SmileDescriptionUtility.AddCheckItLaterVideoIdAsync(o, Mediator, Mediator.Smile.VideoMediator.ManagerPack.CheckItLaterManager).ConfigureAwait(false)); }
        }
        public ICommand MenuAddUnorganizedBookmarkVideoIdCommand
        {
            get { return CreateCommand(o => SmileDescriptionUtility.AddUnorganizedBookmarkAsync(o, Mediator, Mediator.Smile.VideoMediator.ManagerPack.BookmarkManager).ConfigureAwait(false)); }
        }

        public ICommand OpenMyListIdLinkCommand { get { return CreateCommand(o => SmileDescriptionUtility.OpenMyListId(o, Mediator)); } }
        public ICommand MenuOpenMyListIdLinkCommand => OpenMyListIdLinkCommand;
        public ICommand MenuAddMyListIdLinkCommand { get { return CreateCommand(o => SmileDescriptionUtility.AddMyListBookmarkAsync(o, Mediator).ConfigureAwait(false)); } }
        public ICommand MenuCopyMyListIdCommand { get { return CreateCommand(o => SmileDescriptionUtility.CopyMyListId(o, Mediator.Logger)); } }

        public ICommand OpenUserIdLinkCommand
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
        public ICommand OpenChannelIdLinkCommand { get { return CreateCommand(o => SmileDescriptionUtility.OpenChannelId((string)o, Mediator, Mediator)); } }


        #endregion

        #region SmileVideoDownloadViewModel

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                DetachEvent();

                if(PlayerCursorHider != null) {
                    PlayerCursorHider.Dispose();
                    PlayerCursorHider = null;
                }

                if(PlayerTaskbarThumbnailCreator != null) {
                    PlayerTaskbarThumbnailCreator.Dispose();
                    PlayerTaskbarThumbnailCreator = null;
                }

                View = null;
                Player = null;
                Navigationbar = null;
                NormalCommentArea = null;
                OriginalPosterCommentArea = null;
                CommentView = null;
                DetailComment = null;
                EnabledCommentControl = null;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region event

        void View_Loaded(object sender, RoutedEventArgs e)
        {
            View.Loaded -= View_Loaded;
        }

        void Player_LoadedAutoPlay(object sender, RoutedEventArgs e)
        {
            Player.Loaded -= Player_LoadedAutoPlay;

            SetMediaAndPlay();
        }

        private void View_Closing(object sender, CancelEventArgs e)
        {
            //TODO: closingはまずくね…?
            IsViewClosed = true;

            if(Player.State == Meta.Vlc.Interop.Media.MediaState.Playing) {
                if(DownloadCancel != null) {
                    Mediator.Logger.Trace($"{VideoId}: download cancel! from dmc");
                    DownloadCancel.Cancel();
                    DownloadCancel.Dispose();
                    DownloadCancel = null;
                }

                if(UsingDmc.Value) {
                    StopDmcDownloadAsync();
                }

                StopMovie(true);
            }

            ExportSetting();
            Information.SaveSetting(true);

            Information.IsPlaying = false;

            InitializeStatus();

            try {
                Player.Dispose();
            } catch(Exception ex) {
                Mediator.Logger.Error(ex);
            }
            Mediator.Order(new AppCleanMemoryOrderModel(true, true));
        }

        private void View_KeyDown(object sender, KeyEventArgs e)
        {
            var mods = Keyboard.Modifiers;
            var showNavigator = mods.HasFlag(ModifierKeys.Control | ModifierKeys.Shift);
            if(showNavigator) {
                ForceNavigatorbarOperation = true;
            }
        }
        private void View_KeyUp(object sender, KeyEventArgs e)
        {
            ForceNavigatorbarOperation = false;
        }


        private void Player_PositionChanged(object sender, EventArgs e)
        {
            if(CanVideoPlay && !Navigationbar.ChangingVideoPosition) {
                if(WaitingFirstPlay.Value) {
                    SetVideoDataInformation();
                    WaitingFirstPlay.Value = false;
                }
                VideoPosition = Player.Position;
                PlayTime = Player.Time;
                ShowComments();
                ScrollCommentList();

                PrevPlayedTime = PlayTime;

                Mediator.Order(new AppSystemBreakOrderModel(true));

                ClearResidualComments();
            }
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
            this._prevStateChangedPosition = VideoPosition;

            Mediator.Logger.Debug($"{VideoId}: {e.Value}, pos: {VideoPosition}, time: {PlayTime} / {Player.Length}");
            switch(e.Value) {
                case Meta.Vlc.Interop.Media.MediaState.Playing:
                    var prevState = PlayerState;
                    PlayerState = PlayerState.Playing;
                    PlayerTaskbarThumbnailCreator.Refresh();
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
                    ChangedPlayerStateToStop(e);
                    break;

                default:
                    break;
            }
        }

        private void Player_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var isUp = 0 < e.Delta;
            switch(PlayerSetting.WheelOperation) {
                case Define.UI.Player.WheelOperation.None:
                    break;

                case Define.UI.Player.WheelOperation.Volume:
                    ChangeVolume(isUp);
                    break;

                case Define.UI.Player.WheelOperation.Seek:
                    ChangeSeekVideoPosition(isUp, PlayerSetting.SeekOperationIsPercent, PlayerSetting.SeekOperationIsPercent ? PlayerSetting.SeekOperationPercentStep : PlayerSetting.SeekOperationAbsoluteStep);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void EnabledCommentControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if(CanChangeCommentEnabledArea) {
                ShowEnabledCommentPreviewArea = true;
            }
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
            if(PlayerSetting.InactiveIsFullScreenRestore) {
                var restoreNormalWindow = true;
                if(PlayerSetting.InactiveIsFullScreenRestorePrimaryDisplayOnly) {
                    restoreNormalWindow = IsPrimaryDisplayInView();
                }
                if(restoreNormalWindow) {
                    SetWindowMode(true);
                }
            }

            if(!IsViewClosed) {
                PlayerCursorHider.StartHide();
            }
        }

        void View_Activated(object sender, EventArgs e)
        {
            if(!IsViewClosed) {
                ResetFocus();
                PlayerCursorHider.StartHide();
            }
        }

        void Player_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(PlayerState == PlayerState.Playing) {
                ResetFocus();
                PlayerCursorHider.StartHide();
            }
        }

        #endregion
    }
}
