﻿/*
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
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
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
    public class AppManagerViewModel : ManagerViewModelBase
    {
        #region variable

        WindowState _state = WindowState.Normal;

        #endregion

        public AppManagerViewModel(Mediator mediator, AppLogger appLogger)
            : base(mediator)
        {
            Setting = Mediator.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(RequestKind.Setting, ServiceType.Application));

            SmileManager = new SmileManagerViewModel(Mediator);
            AppUpdateManager = new AppUpdateManagerViewModel(Mediator);
            AppInformationManager = new AppInformationManagerViewModel(Mediator, appLogger);
            AppBrowserManager = new AppBrowserManagerViewModel(Mediator);
            AppDownloadManager = new AppDownloadManagerViewModel(Mediator);

            Mediator.SetManager(ServiceType.Application, new ApplicationManagerPackModel(this, SmileManager));

            SmileSession = Mediator.GetResultFromRequest<SessionViewModelBase>(new RequestModel(RequestKind.Session, ServiceType.Smile));

            BackgroundAutoSaveTimer.Tick += AutoSaveTimer_Tick;
            BackgroundGarbageCollectionTimer.Tick += BackgroundGarbageCollectionTimer_Tick;
            AutoRebootWatchTimer.Tick += AutoRebootWatchTimer_Tick;
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
        DispatcherTimer AutoRebootWatchTimer { get; } = new DispatcherTimer() {
            Interval = Constants.AutoRebootWatchTime,
        };

        /// <summary>
        /// 本体が起動した時点の Windows 稼働時間。
        /// <para>BUGS: オーバーフロー。</para>
        /// </summary>
        TimeSpan StartupTime { get; } = TimeSpan.FromMilliseconds(NativeMethods.GetTickCount());

        public AppUpdateManagerViewModel AppUpdateManager { get; }
        public AppInformationManagerViewModel AppInformationManager { get; }
        public AppBrowserManagerViewModel AppBrowserManager { get; }
        public AppDownloadManagerViewModel AppDownloadManager { get; }

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

        public double ViewScale
        {
            get { return Setting.ViewScale; }
            set
            {
                if(SetPropertyValue(Setting, value)) {
                    WebNavigatorUtility.ApplyWebNavigatorScale(View, ViewScale);
                }
            }
        }


        #endregion

        #region command

        #endregion

        #region function

        IEnumerable<ViewModelBase> GetChildWindowViewModels()
        {
            var videoPlayers = Mediator.GetResultFromRequest<IEnumerable<ViewModelBase>>(new RequestModel(RequestKind.WindowViewModels, ServiceType.SmileVideo));
            var livePlayers = Mediator.GetResultFromRequest<IEnumerable<ViewModelBase>>(new RequestModel(RequestKind.WindowViewModels, ServiceType.SmileLive));

            var windows = new[] {
                videoPlayers,
                livePlayers,
            }.SelectMany(i => i);

            return windows;
        }

        void OutputLogGarbageCollection(long gcSize)
        {
            Mediator.Logger.Information($"Storage GC: {RawValueUtility.ConvertHumanLikeByte(gcSize)}", $"{gcSize:n0} byte");
        }

        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return new ManagerViewModelBase[] {
                AppUpdateManager,
                AppInformationManager,
                AppBrowserManager,
                AppDownloadManager,
                SmileManager,
            };
        }

        protected override void ShowViewCore()
        { }

        protected override void HideViewCore()
        { }

        public override Task InitializeAsync()
        {
            return Task.WhenAll(ManagerChildren.Select(m => m.InitializeAsync())).ContinueWith(_ => {
                if(Constants.AutoRebootIsEnabled) {
                    AutoRebootWatchTimer.Start();
                }
            });
        }

        public override Task UninitializeAsync()
        {
            return Task.WhenAll(ManagerChildren.Select(m => m.UninitializeAsync()));
        }

        public override void InitializeView(MainWindow view)
        {
            View = (MainWindow)view;

            foreach(var manager in ManagerChildren) {
                manager.InitializeView(view);
            }

            WebNavigatorUtility.ApplyWebNavigatorScale(View, ViewScale);

            BackgroundAutoSaveTimer.Start();

            if(Constants.BackgroundGarbageCollectionIsEnabledStartup) {
                // GCは裏で走らせておく
                GarbageCollectionAsync(GarbageCollectionLevel.Large, new CacheSpan(DateTime.Now, Setting.CacheLifeTime), false).ContinueWith(t => {
                    OutputLogGarbageCollection(t.Result);
                    Mediator.Order(new AppCleanMemoryOrderModel(true, true));
                    BackgroundGarbageCollectionTimer.Start();
                });
            } else {
                BackgroundGarbageCollectionTimer.Start();
            }

            View.Closing += View_Closing;
            View.Closed += View_Closed;

            Mediator.Order(new AppCleanMemoryOrderModel(false, false));
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

        private void View_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var closeItems = GetChildWindowViewModels().Cast<ICloseView>();

            foreach(var closeItem in closeItems) {
                closeItem.Close();
            }

            Mediator.Order(new AppSaveOrderModel(true));
        }


        private void View_Closed(object sender, EventArgs e)
        {
            BackgroundAutoSaveTimer.Stop();
            BackgroundAutoSaveTimer.Tick -= AutoSaveTimer_Tick;

            BackgroundGarbageCollectionTimer.Stop();
            BackgroundGarbageCollectionTimer.Tick -= BackgroundGarbageCollectionTimer_Tick;

            AutoRebootWatchTimer.Stop();
            AutoRebootWatchTimer.Tick -= AutoRebootWatchTimer_Tick;

            View.Closing -= View_Closing;
            View.Closed -= View_Closed;
        }

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            Mediator.Logger.Debug($"timer: {sender}, {e}");
            try {
                BackgroundAutoSaveTimer.Stop();
                Mediator.Order(new AppSaveOrderModel(false));
            } finally {
                BackgroundAutoSaveTimer.Start();
            }
        }

        private async void BackgroundGarbageCollectionTimer_Tick(object sender, EventArgs e)
        {
            Mediator.Logger.Debug($"timer: {sender}, {e}");
            try {
                BackgroundGarbageCollectionTimer.Stop();

                var cacheSpan = new CacheSpan(DateTime.Now, Setting.CacheLifeTime);
                var gcSize = await GarbageCollectionAsync(Constants.BackgroundGarbageCollectionLevel, cacheSpan, false);
                OutputLogGarbageCollection(gcSize);
                Mediator.Order(new AppCleanMemoryOrderModel(true, true));
            } finally {
                BackgroundGarbageCollectionTimer.Start();
            }
        }

        private void AutoRebootWatchTimer_Tick(object sender, EventArgs e)
        {
            AutoRebootWatchTimer.Stop();

            var reboot = false;

            try {
                //if(!Constants.AutoRebootIsEnabled) {
                //    return;
                //}

                var lastInputInfo = new LASTINPUTINFO() {
                    cbSize = (uint)LASTINPUTINFO.SizeOf,
                };
                if(!NativeMethods.GetLastInputInfo(ref lastInputInfo)) {
                    return;
                }

                var lastInputTime = TimeSpan.FromMilliseconds(lastInputInfo.dwTime);
                var nowRunningTime = TimeSpan.FromMilliseconds(NativeMethods.GetTickCount());

                if(lastInputTime < nowRunningTime) {
                    // 再起動後に最終入力時間見ると連続で再起動するからプログラム起動時間に補正してあげる
                    var useLastInputTime = StartupTime < lastInputTime ? lastInputTime : StartupTime;
                    var elapsedTime = nowRunningTime - useLastInputTime;
                    if(Constants.AutoRebootJudgeTime < elapsedTime) {
                        reboot = true;
                    }
                }
            } finally {
                if(!reboot) {
                    AutoRebootWatchTimer.Start();
                }
            }

            if(reboot) {
                Mediator.Logger.Information("reboot!");
                Mediator.Order(new OrderModel(OrderKind.Reboot, ServiceType.Application));
            }
        }

    }
}
