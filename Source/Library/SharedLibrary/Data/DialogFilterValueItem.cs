﻿/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Data
{
    /// <summary>
    /// ダイアログで使用するフィルタのアイテム。
    /// <para>値を保持する。</para>
    /// </summary>
    public class DialogFilterValueItem<TValue>: DialogFilterItem
    {
        public DialogFilterValueItem(TValue value, string displayText, IEnumerable<string> wildcard)
            : base(displayText, wildcard)
        {
            Value = value;
        }

        public DialogFilterValueItem(TValue value, string displayText, params string[] wildcard)
            : base(displayText, wildcard)
        {
            Value = value;
        }

        #region property

        /// <summary>
        /// 保持する値。
        /// </summary>
        public TValue Value { get; }

        #endregion

    }
}
