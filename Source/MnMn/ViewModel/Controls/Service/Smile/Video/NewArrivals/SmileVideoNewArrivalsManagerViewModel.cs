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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.NewArrivals
{
    public class SmileVideoNewArrivalsManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        public SmileVideoNewArrivalsManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            NewArrival = new SmileVideoNewArrivalsFinderViewModel(Mediation, SmileVideoMediationKey.newarrival);
            Recent = new SmileVideoNewArrivalsFinderViewModel(Mediation, SmileVideoMediationKey.recent);
            Hotlist = new SmileVideoHotlistFinderViewModel(Mediation);
            Recommendations = new SmileVideoRecommendationsFinderViewModel(Mediation);

            ItemsList = new CollectionModel<SmileVideoNewArrivalsFinderViewModel>(new[] {
                Hotlist,
                NewArrival,
                Recent,
                Recommendations,
            });
        }

        #region property

        SmileVideoNewArrivalsFinderViewModel NewArrival { get; }
        SmileVideoNewArrivalsFinderViewModel Recent { get; }
        SmileVideoHotlistFinderViewModel Hotlist { get; }
        SmileVideoRecommendationsFinderViewModel Recommendations { get; }

        public CollectionModel<SmileVideoNewArrivalsFinderViewModel> ItemsList { get; }

        #endregion

        #region SmileVideoManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            if(SelectedItem != null) {
                return;
            }

            SelectedItem = ItemsList.First();
            SelectedItem.LoadDefaultCacheAsync().ConfigureAwait(false);
        }

        protected override void HideViewCore()
        { }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        #endregion
    }
}
