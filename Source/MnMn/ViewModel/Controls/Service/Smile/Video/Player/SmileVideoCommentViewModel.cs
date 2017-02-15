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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    /// <summary>
    /// コメント表示要素のViewModel。
    /// </summary>
    public class SmileVideoCommentViewModel: SingleModelWrapperViewModelBase<RawSmileVideoMsgChatModel>
    {
        #region define

        static Regex regSpace = new Regex(@"(?<SPACE>\s){2,}", RegexOptions.Compiled);
        static readonly string viewLineBreak = Constants.ServiceSmileVideoCommentSimpleNewline;
        static readonly string viewSpace = Constants.ServiceSmileVideoCommentSimpleSpace;

        #endregion

        #region variable

        bool _isSelected;
        bool _nowShowing;

        bool _filteringView;

        bool _approval;

        #endregion

        public SmileVideoCommentViewModel(RawSmileVideoMsgChatModel model, SmileVideoCommentStyleSettingModel setting)
            : base(model)
        {
            Setting = setting;

            Content = Model.Content ?? string.Empty;
            Anonymity = RawValueUtility.ConvertBoolean(Model.Anonymity);
            IsOriginalPoster = RawValueUtility.ConvertBoolean(Model.Fork);
            Timestamp = RawValueUtility.ConvertUnixTime(Model.Date);
            UserId = Model.UserId;
            Number = RawValueUtility.ConvertInteger(Model.No);
            Commands = SmileVideoMsgUtility.ConvertCommands(Model.Mail);
            Score = SmileVideoMsgUtility.ConvertScore(Model.Score);

            CommentSize = SmileVideoMsgUtility.GetFontSize(Commands);
            ActualCommentSize = CommentSize;
            CommentVertical = SmileVideoMsgUtility.GetVerticalAlign(Commands);
            ActualCommentVertical = CommentVertical;

            ForegroundColor = SmileVideoMsgUtility.GetForeColor(Commands, UserKind == SmileVideoUserKind.Premium);
            OutlineColor = GetOoutlineColor(ForegroundColor);
            ShadowColor = GetShadowColor(OutlineColor);
            StrokeColor = GetStrokeColor(OutlineColor);

            ApplyFontStyle(ForegroundColor, ShadowColor, StrokeColor, ActualCommentSize);

            if(IsOriginalPoster) {
                var hasComment = !string.IsNullOrEmpty(Content) && 0 < Content.Length;
                IsNiwanLanguage = hasComment
                    ? Content[0] == '/'
                    : false
                ;

                if(!IsNiwanLanguage && hasComment) {
                    if(Content[0] == '＠') {
                        CommentScript = SmileVideoCommentUtility.GetCommentScript(Content, Commands);
                    }
                }
            }
        }

        #region property

        SmileVideoCommentStyleSettingModel Setting { get; }

        /// <summary>
        /// 自身の投稿コメントか。
        /// <para>投稿時のみ有効。</para>
        /// </summary>
        public bool IsMyPost { get; set; }

        /// <summary>
        /// 表示許可。
        /// </summary>
        public bool Approval
        {
            get { return this._approval; }
            set
            {
                if(SetVariableValue(ref this._approval, value)) {
                    CallOnPropertyChange(nameof(NoApproval));
                }
            }
        }

        public bool NoApproval => !Approval;

        /// <summary>
        /// 表示不許可の概要。
        /// </summary>
        public FewViewModel<string> NoApprovalRemark { get; } = new FewViewModel<string>();
        /// <summary>
        /// 表示不許可の詳細。
        /// </summary>
        public FewViewModel<string> NoApprovalDetail { get; } = new FewViewModel<string>();

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

        public string CommandsText { get { return string.Join(" ", Commands); } }

        /// <summary>
        /// コメント内容。
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// コメントを一行表示。
        /// <para>改行と連続スペースはいい感じに置き換える。</para>
        /// </summary>
        public string ContentSingleLine
        {
            get
            {
                if(Content == null) {
                    return string.Empty;
                }

                var lines = Content
                    .SplitLines()
                    .Select(s => regSpace.Replace(s, viewSpace))
                ;

                return string.Join(viewLineBreak, lines);
            }
        }

        /// <summary>
        /// 実際に表示するコメント内容。
        /// </summary>
        public string ActualContent
        {
            get
            {
                var s = Content;
                if(Setting.ConvertPairYenSlash) {
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
        /// ニワン語か。
        /// </summary>
        public bool IsNiwanLanguage { get; }

        public bool HasCommentScript => CommentScript != null && CommentScript.ScriptType != SmileVideoCommentScriptType.Unknown;

        public SmileVideoCommentScriptModel CommentScript { get; }

        /// <summary>
        /// コメントサイズ。
        /// </summary>
        public SmileVideoCommentSize CommentSize { get; }
        /// <summary>
        /// 実際に使用するコメントサイズ。
        /// </summary>
        public SmileVideoCommentSize ActualCommentSize { get; private set; }

        /// <summary>
        /// フォントサイズ。
        /// </summary>
        public double FontSize { get; private set; }
        /// <summary>
        /// フォント名。
        /// </summary>
        public string FontFamily { get { return Setting.FontFamily; } }
        /// <summary>
        /// 太字か。
        /// </summary>
        public bool FontBold { get { return Setting.FontBold; } }
        /// <summary>
        /// 斜体か。
        /// </summary>
        public bool FontItalic { get { return Setting.FontItalic; } }

        public TextShowMode TextShowMode
        {
            get { return Setting.TextShowMode; }
        }

        /// <summary>
        /// 前景ブラシ。
        /// </summary>
        public Color ForegroundColor { get; }
        /// <summary>
        /// 実際に使用する前景ブラシ。
        /// </summary>
        public Brush ActualForeground { get; private set; }
        /// <summary>
        /// 影色・縁色の基本色。
        /// </summary>
        public Color OutlineColor { get; }
        /// <summary>
        /// 影色。
        /// </summary>
        public Color ShadowColor { get; }
        /// <summary>
        /// 縁色。
        /// </summary>
        public Color StrokeColor { get; }
        /// <summary>
        /// 実際に使用する影色。
        /// </summary>
        public Brush ActualShadow { get; private set; }
        /// <summary>
        /// 実際に使用する縁色。
        /// </summary>
        public Brush ActualStroke { get; private set; }

        public double Opacity { get { return Setting.FontAlpha; } }

        /// <summary>
        /// 縦位置。
        /// </summary>
        public SmileVideoCommentVertical CommentVertical { get; }
        /// <summary>
        /// 実際に使用する縦位置。
        /// </summary>
        public SmileVideoCommentVertical ActualCommentVertical { get; private set; }

        public int Fps { get { return Setting.Fps; } }

        #endregion

        #region function

        static Color GetOoutlineColor(Color foregroundColor)
        {
            return MediaUtility.GetAutoColor(foregroundColor);
        }
        static Color GetShadowColor(Color outlineColor)
        {
            return outlineColor;
        }
        static Color GetStrokeColor(Color outlineColor)
        {
            return Color.FromArgb(0x80, outlineColor.R, outlineColor.G, outlineColor.B);
        }

        static double GetFontSize(double baseSize, SmileVideoCommentSize commentSize)
        {
            switch(commentSize) {
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

        void ApplyFontStyle(Color foreColor, Color shadowColor, Color strokeColor, SmileVideoCommentSize commentSize)
        {
            ActualForeground = new SolidColorBrush(foreColor);
            ActualShadow = new SolidColorBrush(shadowColor);
            ActualStroke = new SolidColorBrush(strokeColor);

            FontSize = GetFontSize(Setting.FontSize, commentSize);

            FreezableUtility.SafeFreeze(ActualForeground);
            FreezableUtility.SafeFreeze(ActualShadow);
            FreezableUtility.SafeFreeze(ActualStroke);
        }

        public void ApplyDefaultCommentScript(SmileVideoCommentScriptModel commentScript)
        {
            Debug.Assert(commentScript.ScriptType == SmileVideoCommentScriptType.Default);

            Color foregroundColor, shadowColor, strokeColor;
            if(ForegroundColor == Colors.White) {
                foregroundColor = commentScript.ForegroundColor;
                var outlineColor = GetOoutlineColor(foregroundColor);
                shadowColor = GetShadowColor(outlineColor);
                strokeColor = GetStrokeColor(outlineColor);
            } else {
                foregroundColor = ForegroundColor;
                shadowColor = ShadowColor;
                strokeColor = StrokeColor;
            }

            SmileVideoCommentSize commentSize;
            if(CommentSize == SmileVideoCommentSize.Medium) {
                commentSize = commentScript.CommentSize;
            } else {
                commentSize = CommentSize;
            }

            SmileVideoCommentVertical commentVertical;
            if(CommentVertical == SmileVideoCommentVertical.Normal) {
                commentVertical = commentScript.CommentVertical;
            }else {
                commentVertical = CommentVertical;
            }
            ActualCommentVertical = commentVertical;

            ApplyFontStyle(foregroundColor, shadowColor, strokeColor, commentSize);

            var propertyNames = new[] {
                nameof(ActualForeground),
                nameof(ActualShadow),
                nameof(ActualStroke),
                nameof(FontSize),
            };
            CallOnPropertyChange(propertyNames);
        }

        public void ChangeActualContent()
        {
            var propertyNames = new[] {
                nameof(ActualContent),
            };
            CallOnPropertyChange(propertyNames);
        }

        public void ChangeActualStyle()
        {
            var propertyNames = new[] {
                nameof(ActualForeground),
                nameof(ActualShadow),
                nameof(ActualStroke),
            };
            CallOnPropertyChange(propertyNames);
        }

        public void ChangeFontStyle()
        {
            FontSize = GetFontSize(Setting.FontSize, ActualCommentSize);

            var propertyNames = new[] {
                nameof(FontSize),
                nameof(FontFamily),
                nameof(FontBold),
                nameof(FontItalic),
                nameof(Opacity),
            };
            CallOnPropertyChange(propertyNames);
        }

        internal void ChangeTextShow()
        {
            var propertyNames = new[] {
                nameof(TextShowMode),
                nameof(ActualShadow),
                nameof(ActualStroke),
            };
            CallOnPropertyChange(propertyNames);
        }

        #endregion
    }
}
