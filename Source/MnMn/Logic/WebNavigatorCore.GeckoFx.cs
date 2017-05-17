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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Data.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Order;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge;
using Gecko;
using Gecko.Cache;
using Microsoft.Win32;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    partial class WebNavigatorCore
    {
        #region property

        static ISet<ServiceGeckoWebBrowser> CreatedGeckoBrowsers { get; } = new HashSet<ServiceGeckoWebBrowser>();

        #endregion

        #region function

        static void InitializeGeckoExtension(string targetDirectoryPath)
        {
            var chromeDir = (nsIFile)Xpcom.NewNativeLocalFile(targetDirectoryPath);
            var chromeFile = chromeDir.Clone();
            chromeFile.Append(new nsAString("chrome.manifest"));
            Xpcom.ComponentRegistrar.AutoRegister(chromeFile);
        }

        static void InitializeGecko()
        {
            var setting = Mediation.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(RequestKind.Setting, ServiceType.Application));

            var settingDirectory = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Application));
            var profileDirectoryPath = Path.Combine(settingDirectory.FullName, Constants.WebNavigatorGeckoFxProfileDirectoryName);
            var profileDirectory = Directory.CreateDirectory(profileDirectoryPath);
            profileDirectory.Refresh();
            Xpcom.EnableProfileMonitoring = false;
            Xpcom.ProfileDirectory = profileDirectory.FullName;
            Xpcom.Initialize(Constants.WebNavigatorGeckoFxLibraryDirectoryPath);

            GeckoPreferences.Default["extensions.blocklist.enabled"] = false;
            GeckoPreferences.Default["plugin.scan.plid.all"] = setting.WebNavigator.GeckoFxScanPlugin;
            GeckoPreferences.Default["security.fileuri.strict_origin_policy"] = false;
            GeckoPreferences.User["security.fileuri.strict_origin_policy"] = false;

            GeckoPreferences.User["browser.cache.disk.enable"] = false;
            GeckoPreferences.User["browser.cache.disk.capacity"] = 0;

            if(NetworkSetting.BrowserUsingCustomUserAgent) {
                var userAgentText = NetworkUtility.GetUserAgentText(NetworkSetting.BrowserCustomUserAgentFormat);
                if(!string.IsNullOrEmpty(userAgentText)) {
                    GeckoPreferences.User["general.useragent.override"] = userAgentText;
                }
            }

            var preferencesFilePath = Path.Combine(profileDirectory.FullName, Constants.WebNavigatorGeckoFxPreferencesFileName);
            if(File.Exists(preferencesFilePath)) {
                //GeckoPreferences.Load(preferencesFilePath);
            } else {
                // http://pieceofnostalgy.blogspot.jp/2013/10/wpf-geckofx.html
                GeckoPreferences.User["browser.cache.disk.enable"] = false;
                GeckoPreferences.User["browser.cache.disk.capacity"] = 0;
                //GeckoPreferences.Default["plugins.load_appdir_plugins"] = true;

                GeckoPreferences.Save(preferencesFilePath);
            }
            var langs = new[] {
                AppUtility.GetCultureLanguage(),
                AppUtility.GetCultureName(),
                "en-US",
                "en"
            };
            GeckoPreferences.User["intl.accept_languages"] = string.Join(",", langs);
            GeckoPreferences.User["font.language.group"] = AppUtility.GetCultureLanguage();

            var historyMan = Xpcom.GetService<nsIBrowserHistory>(Gecko.Contracts.NavHistoryService);
            historyMan = Xpcom.QueryInterface<nsIBrowserHistory>(historyMan);
            historyMan.RemoveAllPages();

            //CacheService.Clear(new CacheStoragePolicy());
            ImageCache.ClearCache(true);
            ImageCache.ClearCache(false);

            // 拡張機能
            foreach(var targetDirPath in Directory.GetDirectories(Constants.WebNavigatorGeckoFxExtensionsDirectoryPath)) {
                InitializeGeckoExtension(targetDirPath);
            }

            LauncherDialog.Download += LauncherDialog_Download;
        }

        static void UninitializeGecko()
        {
            LauncherDialog.Download -= LauncherDialog_Download;

            foreach(var browser in CreatedGeckoBrowsers) {
                browser.Disposed -= GeckoBrowser_Disposed;
                try {
                    browser.Dispose();
                } catch(Exception ex) {
                    Mediation.Logger.Error(ex);
                }
            }
            Xpcom.Shutdown();

            IsUninitialized = true;
            IsInitialized = false;
        }

        /// <summary>
        /// GeckFxのブラウザ生成。
        /// </summary>
        /// <returns></returns>
        public static ServiceGeckoWebBrowser CreateGeckoBrowser()
        {
            ServiceGeckoWebBrowser browser = null;

            Application.Current.Dispatcher.Invoke(() => {
                browser = new ServiceGeckoWebBrowser(Mediation) {
                    Dock = System.Windows.Forms.DockStyle.Fill,
                };
                browser.Disposed += GeckoBrowser_Disposed;
                CreatedGeckoBrowsers.Add(browser);
            });

            return browser;
        }

        public static IEnumerable<GeckoElement> GetRootElementsGeckoFx(GeckoElement lastElement)
        {
            var parent = lastElement.ParentElement;

            if(parent == null) {
                return Enumerable.Empty<GeckoElement>();
            }

            var result = new List<GeckoElement>();

            while(parent != null) {
                result.Add(parent);

                parent = parent.ParentElement;
            };

            result.Reverse();

            return result;
        }

        public static SimpleHtmlElement ConvertSimleHtmlElementGeckoFx(GeckoElement element)
        {
            var result = new SimpleHtmlElement() {
                Name = element.TagName,
                Attributes = element.Attributes
                    .GroupBy(a => a.NodeName)
                    .ToDictionary(a => a.Key, a => a.First().NodeValue)
                ,
            };

            return result;
        }


        #endregion

        private static void GeckoBrowser_Disposed(object sender, EventArgs e)
        {
            var browser = (ServiceGeckoWebBrowser)sender;

            browser.Disposed -= GeckoBrowser_Disposed;
            CreatedGeckoBrowsers.Remove(browser);
        }

        // TODO: セッションがぁぁぁぁ。直接ストリームほしいなぁぁぁ。
        // TODO: ブラウザ自体がほしいなぁぁぁ。
        private static void LauncherDialog_Download(object sender, LauncherDialogEvent e)
        {
            Uri downloadUri;
            if(!Uri.TryCreate(e.Url, UriKind.RelativeOrAbsolute, out downloadUri)) {
                e.Cancel();
                return;
            }

            var downloadDirectory = GetDownloadDirectory();
            var baseFileName = GetDownloadFileName(e.Filename, downloadUri);
            var extension = GetDownloadFileExtension(baseFileName);

            var dialog = GetDownloadFileSaveDialog(downloadDirectory, baseFileName, extension);
            if(!dialog.ShowDialog().GetValueOrDefault()) {
                e.Cancel();
                return;
            }

            var downloadFilePath = dialog.FileName;
            var downloadFile = new FileInfo(downloadFilePath);

            var download = new WebNavigatorFileDownloadItemViewModel(Mediation, downloadUri, downloadFile, new HttpUserAgentHost(NetworkSetting));
            download.LoadImageAsync();

            Mediation.Order(new DownloadOrderModel(download, true, ServiceType.Application));
            e.Cancel();
        }

    }
}
