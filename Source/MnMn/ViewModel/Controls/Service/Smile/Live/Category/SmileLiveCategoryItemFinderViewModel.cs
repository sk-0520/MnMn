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
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live.Category
{
    public class SmileLiveCategoryItemFinderViewModel: SmileLiveFinderViewModelBase
    {
        public SmileLiveCategoryItemFinderViewModel(Mediation mediation, SmileLiveCategoryModel categoryDefine, DefinedElementModel sort, DefinedElementModel order, DefinedElementModel category, int index)
            : base(mediation)
        {
            CategoryDefine = categoryDefine;

            Sort = sort;
            Order = order;
            Category = category;
            Index = index;
        }

        #region property

        SmileLiveCategoryModel CategoryDefine { get; }

        public DefinedElementModel Sort { get; }
        public DefinedElementModel Order { get; }
        public DefinedElementModel Category { get; }
        public int Index { get; }

        #endregion

        #region SmileLiveCategoryItemFinderViewModel

        protected override Task LoadCoreAsync(CacheSpan informationCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            var category = new Logic.Service.Smile.Live.Api.Category(Mediation);
            return category.LoadAsync(Category.Key, Sort.Key, Order.Key, Index + 1).ContinueWith(t => {
                var rss = t.Result;
            });
        }

        #endregion
    }
}
