using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.MnMn.Applications.CrashSender
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App: Application
    {
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

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
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;

            base.OnExit(e);
        }

        #endregion

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name).Name + ".dll";
            var path = Path.Combine(Constants.LibraryDirectoryPath, name);
            var absPath = Path.GetFullPath(path);
            if(File.Exists(absPath)) {
                var asm = Assembly.LoadFrom(absPath);
                return asm;
            }

            // 見つかんないともう何もかもおかしい、と思ったけど resource.dll もこれで飛んでくんのかい
            //throw new FileNotFoundException(absPath);
            return null;
        }
    }
}
