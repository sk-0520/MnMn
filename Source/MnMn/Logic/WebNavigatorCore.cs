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
    public static partial class WebNavigatorCore
    {
        #region property

        static Mediation Mediation { get; set; }

        //public static WebNavigatorEngine Engine { get; } = WebNavigatorEngine.Default;
        public static WebNavigatorEngine Engine { get; } = Constants.WebNavigatorEngine;

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
            Application.Current.Dispatcher.Invoke(() => {
                session.ApplyToWebNavigatorEngine(Engine, uri);
            });
        }

        #endregion
    }
}
