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
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.HalfBakedApi;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search
{
    public class SmileVideoSearchManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        #region variable

        DefinedElementModel _selectedMethod;
        DefinedElementModel _selectedSort;
        //DefinedElementModel _selectedType;

        string _inputQuery;

        SmileVideoSearchGroupFinderViewModel _selectedSearchGroup;

        bool _showSearchTypeArea;
        bool _showHistoryArea;
        bool _showTagArea;
        LoadState _recommendTagLoadState;
        SmileVideoSearchHistoryViewModel _selectedQueryHistory;

        SearchType _selectedSearchType = SearchType.Tag;

        #endregion

        public SmileVideoSearchManagerViewModel(Mediation mediation, SmileVideoSearchModel searchModel)
            : base(mediation)
        {
            SearchModel = searchModel;

            SelectedMethod = MethodItems.First();
            SelectedSort = SortItems.First();
            //SelectedType = TypeItems.First();

            SearchHistoryList = new MVMPairCreateDelegationCollection<SmileVideoSearchHistoryModel, SmileVideoSearchHistoryViewModel>(Setting.Search.SearchHistoryItems, default(object), CreateSearchHistory);
            SearchHistoryItems = CollectionViewSource.GetDefaultView(SearchHistoryList.ViewModelList);
        }


        #region property

        SmileVideoSearchModel SearchModel { get; }

        MVMPairCreateDelegationCollection<SmileVideoSearchHistoryModel, SmileVideoSearchHistoryViewModel> SearchHistoryList { get; }
        public ICollectionView SearchHistoryItems { get; }
        public SmileVideoSearchHistoryViewModel SelectedQueryHistory
        {
            get { return this._selectedQueryHistory; }
            set
            {
                SetVariableValue(ref this._selectedQueryHistory, value);
            }
        }

        public IList<DefinedElementModel> MethodItems => SearchModel.Methods;
        public IList<DefinedElementModel> SortItems => SearchModel.Sort;
        public IList<DefinedElementModel> TypeItems => SearchModel.Type;

        public CollectionModel<SmileVideoSearchGroupFinderViewModel> SearchGroups { get; } = new CollectionModel<SmileVideoSearchGroupFinderViewModel>();

        public DefinedElementModel SelectedMethod
        {
            get { return this._selectedMethod; }
            set { SetVariableValue(ref this._selectedMethod, value); }
        }
        public DefinedElementModel SelectedSort
        {
            get { return this._selectedSort; }
            set { SetVariableValue(ref this._selectedSort, value); }
        }
        //public DefinedElementModel SelectedType
        //{
        //    get { return this._selectedType; }
        //    set { SetVariableValue(ref this._selectedType, value); }
        //}

        public string InputQuery
        {
            get { return this._inputQuery; }
            set { SetVariableValue(ref this._inputQuery, value); }
        }

        public SmileVideoSearchGroupFinderViewModel SelectedSearchGroup
        {
            get { return this._selectedSearchGroup; }
            set {
                if(SetVariableValue(ref this._selectedSearchGroup, value)) {
                    if(this._selectedSearchGroup != null) {
                        //var items = this._selectedSearchGroup.SearchItems;
                        //this._selectedSearchGroup.SearchItems.InitializeRange(items);
                    }
                }
            }
        }

        public SearchType SelectedSearchType
        {
            get { return this._selectedSearchType; }
            set { SetVariableValue(ref this._selectedSearchType, value); }
        }

        public bool ShowSearchTypeArea
        {
            get { return this._showSearchTypeArea; }
            set { SetVariableValue(ref this._showSearchTypeArea, value); }
        }
        public bool ShowHistoryArea
        {
            get { return this._showHistoryArea; }
            set { SetVariableValue(ref this._showHistoryArea, value); }
        }
        public bool ShowTagArea
        {
            get { return this._showTagArea; }
            set
            {
                if(SetVariableValue(ref this._showTagArea, value)) {
                    if(this._showTagArea) {
                        if(RecommendTagLoadState == LoadState.None) {
                            LoadRecommendTagItemsAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        public LoadState RecommendTagLoadState
        {
            get { return this._recommendTagLoadState; }
            set { SetVariableValue(ref this._recommendTagLoadState, value); }
        }

        public CollectionModel<SmileVideoTagViewModel> RecommendTagItems { get; } = new CollectionModel<SmileVideoTagViewModel>();

        #endregion

        #region command

        public ICommand SearchCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        SearchAsync().ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand SearchFromTypeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        SelectedSearchType = (SearchType)Enum.Parse(typeof(SearchType), (string)o);
                        ShowSearchTypeArea = false;
                        SearchAsync().ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand LoadRecommendTagCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LoadRecommendTagItemsAsync().ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand SearchTagCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(ShowTagArea) {
                            ShowTagArea = false;
                        }

                        var tagViewModel = (SmileVideoTagViewModel)o;
                        var parameter = new SmileVideoSearchParameterModel() {
                            SearchType = SearchType.Tag,
                            Query = tagViewModel.TagName,
                        };

                        LoadSearchFromParameterAsync(parameter).ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand SearchHistoryFromHistoryCommand
        {
            get
            {
                return CreateCommand(o => {
                    if(ShowHistoryArea) {
                        ShowHistoryArea = false;
                    }
                    var viewModel = (SmileVideoSearchHistoryViewModel)o;
                    var parameter = new SmileVideoSearchParameterModel() {
                        SearchType = viewModel.SearchType,
                        Query = viewModel.Query,
                    };

                    LoadSearchFromParameterAsync(parameter).ConfigureAwait(false);
                });
            }
        }

        public ICommand SearchHistoryFromTagCommand
        {
            get
            {
                return CreateCommand(o => {
                    if(ShowHistoryArea) {
                        ShowHistoryArea = false;
                    }
                    var viewModel = (SmileVideoSearchHistoryViewModel)o;
                    var parameter = new SmileVideoSearchParameterModel() {
                        SearchType = SearchType.Tag,
                        Query = viewModel.Query,
                    };

                    LoadSearchFromParameterAsync(parameter).ConfigureAwait(false);
                });
            }
        }

        public ICommand SearchHistoryFromKeywordCommand
        {
            get
            {
                return CreateCommand(o => {
                    if(ShowHistoryArea) {
                        ShowHistoryArea = false;
                    }
                    var viewModel = (SmileVideoSearchHistoryViewModel)o;
                    var parameter = new SmileVideoSearchParameterModel() {
                        SearchType = SearchType.Keyword,
                        Query = viewModel.Query,
                    };

                    LoadSearchFromParameterAsync(parameter).ConfigureAwait(false);
                });
            }
        }

        #endregion

        #region function

        static SmileVideoSearchHistoryViewModel CreateSearchHistory(SmileVideoSearchHistoryModel model, object data)
        {
            return new SmileVideoSearchHistoryViewModel(model);
        }

        Task SearchSimpleAsync(string query, SearchType searchType)
        {
            var parameter = new SmileVideoSearchParameterModel() {
                SearchType = searchType,
                Query = query,
            };

            return LoadSearchFromParameterAsync(parameter);
        }

        public Task SearchAsync()
        {
            if(string.IsNullOrWhiteSpace(InputQuery)) {
                return Task.CompletedTask;
            }

            var nowMethod = SelectedMethod;
            var nowSort = SelectedSort;
            //var nowType = SelectedType;
            var nowQuery = InputQuery;

            return SearchCoreAsync(nowMethod, nowSort, SelectedSearchType, nowQuery);
        }

        public Task LoadSearchFromParameterAsync(SmileVideoSearchParameterModel parameter)
        {
            if(string.IsNullOrWhiteSpace(parameter.Query)) {
                return Task.CompletedTask;
            }

            var nowMethod = SelectedMethod;
            var nowSort = SelectedSort;

            return SearchCoreAsync(nowMethod, nowSort, parameter.SearchType, parameter.Query);
        }

        Task SearchCoreAsync(DefinedElementModel method, DefinedElementModel sort, SearchType type, string query)
        {
            CheckUtility.EnforceNotNullAndNotWhiteSpace(query);

            // 存在する場合は該当タブへ遷移
            var selectViewModel = RestrictUtility.IsNotNull(
                SearchGroups.FirstOrDefault(i => i.Query == query && i.Type == type),
                viewModel => {
                    viewModel.SetContextElements(method, sort);
                    return viewModel;
                },
                () => {
                    var finder = new SmileVideoSearchGroupFinderViewModel(Mediation, SearchModel, method, sort, type, query);
                    SearchGroups.Insert(0, finder);
                    return finder;
                }
            );
            SelectedSearchGroup = selectViewModel;
            return selectViewModel.LoadAsync(Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan, true).ContinueWith(t => {
                var history = new SmileVideoSearchHistoryModel() {
                    Query = query,
                    SearchType = type,
                };
                var exsitHistory = SearchHistoryList.ModelList.FirstOrDefault(m => m.SearchType == history.SearchType && m.Query == history.Query);
                if(exsitHistory != null) {
                    history = exsitHistory;
                    SearchHistoryList.Remove(history);
                }
                var historyPair = SearchHistoryList.Insert(0, history, null);
                historyPair.ViewModel.Count += 1;
                historyPair.ViewModel.LastTimestamp = DateTime.Now;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public Task LoadRecommendTagItemsAsync()
        {
            RecommendTagLoadState = LoadState.Preparation;

            var rec = new Recommendations(Mediation);

            RecommendTagLoadState = LoadState.Loading;
            return rec.LoadTagListAsync().ContinueWith(task => {
                var rawTagList = task.Result;
                return rawTagList.Tags.Select(t => new SmileVideoTagViewModel(Mediation, t));
            }).ContinueWith(task => {
                var list = task.Result;
                RecommendTagItems.InitializeRange(list);
                RecommendTagLoadState = LoadState.Loaded;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        #region ManagerViewModelBase

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
