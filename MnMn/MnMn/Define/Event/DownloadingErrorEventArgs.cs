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

namespace ContentTypeTextNet.MnMn.MnMn.Define.Event
{
    public class DownloadingErrorEventArgs: DownloaderEventArgs
    {
        public DownloadingErrorEventArgs(int counter, Exception ex)
        {
            Counter = counter;
            Exception = ex;
        }

        #region property

        /// <summary>
        /// 失敗回数。
        /// </summary>
        public int Counter { get; }
        /// <summary>
        /// 失敗内容。
        /// </summary>
        public Exception Exception { get; }

        //
        // 概要:
        //     イベントをキャンセルするかどうかを示す値を取得または設定します。
        //
        // 戻り値:
        //     イベントを取り消す場合は true。それ以外の場合は false。
        public bool Cancel { get; set; }

        #endregion
    }
}
