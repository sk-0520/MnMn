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
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;
using OxyPlot;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video
{
    /// <summary>
    /// プログラム側で使用するコメント共通処理。
    /// </summary>
    public static class SmileVideoCommentUtility
    {
        #region define

        static readonly TimeSpan correctionTime = Constants.ServiceSmileVideoCommentCorrectionTime;

        #endregion

        #region function

        /// <summary>
        /// コメントフィルタのViewModel生成用ロジック。
        /// <para>別に何もしないけどなんでこいつ独立してんだろう。</para>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SmileVideoCommentFilteringItemEditViewModel CreateVideoCommentFilter(SmileVideoCommentFilteringItemSettingModel model, object data)
        {
            return new SmileVideoCommentFilteringItemEditViewModel(model);
        }

        /// <summary>
        /// コメント表示要素の生成。
        /// </summary>
        /// <param name="commentViewModel"></param>
        /// <param name="commentArea"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static SmileVideoCommentElement CreateCommentElement(SmileVideoCommentViewModel commentViewModel, Size commentArea, SmileVideoCommentStyleSettingModel setting)
        {
            var element = new SmileVideoCommentElement();
            using(Initializer.BeginInitialize(element)) {
                element.DataContext = commentViewModel;
            }

            if(commentViewModel.Vertical != SmileVideoCommentVertical.Normal) {
                element.Width = commentArea.Width;
            }

            return element;
        }

        static int GetSafeYPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, OrderBy orderBy, bool calculationWidth, SmileVideoCommentStyleSettingModel setting)
        {
            var isAsc = orderBy == OrderBy.Ascending;
            // 空いている部分に放り込む, 今から表示するコメントの同一条件全行を取得する
            var lineList = showingCommentList
                .Where(i => i.ViewModel.Vertical == commentViewModel.Vertical)
                .GroupBy(i => (int)Canvas.GetTop(i.Element))
                .IfOrderByAsc(g => g.Key, isAsc)
                .ToArray()
            ;

            var commentAreaHeight = (int)commentArea.Height;
            var myHeight = (int)element.ActualHeight;
            var start = isAsc ? 0 : commentAreaHeight - myHeight;
            var last = isAsc ? commentAreaHeight - myHeight : 0;

            if(lineList.Length == 0) {
                // コメントない
                return start;
            }

            for(var y = start; isAsc ? y < last : last < y;) {

                var dupLine = lineList.FirstOrDefault(ls => ls.Key <= y && y + myHeight <= ls.Key + ls.Max(l => l.Element.ActualHeight));

                if(dupLine == null && !calculationWidth) {
                    // 誰もいないっぽいけど入る余地はあるのか
                    dupLine = lineList.FirstOrDefault(ls => y < ls.Key && ls.Key < y + myHeight);
                }

                if(dupLine == null) {
                    // 誰もいなければ入れる
                    return y;
                } else {
                    // 横方向の重なりを考慮
                    if(calculationWidth) {
                        // 誰かいる場合はその末尾に入れられるか
                        var lastComment = dupLine.FirstOrDefault(l => commentArea.Width < Canvas.GetLeft(l.Element) + l.Element.ActualWidth);
                        if(lastComment == null) {
                            return y;
                        }
                    }

                    // 現在コメント行の最大の高さを加算して次行を検索
                    var plusValue = (int)dupLine.Max(l => l.Element.ActualHeight);
                    if(isAsc) {
                        y += plusValue;
                    } else {
                        y -= plusValue;
                    }
                }
            }

            // 全走査して安全そうなところがなければ一番少なそうなところに設定する
            var compromiseY = lineList
                .OrderBy(line => line.Count())
                .FirstOrDefault()
                ?.Key ?? 0
            ;
            return compromiseY;
        }

        static void SetMarqueeCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, SmileVideoCommentStyleSettingModel setting)
        {
            var y = GetSafeYPosition(commentViewModel, element, commentArea, showingCommentList, OrderBy.Ascending, true, setting);
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, commentArea.Width);
        }

        static void SetStaticCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, OrderBy orderBy, SmileVideoCommentStyleSettingModel setting)
        {
            var y = GetSafeYPosition(commentViewModel, element, commentArea, showingCommentList, orderBy, false, setting);
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, 0);
            element.Width = commentArea.Width;
        }

        static void SetCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, SmileVideoCommentStyleSettingModel setting)
        {
            switch(commentViewModel.Vertical) {
                case SmileVideoCommentVertical.Normal:
                    SetMarqueeCommentPosition(commentViewModel, element, commentArea, showingCommentList, setting);
                    break;

                case SmileVideoCommentVertical.Top:
                    SetStaticCommentPosition(commentViewModel, element, commentArea, showingCommentList, OrderBy.Ascending, setting);
                    break;

                case SmileVideoCommentVertical.Bottom:
                    SetStaticCommentPosition(commentViewModel, element, commentArea, showingCommentList, OrderBy.Descending, setting);
                    break;

                default:
                    break;
            }
        }

        static AnimationTimeline CreateMarqueeCommentAnimeation(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, TimeSpan prevTime, TimeSpan showTime)
        {
            var animation = new DoubleAnimation();
            var starTime = commentViewModel.ElapsedTime.TotalMilliseconds - prevTime.TotalMilliseconds;
            var diffPosition = starTime / commentArea.Width;
            if(double.IsInfinity(diffPosition)) {
                diffPosition = 0;
            }

            animation.From = commentArea.Width + diffPosition;
            animation.To = -element.ActualWidth;
            animation.Duration = new Duration(showTime);

            Timeline.SetDesiredFrameRate(animation, commentViewModel.Fps);

            return animation;
        }

        static AnimationTimeline CreateTopBottomCommentAnimeation(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, TimeSpan prevTime, TimeSpan showTime)
        {
            var animation = new DoubleAnimation();

            // アニメーションさせる必要ないけど停止や移動なんかを考えるとIFとしてアニメーションの方が楽
            animation.From = animation.To = 0;
            animation.Duration = new Duration(showTime);

            Timeline.SetDesiredFrameRate(animation, Constants.FrameworkFps.Head);

            return animation;
        }

        static AnimationTimeline CreateCommentAnimeation(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, TimeSpan prevTime, TimeSpan showTime)
        {
            switch(commentViewModel.Vertical) {
                case SmileVideoCommentVertical.Normal:
                    return CreateMarqueeCommentAnimeation(commentViewModel, element, commentArea, prevTime, showTime);

                case SmileVideoCommentVertical.Top:
                case SmileVideoCommentVertical.Bottom:
                    return CreateTopBottomCommentAnimeation(commentViewModel, element, commentArea, prevTime, showTime);

                default:
                    throw new NotImplementedException();
            }
        }

        public static void ShowSingleComment(SmileVideoCommentViewModel commentViewModel, Canvas commentParentElement, Size commentArea, TimeSpan prevTime, IList<SmileVideoCommentDataModel> showingCommentList, SmileVideoCommentStyleSettingModel setting)
        {
            var element = CreateCommentElement(commentViewModel, commentArea, setting);

            commentViewModel.NowShowing = true;

            commentParentElement.Children.Add(element);
            commentParentElement.UpdateLayout();

            SetCommentPosition(commentViewModel, element, commentArea, showingCommentList, setting);

            // アニメーション設定
            var animation = CreateCommentAnimeation(commentViewModel, element, commentArea, prevTime - correctionTime, setting.ShowTime + correctionTime);

            var data = new SmileVideoCommentDataModel(element, commentViewModel, animation);
            showingCommentList.Add(data);

            EventDisposer<EventHandler> ev = null;
            data.Clock.Completed += EventUtility.Create<EventHandler>((object sender, EventArgs e) => {
                if(element != null) {
                    commentParentElement.Children.Remove(element);
                    element.DataContext = null;
                }
                element = null;

                if(ev != null) {
                    ev.Dispose();
                }
                ev = null;

                showingCommentList.Remove(data);
                data.ViewModel.NowShowing = false;

                data = null;

            }, h => commentParentElement.Dispatcher.BeginInvoke(new Action(() => animation.Completed -= h)), out ev);

            element.ApplyAnimationClock(Canvas.LeftProperty, data.Clock);
        }

        public static bool InShowTime(SmileVideoCommentViewModel comment, TimeSpan prevTime, TimeSpan nowTime)
        {
            var correction = nowTime < correctionTime ? TimeSpan.Zero : correctionTime;
            return prevTime <= (comment.ElapsedTime - correction) && (comment.ElapsedTime - correction) <= nowTime;
        }

        public static IList<SmileVideoCommentViewModel> ShowComments(Canvas commentParentElement, Size commentArea, TimeSpan prevTime, TimeSpan nowTime, IList<SmileVideoCommentViewModel> commentViewModelList, bool isOriginalPoster, IList<SmileVideoCommentDataModel> showingCommentList, bool isEnabledDisplayCommentLimit, int displayCommentLimitCount, SmileVideoCommentStyleSettingModel setting)
        {
            // 現在時間から-1秒したものを表示対象とする
            var newComments = commentViewModelList
                .Where(c => c.Approval)
                .Where(c => !c.NowShowing)
                .Where(c => !c.IsNiwanLanguage) // #405
                .Where(c => InShowTime(c, prevTime, nowTime))
                .ToList()
            ;
            if(newComments.Any()) {
                foreach(var commentViewModel in newComments.Where(c => !c.HasCommentScript)) {
                    ShowSingleComment(commentViewModel, commentParentElement, commentArea, prevTime, showingCommentList, setting);
                }
                // 超過分のコメントを破棄
                if(isEnabledDisplayCommentLimit && 0 < displayCommentLimitCount) {
                    var removeList = showingCommentList
                        .OrderBy(i => i.ViewModel.ElapsedTime)
                        .ThenBy(i => i.ViewModel.Number)
                        .Take(showingCommentList.Count - displayCommentLimitCount)
                        .ToArray()
                    ;
                    foreach(var item in removeList) {
                        item.Clock.Controller.SkipToFill();
                    }
                }
            }

            return newComments;
        }

        /// <summary>
        /// グラフ用データの生成。
        /// </summary>
        /// <param name="commentList"></param>
        /// <param name="totalTime"></param>
        /// <returns></returns>
        public static IEnumerable<DataPoint> CreateCommentChartItems(IReadOnlyList<SmileVideoCommentViewModel> commentList, TimeSpan totalTime)
        {
            var srcItems = commentList
                .GroupBy(c => (int)c.ElapsedTime.TotalSeconds)
                .Select(cg => new DataPoint((int)cg.First().ElapsedTime.TotalSeconds, cg.Count()))
            ;

            var emptySecondItems = Enumerable.Range(0, (int)totalTime.TotalSeconds)
                .Select(s => new DataPoint(s, 0))
            ;

            var mixItems = srcItems
                .Concat(emptySecondItems)
                .GroupBy(cd => cd.X)
                .OrderBy(cg => cg.Key)
                .Select(cg => cg.OrderByDescending(c => c.Y).First())
            ;

            return mixItems;
        }

        /// <summary>
        /// 生メッセージからコメントViewModelを生成する。
        /// </summary>
        /// <param name="rawMsgPacket"></param>
        /// <param name="setting"></param>
        /// <returns>生成されたコメント群。いい感じに並び替えられてる。</returns>
        public static IEnumerable<SmileVideoCommentViewModel> CreateCommentViewModels(RawSmileVideoMsgPacketModel rawMsgPacket, SmileVideoCommentStyleSettingModel styleSetting)
        {
            var comments = rawMsgPacket.Chat
                .Where(c => !string.IsNullOrEmpty(c.Content))
                .GroupBy(c => new { c.No, c.Fork })
                .Select(c => new SmileVideoCommentViewModel(c.First(), styleSetting))
                .OrderBy(c => c.ElapsedTime)
            ;

            return comments;
        }

        /// <summary>
        ///ユーザーのフィルタリング用データ生成。
        ///<para>ここでのフィルタリングはNGの意味ではなくコメントリストで使用するフィルタリング。</para>
        ///<para>投稿者と視聴者は区別しない。区別したければ上位で分けてやる必要あり。</para>
        /// </summary>
        /// <param name="commentList"></param>
        /// <returns></returns>
        public static IEnumerable<SmileVideoFilteringUserViewModel> CreateFilteringUserItems(IReadOnlyList<SmileVideoCommentViewModel> commentList)
        {
            var result = commentList
                .Where(c => !string.IsNullOrWhiteSpace(c.UserId))
                .GroupBy(c => c.UserId)
                .Select(g => new { Count = g.Count(), Comment = g.First() })
                .Select(cc => new SmileVideoFilteringUserViewModel(cc.Comment.UserId, cc.Comment.UserKind, cc.Count))
                .OrderByDescending(fu => fu.Count)
                .ThenBy(fu => fu.UserId)
            ;

            return result;
        }


        static T ConvertFromDefinedToEnum<T>(IReadOnlyDictionary<string, string> map, string key, T failResult)
        {
            string value;

            if(map.TryGetValue(key, out value)) {
                return EnumUtility.Parse<T>(map[key], false);
            }

            return failResult;
        }

        public static SmileVideoCommentFilteringItemSettingModel ConvertFromDefined(DefinedElementModel model)
        {
            var result = new SmileVideoCommentFilteringItemSettingModel() {
                Target = ConvertFromDefinedToEnum(model.Extends, "target", SmileVideoCommentFilteringTarget.Comment),
                IgnoreCase = RawValueUtility.ConvertBoolean(model.Extends["ignore-case"]),
                Type = ConvertFromDefinedToEnum(model.Extends, "type", FilteringType.PartialMatch),
                Source = model.Extends["pattern"],
                Name = model.DisplayText,
            };

            return result;
        }

        static TimeSpan GetEnabledTimeInCommentScript(string command)
        {
            if(string.IsNullOrEmpty(command)) {
                return TimeSpan.MaxValue;
            }

            if(command[0] != '@') {
                return TimeSpan.Zero;
            }

            var rawSeconds = command.Substring(1);
            var seconds = RawValueUtility.ConvertInteger(rawSeconds);
            if(seconds == RawValueUtility.UnknownInteger) {
                return TimeSpan.Zero;
            }

            return TimeSpan.FromSeconds(seconds);
        }

        static SmileVideoCommentScriptModel GetCommentScriptDefault(string[] comments, IList<string> commands)
        {
            var result = new SmileVideoCommentScriptModel() {
                ScriptType = SmileVideoCommentScriptType.Default,
            };

            var enabledTime = TimeSpan.Zero;
            foreach(var command in commands) {
                var time = GetEnabledTimeInCommentScript(command);
                if(time == TimeSpan.Zero) {
                    continue;
                }
                enabledTime = time;
            }
            if(enabledTime == TimeSpan.Zero) {
                return null;
            }

            result.IsEnabledTime = enabledTime;

            result.ForeColor = SmileVideoMsgUtility.GetForeColor(commands, true);
            result.CommentSize = SmileVideoMsgUtility.GetFontSize(commands);
            result.VerticalAlign = SmileVideoMsgUtility.GetVerticalAlign(commands);

            return result;
        }


        public static SmileVideoCommentScriptModel GetCommentScript(string comment, IList<string> commands)
        {
            var comments = comment.Split();

            var head = comments.First();
            // 外部化してぇなぁ
            var map = new Dictionary<string, SmileVideoCommentScriptType>() {
                ["＠デフォルト"] = SmileVideoCommentScriptType.Default,
            };

            SmileVideoCommentScriptType scriptType;
            if(!map.TryGetValue(head, out scriptType)) {
                return null;
            }
            switch(scriptType) {
                case SmileVideoCommentScriptType.Default:
                    if(commands.Any()) {
                        return GetCommentScriptDefault(comments, commands);
                    } else {
                        return null;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
