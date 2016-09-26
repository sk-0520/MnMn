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
using System.Windows.Controls;
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

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// <see cref="MnMn.View.Controls.WebNavigator"/> で動作するブラウザ挙動の担当。
    /// </summary>
    public static class WebNavigatorCore
    {
        #region property

        static Mediation Mediation { get; set; }

        //public static WebNavigatorEngine Engine { get; } = WebNavigatorEngine.Default;
        public static WebNavigatorEngine Engine { get; } = Constants.WebNavigatorEngine;

        static ISet<ServiceGeckoWebBrowser> CreatedGeckoBrowsers { get; } = new HashSet<ServiceGeckoWebBrowser>();

        static bool IsInitialized { get; set; } = false;
        static bool IsUninitialized { get; set; } = false;

        #endregion

        #region function

        static void InitializeDefault()
        {
            var ieVersion = SystemEnvironmentUtility.GetInternetExplorerVersion();
            Mediation.Logger.Information("IE version: " + ieVersion);
            SystemEnvironmentUtility.SetUsingBrowserVersionForExecutingAssembly(ieVersion);
        }

        static void InitializeGecko()
        {
            var setting = Mediation.GetResultFromRequest<AppSettingModel>(new Model.Request.RequestModel(RequestKind.Setting, ServiceType.Application));

            var settingDirectory = Mediation.GetResultFromRequest<DirectoryInfo>(new RequestModel(RequestKind.CacheDirectory, ServiceType.Application));
            var profileDirectoryPath = Path.Combine(settingDirectory.FullName, Constants.WebNavigatorGeckoFxProfileDirectoryName);
            var profileDirectory = Directory.CreateDirectory(profileDirectoryPath);
            Xpcom.ProfileDirectory = profileDirectory.FullName;
            Xpcom.Initialize(Constants.WebNavigatorGeckoFxLibraryDirectoryPath);

            GeckoPreferences.Default["extensions.blocklist.enabled"] = false;
            GeckoPreferences.Default["plugin.scan.plid.all"] = setting.WebNavigator.GeckoFxScanPlugin;
            GeckoPreferences.Default["security.fileuri.strict_origin_policy"] = false;
            GeckoPreferences.User["devtools.debugger.remote-enabled"] = true;

            var preferencesFilePath = Path.Combine(profileDirectory.FullName, Constants.WebNavigatorGeckoFxPreferencesFileName);
            if(File.Exists(preferencesFilePath)) {
                GeckoPreferences.Load(preferencesFilePath);
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
            CookieManager.RemoveAll();
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
                    InitializeDefault();
                    break;

                case WebNavigatorEngine.GeckoFx:
                    InitializeGecko();
                    break;

                default:
                    break;
            }

            IsInitialized = true;
            IsUninitialized = false;
        }

        static void UninitializeDefault()
        {
            SystemEnvironmentUtility.ResetUsingBrowserVersionForExecutingAssembly();
        }

        static void UninitializeGecko()
        {
            foreach(var browser in CreatedGeckoBrowsers) {
                browser.Disposed -= GeckoBrowser_Disposed;
                browser.Dispose();
            }
            Xpcom.Shutdown();

            IsUninitialized = true;
            IsInitialized = false;
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

            // #159
            UninitializeDefault();
        }

        /// <summary>
        /// セッションをエンジンに設定。
        /// <para><see cref="WebBrowser"/>? しらん。</para>
        /// </summary>
        /// <param name="session"></param>
        /// <param name="uri"></param>
        public static void SetSessionEngine(SessionViewModelBase session, Uri uri)
        {
            session.ApplyToWebNavigatorEngine(Engine, uri);
        }

        /// <summary>
        /// GeckFxのブラウザ生成。
        /// <para><see cref="Engine"/>には影響されない。</para>
        /// <para>あくまで呼び出し側で<see cref="WebNavigatorEngine.GeckoFx"/>を保証すること</para>
        /// </summary>
        /// <returns></returns>
        public static ServiceGeckoWebBrowser CreateBrowser()
        {
            ServiceGeckoWebBrowser browser = null;

            Application.Current.Dispatcher.Invoke(() => {
                browser = new ServiceGeckoWebBrowser() {
                    Dock = System.Windows.Forms.DockStyle.Fill,
                };
                browser.Disposed += GeckoBrowser_Disposed;
                CreatedGeckoBrowsers.Add(browser);
            });

            return browser;
        }

        private static void GeckoBrowser_Disposed(object sender, EventArgs e)
        {
            var browser = (ServiceGeckoWebBrowser)sender;

            browser.Disposed -= GeckoBrowser_Disposed;
            CreatedGeckoBrowsers.Remove(browser);
        }

        #endregion
    }
}
