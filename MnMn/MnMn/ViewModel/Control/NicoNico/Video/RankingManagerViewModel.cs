using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico.Video
{
    public class RankingManagerViewModel: ViewModelBase
    {
        public RankingManagerViewModel(RankingModel rankingModel)
        {
            RankingToolbar = new RankingToolbarViewModel(rankingModel);
            RankingCategoryItems = new CollectionModel<RankingCategoryItemViewModel>();
        }

        #region property

        public RankingToolbarViewModel RankingToolbar { get; private set; }

        public CollectionModel<RankingCategoryItemViewModel> RankingCategoryItems { get; private set; } 

        #endregion
    }
}
