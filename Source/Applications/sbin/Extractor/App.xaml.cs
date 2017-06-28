using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.MnMn.SystemApplications.Extractor
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        #region Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var viewModel = new MainWorkerViewModel();
            var view = new MainWindow() {
                DataContext = viewModel,
            };
            viewModel.SetView(view);
            viewModel.Initialize();

            view.Show();MessageBox.Show(Environment.ExpandEnvironmentVariables("%Path%"));
        }

        #endregion
    }
}
