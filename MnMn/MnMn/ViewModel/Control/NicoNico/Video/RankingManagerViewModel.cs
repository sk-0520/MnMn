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
    public class RankingManagerViewModel: ViewModelBase
    {
        #region variable

        ElementModel _selectedTarget;
        ElementModel _selectedPeriod;

        ElementModel _selectedCategory;

        #endregion

        public RankingManagerViewModel(RankingModel rankingModel)
        {
            RankingModel = rankingModel;
            RankingCategoryItems = new CollectionModel<ElementModel>(GetLinearRankingElementList(RankingModel.Items));
            SelectedTarget = TargetItems.First();
            SelectedPeriod = PeriodItems.First();
            SelectedCategory = RankingCategoryItems.First();
        }

        #region property

        #region RankingToolbar

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

        public CollectionModel<ElementModel> RankingCategoryItems { get; private set; }

        public ElementModel SelectedCategory
        {
            get { return this._selectedCategory; }
            set { SetVariableValue(ref this._selectedCategory, value); }
        }

        #endregion

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

        void LoadRankingAsync(ElementModel preiod, ElementModel target, ElementModel category)
        {

        }

        #endregion
    }
}
