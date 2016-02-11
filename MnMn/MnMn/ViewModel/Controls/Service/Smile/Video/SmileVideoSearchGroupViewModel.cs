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

        ICollectionView _selectedVideoInformationItems;

        int _totalCount;
        //CollectionModel<int> _indexItems;

        #endregion

        public SmileVideoSearchGroupViewModel(Mediation mediation, SmileVideoSearchModel searchModel, SmileVideoSettingModel setting, SmileVideoElementModel method, SmileVideoElementModel sort, SmileVideoElementModel type, string query)
            : base(mediation)
        {
            SearchModel = searchModel;
            Query = query;
            Type = type;
            Setting = setting;

            SetContextElements(method, sort);
        }

        #region property

        SmileVideoSearchModel SearchModel { get; }
        SmileVideoSettingModel Setting { get; }

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

        public ICollectionView SelectedVideoInformationItems
        {
            get { return this._selectedVideoInformationItems; }
            set { SetVariableValue(ref this._selectedVideoInformationItems, value); }
        }

        public int TotalCount
        {
            get { return this._totalCount; }
            set { SetVariableValue(ref this._totalCount, value); }
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

                            pageVm.ViewModel.LoadAsync(thumbCacheSpan, imageCacheSpan).ContinueWith(task => {
                                SelectedVideoInformationItems = pageVm.ViewModel.VideoInformationItems;
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                        } else {
                            SelectedVideoInformationItems = pageVm.ViewModel.VideoInformationItems;
                        }
                    }
                );
            }
        }

        #endregion

        #region function

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

        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, bool isReload)
        {
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
                SelectedVideoInformationItems = vm.VideoInformationItems;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}
