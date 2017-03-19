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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.IF.Control
{
    /// <summary>
    /// ページャを保持するFinder。
    /// </summary>
    public interface IPagerFinder<TFinderViewModel, TInformationViewModel, TFinderItemViewModel>
        where TFinderViewModel : TFinderViewModelBase<TInformationViewModel, TFinderItemViewModel>
        where TInformationViewModel : InformationViewModelBase
        where TFinderItemViewModel : FinderItemViewModelBase<TInformationViewModel>
    {
        #region property

        /// <summary>
        /// 保持するページャ。
        /// </summary>
        CollectionModel<PageViewModel<TFinderViewModel>> PageItems { get; }
        /// <summary>
        /// 選択中ページ。
        /// </summary>
        PageViewModel<TFinderViewModel> SelectedPage { get; set; }

        /// <summary>
        /// <see cref="TFinderViewModelBase{TInformationViewModel, TFinderItemViewModel}.FinderItems"/>。
        /// </summary>
        ICollectionView FinderItems { get; }
        /// <summary>
        /// <see cref="TFinderViewModelBase{TInformationViewModel, TFinderItemViewModel}.FinderLoadState"/>。
        /// </summary>
        SourceLoadState FinderLoadState { get; set; }

        TFinderViewModel CurrentFinder { get; set; }

        #endregion

        #region command

        ICommand PageChangeCommand { get; }

        #endregion

        #region function

        void CallPageItemOnPropertyChange();

        #endregion
    }
}
