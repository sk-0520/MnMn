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
using System.IO.Compression;
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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
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
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App.Update;
using Microsoft.Win32.SafeHandles;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public sealed class AppUpdateManagerViewModel : ManagerViewModelBase
    {
        #region variable

        Uri _archiveUri;
        Version _archiveVersion;
        UpdateCheckState _updateCheckState;
        UpdateCheckState _lightweightUpdateCheckState;

        string _updateText;
        bool _hasUpdate;
        bool _hasLightweightUpdate;
        bool _ruuningUpdate;

        bool _useOldUpdateIssue518;

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

        public UpdateCheckState LightweightUpdateCheckState
        {
            get { return this._lightweightUpdateCheckState; }
            set
            {
                if(SetVariableValue(ref this._lightweightUpdateCheckState, value)) {
                    HasLightweightUpdate = LightweightUpdateCheckState == UpdateCheckState.CurrentIsOld;
                }
            }
        }

        public bool HasUpdate
        {
            get { return this._hasUpdate; }
            set { SetVariableValue(ref this._hasUpdate, value); }
        }

        public bool HasLightweightUpdate
        {
            get { return this._hasLightweightUpdate; }
            set { SetVariableValue(ref this._hasLightweightUpdate, value); }
        }

        public string UpdateText
        {
            get { return this._updateText; }
            set { SetVariableValue(ref this._updateText, value); }
        }

        public LightweightUpdateModel LightweightUpdateModel { get; set; }

        public bool RuuningUpdate
        {
            get { return this._ruuningUpdate; }
            set { SetVariableValue(ref this._ruuningUpdate, value); }
        }

        public bool UseOldUpdateIssue518
        {
            get { return this._useOldUpdateIssue518; }
            set { SetVariableValue(ref this._useOldUpdateIssue518, value); }
        }

        public bool IsEnabledUpdate
        {
            get
            {
                return !VariableConstants.IsSafeModeExecute;
            }
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
                return CreateCommand(
                    o => {
                        Mediation.Order(new AppSaveOrderModel(true));
                        UpdateExecuteAsync().ConfigureAwait(false);
                    },
                    o => !RuuningUpdate && (HasUpdate || HasLightweightUpdate)
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

        async Task CheckLightweightUpdateAsync()
        {
            LightweightUpdateCheckState = UpdateCheckState.UnChecked;
            LightweightUpdateModel = null;

            using(var userAgentHost = new HttpUserAgentHost(NetworkSetting))
            using(var userAgent = userAgentHost.CreateHttpUserAgent()) {
                var setting = Mediation.GetResultFromRequest<AppSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Application));

                try {
                    Mediation.Logger.Information($"lightweight update check: {Constants.AppUriLightweightUpdate}");

                    var stream = await userAgent.GetStreamAsync(Constants.AppUriLightweightUpdate);
                    var model = SerializeUtility.LoadXmlSerializeFromStream<LightweightUpdateModel>(stream);

                    var isUpdateVersion = Constants.ApplicationVersionNumber <= new Version(model.Version);
                    var isUpdateTimestamp = setting.RunningInformation.LightweightUpdateTimestamp < model.Timestamp;

                    if(isUpdateVersion && isUpdateTimestamp) {
                        UpdateText = "TARGET VERSION: " + model.Version;
                        LightweightUpdateModel = model;
                        LightweightUpdateCheckState = UpdateCheckState.CurrentIsOld;
                    } else {
                        LightweightUpdateCheckState = UpdateCheckState.CurrentIsNew;
                    }

                } catch(Exception ex) {
                    Mediation.Logger.Warning(ex);
                    LightweightUpdateCheckState = UpdateCheckState.Error;
                }
            }
        }

        async Task CheckVersionAsync()
        {
            UpdateCheckState = UpdateCheckState.UnChecked;
            LightweightUpdateCheckState = UpdateCheckState.UnChecked;

            ArchiveVersion = null;
            ArchiveUri = null;
            UpdateText = string.Empty;

            LightweightUpdateModel = null;

            try {
                var userAgentHost = new HttpUserAgentHost(NetworkSetting);
                var client = userAgentHost.CreateHttpUserAgent();
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
                    await CheckLightweightUpdateAsync();
                }
            }
        }

        static string GetLightweightUpdateHtmlSource(LightweightUpdateModel model)
        {
            var document = HtmlUtility.CreateHtmlDocument(File.ReadAllText(Constants.ApplicationLightweightUpdateHtmlTemplatePath));

            var timeElement = document.GetElementbyId("timestamp");
            timeElement.SetAttributeValue("timeElement", model.Timestamp.ToString("u"));
            HtmlUtility.CreateTextNode(document, timeElement, model.Timestamp.ToString("yyyy/MM/dd HH:mm"));

            var messageElement = document.GetElementbyId("message");
            var message = string.Join("<br />", model.Message.SplitLines());
            HtmlUtility.CreateTextNode(document, messageElement, message);

            var ulElement = document.GetElementbyId("targets");
            foreach(var target in model.Targets) {
                var liElement = HtmlUtility.CreateChildElement(document, ulElement, "li");
                var aElement = HtmlUtility.CreateChildElement(document, liElement, "a");
                HtmlUtility.CreateTextNode(document, aElement, target.Extends["expand"]);
                aElement.SetAttributeValue("href", target.Extends["view"]);
                aElement.SetAttributeValue("target", "MNMN_SOURCE");
            }

            return document.DocumentNode.OuterHtml;
        }

        Task LoadChangelogAsync()
        {
            if(UpdateCheckState == UpdateCheckState.CurrentIsOld) {
                var userAgentHost = new HttpUserAgentHost(NetworkSetting);
                var client = userAgentHost.CreateHttpUserAgent();
                return client.GetStringAsync(Constants.AppUriChangelogRelease).ContinueWith(t => {
                    var htmlSource = t.Result;
                    UpdateBrowser.Dispatcher.BeginInvoke(new Action(() => {
                        UpdateBrowser.LoadHtml(htmlSource);
                    }));
                });
            } else if(LightweightUpdateCheckState == UpdateCheckState.CurrentIsOld) {
                var htmlSource = GetLightweightUpdateHtmlSource(LightweightUpdateModel);
                UpdateBrowser.Dispatcher.BeginInvoke(new Action(() => {
                    UpdateBrowser.LoadHtml(htmlSource);
                }));
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
                { "platform", Environment.Is64BitProcess ? "x64": "x86" },
            };

            foreach(var pair in map) {
                defaultMap[pair.Key] = pair.Value;
            }
            startInfo.Arguments = string.Join(" ", defaultMap.Select(p => string.Format("\"/{0}={1}\"", p.Key, p.Value)));

            return process;
        }

        Task<UpdatedResult> AppUpdateExecuteAsync()
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
                var result = UpdatedResult.None;
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
                        result = UpdatedResult.Exit;
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

        async Task UpdateExecuteAsync()
        {
            RuuningUpdate = true;

            Task<UpdatedResult> task;
            if(HasUpdate) {
                if(!UseOldUpdateIssue518) {
                    var archiveDir = VariableConstants.GetArchiveDirectory();
                    var archivePath = Path.Combine(archiveDir.FullName, ArchiveUri.Segments.Last());
                    var archiveFile = new FileInfo(archivePath);

                    // #158
                    FileUtility.RotateFiles(archiveDir.FullName, Constants.ArchiveSearchPattern, ContentTypeTextNet.Library.SharedLibrary.Define.OrderBy.Descending, Constants.BackupArchiveCount, e => {
                        Mediation.Logger.Warning(e);
                        return true;
                    });

                    var download = new AppUpdateDownloadItemViewModel(Mediation, ArchiveUri, archiveFile, new HttpUserAgentHost(NetworkSetting));
                    Mediation.Order(new DownloadOrderModel(download, false, ServiceType.Application));

                    task = download.StartAsync().ContinueWith(t => {
                        return UpdatedResult.None;
                    });
                } else {
                    task = AppUpdateExecuteAsync();
                }
            } else if(HasLightweightUpdate) {
                //task =  EazyUpdateExecuteAsync();

                var download = new LightweightUpdateDownloadItemViewModel(Mediation, LightweightUpdateModel);
                Mediation.Order(new DownloadOrderModel(download, false, ServiceType.Application));

                task = download.StartAsync().ContinueWith(t => {
                    return UpdatedResult.None;
                });
            } else {
                task = Task.FromResult(UpdatedResult.None);
            }

            try {
                var result = await task;
                switch(result) {
                    case UpdatedResult.Exit:
                        Mediation.Order(new OrderModel(OrderKind.Exit, ServiceType.Application));
                        break;

                    case UpdatedResult.Reboot:
                        Mediation.Order(new OrderModel(OrderKind.Reboot, ServiceType.Application));
                        break;

                    case UpdatedResult.None:
                        break;

                    default:
                        throw new NotImplementedException();
                }
            } catch(Exception ex) {
                Mediation.Logger.Error(ex);
            } finally {
                RuuningUpdate = false;
            }
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
                    if(UpdateCheckState != UpdateCheckState.CurrentIsOld && LightweightUpdateCheckState != UpdateCheckState.CurrentIsOld) {
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
            if(VariableConstants.IsSafeModeExecute) {
                return Task.CompletedTask;
            }

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
