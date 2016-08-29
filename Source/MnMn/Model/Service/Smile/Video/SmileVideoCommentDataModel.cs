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
using System.Windows.Media.Animation;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    /// <summary>
    /// プレイヤー上の表示コメントを保持。
    /// </summary>
    public class SmileVideoCommentDataModel: ModelBase
    {
        public SmileVideoCommentDataModel(SmileVideoCommentElement element, SmileVideoCommentViewModel viewModel, AnimationTimeline animation)
        {
            Element = element;
            ViewModel = viewModel;
            Animation = animation;
            Clock = Animation.CreateClock();
        }

        #region property

        /// <summary>
        /// コメント表示要素。
        /// </summary>
        public SmileVideoCommentElement Element { get; }
        /// <summary>
        /// コメントのViewModel。
        /// </summary>
        public SmileVideoCommentViewModel ViewModel { get; }
        /// <summary>
        /// コメントのアニメーション。
        /// </summary>
        public AnimationTimeline Animation { get; }
        /// <summary>
        /// <see cref="Animation"/>に紐付くクロック。
        /// </summary>
        public AnimationClock Clock { get; }

        #endregion
    }
}
