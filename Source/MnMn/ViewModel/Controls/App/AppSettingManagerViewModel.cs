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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Model.Order;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppSettingManagerViewModel: ManagerViewModelBase
    {
        public AppSettingManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            AppSetting = Mediation.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(Define.RequestKind.Setting, Define.ServiceType.Application));
        }

        #region property

        AppSettingModel AppSetting { get; }

        public FewViewModel<bool> WebNavigatorGeckoFxOwnResponsibility { get; } = new FewViewModel<bool>(false);

        public string CacheDirectoryPath
        {
            get { return AppSetting.CacheDirectoryPath; }
            set { SetPropertyValue(AppSetting, value); }
        }

        public TimeSpan CacheLifeTime
        {
            get { return AppSetting.CacheLifeTime; }
            set { SetPropertyValue(AppSetting, value); }
        }

        public bool WebNavigatorGeckoFxScanPlugin
        {
            get { return AppSetting.WebNavigator.GeckoFxScanPlugin; }
            set { SetPropertyValue(AppSetting.WebNavigator, value, nameof(AppSetting.WebNavigator.GeckoFxScanPlugin)); }
        }

        #endregion

        #region command

        public ICommand SelectCacheDirectoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        CacheDirectoryPath = SelectDirectory(CacheDirectoryPath);
                    }
                );
            }
        }

        public ICommand SetDefaultCacheDirectoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        CacheDirectoryPath = string.Empty;
                    }
                );
            }
        }

        public ICommand RebuildWebNavigatorGeckoFxPluginCommand
        {
            get
            {
                return CreateCommand(
                    o => RebuildWebNavigatorGeckoFxPluginAsync().ConfigureAwait(false),
                    o => WebNavigatorGeckoFxOwnResponsibility.Value
                );
            }
        }

        #endregion

        #region function

        string SelectDirectory(string initialDirectoryPath)
        {
            using(var dialog = new FolderBrowserDialog()) {
                dialog.SelectedPath = initialDirectoryPath;
                if(dialog.ShowDialog().GetValueOrDefault()) {
                    return dialog.SelectedPath;
                }
            }

            return initialDirectoryPath;
        }

        async Task<FileInfo> DownloadWebNavigatorGeckoFxPluginAsync()
        {
            var host = new HttpUserAgentHost();
            var client = host.CreateHttpUserAgent();

            var archiveDir = VariableConstants.GetWebNavigatorGeckFxPluginDirectory();

            FileUtility.RotateFiles(archiveDir.FullName, Constants.ArchiveWebNavigatorGeckFxPluginSearchPattern, ContentTypeTextNet.Library.SharedLibrary.Define.OrderBy.Descending, Constants.BackupWebNavigatorGeckoFxPluginCount, e => {
                Mediation.Logger.Warning(e);
                return true;
            });

            var fileName = PathUtility.AppendExtension(Constants.GetNowTimestampFileName(), "zip");
            var filePath = Path.Combine(archiveDir.FullName, fileName);

            using(var webStream = await client.GetStreamAsync(Constants.AppUriWebNavigatorGeckoFxPlugins))
            using(var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None)) {
                webStream.CopyTo(fileStream);
            }

            return new FileInfo(filePath);
        }

        DirectoryInfo ClearPluginDirectory()
        {
            var dir = new DirectoryInfo(Constants.WebNavigatorGeckoFxPluginsDirectoryPath);
            if(dir.Exists) {
                dir.Delete(true);
            }
            dir.Create();
            return dir;
        }

        void ExpandPlugin(FileInfo archiveFile, DirectoryInfo pluginDirectory)
        {
            ZipFile.ExtractToDirectory(archiveFile.FullName, pluginDirectory.FullName);
        }

        async Task RebuildWebNavigatorGeckoFxPluginAsync()
        {
            Mediation.Order(new AppSaveOrderModel(true));

            WebNavigatorCore.Uninitialize();

            try {
                var archiveFile = await DownloadWebNavigatorGeckoFxPluginAsync();
                var pluginDir = ClearPluginDirectory();
                ExpandPlugin(archiveFile, pluginDir);
            } catch(Exception ex) {
                Mediation.Logger.Error(ex);
            }

            Mediation.Order(new OrderModel(OrderKind.Reboot, ServiceType.Application));
        }

        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        { }

        protected override void HideViewCore()
        { }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }
        public override void UninitializeView(MainWindow view)
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan)
        {
            return GarbageCollectionDummyResult;
        }

        #endregion
    }
}
