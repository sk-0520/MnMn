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
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search
{
    /// <summary>
    /// <para>配列的な操作はこのクラス内で完結させたい思い</para>
    /// </summary>
    public class SmileVideoSearchGroupFinderViewModel: SmileVideoFinderViewModelBase
    {
        #region define

        static IEnumerable<string> ChangePagePropertyNames => new[] {
            nameof(FinderItemsViewer),
            nameof(FinderItems),
            nameof(FinderLoadState),
            nameof(CanLoad),
            nameof(NowLoading),
            nameof(PageItems),
            nameof(PageChangeCommand),
            nameof(IsAscending),
            nameof(SelectedSortType),
            nameof(IsEnabledFinderFiltering),
            nameof(ShowFilterSetting),
        };

        #endregion

        #region variable

        DefinedElementModel _selectedMethod;
        DefinedElementModel _selectedSort;

        int _totalCount;
        PageViewModel<SmileVideoSearchItemFinderViewModel> _selectedPage;

        bool _notfound;

        #endregion

        public SmileVideoSearchGroupFinderViewModel(Mediation mediation, SmileVideoSearchModel searchModel, DefinedElementModel method, DefinedElementModel sort, SearchType type, string query)
            : base(mediation, 0)
        {
            SearchModel = searchModel;
            Query = query;
            Type = type;

            SetContextElements(method, sort);

            PagerPropertyChangedListener = new PropertyChangedWeakEventListener(PageVm_PropertyChanged);
            SearchPropertyChangedListener = new PropertyChangedWeakEventListener(SearchFinder_PropertyChanged_TotalCount);
        }

        #region property

        SmileVideoSearchModel SearchModel { get; }

        SmileVideoSearchItemFinderViewModel SearchFinder { get; set; }

        public IList<DefinedElementModel> MethodItems => SearchModel.GetDefaultSearchTypeDefine().Methods;
        public IList<DefinedElementModel> SortItems => SearchModel.GetDefaultSearchTypeDefine().Sort;

        public string Query { get; }
        public SearchType Type { get; }

        PropertyChangedWeakEventListener PagerPropertyChangedListener { get; }
        PropertyChangedWeakEventListener SearchPropertyChangedListener { get; }

        public DefinedElementModel LoadingMethod { get; private set; }
        public DefinedElementModel LoadingSort { get; private set; }

        public DefinedElementModel SelectedMethod
        {
            get { return this._selectedMethod; }
            set
            {
                if(SetVariableValue(ref this._selectedMethod, value)) {
                    if(SelectedMethod != null && SelectedPage != null) {
                        ReloadCommand.TryExecute(null);
                    }
                }
            }
        }
        public DefinedElementModel SelectedSort
        {
            get { return this._selectedSort; }
            set
            {
                if(SetVariableValue(ref this._selectedSort, value)) {
                    if(SelectedSort != null && SelectedPage != null) {
                        ReloadCommand.TryExecute(null);
                    }
                }
            }
        }

        public CollectionModel<PageViewModel<SmileVideoSearchItemFinderViewModel>> PageItems { get; set; } = new CollectionModel<PageViewModel<SmileVideoSearchItemFinderViewModel>>();

        public CollectionModel<SmileVideoTagViewModel> RelationTagItems { get; } = new CollectionModel<SmileVideoTagViewModel>();

        public SmileVideoSearchHistoryViewModel History { get; set; }

        public PageViewModel<SmileVideoSearchItemFinderViewModel> SelectedPage
        {
            get { return this._selectedPage; }
            set
            {
                var oldSelectedPage = this._selectedPage;
                if(SetVariableValue(ref this._selectedPage, value)) {
                    if(this._selectedPage != null) {
                        this._selectedPage.IsChecked = true;
                    }
                    if(oldSelectedPage != null) {
                        PagerPropertyChangedListener.Remove(oldSelectedPage.ViewModel);
                        SearchPropertyChangedListener.Remove(oldSelectedPage.ViewModel);
                        oldSelectedPage.IsChecked = false;
                    }

                    CallPageItemOnPropertyChange();

                    if(this._selectedPage != null && oldSelectedPage != null) {
                        this._selectedPage.ViewModel.InputTitleFilter = oldSelectedPage.ViewModel.InputTitleFilter;
                        this._selectedPage.ViewModel.SelectedSortType = oldSelectedPage.ViewModel.SelectedSortType;
                        // #168
                        this._selectedPage.ViewModel.IsAscending = oldSelectedPage.ViewModel.IsAscending;
                        this._selectedPage.ViewModel.IsBlacklist = oldSelectedPage.ViewModel.IsBlacklist;
                        this._selectedPage.ViewModel.IsEnabledFinderFiltering = oldSelectedPage.ViewModel.IsEnabledFinderFiltering;

                        this._selectedPage.ViewModel.FinderItems.Refresh();
                    }
                }
            }
        }

        public override ICollectionView FinderItems
        {
            get
            {
                if(SelectedPage == null) {
                    if(SearchFinder != null) {
                        return SearchFinder.FinderItems;
                    } else {
                        return base.FinderItems;
                    }
                }
                return SelectedPage.ViewModel.FinderItems;
            }
        }

        public override IReadOnlyList<SmileVideoFinderItemViewModel> FinderItemsViewer => GetSearchProperty<IReadOnlyList<SmileVideoFinderItemViewModel>>();

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

        [Obsolete]
        /// <summary>
        /// ピン止めされているか。
        /// </summary>
        public bool IsPin
        {
            get
            {
                return SmileVideoSearchUtility.IsPinItem(Setting.Search.SearchPinItems, Query, Type);
            }
        }

        public bool IsBookmark
        {
            get
            {
                return SmileVideoSearchUtility.IsBookmarkItem(Setting.Search.SearchBookmarkItems, Query, Type);
            }
        }

        public override SourceLoadState FinderLoadState
        {
            get
            {
                if(SelectedPage == null) {
                    if(SearchFinder != null) {
                        return SearchFinder.FinderLoadState;
                    } else {
                        return base.FinderLoadState;
                    }
                }
                return SelectedPage.ViewModel.FinderLoadState;
            }

            set
            {
                if(SelectedPage == null) {
                    if(SearchFinder != null) {
                        SearchFinder.FinderLoadState = value;
                    } else {
                        base.FinderLoadState = value;
                    }
                } else {
                    SelectedPage.ViewModel.FinderLoadState = value;
                }
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
                        var pageVm = (PageViewModel<SmileVideoSearchItemFinderViewModel>)o;
                        if(pageVm.LoadState != LoadState.Loaded) {
                            var thumbCacheSpan = Constants.ServiceSmileVideoThumbCacheSpan;
                            var imageCacheSpan = Constants.ServiceSmileVideoImageCacheSpan;

                            SelectedPage = pageVm;
                            PagerPropertyChangedListener.Add(pageVm.ViewModel);

                            pageVm.ViewModel.LoadAsync(thumbCacheSpan, imageCacheSpan).ConfigureAwait(true);
                        } else {
                            SelectedPage = pageVm;
                        }
                    }
                );
            }
        }

        public override ICommand ReloadCommand
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

        [Obsolete]
        public ICommand SwitchPinCommand
        {
            get { return CreateCommand(o => SwitchPin()); }
        }

        public ICommand SwitchBookmarkCommand
        {
            get { return CreateCommand(o => SwitchBookmark()); }
        }

        #endregion

        #region function

        void CallPageItemOnPropertyChange()
        {
            CallOnPropertyChange(ChangePagePropertyNames);
        }

        [Obsolete]
        void SwitchPin()
        {
            if(IsPin) {
                RemovePin();
            } else {
                AddPin();
            }
        }

        [Obsolete]
        void AddPin()
        {
            var item = new SmileVideoSearchPinModel() {
                MethodKey = SelectedMethod.Key,
                SortKey = SelectedSort.Key,
                Query = this.Query,
                SearchType = Type,
            };
            Setting.Search.SearchPinItems.Add(item);

            CallOnPropertyChangeDisplayItem();
        }

        [Obsolete]
        void RemovePin()
        {
            var item = SmileVideoSearchUtility.FindPinItem(Setting.Search.SearchPinItems, Query, Type);
            if(item != null) {
                Setting.Search.SearchPinItems.Remove(item);
            }

            CallOnPropertyChangeDisplayItem();
        }

        void SwitchBookmark()
        {
            if(IsBookmark) {
                RemoveBookmark();
            } else {
                AddBookmark();
            }
        }

        void AddBookmark()
        {
            Mediation.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessSearchBookmarkParameterModel(true, Query, Type)));

            CallOnPropertyChangeDisplayItem();
        }

        void RemoveBookmark()
        {
            Mediation.Request(new SmileVideoProcessRequestModel(new SmileVideoProcessSearchBookmarkParameterModel(false, Query, Type)));

            CallOnPropertyChangeDisplayItem();
        }

        DefinedElementModel GetContextElemetFromChangeElement(IEnumerable<DefinedElementModel> items, DefinedElementModel element)
        {
            if(items.Any(i => i == element)) {
                return element;
            } else {
                return items.FirstOrDefault(i => i.Key == element.Key);
            }
        }

        public void SetContextElements(DefinedElementModel method, DefinedElementModel sort)
        {
            SelectedPage = null;

            LoadingMethod = SelectedMethod = GetContextElemetFromChangeElement(MethodItems, method);
            LoadingSort = SelectedSort = GetContextElemetFromChangeElement(SortItems, sort);
        }

        protected override Task LoadCoreAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            var isReload = (bool)extends;

            DefinedElementModel nowMethod;
            DefinedElementModel nowSort;
            if(isReload) {
                nowMethod = SelectedMethod;
                nowSort = SelectedSort;
                SelectedPage = null;
            } else {
                nowMethod = LoadingMethod;
                nowSort = LoadingSort;
            }

            SearchFinder = new SmileVideoSearchItemFinderViewModel(Mediation, SearchModel, nowMethod, nowSort, Type, Query, 0, SearchModel.GetDefaultSearchTypeDefine().MaximumCount);
            //SearchFinder.PropertyChanged += PageVm_PropertyChanged;
            //PropertyChangedEventManager.AddListener(SearchFinder, PagerPropertyChangedListener, string.Empty);
            PagerPropertyChangedListener.Add(SearchFinder);

            var query = Query;

            if(isReload) {
                SearchPropertyChangedListener.Add(SearchFinder);

                var suggestion = new Suggestion(Mediation);
                suggestion.LoadCompleteAsync(query).ContinueWith(t => {
                    var tags = t.Result;
                    var items = tags.Items
                        .Where(s => s != query)
                        .Select(s => new RawSmileVideoTagItemModel() { Text = s, })
                        .Select(tag => new SmileVideoTagViewModel(Mediation, tag))
                    ;
                    RelationTagItems.InitializeRange(items);
                }, TaskScheduler.FromCurrentSynchronizationContext());

                // #503, いつ復活するか分からんので一応残しておく
                //var tag = new Logic.Service.Smile.Video.Api.V1.Tag(Mediation);
                //var tagTask = tag.LoadRelationTagListAsync(query).ContinueWith(task => {
                //    var list = task.Result;
                //    var items = list.Tags
                //        .Where(t => t.Text != query)
                //        .Select(t => new SmileVideoTagViewModel(Mediation, t))
                //    ;
                //    RelationTagItems.InitializeRange(items);
                //}, TaskScheduler.FromCurrentSynchronizationContext());
            }

            return SearchFinder.LoadAsync(thumbCacheSpan, imageCacheSpan);
        }

        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, bool isReload)
        {
            return LoadCoreAsync(thumbCacheSpan, imageCacheSpan, isReload);
        }

        public new Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            throw new NotSupportedException();
        }

        KeyValuePair<MemberInfo, SmileVideoFinderViewModelBase> GetMemberInfo(bool getMethod, string memberName)
        {
            SmileVideoFinderViewModelBase target;
            if(SelectedPage == null) {
                if(SearchFinder != null) {
                    target = SearchFinder;
                } else {
                    target = this;
                }
            } else {
                target = SelectedPage.ViewModel;
            }
            var type = target.GetType();
            MemberInfo[] members = getMethod
                ? type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(m => m.Name == memberName).ToArray()
                : type.GetMember(memberName)
            ;
            MemberInfo member;
            if(target == this) {
                member = members.First(m => m.DeclaringType == typeof(SmileVideoFinderViewModelBase));
            } else {
                member = members.First();
            }

            return new KeyValuePair<MemberInfo, SmileVideoFinderViewModelBase>(member, target);
        }

        KeyValuePair<PropertyInfo, SmileVideoFinderViewModelBase> GetPropertyInfo([CallerMemberName] string propertyName = "")
        {
            var pair = GetMemberInfo(false, propertyName);

            return new KeyValuePair<PropertyInfo, SmileVideoFinderViewModelBase>((PropertyInfo)pair.Key, pair.Value);
        }

        KeyValuePair<MethodInfo, SmileVideoFinderViewModelBase> GetMethodInfo([CallerMemberName] string propertyName = "")
        {
            var pair = GetMemberInfo(true, propertyName);

            return new KeyValuePair<MethodInfo, SmileVideoFinderViewModelBase>((MethodInfo)pair.Key, pair.Value);
        }

        TResult GetSearchProperty<TResult>([CallerMemberName] string propertyName = "")
        {
            var pair = GetPropertyInfo(propertyName);
            Debug.Assert(pair.Key.PropertyType == typeof(TResult));

            return (TResult)pair.Key.GetValue(pair.Value);
        }
        void SetSearchProperty<TValue>(TValue value, [CallerMemberName] string propertyName = "")
        {
            var pair = GetPropertyInfo(propertyName);
            Debug.Assert(pair.Key.PropertyType == typeof(TValue));

            pair.Key.SetValue(pair.Value, value);
        }

        void DoSearchAction(string methodName, params object[] parameters)
        {
            var pair = GetMethodInfo(methodName);
            Debug.Assert(pair.Key.ReturnType == typeof(void));

            pair.Key.Invoke(pair.Value, parameters);
        }

        TResult DoSearchFunction<TResult>(string methodName, params object[] parameters)
        {
            var pair = GetMethodInfo(methodName);
            Debug.Assert(pair.Key.ReturnType == typeof(TResult));

            return (TResult)pair.Key.Invoke(pair.Value, parameters);
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

        public override string InputTitleFilter
        {
            get { return GetSearchProperty<string>(); }
            set { SetSearchProperty(value); }
        }
        public override bool IsBlacklist
        {
            get { return GetSearchProperty<bool>(); }
            set { SetSearchProperty(value); }
        }

        public override bool IsEnabledFinderFiltering
        {
            get { return GetSearchProperty<bool>(); }
            set { SetSearchProperty(value); }
        }

        public override bool ShowFilterSetting
        {
            get { return GetSearchProperty<bool>(); }
            set { SetSearchProperty(value); }
        }

        internal override void ToggleAllCheck()
        {
            DoSearchAction(nameof(ToggleAllCheck));
        }

        protected override void SwitchShowFilter()
        {
            DoSearchAction(nameof(SwitchShowFilter));
        }

        internal override Task ContinuousPlaybackAsync(bool isRandom, Action<SmileVideoPlayerViewModel> playerPreparationAction = null)
        {
            return DoSearchFunction<Task>(nameof(ContinuousPlaybackAsync), isRandom, playerPreparationAction);
        }

        public override bool IsAscending
        {
            get { return GetSearchProperty<bool>(); }
            set { SetSearchProperty(value); }
        }

        public override SmileVideoSortType SelectedSortType
        {
            get { return GetSearchProperty<SmileVideoSortType>(); }
            set { SetSearchProperty(value); }
        }

        protected override void CallOnPropertyChangeDisplayItem()
        {
            base.CallOnPropertyChangeDisplayItem();

            var propertyNames = new[] {
                nameof(IsBookmark),
            };

            CallOnPropertyChange(propertyNames);
        }


        #endregion

        private void PageVm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Mediation.Logger.Information(e.PropertyName);
            if(ChangePagePropertyNames.Any(n => n == e.PropertyName)) {
                CallPageItemOnPropertyChange();
            }
        }

        private void SearchFinder_PropertyChanged_TotalCount(object sender, PropertyChangedEventArgs e)
        {
            var searchFinder = (SmileVideoSearchItemFinderViewModel)sender;
            if(e.PropertyName == nameof(searchFinder.TotalCount)) {
                SearchPropertyChangedListener.Remove(searchFinder);

                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    TotalCount = searchFinder.TotalCount;
                    History.TotalCount = searchFinder.TotalCount;

                    var usingList = Enumerable.Empty<PageViewModel<SmileVideoSearchItemFinderViewModel>>();

                    var define = SearchModel.GetDefaultSearchTypeDefine();

                    if(TotalCount > define.MaximumCount) {

                        var pageCount = Math.Min(TotalCount / define.MaximumCount, (define.MaximumIndex + define.MaximumCount) / define.MaximumCount);
                        var correctionPage = TotalCount > (define.MaximumIndex + define.MaximumCount) ? 1 : 0;
                        var preList = Enumerable.Range(1, pageCount - correctionPage)
                            .Select((n, i) => new SmileVideoSearchItemFinderViewModel(Mediation, SearchModel, searchFinder.Method, searchFinder.Sort, Type, searchFinder.Query, (i + 1) * define.MaximumCount, define.MaximumCount))
                            .Select((v, i) => new PageViewModel<SmileVideoSearchItemFinderViewModel>(v, i + 2))
                            .ToEvaluatedSequence()
                        ;
                        var pageVm = new PageViewModel<SmileVideoSearchItemFinderViewModel>(searchFinder, 1) {
                            LoadState = LoadState.Loaded,
                        };
                        preList.Insert(0, pageVm);
                        usingList = preList;
                    } else if(TotalCount > 0) {
                        var pageVm = new PageViewModel<SmileVideoSearchItemFinderViewModel>(searchFinder, 1) {
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
    }
}
