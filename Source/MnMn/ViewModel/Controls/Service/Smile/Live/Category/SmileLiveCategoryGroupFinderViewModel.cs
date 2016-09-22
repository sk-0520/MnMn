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
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Live;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live.Category
{
    public class SmileLiveCategoryGroupFinderViewModel: SmileLiveFinderViewModelBase, IPagerFinder<SmileLiveCategoryItemFinderViewModel, SmileLiveInformationViewModel, SmileLiveFinderItemViewModel>
    {
        #region variable

        DefinedElementModel _selectedSort;
        DefinedElementModel _selectedOrder;

        #endregion

        public SmileLiveCategoryGroupFinderViewModel(Mediation mediation, SmileLiveCategoryModel categoryDefine, DefinedElementModel sort, DefinedElementModel order, DefinedElementModel category)
            : base(mediation)
        {
            CategoryModel = categoryDefine;

            Category = category;
            SetContextElements(sort, order);

            PagerFinderProvider = new PagerFinderProvider<SmileLiveFinderViewModelBase, SmileLiveCategoryItemFinderViewModel, SmileLiveInformationViewModel, SmileLiveFinderItemViewModel>(
                Mediation,
                this
            );
        }

        #region property

        SmileLiveCategoryModel CategoryModel { get; }

        PagerFinderProvider<SmileLiveFinderViewModelBase, SmileLiveCategoryItemFinderViewModel, SmileLiveInformationViewModel, SmileLiveFinderItemViewModel> PagerFinderProvider { get; }

        public DefinedElementModel LoadingSort { get; private set; }
        public DefinedElementModel LoadingOrder { get; private set; }
        public DefinedElementModel Category { get; set; }

        public DefinedElementModel SelectedSort
        {
            get { return this._selectedSort; }
            set { SetVariableValue(ref this._selectedSort, value); }
        }

        public DefinedElementModel SelectedOrder
        {
            get { return this._selectedOrder; }
            set { SetVariableValue(ref this._selectedOrder, value); }
        }

        public IList<DefinedElementModel> SortItems { get { return CategoryModel.SortItems; } }
        public IList<DefinedElementModel> OrderItems { get { return CategoryModel.OrderItems; } }

        SmileLiveCategoryItemFinderViewModel SearchFinder { get; set; }

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

        public void SetContextElements(DefinedElementModel sort, DefinedElementModel order)
        {
            SelectedPage = null;

            LoadingSort = SelectedSort = GetContextElemetFromChangeElement(SortItems, sort);
            LoadingOrder = SelectedOrder = GetContextElemetFromChangeElement(OrderItems, order);
        }


        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, bool isReload)
        {
            return LoadCoreAsync(thumbCacheSpan, imageCacheSpan, isReload);
        }

        #endregion

        #region IPagerFinder

        public CollectionModel<PageViewModel<SmileLiveCategoryItemFinderViewModel>> PageItems
        {
            get { return PagerFinderProvider.PageItems; }
        }

        public PageViewModel<SmileLiveCategoryItemFinderViewModel> SelectedPage
        {
            get { return PagerFinderProvider?.SelectedPage; }
            set
            {
                if(PagerFinderProvider != null) {
                    PagerFinderProvider.SelectedPage = value;
                }
            }
        }

        public void CallPageItemOnPropertyChange()
        {
            CallOnPropertyChange(PagerFinderProvider.ChangePagePropertyNames);
        }

        public ICommand PageChangeCommand
        {
            get { return PagerFinderProvider.PageChangeCommand; }
        }

        #endregion

        #region SmileLiveFinderViewModelBase

        protected override Task LoadCoreAsync(CacheSpan informationCacheSpan, CacheSpan imageCacheSpan, object extends)
        {

            return base.LoadCoreAsync(informationCacheSpan, imageCacheSpan, extends);
        }

        #endregion

    }
}
