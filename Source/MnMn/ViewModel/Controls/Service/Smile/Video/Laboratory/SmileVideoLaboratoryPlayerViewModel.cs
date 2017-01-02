using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Wrapper;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Laboratory
{
    public sealed class SmileVideoLaboratoryPlayerViewModel: SmileVideoPlayerViewModel
    {
        public SmileVideoLaboratoryPlayerViewModel(Mediation mediation) 
            : base(mediation)
        {
            Information = new SmileVideoLaboratoryInformationViewModel(Mediation);

            PlayerSetting.AutoPlayLowestSize = Setting.Player.AutoPlayLowestSize;
            PlayerSetting.IsAutoPlay = Setting.Player.IsAutoPlay;

            GlobalCommentFilering = new SmileVideoFilteringViweModel(new SmileVideoCommentFilteringSettingModel(), null, Mediation.Smile.VideoMediation.Filtering);
        }

        #region property

        FileInfo MsgFile { get; set; }

        #endregion

        #region function

        void IgnoreLaboratory([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumer = -1, [CallerMemberName] string callerMemberName = "")
        {
            Mediation.Logger.Debug("ignore", null, 2, callerFilePath, callerLineNumer, callerMemberName);
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
                using(var stream = new FileStream(MsgFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    var comment = Msg.ConvertFromRawPacketData(stream);
                    LoadCommentAsync(comment);
                    OnLoadMsgEnd();
                }
            }

            return Task.CompletedTask;
        }

        protected override Task PostCommentAsync(TimeSpan videoPosition)
        {
            var commentModel = new RawSmileVideoMsgChatModel() {
                //Anonymity = SmileVideoCommentUtility.GetIsAnonymous(PostCommandItems)
                Mail = string.Join(" ", PostCommandItems),
                Content = PostCommentBody,
                Date = RawValueUtility.ConvertRawUnixTime(DateTime.Now).ToString(),
                No = nameof(RawSmileVideoMsgChatModel.No),
                VPos = SmileVideoMsgUtility.ConvertRawElapsedTime(videoPosition),
                Thread = nameof(RawSmileVideoMsgChatModel.Thread),
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

        #endregion
    }
}
