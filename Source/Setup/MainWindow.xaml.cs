using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContentTypeTextNet.MnMn.Setup
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region function

        void ResetInstallDirectoryPath()
        {
            this.inputInstallDirectoryPath.Text = Constants.InstallDirectoryPath;
        }

        void AddLog(LogItem logItem)
        {
            this.listLog.Items.Add(logItem);
        }

        Task InstallCoreAsync(string installPath, bool createShortcut, bool installToExecute)
        {
            return Task.CompletedTask;
        }

        Task InstallAsync()
        {
            var installPath = inputInstallDirectoryPath.Text;
            var createShortcut = selectCreateShortcut.IsChecked.GetValueOrDefault();
            var installToExecute = selectInstallToExecute.IsChecked.GetValueOrDefault();

            AddLog(new LogItem(LogKind.Message, "start"));

            IsEnabled = false;
            return InstallCoreAsync(installPath, createShortcut, installToExecute).ContinueWith(_ => {
                IsEnabled = true;

                AddLog(new LogItem(LogKind.Message, "end"));
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        private void Window_Initialized(object sender, EventArgs e)
        {
            ResetInstallDirectoryPath();
        }

        private void commandResetInstallDirectoryPath_Click(object sender, RoutedEventArgs e)
        {
            ResetInstallDirectoryPath();
        }

        private void commandInstall_Click(object sender, RoutedEventArgs e)
        {
            InstallAsync();
        }

        private void commandClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
