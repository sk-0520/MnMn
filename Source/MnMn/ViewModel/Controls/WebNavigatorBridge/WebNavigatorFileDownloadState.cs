using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public class WebNavigatorFileDownloadState: ViewModelBase, IDownloadState
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="userAgentCreator"></param>
        /// <param name="writeStream"></param>
        /// <param name="leaveOpen"> <see cref="WebNavigatorFileDownloadState"/> オブジェクトを破棄した後に<see cref="writeStream"/>を開いたままにする場合は true、それ以外の場合は false。</param>
        public WebNavigatorFileDownloadState(Uri uri, ICreateHttpUserAgent userAgentCreator, Stream writeStream, bool leaveOpen = false)
        {
            DownloadUri = uri;
            Downloader = new Downloader(DownloadUri, userAgentCreator);
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

        public bool EnabledCompleteSize { get; private set; }

        public long CompleteSize { get; private set; }

        public long DownloadedSize => Downloader.DownloadedSize;

        public IProgress<double> DownloadingProgress { get; set; }

        public LoadState DownLoadState { get; private set; }

        public Task StartAsync()
        {
            DownLoadState = LoadState.Preparation;

            return Downloader.StartAsync();
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
                CompleteSize = Downloader.ResponseHeaders.ContentLength.Value;
            }
        }

        private void Downloader_Downloading(object sender, Define.Event.DownloadingEventArgs e)
        {
            DownLoadState = LoadState.Loading;

            WriteStream.Write(e.Data.Array, 0, e.Data.Count);

            if(EnabledCompleteSize && DownloadingProgress != null) {
                var percent = Downloader.DownloadedSize / (double)CompleteSize;
                DownloadingProgress.Report(percent);
            }
        }

        private void Downloader_Downloaded(object sender, Define.Event.DownloaderEventArgs e)
        {
            DownLoadState = LoadState.Loaded;

            DownloadingProgress?.Report(1);

            if(!LeaveOpen) {
                WriteStream.Dispose();
            }
        }

    }
}
