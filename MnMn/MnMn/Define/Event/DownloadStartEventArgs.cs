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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.Define.Event
{
    public class DownloadStartEventArgs: DownloaderEventArgs
    {
        public DownloadStartEventArgs(DownloadStartType downloadStartType)
            : this(downloadStartType, 0)
        { }

        public DownloadStartEventArgs(DownloadStartType downloadStartType, long rangeHeadPosition)
            : this(downloadStartType, rangeHeadPosition, Downloader.RangeAll)
        { }

        public DownloadStartEventArgs(DownloadStartType downloadStartType, long rangeHeadPosition, long rangeTailPosition)
        {
            if(rangeTailPosition != Downloader.RangeAll) {
                CheckUtility.Enforce<ArgumentException>(rangeHeadPosition < rangeTailPosition);
            }

            DownloadStartType = downloadStartType;
            RangeHeadPosition = rangeHeadPosition;
            RangeHeadPosition = rangeTailPosition;
        }

        #region property

        public DownloadStartType DownloadStartType { get; }

        public long RangeHeadPosition { get; }
        public long RangeTailPosition { get; }

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
