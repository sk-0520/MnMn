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
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico.Video
{
    public class RankingCategoryItemViewModel: ViewModelBase
    {
        #region variable

        ElementModel _selectedPeriod;
        ElementModel _selectedTarget;

        #endregion

        public RankingCategoryItemViewModel(RankingModel rankingModel, ElementModel period, ElementModel target, ElementModel category)
        {
            PeriodItems = new CollectionModel<ElementModel>(rankingModel.Periods.Items.Select(i => (ElementModel)i.DeepClone()));
            TargetItems = new CollectionModel<ElementModel>(rankingModel.Targets.Items.Select(i => (ElementModel)i.DeepClone()));

            SetContextElements(period, target);
            Category = category;
        }

        #region property

        public IEnumerable<ElementModel> PeriodItems { get; private set; }
        public IEnumerable<ElementModel> TargetItems { get; private set; }

        public ElementModel SelectedPeriod
        {
            get { return this._selectedPeriod; }
            set { SetVariableValue(ref this._selectedPeriod, value); }
        }
        public ElementModel SelectedTarget
        {
            get { return this._selectedTarget; }
            set { SetVariableValue(ref this._selectedTarget, value); }
        }

        public ElementModel Category { get; private set; }

        public IList<object> VideoInformationItems { get; set; }

        public string CategoryName => Category.CurrentWord;

        #endregion

        #region command

        public ICommand ReloadCategoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {

                    }
                );
            }
        }

        #endregion

        #region function

        public void LoadRankingAsync()
        { }

        ElementModel GetContextElemetFromChangeElement(IEnumerable<ElementModel> items, ElementModel element)
        {
            if(items.Any(i => i == element)) {
                return element;
            } else {
                return items.FirstOrDefault(i => i.Key == element.Key);
            }
        }

        public void SetContextElements(ElementModel period, ElementModel target)
        {
            SelectedPeriod = GetContextElemetFromChangeElement(PeriodItems, period);
            SelectedTarget = GetContextElemetFromChangeElement(TargetItems, target);
        }

        #endregion
    }
}
