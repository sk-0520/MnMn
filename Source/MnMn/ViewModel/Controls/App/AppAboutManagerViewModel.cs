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
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Order;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using Microsoft.Win32;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppAboutManagerViewModel: ManagerViewModelBase
    {
        #region define

        static string separator = "____________";

        #endregion

        #region variable

        ComponentItemCollectionModel _componentCollection;
        long _totalMemorySize;

        long _workingSet;
        long _virtualMemorySize;

        bool _isOpenDevelopmentMenu;

        string _exceptionType = typeof(Exception).FullName;
        string _httpUri = "http://127.0.0.1";

        #endregion

        public AppAboutManagerViewModel(Mediator mediator, AppLoggingManagerViewModel loggingManager)
            : base(mediator)
        {
            LoggingManager = loggingManager;
        }

        #region property

        AppLoggingManagerViewModel LoggingManager { get; }

        public ComponentItemCollectionModel ComponentCollection
        {
            get
            {
                if(this._componentCollection == null) {
                    this._componentCollection = SerializeUtility.LoadXmlSerializeFromFile<ComponentItemCollectionModel>(Constants.ComponentListFileName);
                }

                return this._componentCollection;
            }
        }

        public long TotalMemorySize
        {
            get { return this._totalMemorySize; }
            private set { SetVariableValue(ref this._totalMemorySize, value); }
        }

        public long WorkingSet
        {
            get { return this._workingSet; }
            private set { SetVariableValue(ref this._workingSet, value); }
        }

        public long VirtualMemorySize
        {
            get { return this._virtualMemorySize; }
            private set { SetVariableValue(ref this._virtualMemorySize, value); }
        }

        public bool IsOpenDevelopmentMenu
        {
            get { return this._isOpenDevelopmentMenu; }
            set { SetVariableValue(ref this._isOpenDevelopmentMenu, value); }
        }

        public string ExceptionType
        {
            get { return this._exceptionType; }
            set { SetVariableValue(ref this._exceptionType, value); }
        }

        public string HttpUri
        {
            get { return this._httpUri; }
            set { SetVariableValue(ref this._httpUri, value); }
        }

        #endregion

        #region command

        public ICommand OpenLinkCommand
        {
            get
            {
                return CreateCommand(o => {
                    var s = o is Uri
                        ? ((Uri)o).OriginalString
                        : (string)o
                    ;

                    if(s.Any(c => c == '@')) {
                        var mail = "mailto:" + s;
                        s = mail;
                    }
                    //Execute(s);
                    ShellUtility.ExecuteCommand(s, Mediator.Logger);
                });
            }
        }

        public ICommand CopyShortInformationCommand
        {
            get
            {
                return CreateCommand(o => {
                    var list = new List<string>();
                    list.Add("Software: " + Constants.ApplicationName);
                    list.Add("Version: " + Constants.ApplicationVersion);
                    list.Add("BuildType: " + Constants.BuildType);
                    //list.Add("Process: " + Constants.BuildProcess);
                    list.Add("Platform: " + (Environment.Is64BitOperatingSystem ? "64" : "32"));
                    list.Add("OS: " + System.Environment.OSVersion);
                    list.Add("CLR: " + Environment.Version);
                    var setting = Mediator.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(RequestKind.Setting, ServiceType.Application));
                    list.Add("Lightweight: " + setting.RunningInformation.LightweightUpdateTimestamp.ToString("u"));
                    var text = Environment.NewLine + separator + Environment.NewLine + string.Join(Environment.NewLine, list.Select(s => "    " + s)) + Environment.NewLine + Environment.NewLine;
                    ShellUtility.SetClipboard(text, Mediator.Logger);
                });
            }
        }

        public ICommand CopyLongInformationCommand
        {
            get
            {
                return CreateCommand(o => {
                    var appInfo = new AppInformationCollection(Mediator);
                    var text
                        = Environment.NewLine
                        + separator
                        + Environment.NewLine
                        + string.Join(
                            Environment.NewLine,
                            appInfo.ToString()
                                .SplitLines()
                                .Select(s => "    " + s)
                        )
                        + Environment.NewLine
                        + Environment.NewLine
                    ;
                    ShellUtility.SetClipboard(text, Mediator.Logger);
                });
            }
        }

        public ICommand ReloadUsingMemoryCommand
        {
            get
            {
                return CreateCommand(o => ReloadUsingMemory());
            }
        }

        public ICommand GarbageCollectionMemoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
#if DEBUG
                        Mediator.Order(new AppCleanMemoryOrderModel(true, true));
#else
                        Mediator.Order(new AppCleanMemoryOrderModel(true, false));
#endif
                        ReloadUsingMemoryCommand.TryExecute(null);
                    }
                );
            }
        }


        public ICommand OpenAppDirectoryCommand
        {
            get
            {
                return CreateCommand(o => {
                    var dir = new DirectoryInfo(Constants.AssemblyRootDirectoryPath);
                    ShellUtility.OpenDirectory(dir, Mediator.Logger);
                });
            }
        }

        public ICommand OpenSettingDirectoryCommand
        {
            get
            {
                return CreateCommand(o => {
                    ShellUtility.OpenDirectory(VariableConstants.GetSettingDirectory(), Mediator.Logger);
                });
            }
        }

        public ICommand OpenCacheDirectoryCommand
        {
            get
            {
                return CreateCommand(o => {
                    var dir = Mediator.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Application));
                    ShellUtility.OpenDirectory(dir, Mediator.Logger);
                });
            }
        }

        public ICommand ExportPublicInformationFileCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        ExportPublicInformationFileFromDialog();
                    }
                );
            }
        }

        public ICommand SettingSaveCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var isBackup = Convert.ToBoolean(o);
                        Mediator.Order(new AppSaveOrderModel(isBackup));
                        IsOpenDevelopmentMenu = false;
                    }
                );
            }
        }

        public ICommand GoogbyeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        IsOpenDevelopmentMenu = false;

                        if(!string.IsNullOrWhiteSpace(ExceptionType)) {
                            var exceptionType = Type.GetType(ExceptionType, false);
                            if(exceptionType != null) {
                                var exception = Activator.CreateInstance(exceptionType, nameof(GoogbyeCommand)) as Exception;
                                if(exception != null) {
                                    throw exception;
                                }
                                throw new InvalidCastException($"{exceptionType}");
                            }
                            throw new InvalidCastException(ExceptionType);
                        }
                        throw new Exception(nameof(GoogbyeCommand));
                    }
                );
            }
        }

        public ICommand ConnectHttpCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        IsOpenDevelopmentMenu = false;

                        var userAgentHost = new HttpUserAgentHost(NetworkSetting, Mediator.Logger);
                        var host = userAgentHost.CreateHttpUserAgent();
                        host.GetStringAsync(HttpUri).ContinueWith(t => {
                            if(t.IsFaulted) {
                                Mediator.Logger.Error(t.Exception);
                            } else {
                                Mediator.Logger.Debug(HttpUri, t.Result);
                            }
                        });
                    }
                );
            }
        }

        #endregion

        #region function

        void ReloadUsingMemory()
        {
            TotalMemorySize = GC.GetTotalMemory(false);
            using(var process = Process.GetCurrentProcess()) {
                WorkingSet = process.WorkingSet64;
                VirtualMemorySize = process.VirtualMemorySize64;
            }
        }

        void ExportPublicInformationFileFromDialog()
        {
            var filter = new DialogFilterList();
            filter.Add(new DialogFilterItem(Properties.Resources.String_App_Setting_PublicExportFileName, Constants.PublicExportFileNamePattern));

            var dialog = new SaveFileDialog() {
                Filter = filter.FilterText,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                FileName = $"{Constants.ApplicationName}_public-export_{Constants.GetNowTimestampFileName()}",
            };
            if(dialog.ShowDialog().GetValueOrDefault()) {
                try {
                    var filePath = dialog.FileName;
                    ExportPublicInformationFile(filePath);
                } catch(Exception ex) {
                    Mediator.Logger.Error(ex);
                }
            }
        }

        AppSettingModel StripCredentials(Stream settingStream)
        {
            var stripSetting = SerializeUtility.LoadSetting<AppSettingModel>(settingStream, SerializeFileType.Json, Mediator.Logger);

            stripSetting.ServiceSmileSetting.Account = new Model.Setting.Service.Smile.SmileUserAccountModel();
            stripSetting.RunningInformation.UserId = null;

            return stripSetting;
        }

        void ExportPublicInformationFile(string filePath)
        {
            var baseSetting = Mediator.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(RequestKind.Setting, ServiceType.Application));
            using(var baseSettingStream = GlobalManager.MemoryStream.GetStreamWidthAutoTag()) {
                SerializeUtility.SaveSetting(baseSettingStream, baseSetting, SerializeFileType.Json, Mediator.Logger);
                baseSettingStream.Position = 0;

                var stripSetting = StripCredentials(baseSettingStream);

                FileUtility.MakeFileParentDirectory(filePath);
                using(var exportStream = new ZipArchive(new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read), ZipArchiveMode.Create)) {

                    var settingEntry = exportStream.CreateEntry(VariableConstants.SettingFileName);
                    using(var zipStream = settingEntry.Open()) {
                        SerializeUtility.SaveSetting(zipStream, stripSetting, SerializeFileType.Json, Mediator.Logger);
                    }

                    var informationEntry = exportStream.CreateEntry(Constants.InformationFileName);
                    using(var zipStream = informationEntry.Open()) {
                        using(var streamWriter = new StreamWriter(zipStream, Encoding.UTF8, Constants.TextFileSaveBuffer, true)) {
                            var info = new AppInformationCollection(Mediator);
                            info.WriteInformation(streamWriter);
                        }
                    }

                    var logEntry = exportStream.CreateEntry(Constants.LogFileName);
                    using(var zipStream = logEntry.Open()) {
                        LoggingManager.WriteLog(zipStream);
                    }

                    var appConfigFilePath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                    var appConfigEntry = exportStream.CreateEntry(Path.GetFileName(appConfigFilePath));
                    using(var zipStream = appConfigEntry.Open()) {
                        using(var stream = new FileStream(appConfigFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                            stream.CopyTo(zipStream);
                        }
                    }

                    var appConfig = ConfigurationManager.OpenExeConfiguration(Constants.AssemblyPath);
                    var appUserConfigFileName = appConfig.AppSettings.File;
                    if(!string.IsNullOrWhiteSpace(appUserConfigFileName)) {
                        var appUserConfigFilePath = Path.Combine(Constants.AssemblyRootDirectoryPath, appUserConfigFileName);
                        if(File.Exists(appUserConfigFilePath)) {
                            var userAppConfigEntry = exportStream.CreateEntry(appUserConfigFileName);
                            using(var zipStream = userAppConfigEntry.Open()) {
                                using(var stream = new FileStream(appUserConfigFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                                    stream.CopyTo(zipStream);
                                }
                            }
                        }
                    }

                }
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
            ReloadUsingMemory();
        }

        protected override void HideViewCore()
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
        }

        public override void UninitializeView(MainWindow view)
        {
        }

        #endregion
    }
}
