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
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
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
        CollectionModel<int> _indexItems;

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

        public CollectionModel<SmileVideoSearchItemViewModel> SearchItems { get; } = new CollectionModel<SmileVideoSearchItemViewModel>();

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

        public CollectionModel<int> IndexItems
        {
            get { return this._indexItems; }
            set { SetVariableValue(ref this._indexItems, value); }
        }

        #endregion

        #region command

        public ICommand PageChangeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var pageNumber = (int)o;
                        var index = pageNumber - 1;
                        var loadIndex = index * Setting.SearchCount;
                        if(SearchItems[index] == null) {
                            var vm = new SmileVideoSearchItemViewModel(Mediation, SearchModel, LoadingMethod, LoadingSort, Type, Query, loadIndex, Setting.SearchCount);
                            vm.LoadAsync().ContinueWith(task => {
                                SearchItems[index] = vm;
                                SelectedVideoInformationItems = vm.VideoInformationItems;
                            });
                        } else {
                            SelectedVideoInformationItems = SearchItems[index].VideoInformationItems;
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

        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, bool reload)
        {
            SmileVideoElementModel nowMethod;
            SmileVideoElementModel nowSort;
            if(reload) {
                nowMethod = SelectedMethod;
                nowSort = SelectedSort;
            } else {
                nowMethod = LoadingMethod;
                nowSort = LoadingSort;
            }

            var vm = new SmileVideoSearchItemViewModel(Mediation, SearchModel, nowMethod, nowSort, Type, Query, 0, Setting.SearchCount);
            return vm.LoadAsync().ContinueWith(task => {
                if(reload) {
                    TotalCount = vm.TotalCount;
                    if(TotalCount > 0) {
                        var splitCount = Math.Min(TotalCount / Setting.SearchCount, (SearchModel.MaximumIndex + SearchModel.MaximumCount) / Setting.SearchCount);
                        var preList = new SmileVideoSearchItemViewModel[Enumerable.Range(0, splitCount).Count()];
                        SearchItems.InitializeRange(preList);

                        IndexItems = new CollectionModel<int>(preList.Select((_,i) => i + 1));
                    }
                }
                var index = vm.Index / Setting.SearchCount;
                SearchItems[index] = vm;
                SelectedVideoInformationItems = vm.VideoInformationItems;
            });
        }

        #endregion
    }
}
