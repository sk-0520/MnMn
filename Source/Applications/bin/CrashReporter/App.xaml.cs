using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.MnMn.Common;

namespace ContentTypeTextNet.MnMn.Applications.CrashReporter
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App: Application
    {
        public App()
        {
            AssemblyResolveHelper = new AssemblyResolveHelper(Constants.LibraryDirectoryPath);
        }

        #region property

        AssemblyResolveHelper AssemblyResolveHelper { get; }

        #endregion

        #region Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var viewModel = new MainWorkerViewModel();
            try {
                viewModel.Initialize();
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
                Shutdown(1);
            }

            var mainWindow = new MainWindow() {
                DataContext = viewModel,
            };
            viewModel.SetView(mainWindow);

            MainWindow = mainWindow;
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AssemblyResolveHelper.Dispose();

            base.OnExit(e);
        }

        #endregion
    }
}
