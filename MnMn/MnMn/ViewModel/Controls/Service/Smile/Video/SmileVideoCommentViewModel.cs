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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
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
        #region variable

        bool _isSelected;
        bool _nowShowing;

        #endregion

        public SmileVideoCommentViewModel(RawSmileVideoMsgChatModel model, SmileVideoSettingModel setting)
            : base(model)
        {
            Setting = setting;

            var commands = Commands.ToArray();

            var foreColor = SmileVideoCommentUtility.GetForeColor(commands, UserKind == SmileVideoUserKind.Premium);
            Foreground = new SolidColorBrush(foreColor);
            FreezableUtility.SafeFreeze(Foreground);
            Shadow = MediaUtility.GetAutoColor(foreColor);

            switch(SmileVideoCommentUtility.GetFontSize(commands)) {
                case SmileVideoCommentSize.Medium:
                    FontSize = Setting.FontSize;
                    break;
                case SmileVideoCommentSize.Small:
                    FontSize = Setting.FontSize * 0.8;
                    break;
                case SmileVideoCommentSize.Big:
                    FontSize = Setting.FontSize * 1.2;
                    break;
            }

            Vertical = SmileVideoCommentUtility.GetVerticalAlign(commands);
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

        public IEnumerable<string> Commands
        {
            get
            {
                if(string.IsNullOrEmpty(Model.Mail)) {
                    return Enumerable.Empty<string>();
                }

                return Model.Mail.Split(null);
            }
        }

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

        public double FontSize { get; }

        public string FontFamily { get { return Setting.FontFamily; } }

        public Brush Foreground { get; }
        public Color Shadow { get; }

        public double Opacity { get { return Setting.FontAlpha; } }

        public SmileVideoCommentVertical Vertical { get; }

        public bool IsSelected
        {
            get { return this._isSelected; }
            set { SetVariableValue(ref this._isSelected, value); }
        }

        public bool NowShowing
        {
            get { return this._nowShowing; }
            set { SetVariableValue(ref this._nowShowing, value); }
        }

        #endregion

        #region function

        #endregion
    }
}
