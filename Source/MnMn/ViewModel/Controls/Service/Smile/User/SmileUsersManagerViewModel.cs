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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.User;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;


namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User
{
    public class SmileUsersManagerViewModel: ManagerViewModelBase
    {
        #region variable

        SmileUserInformationViewModel _selectedUser;

        SmileUserHistoryItemViewModel _selectedUserHistory;
        SmileUserBookmarkItemViewModel _selectedUserBookmark;

        #endregion

        public SmileUsersManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            BindingOperations.EnableCollectionSynchronization(UserItems, new object());

            Setting = Mediation.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));

            UserBookmarkCollection = new MVMPairCreateDelegationCollection<SmileUserBookmarkItemModel, SmileUserBookmarkItemViewModel>(Setting.User.Bookmark, default(object), CreateBookmarkItem);
            UserBookmarkItems = CollectionViewSource.GetDefaultView(UserBookmarkCollection.ViewModelList);

            UserHistoryCollection = new MVMPairCreateDelegationCollection<SmileUserItemModel, SmileUserHistoryItemViewModel>(Setting.User.History, default(object), CreateHistoryItem);
            UserHistoryItems = CollectionViewSource.GetDefaultView(UserHistoryCollection.ViewModelList);
        }

        #region property

        SmileSettingModel Setting { get; }

        public CollectionModel<SmileUserInformationViewModel> UserItems { get; } = new CollectionModel<SmileUserInformationViewModel>();
        public SmileLoginUserInformationViewModel LoginUser { get; private set; }

        public SmileUserInformationViewModel SelectedUser
        {
            get { return this._selectedUser; }
            set
            {
                if(SetVariableValue(ref this._selectedUser, value)) {
                    if(SelectedUser != null) {
                        SelectedUser.IsUserSelected = true;
                    }
                }
            }
        }

        MVMPairCreateDelegationCollection<SmileUserBookmarkItemModel, SmileUserBookmarkItemViewModel> UserBookmarkCollection { get; }
        public ICollectionView UserBookmarkItems { get; }

        MVMPairCreateDelegationCollection<SmileUserItemModel, SmileUserHistoryItemViewModel> UserHistoryCollection { get; }
        public ICollectionView UserHistoryItems { get; }

        public SmileUserHistoryItemViewModel SelectedUserHistory
        {
            get { return this._selectedUserHistory; }
            set
            {
                if(SetVariableValue(ref this._selectedUserHistory, value)) {
                    if(SelectedUserHistory != null) {
                        LoadAsync(SelectedUserHistory.UserId, false, false).ConfigureAwait(false);
                    }
                }
            }
        }

        public SmileUserBookmarkItemViewModel SelectedUserBookmark
        {
            get { return this._selectedUserBookmark; }
            set
            {
                if(SetVariableValue(ref this._selectedUserBookmark, value)) {
                    if(SelectedUserBookmark != null) {
                        LoadAsync(SelectedUserBookmark.UserId, false, true).ConfigureAwait(false);
                    }
                }
            }
        }

        #endregion

        #region command

        public ICommand CloseTabCommand
        {
            get
            {
                return CreateCommand(o => CloseTab((SmileUserInformationViewModel)o));
            }
        }

        public ICommand AddBookmarkCommand
        {
            get { return CreateCommand(o => AddBookmark((SmileUserInformationViewModel)o)); }
        }

        public ICommand RemoveBookmarkCommand
        {
            get { return CreateCommand(o => RemoveBookmark((SmileUserInformationViewModel)o)); }
        }

        #endregion

        #region function

        public Task LoadFromParameterAsync(SmileOpenUserIdParameterModel parameter)
        {
            return LoadAsync(parameter.UserId, parameter.IsLoginUser, parameter.AddHistory);
        }

        public Task LoadAsync(string userId, bool isLoginUser, bool addHistory)
        {
            var existUser = UserItems.FirstOrDefault(i => i.UserId == userId);
            if(existUser != null) {
                SelectedUser = existUser;
                return Task.CompletedTask;
            } else {
                var user = new SmileUserInformationViewModel(Mediation, userId, isLoginUser);
                UserItems.Insert(0, user);
                if(LoginUser == null) {
                    LoadLoginUserAsync().ConfigureAwait(false);
                }

                SelectedUser = user;
                return user.LoadDefaultAsync().ContinueWith(t => {
                    if(!isLoginUser && addHistory) {
                        AddHistory(user);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public Task LoadLoginUserAsync()
        {
            var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            if(session.LoginState == LoginState.LoggedIn) {
                LoginUser = new SmileLoginUserInformationViewModel(Mediation, session.UserId);
                return LoginUser.LoadDefaultAsync().ContinueWith(_ => {
                    UserItems.Add(LoginUser);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }

            return Task.CompletedTask;
        }

        void CloseTab(SmileUserInformationViewModel finder)
        {
            UserItems.Remove(finder);
        }

        static SmileUserBookmarkItemViewModel CreateBookmarkItem(SmileUserBookmarkItemModel model, object data)
        {
            var result = new SmileUserBookmarkItemViewModel(model);

            return result;
        }

        static SmileUserHistoryItemViewModel CreateHistoryItem(SmileUserItemModel model, object data)
        {
            var result = new SmileUserHistoryItemViewModel(model);

            return result;
        }

        void AddBookmarkVideos(SmileUserBookmarkItemModel bookmark, SmileUserInformationViewModel information)
        {
            var videoIds = information.PostFinder.FinderItemsViewer.Select(i => i.Information.VideoId);
            bookmark.Videos.InitializeRange(videoIds);
        }

        void AddBookmark(SmileUserInformationViewModel information)
        {
            var item = new SmileUserBookmarkItemModel() {
                UserId = information.UserId,
                UserName = information.UserName,
                UpdateTimestamp = DateTime.Now,
            };
            var existItem = UserBookmarkCollection.ModelList.FirstOrDefault(i => i.UserId == item.UserId);
            if(existItem != null) {
                return;
            }
            UserBookmarkCollection.Insert(0, item, null);
            RefreshUser();

            if(information.IsPublicPost) {
                if(information.PostFinder.FinderLoadState == SourceLoadState.None) {
                    information.PostFinder.LoadDefaultCacheAsync().Wait();
                    AddBookmarkVideos(item, information);
                } else {
                    PropertyChangedEventHandler propertyChanged = null;
                    propertyChanged = (object sender, PropertyChangedEventArgs e) => {
                        if(e.PropertyName == nameof(information.PostFinder.FinderLoadState)) {
                            if(information.PostFinder.FinderLoadState == SourceLoadState.InformationLoading || information.PostFinder.FinderLoadState == SourceLoadState.Completed) {
                                information.PostFinder.PropertyChanged -= propertyChanged;
                                AddBookmarkVideos(item, information);
                            }
                        }
                    };
                    information.PostFinder.PropertyChanged += propertyChanged;
                }
            }
        }

        void RemoveBookmark(SmileUserInformationViewModel information)
        {
            var viewModel = UserBookmarkCollection.ViewModelList.FirstOrDefault(i => i.UserId == information.UserId);
            if(viewModel != null) {
                UserBookmarkCollection.Remove(viewModel);
                RefreshUser();
            }
        }

        void RefreshUser()
        {
            var selectedUser = SelectedUser;
            SelectedUser = null;
            SelectedUser = selectedUser;
        }

        void AddHistory(SmileUserInformationViewModel information)
        {
            var item = new SmileUserItemModel() {
                UserId = information.UserId,
                UserName = information.UserName,
                UpdateTimestamp = DateTime.Now,
            };
            var existItem = UserHistoryCollection.ModelList.FirstOrDefault(i => i.UserId == item.UserId);
            if(existItem != null) {
                UserHistoryCollection.Remove(existItem);
                item = existItem;
            }
            UserHistoryCollection.Insert(0, item, null);
        }

        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            if(LoginUser == null) {
                LoadLoginUserAsync().ConfigureAwait(false);
            }
        }

        protected override void HideViewCore()
        { }

        public override Task InitializeAsync()
        {
            if(LoginUser != null) {
                var user = UserItems.LastOrDefault();
                if(user != null) {
                    UserItems.Remove(user);
                }
                LoginUser = null;
                SelectedUser = null;
            }

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
    }
}
