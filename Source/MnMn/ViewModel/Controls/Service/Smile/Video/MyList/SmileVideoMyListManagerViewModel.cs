﻿/*
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Exceptions.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video.Api.V1;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw.Feed;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;


namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList
{
    public class SmileVideoMyListManagerViewModel: SmileVideoCustomManagerViewModelBase
    {
        #region variable

        SmileVideoAccountMyListFinderViewModel _selectedAccountFinder;
        SmileVideoBookmarkMyListFinderViewModel _selectedBookmarkFinder;
        SmileVideoMyListFinderViewModelBase _selectedSearchFinder;
        SmileVideoMyListFinderViewModelBase _selectedHistoryFinder;

        SmileVideoAccountMyListFinderViewModel _selectedAccountSortFinder;
        CollectionModel<SmileVideoMyListFinderViewModelBase> _accountSortMyListItems;
        bool _showAccountSortMyList;

        bool _isSelectedAccount;
        bool _isSelectedSearch;
        bool _isSelectedBookmark;
        bool _isSelectedHistory;

        SmileVideoMyListFinderViewModelBase _selectedCurrentFinder;

        string _inputSearchMyList;
        PageViewModel<SmileVideoMyListFinderPageViewModel> _selectedPage;

        #endregion

        public SmileVideoMyListManagerViewModel(Mediator mediator)
            : base(mediator)
        {
            MyList = Mediator.GetResultFromRequest<SmileVideoMyListModel>(new RequestModel(RequestKind.PlayListDefine, ServiceType.SmileVideo));

            var smileSetting = Mediator.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
            MyListSetting = smileSetting.MyList;
            BookmarkUserMyListPairs = new MVMPairCreateDelegationCollection<SmileMyListBookmarkItemModel, SmileVideoBookmarkMyListFinderViewModel>(MyListSetting.Bookmark, default(object), (model, data) => new SmileVideoBookmarkMyListFinderViewModel(Mediator, model));

            SearchUserMyListItems = CollectionViewSource.GetDefaultView(SearchUserMyList);
            AccountMyListItems = CollectionViewSource.GetDefaultView(AccountMyList);
            BookmarkUserMyListItems = CollectionViewSource.GetDefaultView(BookmarkUserMyListPairs.ViewModelList);
            BookmarkUserMyListItems.Filter = BookmarkUserMyListFilter;
            HistoryUserMyListItems = CollectionViewSource.GetDefaultView(HistoryUserMyList);

            TagNamesPropertyChangedListener = new PropertyChangedWeakEventListener(SelectedBookmarkFinder_PropertyChanged);
            TagItemPropertyChangedListener = new PropertyChangedWeakEventListener(Item_PropertyChanged);
            FinderLoadStatePropertyChangedListener = new PropertyChangedWeakEventListener(SelectedCurrentFinder_PropertyChanged);

            SetTagItems();
        }

        #region property

        SmileMyListSettingModel MyListSetting { get; }

        PropertyChangedWeakEventListener TagNamesPropertyChangedListener { get; }
        PropertyChangedWeakEventListener TagItemPropertyChangedListener { get; }
        PropertyChangedWeakEventListener FinderLoadStatePropertyChangedListener { get; }

        SmileVideoMyListModel MyList { get; }

        public char BookmarkTagTokenSplitter { get { return Constants.SmileMyListBookmarkTagTokenSplitter; } }

        CollectionModel<SmileVideoAccountMyListFinderViewModel> AccountMyList { get; } = new CollectionModel<SmileVideoAccountMyListFinderViewModel>();
        public ICollectionView AccountMyListItems { get; }
        public IReadOnlyList<SmileVideoAccountMyListFinderViewModel> AccountMyListViewer => AccountMyList;

        /// <summary>
        /// 並べ替え用マイリスト一覧。
        /// </summary>
        public CollectionModel<SmileVideoMyListFinderViewModelBase> AccountSortMyListItems
        {
            get { return this._accountSortMyListItems; }
            set { SetVariableValue(ref this._accountSortMyListItems, value); }
        }
        /// <summary>
        /// 並べ替え用選択マイリスト。
        /// </summary>
        public SmileVideoAccountMyListFinderViewModel SelectedAccountSortFinder
        {
            get { return this._selectedAccountSortFinder; }
            set { SetVariableValue(ref this._selectedAccountSortFinder, value); }
        }
        public bool ShowAccountSortMyList
        {
            get { return this._showAccountSortMyList; }
            set
            {
                if(SetVariableValue(ref this._showAccountSortMyList, value)) {
                    if(ShowAccountSortMyList) {
                        AccountSortMyListItems = new CollectionModel<SmileVideoMyListFinderViewModelBase>(AccountMyListViewer.Where(f => f.CanEdit));
                    } else {
                        IsSortChanged = false;
                    }
                }
            }
        }
        public bool IsSortChanged { get; set; }

        CollectionModel<SmileVideoMyListFinderViewModelBase> SearchUserMyList { get; } = new CollectionModel<SmileVideoMyListFinderViewModelBase>();
        public ICollectionView SearchUserMyListItems { get; }
        public CollectionModel<PageViewModel<SmileVideoMyListFinderPageViewModel>> PageItems { get; } = new CollectionModel<PageViewModel<SmileVideoMyListFinderPageViewModel>>();

        MVMPairCreateDelegationCollection<SmileMyListBookmarkItemModel, SmileVideoBookmarkMyListFinderViewModel> BookmarkUserMyListPairs { get; }
        public ICollectionView BookmarkUserMyListItems { get; }

        public CollectionModel<SmileVideoMyListBookmarkFilterViewModel> BookmarkTagItems { get; } = new CollectionModel<SmileVideoMyListBookmarkFilterViewModel>();

        public bool UsingBookmarkTagFilter
        {
            get { return MyListSetting.UsingBookmarkTagFilter; }
            set
            {
                if(SetPropertyValue(MyListSetting, value)) {
                    BookmarkUserMyListItems.Refresh();
                }
            }
        }

        CollectionModel<SmileVideoItemsMyListFinderViewModel> HistoryUserMyList { get; } = new CollectionModel<SmileVideoItemsMyListFinderViewModel>();
        public ICollectionView HistoryUserMyListItems { get; }

        public bool IsSelectedAccount
        {
            get { return this._isSelectedAccount; }
            set
            {
                if(SetVariableValue(ref this._isSelectedAccount, value)) {
                    if(IsSelectedAccount) {
                        if(Session.IsLoggedIn) {
                            ChangedSelectedFinder(SelectedAccountFinder);
                        }
                    }
                }
            }
        }
        public bool IsSelectedSearch
        {
            get { return this._isSelectedSearch; }
            set
            {
                if(SetVariableValue(ref this._isSelectedSearch, value)) {
                    if(IsSelectedSearch) {
                        ChangedSelectedFinder(SelectedSearchFinder);
                    }
                }
            }
        }
        public bool IsSelectedBookmark
        {
            get { return this._isSelectedBookmark; }
            set
            {
                if(SetVariableValue(ref this._isSelectedBookmark, value)) {
                    if(IsSelectedBookmark) {
                        var setting = Mediator.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
                        var items = setting.MyList.Bookmark.ToEvaluatedSequence();
                        BookmarkUserMyListPairs.Clear();
                        foreach(var item in items) {
                            BookmarkUserMyListPairs.Add(item, null);
                        }
                        BookmarkUserMyListItems.Refresh();
                        ChangedSelectedFinder(SelectedBookmarkFinder);
                    }
                }
            }
        }
        public bool IsSelectedHistory
        {
            get { return this._isSelectedHistory; }
            set
            {
                if(SetVariableValue(ref this._isSelectedHistory, value)) {
                    if(IsSelectedHistory) {
                        var setting = Mediator.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
                        var items = setting.MyList.History.Select(m => new SmileVideoItemsMyListFinderViewModel(Mediator, m));

                        HistoryUserMyList.InitializeRange(items);
                        HistoryUserMyListItems.Refresh();
                        ChangedSelectedFinder(SelectedHistoryFinder);
                    }
                }
            }
        }

        public GridLength GroupWidth
        {
            get { return new GridLength(MyListSetting.GroupWidth, GridUnitType.Star); }
            set { SetPropertyValue(MyListSetting, value.Value, nameof(MyListSetting.GroupWidth)); }
        }
        public GridLength ItemsWidth
        {
            get { return new GridLength(MyListSetting.ItemsWidth, GridUnitType.Star); }
            set { SetPropertyValue(MyListSetting, value.Value, nameof(MyListSetting.ItemsWidth)); }
        }

        public SmileVideoAccountMyListFinderViewModel SelectedAccountFinder
        {
            get { return this._selectedAccountFinder; }
            set
            {
                if(SetVariableValue(ref this._selectedAccountFinder, value)) {
                    ChangedSelectedFinder(this._selectedAccountFinder);
                }
            }
        }
        public SmileVideoBookmarkMyListFinderViewModel SelectedBookmarkFinder
        {
            get { return this._selectedBookmarkFinder; }
            set
            {
                var prev = SelectedBookmarkFinder;
                if(SetVariableValue(ref this._selectedBookmarkFinder, value)) {
                    if(prev != null) {
                        TagNamesPropertyChangedListener.Remove(prev);
                    }
                    if(SelectedBookmarkFinder != null) {
                        TagNamesPropertyChangedListener.Remove(SelectedBookmarkFinder);
                        TagNamesPropertyChangedListener.Add(SelectedBookmarkFinder);
                    }
                    ChangedSelectedFinder(SelectedBookmarkFinder);
                }
            }
        }

        public SmileVideoMyListFinderViewModelBase SelectedSearchFinder
        {
            get { return this._selectedSearchFinder; }
            set
            {
                if(SetVariableValue(ref this._selectedSearchFinder, value)) {
                    ChangedSelectedFinder(this._selectedSearchFinder);
                }
            }
        }
        public SmileVideoMyListFinderViewModelBase SelectedHistoryFinder
        {
            get { return this._selectedHistoryFinder; }
            set
            {
                if(SetVariableValue(ref this._selectedHistoryFinder, value)) {
                    ChangedSelectedFinder(this._selectedHistoryFinder);
                }
            }
        }

        public SmileVideoMyListFinderViewModelBase SelectedCurrentFinder
        {
            get { return this._selectedCurrentFinder; }
            set {
                var prev = SelectedCurrentFinder;
                if(SetVariableValue(ref this._selectedCurrentFinder, value)) {
                    if(prev != null) {
                        FinderLoadStatePropertyChangedListener.Remove(prev);
                    }
                    if(SelectedCurrentFinder != null) {
                        FinderLoadStatePropertyChangedListener.Add(SelectedCurrentFinder);
                    }
                }
            }
        }

        public string InputSearchMyList
        {
            get { return this._inputSearchMyList; }
            set { SetVariableValue(ref this._inputSearchMyList, value); }
        }

        public PageViewModel<SmileVideoMyListFinderPageViewModel> SelectedPage
        {
            get { return this._selectedPage; }
            set
            {
                var oldSelectedPage = this._selectedPage;
                if(SetVariableValue(ref this._selectedPage, value)) {
                    if(this._selectedPage != null) {
                        this._selectedPage.IsChecked = true;
                        SearchUserMyList.InitializeRange(this._selectedPage.ViewModel.Items);
                        Task.Run(() => {
                            SelectedSearchFinder = (SmileVideoSearchMyListFinderViewModel)this._selectedPage.ViewModel.Items.First();
                        });
                    }
                    if(oldSelectedPage != null) {
                        oldSelectedPage.IsChecked = false;
                    }
                }
            }
        }

        public IReadOnlyList<DefinedElementModel> FolderIdColors => MyList.Folder;
        public IReadOnlyList<DefinedElementModel> Sorts => MyList.Sort;

        #endregion

        #region command

        public ICommand LoadAccountMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LoadAccountMyListAsync(true, null).ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand CreateAccountMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        CreateAccountMyListAsync().ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand RemoveSelectedAccountMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(SelectedAccountFinder != null) {
                            RemoveAccountMyListAsync(SelectedAccountFinder).ConfigureAwait(false);
                        }
                    }
                );
            }
        }

        public ICommand UpAccountSortMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var index = AccountSortMyListItems.IndexOf(SelectedAccountSortFinder);
                        AccountSortMyListItems.SwapIndex(index, index - 1);
                    },
                    o => {
                        if(SelectedAccountSortFinder == null) {
                            return false;
                        }
                        var index = AccountSortMyListItems.IndexOf(SelectedAccountSortFinder);
                        return 0 < index;
                    }
                );
            }
        }

        public ICommand DownAccountSortMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var index = AccountSortMyListItems.IndexOf(SelectedAccountSortFinder);
                        AccountSortMyListItems.SwapIndex(index, index + 1);
                    },
                    o => {
                        if(SelectedAccountSortFinder == null) {
                            return false;
                        }
                        var index = AccountSortMyListItems.IndexOf(SelectedAccountSortFinder);
                        return AccountSortMyListItems.Count - 1 > index;
                    }
                );
            }
        }

        public ICommand SaveAccountSortMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => SaveAccountSortMyListTask().ConfigureAwait(false)
                );
            }
        }

        public ICommand RemoveCheckedAccountMyListVideoCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var finder = SelectedAccountFinder;
                        if(finder != null) {
                            RemoveAccountMyListVideoAsync(finder).ContinueWith(task => {
                                if(task.Result.IsSuccess) {
                                    System.Threading.Thread.Sleep(Constants.ServiceSmileMyListReloadWaitTime);
                                }
                                return task.Result;
                            }).ContinueWith(task => {
                                if(task.Result.IsSuccess) {
                                    finder.IgnoreCache = true;
                                    return finder.LoadDefaultCacheAsync().ContinueWith(t => {
                                        finder.IgnoreCache = false;
                                    });
                                } else {
                                    return Task.CompletedTask;
                                }
                            }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);
                        }
                    }
                );
            }
        }

        public ICommand SearchUserMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(!string.IsNullOrEmpty(InputSearchMyList)) {
                            SearchUserMyListAsync(InputSearchMyList).ConfigureAwait(false);
                        }
                    }
                );
            }
        }

        public ICommand SearchUserMyListPageChangeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var page = o as PageViewModel<SmileVideoMyListFinderPageViewModel>;
                        var mylist = new Logic.Service.Smile.Api.V1.MyList(Mediator);
                        mylist.SearchPage(page.ViewModel.Query, page.ViewModel.PageNumber).ContinueWith(task => {
                            page.ViewModel.Items.InitializeRange(task.Result);
                            SelectedPage = page;
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                );
            }
        }

        public ICommand SaveEditCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var finder = o as SmileVideoAccountMyListFinderViewModel;
                        if(finder.CanEdit) {
                            SaveEditMyListAsync(finder).ConfigureAwait(false);
                        }
                    }
                );
            }
        }

        public ICommand RemoveSelectedBookmarkUserMyListCommand
        {
            get
            {
                return CreateCommand(o => {
                    if(SelectedBookmarkFinder != null) {
                        RemoveSelectedBookmarkUserMyList();
                    }

                });
            }
        }

        public ICommand PositionUpBookmarkUserMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => ChangePositionBookmarkUserMyList((SmileVideoBookmarkMyListFinderViewModel)o, true),
                    o => IsSelectedBookmark && !UsingBookmarkTagFilter && SelectedBookmarkFinder != null && 0 < BookmarkUserMyListPairs.ViewModelList.IndexOf((SmileVideoBookmarkMyListFinderViewModel)o)
                );
            }
        }

        public ICommand PositionDownBookmarkUserMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => ChangePositionBookmarkUserMyList((SmileVideoBookmarkMyListFinderViewModel)o, false),
                    o => IsSelectedBookmark && !UsingBookmarkTagFilter && SelectedBookmarkFinder != null && BookmarkUserMyListPairs.Count - 1 > BookmarkUserMyListPairs.ViewModelList.IndexOf((SmileVideoBookmarkMyListFinderViewModel)o)
                );
            }
        }

        public ICommand AddBookmarkUserMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var finder = o as SmileVideoMyListFinderViewModelBase;
                        if(finder != null) {
                            AddBookmarkUserMyList(finder);
                        }
                    },
                    o => {
                        var finder = o as SmileVideoMyListFinderViewModelBase;
                        var canSaveState = new[] {
                            SourceLoadState.InformationLoading,
                            SourceLoadState.Completed,
                        };
                        return finder != null && canSaveState.Any(l => l == finder.FinderLoadState);
                    }
                );
            }
        }

        #endregion

        #region function

        void ClearSearchUserMyListPage()
        {
            SelectedPage = null;
            PageItems.Clear();
        }

        void ChangedSelectedFinder(SmileVideoMyListFinderViewModelBase selectedFinder)
        {
            if(selectedFinder != null && selectedFinder.CanLoad) {
                if(selectedFinder.FinderLoadState != SourceLoadState.Completed) {
                    selectedFinder.LoadDefaultCacheAsync();
                }
                ShowAccountSortMyList = false;
            }
            SelectedCurrentFinder = selectedFinder;
        }

        /// <summary>
        /// 糖衣構文みたいなもん。
        /// <para>名前空間とクラス名がかぶってしんどいぜ。</para>
        /// </summary>
        /// <returns></returns>
        Logic.Service.Smile.Api.V1.MyList GetMyListApi()
        {
            var mylist = new Logic.Service.Smile.Api.V1.MyList(Mediator);

            return mylist;
        }

        void SelecteTabItem(string name)
        {
            //SelectedTabItem
        }

        async Task LoadAccountMyListAsync(bool isFinderSelected, string selectedMyListId)
        {
            var list = new List<SmileVideoAccountMyListFinderViewModel>();

            // とりあえずマイリスト
            var defaultMyList = new SmileVideoAccountMyListDefaultFinderViewModel(Mediator);
            //var task = defaultMyList.LoadDefaultCacheAsync();
            list.Add(defaultMyList);

            // 自身のマイリスト一覧
            var mylist = new Logic.Service.Smile.Api.V1.MyList(Mediator);
            var accountGroup = await mylist.LoadAccountGroupAsync();
            if(SmileMyListUtility.IsSuccessResponse(accountGroup) && accountGroup.Groups.Any()) {
                foreach(var group in accountGroup.Groups.OrderByDescending(g => RawValueUtility.ConvertInteger(g.SortOder))) {
                    var finder = new SmileVideoAccountMyListFinderViewModel(Mediator, group);
                    list.Add(finder);
                }
            }

            AccountMyList.InitializeRange(list);

            if(isFinderSelected) {
                SelectedAccountFinder = list.FirstOrDefault(i => i.MyListId == selectedMyListId) ?? defaultMyList;
            }

            ShowAccountSortMyList = false;
        }

        async Task CreateAccountMyListAsync()
        {
            var myListIds = AccountMyList.Select(i => i.MyListId);
            var newMyListName = TextUtility.ToUniqueDefault(Properties.Resources.String_Service_Smile_MyList_CreateGroupName, AccountMyList.Select(i => i.MyListName), StringComparison.Ordinal);

            var myList = GetMyListApi();

            var result = await myList.CreateAccountGroupAsync(newMyListName);
            if(result.IsSuccess) {
                var myListId = SmileMyListUtility.GetMyListId(result);
                await LoadAccountMyListAsync(true, myListId);
            }
        }

        async Task RemoveAccountMyListAsync(SmileVideoAccountMyListFinderViewModel accountFinder)
        {
            var defaultMyListFinder = accountFinder as SmileVideoAccountMyListDefaultFinderViewModel;
            if(defaultMyListFinder != null) {
                // とりあえずマイリストは削除できない
                return;
            }

            var myList = GetMyListApi();
            await myList.DeleteAccountGroupAsync(accountFinder.MyListId);
            await LoadAccountMyListAsync(true, null);
        }

        async Task<IReadOnlyCheck> RemoveAccountMyListVideoAsync(SmileVideoAccountMyListFinderViewModel accountFinder)
        {
            var videoIdList = accountFinder.GetCheckedItems()
                .Select(v => v.Information.VideoId)
                .ToEvaluatedSequence()
            ;
            if(!videoIdList.Any()) {
                return CheckModel.Failure();
            }
            var myList = GetMyListApi();

            var defaultMyListFinder = accountFinder as SmileVideoAccountMyListDefaultFinderViewModel;
            if(defaultMyListFinder != null) {
                var model = await myList.LoadAccountDefaultAsync();
                var map = model.Items.ToDictionary(i => i.Data.VideoId, i => i.Id);
                var itemIdList = videoIdList.Select(s => map[s]);
                return await myList.RemoveAccountDefaultMyListFromVideo(itemIdList);
            } else {
                var myListId = accountFinder.MyListId;
                var model = await myList.LoadGroupAsync(myListId);
                var map = model.Channel.Items.ToDictionary(i => SmileVideoFeedUtility.GetVideoId(i), i => SmileVideoFeedUtility.GetItemId(i));
                var itemIdList = videoIdList.Select(s => map[s]);
                return await myList.RemoveAccountMyListFromVideo(myListId, itemIdList);
            }
        }

        async Task SaveAccountSortMyListTask()
        {
            var myList = GetMyListApi();
            var myListIds = AccountSortMyListItems
                .Select(f => f.MyListId)
                .ToEvaluatedSequence()
            ;
            var sortResult = await myList.SortAccountGroupAsync(myListIds);

            await LoadAccountMyListAsync(true, null);
        }


        public Task SearchUserMyListFromParameterAsync(SmileVideoSearchMyListParameterModel parameter)
        {
            switch(parameter.QueryType) {
                case SmileVideoSearchMyListQueryType.MyListId:
                    return SearchUserMyListFromIdAsync(parameter.Query);

                case SmileVideoSearchMyListQueryType.Keyword:
                    return SearchUserMyListFromKeywordAsync(parameter.Query);

                default:
                    throw new NotImplementedException();
            }
        }

        Task SearchUserMyListAsync(string inputSearchMyList)
        {
            var myListId = SmileIdUtility.GetMyListId(inputSearchMyList, Mediator);
            //object outputValue;
            if(!string.IsNullOrWhiteSpace(myListId)) {
                // 完全一致検索
                return SearchUserMyListFromIdAsync(myListId);
            } else {
                // 何かしら検索
                return SearchUserMyListFromKeywordAsync(inputSearchMyList);
            }

        }

        async Task SearchUserMyListFromIdAsync(string myListId)
        {
            // TODO: キャッシュ処理重複
            var dirInfo = Mediator.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Smile));
            var cachedDirPath = Path.Combine(dirInfo.FullName, Constants.SmileMyListCacheDirectoryName);
            var cacheDirectory = RestrictUtility.Is(Directory.Exists(cachedDirPath), () => new DirectoryInfo(cachedDirPath), () => Directory.CreateDirectory(cachedDirPath));
            var cacheFile = new FileInfo(Path.Combine(cacheDirectory.FullName, PathUtility.CreateFileName(myListId, "xml")));

            var mylist = new Logic.Service.Smile.Api.V1.MyList(Mediator);
            FeedSmileVideoModel group = null;
            if(CacheImageUtility.ExistImage(cacheFile.FullName, Constants.ServiceSmileMyListCacheSpan)) {
                using(var stream = new FileStream(cacheFile.FullName, FileMode.Open, FileAccess.Read)) {
                    group = SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoModel>(stream);
                }
            }
            if(group == null) {
                group = await mylist.LoadGroupAsync(myListId);
            }
            if(group != null) {
                SerializeUtility.SaveXmlSerializeToFile(cacheFile.FullName, group);

                var finder = new SmileVideoMatchMyListFinderViewModel(Mediator, group, myListId, false);

                ClearSearchUserMyListPage();
                SearchUserMyList.InitializeRange(new[] { finder });
                if(SearchUserMyList.Any()) {
                    SelectedSearchFinder = (SmileVideoMyListFinderViewModelBase)SearchUserMyList.First();
                }
            }
        }

        async Task SearchUserMyListFromKeywordAsync(string query)
        {
            var mylist = GetMyListApi();
            var finders = await mylist.SearchPage(query, 1);
            if(finders != null && finders.Count > 0) {
                var top = finders.First();
                var pageCount = top.TotalItemCount / top.PageItemCount;
                var list = Enumerable
                    .Range(1, pageCount)
                    .Select(i => new SmileVideoMyListFinderPageViewModel(Mediator, i + 1, query))
                    .ToEvaluatedSequence()
                ;
                list.Insert(0, new SmileVideoMyListFinderPageViewModel(Mediator, finders, query));
                var pages = list
                    .Select((vm, i) => new PageViewModel<SmileVideoMyListFinderPageViewModel>(vm, i + 1))
                ;
                PageItems.InitializeRange(pages);
                SearchUserMyList.InitializeRange(finders);
                SelectedPage = PageItems.First();
            } else {
                ClearSearchUserMyListPage();
            }
        }

        async Task SaveEditMyListAsync(SmileVideoAccountMyListFinderViewModel myListGroup)
        {
            var myList = GetMyListApi();

            await myList.UpdateAccountGroupAsync(
                myListGroup.MyListId,
                myListGroup.EditingMyListFolderIdElement.Key,
                myListGroup.EditingMyListName,
                myListGroup.EditingMyListSortElement.Key,
                myListGroup.EditingMyListDescription,
                myListGroup.EditingMyListIsPublic
            );
            await LoadAccountMyListAsync(true, myListGroup.MyListId);
        }

        void RemoveSelectedBookmarkUserMyList()
        {
            BookmarkUserMyListPairs.Remove(SelectedBookmarkFinder);
            SetTagItems();
        }

        void ChangePositionBookmarkUserMyList(SmileVideoBookmarkMyListFinderViewModel viewModel, bool isUp)
        {
            var srcIndex = BookmarkUserMyListPairs.ViewModelList.IndexOf(viewModel);
            if(srcIndex == -1) {
                return;
            }
            if(isUp && srcIndex == 0) {
                return;
            }
            if(!isUp && srcIndex == BookmarkUserMyListPairs.Count - 1) {
                return;
            }

            var nextIndex = isUp ? srcIndex - 1 : srcIndex + 1;
            BookmarkUserMyListPairs.SwapIndex(srcIndex, nextIndex);
        }

        internal void AddBookmarkUserMyList(SmileVideoMyListFinderViewModelBase finder)
        {
            CheckUtility.DebugEnforceNotNull(finder);

            if(BookmarkUserMyListPairs.Any(i => i.Model.MyListId == finder.MyListId)) {
                Mediator.Logger.Trace($"mylist dup: {finder.MyListId}");
                return;
            }

            var model = new SmileMyListBookmarkItemModel() {
                MyListId = finder.MyListId,
                MyListName = finder.MyListName,
            };
            model.Videos.InitializeRange(finder.FinderItemsViewer.Select(i => i.Information.VideoId));
            //// 見た目履歴と違うけどまぁいいや
            //BookmarkUserMyListPairs.Add(model, null);
            AppUtility.PlusItem(BookmarkUserMyListPairs, model, null);
            BookmarkUserMyListItems.Refresh();
        }

        Task<IEnumerable<SmileVideoVideoItemModel>> CheckBookmarkMyListAsync(string myListId)
        {
            var myList = new Logic.Service.Smile.Api.V1.MyList(Mediator);
            var newMyListTask = myList.LoadGroupAsync(myListId);

            var smileSetting = Mediator.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
            var nowMyList = smileSetting.MyList.Bookmark.FirstOrDefault(m => m.MyListId == myListId);
            if(nowMyList == null) {
                return Task.FromResult(Enumerable.Empty<SmileVideoVideoItemModel>());
            }


            return newMyListTask.ContinueWith(t => {
                nowMyList.MyListName = t.Result.Channel.Title;

                var newModels = t.Result.Channel.Items;

                var newViewModels = newModels
                    .Select(item => {
                        var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(item, SmileVideoInformationFlags.None));
                        return Mediator.GetResultFromRequest<SmileVideoInformationViewModel>(request);
                    })
                    .ToEvaluatedSequence()
                ;

                var exceptVideoViewModel = newViewModels
                    .Select(i => i.VideoId)
                    .Except(nowMyList.Videos)
                    .Select(v => newViewModels.First(i => i.VideoId == v))
                    .Select(i => i.ToVideoItemModel())
                    .ToEvaluatedSequence()
                ;

                if(exceptVideoViewModel.Any()) {
                    nowMyList.Videos.InitializeRange(newViewModels.Select(i => i.VideoId));
                    nowMyList.UpdateTimestamp = DateTime.Now;
                }

                return (IEnumerable<SmileVideoVideoItemModel>)exceptVideoViewModel;
            });
        }

        /// <summary>
        /// マイリストの状態をチェックする。
        /// <para>対象はブックマーク。</para>
        /// </summary>
        /// <returns></returns>
        public Task<IList<SmileVideoVideoItemModel>> CheckMyListAsync()
        {
            var newVideoItems = new List<SmileVideoVideoItemModel>();
            var tasks = new List<Task>();
            foreach(var bookmark in BookmarkUserMyListPairs.ModelList) {
                var task = CheckBookmarkMyListAsync(bookmark.MyListId).ContinueWith(t => {
                    if(t.Result != null && t.Result.Any()) {
                        var videoItems = t.Result;
                        foreach(var item in videoItems) {
                            item.VolatileTag = new SmileVideoCheckItLaterFromModel() {
                                FromId = bookmark.MyListId,
                                FromName = bookmark.MyListCustomName ?? bookmark.MyListName,
                            };
                        }
                        newVideoItems.AddRange(videoItems);
                    }
                });
                tasks.Add(task);
            }

            return Task.WhenAll(tasks).ContinueWith(t => {
                return (IList<SmileVideoVideoItemModel>)newVideoItems;
            });
        }

        void SetTagItems()
        {
            var prevItems = BookmarkTagItems.ToEvaluatedSequence();
            foreach(var item in prevItems) {
                TagItemPropertyChangedListener.Remove(item);
            }

            var tagNames = BookmarkUserMyListPairs.ModelList
                .Where(b => !string.IsNullOrWhiteSpace(b.TagNames))
                .Select(b => b.TagNames.Split(Constants.SmileMyListBookmarkTagTokenSplitter))
                .Select(ts => ts.Select(s => s.Trim()))
                .Select(ts => ts.Where(s => !string.IsNullOrWhiteSpace(s)))
                .SelectMany(ts => ts)
                .OrderBy(s => s)
                .Distinct()
                .Select(s => new SmileVideoMyListBookmarkFilterViewModel(s))
                .ToEvaluatedSequence()
            ;
            foreach(var item in tagNames) {
                var prev = prevItems.FirstOrDefault(t => t.TagName == item.TagName);
                if(prev != null) {
                    item.IsChecked = prev.IsChecked;
                }

                TagItemPropertyChangedListener.Add(item);
            }
            BookmarkTagItems.InitializeRange(tagNames);
        }

        bool BookmarkUserMyListFilter(object obj)
        {
            if(!UsingBookmarkTagFilter) {
                return true;
            }

            if(BookmarkTagItems.All(b => !b.IsChecked.GetValueOrDefault())) {
                return true;
            }

            var targetTags = BookmarkTagItems.Where(b => b.IsChecked.GetValueOrDefault());

            var vm = (SmileVideoBookmarkMyListFinderViewModel)obj;
            var myTags = vm.TagNameItems.ToEvaluatedSequence();
            var show = targetTags.All(t => myTags.Any(s => s == t.TagName));

            return show;
        }



        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            if(SelectedAccountFinder == null) {
                if(AccountMyListViewer.Any()) {
                    SelectedAccountFinder = AccountMyListViewer.First();
                } else {
                    IsSelectedSearch = true;
                }
            }
        }

        protected override void HideViewCore()
        { }

        public override async Task InitializeAsync()
        {
            if(!NetworkUtility.IsNetworkAvailable) {
                Mediator.Logger.Information("skip milist");
                return;
            }

            // 動画IDの補正処理
            foreach(var bookmark in BookmarkUserMyListPairs.ModelList) {
                foreach(var item in bookmark.Videos.Select((v,i) => new { VideoId = v, Index = i})) {
                    if(SmileIdUtility.NeedCorrectionVideoId(item.VideoId, Mediator)) {
                        var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(item.VideoId, Constants.ServiceSmileVideoThumbCacheSpan));
                        try {
                            var info = await Mediator.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);
                            bookmark.Videos[item.Index] = info.VideoId;
                        } catch(SmileVideoGetthumbinfoFailureException ex) {
                            // やっばいことになったら破棄
                            Mediator.Logger.Warning(ex);
                        }
                    }
                }
            }

            Application.Current.Dispatcher.Invoke(() => {
                AccountMyList.Clear();
            });

            if(Session.IsLoggedIn) {
                await LoadAccountMyListAsync(false, null);
            }
        }

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        #endregion

        void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SmileVideoMyListBookmarkFilterViewModel.IsChecked)) {
                BookmarkUserMyListItems.Refresh();
            }
        }

        private void SelectedBookmarkFinder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SmileVideoBookmarkMyListFinderViewModel.TagNames)) {
                SetTagItems();
            }
        }

        private void SelectedCurrentFinder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SmileVideoMyListFinderViewModelBase.FinderLoadState)) {
                CommandManager.InvalidateRequerySuggested();
                var finder = sender as SmileVideoMyListFinderViewModelBase;
                if(finder != null) {
                    if(finder.FinderLoadState == SourceLoadState.Completed) {
                        FinderLoadStatePropertyChangedListener.Remove(finder);
                    }
                }
            }
        }
    }
}
