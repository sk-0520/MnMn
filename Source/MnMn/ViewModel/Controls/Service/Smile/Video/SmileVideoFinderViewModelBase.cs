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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public abstract class SmileVideoFinderViewModelBase: ViewModelBase
    {
        #region variable

        bool _nowLoading;

        SmileVideoFinderItem _selectedFinderItem;
        SourceLoadState _finderLoadState;

        string _inputTitleFilter;
        bool _isBlacklist;
        bool _showFilterSetting;
        bool _isEnabledFinderFiltering;

        bool _isAscending = true;
        SmileVideoSortType _selectedSortType;
        bool _showContinuousPlaybackMenu;

        #endregion

        public SmileVideoFinderViewModelBase(Mediation mediation)
        {
            Mediation = mediation;

            var settingResult = Mediation.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)settingResult.Result;

            SortTypeItems = new CollectionModel<SmileVideoSortType>(EnumUtility.GetMembers<SmileVideoSortType>());

            //BindingOperations.EnableCollectionSynchronization(FinderItemList, new object());
            var filteringResult = Mediation.GetResultFromRequest<SmileVideoFilteringResultModel>(new SmileVideoCustomSettingRequestModel(SmileVideoCustomSettingKind.CommentFiltering));
            FinderFilering = filteringResult.Filtering;

            this._isEnabledFinderFiltering = FinderFilering.IsEnabledFinderFiltering;

            FinderItems = CollectionViewSource.GetDefaultView(FinderItemList);
            FinderItems.Filter = FilterItems;
        }

        #region property

        protected Mediation Mediation { get; set; }
        protected CancellationTokenSource CancelLoading { get; set; }
        protected SmileVideoSettingModel Setting { get; }

        protected CollectionModel<SmileVideoFinderItem> FinderItemList { get; } = new CollectionModel<SmileVideoFinderItem>();
        public IReadOnlyList<SmileVideoFinderItem> FinderItemsViewer => FinderItemList;
        public CollectionModel<SmileVideoSortType> SortTypeItems { get; }
        public virtual ICollectionView FinderItems { get; }

        //public virtual SmileVideoFilteringViweModel Filtering { get; }
        public SmileVideoFilteringViweModel FinderFilering { get; }

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

        public virtual SmileVideoSortType SelectedSortType
        {
            get { return this._selectedSortType; }
            set
            {
                if(SetVariableValue(ref this._selectedSortType, value)) {
                    ChangeSortItems();
                }
            }
        }

        public virtual bool NowLoading
        {
            get { return this._nowLoading; }
            set { SetVariableValue(ref this._nowLoading, value); }
        }

        public SmileVideoFinderItem SelectedFinderItem
        {
            get { return this._selectedFinderItem; }
            set { SetVariableValue(ref this._selectedFinderItem, value); }
        }

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
        public bool IsEnabledFinderFiltering
        {
            get { return this._isEnabledFinderFiltering; }
            set
            {
                if(SetVariableValue(ref this._isEnabledFinderFiltering, value)) {
                    FinderFilering.IsEnabledFinderFiltering = value;
                    FinderItems.Refresh();
                }
            }
        }

        /// <summary>
        /// 設定フィルタ有効無効UI表示。
        /// </summary>
        public bool ShowFilterSetting
        {
            get { return this._showFilterSetting; }
            set { SetVariableValue(ref this._showFilterSetting, value); }
        }

        public virtual bool CanLoad
        {
            get
            {
                var loadSkips = new[] { SourceLoadState.SourceLoading, SourceLoadState.SourceChecking };
                return !loadSkips.Any(l => l == FinderLoadState);
            }
        }

        protected virtual bool IsLoadVideoInformation { get { return Setting.Search.LoadInformation; } }

        public bool ShowContinuousPlaybackMenu
        {
            get { return this._showContinuousPlaybackMenu; }
            set { SetVariableValue(ref this._showContinuousPlaybackMenu, value); }
        }

        #endregion

        #region command

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

        public ICommand AllCheckCommand
        {
            get { return CreateCommand(o => ToggleAllCheck()); }
        }

        public ICommand ContinuousPlaybackCommand
        {
            get { return CreateCommand(o => ContinuousPlaybackAsync(false)); }
        }

        public ICommand RandomContinuousPlaybackCommand
        {
            get { return CreateCommand(o => ContinuousPlaybackAsync(true)); }
        }

        public ICommand ChangedFilteringCommand
        {
            get { return CreateCommand(o => ChangedFiltering()); }
        }

        #endregion

        #region function

        protected virtual Task SetItemsAsync(IEnumerable<SmileVideoInformationViewModel> items, CacheSpan thumbCacheSpan)
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
                .Select(v => new SmileVideoFinderItem(v.Information, v.Index + 1))
                .ToArray()
            ;

            var loader = new SmileVideoInformationLoader(finderItems.Select(i => i.Information));
            return loader.LoadInformationAsync(thumbCacheSpan).ContinueWith(t => {
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    FinderItemList.InitializeRange(finderItems);
                    ChangeSortItems();
                }));
            }, CancelLoading.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.Current);
        }

        protected Task LoadFinderAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            return Task.Run(() => {
                FinderLoadState = SourceLoadState.InformationLoading;
                var loader = new SmileVideoInformationLoader(FinderItemList.Select(i => i.Information));
                return loader.LoadThumbnaiImageAsync(imageCacheSpan);
            }, CancelLoading.Token).ContinueWith(t => {
                FinderLoadState = SourceLoadState.Completed;
                NowLoading = false;
            }, CancelLoading.Token, TaskContinuationOptions.AttachedToParent, TaskScheduler.Current);
        }

        protected void DisposeCancelLoading()
        {
            if(CancelLoading != null) {
                CancelLoading.Dispose();
                CancelLoading = null;
            }
        }

        protected virtual bool FilterItems(object obj)
        {
            var filter = InputTitleFilter;
            if(string.IsNullOrEmpty(filter)) {
                return true;
            }

            var finderItem = (SmileVideoFinderItem)obj;
            var viewModel = finderItem.Information;
            var isHit = viewModel.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1;
            if(IsBlacklist) {
                return !isHit;
            } else {
                return isHit;
            }
        }

        public IEnumerable<SmileVideoFinderItem> GetCheckedItems()
        {
            return FinderItemsViewer
                .Cast<SmileVideoFinderItem>()
                .Where(i => i.IsChecked.GetValueOrDefault())
            ;
        }

        protected abstract Task LoadCoreAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends);

        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            return LoadAsync(thumbCacheSpan, imageCacheSpan, null);
        }

        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            if(CanLoad) {
                if(NowLoading) {
                    Mediation.Logger.Trace("CANCEL!");
                    CancelLoading.Cancel(true);
                }

                FinderLoadState = SourceLoadState.SourceLoading;
                NowLoading = true;

                CancelLoading = new CancellationTokenSource();

                return LoadCoreAsync(thumbCacheSpan, imageCacheSpan, extends).ContinueWith(task => {
                    return LoadFinderAsync(thumbCacheSpan, imageCacheSpan);
                }, CancelLoading.Token);
            } else {
                return Task.CompletedTask;
            }
        }

        public Task LoadDefaultCacheAsync()
        {
            return LoadAsync(Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
        }

        internal virtual void ToggleAllCheck()
        {
            var items = FinderItems.Cast<SmileVideoFinderItem>().ToArray();
            var isChecked = items.Any(i => !i.IsChecked.GetValueOrDefault());

            foreach(var item in items) {
                item.IsChecked = isChecked;
            }
        }

        internal virtual Task ContinuousPlaybackAsync(bool isRandom)
        {
            ShowContinuousPlaybackMenu = false;

            var items = GetCheckedItems().ToArray();

            if(!items.Any()) {
                return Task.CompletedTask;
            }

            var playList = items
                .Select(i => i.Information)
                .ToArray()
            ;

            foreach(var item in items) {
                item.IsChecked = false;
            }

            var vm = new SmileVideoPlayerViewModel(Mediation);
            vm.IsRandomPlay = isRandom;
            var task = vm.LoadAsync(playList, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
            Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, vm, ShowViewState.Foreground));
            return task;
        }

        internal virtual void ChangeSortItems()
        {
            var map = new[] {
                new { Type = SmileVideoSortType.Number, Parent = string.Empty, Property = nameof(SmileVideoFinderItem.Number) },
                new { Type = SmileVideoSortType.Title, Parent = nameof(SmileVideoFinderItem.Information), Property = nameof(SmileVideoInformationViewModel.Title) },
                new { Type = SmileVideoSortType.Length, Parent = nameof(SmileVideoFinderItem.Information), Property = nameof(SmileVideoInformationViewModel.Length) },
                new { Type = SmileVideoSortType.FirstRetrieve, Parent = nameof(SmileVideoFinderItem.Information), Property = nameof(SmileVideoInformationViewModel.FirstRetrieve) },
                new { Type = SmileVideoSortType.ViewCount, Parent = nameof(SmileVideoFinderItem.Information),Property = nameof(SmileVideoInformationViewModel.ViewCounter) },
                new { Type = SmileVideoSortType.CommentCount, Parent = nameof(SmileVideoFinderItem.Information),Property = nameof(SmileVideoInformationViewModel.CommentCounter) },
                new { Type = SmileVideoSortType.MyListCount, Parent = nameof(SmileVideoFinderItem.Information),Property = nameof(SmileVideoInformationViewModel.MylistCounter) },
            }.ToDictionary(
                ak => ak.Type,
                av => $"{av.Parent}.{av.Property}"
            );
            var propertyName = map[SelectedSortType];
            var direction = IsAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            var sort = new SortDescription(propertyName, direction);

            FinderItems.SortDescriptions.Clear();
            FinderItems.SortDescriptions.Add(sort);
            FinderItems.Refresh();
        }

        void ChangedFiltering()
        {
            Mediation.Logger.Information("not impl");
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
