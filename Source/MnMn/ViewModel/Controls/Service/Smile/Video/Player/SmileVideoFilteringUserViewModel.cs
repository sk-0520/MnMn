using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    /// <summary>
    /// コメントリストのユーザーフィルタリング。
    /// <para>コメントを表示するかどうかのフィルタリングでないことに注意。</para>
    /// </summary>
    public class SmileVideoFilteringUserViewModel: ViewModelBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="userId">ユーザーID。</param>
        /// <param name="userKind">ユーザー種別。</param>
        /// <param name="count">投稿数。</param>
        public SmileVideoFilteringUserViewModel(string userId, SmileVideoUserKind userKind, int count)
        {
            UserId = userId;
            UserKind = userKind;
            Count = count;
        }

        #region proeprty

        /// <summary>
        /// ユーザーID。
        /// </summary>
        public string UserId { get; }
        /// <summary>
        /// ユーザー種別。
        /// </summary>
        public SmileVideoUserKind UserKind { get; }
        /// <summary>
        /// 投稿数。
        /// </summary>
        public int Count { get; }

        #endregion

        #region ViewModelBase

        /// <summary>
        /// コンボボックスで選択された際に使用する文字列。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return UserId;
        }

        #endregion
    }
}
