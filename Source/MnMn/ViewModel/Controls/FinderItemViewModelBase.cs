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
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public abstract class FinderItemViewModelBase: ViewModelBase, ICheckable
    {
        #region define

        public static int NoOrderd => -1;

        #endregion

        #region variable

        bool? _isChecked = false;
        int _number;
        bool _approval = true;

        #endregion

        protected FinderItemViewModelBase(int number)
        {
            Number = number;
        }

        #region property

        /// <summary>
        /// 表示位置番号。
        /// </summary>
        public int Number
        {
            get { return this._number; }
            set { SetVariableValue(ref this._number, value); }
        }

        /// <summary>
        /// フィルタにより表示が許可されているか。
        /// </summary>
        public bool Approval
        {
            get { return this._approval; }
            set { SetVariableValue(ref this._approval, value); }
        }

        #endregion

        #region command

        public ICommand ToggleCheckCommand
        {
            get { return CreateCommand(o => IsChecked = !IsChecked.GetValueOrDefault()); }
        }

        #endregion

        #region ICheckable

        public bool? IsChecked
        {
            get { return this._isChecked; }
            set { SetVariableValue(ref this._isChecked, value); }
        }

        #endregion
    }

    public class FinderItemViewModelBase<TInformationViewModel>: FinderItemViewModelBase
        where TInformationViewModel : InformationViewModelBase
    {
        protected FinderItemViewModelBase(TInformationViewModel information, int number)
            : base(number)
        {
            Information = information;
        }

        #region property

        public TInformationViewModel Information { get; }

        #endregion
    }
}
