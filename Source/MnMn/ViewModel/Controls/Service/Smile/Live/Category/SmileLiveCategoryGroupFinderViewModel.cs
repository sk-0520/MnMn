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
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
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

        int _totalCount;
        bool _notfound;

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
            PagerFinderProvider.ChangedSelectedPage += PagerFinderProvider_ChangedSelectedPage;
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

        public int TotalCount
        {
            get { return this._totalCount; }
            set { SetVariableValue(ref this._totalCount, value); }
        }

        public bool NotFound
        {
            get { return this._notfound; }
            set { SetVariableValue(ref this._notfound, value); }
        }

        #endregion

        #region command

        public override ICommand ReloadCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LoadAsync(Constants.ServiceSmileLiveInformationCacheSpan, Constants.ServiceSmileLiveImageCacheSpan, true).ConfigureAwait(true);
                    }
                );
            }
        }

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
            var isReload = (bool)extends;

            DefinedElementModel nowSort;
            DefinedElementModel nowOrder;
            if(isReload) {
                nowSort = SelectedSort;
                nowOrder = SelectedOrder;
                SelectedPage = null;
            } else {
                nowSort = LoadingSort;
                nowOrder = LoadingOrder;
            }

            SearchFinder = new SmileLiveCategoryItemFinderViewModel(Mediation, CategoryModel, nowSort, nowOrder, Category, 0);
            //SearchFinder.PropertyChanged += PageVm_PropertyChanged;
            //PagerFinderProvider.AttachmentChildProprtyChange(SearchFinder);

            if(isReload) {
                SearchFinder.PropertyChanged += SearchFinder_PropertyChanged_TotalCount;
            }

            return SearchFinder.LoadAsync(informationCacheSpan, imageCacheSpan);
        }

        protected override void Dispose(bool disposing)
        {
            PagerFinderProvider.ChangedSelectedPage -= PagerFinderProvider_ChangedSelectedPage;

            base.Dispose(disposing);
        }

        #endregion

        void SearchFinder_PropertyChanged_TotalCount(object sender, PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
            var searchFinder = (SmileLiveCategoryItemFinderViewModel)sender;
            if(e.PropertyName == nameof(searchFinder.TotalCount)) {
                searchFinder.PropertyChanged -= SearchFinder_PropertyChanged_TotalCount;

                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    TotalCount = searchFinder.TotalCount;

                    var usingList = Enumerable.Empty<PageViewModel<SmileLiveCategoryItemFinderViewModel>>();

                    if(TotalCount > CategoryModel.MaxCount) {
                        var pageCount = TotalCount / CategoryModel.MaxCount;
                        //var correctionPage = TotalCount > (SearchModel.MaximumIndex + SearchModel.MaximumCount) ? 1 : 0;
                        var preList = Enumerable.Range(1, pageCount - 1)
                            .Select((n, i) => new SmileLiveCategoryItemFinderViewModel(Mediation, CategoryModel, searchFinder.Sort, searchFinder.Order, searchFinder.Category, i + 1))
                            .Select((v, i) => new PageViewModel<SmileLiveCategoryItemFinderViewModel>(v, i + 2))
                            .ToList()
                        ;
                        var pageVm = new PageViewModel<SmileLiveCategoryItemFinderViewModel>(searchFinder, 1) {
                            LoadState = LoadState.Loaded,
                        };
                        preList.Insert(0, pageVm);
                        usingList = preList;
                    } else if(TotalCount > 0) {
                        var pageVm = new PageViewModel<SmileLiveCategoryItemFinderViewModel>(searchFinder, 1) {
                            LoadState = LoadState.Loaded,
                        };
                        usingList = new[] { pageVm };
                    }

                    PageItems.InitializeRange(usingList);
                    NotFound = !PageItems.Any();
                    if(!NotFound) {
                        SelectedPage = PageItems.First();
                    }
                }));
            }
        }

        void PagerFinderProvider_ChangedSelectedPage(object sender, Define.Event.ChangedSelectedPageEventArgs<SmileLiveCategoryItemFinderViewModel, SmileLiveInformationViewModel, SmileLiveFinderItemViewModel> e)
        {
            if(e.OldSelectedPage != null) {
                e.OldSelectedPage.ViewModel.PropertyChanged -= SearchFinder_PropertyChanged_TotalCount;
            }
            if(e.NewSelectedPage != null && e.OldSelectedPage != null) {
                //e.NewSelectedPage.ViewModel.SelectedSortType = e.OldSelectedPage.ViewModel.SelectedSortType;
            }
        }

    }
}
