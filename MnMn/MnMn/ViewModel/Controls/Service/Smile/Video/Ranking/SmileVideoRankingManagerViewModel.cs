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
using MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Ranking
{
    public class SmileVideoRankingManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        #region variable

        DefinedElementModel _selectedPeriod;
        DefinedElementModel _selectedTarget;

        SmileVideoRankingCategoryDefinedElementViewModel _selectedCategory;

        SmileVideoRankingCategoryFinderViewModel _selectedRankingCategory;

        #endregion

        public SmileVideoRankingManagerViewModel(Mediation mediation, SmileVideoRankingModel rankingModel)
            : base(mediation)
        {
            RankingModel = rankingModel;

            SelectedPeriod = PeriodItems.FirstOrDefault(m => m.Key == Setting.Ranking.DefaultPeriodKey) ?? PeriodItems.First();
            SelectedTarget = TargetItems.FirstOrDefault(m => m.Key == Setting.Ranking.DefaultTargetKey) ?? TargetItems.First();

            // 初期化で読み込みが動いちゃう対応
            var categoryItems = new CollectionModel<SmileVideoRankingCategoryDefinedElementViewModel>(GetLinearRankingElementList(RankingModel.Items));
            SelectedCategory = categoryItems.FirstOrDefault(m => m.Model.Key == Setting.Ranking.DefaultCategoryKey) ?? CategoryItems.First();
            CategoryItems = categoryItems;
        }

        #region property

        SmileVideoRankingModel RankingModel { get; set; }

        public IList<DefinedElementModel> PeriodItems { get { return RankingModel.Periods.Items; } }
        public IList<DefinedElementModel> TargetItems { get { return RankingModel.Targets.Items; } }

        public DefinedElementModel SelectedPeriod
        {
            get { return this._selectedPeriod; }
            set
            {
                if(SetVariableValue(ref this._selectedPeriod, value)) {
                    Setting.Ranking.DefaultPeriodKey = SelectedPeriod.Key;
                }
            }
        }
        public DefinedElementModel SelectedTarget
        {
            get { return this._selectedTarget; }
            set
            {
                if(SetVariableValue(ref this._selectedTarget, value)) {
                    Setting.Ranking.DefaultTargetKey = SelectedTarget.Key;
                }
            }
        }
        public SmileVideoRankingCategoryDefinedElementViewModel SelectedCategory
        {
            get { return this._selectedCategory; }
            set
            {
                if(SetVariableValue(ref this._selectedCategory, value)) {
                    Setting.Ranking.DefaultCategoryKey = SelectedCategory.Model.Key;
                    // CategoryItems が null だとコンストラクタ中！
                    if(this._selectedCategory != null && CategoryItems != null) {
                        LoadRankingCategoryAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        public IList<SmileVideoRankingCategoryDefinedElementViewModel> CategoryItems { get; private set; }

        public SmileVideoRankingCategoryFinderViewModel SelectedRankingCategory
        {
            get { return this._selectedRankingCategory; }
            set { SetVariableValue(ref this._selectedRankingCategory, value); }
        }

        public CollectionModel<SmileVideoRankingCategoryFinderViewModel> RankingCategoryGroupItems { get; set; } = new CollectionModel<SmileVideoRankingCategoryFinderViewModel>();

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

                        LoadRankingCategoryCoreAsync(nowPeriod, nowTarget, nowCategory.Model).ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand CloseTabCommand
        {
            get
            {
                return CreateCommand(o => CloseTab((SmileVideoRankingCategoryFinderViewModel)o));
            }
        }

        #endregion

        #region function

        IEnumerable<SmileVideoRankingCategoryDefinedElementViewModel> GetLinearRankingElementList(IEnumerable<SmileVideoCategoryGroupModel> items)
        {
            foreach(var item in items) {
                if(item.IsSingleCategory) {
                    yield return new SmileVideoRankingCategoryDefinedElementViewModel(item.Categories.Single(), true);
                } else {
                    foreach(var c in item.Categories.Take(1)) {
                        yield return new SmileVideoRankingCategoryDefinedElementViewModel(c, true);
                    }
                    foreach(var c in item.Categories.Skip(1)) {
                        yield return new SmileVideoRankingCategoryDefinedElementViewModel(c, false);
                    }
                }
            }
        }

        public Task LoadRankingCategoryAsync()
        {
            var nowPeriod = SelectedPeriod;
            var nowTarget = SelectedTarget;
            var nowCategory = SelectedCategory;

            return LoadRankingCategoryCoreAsync(nowPeriod, nowTarget, nowCategory.Model);
        }

        Task LoadRankingCategoryCoreAsync(DefinedElementModel period, DefinedElementModel target, DefinedElementModel category)
        {
            // 存在する場合は該当タブへ遷移
            var selectViewModel = RestrictUtility.IsNotNull(
                RankingCategoryGroupItems.FirstOrDefault(i => i.Category.Key == category.Key),
                viewModel => {
                    viewModel.SetContextElements(period, target);
                    return viewModel;
                },
                () => {
                    var viewModel = new SmileVideoRankingCategoryFinderViewModel(Mediation, RankingModel, period, target, category);
                    RankingCategoryGroupItems.Insert(0, viewModel);
                    return viewModel;
                }
            );

            SelectedRankingCategory = selectViewModel;
            return selectViewModel.LoadDefaultCacheAsync();
        }

        void CloseTab(SmileVideoRankingCategoryFinderViewModel finder)
        {
            RankingCategoryGroupItems.Remove(finder);
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

        public override void InitializeView(MainWindow view)
        { }
        public override void UninitializeView(MainWindow view)
        { }

        #endregion
    }
}
