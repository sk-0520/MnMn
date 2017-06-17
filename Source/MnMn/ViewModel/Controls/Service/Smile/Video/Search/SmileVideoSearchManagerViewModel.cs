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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.HalfBakedApi;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

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
        LoadState _trendTagLoadState;
        SmileVideoSearchHistoryViewModel _selectedQueryHistory;

        SearchType _selectedSearchType = SearchType.Tag;

        #endregion

        public SmileVideoSearchManagerViewModel(Mediation mediation, SmileVideoSearchModel searchModel)
            : base(mediation)
        {
            SearchModel = searchModel;

            SelectedMethod = MethodItems.FirstOrDefault(m => m.Key == Setting.Search.DefaultMethodKey) ?? MethodItems.First();
            SelectedSort = MethodItems.FirstOrDefault(m => m.Key == Setting.Search.DefaultSortKey) ?? SortItems.First();
            SelectedSearchType = Setting.Search.DefaultType;

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

        public IList<DefinedElementModel> MethodItems => SearchModel.GetDefaultSearchTypeDefine().Methods;
        public IList<DefinedElementModel> SortItems => SearchModel.GetDefaultSearchTypeDefine().Sort;
        public IList<DefinedElementModel> TypeItems => SearchModel.GetDefaultSearchTypeDefine().Type;

        public CollectionModel<SmileVideoSearchGroupFinderViewModel> SearchGroups { get; } = new CollectionModel<SmileVideoSearchGroupFinderViewModel>();

        public DefinedElementModel SelectedMethod
        {
            get { return this._selectedMethod; }
            set
            {
                if(SetVariableValue(ref this._selectedMethod, value)) {
                    Setting.Search.DefaultMethodKey = SelectedMethod.Key;
                }
            }
        }
        public DefinedElementModel SelectedSort
        {
            get { return this._selectedSort; }
            set
            {
                if(SetVariableValue(ref this._selectedSort, value)) {
                    Setting.Search.DefaultSortKey = SelectedSort.Key;
                }
            }
        }

        public string InputQuery
        {
            get { return this._inputQuery; }
            set { SetVariableValue(ref this._inputQuery, value); }
        }

        public SmileVideoSearchGroupFinderViewModel SelectedSearchGroup
        {
            get { return this._selectedSearchGroup; }
            set
            {
                if(SetVariableValue(ref this._selectedSearchGroup, value)) {
                    //[Obsolete]
                    //if(SelectedSearchGroup != null) {
                    //    if(SelectedSearchGroup.IsPin && SelectedSearchGroup.FinderLoadState == SourceLoadState.None) {
                    //        SelectedSearchGroup.LoadAsync(Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan, true).ConfigureAwait(false);
                    //    }
                    //}
                }
            }
        }

        public IEnumerable<SearchType> SearchTypeItems
        {
            get
            {
                return new[] { SearchType.Tag, SearchType.Keyword };
            }
        }

        public SearchType SelectedSearchType
        {
            get { return this._selectedSearchType; }
            set
            {
                if(SetVariableValue(ref this._selectedSearchType, value)) {
                    Setting.Search.DefaultType = SelectedSearchType;
                }
            }
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
                        if(RecommendTagLoadState == LoadState.None && Session.IsLoggedIn) {
                            LoadRecommendTagItemsAsync().ConfigureAwait(false);
                        }
                        if(TrendTagLoadState == LoadState.None) {
                            LoadTrendTagAsync().ConfigureAwait(false);
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

        public LoadState TrendTagLoadState
        {
            get { return this._trendTagLoadState; }
            set { SetVariableValue(ref this._trendTagLoadState, value); }
        }

        public CollectionModel<SmileVideoTagViewModel> RecommendTagItems { get; } = new CollectionModel<SmileVideoTagViewModel>();
        public CollectionModel<SmileVideoTagViewModel> TrendTagItems { get; } = new CollectionModel<SmileVideoTagViewModel>();

        bool UsingIme { get; set; }

        public bool ShowSearchBookmark
        {
            get { return Setting.Search.ShowSearchBookmark; }
            set { SetPropertyValue(Setting.Search, value); }
        }

        public GridLength SearchBookmarkWidth
        {
            get { return new GridLength(Setting.Search.SearchBookmarkWidth, GridUnitType.Star); }
            set { SetPropertyValue(Setting.Search, value.Value); }
        }
        public GridLength SearchFinderWidth
        {
            get { return new GridLength(Setting.Search.SearchFinderWidth, GridUnitType.Star); }
            set { SetPropertyValue(Setting.Search, value.Value); }
        }

        #endregion

        #region command

        public ICommand SearchCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        SearchAsync().ConfigureAwait(false);
                        InputQuery = string.Empty;
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
                        InputQuery = string.Empty;
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

        public ICommand LoadTrendTagCommand
        {
            get { return CreateCommand(o => { LoadTrendTagAsync().ConfigureAwait(false); }); }
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

        public ICommand ShowTagAreaCommand { get { return CreateCommand(o => ShowTagArea = true); } }

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

        public ICommand RemoveHistoryCommand
        {
            get
            {
                return CreateCommand(o => RemoveHistory((SmileVideoSearchHistoryViewModel)o));
            }
        }

        public ICommand CloseTabCommand
        {
            get
            {
                return CreateCommand(o => CloseTab((SmileVideoSearchGroupFinderViewModel)o));
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

            return SearchCoreAsync(nowMethod, nowSort, SelectedSearchType, nowQuery, true);
        }

        public Task LoadSearchFromParameterAsync(SmileVideoSearchParameterModel parameter)
        {
            if(string.IsNullOrWhiteSpace(parameter.Query)) {
                return Task.CompletedTask;
            }

            var nowMethod = SelectedMethod;
            var nowSort = SelectedSort;

            return SearchCoreAsync(nowMethod, nowSort, parameter.SearchType, parameter.Query, true);
        }

        [Obsolete]
        public Task LoadSearchFromPinAsync(SmileVideoSearchPinModel pin)
        {
            if(string.IsNullOrWhiteSpace(pin.Query)) {
                return Task.CompletedTask;
            }

            var method = MethodItems.FirstOrDefault(i => i.Key == pin.MethodKey) ?? SelectedMethod;
            var sort = SortItems.FirstOrDefault(i => i.Key == pin.SortKey) ?? SelectedSort;

            return SearchCoreAsync(method, sort, pin.SearchType, pin.Query, false);
        }

        Task SearchCoreAsync(DefinedElementModel method, DefinedElementModel sort, SearchType type, string query, bool isLoad)
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
                    SearchGroups.Add(finder);
                    return finder;
                }
            );

            if(isLoad) {
                SelectedSearchGroup = selectViewModel;
            }

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
            var historyViewModel = historyPair.ViewModel;
            historyViewModel.Count = RangeUtility.Increment(historyViewModel.Count);
            historyViewModel.LastTimestamp = DateTime.Now;
            selectViewModel.History = historyViewModel;

            if(isLoad) {
                return selectViewModel.LoadAsync(Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan, true);
            } else {
                return Task.CompletedTask;
            }
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

        public Task LoadTrendTagAsync()
        {
            TrendTagLoadState = LoadState.Preparation;

            var rec = new Logic.Service.Smile.Video.Api.V1.Tag(Mediation);

            TrendTagLoadState = LoadState.Loading;
            return rec.LoadTrendTagListAsync().ContinueWith(task => {
                var rawTagList = task.Result;
                return rawTagList.Tags.Select(t => new SmileVideoTagViewModel(Mediation, t));
            }).ContinueWith(task => {
                var list = task.Result;
                TrendTagItems.InitializeRange(list);
                TrendTagLoadState = LoadState.Loaded;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void RemoveHistory(SmileVideoSearchHistoryViewModel o)
        {
            SearchHistoryList.Remove(o);
        }

        void CloseTab(SmileVideoSearchGroupFinderViewModel finder)
        {
            SearchGroups.Remove(finder);
        }

        /// <summary>
        /// 無効アイテムの破棄。
        /// </summary>
        void RemoveInvalidHistoryItems()
        {
            var removeItems = SearchHistoryList.ModelList
                .Where(m => m.TotalCount == 0)
                .ToEvaluatedSequence()
            ;

            if(removeItems.Any()) {
                Mediation.Logger.Information($"remove history: {removeItems.Count}", string.Join(Environment.NewLine, removeItems.Select(i => i.Query)));

                foreach(var item in removeItems) {
                    SearchHistoryList.Remove(item);
                }
            }
        }


        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            UsingIme = InputMethod.Current.ImeState == InputMethodState.On;
        }

        private void OnPreviewTextInputStart(object sender, TextCompositionEventArgs e)
        {
            UsingIme = InputMethod.Current.ImeState == InputMethodState.On;
        }

        private void OnPreviewTextInputUpdate(object sender, TextCompositionEventArgs e)
        {
            if(e.TextComposition.CompositionText.Length == 0) {
                UsingIme = false;
            }
        }

        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        { }

        protected override void HideViewCore()
        { }

        public override Task InitializeAsync()
        {
            RemoveInvalidHistoryItems();

            //[Obsolete]
            //if(!Setting.Search.SearchPinItems.Any()) {
            //    return Task.CompletedTask;
            //}
            //
            //var tasks = Setting.Search.SearchPinItems.Select(p => LoadSearchFromPinAsync(p));
            //
            //return Task.WhenAll(tasks).ContinueWith(t => {
            //    SelectedSearchGroup = SearchGroups.First();
            //}, TaskScheduler.FromCurrentSynchronizationContext());
            return Task.CompletedTask;
        }

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
            if(Constants.ComboBoxInputIme549Enabled) {
                view.smile.search.listSearch.AddHandler(
                    TextBoxBase.TextChangedEvent,
                    new TextChangedEventHandler(listSearch_TextChanged)
                );

                TextCompositionManager.AddPreviewTextInputHandler(view.smile.search.listSearch, OnPreviewTextInput);
                TextCompositionManager.AddPreviewTextInputStartHandler(view.smile.search.listSearch, OnPreviewTextInputStart);
                TextCompositionManager.AddPreviewTextInputUpdateHandler(view.smile.search.listSearch, OnPreviewTextInputUpdate);
            }
        }

        public override void UninitializeView(MainWindow view)
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        #endregion

        private void listSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(UsingIme) {
                return;
            }
        }

    }
}
