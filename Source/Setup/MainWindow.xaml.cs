#if RAM
//#define DUMMY_WAIT
#define _DUMMY_WAIT
#endif

/*
 * ようこそ古き良き Forms 時代へ！
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
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
using System.Windows.Threading;
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
                Trace.WriteLine($"{logItem.Timestamp}, {logItem.Kind}, {logItem.Message}");
                this.listLog.Items.Add(logItem);
                this.listLog.ScrollIntoView(logItem);
            }));
        }

        void AddMessageLog(string message)
        {
            AddLog(new LogItem(LogKind.Message, message));
        }

        static void SetProgressValue(ProgressBar progressBar, double value)
        {
            progressBar.Dispatcher.Invoke(() => {
                progressBar.Value = value;
            }, DispatcherPriority.ApplicationIdle);
        }

        void SetDisabled(bool isDisabled)
        {
            var controls = new Control[] {
                this.inputInstallDirectoryPath,
                this.commandResetInstallDirectoryPath,
                this.selectCreateShortcut,
                this.selectInstallToExecute,
                this.commandInstall,
                this.commandClose,
            };

            foreach(var control in controls) {
                control.IsEnabled = !isDisabled;
            }
        }

        HttpClient CreateHttpClient()
        {
            var client = new HttpClient() {
                Timeout = Constants.HttpWaitTime,
            };

            return client;
        }

        async Task<Tuple<Uri, Version>> GetArchiveInformationAsync()
        {
            AddMessageLog(Properties.Resources.String_GetArchiveInformation);

            var client = CreateHttpClient();

            var response = await client.GetAsync(Constants.UpdateUri, HttpCompletionOption.ResponseHeadersRead);
            AddMessageLog(
                string.Format(
                    Properties.Resources.String_Http_Response_Header_Format,
                    response.IsSuccessStatusCode,
                    response.StatusCode
                )
            );

            var xmlSource = await response.Content.ReadAsStringAsync();
            SetProgressValue(this.progressInformation, 1);

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

            SetProgressValue(this.progressInformation, 2);

            var archive = item.ArchiveElements
                .Select(x => new {
                    Uri = new Uri(x.Attribute("uri").Value),
                    Platform = x.Attribute("platform").Value,
                    Version = item.Version,
                })
                //.FirstOrDefault(x => x.Platform == (Environment.Is64BitProcess ? "x64" : "x86"))
                .FirstOrDefault(x => x.Platform == "x86")
            ;

            SetProgressValue(this.progressInformation, 3);

            return Tuple.Create(archive.Uri, archive.Version) ;
        }

        async Task<bool> DownloadArchiveAsync(Uri uri, string savePath)
        {
            AddMessageLog(Properties.Resources.String_DownloadArchive);

            var client = CreateHttpClient();

            var downloadTotalSize = 0;

            var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            AddMessageLog(
                string.Format(
                    Properties.Resources.String_Http_Response_Header_Format,
                    response.IsSuccessStatusCode,
                    response.StatusCode
                )
            );


            var downloadFileSize = response.Content.Headers.ContentLength;
            if(!downloadFileSize.HasValue) {
                AddMessageLog(Properties.Resources.String_DownloadArchive_Disabled_ContentLength);
                this.progressDownload.IsIndeterminate = true;
            } else {
                AddMessageLog(
                    string.Format(
                        Properties.Resources.String_DownloadArchive_Enabled_ContentLength_Format,
                        downloadFileSize.Value
                    )
                );
            }

            AddMessageLog(Properties.Resources.String_DownloadArchive_Start);

            var sw = new Stopwatch();
            sw.Start();

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
                                SetProgressValue(this.progressDownload, percent);
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

            sw.Stop();
            AddMessageLog(
                string.Format(
                    Properties.Resources.String_DownloadArchive_End_Format,
                    sw.Elapsed
                )
            );

            return true;
        }

        Task ExpandAsync(string archivePath, string installPath)
        {
            AddMessageLog(Properties.Resources.String_Expand);

            return Task.Run(() => {
                using(var archive = ZipFile.OpenRead(archivePath)) {
                    var entries = archive.Entries
                        .Where(e => e.Name.Length > 0)
                        .ToList()
                    ;

                    this.progressExpand.Dispatcher.Invoke(() => {
                        this.progressExpand.Maximum = entries.Count;
                    });

                    int fileCount = 0;

                    foreach(var entry in entries) {
                        var expandPath = System.IO.Path.Combine(installPath, entry.FullName);
                        var dirPath = System.IO.Path.GetDirectoryName(expandPath);
                        if(!Directory.Exists(dirPath)) {
                            Directory.CreateDirectory(dirPath);
                        }

                        AddMessageLog($"{entry.FullName}, {entry.Length} byte");
                        entry.ExtractToFile(expandPath, true);
                        SetProgressValue(this.progressExpand, ++fileCount);
#if DUMMY_WAIT
                        Task.Delay(TimeSpan.FromMilliseconds(25)).Wait();
#endif
                    }
                }
            });
        }

        async Task InstallCoreAsync(string installPath, bool createShortcut, bool installToExecute)
        {
            var archiveInfo = await GetArchiveInformationAsync();
            AddMessageLog(
                string.Format(
                    Properties.Resources.String_GetArchiveInformation_Fromat,
                    archiveInfo.Item1,
                    archiveInfo.Item2
                )
            );

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
            var uninstallBatchFilePath = System.IO.Path.Combine(expandPath, "bat", "uninstall.bat");

            if(createShortcut) {
                // 解放？ いらんいらん

                var link = (IShellLink)new ShellLink();
                link.SetDescription(Constants.ProjectName);
                link.SetPath(appPath);

                var file = (IPersistFile)link;
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                file.Save(System.IO.Path.Combine(desktopPath, Constants.ProjectName + ".lnk"), false);
            }

            if(installToExecute) {
                // 10秒の待機時間にすべてを託して起動
                Process.Start(appPath);
            }

            // ばいびー
            Close();
        }


        async Task InstallAsync()
        {
            var installPath = this.inputInstallDirectoryPath.Text;
            var createShortcut = this.selectCreateShortcut.IsChecked.GetValueOrDefault();
            var installToExecute = this.selectInstallToExecute.IsChecked.GetValueOrDefault();

            this.progressInformation.Value = 0;
            this.progressDownload.Value = 0;
            this.progressExpand.Value = 0;

            this.listLog.Items.Clear();
            AddMessageLog(Properties.Resources.String_App_Start);

            SetDisabled(true);

            try {
                await InstallCoreAsync(installPath, createShortcut, installToExecute);
            } catch(Exception ex) {
                AddMessageLog(ex.ToString());
            } finally {
                SetDisabled(false);
                AddMessageLog(Properties.Resources.String_App_End);
            }
        }

        void OpenLink(Uri uri)
        {
            try {
                Process.Start(uri.OriginalString);
            }catch(Exception ex) {
                AddLog(new LogItem(LogKind.Warning, ex.ToString()));
            }
        }

#endregion

        private void Window_Initialized(object sender, EventArgs e)
        {
            var asm = Assembly.GetExecutingAssembly().GetName();
            this.textVersion.Text = $"{asm.Name}: {asm.Version}";
            ResetInstallDirectoryPath();
        }

        private void commandResetInstallDirectoryPath_Click(object sender, RoutedEventArgs e)
        {
            ResetInstallDirectoryPath();
        }

        private void commandInstall_Click(object sender, RoutedEventArgs e)
        {
            InstallAsync().ConfigureAwait(false);
        }

        private void commandClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void commandOpenProject_Click(object sender, RoutedEventArgs e)
        {
            OpenLink(Constants.ProjectUri);
        }

        private void commandOpenLicense_Click(object sender, RoutedEventArgs e)
        {
            OpenLink(Constants.LicenseUri);
        }

        private void commandOpenHelp_Click(object sender, RoutedEventArgs e)
        {
            OpenLink(Constants.HelpUri);
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if(e.OriginalSource != null) {
                var run = e.OriginalSource as Run;
                if(run != null && run.Parent == this.commandOpenHelp) {
                    return;
                }
            }

            if(e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.listLog.Dispatcher.Invoke(() => {
                if(0 < this.listLog.Items.Count) {
                    var logs = this.listLog.Items
                        .Cast<LogItem>()
                        .ToArray()
                        .Select(l => $"{l.Timestamp:yyyy/MM/dd HH:mm:ss}:{l.Kind.ToString()[0]}: {l.Message}")
                    ;

                    Clipboard.SetText(string.Join(Environment.NewLine, logs));
                }
            });
        }
    }
}
