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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Feed.Rss2;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Ranking
{
    public class SmileVideoRankingCategoryFinderViewModel: SmileVideoFeedFinderViewModelBase, ISelected
    {
        #region variable

        DefinedElementModel _selectedPeriod;
        DefinedElementModel _selectedTarget;

        bool _isSelected;

        #endregion

        public SmileVideoRankingCategoryFinderViewModel(Mediation mediation, SmileVideoRankingModel rankingModel, DefinedElementModel period, DefinedElementModel target, DefinedElementModel category)
            : base(mediation)
        {
            PeriodItems = new CollectionModel<DefinedElementModel>(rankingModel.Periods.Items.Select(i => (DefinedElementModel)i.DeepClone()));
            TargetItems = new CollectionModel<DefinedElementModel>(rankingModel.Targets.Items.Select(i => (DefinedElementModel)i.DeepClone()));

            SetContextElements(period, target);
            Category = category;
        }

        #region property

        public IEnumerable<DefinedElementModel> PeriodItems { get; private set; }
        public IEnumerable<DefinedElementModel> TargetItems { get; private set; }

        public DefinedElementModel SelectedPeriod
        {
            get { return this._selectedPeriod; }
            set
            {
                if(SetVariableValue(ref this._selectedPeriod, value)) {
                    if(SelectedPeriod != null && Category != null) {
                        ReloadCommand.TryExecute(null);
                    }
                }
            }
        }
        public DefinedElementModel SelectedTarget
        {
            get { return this._selectedTarget; }
            set
            {
                if(SetVariableValue(ref this._selectedTarget, value)) {
                    if(SelectedTarget != null && Category != null) {
                        ReloadCommand.TryExecute(null);
                    }
                }
            }
        }

        public DefinedElementModel Category { get; private set; }

        public string CategoryName => Category.DisplayText;

        #endregion

        #region command

        #endregion

        #region function

        DefinedElementModel GetContextElemetFromChangeElement(IEnumerable<DefinedElementModel> items, DefinedElementModel element)
        {
            if(items.Any(i => i == element)) {
                return element;
            } else {
                return items.FirstOrDefault(i => i.Key == element.Key);
            }
        }

        public void SetContextElements(DefinedElementModel period, DefinedElementModel target)
        {
            SelectedPeriod = GetContextElemetFromChangeElement(PeriodItems, period);
            SelectedTarget = GetContextElemetFromChangeElement(TargetItems, target);
        }

        #endregion

        #region SmileVideoFeedFinderViewModelBase

        [Obsolete]
        protected override bool IsLoadVideoInformation => false;
        protected override SmileVideoInformationFlags InformationFlags => SmileVideoInformationFlags.All;

        protected override Task<FeedSmileVideoModel> LoadFeedAsync()
        {
            var ranking = new Logic.Service.Smile.Video.Api.V1.Ranking(Mediation);
            return ranking.LoadAsync(SelectedTarget.Key, SelectedPeriod.Key, Category.Key);
        }

        #endregion

        #region ISelected

        public bool IsSelected
        {
            get { return this._isSelected; }
            set { SetVariableValue(ref this._isSelected, value); }
        }

        #endregion
    }
}
