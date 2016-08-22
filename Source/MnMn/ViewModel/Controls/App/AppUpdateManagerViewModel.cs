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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Order;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using Microsoft.Win32.SafeHandles;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public sealed class AppUpdateManagerViewModel: ManagerViewModelBase
    {
        #region variable

        Uri _archiveUri;
        Version _archiveVersion;
        UpdateCheckState _updateCheckState;

        string _updateText;
        bool _hasUpdate;

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

        WebBrowser UpdateBrowser { get; set; }

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

        public bool HasUpdate
        {
            get { return this._hasUpdate; }
            set { SetVariableValue(ref this._hasUpdate, value); }
        }

        public string UpdateText
        {
            get { return this._updateText; }
            set { SetVariableValue(ref this._updateText, value); }
        }
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
                return CreateCommand(o => {
                    Mediation.Order(new AppSaveOrderModel(true));
                    UpdateExecuteAsunc().ContinueWith(t => {
                        if(t.Result) {
                            Mediation.Order(new Model.Request.OrderModel(OrderKind.Exit, ServiceType.Application));
                        }
                    });
                });
            }
        }
        #endregion

        #region function

        async Task CheckVersionAsync()
        {
            UpdateCheckState = UpdateCheckState.UnChecked;
            ArchiveVersion = null;
            ArchiveUri = null;
            UpdateText = string.Empty;

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
            }
        }

        Task LoadChangelogAsync()
        {
            if(UpdateCheckState == UpdateCheckState.CurrentIsOld) {
                var client = new HttpClient();
                return client.GetStringAsync(Constants.AppUriChangelogRelease).ContinueWith(t => {
                    var htmlSource = t.Result;
                    UpdateBrowser.Dispatcher.BeginInvoke(new Action(() => {
                        UpdateBrowser.NavigateToString(htmlSource);
                    }));
                });
            } else {
                return UpdateBrowser.Dispatcher.BeginInvoke(new Action(() => {
                    UpdateBrowser.NavigateToString("<html><head></head><body></body></html>");
                })).Task;
            }
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
            //FileUtility.MakeFileParentDirectory(archiveDirPath);
            //if(!Directory.Exists(archiveDirPath)) {
            //    Directory.CreateDirectory(archiveDirPath);
            //}
            // #158
            FileUtility.RotateFiles(archiveDir.FullName, Constants.ArchiveSearchPattern, ContentTypeTextNet.Library.SharedLibrary.Define.OrderBy.Descending, Constants.BackupArchiveCount, e => {
                Mediation.Logger.Warning(e);
                return true;
            });

            //var pipe = new NamedPipeServerStream(pipeName, PipeDirection.In);
            var waitEvent = new EventWaitHandle(false, EventResetMode.AutoReset, eventName);
            var process = CreateProcess(map);
            //this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/exec"], process.StartInfo.Arguments);
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

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return CheckVersionAsync().ContinueWith(t => {
                BackgroundUpdateCheckTimer.Start();
            });
        }

        public override void InitializeView(MainWindow view)
        {
            UpdateBrowser = view.updateBrowser;
        }

        public override void UninitializeView(MainWindow view)
        {
            //throw new NotImplementedException();
        }

        protected override void ShowView()
        {
            if(UpdateBrowser.Source == null) {
                LoadChangelogAsync().ConfigureAwait(false);
            }
        }

        #endregion

        private void BackgroundUpdateCheckTimer_Tick(object sender, EventArgs e)
        {
            UpdateCheckAsync().ConfigureAwait(false);
        }

    }
}
