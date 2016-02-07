﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoCommentViewModel: SingleModelWrapperViewModelBase<RawSmileVideoMsgChatModel>
    {
        public SmileVideoCommentViewModel(RawSmileVideoMsgChatModel model, SmileVideoSettingModel setting)
            : base(model)
        {
            Setting = setting;
        }

        #region property

        SmileVideoSettingModel Setting { get; }

        public int Score
        {
            get { return RawValueUtility.ConvertInteger(Model.Score); }
        }

        public int Number
        {
            get { return RawValueUtility.ConvertInteger(Model.No); }
        }

        /// <summary>
        /// 発言日時。
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                return SmileVideoMsgUtility.ConvertUnixTime(Model.Date);
            }
        }

        public string UserId { get { return Model.UserId; } }

        public SmileVideoUserKind UserKind
        {
            get
            {
                return SmileVideoMsgUtility.ConvertUserKind(Model.Premium);
            }
        }

        /// <summary>
        /// 経過時間。
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get
            {
                return SmileVideoMsgUtility.ConvertElapsedTime(Model.VPos);
            }
        }

        public string Command { get { return Model.Mail ?? string.Empty; ; } }

        public string Content { get { return Model.Content ?? string.Empty; } }

        public bool Anonymity
        {
            get { return RawValueUtility.ConvertBoolean(Model.Anonymity); }
        }

        /// <summary>
        /// 投稿者コメント。
        /// </summary>
        public bool IsContributor
        {
            get { return RawValueUtility.ConvertBoolean(Model.Fork); }
        }

        public double FontSize { get { return Setting.FontSize; } }

        public string FontFamily { get { return Setting.FontFamily; } }

        public Brush Foreground
        {
            get
            {
                return new SolidColorBrush(GetForeColor());
            }
        }

        public Brush Shadow
        {
            get
            {
                return new SolidColorBrush(GetShadowColor(GetForeColor()));
            }
        }


        #endregion

        #region function

        public Color GetForeColor()
        {
            return Colors.White;
        }

        public Color GetShadowColor(Color foreColor)
        {
            return MediaUtility.GetAutoColor(foreColor);
        }

        #endregion
    }
}
