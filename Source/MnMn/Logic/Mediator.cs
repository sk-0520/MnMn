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
using System.Net.Http.Headers;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Data.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.ProcessLink;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.IdleTalk;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Order;
using ContentTypeTextNet.MnMn.MnMn.Model.Order.AppProcessLink;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.WebNavigator;
using ContentTypeTextNet.MnMn.MnMn.Model.Response;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// データ連携等々の橋渡し。
    /// </summary>
    public class Mediator : MediatorBase
    {
        #region variable

        ILogger _logger;

        #endregion

        public Mediator(AppSettingModel mainSettingModel, ILogger logger)
        {
            this._logger = logger;
            Debug.Listeners.Add(new LogListener(Logger, LogKind.Debug));

            Logger.LoggerConfig.EnabledAll = true;
            Logger.LoggerConfig.PutsDebug = true;
            Logger.LoggerConfig.PutsConsole = true;
            Logger.Information("start!");

            Script = CreateScript();

            WebNavigatorBridge = LoadModelFromFile<WebNavigatorBridgeModel>(Constants.ApplicationWebNavigatorBridgePath);
            WebNavigatorNavigatingItems = WebNavigatorBridge.Navigating.Items
                .Select(i => new WebNavigatorNavigatingItemViewModel(i))
                .ToEvaluatedSequence()
            ;

            WebNavigatorContextMenuItems = WebNavigatorBridge.ContextMenu.Items
                .Select(i => new WebNavigatorContextMenuItemViewModel(i))
                .ToEvaluatedSequence()
            ;
            WebNavigatorContextMenuMap = WebNavigatorContextMenuItems.ToDictionary(i => i.Key, i => i);

            Setting = mainSettingModel;
            Common = new CommonMediator(this);
            Smile = new SmileMediator(this, Setting.ServiceSmileSetting);
            IdleTalk = new IdleTalkMediator(this);

            ProcessLinkerHost = new ProcessLinkHost(this);
        }

        #region property

        AppSettingModel Setting { get; }

        ProcessLinkHost ProcessLinkerHost { get; }

        IReadOnlyWebNavigatorBridge WebNavigatorBridge { get; }
        IReadOnlyList<WebNavigatorNavigatingItemViewModel> WebNavigatorNavigatingItems { get; }
        IReadOnlyList<WebNavigatorContextMenuItemViewModel> WebNavigatorContextMenuItems { get; }
        IReadOnlyDictionary<string, WebNavigatorContextMenuItemViewModel> WebNavigatorContextMenuMap { get; }

        CommonMediator Common { get; }

        /// <summary>
        /// ニコニコ関係。
        /// </summary>
        internal SmileMediator Smile { get; private set; }

        internal IdleTalkMediator IdleTalk { get; }

        internal ApplicationManagerPackModel ManagerPack { get; private set; }

        /// <summary>
        /// スリープ・ロック抑制を最後に実施した時間。
        /// </summary>
        DateTime LastSystemBreakSuppressionTime { get; set; } = DateTime.MinValue;
        int CalledEmptyWorkingSetCount { get; set; }

        #endregion

        #region function

        private ResponseModel Request_CacheDirectoryCore(RequestModel request)
        {
            Debug.Assert(request.RequestKind == RequestKind.CacheDirectory);

            var map = new Dictionary<ServiceType, IEnumerable<string>>() {
                {ServiceType.Application, new string[0] },
                {ServiceType.Smile, new [] { Constants.ServiceName, Constants.ServiceSmileName } },
                {ServiceType.SmileVideo, new [] { Constants.ServiceName, Constants.ServiceSmileName, Constants.ServiceSmileVideoName } },
                {ServiceType.SmileLive, new [] { Constants.ServiceName, Constants.ServiceSmileName, Constants.ServiceSmileLiveName } },
            };

            // 設定値よりコマンドラインオプションを優先する
            var baseDir = VariableConstants.HasOptionCacheRootDirectoryPath
                ? Path.Combine(VariableConstants.OptionValueCacheRootDirectoryPath, Constants.ApplicationDirectoryName)
                : Setting.CacheDirectoryPath;
            ;
            if(string.IsNullOrWhiteSpace(baseDir)) {
                baseDir = Path.Combine(Constants.CacheDirectoryPath, Constants.ApplicationDirectoryName);
            }

            var path = new List<string>() {
                baseDir,
            };
            path.AddRange(map[request.ServiceType]);

            var directoryPath = Environment.ExpandEnvironmentVariables(Path.Combine(path.ToArray()));

            if(Directory.Exists(directoryPath)) {
                var response = new ResponseModel(request, new DirectoryInfo(directoryPath));
                return response;
            } else {
                var response = new ResponseModel(request, Directory.CreateDirectory(directoryPath));
                return response;
            }
        }

        private object RequestShowViewCore(ShowViewRequestModel request)
        {
            Debug.Assert(request.ServiceType == ServiceType.Application);

            if(request.ParameterIsViewModel) {
                throw new NotImplementedException();
            } else {
                var appBrowserParameter = request.Parameter as AppBrowserParameterModel;
                ManagerPack.AppManager.AppBrowserManager.NavigateFromParameter(appBrowserParameter);

                return ManagerPack.AppManager.AppBrowserManager;
            }

            throw new NotImplementedException();
        }

        private ResponseModel Request_ShowView(ShowViewRequestModel request)
        {
            var result = RestrictUtility.Block(() => {
                switch(request.ServiceType) {
                    case ServiceType.Application:
                        return RequestShowViewCore(request);

                    case ServiceType.Smile:
                    case ServiceType.SmileVideo:
                    case ServiceType.SmileLive:
                        return Smile.RequestShowView(request);

                    default:
                        throw new NotImplementedException();
                }
            });

            var uiElement = result as FrameworkElement;
            if(uiElement != null) {
                CastUtility.AsAction<ISetView>(request.Parameter, vm => {
                    vm.SetView(uiElement);
                });
                var window = result as Window;
                if(window != null) {
                    window.ShowActivated = request.ShowViewState == ShowViewState.Foreground;
                    if(window.ShowActivated) {
                        var windowTask = window.Dispatcher.BeginInvoke(new Action(() => {
                            window.Show();
                        }));
                        return new ResponseModel(request, windowTask);
                    } else {
                        return new ResponseModel(request, null);
                    }
                }
            }
            var viewModel = result as ViewModelBase;
            if(viewModel != null) {
                // コントロール側はうまいことがんばる
                switch(request.ServiceType) {
                    case ServiceType.Application:
                        if(viewModel is AppBrowserManagerViewModel) {
                            ManagerPack.AppManager.SelectedManager = viewModel as ManagerViewModelBase;
                            return new ResponseModel(request, null);
                        }
                        throw new NotImplementedException();

                    case ServiceType.Smile:
                    case ServiceType.SmileVideo:
                    case ServiceType.SmileLive:
                        ManagerPack.AppManager.SelectedManager = ManagerPack.SmileManager;
                        ManagerPack.SmileManager.SelectedManager = viewModel as ManagerViewModelBase;
                        return new ResponseModel(request, null);

                    default:
                        throw new NotImplementedException();
                }
            }

            throw new NotImplementedException();
        }

        WebNavigatorContextMenuItemResultModel Request_WebNavigatorFromContextMenuItem(WebNavigatorRequestModel request, WebNavigatorContextMenuItemParameterModel parameter)
        {
            //WebNavigatorContextMenuItemViewModel menuItemResult;
            if(WebNavigatorContextMenuMap.TryGetValue(parameter.Key, out var menuItemResult)) {
                if(!menuItemResult.Conditions.Any()) {
                    return new WebNavigatorContextMenuItemResultModel(false, true, true, null);
                }

                foreach(var condition in menuItemResult.Conditions) {
                    var showMenuItem = false;
                    var enabledMenuItem = false;

                    if(condition.IsEnabledBaseUri) {
                        // メニュー表示可能サイト判定
                        showMenuItem = condition.BaseUriRegex.IsMatch(parameter.CurrentUri.OriginalString);
                    }

                    if(!condition.IsEnabledTagName) {
                        continue;
                    }

                    var elementList = new SimleHtmlElementList(condition.TagNameRegex, parameter.RootNodes, parameter.Element);

                    var hitElements = condition.TargetItems
                        .Select(i => elementList.MatchElement(i))
                    ;

                    if(elementList.HitTagNameInNodesPath && hitElements.All(h => h.IsHit)) {
                        if(showMenuItem) {
                            enabledMenuItem = true;
                        } else {
                            showMenuItem = true;
                            enabledMenuItem = true;
                        }
                    }

                    if(showMenuItem) {
                        var param = string.Empty;
                        if(enabledMenuItem) {
                            var hit = elementList.MatchElement(condition.Parameter);
                            if(hit.IsHit) {
                                param = hit.Match.Result(condition.Parameter.ParameterSource);
                            }
                        }

                        return new WebNavigatorContextMenuItemResultModel(false, enabledMenuItem, showMenuItem, param);
                    }
                }
            }

            return new WebNavigatorContextMenuItemResultModel(false, false, false, null);
        }

        WebNavigatorResultModel Request_WebNavigatorFromNavigating(WebNavigatorRequestModel request, WebNavigatorNavigatingParameterModel parameter)
        {
            var uriText = parameter.NextUri.OriginalString;

            foreach(var item in WebNavigatorNavigatingItems) {
                // 二重...
                var hitCondition = item.Conditions.FirstOrDefault(c => c.UriRegex.IsMatch(uriText));
                if(hitCondition != null) {
                    var match = hitCondition.UriRegex.Match(uriText);
                    var param = match.Result(hitCondition.ParameterSource);

                    return new WebNavigatorNavigatingResultModel(true, item, param);
                }
            }


            return new WebNavigatorResultModel(false);
        }

        private ResponseModel Request_WebNavigator(WebNavigatorRequestModel request)
        {
            switch(request.Parameter.Kind) {
                case WebNavigatorParameterKind.ContextMenuDefine: {
                        var contextMenuDefineResult = new WebNavigatorContextMenuDefineResultModel(WebNavigatorContextMenuItems);
                        return new ResponseModel(request, contextMenuDefineResult);
                    }

                case WebNavigatorParameterKind.Navigating: {
                        var parameter = (WebNavigatorNavigatingParameterModel)request.Parameter;
                        var navigatingResult = Request_WebNavigatorFromNavigating(request, parameter);
                        return new ResponseModel(request, navigatingResult);
                    }

                case WebNavigatorParameterKind.Click: {
                        var parameter = (WebNavigatorClickParameterModel)request.Parameter;
                    }
                    break;

                case WebNavigatorParameterKind.ContextMenu: {
                        var parameter = (WebNavigatorContextMenuParameterModel)request.Parameter;
                    }
                    break;

                case WebNavigatorParameterKind.ContextMenuItem: {
                        //todo:parameter
                        var parameter = (WebNavigatorContextMenuItemParameterModel)request.Parameter;
                        var contextMenuItemResult = Request_WebNavigatorFromContextMenuItem(request, parameter);
                        return new ResponseModel(request, contextMenuItemResult);
                    }

                case WebNavigatorParameterKind.Gesture: {
                        var gestureResult = new WebNavigatorGestureResultModel(WebNavigatorBridge.Gesture.Items);
                        return new ResponseModel(request, gestureResult);
                    }

                default:
                    throw new NotImplementedException();
            }

            var result = new WebNavigatorResultModel(false);
            return new ResponseModel(request, result);
        }

        public void SetManager(ServiceType serviceType, ManagerPackModelBase managerPack)
        {
            switch(serviceType) {
                case ServiceType.Application:
                    ManagerPack = (ApplicationManagerPackModel)managerPack;
                    break;

                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    Smile.SetManager(serviceType, managerPack);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        bool OrderCore_Exit(OrderModel order)
        {
            Logger.Trace("exit");

            // #724
            try {
                WebNavigatorCore.Uninitialize();
            } catch(System.Runtime.InteropServices.InvalidComObjectException ex) {
                Logger.Error(ex);
            }

            Application.Current.Dispatcher.Invoke(() => {
                Application.Current.Shutdown();
            });
            return true;
        }

        private bool OrderCore_Reboot(OrderModel order)
        {
            Logger.Trace("reboot");

            // #724
            try {
                WebNavigatorCore.Uninitialize();
            } catch(System.Runtime.InteropServices.InvalidComObjectException ex) {
                Logger.Error(ex);
            }

            Process.Start(Constants.AssemblyPath, Environment.CommandLine);
            //var processStartInfo = new ProcessStartInfo() {
            //    FileName = Constants.AssemblyPath,
            //    Arguments = Environment.CommandLine,
            //    UseShellExecute = false,
            //};
            //Process.Start(processStartInfo);
            Application.Current.Shutdown();
            return true;
        }


        bool OrderCore_Save(AppSaveOrderModel order)
        {
            var settingDirectory = VariableConstants.GetSettingDirectory();
            var settingFilePath = Path.Combine(settingDirectory.FullName, VariableConstants.SettingFileName);
            SerializeUtility.SaveSetting(settingFilePath, Setting, SerializeFileType.Json, true, Logger);

            // セーフモード時にバックアップは行わない
            if(!VariableConstants.IsSafeModeExecute) {
                if(order.IsBackup) {
                    var baseName = $"{Constants.GetNowTimestampFileName()}_ver.{Constants.ApplicationVersionNumber.ToString()}";
                    var fileName = PathUtility.AppendExtension(baseName, "zip");
                    var backupDirectory = VariableConstants.GetBackupDirectory();

                    var backupFilePath = Path.Combine(backupDirectory.FullName, fileName);
                    FileUtility.MakeFileParentDirectory(backupFilePath);

                    FileUtility.RotateFiles(backupDirectory.FullName, Constants.BackupSearchPattern, OrderBy.Descending, Constants.BackupSettingCount, e => {
                        Logger.Warning(e);
                        return true;
                    });

                    #region #639 保守
                    // #623 後処理の gzip ローテート
                    var zipCount = Directory.EnumerateFiles(backupDirectory.FullName, Constants.BackupSearchPattern).Count();
                    var issue623RemoveCount = Constants.BackupSettingCount - zipCount;
                    if(0 < issue623RemoveCount) {
                        FileUtility.RotateFiles(backupDirectory.FullName, Constants.BackupSearchPattern_Issue623, OrderBy.Descending, issue623RemoveCount, e => {
                            Logger.Warning(e);
                            return true;
                        });
                    }
                    #endregion

                    using(var archive = new ZipArchive(new FileStream(backupFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None), ZipArchiveMode.Create)) {
                        var settingFileArchive = archive.CreateEntry(VariableConstants.SettingFileName, CompressionLevel.Optimal);
                        using(var entryStream = settingFileArchive.Open()) {
                            using(var settingStream = new FileStream(settingFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                                settingStream.CopyTo(entryStream);
                            }
                        }
                    }
                }
            } else {
                Logger.Information("skip backup: safe mode");
            }

            return true;
        }

        bool OrderCore_CleanMemory(AppCleanMemoryOrderModel order)
        {
            var prevTime = DateTime.Now;
            var prevUsingMemory = GC.GetTotalMemory(false);

            var callEmptyWorkingSet = false;
            if(order.IsTargetLargeObjectHeap && order.CallEmptyWorkingSet) {
                CalledEmptyWorkingSetCount += 1;
                if(Constants.GarbageCollectionCallEmptyworkingsetCount < CalledEmptyWorkingSetCount) {
                    callEmptyWorkingSet = true;
                    CalledEmptyWorkingSetCount = 0;
                }
            }

            if(order.IsTargetLargeObjectHeap) {
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            }
            if(callEmptyWorkingSet) {
                NativeMethods.EmptyWorkingSet(Process.GetCurrentProcess().Handle);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if(callEmptyWorkingSet) {
                NativeMethods.EmptyWorkingSet(Process.GetCurrentProcess().Handle);
            }
            GC.Collect();

            var gcUsingMemory = GC.GetTotalMemory(true);
            var gcTime = DateTime.Now;

            // なんでこんなことしてるんだ
            var detail = new[] {
                prevTime.ToString() + " " + prevUsingMemory.ToString(),
                "GC (LOH: " + order.IsTargetLargeObjectHeap.ToString() + ")",
                gcTime.ToString() + " " + gcUsingMemory.ToString(),
            };
            var gcSize = RawValueUtility.ConvertHumanLikeByte(prevUsingMemory - gcUsingMemory);
            Logger.Information($"Memory GC: {gcSize}", string.Join(Environment.NewLine, detail));

            return true;
        }

        bool Order_Download(DownloadOrderModel order)
        {
            var downloadState = order.DownloadItem;
            //DownloadStatus.Insert(0, downloadState);

            var downloadItem = new AppDownloadItemViewModel(order.ServiceType, order.DownloadItem);

            ManagerPack.AppManager.AppDownloadManager.AddDownloadItem(downloadItem, order.CanManagement);

            return true;
        }

        bool OrderCore_SystemBreak(AppSystemBreakOrderModel order)
        {
            Debug.Assert(order.Suppression);

            if(!Setting.SystemBreakSuppression) {
                return false;
            }

            var elapsed = DateTime.Now - LastSystemBreakSuppressionTime;
            if(elapsed <= Constants.AppSystemBreakTime) {
                // 前回抑制時間から閾値の時間に至ってない場合は抑制しない
                return false;
            }

            var vistaFlag
                = ES.ES_DISPLAY_REQUIRED
                | ES.ES_SYSTEM_REQUIRED
                | ES.ES_AWAYMODE_REQUIRED
            ;
            if((int)NativeMethods.SetThreadExecutionState(vistaFlag) == 0) {
                // OS バージョン的には到達しない。でも失敗の場合は到達する
                var toutastu_shinai
                    = ES.ES_DISPLAY_REQUIRED
                    | ES.ES_SYSTEM_REQUIRED
                ;
                NativeMethods.SetThreadExecutionState(toutastu_shinai);
            }

            LastSystemBreakSuppressionTime = DateTime.Now;

            Logger.Debug("suppression: screen savrer/lock");

            return true;
        }

        bool OrderCore_ProcessLink(AppProcessLinkOrderModelBase order)
        {
            return ProcessLinkerHost.ReceiveOrder(order);
        }

        bool OrderCore(OrderModel order)
        {
            switch(order.OrderKind) {
                case OrderKind.Exit:
                    return OrderCore_Exit(order);

                case OrderKind.Reboot:
                    return OrderCore_Reboot(order);

                case OrderKind.Save:
                    return OrderCore_Save((AppSaveOrderModel)order);

                case OrderKind.CleanMemory:
                    return OrderCore_CleanMemory((AppCleanMemoryOrderModel)order);

                case OrderKind.SystemBreak:
                    return OrderCore_SystemBreak((AppSystemBreakOrderModel)order);

                case OrderKind.ProcessLink:
                    return OrderCore_ProcessLink((AppProcessLinkOrderModelBase)order);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region MediatorBase

        public override ILogger Logger { get { return this._logger; } }

        protected override SpaghettiScript CreateScript()
        {
            var myType = GetType();
            var domainName = myType.Name;

            var script = new SpaghettiScript(domainName, Logger);

            script.ConstructPreparations(new DirectoryInfo(Constants.SpaghettiDirectoryPath), GetKeys());

            return script;
        }

        ResponseModel RequestCore_Process(ProcessRequestModelBase request)
        {
            var appLoggingParameter = request.ParameterBase as AppLoggingParameterModel;
            if(appLoggingParameter != null) {
                var logs = ManagerPack.AppManager.AppInformationManager.AppLoggingManager.LogListViewer;
                if(appLoggingParameter.GetClone) {
                    logs = logs.ToEvaluatedSequence();
                }

                return new ResponseModel(request, logs);
            }

            throw new NotImplementedException();
        }

        ResponseModel RequestCore(RequestModel request)
        {
            switch(request.RequestKind) {
                case RequestKind.Setting:
                    return new ResponseModel(request, Setting);

                case RequestKind.Process:
                    return RequestCore_Process((ProcessRequestModelBase)request);

                default:
                    throw new NotImplementedException();
            }
        }

        public override ResponseModel Request(RequestModel request)
        {
            CheckUtility.DebugEnforceNotNull(request);

            if(request.RequestKind == RequestKind.CacheDirectory) {
                return Request_CacheDirectoryCore(request);
            }

            if(request.RequestKind == RequestKind.ShowView) {
                return Request_ShowView((ShowViewRequestModel)request);
            }

            if(request.RequestKind == RequestKind.WebNavigator) {
                return Request_WebNavigator((WebNavigatorRequestModel)request);
            }

            switch(request.ServiceType) {
                case ServiceType.Application:
                    return RequestCore(request);

                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.Request(request);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.Request(request);

                default:
                    ThrowNotSupportRequest(request);
                    throw new NotImplementedException();
            }
        }

        public override bool Order(OrderModel order)
        {
            CheckUtility.DebugEnforceNotNull(order);

            if(order.OrderKind == OrderKind.Download) {
                return Order_Download((DownloadOrderModel)order);
            }

            switch(order.ServiceType) {
                case ServiceType.Application:
                    return OrderCore(order);

                default:
                    ThrowNotSupportOrder(order);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyUriResult GetUri(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Application:
                    return GetUriCore(key, replaceMap, serviceType);

                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.GetUri(key, replaceMap, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.GetUri(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetUri(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertUri(string key, string uri, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.ConvertUri(key, uri, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.ConvertUri(key, uri, serviceType);

                default:
                    ThrowNotSupportConvertUri(key, uri, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestHeader(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.GetRequestHeader(key, replaceMap, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.GetRequestHeader(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestHeader(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> ConvertRequestHeader(string key, IDictionary<string, string> requestHeaders, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.ConvertRequestHeader(key, requestHeaders, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.ConvertRequestHeader(key, requestHeaders, serviceType);

                default:
                    ThrowNotSupportConvertRequestHeader(key, requestHeaders, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> GetRequestParameter(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.GetRequestParameter(key, replaceMap, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.GetRequestParameter(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyMappingResult GetRequestMapping(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.GetRequestMapping(key, replaceMap, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.GetRequestMapping(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestMapping(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyExpression GetExpression(string key, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Common:
                    return Common.GetExpression(key, serviceType);

                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.GetExpression(key, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.GetExpression(key, serviceType);

                default:
                    ThrowNotSupportGetExpression(key, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyExpression GetExpression(string key, string id, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Common:
                    return Common.GetExpression(key, id, serviceType);

                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.GetExpression(key, id, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.GetExpression(key, id, serviceType);

                default:
                    ThrowNotSupportGetExpression(key, id, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IDictionary<string, string> ConvertRequestParameter(string key, IDictionary<string, string> requestParams, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.ConvertRequestParameter(key, requestParams, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.ConvertRequestParameter(key, requestParams, serviceType);

                default:
                    ThrowNotSupportConvertRequestParameter(key, requestParams, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertRequestMapping(string key, string mapping, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.ConvertRequestMapping(key, mapping, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.ConvertRequestMapping(key, mapping, serviceType);

                default:
                    ThrowNotSupportConvertRequestMapping(key, mapping, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override IReadOnlyCheck CheckResponseHeader(string key, Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.CheckResponseHeader(key, uri, headers, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.CheckResponseHeader(key, uri, headers, serviceType);

                default:
                    ThrowNotSupportCheckResponseHeader(key, uri, headers, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override void ConvertBinary(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    Smile.ConvertBinary(key, uri, stream, serviceType);
                    break;

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    IdleTalk.ConvertBinary(key, uri, stream, serviceType);
                    break;

                default:
                    ThrowNotSupportConvertBinary(key, uri, stream, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override Encoding GetEncoding(string key, Uri uri, Stream stream, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.GetEncoding(key, uri, stream, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.GetEncoding(key, uri, stream, serviceType);

                default:
                    ThrowNotSupportGetEncoding(key, uri, stream, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override string ConvertString(string key, Uri uri, string text, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.ConvertString(key, uri, text, serviceType);

                case ServiceType.IdleTalk:
                case ServiceType.IdleTalkMutter:
                    return IdleTalk.ConvertString(key, uri, text, serviceType);

                default:
                    ThrowNotSupportConvertString(key, uri, text, serviceType);
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
