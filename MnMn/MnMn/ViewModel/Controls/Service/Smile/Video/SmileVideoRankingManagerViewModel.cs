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
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoRankingManagerViewModel: ViewModelBase
    {
        #region variable

        SmileVideoElementModel _selectedPeriod;
        SmileVideoElementModel _selectedTarget;

        SmileVideoElementModel _selectedCategory;

        SmileVideoRankingCategoryItemViewModel _selectedRankingCategory;

        #endregion

        public SmileVideoRankingManagerViewModel(Mediation mediation, SmileVideoRankingModel rankingModel)
        {
            Mediation = mediation;

            RankingModel = rankingModel;
            CategoryItems = new CollectionModel<SmileVideoElementModel>(GetLinearRankingElementList(RankingModel.Items));
            SelectedPeriod = PeriodItems.First();
            SelectedTarget = TargetItems.First();
            SelectedCategory = CategoryItems.First();
        }

        #region property

        Mediation Mediation { get; set; }

        SmileVideoRankingModel RankingModel { get; set; }

        public IList<SmileVideoElementModel> PeriodItems { get { return RankingModel.Periods.Items; } }
        public IList<SmileVideoElementModel> TargetItems { get { return RankingModel.Targets.Items; } }

        public SmileVideoElementModel SelectedPeriod
        {
            get { return this._selectedPeriod; }
            set { SetVariableValue(ref this._selectedPeriod, value); }
        }
        public SmileVideoElementModel SelectedTarget
        {
            get { return this._selectedTarget; }
            set { SetVariableValue(ref this._selectedTarget, value); }
        }
        public SmileVideoElementModel SelectedCategory
        {
            get { return this._selectedCategory; }
            set { SetVariableValue(ref this._selectedCategory, value); }
        }

        public IList<SmileVideoElementModel> CategoryItems { get; private set; }

        public SmileVideoRankingCategoryItemViewModel SelectedRankingCategory
        {
            get { return this._selectedRankingCategory; }
            set { SetVariableValue(ref this._selectedRankingCategory, value); }
        }

        public CollectionModel<SmileVideoRankingCategoryItemViewModel> RankingCategoryItems { get; set; } = new CollectionModel<SmileVideoRankingCategoryItemViewModel>();

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
                                var viewModel = new SmileVideoRankingCategoryItemViewModel(Mediation, RankingModel, nowPeriod, nowTarget, nowCategory);
                                RankingCategoryItems.Insert(0, viewModel);
                                return viewModel;
                            }
                        );
                        selectViewModel.LoadRankingAsync(Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ContinueWith(task => {
                            SelectedRankingCategory = selectViewModel;
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                );
            }
        }

        #endregion

        #region function

        IEnumerable<SmileVideoElementModel> GetLinearRankingElementList(IEnumerable<SmileVideoCategoryGroupModel> items)
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
