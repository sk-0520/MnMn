using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Raw;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Market
{
    public class SmileMarketVideoRelationItemViewModel: SingleModelWrapperViewModelBase<SmileMarketVideoItemModel>
    {
        public SmileMarketVideoRelationItemViewModel(Mediation mediation, SmileMarketVideoItemModel model) 
            : base(model)
        {
            Mediation = mediation;
        }

        #region property

        Mediation Mediation { get; set; }

        public string Title => Model.Title;

        #endregion
    }
}
