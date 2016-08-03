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

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    public class SmileVideoCommentViewModel: SingleModelWrapperViewModelBase<RawSmileVideoMsgChatModel>
    {
        #region variable

        bool _isSelected;
        bool _nowShowing;

        bool _filteringView;

        bool _approval;

        #endregion

        public SmileVideoCommentViewModel(RawSmileVideoMsgChatModel model, SmileVideoSettingModel setting)
            : base(model)
        {
            Setting = setting;

            Content = Model.Content ?? string.Empty;
            Anonymity = RawValueUtility.ConvertBoolean(Model.Anonymity);
            IsOriginalPoster = RawValueUtility.ConvertBoolean(Model.Fork);
            Timestamp = RawValueUtility.ConvertUnixTime(Model.Date);
            UserId = Model.UserId;
            Number = RawValueUtility.ConvertInteger(Model.No);
            Commands = SmileVideoCommentUtility.ConvertCommands(Model.Mail);
            Score = SmileVideoCommentUtility.ConvertScore(Model.Score);

            var foreColor = SmileVideoCommentUtility.GetForeColor(Commands, UserKind == SmileVideoUserKind.Premium);
            Foreground = new SolidColorBrush(foreColor);
            FreezableUtility.SafeFreeze(Foreground);
            Shadow = MediaUtility.GetAutoColor(foreColor);
            var strokeColor = MediaUtility.GetAutoColor(foreColor);
            Stroke = new SolidColorBrush(Color.FromArgb(0x7e, strokeColor.R, strokeColor.G, strokeColor.B));
            FreezableUtility.SafeFreeze(Stroke);

            ActualForeground = Foreground;
            ActualShadow = Shadow;
            ActualStroke = Stroke;

            FontSize = GetFontSize(Setting.Comment.FontSize, Commands);

            Vertical = SmileVideoCommentUtility.GetVerticalAlign(Commands);
        }

        #region property

        SmileVideoSettingModel Setting { get; }

        public bool Approval
        {
            get { return this._approval; }
            set { SetVariableValue(ref this._approval, value); }
        }

        /// <summary>
        /// 選択されているか。
        /// </summary>
        public bool IsSelected
        {
            get { return this._isSelected; }
            set { SetVariableValue(ref this._isSelected, value); }
        }

        /// <summary>
        /// 表示中か。
        /// </summary>
        public bool NowShowing
        {
            get { return this._nowShowing; }
            set { SetVariableValue(ref this._nowShowing, value); }
        }

        /// <summary>
        /// フィルタリングされているか。
        /// </summary>
        public bool FilteringView
        {
            get { return this._filteringView; }
            set { SetVariableValue(ref this._filteringView, value); }
        }

        public int Score { get; }

        public int Number { get; }

        /// <summary>
        /// 発言日時。
        /// </summary>
        public DateTime Timestamp { get; }

        public string UserId { get; }

        public SmileVideoUserKind UserKind
        {
            get { return SmileVideoMsgUtility.ConvertUserKind(Model.Premium); }
        }

        /// <summary>
        /// 経過時間。
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get { return SmileVideoMsgUtility.ConvertElapsedTime(Model.VPos); }
        }

        public IList<string> Commands { get; }

        /// <summary>
        /// コメント内容。
        /// </summary>
        public string Content { get; }
        /// <summary>
        /// 実際に表示するコメント内容。
        /// </summary>
        public string ActualContent
        {
            get
            {
                var s = Content;
                if(Setting.Comment.ConvertPairYenSlash) {
                    s = ReplaceYenToBackslash(s);
                }
                return s;
            }
        }

        /// <summary>
        /// 匿名。
        /// </summary>
        public bool Anonymity { get; }

        /// <summary>
        /// 投稿者コメントか。
        /// </summary>
        public bool IsOriginalPoster { get; }

        /// <summary>
        /// フォントサイズ。
        /// </summary>
        public double FontSize { get; private set; }
        /// <summary>
        /// フォント名。
        /// </summary>
        public string FontFamily { get { return Setting.Comment.FontFamily; } }
        /// <summary>
        /// 太字か。
        /// </summary>
        public bool FontBold { get { return Setting.Comment.FontBold; } }
        /// <summary>
        /// 斜体か。
        /// </summary>
        public bool FontItalic { get { return Setting.Comment.FontItalic; } }

        /// <summary>
        /// 前景ブラシ。
        /// </summary>
        public Brush Foreground { get; }
        /// <summary>
        /// 実際に使用する前景ブラシ。
        /// </summary>
        public Brush ActualForeground { get; private set; }
        /// <summary>
        /// 背景色。
        /// </summary>
        public Color Shadow { get; }
        /// <summary>
        /// 縁取り。
        /// </summary>
        public Brush Stroke { get; }
        /// <summary>
        /// 実際に使用する背景色。
        /// </summary>
        public Color ActualShadow { get; private set; }
        /// <summary>
        /// 実際に使用する縁取り。
        /// </summary>
        public Brush ActualStroke { get; private set; }

        public double Opacity { get { return Setting.Comment.FontAlpha; } }

        public SmileVideoCommentVertical Vertical { get; }

        #endregion

        #region function

        static double GetFontSize(double baseSize, IList<string> commands)
        {
            switch(SmileVideoCommentUtility.GetFontSize(commands)) {
                case SmileVideoCommentSize.Medium:
                    return baseSize;

                case SmileVideoCommentSize.Small:
                    return baseSize * 0.8;

                case SmileVideoCommentSize.Big:
                    return baseSize * 1.2;

                default:
                    throw new NotImplementedException();
            }
        }

        string ReplaceYenToBackslash(string s)
        {
            if(s.Any(c => c == '\\') && s.Any(c => c == '/')) {
                return s.Replace('\\', '\u29F5');
            }

            return s;
        }


        public void ChangeDefaultCommand(Brush foreground, Color backcolor)
        {
            // not imple
        }

        public void ChangeActualContent()
        {
            var propertyNames = new[] {
                nameof(ActualContent),
            };
            CallOnPropertyChange(propertyNames);
        }

        public void ChangeFontStyle()
        {
            FontSize = GetFontSize(Setting.Comment.FontSize, Commands);

            var propertyNames = new[] {
                nameof(FontSize),
                nameof(FontFamily),
                nameof(FontBold),
                nameof(FontItalic),
                nameof(Opacity),
            };
            CallOnPropertyChange(propertyNames);
        }

        #endregion
    }
}
