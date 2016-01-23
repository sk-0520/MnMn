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
    public class RankingGroupElementsViewModel: ViewModelBase
    {
        #region variable

        ElementModel _selectedTarget;
        ElementModel _selectedPeriod;

        #endregion

        public RankingGroupElementsViewModel(RankingGroupModel targetsModel, RankingGroupModel periodsModel)
        {
            TargetsModel = targetsModel;
            PeriodsModel = periodsModel;

            SelectedTarget = TargetItems.First();
            SelectedPeriod = PeriodItems.First();
        }

        #region property

        RankingGroupModel TargetsModel { get; set; }
        RankingGroupModel PeriodsModel { get; set; }

        public IList<ElementModel> TargetItems { get { return TargetsModel.Items; } }
        public IList<ElementModel> PeriodItems { get { return PeriodsModel.Items; } }

        public ElementModel SelectedTarget
        {
            get { return this._selectedTarget; }
            set { SetVariableValue(ref this._selectedTarget, value); }
        }

        public ElementModel SelectedPeriod
        {
            get { return this._selectedPeriod; }
            set { SetVariableValue(ref this._selectedPeriod, value); }
        }

        #endregion
    }
}
