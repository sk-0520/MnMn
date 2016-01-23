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
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico.Video
{
    public class RankingToolbarViewModel: SingleModelWrapperViewModelBase<RankingModel>
    {
        #region variable

        ElementModel _selectedCategoryItem;
        IList<ElementModel> _rankingCategoryItems;

        #endregion

        public RankingToolbarViewModel(RankingModel model)
            :base(model)
        {
            RankingCategoryItems = GetLinearRankingElementList(Model.Items).ToList();
            RankingGroupElements = new RankingGroupElementsViewModel(Model.Targets, Model.Periods);
            SelectedCategoryItem = RankingCategoryItems.First();
        }

        #region property

        public RankingGroupElementsViewModel RankingGroupElements { get; }

        public IList<ElementModel> RankingCategoryItems {
            get { return this._rankingCategoryItems; }
            private set { this._rankingCategoryItems = value; }
        }

        public ElementModel SelectedCategoryItem
        {
            get { return this._selectedCategoryItem; }
            set { SetVariableValue(ref this._selectedCategoryItem, value); }
        }

        public ElementModel SelectedTarget
        {
            get { return RankingGroupElements.SelectedTarget; }
        }
        public ElementModel SelectedPeriod
        {
            get { return RankingGroupElements.SelectedPeriod; }
        }

        #endregion

        #region command
        #endregion

        #region function

        IEnumerable<ElementModel> GetLinearRankingElementList(IEnumerable<CategoryGroupModel> items)
        {
            foreach(var item in items) {
                if(item.IsSingleCategory) {
                    yield return item.Categories.Single();
                } else {
                    foreach(var c in item.Categories) {
                        yield return c;
                    }
                }
            }
        }

        #endregion
    }
}
