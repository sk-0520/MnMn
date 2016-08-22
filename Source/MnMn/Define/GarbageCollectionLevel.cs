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
    /// ゴミ掃除の規模。
    /// </summary>
    [Flags]
    public enum GarbageCollectionLevel
    {
        /// <summary>
        /// 完全な一時データ。
        /// </summary>
        Temporary = 0x01,
        /// <summary>
        /// 小さなデータ。
        /// </summary>
        Small = 0x02,
        /// <summary>
        /// 大きなデータ。
        /// </summary>
        Large = 0x04,
        /// <summary>
        /// 全データ。
        /// </summary>
        All = Temporary | Small | Large,
    }
}
