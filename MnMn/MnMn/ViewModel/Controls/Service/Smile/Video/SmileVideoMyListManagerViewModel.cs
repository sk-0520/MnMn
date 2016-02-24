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
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public class SmileVideoMyListManagerViewModel: SmileVideoManagerViewModelBase
    {
        #region variable

        SmileVideoMyListFinderViewModel _selectedFinder;
        string _inputSearchMyList;

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

        CollectionModel<SmileVideoMyListFinderViewModel> AccountMyList { get; } = new CollectionModel<SmileVideoMyListFinderViewModel>();
        public ICollectionView AccountMyListItems { get; }

        CollectionModel<SmileVideoMyListFinderViewModel> SearchUserMyList { get; } = new CollectionModel<SmileVideoMyListFinderViewModel>();
        public ICollectionView SearchUserMyListItems { get; }

        CollectionModel<SmileVideoMyListFinderViewModel> LocalUserMyList { get; } = new CollectionModel<SmileVideoMyListFinderViewModel>();
        public ICollectionView LocalUserMyListItems { get; }

        CollectionModel<SmileVideoMyListFinderViewModel> HistoryUserMyList { get; } = new CollectionModel<SmileVideoMyListFinderViewModel>();
        public ICollectionView HistoryUserMyListItems { get; }

        public SmileVideoMyListFinderViewModel SelectedFinder
        {
            get { return this._selectedFinder; }
            set
            {
                if(SetVariableValue(ref this._selectedFinder, value)) {
                    if(this._selectedFinder != null && this._selectedFinder.CanLoad) {
                        if(this._selectedFinder.FinderLoadState != SmileVideoFinderLoadState.Completed) {
                            this._selectedFinder.LoadDefaultCacheAsync();
                        }
                    }
                }
            }
        }

        public string InputSearchMyList
        {
            get { return this._inputSearchMyList; }
            set { SetVariableValue(ref this._inputSearchMyList, value); }
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

        #endregion

        #region function

        public async Task LoadAccountMyListAsync()
        {
            var list = new List<SmileVideoMyListFinderViewModel>();

            // とりあえずマイリスト
            var defaultMyList = new SmileVideoAccountMyListDefaultFinderViewModel(Mediation);
            //var task = defaultMyList.LoadDefaultCacheAsync();
            list.Add(defaultMyList);

            // 自身のマイリスト一覧
            var mylist = new MyList(Mediation, Session) {
                SessionSupport = true,
            };
            var accountGroup = await mylist.GetAccountGroupAsync();
            if(MyListUtility.IsSuccessResponse(accountGroup) && accountGroup.Groups.Any()) {
                foreach(var group in accountGroup.Groups) {
                    var finder = new SmileVideoAccountMyListFinderViewModel(Mediation, group);
                    list.Add(finder);
                }
            }

            AccountMyList.InitializeRange(list);

            SelectedFinder = defaultMyList;
            Debug.WriteLine(accountGroup);
        }

        public async Task SearchUserMyListAsync(string inputSearchMyList)
        {
            object outputValue;
            if(Mediation.ConvertValue(out outputValue, typeof(string), SmileMediationKey.inputGetMyListId, inputSearchMyList, inputSearchMyList.GetType(), ServiceType.Smile)) {
                var mylist = new MyList(Mediation, Session) {
                    SessionSupport = true,
                };
                var myListId = (string)outputValue;
                var group = await mylist.GetGroupAsync(myListId);

                var finder = new SmileVideoMyListFinderViewModel(Mediation, group, myListId, false);

                SearchUserMyList.InitializeRange(new[] { finder });
            }

            #endregion

        }
    }
}
