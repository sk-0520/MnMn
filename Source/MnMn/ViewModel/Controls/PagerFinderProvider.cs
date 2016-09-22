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
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Event;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public class PagerFinderProvider<TFinderViewModel, TInformationViewModel, TFinderItemViewModel>: ViewModelBase, IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel>
        where TFinderViewModel : FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>
        where TInformationViewModel : InformationViewModelBase
        where TFinderItemViewModel : FinderItemViewModelBase<TInformationViewModel>
    {
        #region define

        static IEnumerable<string> ChangePagePropertyNames => new[] {
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.FinderItemsViewer),
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.FinderItems),
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.FinderLoadState),
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.CanLoad),
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.NowLoading),
            nameof(IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel>.PageItems),
            nameof(IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel>.PageChangeCommand),
            nameof(FinderViewModelBase<TInformationViewModel, TFinderItemViewModel>.IsAscending),
            //nameof(TFinderViewModel.SelectedSortType),
            //nameof(TFinderViewModel.IsEnabledFinderFiltering),
            //nameof(TFinderViewModel.ShowFilterSetting),
        };

        #endregion

        #region event

        public event EventHandler<ChangedSelectedPageEventArgs<TFinderViewModel, TInformationViewModel, TFinderItemViewModel>> ChangedSelectedPage;

        #endregion

        #region variable

        TFinderViewModel _currentFinder;
        PageViewModel<TFinderViewModel> _selectedPage;

        #endregion

        public PagerFinderProvider(Mediation mediation, IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel> parentFinder, IEnumerable<string> changePropertyNames)
        {
            Mediation = mediation;
            ParentFinder = parentFinder;
            if(changePropertyNames.Any()) {
                CustomChangePagePropertyNames.AddRange(changePropertyNames);
            }
        }

        public PagerFinderProvider(Mediation mediation, IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel> parentFinder)
            : this(mediation, parentFinder, Enumerable.Empty<string>())
        { }

        #region property

        Mediation Mediation { get; }
        CollectionModel<string> CustomChangePagePropertyNames { get; } = new CollectionModel<string>(ChangePagePropertyNames);

        IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel> ParentFinder { get; }
        public TFinderViewModel CurrentFinder
        {
            get { return this._currentFinder; }
            set
            {
                if(SetVariableValue(ref this._currentFinder, value)) {
                    if(CurrentFinder != null) {
                        DetachmentChildProprtyChange(CurrentFinder);
                        AttachmentChildProprtyChange(CurrentFinder);
                    }
                }
            }
        }

        #endregion

        #region property

        public TFinderViewModel SelectedFinder => SelectedPage.ViewModel;

        public ICollectionView FinderItems
        {
            get
            {
                if(SelectedPage == null) {
                    if(CurrentFinder != null) {
                        return CurrentFinder.FinderItems;
                    } else {
                        return ParentFinder.FinderItems;
                    }
                }
                return SelectedPage.ViewModel.FinderItems;
            }
        }

        #endregion

        #region function

        protected void OnChangedSelectedPage(PageViewModel<TFinderViewModel> oldSelectedPage, PageViewModel<TFinderViewModel> newSelectedPage)
        {
            if(ChangedSelectedPage == null) {
                return;
            }

            //var e = new ChangedSelectedPageEventArgs(oldSelectedPage, newSelectedPage);
            var e = new ChangedSelectedPageEventArgs<TFinderViewModel, TInformationViewModel, TFinderItemViewModel>(oldSelectedPage, newSelectedPage);
            ChangedSelectedPage(this, e);
        }

        public void AttachmentChildProprtyChange(INotifyPropertyChanged target)
        {
            target.PropertyChanged += PageVm_PropertyChanged;
        }

        public void DetachmentChildProprtyChange(INotifyPropertyChanged target)
        {
            target.PropertyChanged -= PageVm_PropertyChanged;
        }

        #endregion

        #region IPagerFinder

        /// <summary>
        /// 保持するページャ。
        /// </summary>
        public CollectionModel<PageViewModel<TFinderViewModel>> PageItems { get; } = new CollectionModel<PageViewModel<TFinderViewModel>>();

        /// <summary>
        /// 選択中ページ。
        /// </summary>
        public PageViewModel<TFinderViewModel> SelectedPage
        {
            get { return this._selectedPage; }
            set
            {
                var oldSelectedPage = SelectedPage;
                if(SetVariableValue(ref this._selectedPage, value)) {
                    if(SelectedPage != null) {
                        SelectedPage.IsChecked = true;
                    }
                    if(oldSelectedPage != null) {
                        oldSelectedPage.ViewModel.PropertyChanged -= PageVm_PropertyChanged;
                        //oldSelectedPage.ViewModel.PropertyChanged -= SearchFinder_PropertyChanged_TotalCount;
                        oldSelectedPage.IsChecked = false;
                    }

                    if(SelectedPage != null && oldSelectedPage != null) {
                        SelectedPage.ViewModel.InputTitleFilter = oldSelectedPage.ViewModel.InputTitleFilter;
                        //SelectedPage.ViewModel.SelectedSortType = oldSelectedPage.ViewModel.SelectedSortType;
                        SelectedPage.ViewModel.FinderItems.Refresh();
                    }

                    OnChangedSelectedPage(oldSelectedPage, SelectedPage);
                    CallPageItemOnPropertyChange();
                }
            }
        }

        public ICommand PageChangeCommand
        {
            get
            {
                return CreateCommand(
                    o => {
                        var pageVm = (PageViewModel<TFinderViewModel>)o;
                        if(pageVm.LoadState != LoadState.Loaded) {
                            SelectedPage = pageVm;
                            //pageVm.ViewModel.PropertyChanged += PageVm_PropertyChanged;
                            AttachmentChildProprtyChange(pageVm.ViewModel);
                            pageVm.ViewModel.LoadDefaultCacheAsync().ConfigureAwait(true);
                        } else {
                            SelectedPage = pageVm;
                        }
                    }
                );
            }
        }

        public void CallPageItemOnPropertyChange()
        {
            ParentFinder.CallPageItemOnPropertyChange();
        }

        #endregion

        private void PageVm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Mediation.Logger.Information(e.PropertyName);
            if(CustomChangePagePropertyNames.Any(n => n == e.PropertyName)) {
                CallPageItemOnPropertyChange();
            }
        }

    }
}
