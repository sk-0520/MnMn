#if RAM
#define DUMMY_WAIT
//#define _DUMMY_WAIT
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
using Microsoft.Win32;

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
            this.listLog.Dispatcher.BeginInvoke(new Action(() => {
                this.listLog.Items.Add(logItem);
            }));
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
            this.progressInformation.Value = 1;

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

            this.progressInformation.Value = 2;

            var archive = item.ArchiveElements
                .Select(x => new {
                    Uri = new Uri(x.Attribute("uri").Value),
                    Platform = x.Attribute("platform").Value,
                    Version = item.Version,
                })
                //.FirstOrDefault(x => x.Platform == (Environment.Is64BitProcess ? "x64" : "x86"))
                .FirstOrDefault(x => x.Platform == "x86")
            ;

            this.progressInformation.Value = 3;

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
#if DUMMY_WAIT
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

#if DUMMY_WAIT
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

        Task ExpandAsync(string archivePath, string installPath)
        {
            return Task.Run(() => {
                using(var archive = ZipFile.OpenRead(archivePath)) {
                    var entries = archive.Entries
                        .Where(e => e.Name.Length > 0)
                        .ToList()
                    ;

                    this.progressExpand.Dispatcher.Invoke(() => {
                        this.progressExpand.Maximum = entries.Count;
                    });

                    foreach(var entry in entries) {
                        var expandPath = System.IO.Path.Combine(installPath, entry.FullName);
                        var dirPath = System.IO.Path.GetDirectoryName(expandPath);
                        if(!Directory.Exists(dirPath)) {
                            Directory.CreateDirectory(dirPath);
                        }
                        AddMessageLog($"{expandPath}");
                        entry.ExtractToFile(expandPath, true);
                        this.progressExpand.Dispatcher.Invoke(() => {
                            this.progressExpand.Value += 1;
                        });

#if DUMMY_WAIT
                        Task.Delay(TimeSpan.FromMilliseconds(25)).Wait();
#endif
                    }
                }
            });
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

            var expandPath = Environment.ExpandEnvironmentVariables(installPath);
            if(!Directory.Exists(expandPath)) {
                Directory.CreateDirectory(expandPath);
            }
            await ExpandAsync(downloadPath, expandPath);

            // 各種登録処理開始
            var appPath = System.IO.Path.Combine(expandPath, Constants.ApplicationFileName);

            // レジストリ登録
            using(var reg = Registry.CurrentUser.CreateSubKey(Constants.BaseRegistryPath, true)) {
                reg.SetValue(Constants.ApplicationPathName, appPath);
            }


        }


        Task InstallAsync()
        {
            var installPath = inputInstallDirectoryPath.Text;
            var createShortcut = selectCreateShortcut.IsChecked.GetValueOrDefault();
            var installToExecute = selectInstallToExecute.IsChecked.GetValueOrDefault();

            this.progressInformation.Value = 0;
            this.progressDownload.Value = 0;
            this.progressExpand.Value = 0;

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
