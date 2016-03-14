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
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile
{
    public class SmileManagerViewModel: ManagerViewModelBase
    {
        public SmileManagerViewModel(Mediation mediation)
            :base(mediation)
        {
            VideoManager = new SmileVideoManagerViewModel(Mediation);
            SettingManager = new SmileSettingManagerViewModel(Mediation);

            Mediation.SetManager(ServiceType.Smile, new SmileManagerPackModel(VideoManager, SettingManager));
        }

        #region property

        public SmileVideoManagerViewModel VideoManager { get; set; }
        public SmileSettingManagerViewModel SettingManager { get; set; }

        #endregion

        #region ManagerViewModelBase

        public async override Task InitializeAsync()
        {
            var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            await session.LoginAsync();
            // TODO: ログインできない場合は設定画面へ
            if(session.LoginState != LoginState.Logged) {

            } else {
                await VideoManager.InitializeAsync();
            }
        }

        #endregion

    }
}
