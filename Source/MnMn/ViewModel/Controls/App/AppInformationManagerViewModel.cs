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
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.Pe.PeMain.Logic;
using Microsoft.Win32.SafeHandles;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppInformationManagerViewModel: ManagerViewModelBase
    {
        public AppInformationManagerViewModel(Mediation mediation, AppLogger appLogger)
            : base(mediation)
        {
            AppAboutManager = new AppAboutManagerViewModel(Mediation);
            AppLoggingManager = new AppLoggingManagerViewModel(Mediation, appLogger);
            AppIssueManager = new AppIssueManagerViewModel(Mediation);
            AppForumManager = new AppForumManagerViewModel(Mediation);
            AppHelpManager = new AppHelpManagerViewModel(Mediation);
            AppSettingManager = new AppSettingManagerViewModel(Mediation);
            AppFlashPlayerManager = new AppFlashPlayerManagerViewModel(Mediation);
        }

        #region property

        WebBrowser HelpBrowser { get; set; }

        public AppAboutManagerViewModel AppAboutManager { get; }
        public AppLoggingManagerViewModel AppLoggingManager { get; }
        public AppIssueManagerViewModel AppIssueManager { get; }
        public AppForumManagerViewModel AppForumManager { get; }
        public AppHelpManagerViewModel AppHelpManager { get; }
        public AppSettingManagerViewModel AppSettingManager { get; }
        public AppFlashPlayerManagerViewModel AppFlashPlayerManager { get; }

        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return new ManagerViewModelBase[] {
                AppAboutManager,
                AppLoggingManager,
                AppIssueManager,
                AppForumManager,
                AppHelpManager,
                AppSettingManager,
                AppFlashPlayerManager
            };
        }

        protected override void ShowViewCore()
        { }

        protected override void HideViewCore()
        { }

        public override Task InitializeAsync()
        {
            return Task.WhenAll(ManagerChildren.Select(m => m.InitializeAsync()));
        }

        public override void InitializeView(MainWindow view)
        {
            foreach(var child in ManagerChildren) {
                child.InitializeView(view);
            }
        }

        public override void UninitializeView(MainWindow view)
        {
            foreach(var child in ManagerChildren) {
                child.UninitializeView(view);
            }
        }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return Task.WhenAll(ManagerChildren.Select(m => m.GarbageCollectionAsync(garbageCollectionLevel, cacheSpan, force))).ContinueWith(t => {
                return t.Result.Sum();
            });
        }

        #endregion
    }
}
