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
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Order;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using Microsoft.Win32.SafeHandles;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public sealed class AppUpdateManagerViewModel : ManagerViewModelBase
    {
        #region variable

        Uri _archiveUri;
        Version _archiveVersion;
        UpdateCheckState _updateCheckState;
        UpdateCheckState _eazyUpdateCheckState;

        string _updateText;
        bool _hasUpdate;
        bool _hasEasyUpdate;

        #endregion

        public AppUpdateManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            BackgroundUpdateCheckTimer = new DispatcherTimer() {
                Interval = Constants.BackgroundUpdateCheckTime,
            };
            BackgroundUpdateCheckTimer.Tick += BackgroundUpdateCheckTimer_Tick;
        }

        #region property

        DispatcherTimer BackgroundUpdateCheckTimer { get; }

        WebNavigator UpdateBrowser { get; set; }

        public Uri ArchiveUri
        {
            get { return this._archiveUri; }
            set { SetVariableValue(ref this._archiveUri, value); }
        }

        public Version ArchiveVersion
        {
            get { return this._archiveVersion; }
            set { SetVariableValue(ref this._archiveVersion, value); }
        }

        public UpdateCheckState UpdateCheckState
        {
            get { return this._updateCheckState; }
            set
            {
                if(SetVariableValue(ref this._updateCheckState, value)) {
                    HasUpdate = UpdateCheckState == UpdateCheckState.CurrentIsOld;
                }
            }
        }

        public UpdateCheckState EazyUpdateCheckState
        {
            get { return this._eazyUpdateCheckState; }
            set
            {
                if(SetVariableValue(ref this._eazyUpdateCheckState, value)) {
                    HasEasyUpdate = EazyUpdateCheckState == UpdateCheckState.CurrentIsOld;
                }
            }
        }

        public bool HasUpdate
        {
            get { return this._hasUpdate; }
            set { SetVariableValue(ref this._hasUpdate, value); }
        }

        public bool HasEasyUpdate
        {
            get { return this._hasEasyUpdate; }
            set { SetVariableValue(ref this._hasEasyUpdate, value); }
        }

        public string UpdateText
        {
            get { return this._updateText; }
            set { SetVariableValue(ref this._updateText, value); }
        }

        public EazyUpdateModel EazyUpdateModel { get; set; }

        #endregion

        #region command

        public ICommand UpdateCheckCommand
        {
            get { return CreateCommand(o => UpdateCheckAsync().ConfigureAwait(false)); }
        }

        public ICommand UpdateExecuteCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        Mediation.Order(new AppSaveOrderModel(true));
                        UpdateExecuteAsunc().ContinueWith(t => {
                            if(t.Result) {
                                Mediation.Order(new Model.Request.OrderModel(OrderKind.Exit, ServiceType.Application));
                            }
                        });
                    },
                    o => HasUpdate
                );
            }
        }

        public ICommand NewWindowCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var data = (WebNavigatorEventDataBase)o;
                        WebNavigatorUtility.OpenNewWindowWrapper(data, Mediation.Logger);
                    }
                );
            }
        }

        #endregion

        #region function

        async Task CheckEazyUpdateAsync()
        {
            EazyUpdateCheckState = UpdateCheckState.UnChecked;
            EazyUpdateModel = null;

            using(var userAgentHost = new HttpUserAgentHost())
            using(var userAgent = userAgentHost.CreateHttpUserAgent()) {
                var setting = Mediation.GetResultFromRequest<AppSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Application));

                try {
                    Mediation.Logger.Information($"eazy update check: {Constants.AppUriEazyUpdate}");

                    var stream = await userAgent.GetStreamAsync(Constants.AppUriEazyUpdate);
                    var model = SerializeUtility.LoadXmlSerializeFromStream<EazyUpdateModel>(stream);

                    var isUpdateVersion = Constants.ApplicationVersionNumber <= new Version(model.Version);
                    var isUpdateTimestamp = setting.RunningInformation.LastEazyUpdateTimestamp < model.Timestamp;

                    if(isUpdateVersion && isUpdateTimestamp) {
                        UpdateText = model.Version + " - " + model.Version;
                        EazyUpdateModel = model;
                        EazyUpdateCheckState = UpdateCheckState.CurrentIsOld;
                    } else {
                        EazyUpdateCheckState = UpdateCheckState.CurrentIsNew;
                    }

                } catch(Exception ex) {
                    Mediation.Logger.Warning(ex);
                    EazyUpdateCheckState = UpdateCheckState.Error;
                }
            }
        }

        async Task CheckVersionAsync()
        {
            UpdateCheckState = UpdateCheckState.UnChecked;
            EazyUpdateCheckState = UpdateCheckState.UnChecked;

            ArchiveVersion = null;
            ArchiveUri = null;
            UpdateText = string.Empty;

            EazyUpdateModel = null;

            try {
                var client = new HttpClient();
                Mediation.Logger.Trace("update check: " + Constants.AppUriUpdate);
                var response = await client.GetAsync(Constants.AppUriUpdate);

                Mediation.Logger.Trace("update response state: " + response.StatusCode);
                if(!response.IsSuccessStatusCode) {
                    UpdateCheckState = UpdateCheckState.Error;
                    return;
                }

                var resultText = await response.Content.ReadAsStringAsync();

                var xml = XElement.Parse(resultText);

                var item = xml
                    .Elements()
                    .Select(
                        x => new {
                            Version = new Version(x.Attribute("version").Value),
                            //IsRC = x.Attribute("type").Value == "rc",
                            ArchiveElements = x.Elements(),
                        }
                    )
                    .Where(x => Constants.ApplicationVersionNumber <= x.Version)
                    .OrderByDescending(x => x.Version)
                    .FirstOrDefault()
                ;

                if(item == null) {
                    UpdateCheckState = UpdateCheckState.CurrentIsNew;
                    return;
                }

                var archive = item.ArchiveElements
                    .Select(x => new {
                        Uri = new Uri(x.Attribute("uri").Value),
                        Platform = x.Attribute("platform").Value,
                        Version = item.Version,
                    })
                    .FirstOrDefault(x => x.Platform == (Environment.Is64BitProcess ? "x64" : "x86"))
                ;

                if(archive == null) {
                    UpdateCheckState = UpdateCheckState.CurrentIsNew;
                    return;
                }

                ArchiveUri = archive.Uri;
                ArchiveVersion = archive.Version;

                var map = new Dictionary<string, string>() {
                    { "NEW-VERSION", ArchiveVersion.ToString() },
                    { "NOW-VERSION", Constants.ApplicationVersionNumber.ToString() },
                };
                UpdateText = AppUtility.ReplaceString(Properties.Resources.String_App_Update_Format, map);

                UpdateCheckState = UpdateCheckState.CurrentIsOld;
            } catch(Exception ex) {
                Mediation.Logger.Warning(ex);
                UpdateCheckState = UpdateCheckState.Error;
            } finally {
                // ぶっこんだなぁ
                if(!HasUpdate) {
                    await CheckEazyUpdateAsync();
                }
            }
        }

        Task LoadChangelogAsync()
        {
            if(UpdateCheckState == UpdateCheckState.CurrentIsOld) {
                var client = new HttpClient();
                return client.GetStringAsync(Constants.AppUriChangelogRelease).ContinueWith(t => {
                    var htmlSource = t.Result;
                    UpdateBrowser.Dispatcher.BeginInvoke(new Action(() => {
                        UpdateBrowser.LoadHtml(htmlSource);
                    }));
                });
            } else if(EazyUpdateCheckState == UpdateCheckState.CurrentIsOld) {

            }

            return Task.CompletedTask;
        }

        Task SetUpdateStateViewAsync()
        {
            if(UpdateBrowser == null) {
                return Task.CompletedTask;
            }

            return UpdateBrowser.Dispatcher.BeginInvoke(new Action(() => {
                var rawHtmlSource = Properties.Resources.File_Html_UpdateState;
                var map = new Dictionary<string, string>() {
                    {"UPDATE-TITLE", Properties.Resources.String_App_Update_Title },
                    {"UPDATE-CONTENT",  DisplayTextUtility.GetDisplayText(UpdateCheckState) },
                    {"UPDATE-TIMESTAMP",  DateTime.Now.ToString("s") },
                };
                var htmlSource = AppUtility.ReplaceString(rawHtmlSource, map);
                UpdateBrowser.LoadHtml(htmlSource);
            })).Task;
        }

        Task UpdateCheckAsync()
        {
            BackgroundUpdateCheckTimer.Stop();

            return CheckVersionAsync().ContinueWith(t => {
                return LoadChangelogAsync().ContinueWith(_ => {
                    BackgroundUpdateCheckTimer.Start();
                });
            }, TaskContinuationOptions.AttachedToParent);
        }

        Process CreateProcess(Dictionary<string, string> map)
        {
            var process = new Process();
            var startInfo = process.StartInfo;
            startInfo.FileName = Constants.UpdaterExecuteFilePath;

            var defaultMap = new Dictionary<string, string>() {
                { "pid",      string.Format("{0}", Process.GetCurrentProcess().Id) },
                { "version",  Constants.ApplicationVersionNumber.ToString() },
                //{ "uri",      Constants.UriUpdate },
                { "platform", Environment.Is64BitProcess ? "x64": "x86" },
                //{ "rc",       this._donwloadRc ? "true": "false" },
            };

            foreach(var pair in map) {
                defaultMap[pair.Key] = pair.Value;
            }
            startInfo.Arguments = string.Join(" ", defaultMap.Select(p => string.Format("\"/{0}={1}\"", p.Key, p.Value)));

            return process;
        }

        Task<bool> UpdateExecuteAsunc()
        {
            var eventName = "mnmn-event";

            var archiveDir = VariableConstants.GetArchiveDirectory();

            var lines = new List<string>();
            var map = new Dictionary<string, string>() {
                { "download",       archiveDir.FullName },
                { "expand",         Constants.AssemblyRootDirectoryPath },
                { "wait",           "true" },
                { "no-wait-update", "true" },
                { "event",           eventName },
                { "script",          Path.Combine(Constants.ApplicationEtcDirectoryPath, Constants.ScriptDirectoryName, "Updater", "UpdaterScript.cs") },
                { "uri",             ArchiveUri.OriginalString },
            };

            // #158
            FileUtility.RotateFiles(archiveDir.FullName, Constants.ArchiveSearchPattern, ContentTypeTextNet.Library.SharedLibrary.Define.OrderBy.Descending, Constants.BackupArchiveCount, e => {
                Mediation.Logger.Warning(e);
                return true;
            });

            var waitEvent = new EventWaitHandle(false, EventResetMode.AutoReset, eventName);
            var process = CreateProcess(map);
            Mediation.Logger.Information("update exec", process.StartInfo.Arguments);

            process.Start();
            return Task.Run(() => {
                var result = false;
                var processEvent = new EventWaitHandle(false, EventResetMode.AutoReset) {
                    SafeWaitHandle = new SafeWaitHandle(process.Handle, false),
                };
                var handles = new[] { waitEvent, processEvent };
                var waitResult = WaitHandle.WaitAny(handles, Constants.UpdateAppExitWaitTime);

                Mediation.Logger.Debug("WaitHandle.WaitAny", waitResult);
                if(0 <= waitResult && waitResult < handles.Length) {
                    if(handles[waitResult] == waitEvent) {
                        // イベントが立てられたので終了
                        Mediation.Logger.Information("exit", process.StartInfo.Arguments);
                        result = true;
                    } else if(handles[waitResult] == processEvent) {
                        // Updaterがイベント立てる前に死んだ
                        Mediation.Logger.Information("error-process", process.ExitCode);
                    }
                } else {
                    // タイムアウト
                    if(!process.HasExited) {
                        // まだ生きてるなら強制的に殺す
                        process.Kill();
                    }
                    Mediation.Logger.Information("error-timeout", process.ExitCode);
                }

                return result;
            });
        }


        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            if(UpdateBrowser.IsEmptyContent) {
                LoadChangelogAsync().ContinueWith(_ => {
                    if(UpdateCheckState != UpdateCheckState.CurrentIsOld) {
                        SetUpdateStateViewAsync().ConfigureAwait(false);
                    }
                }).ConfigureAwait(false);
            }
        }

        protected override void HideViewCore()
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return CheckVersionAsync().ContinueWith(t => {
                BackgroundUpdateCheckTimer.Start();
            });
        }

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
            UpdateBrowser = view.updateBrowser;
        }

        public override void UninitializeView(MainWindow view)
        {
            //throw new NotImplementedException();
        }


        #endregion

        private void BackgroundUpdateCheckTimer_Tick(object sender, EventArgs e)
        {
            UpdateCheckAsync().ConfigureAwait(false);
        }

    }
}
