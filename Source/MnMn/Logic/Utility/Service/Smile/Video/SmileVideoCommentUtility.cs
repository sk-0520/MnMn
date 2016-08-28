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
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

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
        public static SmileVideoCommentElement CreateCommentElement(SmileVideoCommentViewModel commentViewModel, Size commentArea, SmileVideoSettingModel setting)
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

        static int GetSafeYPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, OrderBy orderBy, bool calculationWidth, SmileVideoSettingModel setting)
        {
            var isAsc = orderBy == OrderBy.Ascending;
            // 空いている部分に放り込む
            var lineList = showingCommentList
                .Where(i => i.ViewModel.Vertical == commentViewModel.Vertical)
                .GroupBy(i => (int)Canvas.GetTop(i.Element))
                .IfOrderByAsc(g => g.Key, isAsc)
                .ToArray()
            ;

            var myHeight = (int)element.ActualHeight;
            var start = isAsc ? 0 : (int)commentArea.Height - myHeight;
            var last = isAsc ? (int)commentArea.Height - myHeight : 0;

            if(lineList.Length == 0) {
                // コメントない
                return start;
            }

            for(var y = start; isAsc ? y < last : last < y;) {
                var dupLine = lineList.FirstOrDefault(ls => ls.Key <= y && y + myHeight <= ls.Key + ls.Max(l => l.Element.ActualHeight));
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

        static void SetMarqueeCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, SmileVideoSettingModel setting)
        {
            var y = GetSafeYPosition(commentViewModel, element, commentArea, showingCommentList, OrderBy.Ascending, true, setting);
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, commentArea.Width);
        }

        static void SetStaticCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, OrderBy orderBy, SmileVideoSettingModel setting)
        {
            var y = GetSafeYPosition(commentViewModel, element, commentArea, showingCommentList, orderBy, false, setting);
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, 0);
            element.Width = commentArea.Width;
        }

        static void SetCommentPosition(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, IList<SmileVideoCommentDataModel> showingCommentList, SmileVideoSettingModel setting)
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

            return animation;
        }

        static AnimationTimeline CreateTopBottomCommentAnimeation(SmileVideoCommentViewModel commentViewModel, FrameworkElement element, Size commentArea, TimeSpan prevTime, TimeSpan showTime)
        {
            var animation = new DoubleAnimation();
            //var starTime = commentViewModel.ElapsedTime.TotalMilliseconds - prevTime.TotalMilliseconds;
            //var diffPosition = starTime / commentArea.Width;

            // アニメーションさせる必要ないけど停止や移動なんかを考えるとIFとしてアニメーションの方が楽
            animation.From = animation.To = 0;
            animation.Duration = new Duration(showTime);

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

        public static void FireShowSingleComment(SmileVideoCommentViewModel commentViewModel, Canvas commentParentElement, Size commentArea, TimeSpan prevTime, IList<SmileVideoCommentDataModel> showingCommentList, SmileVideoSettingModel setting)
        {
            var element = CreateCommentElement(commentViewModel, commentArea, setting);

            commentViewModel.NowShowing = true;

            commentParentElement.Children.Add(element);
            commentParentElement.UpdateLayout();

            SetCommentPosition(commentViewModel, element, commentArea, showingCommentList, setting);

            // アニメーション設定
            var animation = CreateCommentAnimeation(commentViewModel, element, commentArea, prevTime - correctionTime, setting.Comment.ShowTime + correctionTime);

            var data = new SmileVideoCommentDataModel(element, commentViewModel, animation);
            showingCommentList.Add(data);

            EventDisposer<EventHandler> ev = null;
            data.Clock.Completed += EventUtility.Create<EventHandler>((object sender, EventArgs e) => {
                if(element != null) {
                    commentParentElement.Children.Remove(element);
                }
                element = null;

                if(ev != null) {
                    ev.Dispose();
                }
                ev = null;

                showingCommentList.Remove(data);
                data.ViewModel.NowShowing = false;

            }, h => commentParentElement.Dispatcher.BeginInvoke(new Action(() => animation.Completed -= h)), out ev);

            element.ApplyAnimationClock(Canvas.LeftProperty, data.Clock);
        }

        public static bool InShowTime(SmileVideoCommentViewModel comment, TimeSpan prevTime, TimeSpan nowTime)
        {
            var correction = nowTime < correctionTime ? TimeSpan.Zero : correctionTime;
            return prevTime <= (comment.ElapsedTime - correction) && (comment.ElapsedTime - correction) <= nowTime;
        }

        public static void FireShowCommentsCore(Canvas commentParentElement, Size commentArea, TimeSpan prevTime, TimeSpan nowTime, IList<SmileVideoCommentViewModel> commentViewModelList, IList<SmileVideoCommentDataModel> showingCommentList, SmileVideoSettingModel setting)
        {
            //var commentArea = new Size(
            //   commentParentElement.ActualWidth,
            //   commentParentElement.ActualHeight
            //);

            var list = commentViewModelList.ToArray();
            // 現在時間から-1秒したものを表示対象とする
            var newComments = list
                .Where(c => c.Approval)
                .Where(c => !c.NowShowing)
                .Where(c => InShowTime(c, prevTime, nowTime))
                .ToArray()
            ;
            if(newComments.Any()) {
                foreach(var commentViewModel in newComments) {
                    FireShowSingleComment(commentViewModel, commentParentElement, commentArea, prevTime, showingCommentList, setting);
                    //var element = CreateCommentElement(commentViewModel, commentArea, setting);

                    //commentViewModel.NowShowing = true;

                    //commentParentElement.Children.Add(element);
                    //commentParentElement.UpdateLayout();

                    //SetCommentPosition(commentViewModel, element, commentArea, showingCommentList, setting);

                    //// アニメーション設定
                    //var animation = CreateCommentAnimeation(commentViewModel, element, commentArea, prevTime - correctionTime, setting.Comment.ShowTime + correctionTime);

                    //var data = new SmileVideoCommentDataModel(element, commentViewModel, animation);
                    //showingCommentList.Add(data);

                    //EventDisposer<EventHandler> ev = null;
                    //data.Clock.Completed += EventUtility.Create<EventHandler>((object sender, EventArgs e) => {
                    //    if(element != null) {
                    //        commentParentElement.Children.Remove(element);
                    //    }
                    //    element = null;

                    //    if(ev != null) {
                    //        ev.Dispose();
                    //    }
                    //    ev = null;

                    //    showingCommentList.Remove(data);
                    //    data.ViewModel.NowShowing = false;

                    //}, h => commentParentElement.Dispatcher.BeginInvoke(new Action(() => animation.Completed -= h)), out ev);

                    //element.ApplyAnimationClock(Canvas.LeftProperty, data.Clock);
                }
                // 超過分のコメントを破棄
                if(setting.Player.IsEnabledDisplayCommentLimit && 0 < setting.Player.DisplayCommentLimitCount) {
                    var removeList = showingCommentList
                        .OrderBy(i => i.ViewModel.ElapsedTime)
                        .ThenBy(i => i.ViewModel.Number)
                        .Take(showingCommentList.Count - setting.Player.DisplayCommentLimitCount)
                        .ToArray()
                    ;
                    foreach(var item in removeList) {
                        item.Clock.Controller.SkipToFill();
                    }
                }
            }
        }

        #endregion
    }
}
