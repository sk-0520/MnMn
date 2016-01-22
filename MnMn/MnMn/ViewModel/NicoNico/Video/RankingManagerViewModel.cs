using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.NicoNico.Video
{
    public class RankingManagerViewModel: ViewModelBase
    {
        public RankingManagerViewModel(RankingModel rankingModel)
        {
            RankingToolbar = new RankingToolbarViewModel(rankingModel);
        }

        #region property

        RankingToolbarViewModel RankingToolbar { get; set; }

        #endregion
    }
}
