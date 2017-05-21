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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
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
    partial class WebNavigatorCore
    {
        #region define

        enum UOU
        {
            URLMON_OPTION_USERAGENT = 0x10000001,
            URLMON_OPTION_USERAGENT_REFRESH = 0x10000002
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(UOU dwOption, string pBuffer, int dwBufferLength, int dwReserved);

        #endregion

        #region function

        static void InitializeDefault()
        {
            var ieVersion = SystemEnvironmentUtility.GetInternetExplorerVersion();
            Mediation.Logger.Information("IE version: " + ieVersion);
            SystemEnvironmentUtility.SetUsingBrowserVersionForExecutingAssembly(ieVersion);

            if(NetworkSetting.BrowserUserAgent.UsingCustomUserAgent) {
                var userAgentText = NetworkUtility.GetUserAgentText(NetworkSetting.BrowserUserAgent.CustomUserAgentFormat);
                if(!string.IsNullOrEmpty(userAgentText)) {
                    UrlMkSetSessionOption(UOU.URLMON_OPTION_USERAGENT, userAgentText, userAgentText.Length, 0);
                }
            }
        }


        public static WebBrowser CreateDefaultBrowser()
        {
            WebBrowser browser = null;
            Application.Current.Dispatcher.Invoke(() => {
                browser = new WebBrowser() {
                    Tag = new WebNavigatorTagModel() {
                        Mediation = Mediation,
                    },
                };
            });

            return browser;
        }

        #endregion
    }
}
