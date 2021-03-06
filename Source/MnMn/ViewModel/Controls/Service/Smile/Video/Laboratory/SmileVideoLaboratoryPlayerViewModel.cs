﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Wrapper;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory
{
    public sealed class SmileVideoLaboratoryPlayerViewModel : SmileVideoPlayerViewModel
    {
        public SmileVideoLaboratoryPlayerViewModel(Mediator mediator)
            : base(mediator)
        {
            Information = new SmileVideoLaboratoryInformationViewModel(Mediator);

            // プレイヤー設定から UI 設定できない項目を引っ張ってくる

            PlayerSetting.AutoPlayLowestSize = Setting.Player.AutoPlayLowestSize;
            PlayerSetting.IsAutoPlay = Setting.Player.IsAutoPlay;

            PlayerSetting.InactiveIsFullScreenRestore = Setting.Player.InactiveIsFullScreenRestore;
            PlayerSetting.InactiveIsFullScreenRestorePrimaryDisplayOnly = Setting.Player.InactiveIsFullScreenRestorePrimaryDisplayOnly;
            PlayerSetting.StopFullScreenRestore = Setting.Player.StopFullScreenRestore;
            PlayerSetting.StopFullScreenRestorePrimaryDisplayOnly = Setting.Player.StopFullScreenRestorePrimaryDisplayOnly;

            PlayerSetting.KeySpaceToPause = Setting.Player.KeySpaceToPause;
            PlayerSetting.MoseClickToPause = Setting.Player.MoseClickToPause;

            PlayerSetting.WheelOperation = Setting.Player.WheelOperation;
            PlayerSetting.VolumeOperationStep = Setting.Player.VolumeOperationStep;
            PlayerSetting.SeekOperationAbsoluteStep = Setting.Player.SeekOperationAbsoluteStep;
            PlayerSetting.SeekOperationIsPercent = Setting.Player.SeekOperationIsPercent;
            PlayerSetting.SeekOperationPercentStep = Setting.Player.SeekOperationPercentStep;

            PlayerSetting.BackgroundColor = Setting.Player.BackgroundColor;
            PlayerSetting.BackgroundKind = Setting.Player.BackgroundKind;

            GlobalCommentFiltering = new SmileVideoFilteringViweModel(new SmileVideoCommentFilteringSettingModel(), null, Mediator.Smile.VideoMediator.Filtering);
        }

        #region property

        FileInfo MsgFile { get; set; }

        #endregion

        #region function

        void IgnoreLaboratory([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumer = -1, [CallerMemberName] string callerMemberName = "")
        {
            Mediator.Logger.Debug("ignore", null, 2, callerFilePath, callerLineNumer, callerMemberName);
        }

        public Task LoadAsync(FileInfo videoFile, FileInfo msgFile)
        {
            videoFile.Refresh();
            if(!videoFile.Exists) {
                return Task.CompletedTask;
            }
            VideoFile = videoFile;

            msgFile.Refresh();
            if(msgFile.Exists) {
                MsgFile = msgFile;
            }

            return base.LoadAsync(Information, false, CacheSpan.NoCache, CacheSpan.NoCache);
        }

        #endregion

        #region SmileVideoPlayerViewModel

        protected override SmileVideoPlayerSettingModel PlayerSetting { get { return Setting.Laboratory.Player; } }

        public override IReadOnlyList<SmileVideoMyListFinderViewModelBase> AccountMyListItems { get; } = new List<SmileVideoMyListFinderViewModelBase>();
        public override IReadOnlyList<SmileVideoBookmarkNodeViewModel> BookmarkItems { get; } = new List<SmileVideoBookmarkNodeViewModel>();

        public override bool IsPremiumAccount { get; } = true;

        public override ICommand SwitchWorkingPlayerCommand => CreateCommand(o => { }, o => false);

        public override ImageSource PosterThumbnailImage => new BitmapImage(SharedConstants.GetPackUri("/Resources/MnMn-Header.png"));

        protected override FileInfo PlayFile
        {
            get { return VideoFile; }
        }

        protected override void AddHistory(SmileVideoInformationViewModel information)
        {
            IgnoreLaboratory();
        }

        protected override void SetCheckedCheckItLater(string videoId, Uri watchUrl)
        {
            IgnoreLaboratory();
        }

        protected override Task LoadRelationVideoAsync()
        {
            IgnoreLaboratory();
            return Task.CompletedTask;
        }

        protected override void CheckTagPedia()
        {
            IgnoreLaboratory();
            IsCheckedTagPedia = true;
        }
        protected override Task LoadDataWithSessionAsync()
        {
            LoadVideoFromCache(VideoFile);

            if(MsgFile != null && MsgFile.Exists) {
                if(string.Equals(MsgFile.Extension, ".xml", StringComparison.OrdinalIgnoreCase)) {
                    using(var stream = new FileStream(MsgFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        var comment = Msg.ConvertFromRawPacketData_Issue665NA(stream);
                        LoadComment_Issue665NA_Async(comment);
                    }
                } else {
                    var comment = Msg.ConvertMsgSettingModel(MsgFile);
                    LoadCommentAsync(comment);
                }
                OnLoadMsgEnd();
            }

            return Task.CompletedTask;
        }

        protected override Task PostCommentAsync(TimeSpan videoPosition)
        {
            var chat = new RawSmileVideoMsgChatResultModel() {
                No = nameof(RawSmileVideoMsgChatResultModel.No),
                Thread = nameof(RawSmileVideoMsgChatResultModel.Thread),
            };

            var commentViewModel = CreateSingleComment(chat, videoPosition);

            AppendComment(commentViewModel, true);

            return Task.CompletedTask;
        }

        protected override Task LoadPosterAsync()
        {
            CallOnPropertyChange(nameof(PosterThumbnailImage));
            return Task.CompletedTask;
        }

        public override ICommand OpenUserOrChannelIdCommand
        {
            get
            {
                return CreateCommand(
                    o => IgnoreLaboratory()
                );
            }
        }

        public override ICommand OpenCacheDirectoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(VideoFile == null) {
                            return;
                        }

                        VideoFile.Refresh();
                        if(VideoFile.Exists) {
                            ShellUtility.OpenDirectory(VideoFile.Directory, Mediator.Logger);
                        }
                    },
                    o => VideoFile != null
                );
            }
        }

        public override ICommand OpenIdleTalkMutterCommand
        {
            get
            {
                return CreateCommand(
                    o => Mediator.Logger.Debug("ついっついー"),
                    o => false
                );
            }
        }

        #endregion
    }
}
