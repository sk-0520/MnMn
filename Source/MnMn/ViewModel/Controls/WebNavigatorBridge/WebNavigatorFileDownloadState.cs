using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public class WebNavigatorFileDownloadState: ViewModelBase, IDownloadItem
    {
        #region variable

        bool _enabledCompleteSize;
        long _downloadTotalSize;
        long _downloadedSize;
        LoadState _downLoadState;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="userAgentCreator"></param>
        public WebNavigatorFileDownloadState(Uri uri, FileInfo downloadFile, ICreateHttpUserAgent userAgentCreator)
        {
            DownloadUri = uri;
            Downloader = new Downloader(DownloadUri, userAgentCreator);
            DownloadFile = downloadFile;

            Downloader.DownloadStart += Downloader_DownloadStart;
            Downloader.Downloading += Downloader_Downloading;
            Downloader.Downloaded += Downloader_Downloaded;
            Downloader.DownloadingError += Downloader_DownloadingError;

            DownLoadState = LoadState.None;
        }

        #region property

        Downloader Downloader { get; }
        Stream WriteStream { get; set; }

        FileInfo DownloadFile { get; }

        #endregion

        #region function

        #endregion

        #region IDownloader

        public Uri DownloadUri { get; }

        public bool EnabledTotalSize
        {
            get { return this._enabledCompleteSize; }
            private set { SetVariableValue(ref this._enabledCompleteSize, value); }
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

        public LoadState DownLoadState
        {
            get { return this._downLoadState; }
            private set { SetVariableValue(ref this._downLoadState, value); }
        }

        public ICommand OpenDirectoryCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                    }
                );
            }
        }

        public ICommand ExecuteTargetCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                    }
                );
            }
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            DownLoadState = LoadState.Preparation;

            EnabledTotalSize = false;
            DownloadedSize = DownloadTotalSize = 0;
            WriteStream = DownloadFile.Create();

            return Downloader.StartAsync(cancellationToken);
        }

        #endregion

        #region ViewModelBase

        public override string DisplayText => DownloadFile.Name;

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
            DownLoadState = LoadState.Loading;

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
                    DownLoadState = LoadState.Failure;
                    WriteStream?.Dispose();
                    WriteStream = null;
                    return;
                }
            }

            DownLoadState = LoadState.Loading;

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
                DownLoadState = LoadState.Loaded;
                DownloadingProgress?.Report(1);
            } else {
                DownLoadState = LoadState.Failure;
            }

            DownloadedSize = Downloader.DownloadedSize;

            WriteStream?.Dispose();
            WriteStream = null;
        }

        private void Downloader_DownloadingError(object sender, Define.Event.DownloadingErrorEventArgs e)
        {
            DownLoadState = LoadState.Failure;

            WriteStream?.Dispose();
            WriteStream = null;
        }


    }
}
