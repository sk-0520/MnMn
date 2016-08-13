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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.App;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel
{
    public class AppManagerViewModel: ManagerViewModelBase
    {
        #region variable

        WindowState _state = WindowState.Normal;

        #endregion

        public AppManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            Setting = Mediation.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(RequestKind.Setting, ServiceType.Application));

            SmileManager = new SmileManagerViewModel(Mediation);
            AppInformationManager = new AppInformationManagerViewModel(Mediation);
            AppSettingManager = new AppSettingManagerViewModel(Mediation);

            Mediation.SetManager(ServiceType.Application, new ApplicationManagerPackModel(AppSettingManager, SmileManager));

            SmileSession = Mediation.GetResultFromRequest<SessionViewModelBase>(new RequestModel(RequestKind.Session, ServiceType.Smile));
        }

        #region property

        MainWindow View { get; set; }
        AppSettingModel Setting { get; }

        public AppInformationManagerViewModel AppInformationManager { get; }
        public AppSettingManagerViewModel AppSettingManager { get; }
        public SmileManagerViewModel SmileManager { get; }

        public IEnumerable<ManagerViewModelBase> ManagerItems
        {
            get
            {
                return new ManagerViewModelBase[] {
                    AppInformationManager,
                    AppSettingManager,
                    SmileManager
                };
            }
        }

        public SessionViewModelBase SmileSession { get; }

        public WindowState State
        {
            get { return this._state; }
            set { SetVariableValue(ref this._state, value); }
        }
        public double Left
        {
            get { return Setting.Window.Left; }
            set
            {
                if(State == WindowState.Normal) {
                    SetPropertyValue(Setting.Window, value, nameof(Setting.Window.Left));
                }
            }
        }
        public double Top
        {
            get { return Setting.Window.Top; }
            set
            {
                if(State == System.Windows.WindowState.Normal) {
                    SetPropertyValue(Setting.Window, value, nameof(Setting.Window.Top));
                }
            }
        }
        public double Width
        {
            get { return Setting.Window.Width; }
            set
            {
                if(State == WindowState.Normal) {
                    SetPropertyValue(Setting.Window, value, nameof(Setting.Window.Width));
                }
            }
        }
        public double Height
        {
            get { return Setting.Window.Height; }
            set
            {
                if(State == System.Windows.WindowState.Normal) {
                    SetPropertyValue(Setting.Window, value, nameof(Setting.Window.Height));
                }
            }
        }


        #endregion

        #region command

        public ICommand HelpCommand
        {
            get { return CreateCommand(o => { ExecuteCommand.TryExecute(Constants.HelpFilePath); }); }
        }

        public ICommand ExecuteCommand
        {
            get
            {
                return CreateCommand(o => {
                    var s = (string)o;
                    try {
                        Process.Start(s);
                    } catch(Exception ex) {
                        Mediation.Logger.Warning(ex);
                    }
                });
            }
        }

        #endregion

        #region function

        void SaveSetting()
        {
            var dir = VariableConstants.GetSettingDirectory();
            var filePath = Path.Combine(dir.FullName, Constants.SettingFileName);

            SerializeUtility.SaveSetting(filePath, Setting, SerializeFileType.Json, true, Mediation.Logger);
        }

        Updater CheckUpdate(bool force)
        {
            var dir = VariableConstants.GetSettingDirectory();
            var dirPath = Path.Combine(dir.FullName, Constants.ArchiveDirectoryName);

            var IsPause = false;
            var checkRc = false;
            var CheckUpdateRelease = true;
            var updateData = new Updater(dirPath, checkRc, Mediation);
            //CommonData.Logger.Debug(CommonData.Language["log/update/state"], string.Format("force = {0}, setting = {1}", force, CommonData.MainSetting.RunningInformation.CheckUpdateRelease));
            if(force || !IsPause && CheckUpdateRelease) {
                var updateInfo = updateData.Check();
            }
            return updateData;
        }

        #endregion

        #region ManagerViewModelBase

        public override Task InitializeAsync()
        {
            return Task.WhenAll(ManagerItems.Select(m => m.InitializeAsync()));
        }

        public override void InitializeView(MainWindow view)
        {
            View = (MainWindow)view;

            foreach(var manager in ManagerItems) {
                manager.InitializeView(view);
            }

            GarbageCollectionAsync();
        }

        public override void UninitializeView(MainWindow view)
        {
            SaveSetting();

            foreach(var manager in ManagerItems) {
                manager.UninitializeView(view);
            }
        }

        public override Task GarbageCollectionAsync()
        {
            return Task.WhenAll(ManagerItems.Select(m => m.GarbageCollectionAsync()));
        }

        #endregion
    }
}
