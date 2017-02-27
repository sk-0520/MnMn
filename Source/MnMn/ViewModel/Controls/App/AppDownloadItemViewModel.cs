using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
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

        public AppDownloadItemViewModel(ServiceType serviceType, IDownloadItem downloadState)
        {
            ServiceType = serviceType;
            Item = downloadState;

            Item.DownloadingProgress = new Progress<double>(ChangedDownloadProgress);
        }

        #region property

        public ServiceType ServiceType { get; }
        public IDownloadItem Item { get; }

        public double DownloadingProgress
        {
            get { return this._downloadingProgress; }
            private set { SetVariableValue(ref this._downloadingProgress, value); }
        }

        #endregion

        #region command

        public ICommand ExecuteTargetCommand
        {
            get
            {
                return CreateCommand(
                    o => Item.ExecuteTargetCommand.TryExecute(o),
                    o => Item.DownLoadState == Define.LoadState.Loaded
                );
            }
        }

        public ICommand StartDownloadCommand
        {
            get
            {
                return CreateCommand(
                    o => StartAsync(),
                    o => Item.DownLoadState == Define.LoadState.Failure
                );
            }
        }

        public ICommand CancelDownloadCommand
        {
            get
            {
                return CreateCommand(
                    o => CancelDownload(),
                    o => Item.DownLoadState == Define.LoadState.Loading
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
            return Item.StartAsync();
        }

        public void CancelDownload()
        {
            Item.Cancel();
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
