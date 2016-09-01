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
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppIssueManagerViewModel: ManagerViewModelBase
    {
        public AppIssueManagerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

        WebBrowser IssueBrowser { get; set; }

        #endregion

        #region command

        public ICommand ReloadCommand
        {
            get
            {
                return CreateCommand(o => {
                    if(IssueBrowser.Source == null) {
                        // 初回表示は ShowView に任せる
                        return;
                    }
                    // ページ変わってるかもしれないから指定ページを読み込み
                    IssueBrowser.Navigate(Constants.AppUriIssueResolved);
                });
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
            if(IssueBrowser.Source == null) {
                // http://stackoverflow.com/questions/6138199/wpf-webbrowser-control-how-to-supress-script-errors
                dynamic activeX = IssueBrowser.GetType().InvokeMember(
                    "ActiveXInstance",
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    IssueBrowser,
                    new object[] { }
                );
                activeX.Silent = true;

                IssueBrowser.Navigate(Constants.AppUriIssueResolved);
            }
        }

        protected override void HideViewCore()
        { }

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
            IssueBrowser = view.information.issueBrowser;
        }

        public override void UninitializeView(MainWindow view)
        {
            IssueBrowser = null;
        }

        #endregion
    }
}
