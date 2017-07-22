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
    /// 命令種別。
    /// </summary>
    public enum OrderKind
    {
        /// <summary>
        /// データ保存。
        /// </summary>
        Save,
        /// <summary>
        /// 終了。
        /// </summary>
        Exit,
        /// <summary>
        /// 再起動。
        /// </summary>
        Reboot,
        /// <summary>
        /// メモリのGC。
        /// </summary>
        CleanMemory,
        /// <summary>
        /// ダウンロードマネージャに登録
        /// </summary>
        Download,
        /// <summary>
        /// スリープとかロック回り。
        /// </summary>
        SystemBreak,
        /// <summary>
        /// プロセス間通信用処理。
        /// </summary>
        ProcessLink,
    }
}
