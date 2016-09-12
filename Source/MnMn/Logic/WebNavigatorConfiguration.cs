﻿/*
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
using ContentTypeTextNet.MnMn.MnMn.Define;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    /// <summary>
    /// <see cref="MnMn.View.Controls.WebNavigator"/> で動作するブラウザ挙動の担当。
    /// </summary>
    public static class WebNavigatorConfiguration
    {
        #region property

        public static WebNavigatorEngine Engine { get; } = WebNavigatorEngine.GeckoFx;

        static ISet<GeckoWebBrowser> CreatedGeckoBrowsers { get; } = new HashSet<GeckoWebBrowser>();

        #endregion

        #region function

        static void InitializeGecko()
        {
            var settingDirectory = VariableConstants.GetSettingDirectory();
            var profileDirectoryPath = Path.Combine(settingDirectory.FullName, Constants.BrowserProfileDirectoryName);
            var profileDirectory = Directory.CreateDirectory(profileDirectoryPath);
            Xpcom.ProfileDirectory = profileDirectory.FullName;
            Xpcom.Initialize(Constants.BrowserLibraryDirectoryPath);

            var preferencesFilePath = Path.Combine(profileDirectory.FullName, Constants.BrowserPreferencesFileName);
            if(File.Exists(preferencesFilePath)) {
                GeckoPreferences.Load(preferencesFilePath);
            } else {
                // http://pieceofnostalgy.blogspot.jp/2013/10/wpf-geckofx.html
                GeckoPreferences.User["browser.cache.disk.enable"] = false;
                GeckoPreferences.User["browser.cache.disk.capacity"] = 0;
                GeckoPreferences.Save(preferencesFilePath);
            }
        }

        /// <summary>
        /// 初期化。
        /// </summary>
        public static void Initialize()
        {
            InitializeGecko();
        }

        static void UninitializeGecko()
        {
            foreach(var browser in CreatedGeckoBrowsers) {
                browser.Disposed -= GeckoBrowser_Disposed;
                browser.Dispose();
            }
            Xpcom.Shutdown();
        }

        /// <summary>
        /// さよなら。
        /// </summary>
        public static void Uninitialize()
        {
            UninitializeGecko();
        }

        /// <summary>
        /// GeckFxのブラウザ生成。
        /// <para><see cref="Engine"/>には影響されない。</para>
        /// <para>あくまで呼び出し側で<see cref="WebNavigatorEngine.GeckoFx"/>を保証すること</para>
        /// </summary>
        /// <returns></returns>
        public static GeckoWebBrowser CreateBrowser()
        {
            GeckoWebBrowser browser = null;

            Application.Current.Dispatcher.Invoke(() => {
                browser = new GeckoWebBrowser() {
                    Dock = System.Windows.Forms.DockStyle.Fill,
                };
                browser.Disposed += GeckoBrowser_Disposed;
                CreatedGeckoBrowsers.Add(browser);
            });

            return browser;
        }

        private static void GeckoBrowser_Disposed(object sender, EventArgs e)
        {
            var browser = (GeckoWebBrowser)sender;

            browser.Disposed -= GeckoBrowser_Disposed;
            CreatedGeckoBrowsers.Remove(browser);
        }

        #endregion
    }
}
