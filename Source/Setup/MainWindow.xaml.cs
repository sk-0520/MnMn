using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using System.Xml.Linq;

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

        void AddMessageLog(string message)
        {
            AddLog(new LogItem(LogKind.Message, message));
        }

        async Task<Tuple<Uri, Version>> GetArchiveUriAsync()
        {
            AddMessageLog("get archive uri");

            var client = new HttpClient();

            var xmlSource = await client.GetStringAsync(Constants.UpdateUri);

            var xml = XElement.Parse(xmlSource);

            var item = xml
                .Elements()
                .Select(
                    x => new {
                        Version = new Version(x.Attribute("version").Value),
                            //IsRC = x.Attribute("type").Value == "rc",
                            ArchiveElements = x.Elements(),
                    }
                )
                //.Where(x => Constants.ApplicationVersionNumber <= x.Version)
                .OrderByDescending(x => x.Version)
                .FirstOrDefault()
            ;

            var archive = item.ArchiveElements
                .Select(x => new {
                    Uri = new Uri(x.Attribute("uri").Value),
                    Platform = x.Attribute("platform").Value,
                    Version = item.Version,
                })
                //.FirstOrDefault(x => x.Platform == (Environment.Is64BitProcess ? "x64" : "x86"))
                .FirstOrDefault(x => x.Platform == "x86")
            ;

            return Tuple.Create(archive.Uri, archive.Version) ;
        }

        async Task<bool> DownloadArchiveAsync(Uri uri, string savePath)
        {
            var client = new HttpClient();

            var downloadTotalSize = 0;

            var response = await client.GetAsync(uri);

            var downloadFileSize = response.Content.Headers.ContentLength;
            if(!downloadFileSize.HasValue) {
                this.progressDownload.IsIndeterminate = true;
            }

            using(var archiveStream = await response.Content.ReadAsStreamAsync()) {
                var buffer = new byte[Constants.DownloadBufferSize];
                using(var localStream = new FileStream(savePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)) {
#if RAM
                    var _i = 0;
#endif
                    while(true) {
                        var downloadCurrentSize = archiveStream.Read(buffer, 0, buffer.Length);

                        if(downloadCurrentSize == 0) {
                            break;
                        }

                        if(0 < downloadCurrentSize) {
                            downloadTotalSize += downloadCurrentSize;
                            localStream.Write(buffer, 0, downloadCurrentSize);

                            if(downloadFileSize.HasValue) {
                                var percent = ((double)downloadTotalSize / (double)downloadFileSize.Value) * 100.0;
                                this.progressDownload.Value = percent;
                            }
                        }

#if RAM
                        if(10 < _i++) {
                        await Task.Delay(TimeSpan.FromMilliseconds(5));
                            _i = 0;
                        }
#endif
                    }
                }
            }

            return true;
        }

        async Task InstallCoreAsync(string installPath, bool createShortcut, bool installToExecute)
        {
            var archiveInfo = await GetArchiveUriAsync();
            AddMessageLog($"archive uri: {archiveInfo.Item1} , {archiveInfo.Item2}");

            // bitbucket を信じる！
            var downloadName = archiveInfo.Item1.Segments.Last();
            var downloadDirPath = Environment.ExpandEnvironmentVariables(Constants.ArchiveDirectoryPath);
            if(!Directory.Exists(downloadDirPath)) {
                Directory.CreateDirectory(downloadDirPath);
            }
            var downloadPath = System.IO.Path.Combine(downloadDirPath, downloadName);
            await DownloadArchiveAsync(archiveInfo.Item1, Environment.ExpandEnvironmentVariables(downloadPath));
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
