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
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public abstract class PageViewModelBase: ViewModelBase, ICheckable
    {
        #region variable

        LoadState _loadState;
        bool? _isChecked;

        #endregion

        public PageViewModelBase(int pageNumber)
        {
            PageNumber = pageNumber;
        }

        #region property

        public int PageNumber { get; }

        public LoadState LoadState
        {
            get { return this._loadState; }
            set { SetVariableValue(ref this._loadState, value); }
        }

        #endregion

        #region 

        public bool? IsChecked
        {
            get { return this._isChecked; }
            set { SetVariableValue(ref this._isChecked, value); }
        }

        #endregion
    }

    public class PageViewModel: PageViewModelBase
    {
        public PageViewModel(ViewModelBase viewModelBase, int pageNumber)
            : base(pageNumber)
        {
            ViewModelBase = viewModelBase;
        }

        #region property

        public ViewModelBase ViewModelBase { get; }

        #endregion
    }

    public class PageViewModel<TViewModel>: PageViewModel
        where TViewModel : ViewModelBase
    {
        public PageViewModel(TViewModel viewModel, int pageNumber)
            : base(viewModel, pageNumber)
        {
            ViewModel = viewModel;
        }

        #region property

        public TViewModel ViewModel { get; }

        #endregion
    }
}
