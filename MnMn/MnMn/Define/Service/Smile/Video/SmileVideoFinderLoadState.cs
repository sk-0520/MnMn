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

namespace ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video
{
    /// <summary>
    /// 動画リストの読込状態。
    /// </summary>
    public enum SmileVideoFinderLoadState
    {
        /// <summary>
        /// 読み込んでない。
        /// </summary>
        None,
        /// <summary>
        /// リスト取得中。
        /// </summary>
        VideoSourceLoading,
        /// <summary>
        /// リストチェック中。
        /// </summary>
        VideoSourceChecking,
        /// <summary>
        /// 動画情報取得中。
        /// </summary>
        InformationLoading,
        /// <summary>
        /// サムネイル取得中。
        /// </summary>
        ImageLoading,
        /// <summary>
        /// 完了。
        /// </summary>
        Completed,
        /// <summary>
        /// 失敗。
        /// </summary>
        Failure,
    }
}
