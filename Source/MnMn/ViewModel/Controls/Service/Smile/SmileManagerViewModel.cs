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
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile
{
    public class SmileManagerViewModel: ManagerViewModelBase
    {
        #region variable

        string _inputVideoId;

        #endregion

        public SmileManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            VideoManager = new SmileVideoManagerViewModel(Mediation);
            UsersManager = new SmileUsersManagerViewModel(Mediation);
            SettingManager = new SmileSettingManagerViewModel(Mediation);

            Mediation.SetManager(ServiceType.Smile, new SmileManagerPackModel(VideoManager, UsersManager, SettingManager));

            Session = Mediation.GetResultFromRequest<SessionViewModelBase>(new RequestModel(RequestKind.Session, ServiceType.Smile));
        }

        #region property

        public SessionViewModelBase Session { get; }

        public SmileVideoManagerViewModel VideoManager { get; set; }
        public SmileUsersManagerViewModel UsersManager { get; }
        public SmileSettingManagerViewModel SettingManager { get; set; }

        public string InputVideoId
        {
            get { return this._inputVideoId; }
            set { SetVariableValue(ref this._inputVideoId, value); }
        }

        #endregion

        #region command

        public ICommand OpenVideoPlayerCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        if(!string.IsNullOrWhiteSpace(InputVideoId)) {
                            OpenVideoPlayerAsync(InputVideoId.Trim()).ConfigureAwait(false);
                            InputVideoId = string.Empty;
                        }
                    },
                    o => Session.IsLoggedIn
                );
            }
        }

        #endregion

        #region function

        async Task<bool> OpenVideoPlayerAsync(string videoId)
        {
            //var videoInformation = await SmileVideoInformationViewModel.CreateFromVideoIdAsync(Mediation, videoId, Constants.ServiceSmileVideoThumbCacheSpan);
            var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));
            var videoInformation = await Mediation.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);

            await videoInformation.OpenPlayerAsync(false);
            return true;
        }

        #endregion

        #region ManagerViewModelBase

        public async override Task InitializeAsync()
        {
            var session = Mediation.GetResultFromRequest<SmileSessionViewModel>(new RequestModel(RequestKind.Session, ServiceType.Smile));
            await session.LoginAsync();
            // TODO: ログインできない場合は設定画面へ
            if(session.LoginState != LoginState.LoggedIn) {

            } else {
                await VideoManager.InitializeAsync();
            }
        }

        public override void InitializeView(MainWindow view)
        {
            VideoManager.InitializeView(view);
        }

        public override void UninitializeView(MainWindow view)
        {
            VideoManager.UninitializeView(view);
        }

        public override Task GarbageCollectionAsync()
        {
            var tasks = new[] {
                UsersManager.GarbageCollectionAsync(),
                VideoManager.GarbageCollectionAsync(),
                SettingManager.GarbageCollectionAsync(),
            };
            return Task.WhenAll(tasks);
        }

        #endregion

    }
}
