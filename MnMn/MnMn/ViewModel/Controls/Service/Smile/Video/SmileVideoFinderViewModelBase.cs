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
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    public abstract class SmileVideoFinderViewModelBase: ViewModelBase
    {
        #region property

        ICollectionView _VideoInformationItems;
        bool _nowLoading;
        SmileVideoInformationViewModel _selectedVideoInformation;
        SmileVideoFinderLoadState _finderLoadState;

        #endregion
        public SmileVideoFinderViewModelBase(Mediation mediation)
        {
            Mediation = mediation;

            VideoInformationList = new CollectionModel<SmileVideoInformationViewModel>();
            VideoInformationItems = CollectionViewSource.GetDefaultView(VideoInformationList);
        }

        #region property

        protected Mediation Mediation { get; set; }
        protected CancellationTokenSource CancelLoading { get; set; }

        protected CollectionModel<SmileVideoInformationViewModel> VideoInformationList { get; }

        public ICollectionView VideoInformationItems { get; }
        //{
        //    get { return this._VideoInformationItems; }
        //    private set { SetVariableValue(ref this._VideoInformationItems, value); }
        //}

        public bool NowLoading
        {
            get { return this._nowLoading; }
            set { SetVariableValue(ref this._nowLoading, value); }
        }

        public SmileVideoInformationViewModel SelectedVideoInformation
        {
            get { return this._selectedVideoInformation; }
            set { SetVariableValue(ref this._selectedVideoInformation, value); }
        }

        public SmileVideoFinderLoadState FinderLoadState
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

        public virtual bool CanLoad { get; }

        #endregion

        #region command

        public ICommand OpenVideoCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var item = SelectedVideoInformation;
                        if(item != null) {
                            OpenPlayer(item);
                        }
                    }
                );
            }
        }

        #endregion

        #region function

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
