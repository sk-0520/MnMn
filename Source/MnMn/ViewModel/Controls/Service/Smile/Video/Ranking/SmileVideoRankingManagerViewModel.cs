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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Ranking
{
    public class SmileVideoRankingManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        #region variable

        IReadOnlyDefinedElement _selectedPeriod;
        IReadOnlyDefinedElement _selectedTarget;

        SmileVideoRankingCategoryDefinedElementViewModel _selectedCategory;

        SmileVideoRankingCategoryFinderViewModel _selectedRankingCategory;

        #endregion

        public SmileVideoRankingManagerViewModel(Mediation mediation, IReadOnlySmileVideoRanking rankingDefine)
            : base(mediation)
        {
            RankingDefine = rankingDefine;

            SelectedPeriod = PeriodItems.FirstOrDefault(m => m.Key == Setting.Ranking.DefaultPeriodKey) ?? PeriodItems.First();
            SelectedTarget = TargetItems.FirstOrDefault(m => m.Key == Setting.Ranking.DefaultTargetKey) ?? TargetItems.First();
        }

        #region property

        IReadOnlySmileVideoRanking RankingDefine { get; set; }

        IReadOnlyList<string> CurrentIgnoreCategoryItems { get; set; }

        public IReadOnlyList<IReadOnlyDefinedElement> PeriodItems { get { return RankingDefine.Periods.Items; } }
        public IReadOnlyList<IReadOnlyDefinedElement> TargetItems { get { return RankingDefine.Targets.Items; } }

        public IReadOnlyDefinedElement SelectedPeriod
        {
            get { return this._selectedPeriod; }
            set
            {
                if(SetVariableValue(ref this._selectedPeriod, value)) {
                    Setting.Ranking.DefaultPeriodKey = SelectedPeriod.Key;
                }
            }
        }
        public IReadOnlyDefinedElement SelectedTarget
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

        void MakeUsingCategory()
        {
            // 初期化で読み込みが動いちゃう対応
            IEnumerable<SmileVideoRankingCategoryDefinedElementViewModel> items = GetLinearRankingElementList(RankingDefine.Items).ToEvaluatedSequence();
            var removeItems = items.Where(i => Setting.Ranking.IgnoreCategoryItems.Any(s => s == i.Key));
            if(removeItems.Any()) {
                var removedItems = items.Except(removeItems);
                if(removedItems.Any()) {
                    items = removedItems;
                }
            }

            IList<SmileVideoRankingCategoryDefinedElementViewModel> categoryItems = new List<SmileVideoRankingCategoryDefinedElementViewModel>(items);

            if(!Session.IsLoggedIn || !Session.IsOver18) {
                var expitems = categoryItems
                    .Where(c => c.Extends.ContainsKey("age"))
                    .Where(c => Session.Age < int.Parse(c.Extends["age"]))
                ;
                categoryItems = categoryItems.Except(expitems).ToEvaluatedSequence();
            }
            CategoryItems = null;
            SelectedCategory = categoryItems.FirstOrDefault(m => m.Model.Key == Setting.Ranking.DefaultCategoryKey) ?? categoryItems.First();
            CategoryItems = CollectionModel.Create(categoryItems);
            CallOnPropertyChange(nameof(CategoryItems));

            CurrentIgnoreCategoryItems = Setting.Ranking.IgnoreCategoryItems.ToEvaluatedSequence();
        }

        IEnumerable<SmileVideoRankingCategoryDefinedElementViewModel> GetLinearRankingElementList(IEnumerable<IReadOnlySmileVideoCategoryGroup> items)
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

        Task LoadRankingCategoryAsync()
        {
            var nowPeriod = SelectedPeriod;
            var nowTarget = SelectedTarget;
            var nowCategory = SelectedCategory;

            return LoadRankingCategoryCoreAsync(nowPeriod, nowTarget, nowCategory.Model);
        }

        public Task LoadRankingCategoryFromParameterAsync(SmileVideoRankingCategoryNameParameterModel parameter)
        {
            if(string.IsNullOrWhiteSpace(parameter.CategoryName)) {
                return Task.CompletedTask;
            }

            if(CategoryItems == null) {
                MakeUsingCategory();
            }

            var targetCategory = CategoryItems.FirstOrDefault(c => c.DisplayText == parameter.CategoryName);
            if(targetCategory == null) {
                Mediation.Logger.Warning($"not found: {parameter.CategoryName}");
                return Task.CompletedTask;
            }

            var nowPeriod = SelectedPeriod;
            var nowTarget = SelectedTarget;

            return LoadRankingCategoryCoreAsync(nowPeriod, nowTarget, targetCategory.Model);
        }

        Task LoadRankingCategoryCoreAsync(IReadOnlyDefinedElement period, IReadOnlyDefinedElement target, IReadOnlyDefinedElement category)
        {
            // 存在する場合は該当タブへ遷移
            var selectViewModel = RestrictUtility.IsNotNull(
                RankingCategoryGroupItems.FirstOrDefault(i => i.Category.Key == category.Key),
                viewModel => {
                    viewModel.SetContextElements(period, target);
                    return viewModel;
                },
                () => {
                    var viewModel = new SmileVideoRankingCategoryFinderViewModel(Mediation, RankingDefine, period, target, category);
                    RankingCategoryGroupItems.Add(viewModel);

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

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            if(CategoryItems == null) {
                MakeUsingCategory();
            } else if(CurrentIgnoreCategoryItems != null) {
                var hasDiff = !Setting.Ranking.IgnoreCategoryItems
                    .OrderBy(s => s)
                    .SequenceEqual(CurrentIgnoreCategoryItems.OrderBy(s => s))
                ;
                if(hasDiff) {
                    MakeUsingCategory();
                }
            }

            if(!RankingCategoryGroupItems.Any()) {
                LoadRankingCategoryAsync();
            }
        }

        protected override void HideViewCore()
        { }

        public override Task InitializeAsync()
        {
            MakeUsingCategory();

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
