using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.MultiCommandParameter.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Market;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    // コマンド部分
    partial class SmileVideoPlayerViewModel
    {
        #region command

        public ICommand OpenRelationVideo
        {
            get
            {
                return CreateCommand(
                    o => {
                        var videoInformation = (SmileVideoInformationViewModel)o;
                        if(SmileVideoInformationUtility.CheckCanPlay(videoInformation, Mediation.Logger)) {
                            LoadAsync(videoInformation, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ConfigureAwait(false);
                        }
                    }
                );
            }
        }

        public ICommand OpenMarketCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var marketItem = (SmileMarketVideoRelationItemViewModel)o;
                        ShellUtility.OpenUriInDefaultBrowser(marketItem.CashRegisterUri, Mediation.Logger);
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
                                        SetMediaAndPlay();
                                        break;

                                    default:
                                        //Player.BeginStop(() => {
                                        //    Player.Play();
                                        //});
                                        StopMovie(true);
                                        PlayMovie();
                                        break;
                                }
                                break;

                            case PlayerState.Playing:
                                Player.PauseOrResume();
                                break;

                            case PlayerState.Pause:
                                Mediation.Logger.Debug("resume");
                                View.Dispatcher.BeginInvoke(new Action(() => {
                                    Player.PauseOrResume();
                                }));
                                break;

                            case PlayerState.Buffering:
                                Mediation.Logger.Debug("restart");
                                ResumeBufferingStop();
                                break;

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

        public ICommand ShowRankingCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var tag = (SmileVideoTagViewModel)o;

                        var parameter = new SmileVideoRankingCategoryNameParameterModel() {
                            CategoryName = tag.TagName,
                        };

                        Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, parameter, ShowViewState.Foreground));
                    },
                    o => {
                        var tag = (SmileVideoTagViewModel)o;
                        if(tag == null) {
                            // プレイヤー立ち上げ中はぬるりん
                            return false;
                        }

                        var rankingDefine = Mediation.GetResultFromRequest<IReadOnlySmileVideoRanking>(new RequestModel(RequestKind.RankingDefine, ServiceType.SmileVideo));
                        var rankingCategory = rankingDefine.Items
                            .SelectMany(i => i.Categories)
                            .FirstOrDefault(c => c.Words.Values.Any(s => string.Equals(s, tag.TagName, StringComparison.InvariantCultureIgnoreCase)))
                        ;
                        if(rankingCategory == null) {
                            return false;
                        }

                        return !Setting.Ranking.IgnoreCategoryItems.Any(i => i == rankingCategory.Key);
                    }
                );
            }
        }

        public ICommand OpenPediaInSystemBrowserCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var tag = (SmileVideoTagViewModel)o;
                        var pediaUri = SmilePediaUtility.GetArticleUriFromWord(Mediation, tag.TagName);
                        ShellUtility.OpenUriInDefaultBrowser(pediaUri, Mediation.Logger);
                    },
                    o => {
                        var tag = (SmileVideoTagViewModel)o;
                        return tag?.ExistPedia ?? false;
                    }
                );
            }
        }

        public ICommand OpenPediaInAppBrowserCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var tag = (SmileVideoTagViewModel)o;
                        var pediaUri = SmilePediaUtility.GetArticleUriFromWord(Mediation, tag.TagName);
                        DescriptionUtility.OpenUriInAppBrowser(pediaUri, Mediation);
                    },
                    o => {
                        var tag = (SmileVideoTagViewModel)o;
                        return tag?.ExistPedia ?? false;
                    }
                );
            }
        }

        public ICommand CopyTagCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var tag = (SmileVideoTagViewModel)o;
                        ShellUtility.SetClipboard(tag.TagName, Mediation.Logger);
                    }
                );
            }
        }

        public virtual ICommand OpenUserOrChannelIdCommand
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

        public virtual ICommand OpenCacheDirectoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(Information.CacheDirectory.Exists) {
                            ShellUtility.OpenDirectory(Information.CacheDirectory, Mediation.Logger);
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
                        if(SmileVideoInformationUtility.CheckCanPlay(selectVideoInformation, Mediation.Logger)) {
                            LoadAsync(selectVideoInformation, false, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ConfigureAwait(false);
                        }
                    },
                    o => {
                        var selectVideoInformation = o as SmileVideoInformationViewModel;
                        if(Information == selectVideoInformation) {
                            // 自分自身は活性
                            return true;
                        }
                        // よそで何かしているかもしれない
                        return SmileVideoInformationUtility.CheckCanPlay(selectVideoInformation, Mediation.Logger);
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
                    o => !WaitingFirstPlay.Value && IsSettedMedia && 0 < RealVideoWidth && 0 < RealVideoHeight
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
            get
            {
                return CreateCommand(
                    o => PostCommentAsync(PlayTime).ConfigureAwait(false),
                    o => !string.IsNullOrWhiteSpace(PostCommentBody)
                );
            }
        }

        public ICommand ClearCommentCommand
        {
            get { return CreateCommand(o => ResetCommentInformation()); }
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
                        if(PlayerSetting.MoseClickToPause) {
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
                        if(PlayerSetting.KeySpaceToPause) {
                            PlayCommand.TryExecute(null);
                        }
                    }
                );
            }
        }

        public ICommand SeekHeadCommand
        {
            get { return CreateCommand(o => SeekHead()); }
        }

        public ICommand FullScreenCancelCommand
        {
            get
            {
                return CreateCommand(o => {
                    if(IsNormalWindow) {
                        return;
                    }

                    var fireIsMouse = (bool)o;

                    var canRestore = true;
                    if(fireIsMouse) {
                        canRestore = Mouse.LeftButton == MouseButtonState.Pressed;
                    }

                    if(canRestore) {
                        // フルスクリーン時は元に戻してあげる
                        SetWindowMode(true);
                    }

                    if(!IsViewClosed) {
                        PlayerCursorHider.StartHide();
                    }
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
                        Mediation.Logger.Information($"{param.PositionType}: {PlayerSetting.SeekOperationAbsoluteStep}");
                        switch(param.PositionType) {
                            case SmileVideoChangeVideoPositionType.Setting:
                                ChangeSeekVideoPosition(param.IsNext, PlayerSetting.SeekOperationIsPercent, PlayerSetting.SeekOperationIsPercent ? PlayerSetting.SeekOperationPercentStep : PlayerSetting.SeekOperationAbsoluteStep);
                                break;

                            case SmileVideoChangeVideoPositionType.Percent:
                                ChangeSeekVideoPosition(param.IsNext, true, PlayerSetting.SeekOperationPercentStep);
                                break;

                            case SmileVideoChangeVideoPositionType.Absolute:
                                ChangeSeekVideoPosition(param.IsNext, false, PlayerSetting.SeekOperationAbsoluteStep);
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

        public ICommand SwitchMuteCommand
        {
            get { return CreateCommand(o => SwitchMute()); }
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

        public ICommand ChangeSelectedCommentUpCommand
        {
            get
            {
                return CreateCommand(
                    o => ChangeSelectedComment(true),
                    o => SelectedComment != null
                );
            }
        }

        public ICommand ChangeSelectedCommentDownCommand
        {
            get
            {
                return CreateCommand(
                    o => ChangeSelectedComment(false),
                    o => SelectedComment != null
                );
            }
        }

        public ICommand SwitchPlayerShowCommentAreaCommand
        {
            get
            {
                return CreateCommand(o => {
                    PlayerShowCommentArea = !PlayerShowCommentArea;
                });
            }
        }

        public ICommand SwicthPlayerShowDetailAreaCommand
        {
            get
            {
                return CreateCommand(o => {
                    PlayerShowDetailArea = !PlayerShowDetailArea;
                });
            }
        }

        public ICommand StartSeekChangingCommand
        {
            get
            {
                return CreateCommand(
                    o => ResetFocus(),
                    o => CanVideoPlay
                );
            }
        }

        public ICommand EndSeekChangeCommand
        {
            get
            {
                return CreateCommand(
                    o => MoveVideoPosition((float)o)
                );
            }
        }

        public ICommand OpenDefaultBrowserCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        ShellUtility.OpenUriInDefaultBrowser(Information.WatchUrl, Mediation.Logger);
                    },
                    o => Information != null
                );
            }
        }

        public virtual ICommand SwitchWorkingPlayerCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(!IsViewClosed) {
                            var players = GetEnabledPlayers();
                            if(players.Any(p => p == this)) {
                                foreach(var p in players) {
                                    p.IsWorkingPlayer.Value = false;
                                    p.IsOpenWorkingPlayer = false;
                                }
                                IsWorkingPlayer.Value = true;

                                CommandManager.InvalidateRequerySuggested();
                            }
                        }
                    }
                );
            }
        }

        public ICommand SwitchGlobalCommentFilering
        {
            get
            {
                return CreateCommand(
                    o => {
                        IsEnabledGlobalCommentFilering = !IsEnabledGlobalCommentFilering;
                    },
                    o => Information != null
                );
            }
        }

        public virtual ICommand OpenIdleTalkMutterCommand
        {
            get
            {
                return CreateCommand(
                    o => OpenIdleTalkMutter((bool)o),
                    o => Information != null
                );
            }
        }

        public ICommand SavePlayListToBookmarkCommand
        {
            get
            {
                return CreateCommand(
                    o => SavePlayListToBookmark(),
                    o => PlayListItems.Any() && ((!IsNewBookmark) || (IsNewBookmark && !string.IsNullOrEmpty(NewBookmarkName)))
                );
            }
        }

        public ICommand UpSelectedPlayListItemCommand
        {
            get
            {
                return CreateCommand(
                    o => ItemsControlUtility.MoveItem(PlayListItems, SelectedPlayListItem, true),
                    o => SelectedPlayListItem != null && ItemsControlUtility.CanMoveNext(PlayListItems, SelectedPlayListItem, true)
                );
            }
        }

        public ICommand ReleaseSelectedPlayListItemCommand
        {
            get
            {
                return CreateCommand(
                    o => SelectedPlayListItem = null,
                    o => SelectedPlayListItem != null
                );
            }
        }

        public ICommand DownSelectedPlayListItemCommand
        {
            get
            {
                return CreateCommand(
                    o => ItemsControlUtility.MoveItem(PlayListItems, SelectedPlayListItem, false),
                    o => SelectedPlayListItem != null && ItemsControlUtility.CanMoveNext(PlayListItems, SelectedPlayListItem, false)
                );
            }
        }

        public ICommand RemoveSelectedPlayListItemCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        PlayListItems.Remove(SelectedPlayListItem);
                        if(PlayListItems.Count == 1) {
                            ShowPlayListTab = false;
                            ShowCommentTab = true;
                        }
                    },
                    o => SelectedPlayListItem != null && SelectedPlayListItem != Information
                );
            }
        }

        public ICommand SwitchPlayNextVideoCommand
        {
            get
            {
                return CreateCommand(
                    o => CanPlayNextVideo.Value = !CanPlayNextVideo.Value,
                    o => PlayListItems.CanItemChange
                );
            }
        }

        #endregion
    }
}
