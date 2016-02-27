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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Api;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoMyListManagerViewModel: SmileVideoManagerViewModelBase
    {
        #region variable

        SmileVideoMyListFinderViewModelBase _selectedAccountFinder;
        SmileVideoMyListFinderViewModelBase _selectedLocalFinder;
        SmileVideoMyListFinderViewModelBase _selectedSearchFinder;
        SmileVideoMyListFinderViewModelBase _selectedHistoryFinder;

        SmileVideoMyListFinderViewModelBase _selectedCurrentFinder;

        string _inputSearchMyList;
        PageViewModel<SmileVideoMyListFinderPageViewModel> _selectedPage;

        #endregion

        public SmileVideoMyListManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            Session = MediationUtility.GetResultFromRequestResponse<SmileSessionViewModel>(Mediation, new RequestModel(RequestKind.Session, ServiceType.Smile));

            SearchUserMyListItems = CollectionViewSource.GetDefaultView(SearchUserMyList);
            AccountMyListItems = CollectionViewSource.GetDefaultView(AccountMyList);
            LocalUserMyListItems = CollectionViewSource.GetDefaultView(LocalUserMyList);
            HistoryUserMyListItems = CollectionViewSource.GetDefaultView(HistoryUserMyList);

        }

        #region property

        SmileSessionViewModel Session { get; }

        CollectionModel<SmileVideoMyListFinderViewModelBase> AccountMyList { get; } = new CollectionModel<SmileVideoMyListFinderViewModelBase>();
        public ICollectionView AccountMyListItems { get; }

        CollectionModel<SmileVideoMyListFinderViewModelBase> SearchUserMyList { get; } = new CollectionModel<SmileVideoMyListFinderViewModelBase>();
        public ICollectionView SearchUserMyListItems { get; }
        public CollectionModel<PageViewModel<SmileVideoMyListFinderPageViewModel>> SearchItems { get; } = new CollectionModel<PageViewModel<SmileVideoMyListFinderPageViewModel>>();

        CollectionModel<SmileVideoMyListFinderViewModelBase> LocalUserMyList { get; } = new CollectionModel<SmileVideoMyListFinderViewModelBase>();
        public ICollectionView LocalUserMyListItems { get; }

        CollectionModel<SmileVideoMyListFinderViewModelBase> HistoryUserMyList { get; } = new CollectionModel<SmileVideoMyListFinderViewModelBase>();
        public ICollectionView HistoryUserMyListItems { get; }

        public SmileVideoMyListFinderViewModelBase SelectedAccountFinder
        {
            get { return this._selectedAccountFinder; }
            set
            {
                if(SetVariableValue(ref this._selectedAccountFinder, value)) {
                    ChangedSelectedFinder(this._selectedAccountFinder);
                }
            }
        }
        public SmileVideoMyListFinderViewModelBase SelectedLocalFinder
        {
            get { return this._selectedLocalFinder; }
            set
            {
                if(SetVariableValue(ref this._selectedLocalFinder, value)) {
                    ChangedSelectedFinder(this._selectedLocalFinder);
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
                    SelectedSearchFinder = this._selectedPage.ViewModel.Items.First();
                    SearchUserMyList.InitializeRange(this._selectedPage.ViewModel.Items);
                }
            }
        }

        #endregion

        #region command

        public ICommand LoadAccountMyListCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LoadAccountMyListAsync().ConfigureAwait(false);
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
                        SearchUserMyListAsync(InputSearchMyList).ConfigureAwait(false);
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
                        var mylist = new MyList(Mediation, Session) {
                            SessionSupport = true,
                        };
                        mylist.SearchPage(page.ViewModel.Query, page.ViewModel.PageNumber).ContinueWith(task => {
                            page.ViewModel.Items.InitializeRange(task.Result);
                            SelectedPage = page;
                            //SearchUserMyList.InitializeRange(page.ViewModel.Items);
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                );
            }
        }

        #endregion

        #region function

        void ClearSearchUserMyListPage()
        {
            SelectedPage = null;
            SearchItems.Clear();
        }

        void ChangedSelectedFinder(SmileVideoMyListFinderViewModelBase selectedFinder)
        {
            if(selectedFinder != null && selectedFinder.CanLoad) {
                if(selectedFinder.FinderLoadState != SmileVideoFinderLoadState.Completed) {
                    selectedFinder.LoadDefaultCacheAsync();
                }
            }
            SelectedCurrentFinder = selectedFinder;
        }

        public async Task LoadAccountMyListAsync()
        {
            var list = new List<SmileVideoMyListFinderViewModelBase>();

            // とりあえずマイリスト
            var defaultMyList = new SmileVideoAccountMyListDefaultFinderViewModel(Mediation);
            //var task = defaultMyList.LoadDefaultCacheAsync();
            list.Add(defaultMyList);

            // 自身のマイリスト一覧
            var mylist = new MyList(Mediation, Session) {
                SessionSupport = true,
            };
            var accountGroup = await mylist.GetAccountGroupAsync();
            if(SmileVideoMyListUtility.IsSuccessResponse(accountGroup) && accountGroup.Groups.Any()) {
                foreach(var group in accountGroup.Groups) {
                    var finder = new SmileVideoAccountMyListFinderViewModel(Mediation, group);
                    list.Add(finder);
                }
            }

            AccountMyList.InitializeRange(list);

            SelectedAccountFinder = defaultMyList;
            Debug.WriteLine(accountGroup);
        }

        public async Task SearchUserMyListAsync(string inputSearchMyList)
        {
            object outputValue;
            if(Mediation.ConvertValue(out outputValue, typeof(string), SmileMediationKey.inputGetMyListId, inputSearchMyList, inputSearchMyList.GetType(), ServiceType.Smile)) {
                // 完全一致検索
                var mylist = new MyList(Mediation, Session) {
                    SessionSupport = true,
                };
                var myListId = (string)outputValue;
                var group = await mylist.GetGroupAsync(myListId);

                var finder = new SmileVideoMatchMyListFinderViewModel(Mediation, group, myListId, false);

                ClearSearchUserMyListPage();
                SearchUserMyList.InitializeRange(new[] { finder });
            } else {
                // 何かしら検索
                var mylist = new MyList(Mediation, Session) {
                    SessionSupport = true,
                };
                var finders = await mylist.SearchPage(inputSearchMyList, 1);
                if(finders.Count > 0) {
                    var top = finders.First();
                    var pageCount = top.TotalItemCount / top.PageItemCount;
                    var list = Enumerable
                        .Range(1, pageCount)
                        .Select(i => new SmileVideoMyListFinderPageViewModel(Mediation, i+1, inputSearchMyList))
                        .ToList()
                    ;
                    list.Insert(0, new SmileVideoMyListFinderPageViewModel(Mediation, finders, inputSearchMyList));
                    var pages = list
                        .Select((vm, i) => new PageViewModel<SmileVideoMyListFinderPageViewModel>(vm, i + 1))
                    ;
                    SearchItems.InitializeRange(pages);
                    SearchUserMyList.InitializeRange(finders);
                    SelectedPage = SearchItems.First();
                } else {
                    ClearSearchUserMyListPage();
                }
            }

        }

        #endregion
    }
}
