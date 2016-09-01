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
using System.Windows.Threading;
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
using ContentTypeTextNet.MnMn.MnMn.Model.Order;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.App;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Service.Smile;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel
{
    public class AppManagerViewModel: ManagerViewModelBase
    {
        #region variable

        WindowState _state = WindowState.Normal;

        #endregion

        public AppManagerViewModel(Mediation mediation, AppLogger appLogger)
            : base(mediation)
        {
            Setting = Mediation.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(RequestKind.Setting, ServiceType.Application));

            SmileManager = new SmileManagerViewModel(Mediation);
            AppUpdateManager = new AppUpdateManagerViewModel(Mediation);
            AppInformationManager = new AppInformationManagerViewModel(Mediation, appLogger);
            AppSettingManager = new AppSettingManagerViewModel(Mediation);

            Mediation.SetManager(ServiceType.Application, new ApplicationManagerPackModel(AppSettingManager, SmileManager));

            SmileSession = Mediation.GetResultFromRequest<SessionViewModelBase>(new RequestModel(RequestKind.Session, ServiceType.Smile));

            BackgroundAutoSaveTimer.Tick += AutoSaveTimer_Tick;
            BackgroundGarbageCollectionTimer.Tick += BackgroundGarbageCollectionTimer_Tick;
        }

        #region property

        MainWindow View { get; set; }
        AppSettingModel Setting { get; }

        DispatcherTimer BackgroundAutoSaveTimer { get; } = new DispatcherTimer() {
            Interval = Constants.BackgroundAutoSaveSettingTime,
        };
        DispatcherTimer BackgroundGarbageCollectionTimer { get; } = new DispatcherTimer() {
            Interval = Constants.BackgroundGarbageCollectionTime,
        };

        public AppUpdateManagerViewModel AppUpdateManager { get; }
        public AppInformationManagerViewModel AppInformationManager { get; }
        public AppSettingManagerViewModel AppSettingManager { get; }
        public SmileManagerViewModel SmileManager { get; }

        public SessionViewModelBase SmileSession { get; }

        #region window
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

        #endregion

        #region command

        #endregion

        #region function

        //void SaveSetting()
        //{
        //    var dir = VariableConstants.GetSettingDirectory();
        //    var filePath = Path.Combine(dir.FullName, Constants.SettingFileName);

        //    SerializeUtility.SaveSetting(filePath, Setting, SerializeFileType.Json, true, Mediation.Logger);
        //}
        IEnumerable<ViewModelBase> GetChildWindowViewModels()
        {
            var players = Mediation.GetResultFromRequest<IEnumerable<ViewModelBase>>(new RequestModel(RequestKind.WindowViewModels, ServiceType.SmileVideo));

            var windows = new[] {
                players,
            }.SelectMany(i => i);

            return windows;
        }

        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return new ManagerViewModelBase[] {
                AppUpdateManager,
                AppInformationManager,
                AppSettingManager,
                SmileManager
            };
        }

        protected override void ShowViewCore()
        { }

        protected override void HideViewCore()
        { }

        public override Task InitializeAsync()
        {
            return Task.WhenAll(ManagerChildren.Select(m => m.InitializeAsync()));
        }

        public override void InitializeView(MainWindow view)
        {
            View = (MainWindow)view;

            foreach(var manager in ManagerChildren) {
                manager.InitializeView(view);
            }

            BackgroundAutoSaveTimer.Start();

            // GCは裏で走らせておく
            GarbageCollectionAsync(GarbageCollectionLevel.Large, new CacheSpan(DateTime.Now, Setting.CacheLifeTime)).ContinueWith(t => {
                var gcSize = t.Result;
                Mediation.Logger.Information($"GC: {gcSize:n0} byte");
                Mediation.Order(new AppCleanMemoryOrderModel(true));
                BackgroundGarbageCollectionTimer.Start();
            });

            View.UserClosing += View_UserClosing;
            View.Closing += View_Closing;
            View.Closed += View_Closed;

            Mediation.Order(new AppCleanMemoryOrderModel(false));
        }

        public override void UninitializeView(MainWindow view)
        {
            foreach(var manager in ManagerChildren) {
                manager.UninitializeView(view);
            }
        }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan)
        {
            return Task.WhenAll(ManagerChildren.Select(m => m.GarbageCollectionAsync(garbageCollectionLevel, cacheSpan))).ContinueWith(t => {
                return t.Result.Sum();
            });
        }

        #endregion

        private void View_UserClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var viewModels = GetChildWindowViewModels();

            if(viewModels.Any()) {
                // TODO: 将来何かに使いましょうさ
                //e.Cancel = true;
            }

            if(!e.Cancel) {
                Mediation.Order(new AppSaveOrderModel(true));
            }
        }

        private void View_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var closeItems = GetChildWindowViewModels().Cast<ICloseView>();

            foreach(var closeItem in closeItems) {
                closeItem.Close();
            }
        }


        private void View_Closed(object sender, EventArgs e)
        {
            BackgroundAutoSaveTimer.Stop();
            BackgroundAutoSaveTimer.Tick -= AutoSaveTimer_Tick;

            BackgroundGarbageCollectionTimer.Stop();
            BackgroundGarbageCollectionTimer.Tick -= BackgroundGarbageCollectionTimer_Tick;

            View.UserClosing -= View_UserClosing;
            View.Closing -= View_Closing;
            View.Closed -= View_Closed;
        }

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            Mediation.Logger.Debug($"timer: {sender}, {e}");
            try {
                BackgroundAutoSaveTimer.Stop();
                Mediation.Order(new AppSaveOrderModel(false));
            } finally {
                BackgroundAutoSaveTimer.Start();
            }
        }

        private async void BackgroundGarbageCollectionTimer_Tick(object sender, EventArgs e)
        {
            Mediation.Logger.Debug($"timer: {sender}, {e}");
            try {
                BackgroundGarbageCollectionTimer.Stop();

                var cacheSpan = new CacheSpan(DateTime.Now, Setting.CacheLifeTime);
                var gcSize = await GarbageCollectionAsync(Constants.BackgroundGarbageCollectionLevel, cacheSpan);
                Mediation.Logger.Information($"GC: {gcSize:n0} byte");
                Mediation.Order(new AppCleanMemoryOrderModel(true));
            } finally {
                BackgroundGarbageCollectionTimer.Start();
            }
        }


    }
}
