using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public abstract class TFinderViewModelBase<TInformationViewModel, TFinderItemViewModel> : FinderViewModelBase
        where TInformationViewModel : InformationViewModelBase
        where TFinderItemViewModel : FinderItemViewModelBase<TInformationViewModel>
    {
        #region variable

        TFinderItemViewModel _selectedFinderItem;

        bool _showFilterSetting;
        bool _isEnabledFinderFiltering = true;
        CheckedProcessType _selectedCheckedProcess;

        #endregion

        public TFinderViewModelBase(Mediator mediator, int baseNumber)
            :base(mediator, baseNumber)
        {
            FinderItems = CollectionViewSource.GetDefaultView(FinderItemList);
        }

        #region property

        protected CollectionModel<TFinderItemViewModel> FinderItemList { get; } = new CollectionModel<TFinderItemViewModel>();
        public virtual IReadOnlyList<TFinderItemViewModel> FinderItemsViewer => FinderItemList;

        protected CancellationTokenSource CancelLoading { get; set; }

        /// <summary>
        /// フィルタリング設定をそもそも使用するか。
        /// </summary>
        public virtual bool IsUsingFinderFilter { get; } = true;

        /// <summary>
        /// 表に出てこない処理か。
        /// <para>けっこうな設計ミス。</para>
        /// </summary>
        internal virtual bool IsBackgroundOnly { get; set; } = false;

        public abstract CacheSpan DefaultInformationCacheSpan { get; }
        public abstract CacheSpan DefaultImageCacheSpan { get; }
        public abstract object DefaultExtends { get; }

        /// <summary>
        /// 選択中アイテム。
        /// </summary>
        public TFinderItemViewModel SelectedFinderItem
        {
            get { return this._selectedFinderItem; }
            set { SetVariableValue(ref this._selectedFinderItem, value); }
        }

        /// <summary>
        /// 設定フィルタを使用するか。
        /// </summary>
        public virtual bool IsEnabledFinderFiltering
        {
            get { return this._isEnabledFinderFiltering; }
            set
            {
                if(SetVariableValue(ref this._isEnabledFinderFiltering, value)) {
                    FinderItems.Refresh();
                }
            }
        }

        /// <summary>
        /// 設定フィルタ有効無効UI表示。
        /// </summary>
        public virtual bool ShowFilterSetting
        {
            get { return this._showFilterSetting; }
            set { SetVariableValue(ref this._showFilterSetting, value); }
        }

        public virtual CollectionModel<CheckedProcessType> CheckedProcessItems { get; protected set; } = new CollectionModel<CheckedProcessType>(new[] { CheckedProcessType.SequencePlay, CheckedProcessType.RandomPlay });

        public virtual CheckedProcessType SelectedCheckedProcess
        {
            get { return this._selectedCheckedProcess; }
            set { SetVariableValue(ref this._selectedCheckedProcess, value); }
        }

        #endregion

        #region command

        public ICommand AllCheckCommand
        {
            get { return CreateCommand(o => ToggleAllCheck()); }
        }

        public virtual ICommand ReloadCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LoadDefaultCacheAsync().ConfigureAwait(false);
                    }
                );
            }
        }

        public ICommand SwitchShowFilterCommand
        {
            get { return CreateCommand(o => SwitchShowFilter()); }
        }

        public ICommand CheckedProcessCommand
        {
            get { return CreateCommand(o => CheckedProcessAsync((CheckedProcessType)o)); }
        }

        #endregion

        #region function

        public Task LoadDefaultCacheAsync()
        {
            return LoadAsync(DefaultInformationCacheSpan, DefaultImageCacheSpan);
        }

        public Task LoadAsync(CacheSpan informationCacheSpan, CacheSpan imageCacheSpan)
        {
            return LoadAsync(informationCacheSpan, imageCacheSpan, DefaultExtends);
        }

        public Task LoadAsync(CacheSpan informationCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            if(CanLoad) {
                if(NowLoading) {
                    Mediator.Logger.Trace("CANCEL!");
                    CancelLoading.Cancel(true);
                }

                FinderLoadState = SourceLoadState.SourceLoading;
                NowLoading = true;

                CancelLoading = new CancellationTokenSource();

                return LoadCoreAsync(informationCacheSpan, imageCacheSpan, extends).ContinueWith(task => {
                    return LoadFinderAsync(informationCacheSpan, imageCacheSpan);
                }, CancelLoading.Token);
            } else {
                return Task.CompletedTask;
            }
        }

        protected Task LoadFinderAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            return Task.Run(() => {
                FinderLoadState = SourceLoadState.InformationLoading;
                if(IsBackgroundOnly) {
                    return Task.CompletedTask;
                } else {
                    return LoadImageAsync(FinderItemList.Select(i => i.Information), imageCacheSpan);
                }
            }, CancelLoading.Token).ContinueWith(t => {
                FinderLoadState = SourceLoadState.Completed;
                NowLoading = false;
            }, CancelLoading.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.Current);
        }

        protected abstract Task LoadCoreAsync(CacheSpan informationCacheSpan, CacheSpan imageCacheSpan, object extends);
        protected abstract Task LoadInformationAsync(IEnumerable<TInformationViewModel> informationItems, CacheSpan informationCacheSpan);
        protected abstract Task LoadImageAsync(IEnumerable<TInformationViewModel> informationItems, CacheSpan imageCacheSpan);

        internal virtual void ToggleAllCheck()
        {
            var items = FinderItems.Cast<TFinderItemViewModel>().ToEvaluatedSequence();
            var isChecked = items.Any(i => !i.IsChecked.GetValueOrDefault());

            foreach(var item in items) {
                item.IsChecked = isChecked;
            }
        }

        protected virtual void SwitchShowFilter()
        {
            ShowFilterSetting = !ShowFilterSetting;
        }

        protected abstract Task CheckedProcessAsync(CheckedProcessType checkedProcessType);

        public IEnumerable<TFinderItemViewModel> GetCheckedItems()
        {
            return FinderItems
                .Cast<TFinderItemViewModel>()
                .Where(i => i.IsChecked.GetValueOrDefault())
            ;
        }

        protected abstract TFinderItemViewModel CreateFinderItem(TInformationViewModel information, int number);

        protected virtual Task SetItemsAsync(IEnumerable<TInformationViewModel> items, CacheSpan informationCacheSpan)
        {
            var prevInformations = FinderItemList
                .Select(i => i.Information)
                .ToEvaluatedSequence()
            ;
            foreach(var item in prevInformations) {
                item.DecrementReference();
            }

            var finderItems = items
                .Select((v, i) => new { Information = v, Index = i })
                .Where(v => v.Information != null)
                .Select(v => CreateFinderItem(v.Information, BaseNumber + v.Index + 1))
                .ToEvaluatedSequence()
            ;

            return LoadInformationAsync(finderItems.Select(i => i.Information), informationCacheSpan).ContinueWith(t => {
                Dispatcher dispatcher;
                if(IsBackgroundOnly) {
                    var view = (CollectionView)FinderItems;
                    dispatcher = view.Dispatcher;
                } else {
                    dispatcher = Application.Current.Dispatcher;
                }
                dispatcher.Invoke(new Action(() => {
                    FinderItemList.InitializeRange(finderItems);
                    ChangeSortItems();
                }));
            }, CancelLoading.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.Current);
        }

        protected void DisposeCancelLoading()
        {
            if(CancelLoading != null) {
                CancelLoading.Dispose();
                CancelLoading = null;
            }
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                DisposeCancelLoading();
            }
            base.Dispose(disposing);
        }

        #endregion
    }

}
