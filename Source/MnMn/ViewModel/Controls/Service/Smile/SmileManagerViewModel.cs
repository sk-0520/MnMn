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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Model.Service.Smile.Video.Raw;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Channel;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Live;
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

        public SmileManagerViewModel(Mediator mediator)
            : base(mediator)
        {
            Session = Mediator.GetResultFromRequest<SessionViewModelBase>(new RequestModel(RequestKind.Session, ServiceType.Smile));

            VideoManager = new SmileVideoManagerViewModel(Mediator);
            LiveManager = new SmileLiveManagerViewModel(Mediator);
            UsersManager = new SmileUsersManagerViewModel(Mediator);
            ChannelManager = new SmileChannelManagerViewModel(Mediator);
            SettingManager = new SmileSettingManagerViewModel(Mediator);

            WebSiteManager = new SmileWebSiteManagerViewModel(Mediator);

            Mediator.SetManager(ServiceType.Smile, new SmileManagerPackModel(VideoManager, LiveManager, UsersManager, ChannelManager, WebSiteManager, SettingManager));
        }

        #region property

        public SessionViewModelBase Session { get; }

        public SmileVideoManagerViewModel VideoManager { get; set; }
        public SmileLiveManagerViewModel LiveManager { get; }
        public SmileUsersManagerViewModel UsersManager { get; }
        public SmileChannelManagerViewModel ChannelManager { get; }
        public SmileSettingManagerViewModel SettingManager { get; set; }

        public SmileWebSiteManagerViewModel WebSiteManager { get; }

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
                            var inputValue = InputVideoId.Trim();
                            var videoId = SmileIdUtility.GetVideoId(inputValue, Mediator);
                            if(!string.IsNullOrWhiteSpace(videoId)) {
                                OpenVideoPlayerAsync(videoId).ConfigureAwait(false);
                                InputVideoId = string.Empty;
                            }
                        }
                    },
                    o => Session.IsLoggedIn && !string.IsNullOrWhiteSpace(InputVideoId)
                );
            }
        }

        #endregion

        #region function

        async Task<bool> OpenVideoPlayerAsync(string videoId)
        {
            //var videoInformation = await SmileVideoInformationViewModel.CreateFromVideoIdAsync(Mediation, videoId, Constants.ServiceSmileVideoThumbCacheSpan);
            var request = new SmileVideoInformationCacheRequestModel(new SmileVideoInformationCacheParameterModel(videoId, Constants.ServiceSmileVideoThumbCacheSpan));
            var videoInformation = await Mediator.GetResultFromRequest<Task<SmileVideoInformationViewModel>>(request);

            await videoInformation.OpenVideoDefaultAsync(false);
            return true;
        }

        private Task InitializeMarketAsync()
        {
            // 市場専用のマネージャ系がないのでここでキャッシュ破棄する。
            var dirInfo = Mediator.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Smile));
            var cachedDirPath = Path.Combine(dirInfo.FullName, Constants.SmileMarketCacheDirectoryName);
            var marketDir = Directory.CreateDirectory(cachedDirPath);

            return Task.Run(() => {
                var cacheSpan = Constants.ServiceSmileMarketImageCacheSpan;

                var files = marketDir.EnumerateFiles()
                    .Where(f => !cacheSpan.IsCacheTime(f.CreationTime))
                    .ToEvaluatedSequence()
                ;
                foreach(var file in files) {
                    try {
                        file.Delete();
                    } catch(Exception ex) {
                        Mediator.Logger.Warning(ex);
                    }
                }
            });
        }


        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return new ManagerViewModelBase[] {
                UsersManager,
                ChannelManager,
                VideoManager,
                WebSiteManager,
                SettingManager,
            };
        }

        protected override void ShowViewCore()
        { }

        protected override void HideViewCore()
        { }

        public async override Task InitializeAsync()
        {
            await InitializeMarketAsync();

            if(Session.EnabledStartupAutoLogin) {
                if(NetworkUtility.IsNetworkAvailable) {
                    await Session.LoginAsync();
                } else {
                    Mediator.Logger.Information("skip smile login");
                }

                // TODO: ログインできない場合は設定画面へ
                if(Session.LoginState != LoginState.LoggedIn) {

                } else {
                    foreach(var manager in ManagerChildren) {
                        await manager.InitializeAsync();
                    }
                }
            }
        }

        public async override Task UninitializeAsync()
        {
            foreach(var manager in ManagerChildren) {
                await manager.UninitializeAsync().ConfigureAwait(false);
            }

            if(Session.IsLoggedIn) {
                if(NetworkUtility.IsNetworkAvailable) {
                    await Session.LogoutAsync().ConfigureAwait(false);
                }
            }
        }

        public override void InitializeView(MainWindow view)
        {
            foreach(var manager in ManagerChildren) {
                manager.InitializeView(view);
            }
        }

        public override void UninitializeView(MainWindow view)
        {
            foreach(var manager in ManagerChildren) {
                manager.UninitializeView(view);
            }
        }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return Task.WhenAll(ManagerChildren.Select(m => m.GarbageCollectionAsync(garbageCollectionLevel, cacheSpan, force))).ContinueWith(t => {
                return t.Result.Sum();
            });
        }

        #endregion

    }
}
