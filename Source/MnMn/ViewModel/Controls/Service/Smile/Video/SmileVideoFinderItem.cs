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
using ContentTypeTextNet.MnMn.MnMn.IF.Control;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video
{
    /// <summary>
    /// <see cref="View.Controls.Service.Smile.Video.SmileVideoFinderControl"/>に設定するアイテム。
    /// <para><see cref="SmileVideoInformationViewModel"/>を<see cref="View.Controls.Service.Smile.Video.SmileVideoFinderControl"/>に設定するために超頑張ったのである。</para>
    /// </summary>
    public sealed class SmileVideoFinderItem: ViewModelBase, ICheckable
    {
        #region define

        public static int NoOrderd => -1;

        #endregion

        #region variable

        bool? _isChecked = false;
        int _number;

        #endregion

        public SmileVideoFinderItem(SmileVideoInformationViewModel information, int number)
        {
            Information = information;
            Number = number;
        }

        #region property

        public SmileVideoInformationViewModel Information { get; }

        public int Number
        {
            get { return this._number; }
            set { SetVariableValue(ref this._number, value); }
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
}
