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

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public abstract class SmileVideoFinderViewModelBase: ViewModelBase
    {
        #region variable

        bool _nowLoading;
        SmileVideoInformationViewModel _selectedVideoInformation;
        SmileVideoFinderLoadState _finderLoadState;

        string _inputFilter="#";
        bool _isBlacklist;

        #endregion

        public SmileVideoFinderViewModelBase(Mediation mediation)
        {
            Mediation = mediation;

            var settingResult = Mediation.Request(new RequestModel(RequestKind.Setting, ServiceType.SmileVideo));
            Setting = (SmileVideoSettingModel)settingResult.Result;

            VideoInformationList = new CollectionModel<SmileVideoInformationViewModel>();
            VideoInformationItems = CollectionViewSource.GetDefaultView(VideoInformationList);
            VideoInformationItems.Filter = FilterItems;
        }

        #region property

        protected Mediation Mediation { get; set; }
        protected CancellationTokenSource CancelLoading { get; set; }
        protected SmileVideoSettingModel Setting { get; }

        protected CollectionModel<SmileVideoInformationViewModel> VideoInformationList { get; }
        
        public virtual ICollectionView VideoInformationItems { get; }
        //{
        //    get { return this._VideoInformationItems; }
        //    private set { SetVariableValue(ref this._VideoInformationItems, value); }
        //}

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

        public virtual SmileVideoFinderLoadState FinderLoadState
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

        public string InputFilter
        {
            get { return this._inputFilter; }
            set
            {
                if(SetVariableValue(ref this._inputFilter, value)) {
                    VideoInformationItems.Refresh();
                }
            }
        }
        public bool IsBlacklist
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
                var loadSkips = new[] { SmileVideoFinderLoadState.VideoSourceLoading, SmileVideoFinderLoadState.VideoSourceChecking };
                return !loadSkips.Any(l => l == FinderLoadState);
            }
        }

        protected virtual bool IsLoadVideoInformation { get { return Setting.LoadVideoInformation; } }

        #endregion

        #region command

        public virtual ICommand ReloadCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        LoadAsync(Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan).ConfigureAwait(false);
                    }
                );
            }
        }

        #endregion

        #region function

        protected virtual void SetItems(IEnumerable<SmileVideoInformationViewModel> items)
        {
            VideoInformationList.InitializeRange(items);
            VideoInformationItems.Refresh();
        }

        protected Task LoadFinderAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            var cancel = CancelLoading = new CancellationTokenSource();
            return Task.Run(() => {
                FinderLoadState = SmileVideoFinderLoadState.InformationLoading;
                var loader = new SmileVideoInformationLoader(VideoInformationList);
                var imageTask = loader.LoadThumbnaiImageAsync(imageCacheSpan);
                var infoTask = IsLoadVideoInformation ? loader.LoadInformationAsync(thumbCacheSpan) : Task.CompletedTask;
                return Task.WhenAll(infoTask, infoTask);
            }).ContinueWith(t => {
                FinderLoadState = SmileVideoFinderLoadState.Completed;
                NowLoading = false;
            }, cancel.Token, TaskContinuationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public async void OpenPlayer(SmileVideoInformationViewModel videoInformation)
        {
            var vm = new SmileVideoPlayerViewModel(Mediation);
            var window = new SmileVideoPlayerWindow() {
                DataContext = vm,
            };
            vm.SetView(window);
            window.Show();

            await vm.LoadAsync(videoInformation, Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
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
            var isBlack = IsBlacklist;

            var viewModel = (SmileVideoInformationViewModel)obj;
            var isHit = viewModel.Title.IndexOf(filter ?? string.Empty) >= -1;
            if(IsBlacklist) {
                return !isHit;
            } else {
                return isHit;
            }
        }

        protected abstract PageLoader CreatePageLoader();

        protected abstract Task LoadAsync_Impl(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends);

        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan)
        {
            return LoadAsync(thumbCacheSpan, imageCacheSpan, null);
        }

        public Task LoadAsync(CacheSpan thumbCacheSpan, CacheSpan imageCacheSpan, object extends)
        {
            if(CanLoad) {
                if(NowLoading) {
                    Debug.WriteLine("CANCEL!");
                    CancelLoading.Cancel(true);
                    Debug.WriteLine(";-)");
                }

                return LoadAsync_Impl(thumbCacheSpan, imageCacheSpan, null);
            } else {
                return Task.CompletedTask;
            }
        }

        public Task LoadDefaultCacheAsync()
        {
            return LoadAsync(Constants.ServiceSmileVideoThumbCacheSpan, Constants.ServiceSmileVideoImageCacheSpan);
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
