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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Order;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// データ連携等々の橋渡し。
    /// </summary>
    public class Mediation: MediationBase
    {
        #region variable

        ILogger _logger;

        #endregion

        public Mediation(AppSettingModel mainSettingModel, ILogger logger)
        {
            this._logger = logger;
            Debug.Listeners.Add(new LogListener(Logger, LogKind.Debug));

            Logger.LoggerConfig.EnabledAll = true;
            Logger.LoggerConfig.PutsDebug = true;
            Logger.LoggerConfig.PutsConsole = true;
            Logger.Information("start!");

            Script = CreateScript();

            Setting = mainSettingModel;
            Smile = new SmileMediation(this, Setting.ServiceSmileSetting);
        }

        #region property

        AppSettingModel Setting { get; }

        /// <summary>
        /// ニコニコ関係。
        /// </summary>
        internal SmileMediation Smile { get; private set; }

        internal ApplicationManagerPackModel ManagerPack { get; private set; }

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
            //Application.Current.Dispatcher.InvokeShutdown;
            Application.Current.Dispatcher.Invoke(() => {
                Application.Current.Shutdown();
            });
            return true;
        }

        private bool OrderCore_Rebbot(OrderModel order)
        {
            Process.Start(Constants.AssemblyPath, Environment.CommandLine);
            Application.Current.Shutdown();
            return true;
        }


        bool OrderCore_Save(AppSaveOrderModel order)
        {
            var settingDirectory = VariableConstants.GetSettingDirectory();
            var settingFilePath = Path.Combine(settingDirectory.FullName, Constants.SettingFileName);
            SerializeUtility.SaveSetting(settingFilePath, Setting, SerializeFileType.Json, true, Logger);

            if(order.IsBackup) {
                var baseName = $"{Constants.GetNowTimestampFileName()}_ver.{Constants.ApplicationVersionNumber.ToString()}";
                var fileName = PathUtility.AppendExtension(baseName, "json.gz");
                var backupDirectory = VariableConstants.GetBackupDirectory();
                FileUtility.RotateFiles(backupDirectory.FullName, Constants.BackupSearchPattern, OrderBy.Descending, Constants.BackupSettingCount, e => {
                    Logger.Warning(e);
                    return true;
                });
                var backupFilePath = Path.Combine(backupDirectory.FullName, fileName);
                FileUtility.MakeFileParentDirectory(backupFilePath);
                //File.Copy(settingFilePath, backupFilePath, true);
                // 1ファイル集約の元 gzip でポン!
                using(var output = new GZipStream(new FileStream(backupFilePath, FileMode.Create, FileAccess.ReadWrite), CompressionMode.Compress)) {
                    using(var input = File.OpenRead(settingFilePath)) {
                        input.CopyTo(output);
                    }
                }
            }

            return true;
        }

        bool OrderCore_CleanMemory(AppCleanMemoryOrderModel order)
        {
            var prevTime = DateTime.Now;
            var prevUsingMemory = GC.GetTotalMemory(false);

            if(order.IsTargetLargeObjectHeap) {
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
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

        bool OrderCore(OrderModel order)
        {
            switch(order.OrderKind) {
                case OrderKind.Exit:
                    return OrderCore_Exit(order);

                case OrderKind.Reboot:
                    return OrderCore_Rebbot(order);

                case OrderKind.Save:
                    return OrderCore_Save((AppSaveOrderModel)order);

                case OrderKind.CleanMemory:
                    return OrderCore_CleanMemory((AppCleanMemoryOrderModel)order);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region MediationBase

        public override ILogger Logger { get { return this._logger; } }

        protected override SpaghettiScript CreateScript()
        {
            var myType = GetType();
            var domainName = myType.Name;

            var script = new SpaghettiScript(domainName, Logger);

            script.ConstructPreparations(new DirectoryInfo(Constants.SpaghettiDirectoryPath), GetKeys());

            return script;
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

            switch(request.ServiceType) {
                case ServiceType.Application:
                    return new ResponseModel(request, Setting);

                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.Request(request);

                default:
                    ThrowNotSupportRequest(request);
                    throw new NotImplementedException();
            }
        }

        public override bool Order(OrderModel order)
        {
            CheckUtility.DebugEnforceNotNull(order);

            switch(order.ServiceType) {
                case ServiceType.Application:
                    return OrderCore(order);

                default:
                    ThrowNotSupportOrder(order);
                    throw new NotImplementedException();
            }
        }

        public override UriResultModel GetUri(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Application:
                    return GetUriCore(key, replaceMap, serviceType);

                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.GetUri(key, replaceMap, serviceType);

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

                default:
                    ThrowNotSupportGetRequestParameter(key, replaceMap, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override MappingResultModel GetRequestMapping(string key, IDictionary<string, string> replaceMap, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.GetRequestMapping(key, replaceMap, serviceType);

                default:
                    ThrowNotSupportGetRequestMapping(key, replaceMap, serviceType);
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

                default:
                    ThrowNotSupportConvertRequestMapping(key, mapping, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override CheckModel CheckResponseHeader(string key, Uri uri, HttpHeaders headers, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.CheckResponseHeader(key, uri, headers, serviceType);

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

                default:
                    ThrowNotSupportConvertString(key, uri, text, serviceType);
                    throw new NotImplementedException();
            }
        }

        public override bool ConvertValue(out object outputValue, Type outputType, string inputKey, object inputValue, Type inputType, ServiceType serviceType)
        {
            switch(serviceType) {
                case ServiceType.Smile:
                case ServiceType.SmileVideo:
                case ServiceType.SmileLive:
                    return Smile.ConvertValue(out outputValue, outputType, inputKey, inputValue, inputType, serviceType);

                default:
                    ThrowNotSupportValueConvert(inputKey, inputValue, inputType, outputType, serviceType);
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
