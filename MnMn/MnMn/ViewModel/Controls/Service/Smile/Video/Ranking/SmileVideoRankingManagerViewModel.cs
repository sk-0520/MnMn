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
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Ranking
{
    public class SmileVideoRankingManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        #region variable

        DefinedElementModel _selectedPeriod;
        DefinedElementModel _selectedTarget;

        DefinedElementModel _selectedCategory;

        SmileVideoRankingCategoryGroupItemViewModel _selectedRankingCategory;

        #endregion

        public SmileVideoRankingManagerViewModel(Mediation mediation, SmileVideoRankingModel rankingModel)
            : base(mediation)
        {
            RankingModel = rankingModel;
            CategoryItems = new CollectionModel<DefinedElementModel>(GetLinearRankingElementList(RankingModel.Items));
            SelectedPeriod = PeriodItems.First();
            SelectedTarget = TargetItems.First();
            SelectedCategory = CategoryItems.First();
        }

        #region property

        SmileVideoRankingModel RankingModel { get; set; }

        public IList<DefinedElementModel> PeriodItems { get { return RankingModel.Periods.Items; } }
        public IList<DefinedElementModel> TargetItems { get { return RankingModel.Targets.Items; } }

        public DefinedElementModel SelectedPeriod
        {
            get { return this._selectedPeriod; }
            set { SetVariableValue(ref this._selectedPeriod, value); }
        }
        public DefinedElementModel SelectedTarget
        {
            get { return this._selectedTarget; }
            set { SetVariableValue(ref this._selectedTarget, value); }
        }
        public DefinedElementModel SelectedCategory
        {
            get { return this._selectedCategory; }
            set { SetVariableValue(ref this._selectedCategory, value); }
        }

        public IList<DefinedElementModel> CategoryItems { get; private set; }

        public SmileVideoRankingCategoryGroupItemViewModel SelectedRankingCategory
        {
            get { return this._selectedRankingCategory; }
            set { SetVariableValue(ref this._selectedRankingCategory, value); }
        }

        public CollectionModel<SmileVideoRankingCategoryGroupItemViewModel> RankingCategoryGroupItems { get; set; } = new CollectionModel<SmileVideoRankingCategoryGroupItemViewModel>();

        #endregion

        #region command

        public ICommand LoadRankingCategoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var nowPeriod = SelectedPeriod;
                        var nowTarget = SelectedTarget;
                        var nowCategory = SelectedCategory;

                        LoadRankingCategoryAsync(nowPeriod, nowTarget, nowCategory).ConfigureAwait(false);
                    }
                );
            }
        }

        #endregion

        #region function

        IEnumerable<DefinedElementModel> GetLinearRankingElementList(IEnumerable<SmileVideoCategoryGroupModel> items)
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

        public Task LoadRankingCategoryAsync()
        {
            var nowPeriod = SelectedPeriod;
            var nowTarget = SelectedTarget;
            var nowCategory = SelectedCategory;

            return LoadRankingCategoryAsync(nowPeriod, nowTarget, nowCategory);
        }

        Task LoadRankingCategoryAsync(DefinedElementModel period, DefinedElementModel target, DefinedElementModel category)
        {
            // 存在する場合は該当タブへ遷移
            var selectViewModel = RestrictUtility.IsNotNull(
                RankingCategoryGroupItems.FirstOrDefault(i => i.Category.Key == category.Key),
                viewModel => {
                    viewModel.SetContextElements(period, target);
                    return viewModel;
                },
                () => {
                    var viewModel = new SmileVideoRankingCategoryGroupItemViewModel(Mediation, RankingModel, period, target, category);
                    RankingCategoryGroupItems.Insert(0, viewModel);
                    return viewModel;
                }
            );

            return selectViewModel.LoadDefaultCacheAsync().ContinueWith(task => {
                SelectedRankingCategory = selectViewModel;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        #region SmileVideoManagerViewModelBase

        protected override void ShowView()
        {
            base.ShowView();

            if(!RankingCategoryGroupItems.Any()) {
                LoadRankingCategoryAsync();
            }
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
