using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App.Update
{
    public class EazyUpdateDownloadItemViewModel : ViewModelBase, IDownloadItem
    {
        #region variable

        Uri _downloadUri;
        LoadState _downLoadState;

        long _downloadedSize;

        #endregion

        public EazyUpdateDownloadItemViewModel(Mediation mediation, EazyUpdateModel model)
        {
            Mediation = mediation;
            Model = model;
        }

        #region property

        Mediation Mediation { get; }
        EazyUpdateModel Model { get; }

        CancellationTokenSource Cancellation { get; set; }

        #endregion

        #region function

        Task UpdateAsync()
        {
            return Task.CompletedTask;
        }

        #endregion

        #region IDownloadItem

        public Uri DownloadUri
        {
            get { return this._downloadUri; }
            private set { SetVariableValue(ref this._downloadUri, value); }
        }

        public LoadState DownLoadState
        {
            get { return this._downLoadState; }
            private set { SetVariableValue(ref this._downLoadState, value); }
        }

        public bool EnabledTotalSize => true;

        public DownloadUnit DownloadUnit => DownloadUnit.Count;

        public long DownloadTotalSize => Model.Targets.Count;

        public long DownloadedSize
        {
            get { return this._downloadedSize; }
            private set { SetVariableValue(ref this._downloadedSize, value); }
        }

        public IProgress<double> DownloadingProgress { get; set; }

        public ImageSource Image => throw new NotImplementedException();

        public ICommand OpenDirectoryCommand => CreateCommand(o => { }, o => false);

        public ICommand ExecuteTargetCommand => CreateCommand(o => { }, o => false);

        public ICommand AutoExecuteTargetCommand => CreateCommand(o => UpdateAsync(), o => DownLoadState == LoadState.Loaded);

        public Task StartAsync()
        {
            Cancellation = new CancellationTokenSource();
            return Task.CompletedTask;
        }

        public void Cancel()
        {
            Cancellation.Cancel();
        }

        #endregion
    }
}
