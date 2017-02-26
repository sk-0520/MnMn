using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public class WebNavigatorFileDownloadState: ViewModelBase, IDownloadState
    {
        #region variable

        string _displayText;

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
        /// <param name="writeStream"></param>
        /// <param name="leaveOpen"> <see cref="WebNavigatorFileDownloadState"/> オブジェクトを破棄した後に<see cref="writeStream"/>を開いたままにする場合は true、それ以外の場合は false。</param>
        public WebNavigatorFileDownloadState(Uri uri, string displayText, ICreateHttpUserAgent userAgentCreator, Stream writeStream, bool leaveOpen = false)
        {
            DownloadUri = uri;
            Downloader = new Downloader(DownloadUri, userAgentCreator);
            this._displayText = displayText;
            WriteStream = writeStream;
            LeaveOpen = leaveOpen;

            Downloader.DownloadStart += Downloader_DownloadStart;
            Downloader.Downloading += Downloader_Downloading;
            Downloader.Downloaded += Downloader_Downloaded;

            DownLoadState = LoadState.None;
        }

        #region property

        Downloader Downloader { get; }
        Stream WriteStream { get; }

        /// <summary>
        /// <see cref="WebNavigatorFileDownloadState"/> オブジェクトを破棄した後に<see cref="WriteStream"/>を開いたままにする場合は true、それ以外の場合は false。
        /// </summary>
        bool LeaveOpen { get; }

        #endregion

        #region function

        #endregion

        #region IDownloader

        public Uri DownloadUri { get; }

        public bool EnabledCompleteSize
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

        public Task StartAsync(CancellationToken cancellationToken)
        {
            DownLoadState = LoadState.Preparation;

            return Downloader.StartAsync(cancellationToken);
        }

        #endregion

        #region ViewModelBase

        public override string DisplayText => this._displayText;

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Downloader.DownloadStart -= Downloader_DownloadStart;
                Downloader.Downloading -= Downloader_Downloading;
                Downloader.Downloaded -= Downloader_Downloaded;
                Downloader.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

        private void Downloader_DownloadStart(object sender, Define.Event.DownloadStartEventArgs e)
        {
            DownLoadState = LoadState.Loading;

            DownloadingProgress?.Report(0);

            EnabledCompleteSize = Downloader.ResponseHeaders.ContentLength.HasValue;
            if(EnabledCompleteSize) {
                DownloadTotalSize = Downloader.ResponseHeaders.ContentLength.Value;
            }
        }

        private void Downloader_Downloading(object sender, Define.Event.DownloadingEventArgs e)
        {
            DownLoadState = LoadState.Loading;

            WriteStream.Write(e.Data.Array, 0, e.Data.Count);
            DownloadedSize = Downloader.DownloadedSize;

            if(EnabledCompleteSize && DownloadingProgress != null) {
                var percent = Downloader.DownloadedSize / (double)DownloadTotalSize;
                DownloadingProgress.Report(percent);
            }
        }

        private void Downloader_Downloaded(object sender, Define.Event.DownloaderEventArgs e)
        {
            DownLoadState = LoadState.Loaded;

            DownloadedSize = Downloader.DownloadedSize;
            DownloadingProgress?.Report(1);

            if(!LeaveOpen) {
                WriteStream.Dispose();
            }
        }

    }
}
