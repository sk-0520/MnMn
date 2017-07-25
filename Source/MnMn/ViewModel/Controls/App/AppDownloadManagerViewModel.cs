using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.App
{
    public class AppDownloadManagerViewModel : ManagerViewModelBase
    {
        #region variable

        int _downloadingCount;
        int _waitingCount;

        #endregion

        public AppDownloadManagerViewModel(Mediator mediator)
            : base(mediator)
        {
            DownloadItemPropertyChangedListener = new PropertyChangedWeakEventListener(DownloadItem_PropertyChanged);
        }

        #region property

        PropertyChangedWeakEventListener DownloadItemPropertyChangedListener { get; }

         object WaitingItemLock { get; } = new object();

        public CollectionModel<AppDownloadItemViewModel> DownloadStateItems { get; } = new CollectionModel<AppDownloadItemViewModel>();

        public int DownloadingCount
        {
            get { return this._downloadingCount; }
            private set { SetVariableValue(ref this._downloadingCount, value); }
        }

        public int WaitingCount
        {
            get { return this._waitingCount; }
            private set { SetVariableValue(ref this._waitingCount, value); }
        }

        #endregion

        #region function

        internal void AddDownloadItem(AppDownloadItemViewModel downloadItem, bool startDownload)
        {
            var stockedItem = DownloadStateItems.FirstOrDefault(i => i.Item.DownloadUniqueItem == downloadItem.Item.DownloadUniqueItem);
            if(stockedItem != null) {
                Mediator.Logger.Warning($"stocked: {downloadItem.Item.DownloadUniqueItem}");
                switch(stockedItem.Item.DownloadState) {
                    case DownloadState.None:
                    case DownloadState.Completed:
                    case DownloadState.Failure:
                        DownloadStateItems.Remove(stockedItem);
                        Mediator.Logger.Information($"reset download item");
                        break;

                    case DownloadState.Waiting:
                    case DownloadState.Preparation:
                    case DownloadState.Downloading:
                        Mediator.Logger.Trace($"ignore download item");
                        return;

                    default:
                        throw new NotImplementedException();
                }
            }

            DownloadStateItems.Insert(0, downloadItem);

            DownloadItemPropertyChangedListener.Add(downloadItem.Item);

            if(startDownload) {
                downloadItem.StartAsync();
            } else if(downloadItem.Item.DownloadState == DownloadState.Waiting) {
                StartIfDownloadableWaitingItem();
            }

            RefreshDownloadingCount();
        }

        void StartIfDownloadableWaitingItem()
        {
            var serviceGroups = DownloadStateItems
                .GroupBy(i => i.ServiceType)
            ;

            var nextDownloadItems = new List<AppDownloadItemViewModel>();

            foreach(var serviceGroup in serviceGroups) {
                // 同一サービスでダウンロード中(と準備中)のものがあったら無視
                if(serviceGroup.Any(i => i.Item.DownloadState == DownloadState.Downloading || i.Item.DownloadState == DownloadState.Preparation)) {
                    Mediator.Logger.Trace($"downloading: {serviceGroup.Key}");
                    continue;
                }

                // 待機中アイテムのダウンロード開始(ダウンローダーに積まれた順序)
                var waitItem = serviceGroup.LastOrDefault(i => i.Item.DownloadState == DownloadState.Waiting);
                if(waitItem != null) {
                    Mediator.Logger.Trace($"download target: {serviceGroup.Key}, {waitItem.Item.DownloadTitle}");
                    nextDownloadItems.Add(waitItem);
                    DownloadItemPropertyChangedListener.Remove(waitItem.Item);
                }
            }

            foreach(var nextDownloadItem in nextDownloadItems) {
                Mediator.Logger.Trace($"download start: {nextDownloadItem.Item.DownloadTitle}");
                nextDownloadItem.StartAsync();
            }
            foreach(var nextDownloadItem in nextDownloadItems) {
                DownloadItemPropertyChangedListener.Add(nextDownloadItem.Item);
            }
        }

        void RefreshDownloadingCount()
        {
            DownloadingCount = DownloadStateItems.Count(i => i.Item.DownloadState == DownloadState.Downloading);
            WaitingCount = DownloadStateItems.Count(i => i.Item.DownloadState == DownloadState.Waiting);
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
                if(download.Item.DownloadState == DownloadState.Downloading || download.Item.DownloadState == DownloadState.Preparation) {
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
            if(e.PropertyName == nameof(IDownloadItem.DownloadState)) {
                //var prevCount = DownloadingCount;
                RefreshDownloadingCount();

                Application.Current.Dispatcher.Invoke(() => {
                    lock(WaitingItemLock) {
                        StartIfDownloadableWaitingItem();
                    }
                });

                var downloadItem = (IDownloadItem)sender;
                if(downloadItem.DownloadState == DownloadState.Completed) {
                    downloadItem.AutoExecuteTargetCommand.TryExecute(null);
                    DownloadItemPropertyChangedListener.Remove(downloadItem);
                }
            }
        }
    }
}
