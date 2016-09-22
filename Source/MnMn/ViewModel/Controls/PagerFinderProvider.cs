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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define.Event;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;

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

        PageViewModel<TFinderViewModel> _selectedPage;

        #endregion

        public PagerFinderProvider(IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel> parentFinder, IEnumerable<string> changePropertyNames)
        {
            ParentFinder = parentFinder;
            if(changePropertyNames.Any()) {
                CustomChangePagePropertyNames.AddRange(changePropertyNames);
            }
        }

        public PagerFinderProvider(IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel> parentFinder)
            : this(parentFinder, Enumerable.Empty<string>())
        { }

        #region property

        CollectionModel<string> CustomChangePagePropertyNames { get; } = new CollectionModel<string>(ChangePagePropertyNames);

        IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel> ParentFinder { get; }

        #endregion

        #region property

        public TFinderViewModel SelectedFinder => SelectedPage.ViewModel;

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
                var prevSelectedPage = SelectedPage;
                if(SetVariableValue(ref this._selectedPage, value)) {
                    OnChangedSelectedPage(prevSelectedPage, SelectedPage);
                }
            }
        }

        public ICommand PageChangeCommand
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        #endregion

    }
}
