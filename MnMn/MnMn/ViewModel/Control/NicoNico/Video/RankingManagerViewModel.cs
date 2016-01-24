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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.NicoNico.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Control.NicoNico.Video
{
    public class RankingManagerViewModel: ViewModelBase
    {
        #region variable

        ElementModel _selectedPeriod;
        ElementModel _selectedTarget;

        ElementModel _selectedCategory;

        RankingCategoryItemViewModel _selectedRankingCategory;

        #endregion

        public RankingManagerViewModel(Mediation mediation, RankingModel rankingModel)
        {
            Mediation = mediation;

            RankingModel = rankingModel;
            CategoryItems = new CollectionModel<ElementModel>(GetLinearRankingElementList(RankingModel.Items));
            SelectedPeriod = PeriodItems.First();
            SelectedTarget = TargetItems.First();
            SelectedCategory = CategoryItems.First();
        }

        #region property

        Mediation Mediation { get; set; }

        RankingModel RankingModel { get; set; }

        public IList<ElementModel> PeriodItems { get { return RankingModel.Periods.Items; } }
        public IList<ElementModel> TargetItems { get { return RankingModel.Targets.Items; } }

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
        public ElementModel SelectedCategory
        {
            get { return this._selectedCategory; }
            set { SetVariableValue(ref this._selectedCategory, value); }
        }

        public IList<ElementModel> CategoryItems { get; private set; }

        public RankingCategoryItemViewModel SelectedRankingCategory{
            get { return this._selectedRankingCategory; }
            set { SetVariableValue(ref this._selectedRankingCategory, value); }
        }

        public CollectionModel<RankingCategoryItemViewModel> RankingCategoryItems { get; set; } = new CollectionModel<RankingCategoryItemViewModel>();

        #endregion

        #region command

        public ICommand GetRankingCategoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var nowPeriod = SelectedPeriod;
                        var nowTarget = SelectedTarget;
                        var nowCategory = SelectedCategory;

                        // すでに存在する場合はそのタブへ遷移
                        var selectViewModel = RestrictUtility.IsNotNull(
                            RankingCategoryItems.FirstOrDefault(i => i.Category.Key == nowCategory.Key),
                            viewModel => {
                                viewModel.SetContextElements(nowPeriod, nowTarget);
                                return viewModel;
                            },
                            () => {
                                var viewModel = new RankingCategoryItemViewModel(Mediation, RankingModel, nowPeriod, nowTarget, nowCategory);
                                return viewModel;
                            }
                        );
                        selectViewModel.LoadRankingAsync().ContinueWith(task => {
                            if(!RankingCategoryItems.Any(vm => vm == selectViewModel)) {
                                RankingCategoryItems.Insert(0, selectViewModel);
                            }
                            SelectedRankingCategory = selectViewModel;
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                );
            }
        }

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
