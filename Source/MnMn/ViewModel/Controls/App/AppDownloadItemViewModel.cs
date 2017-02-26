using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
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
            State = downloadState;

            State.DownloadingProgress = new Progress<double>(ChangedDownloadProgress);
        }

        #region property

        public ServiceType ServiceType { get; }
        public IDownloadState State { get; }

        CancellationTokenSource DonwloadCancel { get; set; }

        public double DownloadingProgress
        {
            get { return this._downloadingProgress; }
            private set { SetVariableValue(ref this._downloadingProgress, value); }
        }

        #endregion

        #region command

        public ICommand CancelDownloadCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        DonwloadCancel.Cancel();
                    },
                    o => State.DownLoadState == Define.LoadState.Loading
                );
            }
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

            return State.StartAsync(DonwloadCancel.Token);
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
