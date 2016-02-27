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
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.NewArrivals;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Ranking;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video
{
    public class SmileVideoManagerPackModel: ManagerPackModelBase
    {
        public SmileVideoManagerPackModel(SmileVideoSearchManagerViewModel searchManager, SmileVideoRankingManagerViewModel rankingManager, SmileVideoNewArrivalsManagerViewModel newArrivalsManager, SmileVideoMyListManagerViewModel myListManager)
        {
            RankingManager = rankingManager;
            SearchManager = searchManager;
            NewArrivalsManager = newArrivalsManager;
            MyListManager = myListManager;
        }

        public SmileVideoRankingManagerViewModel RankingManager { get; }
        public SmileVideoSearchManagerViewModel SearchManager { get; }
        public SmileVideoNewArrivalsManagerViewModel NewArrivalsManager { get; }
        public SmileVideoMyListManagerViewModel MyListManager { get; }
    }
}
