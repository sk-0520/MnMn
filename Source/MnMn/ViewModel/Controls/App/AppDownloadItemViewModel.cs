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
        public AppDownloadItemViewModel(ServiceType serviceType, IDownloadState downloadState)
        {
            ServiceType = serviceType;
            DownloadState = downloadState;
        }

        #region property

        public ServiceType ServiceType { get; }
        public IDownloadState DownloadState { get; }

        CancellationTokenSource DonwloadCancel { get; set; }

        #endregion

        #region function

        public Task StartAsync()
        {
            DonwloadCancel = new CancellationTokenSource();

            return DownloadState.StartAsync(DonwloadCancel.Token);
        }

        #endregion
    }
}
