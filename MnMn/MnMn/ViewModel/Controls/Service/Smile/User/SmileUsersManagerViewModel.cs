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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
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
        { }

        #region property

        public CollectionModel<SmileUserInformationViewModel> UserItems { get; } = new CollectionModel<SmileUserInformationViewModel>();
        public SmileLoginUserInformationViewModel LoginUser { get; private set; }

        public SmileUserInformationViewModel SelectedUser
        {
            get { return this._selectedUser; }
            set { SetVariableValue(ref this._selectedUser, value); }
        }

        #endregion

        #region ManagerViewModelBase

        protected override void ShowView()
        {
            if(LoginUser == null) {
                var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
                if(session.LoginState == LoginState.Logged) {
                    LoginUser = new SmileLoginUserInformationViewModel(Mediation, session.UserId);
                    LoginUser.LoadDefaultAsync().ConfigureAwait(false);
                    UserItems.Add(LoginUser);
                }
            }
            base.ShowView();
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
