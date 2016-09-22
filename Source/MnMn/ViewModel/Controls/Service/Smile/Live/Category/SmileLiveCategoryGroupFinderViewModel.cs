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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live.Category
{
    public class SmileLiveCategoryGroupFinderViewModel: SmileLiveFinderViewModelBase
    {
        public SmileLiveCategoryGroupFinderViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

        SmileLiveCategoryItemFinderViewModel SearchFinder { get; set; }

        public CollectionModel<PageViewModel<SmileLiveCategoryItemFinderViewModel>> PageItems { get; set; } = new CollectionModel<PageViewModel<SmileLiveCategoryItemFinderViewModel>>();

        #endregion

        #region function

        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, bool isReload)
        {
            return LoadCoreAsync(thumbCacheSpan, imageCacheSpan, isReload);
        }

        #endregion

        #region SmileLiveFinderViewModelBase

        protected override Task LoadCoreAsync(CacheSpan informationCacheSpan, CacheSpan imageCacheSpan, object extends)
        {

            return base.LoadCoreAsync(informationCacheSpan, imageCacheSpan, extends);
        }

        #endregion
    }
}
