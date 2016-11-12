/*
This file is part of MnMn.

MnMn is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MnMn is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MnMn.  If not, see <http://www.gnu.org/licenses/>.
*/
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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public abstract class FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>: ViewModelBase
        where TInformationViewModel : InformationViewModelBase
        where TFinderItemViewModel : FinderItemViewModelBase<TInformationViewModel>
    {
        #region variable

        bool _isAscending = true;
        bool _nowLoading;
        SourceLoadState _finderLoadState;

        TFinderItemViewModel _selectedFinderItem;

        string _inputTitleFilter;
        bool _isBlacklist;

        bool _showFilterSetting;
        bool _isEnabledFinderFiltering = true;
        CheckedProcessType _selectedCheckedProcess;

        #endregion

        public FinderViewModelBase(Mediation mediation)
        {
            Mediation = mediation;
            FinderItems = CollectionViewSource.GetDefaultView(FinderItemList);
        }

        #region property

        protected CollectionModel<TFinderItemViewModel> FinderItemList { get; } = new CollectionModel<TFinderItemViewModel>();
        public virtual IReadOnlyList<TFinderItemViewModel> FinderItemsViewer => FinderItemList;
        public virtual ICollectionView FinderItems { get; }

        protected Mediation Mediation { get; }
        protected CancellationTokenSource CancelLoading { get; set; }

        /// <summary>
        /// フィルタリング設定をそもそも使用するか。
        /// </summary>
        public virtual bool IsUsingFinderFilter { get; } = true;

        public abstract CacheSpan DefaultInformationCacheSpan { get; }
        public abstract CacheSpan DefaultImageCacheSpan { get; }
        public abstract object DefaultExtends { get; }

        public virtual bool AllowDrop { get; } = false;
        public virtual bool IsEnabledDrag { get; } = false;
        public IDragAndDrop DragAndDrop { get; protected set; }

        /// <summary>
        /// 昇順か。
        /// </summary>
        public virtual bool IsAscending
        {
            get { return this._isAscending; }
            set
            {
                if(SetVariableValue(ref this._isAscending, value)) {
                    ChangeSortItems();
                }
            }
        }

        /// <summary>
        /// 現在読み込み中か。
        /// </summary>
        public virtual bool NowLoading
        {
            get { return this._nowLoading; }
            set { SetVariableValue(ref this._nowLoading, value); }
        }

        /// <summary>
        /// 読込状態。
        /// </summary>
        public virtual SourceLoadState FinderLoadState
        {
            get { return this._finderLoadState; }
            set
            {
                if(SetVariableValue(ref this._finderLoadState, value)) {
                    var propertyNames = new[] {
                        nameof(CanLoad),
                        nameof(NowLoading),
                    };
                    CallOnPropertyChange(propertyNames);
                }
            }
        }

        /// <summary>
        /// 読込可能か。
        /// </summary>
        public virtual bool CanLoad
        {
            get
            {
                var loadSkips = new[] { SourceLoadState.SourceLoading, SourceLoadState.SourceChecking };
                return !loadSkips.Any(l => l == FinderLoadState);
            }
        }
        /// <summary>
        /// 選択中アイテム。
        /// </summary>
        public TFinderItemViewModel SelectedFinderItem
        {
            get { return this._selectedFinderItem; }
            set { SetVariableValue(ref this._selectedFinderItem, value); }
        }

        /// <summary>
        /// タイトルフィルター文字列。
        /// </summary>
        public virtual string InputTitleFilter
        {
            get { return this._inputTitleFilter; }
            set
            {
                if(SetVariableValue(ref this._inputTitleFilter, value)) {
                    FinderItems.Refresh();
                }
            }
        }

        /// <summary>
        /// タイトルフィルタを除外として扱うか。
        /// </summary>
        public virtual bool IsBlacklist
        {
            get { return this._isBlacklist; }
            set
            {
                if(SetVariableValue(ref this._isBlacklist, value)) {
                    FinderItems.Refresh();
                }
            }
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

        internal abstract void ChangeSortItems();

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
                    Mediation.Logger.Trace("CANCEL!");
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
                return LoadImageAsync(FinderItemList.Select(i => i.Information), imageCacheSpan);
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
            var items = FinderItems.Cast<TFinderItemViewModel>().ToArray();
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
                .ToArray()
            ;
            foreach(var item in prevInformations) {
                item.DecrementReference();
            }

            var finderItems = items
                .Select((v, i) => new { Information = v, Index = i })
                .Where(v => v.Information != null)
                .Select(v => CreateFinderItem(v.Information, v.Index + 1))
                .ToArray()
            ;

            return LoadInformationAsync(finderItems.Select(i => i.Information), informationCacheSpan).ContinueWith(t => {
                Application.Current.Dispatcher.Invoke(new Action(() => {
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
