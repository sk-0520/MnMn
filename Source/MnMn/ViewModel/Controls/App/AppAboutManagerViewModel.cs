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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
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

        #endregion

        public AppAboutManagerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

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
                    Execute(s);
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
                    list.Add("CLR: " + System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion());
                    var text = Environment.NewLine + separator + Environment.NewLine + string.Join(Environment.NewLine, list.Select(s => "    " + s)) + Environment.NewLine + Environment.NewLine;
                    Clipboard.SetText(text);
                });
            }
        }

        public ICommand CopyLongInformationCommand
        {
            get
            {
                return CreateCommand(o => {
                    var appInfo = new AppInformationCollection();
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
                    Clipboard.SetText(text);
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
                        Mediation.Order(new AppCleanMemoryOrderModel(true));
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
                    Execute(Constants.AssemblyRootDirectoryPath);
                });
            }
        }

        public ICommand OpenSettingDirectoryCommand
        {
            get
            {
                return CreateCommand(o => {
                    Execute(VariableConstants.GetSettingDirectory().FullName);
                });
            }
        }

        public ICommand OpenCacheDirectoryCommand
        {
            get
            {
                return CreateCommand(o => {
                    var dir = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Application));
                    Execute(dir.FullName);
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

        #endregion

        #region function

        void Execute(string command)
        {
            try {
                Process.Start(command);
            } catch(Exception ex) {
                Mediation.Logger.Error(ex);
            }
        }

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
                    Mediation.Logger.Error(ex);
                }
            }
        }

        AppSettingModel StripCredentials(Stream settingStream)
        {
            var stripSetting = SerializeUtility.LoadSetting<AppSettingModel>(settingStream, SerializeFileType.Json, Mediation.Logger);

            stripSetting.ServiceSmileSetting.Account = new Model.Setting.Service.Smile.SmileUserAccountModel();

            return stripSetting;
        }

        void ExportPublicInformationFile(string filePath)
        {
            var baseSetting = Mediation.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(RequestKind.Setting, ServiceType.Application));
            using(var baseSettingStream = GlobalManager.MemoryStream.GetStreamWidthAutoTag()) {
                SerializeUtility.SaveSetting(baseSettingStream, baseSetting, SerializeFileType.Json, Mediation.Logger);
                baseSettingStream.Position = 0;

                var stripSetting = StripCredentials(baseSettingStream);

                FileUtility.MakeFileParentDirectory(filePath);
                using(var exportStream = new ZipArchive(new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read), ZipArchiveMode.Create)) {

                    var settingEntry = exportStream.CreateEntry(Constants.SettingFileName);
                    using(var zipStream = settingEntry.Open()) {
                        SerializeUtility.SaveSetting(zipStream, stripSetting, SerializeFileType.Json, Mediation.Logger);
                    }

                    var informationEntry = exportStream.CreateEntry(Constants.InformationFileName);
                    using(var zipStream = informationEntry.Open()) {
                        using(var streamWriter = new StreamWriter(zipStream, Encoding.UTF8, Constants.TextFileSaveBuffer, true)) {
                            var info = new AppInformationCollection();
                            streamWriter.Write(info.ToString());
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

        public override void InitializeView(MainWindow view)
        {
        }

        public override void UninitializeView(MainWindow view)
        {
        }

        #endregion
    }
}
