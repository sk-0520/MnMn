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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppHelpManagerViewModel: ManagerViewModelBase
    {
        #region variable

        bool _showHelpOpenWebBrowserTips = true;

        #endregion

        public AppHelpManagerViewModel(Mediator mediator)
            : base(mediator)
        { }

        #region property

        WebNavigator HelpBrowser { get; set; }

        public bool ShowHelpOpenWebBrowserTips
        {
            get { return this._showHelpOpenWebBrowserTips; }
            set { SetVariableValue(ref _showHelpOpenWebBrowserTips, value); }
        }

        #endregion

        #region command

        public ICommand HideHelpOpenWebBrowserTipsCommand
        {
            get { return CreateCommand(o => ShowHelpOpenWebBrowserTips = false); }
        }

        public ICommand NewWindowCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var data = (WebNavigatorEventDataBase)o;
                        WebNavigatorUtility.OpenNewWindowWrapper(data, Mediation.Logger);
                    }
                );
            }
        }

        #endregion

        #region function

        #endregion

        #region ManagerViewModelBase

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void ShowViewCore()
        {
            if(HelpBrowser.IsEmptyContent) {
                var helpUri = new Uri(Constants.HelpFilePath);
                HelpBrowser.HomeSource = helpUri;
                //HelpBrowser.Source = helpUri;
                HelpBrowser.Navigate(helpUri);
            }
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

        public override Task UninitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
            HelpBrowser = view.information.helpBrowser;
        }

        public override void UninitializeView(MainWindow view)
        {
        }

        #endregion
    }
}
