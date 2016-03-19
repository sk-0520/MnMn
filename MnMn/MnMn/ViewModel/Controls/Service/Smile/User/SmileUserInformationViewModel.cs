using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User
{
    /// <summary>
    /// ユーザー情報表示用VM。
    /// </summary>
    public class SmileUserInformationViewModel: ViewModelBase
    {
        public SmileUserInformationViewModel(Mediation mediation)
        {
            Mediation = mediation;
        }

        #region property

        protected Mediation Mediation { get; set; }

        #endregion
    }
}
