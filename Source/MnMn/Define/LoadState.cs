﻿/*
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

namespace ContentTypeTextNet.MnMn.MnMn.Define
{
    /// <summary>
    /// 読込状態。
    /// <para>読み込んだものがどういったものかまでは状態表示しない。</para>
    /// </summary>
    public enum LoadState
    {
        /// <summary>
        /// 何もしてない。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_LoadState_None))]
        None,
        /// <summary>
        /// 準備中。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_LoadState_Preparation))]
        Preparation,
        /// <summary>
        /// 読み込み中。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_LoadState_Loading))]
        Loading,
        /// <summary>
        /// 読み込んだ。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_LoadState_Loaded))]
        Loaded,
        /// <summary>
        /// 読み込み失敗。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_LoadState_Failure))]
        Failure,
    }
}
