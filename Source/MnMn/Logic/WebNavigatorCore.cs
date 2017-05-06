#define ISSUE_551

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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using Gecko;
using Gecko.Cache;
using Microsoft.Win32;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// <see cref="MnMn.View.Controls.WebNavigator"/> で動作するブラウザ挙動の担当。
    /// </summary>
    public static partial class WebNavigatorCore
    {
        #region define

        const string allFilterExtension = "*";

        #endregion

        #region property

        static Mediation Mediation { get; set; }

        //public static WebNavigatorEngine Engine { get; } = WebNavigatorEngine.Default;
        public static WebNavigatorEngine Engine {
            get {
                if(ForceDefaultEngine) {
                    return WebNavigatorEngine.Default;
                }

                return Constants.WebNavigatorEngine;
            }
        }

        public static bool ForceDefaultEngine { get; set; } = false;

        static bool IsInitialized { get; set; } = false;
        static bool IsUninitialized { get; set; } = false;

        /// <summary>
        /// システムの標準ブラウザ。
        /// <para>null の時は設定されてないか上手いこと取ってこれなかった。</para>
        /// </summary>
        public static ExecuteData? DefaultBrowserExecuteData { get; private set; }
        public static ImageSource DefaultBrowserIcon { get; private set; }

        #endregion

        #region function

        static void InitializeDefault()
        {
            var ieVersion = SystemEnvironmentUtility.GetInternetExplorerVersion();
            Mediation.Logger.Information("IE version: " + ieVersion);
            SystemEnvironmentUtility.SetUsingBrowserVersionForExecutingAssembly(ieVersion);
        }

        /// <summary>
        /// 初期化。
        /// </summary>
        public static void Initialize(Mediation mediation)
        {
            if(IsInitialized) {
                return;
            }

            Mediation = mediation;

            switch(Engine) {
                case WebNavigatorEngine.Default:
                WebNavigatorEngine_Default:
                    InitializeDefault();
                    break;

                case WebNavigatorEngine.GeckoFx:
                    try {
                        InitializeGecko();
#if ISSUE_551
                        throw new Exception("#551");
#if !DEBUG
#error NOT DEBUG!
#endif
#endif
                    } catch(Exception ex) {
                        Mediation.Logger.Error(ex);
                        ForceDefaultEngine = true;
                        goto WebNavigatorEngine_Default;
                    }
#if !ISSUE_551
                    break;
#endif


                default:
                    break;
            }

            RefreshDefaultBrowser();

            IsInitialized = true;
            IsUninitialized = false;
        }

        public static void RefreshDefaultBrowser()
        {
            DefaultBrowserExecuteData = null;
            DefaultBrowserIcon = null;

            var command = GetDefaultBrowserCommand();
            if(string.IsNullOrWhiteSpace(command)) {
                Mediation.Logger.Warning($"fail: default browser: registry is null");
                return;
            }

            try {
                DefaultBrowserExecuteData = GetBrowserPureExecuteData(command);
            } catch(Exception ex) {
                Mediation.Logger.Warning($"fail: default browser: {command}");
                Mediation.Logger.Warning(ex);
            }

            if(DefaultBrowserExecuteData.HasValue) {
                Application.Current.Dispatcher.Invoke(() => {
                    var path = DefaultBrowserExecuteData.Value.ApplicationPath ?? string.Empty;
                    var usingPath = Environment.ExpandEnvironmentVariables(path);
                    if(File.Exists(usingPath)) {
                        DefaultBrowserIcon = IconUtility.Load(usingPath, ContentTypeTextNet.Library.SharedLibrary.Define.IconScale.Small, 0, Mediation.Logger);
                    }
                });
            }
        }

        static void UninitializeDefault()
        {
            SystemEnvironmentUtility.ResetUsingBrowserVersionForExecutingAssembly();
        }

        /// <summary>
        /// さよなら。
        /// </summary>
        public static void Uninitialize()
        {
            if(IsUninitialized) {
                return;
            }

            switch(Engine) {
                case WebNavigatorEngine.Default:
                    UninitializeDefault();
                    break;

                case WebNavigatorEngine.GeckoFx:
                    UninitializeGecko();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// セッションをエンジンに設定。
        /// <para><see cref="WebBrowser"/>? しらん。</para>
        /// </summary>
        /// <param name="session"></param>
        /// <param name="uri"></param>
        public static void SetSessionEngine(SessionViewModelBase session, Uri uri)
        {
            Application.Current.Dispatcher.Invoke(() => {
                session.ApplyToWebNavigatorEngine(Engine, uri);
            });
        }

        static string GetDefaultBrowserCommandCore(RegistryKey registryKey, string path, string name = null)
        {
            using(var registry = registryKey.OpenSubKey(path, false)) {
                if(registry == null) {
                    return null;
                }

                var value = (string)registry.GetValue(name);
                return value;
            }
        }

        public static string GetDefaultBrowserCommand()
        {
            var choiceBurowserId = GetDefaultBrowserCommandCore(Registry.CurrentUser, @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "Progid");
            if(choiceBurowserId != null) {
                using(var registry = Registry.ClassesRoot.OpenSubKey($@"{choiceBurowserId}\Shell\Open\Command")) {
                    var choiceBurowser = (string)registry.GetValue(null);
                    if(choiceBurowser != null) {
                        return choiceBurowser;
                    }
                }
            }

            var currentUser = GetDefaultBrowserCommandCore(Registry.CurrentUser, @"Software\Classes\http\shell\open\command");
            if(currentUser != null) {
                return currentUser;
            }

            var classesRoot = GetDefaultBrowserCommandCore(Registry.ClassesRoot, @"HTTP\shell\open\command");
            return classesRoot;
        }

        static ExecuteData GetBrowserPureExecuteDataCore(string browserCommand)
        {
            Debug.Assert(browserCommand != null);

            string applicationPath;
            int applicationSkipIndex;

            if(browserCommand.First() == '"') {
                // "xxx.exe" nn nnn 形式
                var basePath = browserCommand
                    .Skip(1)
                    .TakeWhile(c => c != '"')
                    .ToArray()
                ;
                applicationPath = new string(basePath);
                applicationSkipIndex = 1;
            } else {
                // xxx.exe nn nnn 形式
                var ext = ".exe";
                var index = browserCommand.IndexOf(ext, StringComparison.OrdinalIgnoreCase);
                applicationPath = browserCommand.Substring(0, index + ext.Length);
                applicationSkipIndex = 0;
            }

            var baseArguments = browserCommand
                .Skip(applicationSkipIndex)
                .Skip(applicationPath.Length)
                .Skip(applicationSkipIndex)
                .ToArray()
            ;
            var argument = new string(baseArguments);

            return new ExecuteData(applicationPath, argument.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)));
        }

        public static ExecuteData GetBrowserPureExecuteData(string browserCommand)
        {
            if(browserCommand == null) {
                throw new ArgumentNullException(nameof(browserCommand));
            }

            return GetBrowserPureExecuteDataCore(browserCommand);
        }

        public static ExecuteData GetOpenUriExecuteData(ExecuteData executeData, Uri uri)
        {
            IEnumerable<string> arguments;

            if(executeData.Arguments.Any(s => s.IndexOf("%1") != -1)) {
                arguments = executeData.Arguments.Select(s => s.Replace("%1", uri.OriginalString));
            } else {
                var list = executeData.Arguments.ToList();
                list.Add(uri.OriginalString);
                arguments = list;
            }

            return new ExecuteData(executeData.ApplicationPath, arguments);
        }

        static DirectoryInfo GetDownloadDirectory()
        {
            var setting = Mediation.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(RequestKind.Setting, ServiceType.Application));
            var downloadDirPath = setting.WebNavigator.DownloadDirectoryPath;
            if(!string.IsNullOrWhiteSpace(downloadDirPath)) {
                downloadDirPath = Environment.ExpandEnvironmentVariables(downloadDirPath);
            } else {
                downloadDirPath = Syroot.Windows.IO.KnownFolders.Downloads.ExpandedPath;
            }

            try {
                return Directory.CreateDirectory(downloadDirPath);
            } catch(Exception ex) {
                Mediation.Logger.Warning(ex);
            }

            downloadDirPath = Syroot.Windows.IO.KnownFolders.Downloads.ExpandedPath;

            try {
                return Directory.CreateDirectory(downloadDirPath);
            } catch(Exception ex) {
                Mediation.Logger.Error(ex);
            }
            // ここまで頑張って無理ならいっそ清々しい
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            return new DirectoryInfo(desktopPath);
        }

        static string GetDownloadFileName(string baseFIleName, Uri downloadUri)
        {
            var usingFileName = baseFIleName;

            if(string.IsNullOrWhiteSpace(usingFileName)) {
                usingFileName = downloadUri.Segments.LastOrDefault();
            }
            if(string.IsNullOrWhiteSpace(usingFileName)) {
                usingFileName = downloadUri.OriginalString;
            }
            return PathUtility.ToSafeNameDefault(usingFileName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>.なし</returns>
        static string GetDownloadFileExtension(string fileName)
        {
            var dotExt = Path.GetExtension(fileName);
            var usingExtension = allFilterExtension;
            if(!string.IsNullOrEmpty(dotExt) && 1 < dotExt.Length) {
                usingExtension = PathUtility.ToSafeNameDefault(dotExt.Substring(1));
            }

            return usingExtension;
        }

        static SaveFileDialog GetDownloadFileSaveDialog(DirectoryInfo directory, string baseFileName, string baseExtension)
        {
            var filter = new DialogFilterList();
            if(baseExtension != "*") {
                var pattern = $"*.{baseExtension}";
                filter.Add(new DialogFilterItem(baseExtension, pattern));
            }
            filter.Add(new DialogFilterItem(Properties.Resources.String_App_Browser_Download_AllFile_Display, Properties.Resources.String_App_Browser_Download_AllFile_Pattern));

            var downladFileName = baseFileName;
            directory.Refresh();
            if(directory.Exists) {
                // TODO: ワイルドカードは若干の妥協
                var files = directory.EnumerateFiles(filter.First().Wildcard.First(), SearchOption.TopDirectoryOnly)
                    .Select(f => f.Name)
                    .ToList()
                ;
                downladFileName = TextUtility.ToUnique(downladFileName, files, StringComparison.OrdinalIgnoreCase, (n, i) => {
                    var name = Path.GetFileNameWithoutExtension(n);
                    var ext = Path.GetExtension(n);

                    return $"{name}_{i}{ext}";
                });
            }

            var dialog = new SaveFileDialog() {
                Filter = filter.FilterText,
                InitialDirectory = directory.FullName,
                FileName = downladFileName,
            };

            return dialog;
        }

#endregion
    }
}
