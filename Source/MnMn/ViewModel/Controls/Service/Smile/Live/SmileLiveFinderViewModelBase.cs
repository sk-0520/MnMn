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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Live;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live
{
    public abstract class SmileLiveFinderViewModelBase: TFinderViewModelBase<SmileLiveInformationViewModel, SmileLiveFinderItemViewModel>
    {
        #region variable

        SmileLiveSortType _selectedSortType;

        #endregion

        public SmileLiveFinderViewModelBase(Mediator mediator, int baseNumber)
            : base(mediator, baseNumber)
        {
            Setting = Mediation.GetResultFromRequest<SmileLiveSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.SmileLive));

            SortTypeItems = new CollectionModel<SmileLiveSortType>(EnumUtility.GetMembers<SmileLiveSortType>());

            FinderItems.Filter = FilterItems;
        }

        #region property

        protected SmileLiveSettingModel Setting { get; }

        public CollectionModel<SmileLiveSortType> SortTypeItems { get; }

        /// <summary>
        /// ソート方法。
        /// </summary>
        public virtual SmileLiveSortType SelectedSortType
        {
            get { return this._selectedSortType; }
            set
            {
                if(SetVariableValue(ref this._selectedSortType, value)) {
                    ChangeSortItems();
                }
            }
        }

        #endregion

        #region command
        #endregion

        #region function

        protected virtual bool FilterItems(object obj)
        {
            var filter = InputTitleFilter;
            if(string.IsNullOrEmpty(filter)) {
                return true;
            }

            var finderItem = (SmileLiveFinderItemViewModel)obj;
            var viewModel = finderItem.Information;
            var isHit = viewModel.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1;
            if(IsBlacklist) {
                return !isHit;
            } else {
                return isHit;
            }
        }

        #endregion

        #region FinderViewModelBase

        public override CacheSpan DefaultInformationCacheSpan => Constants.ServiceSmileLiveInformationCacheSpan;
        public override CacheSpan DefaultImageCacheSpan => Constants.ServiceSmileLiveImageCacheSpan;
        public override object DefaultExtends { get; } = null;

        protected override Task LoadInformationAsync(IEnumerable<SmileLiveInformationViewModel> informationItems, CacheSpan informationCacheSpan)
        {
            var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            var loader = new SmileLiveInformationLoader(informationItems, session);
            return loader.LoadInformationAsync(informationCacheSpan);
        }

        protected override Task LoadImageAsync(IEnumerable<SmileLiveInformationViewModel> informationItems, CacheSpan imageCacheSpan)
        {
            var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            var loader = new SmileLiveInformationLoader(informationItems, session);
            return loader.LoadThumbnaiImageAsync(imageCacheSpan);
        }

        protected override SmileLiveFinderItemViewModel CreateFinderItem(SmileLiveInformationViewModel information, int number)
        {
            return new SmileLiveFinderItemViewModel(information, number);
        }

        internal override void ChangeSortItems()
        {
            var map = new[] {
                new { Type = SmileLiveSortType.Number, Parent = string.Empty, Property = nameof(SmileLiveFinderItemViewModel.Number) },
            }.ToDictionary(
                ak => ak.Type,
                av => $"{av.Parent}.{av.Property}"
            );
            var propertyName = map[SelectedSortType];
            var direction = IsAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            var sort = new SortDescription(propertyName, direction);

            FinderItems.SortDescriptions.Clear();
            FinderItems.SortDescriptions.Add(sort);
            FinderItems.Refresh();
        }

        protected override Task CheckedProcessAsync(CheckedProcessType checkedProcessType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
