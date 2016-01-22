using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.NicoNico.Video
{
    public class RankingCategoryItemViewModel: ViewModelBase
    {
        public RankingCategoryItemViewModel(RankingModel rankigModel, ElementModel initTarget, ElementModel initPeriod)
        {

        }

        #region property

        RankingToolbarViewModel RankingToolbar { get; set; }


        #endregion
    }
}
