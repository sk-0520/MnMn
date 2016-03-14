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
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using ContentTypeTextNet.MnMn.MnMn.ViewModel;
using MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App: Application
    {
        App()
        {
            SplashWindow = new SplashWindow();
        }

        #region property

        SplashWindow SplashWindow { get; }

        #endregion

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if DEBUG
            DoDebug();
#endif
            var viewModel = new AppManagerViewModel();

            SplashWindow.Show();

            await viewModel.InitializeAsync();
            MainWindow = new MainWindow() {
                DataContext = viewModel,
            };
            MainWindow.Show();
            SplashWindow.Close();
        }
    }
}
