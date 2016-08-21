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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Parameter;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;


namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User
{
    public class SmileUsersManagerViewModel: ManagerViewModelBase
    {
        #region variable

        SmileUserInformationViewModel _selectedUser;

        #endregion

        public SmileUsersManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            BindingOperations.EnableCollectionSynchronization(UserItems, new object());
        }

        #region property

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

        #endregion

        #region command

        public ICommand CloseTabCommand
        {
            get
            {
                return CreateCommand(o => CloseTab((SmileUserInformationViewModel)o));
            }
        }

        #endregion

        #region function

        public Task LoadFromParameterAsync(SmileOpenUserIdParameterModel parameter)
        {
            return LoadAsync(parameter.UserId, parameter.IsLoginUser);
        }

        public Task LoadAsync(string userId, bool isLoginUser)
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
                return user.LoadDefaultAsync();
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

        #endregion

        #region ManagerViewModelBase

        protected override void ShowView()
        {
            if(LoginUser == null) {
                LoadLoginUserAsync().ConfigureAwait(false);
            }
            base.ShowView();
        }

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

        public override Task GarbageCollectionAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
