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
using ContentTypeTextNet.MnMn.MnMn.Define;
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
        SmileVideoItemsMyListFinderViewModel _selectedBookmarkFinder;
        SmileVideoMyListFinderViewModelBase _selectedSearchFinder;
        SmileVideoMyListFinderViewModelBase _selectedHistoryFinder;

        bool _isSelectedAccount;
        bool _isSelectedSearch;
        bool _isSelectedBookmark;
        bool _isSelectedHistory;

        SmileVideoMyListFinderViewModelBase _selectedCurrentFinder;

        string _inputSearchMyList;
        PageViewModel<SmileVideoMyListFinderPageViewModel> _selectedPage;

        #endregion

        public SmileVideoMyListManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            MyList = Mediation.GetResultFromRequest<SmileVideoMyListModel>(new RequestModel(RequestKind.PlayListDefine, ServiceType.SmileVideo));

            var smileSetting = Mediation.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
            BookmarkUserMyListPairs = new MVMPairCreateDelegationCollection<SmileMyListBookmarkItemModel, SmileVideoItemsMyListFinderViewModel>(smileSetting.MyList.Bookmark, default(object), (model, data) => new SmileVideoItemsMyListFinderViewModel(Mediation, model));

            SearchUserMyListItems = CollectionViewSource.GetDefaultView(SearchUserMyList);
            AccountMyListItems = CollectionViewSource.GetDefaultView(AccountMyList);
            BookmarkUserMyListItems = CollectionViewSource.GetDefaultView(BookmarkUserMyListPairs.ViewModelList);
            HistoryUserMyListItems = CollectionViewSource.GetDefaultView(HistoryUserMyList);
        }

        #region property

        SmileVideoMyListModel MyList { get; }

        CollectionModel<SmileVideoAccountMyListFinderViewModel> AccountMyList { get; } = new CollectionModel<SmileVideoAccountMyListFinderViewModel>();
        public ICollectionView AccountMyListItems { get; }
        public IReadOnlyList<SmileVideoAccountMyListFinderViewModel> AccountMyListViewer => AccountMyList;

        CollectionModel<SmileVideoMyListFinderViewModelBase> SearchUserMyList { get; } = new CollectionModel<SmileVideoMyListFinderViewModelBase>();
        public ICollectionView SearchUserMyListItems { get; }
        public CollectionModel<PageViewModel<SmileVideoMyListFinderPageViewModel>> PageItems { get; } = new CollectionModel<PageViewModel<SmileVideoMyListFinderPageViewModel>>();

        MVMPairCreateDelegationCollection<SmileMyListBookmarkItemModel, SmileVideoItemsMyListFinderViewModel> BookmarkUserMyListPairs { get; }
        public ICollectionView BookmarkUserMyListItems { get; }

        CollectionModel<SmileVideoItemsMyListFinderViewModel> HistoryUserMyList { get; } = new CollectionModel<SmileVideoItemsMyListFinderViewModel>();
        public ICollectionView HistoryUserMyListItems { get; }

        public bool IsSelectedAccount
        {
            get { return this._isSelectedAccount; }
            set { SetVariableValue(ref this._isSelectedAccount, value); }
        }
        public bool IsSelectedSearch
        {
            get { return this._isSelectedSearch; }
            set { SetVariableValue(ref this._isSelectedSearch, value); }
        }
        public bool IsSelectedBookmark
        {
            get { return this._isSelectedBookmark; }
            set
            {
                if(SetVariableValue(ref this._isSelectedBookmark, value)) {
                    if(value) {
                        var setting = Mediation.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
                        var items = setting.MyList.Bookmark.ToArray();
                        BookmarkUserMyListPairs.Clear();
                        foreach(var item in items) {
                            BookmarkUserMyListPairs.Add(item, null);
                        }
                        BookmarkUserMyListItems.Refresh();
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
                    if(value) {
                        var setting = Mediation.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
                        var items = setting.MyList.History.Select(m => new SmileVideoItemsMyListFinderViewModel(Mediation, m));

                        HistoryUserMyList.InitializeRange(items);
                        HistoryUserMyListItems.Refresh();
                    }
                }
            }
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
        public SmileVideoItemsMyListFinderViewModel SelectedBookmarkFinder
        {
            get { return this._selectedBookmarkFinder; }
            set
            {
                if(SetVariableValue(ref this._selectedBookmarkFinder, value)) {
                    ChangedSelectedFinder(this._selectedBookmarkFinder);
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
            set { SetVariableValue(ref this._selectedCurrentFinder, value); }
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
                        SelectedSearchFinder = (SmileVideoSearchMyListFinderViewModel)this._selectedPage.ViewModel.Items.First();
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
                                    return finder.LoadDefaultCacheAsync();
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
                        var mylist = new Logic.Service.Smile.Api.V1.MyList(Mediation);
                        mylist.SearchPage(page.ViewModel.Query, page.ViewModel.PageNumber).ContinueWith(task => {
                            page.ViewModel.Items.InitializeRange(task.Result);
                            SelectedPage = page;
                            //SearchUserMyList.InitializeRange(page.ViewModel.Items);
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
                return CreateCommand(o => ChangePositionBookmarkUserMyList((SmileVideoItemsMyListFinderViewModel)o, true));
            }
        }

        public ICommand PositionDownBookmarkUserMyListCommand
        {
            get
            {
                return CreateCommand(o => ChangePositionBookmarkUserMyList((SmileVideoItemsMyListFinderViewModel)o, false));
            }
        }

        public ICommand AddBookmarkUserMyListCommand
        {
            get
            {
                return CreateCommand(o => {
                    var finder = o as SmileVideoMyListFinderViewModelBase;
                    if(finder != null) {
                        AddBookmarkUserMyList(finder);
                    }
                });
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
            var mylist = new Logic.Service.Smile.Api.V1.MyList(Mediation);

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
            var defaultMyList = new SmileVideoAccountMyListDefaultFinderViewModel(Mediation);
            //var task = defaultMyList.LoadDefaultCacheAsync();
            list.Add(defaultMyList);

            // 自身のマイリスト一覧
            var mylist = new Logic.Service.Smile.Api.V1.MyList(Mediation);
            var accountGroup = await mylist.LoadAccountGroupAsync();
            if(SmileMyListUtility.IsSuccessResponse(accountGroup) && accountGroup.Groups.Any()) {
                foreach(var group in accountGroup.Groups.OrderByDescending(g => RawValueUtility.ConvertInteger(g.SortOder))) {
                    var finder = new SmileVideoAccountMyListFinderViewModel(Mediation, group);
                    list.Add(finder);
                }
            }

            AccountMyList.InitializeRange(list);

            if(isFinderSelected) {
                SelectedAccountFinder = list.FirstOrDefault(i => i.MyListId == selectedMyListId) ?? defaultMyList;
            }
        }

        async Task CreateAccountMyListAsync()
        {
            var myListIds = AccountMyList.Select(i => i.MyListId);
            var newMyListName = TextUtility.ToUniqueDefault(MyList.CreateGroupName.DisplayText, AccountMyList.Select(i => i.MyListName));

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

        async Task<CheckModel> RemoveAccountMyListVideoAsync(SmileVideoAccountMyListFinderViewModel accountFinder)
        {
            var videoIdList = accountFinder.GetCheckedItems()
                .Select(v => v.Information.VideoId)
                .ToArray()
            ;
            if(videoIdList.Length == 0) {
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
            object outputValue;
            if(Mediation.ConvertValue(out outputValue, typeof(string), SmileMediationKey.inputGetMyListId, inputSearchMyList, inputSearchMyList.GetType(), ServiceType.Smile)) {
                // 完全一致検索
                var myListId = (string)outputValue;
                return SearchUserMyListFromIdAsync(myListId);
            } else {
                // 何かしら検索
                return SearchUserMyListFromKeywordAsync(inputSearchMyList);
            }

        }

        async Task SearchUserMyListFromIdAsync(string myListId)
        {
            // TODO: キャッシュ処理重複
            var dirInfo = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Smile));
            var cachedDirPath = Path.Combine(dirInfo.FullName, Constants.SmileMyListCacheDirectoryName);
            var cacheDirectory = RestrictUtility.Is(Directory.Exists(cachedDirPath), () => new DirectoryInfo(cachedDirPath), () => Directory.CreateDirectory(cachedDirPath));
            var cacheFile = new FileInfo(Path.Combine(cacheDirectory.FullName, PathUtility.CreateFileName(myListId, "xml")));

            var mylist = new Logic.Service.Smile.Api.V1.MyList(Mediation);
            FeedSmileVideoModel group = null;
            if(CacheImageUtility.ExistImage(cacheFile.FullName, Constants.ServiceSmileMyListCacheSpan)) {
                using(var stream = new FileStream(cacheFile.FullName, FileMode.Open, FileAccess.Read)) {
                    group = SerializeUtility.LoadXmlSerializeFromStream<FeedSmileVideoModel>(stream);
                }
            }
            if(group == null) {
                group = await mylist.LoadGroupAsync(myListId);
                SerializeUtility.SaveXmlSerializeToFile(cacheFile.FullName, group);
            }

            var finder = new SmileVideoMatchMyListFinderViewModel(Mediation, group, myListId, false);

            ClearSearchUserMyListPage();
            SearchUserMyList.InitializeRange(new[] { finder });
            if(SearchUserMyList.Any()) {
                SelectedSearchFinder = (SmileVideoMyListFinderViewModelBase)SearchUserMyList.First();
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
                    .Select(i => new SmileVideoMyListFinderPageViewModel(Mediation, i + 1, query))
                    .ToList()
                ;
                list.Insert(0, new SmileVideoMyListFinderPageViewModel(Mediation, finders, query));
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
        }

        void ChangePositionBookmarkUserMyList(SmileVideoItemsMyListFinderViewModel viewModel, bool isUp)
        {
            var srcIndex = BookmarkUserMyListPairs.ViewModelList.IndexOf(viewModel);
            if(isUp && srcIndex == 0) {
                return;
            }
            if(!isUp && srcIndex == BookmarkUserMyListPairs.Count - 1) {
                return;
            }

            var nextIndex = isUp ? srcIndex - 1 : srcIndex + 1;
            BookmarkUserMyListPairs.SwapIndex(srcIndex, nextIndex);
        }

        void AddBookmarkUserMyList(SmileVideoMyListFinderViewModelBase finder)
        {
            CheckUtility.DebugEnforceNotNull(finder);

            // TODO 情報読み込み待ち
            //finder.FinderLoadState == SourceLoadState.None || SourceLoadState.SourceLoading || SourceLoadState.SourceChecking

            var model = new SmileMyListBookmarkItemModel() {
                MyListId = finder.MyListId,
                MyListName = finder.MyListName,
            };
            model.Videos.InitializeRange(finder.FinderItemsViewer.Select(i => i.Information.VideoId));
            // 見た目履歴と違うけどまぁいいや
            BookmarkUserMyListPairs.Add(model, null);
            BookmarkUserMyListItems.Refresh();
        }

        Task<IEnumerable<SmileVideoVideoItemModel>> CheckBookmarkMyListAsync(string myListId)
        {
            var myList = new Logic.Service.Smile.Api.V1.MyList(Mediation);
            var newMyListTask = myList.LoadGroupAsync(myListId);

            var smileSetting = Mediation.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
            var nowMyList = smileSetting.MyList.Bookmark.FirstOrDefault(m => m.MyListId == myListId);
            if(nowMyList == null) {
                return Task.FromResult(Enumerable.Empty<SmileVideoVideoItemModel>());
            }


            return newMyListTask.ContinueWith(t => {
                var newModels = t.Result.Channel.Items;
                var newViewModels = newModels
                    .Select(item => {
                        var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(item, SmileVideoInformationFlags.None));
                        return Mediation.GetResultFromRequest<SmileVideoInformationViewModel>(request);
                    })
                    .ToArray()
                ;

                var exceptVideoViewModel = newViewModels
                    .Select(i => i.VideoId)
                    .Except(nowMyList.Videos)
                    .Select(v => newViewModels.First(i => i.VideoId == v))
                    .Select(i => i.ToVideoItemModel())
                    .ToArray()
                ;

                if(exceptVideoViewModel.Any()) {
                    nowMyList.Videos.InitializeRange(newViewModels.Select(i => i.VideoId));
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
                        newVideoItems.AddRange(t.Result);
                    }
                });
                tasks.Add(task);
            }

            return Task.WhenAll(tasks).ContinueWith(t => {
                return (IList<SmileVideoVideoItemModel>)newVideoItems;
            });
        }


        #endregion

        #region ManagerViewModelBase

        protected override void ShowView()
        {
            if(SelectedAccountFinder == null) {
                if(AccountMyListViewer.Any()) {
                    SelectedAccountFinder = AccountMyListViewer.First();
                } else {
                    IsSelectedSearch = true;
                }
            }

            base.ShowView();
        }

        public override Task InitializeAsync()
        {
            if(Session.IsLoggedIn) {
                return LoadAccountMyListAsync(false, null);
            }

            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        public override Task GarbageCollectionAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
