using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppDownloadManagerViewModel: ManagerViewModelBase
    {
        #region variable

        int _downloadingCount;

        #endregion

        public AppDownloadManagerViewModel(Mediation mediation)
            : base(mediation)
        {
            DownloadItemPropertyChangedListener = new PropertyChangedWeakEventListener(DownloadItem_PropertyChanged);
        }

        #region property

        PropertyChangedWeakEventListener DownloadItemPropertyChangedListener { get; }

        public CollectionModel<AppDownloadItemViewModel> DownloadStateItems { get; } = new CollectionModel<AppDownloadItemViewModel>();

        public int DownloadingCount
        {
            get { return this._downloadingCount; }
            private set { SetVariableValue(ref this._downloadingCount, value); }
        }

        #endregion

        #region function

        internal void AddDownloadItem(AppDownloadItemViewModel downloadItem, bool startDownload)
        {
            DownloadStateItems.Insert(0, downloadItem);

            DownloadItemPropertyChangedListener.Add(downloadItem.Item);

            if(startDownload) {
                downloadItem.StartAsync();
            }
        }

        void RefreshDownloadingCount()
        {
            DownloadingCount = DownloadStateItems.Count(i => i.Item.DownLoadState == LoadState.Loading);
        }

        #endregion

        #region ManagerViewModelBase

        public override Task<long> GarbageCollectionAsync(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan, bool force)
        {
            return GarbageCollectionDummyResult;
        }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public override Task UninitializeAsync()
        {
            foreach(var download in DownloadStateItems) {
                // 列挙中に終わるかもしんないから逐次実行。
                if(download.Item.DownLoadState == LoadState.Loading || download.Item.DownLoadState == LoadState.Preparation) {
                    download.CancelDownload();
                }
            }

            return Task.CompletedTask;
        }

        public override void InitializeView(MainWindow view)
        { }

        public override void UninitializeView(MainWindow view)
        { }

        protected override IEnumerable<ManagerViewModelBase> GetManagerChildren()
        {
            return Enumerable.Empty<ManagerViewModelBase>();
        }

        protected override void HideViewCore()
        { }

        protected override void ShowViewCore()
        { }

        #endregion

        private void DownloadItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IDownloadItem.DownLoadState)) {
                RefreshDownloadingCount();
            }
        }
    }
}
