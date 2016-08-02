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

namespace ContentTypeTextNet.MnMn.MnMn.Define
{
    /// <summary>
    /// 読込状態。
    /// <para><see cref="ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video.SmileVideoFinderLoadState"/>から切り替えていく。</para>
    /// </summary>
    public enum SourceLoadState
    {
        /// <summary>
        /// 読み込んでない。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_SourceLoadState_None))]
        None,
        /// <summary>
        /// 取得中。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_SourceLoadState_SourceLoading))]
        SourceLoading,
        /// <summary>
        /// リストチェック中。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_SourceLoadState_SourceChecking))]
        SourceChecking,
        /// <summary>
        /// 情報取得中。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_SourceLoadState_InformationLoading))]
        InformationLoading,
        /// <summary>
        /// 完了。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_SourceLoadState_Completed))]
        Completed,
        /// <summary>
        /// 失敗。
        /// </summary>
        [EnumResourceDisplay(nameof(Properties.Resources.String_App_Define_SourceLoadState_Failure))]
        Failure,
    }
}

