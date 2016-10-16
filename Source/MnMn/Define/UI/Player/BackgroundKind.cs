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
using ContentTypeTextNet.MnMn.MnMn.Attribute;

namespace ContentTypeTextNet.MnMn.MnMn.Define.UI.Player
{
    /// <summary>
    /// プレイヤーの背景描画。
    /// </summary>
    public enum BackgroundKind
    {
        /// <summary>
        /// 自動算出色。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_UI_Player_BackgroundKind_AutoColor))]
        AutoColor,
        /// <summary>
        /// (たぶん選択した)色。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_UI_Player_BackgroundKind_SimpleColor))]
        SimpleColor,
    }
}
