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
using Microsoft.Win32.SafeHandles;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppInformationManagerViewModel: ManagerViewModelBase
    {
        public AppInformationManagerViewModel(Mediation mediation)
            : base(mediation)
        { }

        #region property

        WebBrowser HelpBrowser { get; set; }



        #endregion

        #region command



        public ICommand ExecuteCommand
        {
            get
            {
                return CreateCommand(o => {
                    var s = (string)o;
                    try {
                        Process.Start(s);
                    } catch(Exception ex) {
                        Mediation.Logger.Warning(ex);
                    }
                });
            }
        }

        #endregion

        #region function

        #endregion

        #region ManagerViewModelBase

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        {
            HelpBrowser = view.information.helpBrowser;
        }

        public override void UninitializeView(MainWindow view)
        { }

        public override Task GarbageCollectionAsync()
        {
            return Task.CompletedTask;
        }

        protected override void ShowView()
        {
            base.ShowView();

            if(HelpBrowser.Source == null) {
                HelpBrowser.Source = new Uri(Constants.HelpFilePath);
            }
        }

        #endregion
    }
}
