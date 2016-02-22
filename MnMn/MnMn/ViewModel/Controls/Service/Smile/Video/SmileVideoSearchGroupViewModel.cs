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
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    /// <summary>
    /// <para>配列的な操作はこのクラス内で完結させたい思い</para>
    /// </summary>
    public class SmileVideoSearchGroupViewModel: SmileVideoFinderViewModelBase
    {
        #region variable

        SmileVideoElementModel _selectedMethod;
        SmileVideoElementModel _selectedSort;

        //ICollectionView _selectedVideoInformationItems;

        int _totalCount;
        SmileSearchPageViewModel<SmileVideoSearchItemViewModel> _selectedPage;

        bool _notfound;

        #endregion

        public SmileVideoSearchGroupViewModel(Mediation mediation, SmileVideoSearchModel searchModel, SmileVideoElementModel method, SmileVideoElementModel sort, SmileVideoElementModel type, string query)
            : base(mediation)
        {
            SearchModel = searchModel;
            Query = query;
            Type = type;

            SetContextElements(method, sort);
        }

        #region property

        SmileVideoSearchModel SearchModel { get; }

        public IList<SmileVideoElementModel> MethodItems => SearchModel.Methods;
        public IList<SmileVideoElementModel> SortItems => SearchModel.Sort;

        public string Query { get; }
        public SmileVideoElementModel Type { get; }

        public SmileVideoElementModel LoadingMethod { get; private set; }
        public SmileVideoElementModel LoadingSort { get; private set; }

        public SmileVideoElementModel SelectedMethod
        {
            get { return this._selectedMethod; }
            set { SetVariableValue(ref this._selectedMethod, value); }
        }
        public SmileVideoElementModel SelectedSort
        {
            get { return this._selectedSort; }
            set { SetVariableValue(ref this._selectedSort, value); }
        }

        public CollectionModel<SmileSearchPageViewModel<SmileVideoSearchItemViewModel>> SearchItems { get; } = new CollectionModel<SmileSearchPageViewModel<SmileVideoSearchItemViewModel>>();

        public SmileSearchPageViewModel<SmileVideoSearchItemViewModel> SelectedPage
        {
            get { return this._selectedPage; }
            set {
                var oldSelectedPage = this._selectedPage;
                if(SetVariableValue(ref this._selectedPage, value)) {
                    if(oldSelectedPage!= null) {
                        oldSelectedPage.ViewModel.PropertyChanged -= PageVm_PropertyChanged;
                    }
                    CallPageItemOnPropertyChange();
                }
            }
        }

        public override ICollectionView VideoInformationItems
        {
            get
            {
                if(SelectedPage == null) {
                    //return null;
                    return base.VideoInformationItems;
                }
                return SelectedPage.ViewModel.VideoInformationItems;
        }
    }

        //public ICollectionView SelectedVideoInformationItems
        //{
        //    get {
        //        if(SelectedPage == null) {
        //            return null;
        //        }
        //        return SelectedPage.ViewModel.VideoInformationItems; }
        //    //set { SetVariableValue(ref this._selectedVideoInformationItems, value); }
        //}

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

        public override SmileVideoFinderLoadState FinderLoadState
        {
            get
            {
                if(SelectedPage == null) {
                    return SmileVideoFinderLoadState.None;
                }
                return SelectedPage.ViewModel.FinderLoadState;
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region command

        public ICommand PageChangeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var pageVm = (SmileSearchPageViewModel<SmileVideoSearchItemViewModel>)o;
                        if(pageVm.LoadState != LoadState.Loaded) {
                            var thumbCacheSpan = Constants.ServiceSmileVideoThumbCacheSpan;
                            var imageCacheSpan = Constants.ServiceSmileVideoImageCacheSpan;

                            SelectedPage = pageVm;
                            pageVm.ViewModel.PropertyChanged += PageVm_PropertyChanged;
                            pageVm.ViewModel.LoadAsync(thumbCacheSpan, imageCacheSpan).ConfigureAwait(true);
                        } else {
                            SelectedPage = pageVm;
                        }
                    }
                );
            }
        }

        public ICommand ReloadSearchCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LoadAsync(Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan, true).ConfigureAwait(true);
                    }
                );
            }
        }

        static IEnumerable<string> ChangePagePropertyNames => new[] {
            nameof(VideoInformationItems),
            nameof(FinderLoadState),
            nameof(CanLoad),
            nameof(NowLoading),
        };


        #endregion

        #region function

        void CallPageItemOnPropertyChange()
        {
            CallOnPropertyChange(ChangePagePropertyNames);
        }

        SmileVideoElementModel GetContextElemetFromChangeElement(IEnumerable<SmileVideoElementModel> items, SmileVideoElementModel element)
        {
            if(items.Any(i => i == element)) {
                return element;
            } else {
                return items.FirstOrDefault(i => i.Key == element.Key);
            }
        }

        public void SetContextElements(SmileVideoElementModel method, SmileVideoElementModel sort)
        {
            LoadingMethod = SelectedMethod = GetContextElemetFromChangeElement(MethodItems, method);
            LoadingSort = SelectedSort = GetContextElemetFromChangeElement(SortItems, sort);
        }

        protected override Task LoadAsync_Impl(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            var isReload =(bool)extends;

            SmileVideoElementModel nowMethod;
            SmileVideoElementModel nowSort;
            if(isReload) {
                nowMethod = SelectedMethod;
                nowSort = SelectedSort;
            } else {
                nowMethod = LoadingMethod;
                nowSort = LoadingSort;
            }

            var vm = new SmileVideoSearchItemViewModel(Mediation, SearchModel, nowMethod, nowSort, Type, Query, 0, Setting.SearchCount);
            vm.PropertyChanged += PageVm_PropertyChanged;
            return vm.LoadAsync(thumbCacheSpan, imageCacheSpan).ContinueWith(task => {
                if(isReload) {
                    TotalCount = vm.TotalCount;
                    if(TotalCount > Setting.SearchCount) {
                        var pageCount = Math.Min(TotalCount / Setting.SearchCount, (SearchModel.MaximumIndex + SearchModel.MaximumCount) / Setting.SearchCount);
                        var preList = Enumerable.Range(1, pageCount - 1)
                            .Select((n, i) => new SmileVideoSearchItemViewModel(Mediation, SearchModel, nowMethod, nowSort, Type, Query, (i + 1) * Setting.SearchCount, Setting.SearchCount))
                            .Select((v, i) => new SmileSearchPageViewModel<SmileVideoSearchItemViewModel>(v, i + 2))
                            .ToList()
                        ;
                        var pageVm = new SmileSearchPageViewModel<SmileVideoSearchItemViewModel>(vm, 1) {
                            LoadState = LoadState.Loaded,
                        };
                        preList.Insert(0, pageVm);
                        SearchItems.InitializeRange(preList);
                    } else if(TotalCount > 0) {
                        var pageVm = new SmileSearchPageViewModel<SmileVideoSearchItemViewModel>(vm, 1) {
                            LoadState = LoadState.Loaded,
                        };
                        SearchItems.InitializeRange(new[] { pageVm });
                    }
                }
                NotFound = !SearchItems.Any();
                if(!NotFound) {
                    SelectedPage = SearchItems.First();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, bool isReload)
        {
            return LoadAsync_Impl(thumbCacheSpan, imageCacheSpan, isReload);
        }

        public new Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            throw new NotSupportedException();
        }


        #endregion

        #region SmileVideoFinderViewModelBase

        public override bool CanLoad
        {
            get
            {
                if(SelectedPage == null) {
                    return false;
                }

                return SelectedPage.ViewModel.CanLoad;
            }
        }

        protected override PageLoader CreatePageLoader()
        {
            throw new NotSupportedException();
        }

        #endregion

        private void PageVm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(FinderLoadState)) {
                CallPageItemOnPropertyChange();
            }
        }

    }
}
