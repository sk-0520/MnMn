using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppDownloadItemViewModel:ViewModelBase
    {
        #region variable

        double _downloadingProgress;

        #endregion

        public AppDownloadItemViewModel(ServiceType serviceType, IDownloadState downloadState)
        {
            ServiceType = serviceType;
            DownloadState = downloadState;

            DownloadState.DownloadingProgress = new Progress<double>(ChangedDownloadProgress);
        }

        #region property

        public ServiceType ServiceType { get; }
        IDownloadState DownloadState { get; }

        CancellationTokenSource DonwloadCancel { get; set; }

        public double DownloadingProgress
        {
            get { return this._downloadingProgress; }
            private set { SetVariableValue(ref this._downloadingProgress, value); }
        }

        #endregion

        #region function

        void ChangedDownloadProgress(double value)
        {
            DownloadingProgress = value;
        }

        public Task StartAsync()
        {
            DonwloadCancel = new CancellationTokenSource();

            return DownloadState.StartAsync(DonwloadCancel.Token);
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
