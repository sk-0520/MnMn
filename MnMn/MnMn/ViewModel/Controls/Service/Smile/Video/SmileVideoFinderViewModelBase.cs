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
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Player;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public abstract class SmileVideoFinderViewModelBase: ViewModelBase
    {
        #region variable

        bool _nowLoading;
        SmileVideoInformationViewModel _selectedVideoInformation;
        SourceLoadState _finderLoadState;

        string _inputFilter;
        bool _isBlacklist;

        bool _isAscending =  true;
        SmileVideoSortType _selectedSortType;

        #endregion

        public SmileVideoFinderViewModelBase(Mediation mediation)
        {
            Mediation = mediation;

            var settingResult = Mediation.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)settingResult.Result;

            SortTypeItems = new CollectionModel<SmileVideoSortType>(EnumUtility.GetMembers<SmileVideoSortType>());

            VideoInformationItems = CollectionViewSource.GetDefaultView(VideoInformationList);
            VideoInformationItems.Filter = FilterItems;
        }

        #region property

        protected Mediation Mediation { get; set; }
        protected CancellationTokenSource CancelLoading { get; set; }
        protected SmileVideoSettingModel Setting { get; }

        protected CollectionModel<SmileVideoInformationViewModel> VideoInformationList { get; } = new CollectionModel<SmileVideoInformationViewModel>();
        public IReadOnlyList<SmileVideoInformationViewModel> VideoInformationViewer => VideoInformationList;
        public CollectionModel<SmileVideoSortType> SortTypeItems { get; }
        public virtual ICollectionView VideoInformationItems { get; }

        public bool IsAscending
        {
            get { return this._isAscending; }
            set
            {
                if(SetVariableValue(ref this._isAscending, value)) {
                    ChangeSortItems();
                }
            }
        }

        public SmileVideoSortType SelectedSortType
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

        public SmileVideoInformationViewModel SelectedVideoInformation
        {
            get { return this._selectedVideoInformation; }
            set { SetVariableValue(ref this._selectedVideoInformation, value); }
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

        public virtual string InputFilter
        {
            get { return this._inputFilter; }
            set
            {
                if(SetVariableValue(ref this._inputFilter, value)) {
                    VideoInformationItems.Refresh();
                }
            }
        }
        public virtual bool IsBlacklist
        {
            get { return this._isBlacklist; }
            set
            {
                if(SetVariableValue(ref this._isBlacklist, value)) {
                    VideoInformationItems.Refresh();
                }
            }
        }

        public virtual bool CanLoad {
            get
            {
                var loadSkips = new[] { SourceLoadState.SourceLoading, SourceLoadState.SourceChecking };
                return !loadSkips.Any(l => l == FinderLoadState);
            }
        }

        protected virtual bool IsLoadVideoInformation { get { return Setting.Search.LoadInformation; } }

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
            get { return CreateCommand(o => ContinuousPlaybackAsync()); }
        }

        #endregion

        #region function

        protected virtual void SetItems(IEnumerable<SmileVideoInformationViewModel> items)
        {
            VideoInformationList.InitializeRange(items);
            VideoInformationItems.Refresh();
            CallOnPropertyChange(nameof(VideoInformationItems));
        }

        protected Task LoadFinderAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            return Task.Run(() => {
                FinderLoadState = SourceLoadState.InformationLoading;
                var loader = new SmileVideoInformationLoader(VideoInformationList);
                var imageTask = loader.LoadThumbnaiImageAsync(imageCacheSpan);
                var infoTask = IsLoadVideoInformation ? loader.LoadInformationAsync(thumbCacheSpan) : Task.CompletedTask;
                return Task.WhenAll(imageTask, infoTask);
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

        bool FilterItems(object obj)
        {
            var filter = InputFilter;
            if(string.IsNullOrEmpty(filter)) {
                return true;
            }

            var viewModel = (SmileVideoInformationViewModel)obj;
            var isHit = viewModel.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1;
            if(IsBlacklist) {
                return !isHit;
            } else {
                return isHit;
            }
        }

        //protected abstract PageLoader CreatePageLoader();

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

                return LoadCoreAsync(thumbCacheSpan, imageCacheSpan, null).ContinueWith(task => {
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
            var items = VideoInformationItems.Cast<SmileVideoInformationViewModel>().ToArray();
            var isChecked = items.Any(i => !i.IsChecked.GetValueOrDefault());

            foreach(var item in items) {
                item.IsChecked = isChecked;
            }
        }

        internal virtual Task ContinuousPlaybackAsync()
        {
            var items = VideoInformationItems.Cast<SmileVideoInformationViewModel>().ToArray();

            if(!items.Any(i => i.IsChecked.GetValueOrDefault())) {
                return Task.CompletedTask;
            }

            var playList = items
                .Where(i => i.IsChecked.GetValueOrDefault())
                .ToArray()
            ;
            var vm = new SmileVideoPlayerViewModel(Mediation);
            var task = vm.LoadAsync(playList, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ContinueWith(t => {
                foreach(var item in playList) {
                    item.IsChecked = false;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            Mediation.Request(new ShowViewRequestModel(RequestKind.ShowView, ServiceType.SmileVideo, vm, ShowViewState.Foreground));
            return task;
        }

        internal void ChangeSortItems()
        {
            var map = new Dictionary<SmileVideoSortType, string>() {
                { SmileVideoSortType.Number, nameof(SmileVideoInformationViewModel.Number) },
                { SmileVideoSortType.Title, nameof(SmileVideoInformationViewModel.Title) },
                { SmileVideoSortType.FirstRetrieve, nameof(SmileVideoInformationViewModel.FirstRetrieve) },
                { SmileVideoSortType.ViewCount, nameof(SmileVideoInformationViewModel.ViewCounter) },
                { SmileVideoSortType.CommentCount, nameof(SmileVideoInformationViewModel.CommentCounter) },
                { SmileVideoSortType.MyListCount, nameof(SmileVideoInformationViewModel.MylistCounter) },
            };
            var propertyName = map[SelectedSortType];
            var direction = IsAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            var sort = new SortDescription(propertyName, direction);

            VideoInformationItems.SortDescriptions.Clear();
            VideoInformationItems.SortDescriptions.Add(sort);
            VideoInformationItems.Refresh();
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
