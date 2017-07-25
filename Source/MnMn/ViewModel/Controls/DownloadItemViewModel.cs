using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    /// <summary>
    /// ダウンローダーを用いたダウンロード処理担当。
    /// </summary>
    public class DownloadItemViewModel: ViewModelBase, IDownloadItem
    {
        #region variable

        bool _enabledCompleteSize;
        DownloadUnit _downloadUnit;

        long _downloadTotalSize;
        long _downloadedSize;

        DownloadState _downloadState;
        ImageSource _image;

        #endregion

        public DownloadItemViewModel(Mediator mediator, Uri uri, FileInfo downloadFile, IHttpUserAgentCreator userAgentCreator)
        {
            Mediation = mediator;

            DownloadUri = uri;
            Downloader = new Downloader(DownloadUri, userAgentCreator);
            DownloadFile = downloadFile;

            Downloader.DownloadStart += Downloader_DownloadStart;
            Downloader.Downloading += Downloader_Downloading;
            Downloader.Downloaded += Downloader_Downloaded;
            Downloader.DownloadingError += Downloader_DownloadingError;

            DownloadState = DownloadState.None;
        }

        #region property

        protected Mediator Mediation { get; }

        Downloader Downloader { get; }
        Stream WriteStream { get; set; }

        protected FileInfo DownloadFile { get; }

        protected CancellationTokenSource Cancellation { get; set; }

        #endregion

        #region function

        public Task LoadImageAsync()
        {
            return Task.Run(() => {
                Application.Current.Dispatcher.Invoke(() => {
                    var icon = IconUtility.Load(DownloadFile.FullName, ContentTypeTextNet.Library.SharedLibrary.Define.IconScale.Big, 0, Mediation.Logger);
                    FreezableUtility.SafeFreeze(icon);
                    Image = icon;
                });
            });
        }

        #endregion

        #region IDownloader

        public Uri DownloadUri { get; }

        public string DownloadTitle => DownloadFile.Name;

        public bool EnabledTotalSize
        {
            get { return this._enabledCompleteSize; }
            private set { SetVariableValue(ref this._enabledCompleteSize, value); }
        }

        public DownloadUnit DownloadUnit
        {
            get { return this._downloadUnit; }
            private set { SetVariableValue(ref this._downloadUnit, value); }
        }


        public long DownloadTotalSize
        {
            get { return this._downloadTotalSize; }
            private set { SetVariableValue(ref this._downloadTotalSize, value); }
        }

        public long DownloadedSize
        {
            get { return this._downloadedSize; }
            private set { SetVariableValue(ref this._downloadedSize, value); }
        }

        public IProgress<double> DownloadingProgress { get; set; }

        public DownloadState DownloadState
        {
            get { return this._downloadState; }
            private set { SetVariableValue(ref this._downloadState, value); }
        }

        public virtual ImageSource Image
        {
            get { return this._image; }
            private set { SetVariableValue(ref this._image, value); }
        }

        public virtual bool CanRestart => true;

        public virtual object DownloadUniqueItem => this;

        public virtual ICommand OpenDirectoryCommand
        {
            get
            {
                return CreateCommand(
                    o => ShellUtility.OpenFileInDirectory(DownloadFile, Mediation.Logger)
                );
            }
        }

        public virtual ICommand ExecuteTargetCommand
        {
            get
            {
                return CreateCommand(
                    o => ShellUtility.ExecuteFile(DownloadFile, Mediation.Logger)
                );
            }
        }

        public virtual ICommand AutoExecuteTargetCommand
        {
            get
            {
                return CreateCommand(o => { }, o => false);
            }
        }

        public Task StartAsync()
        {
            Cancellation = new CancellationTokenSource();

            DownloadState = DownloadState.Preparation;

            EnabledTotalSize = false;
            DownloadedSize = DownloadTotalSize = 0;
            WriteStream = DownloadFile.Create();

            return Downloader.StartAsync(Cancellation.Token);
        }

        public void Cancel()
        {
            Cancellation.Cancel();
        }


        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Downloader.DownloadStart -= Downloader_DownloadStart;
                Downloader.Downloading -= Downloader_Downloading;
                Downloader.Downloaded -= Downloader_Downloaded;
                Downloader.Dispose();

                WriteStream?.Dispose();
                WriteStream = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        private void Downloader_DownloadStart(object sender, Define.Event.DownloadStartEventArgs e)
        {
            DownloadState = DownloadState.Downloading;

            DownloadingProgress?.Report(0);

            EnabledTotalSize = Downloader.ResponseHeaders.ContentLength.HasValue;
            if(EnabledTotalSize) {
                DownloadTotalSize = Downloader.ResponseHeaders.ContentLength.Value;
            }
        }

        private void Downloader_Downloading(object sender, Define.Event.DownloadingEventArgs e)
        {
            var downloader = (Downloader)sender;
            if(e.Cancel) {
                if(!downloader.Completed) {
                    DownloadState = DownloadState.Failure;
                    WriteStream?.Dispose();
                    WriteStream = null;
                    return;
                }
            }

            DownloadState = DownloadState.Downloading;

            WriteStream.Write(e.Data.Array, 0, e.Data.Count);
            DownloadedSize = Downloader.DownloadedSize;

            if(EnabledTotalSize && DownloadingProgress != null) {
                var percent = Downloader.DownloadedSize / (double)DownloadTotalSize;
                DownloadingProgress.Report(percent);
            }
        }

        private void Downloader_Downloaded(object sender, Define.Event.DownloaderEventArgs e)
        {
            var downloader = (Downloader)sender;
            if(downloader.Completed) {
                DownloadState = DownloadState.Completed;
                DownloadingProgress?.Report(1);
            } else {
                DownloadState = DownloadState.Failure;
            }

            DownloadedSize = Downloader.DownloadedSize;

            WriteStream?.Dispose();
            WriteStream = null;
        }

        private void Downloader_DownloadingError(object sender, Define.Event.DownloadingErrorEventArgs e)
        {
            DownloadState = DownloadState.Failure;

            WriteStream?.Dispose();
            WriteStream = null;
        }

    }
}
