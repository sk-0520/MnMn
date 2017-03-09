using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppDownloadManagerViewModel: ManagerViewModelBase
    {
        public AppDownloadManagerViewModel(Mediation mediation) 
            : base(mediation)
        { }

        #region property

        public CollectionModel<AppDownloadItemViewModel> DownloadStateItems { get; } = new CollectionModel<AppDownloadItemViewModel>();

        #endregion

        #region function

        internal void AddDownloadItem(AppDownloadItemViewModel downloadItem, bool startDownload)
        {
            DownloadStateItems.Insert(0, downloadItem);

            if(startDownload) {
                downloadItem.StartAsync();
            }
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
    }
}
