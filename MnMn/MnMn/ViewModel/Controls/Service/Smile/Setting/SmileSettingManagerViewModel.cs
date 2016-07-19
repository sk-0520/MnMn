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
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting
{
    public class SmileSettingManagerViewModel: ManagerViewModelBase
    {
        public SmileSettingManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            Setting = Mediation.GetResultFromRequest<SmileSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Smile));
            Session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
        }

        #region property

        SmileSettingModel Setting { get; }
        public SmileSessionViewModel Session { get; }

        public string AccountName
        {
            get { return Setting.Account.Name; }
            set { SetPropertyValue(Setting.Account, value, nameof(Setting.Account.Name)); }
        }
        public string AccountPassword
        {
            get { return Setting.Account.Password; }
            set { SetPropertyValue(Setting.Account, value, nameof(Setting.Account.Password)); }
        }

        #endregion

        #region command

        public ICommand LoginCommand
        {
            get
            {
                return CreateCommand(o => LoginAsync().ConfigureAwait(false));
            }
        }

        #endregion

        #region function

        Task LoginAsync()
        {
            return Session.ChangeUserAccountAsync(Setting.Account).ContinueWith(t => {
                return Session.LoginAsync();
            }).ContinueWith(t => {
                Mediation.Smile.ManagerPack.VideoManager.InitializeAsync();
            });
        }

        #endregion

        #region SmileVideoCustomManagerViewModelBase

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
