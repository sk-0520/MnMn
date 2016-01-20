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

namespace ContentTypeTextNet.MnMn.MnMn.Define
{
    /// <summary>
    /// セッション状態。
    /// </summary>
    public enum LoginState
    {
        /// <summary>
        /// ログインしていない。
        /// </summary>
        None,
        /// <summary>
        /// ログイン中。
        /// </summary>
        In,
        /// <summary>
        /// ログインチェック中。
        /// </summary>
        Check,
        /// <summary>
        /// ログイン済み。
        /// </summary>
        Logged,
        /// <summary>
        /// ログイン失敗。
        /// </summary>
        Failure,
    }
}
