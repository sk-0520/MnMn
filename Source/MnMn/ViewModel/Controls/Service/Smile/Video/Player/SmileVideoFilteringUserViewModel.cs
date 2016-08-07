using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player
{
    public class SmileVideoFilteringUserViewModel: ViewModelBase
    {
        public SmileVideoFilteringUserViewModel(string userId, SmileVideoUserKind userKind, int count)
        {
            UserId = userId;
            UserKind = userKind;
            Count = count;
        }

        #region proeprty

        public string UserId { get; }
        public SmileVideoUserKind UserKind { get; }
        public int Count { get; }

        #endregion

        #region ViewModelBase

        public override string ToString()
        {
            return UserId;
        }

        #endregion
    }
}
