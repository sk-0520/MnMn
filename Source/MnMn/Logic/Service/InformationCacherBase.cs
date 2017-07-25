using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Service
{
    public abstract class InformationCacherBase<TInformationViewModelBase>: Cacher<string, TInformationViewModelBase>
        where TInformationViewModelBase: InformationViewModelBase
    {
        #region variable

        protected object checker = new object();

        #endregion

        public InformationCacherBase(Mediator mediator)
            : base(true)
        {
            Mediation = mediator;
        }

        #region property

        protected Mediator Mediation { get; }

        #endregion
    }
}
