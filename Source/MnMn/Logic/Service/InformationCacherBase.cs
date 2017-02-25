using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service
{
    public abstract class InformationCacherBase<TInformationViewModelBase>: Caching<string, TInformationViewModelBase>
        where TInformationViewModelBase: InformationViewModelBase
    {
        #region variable

        protected object checker = new object();

        #endregion

        public InformationCacherBase(Mediation mediation)
            : base(true)
        {
            Mediation = mediation;
        }

        #region property

        protected Mediation Mediation { get; }

        #endregion
    }
}
