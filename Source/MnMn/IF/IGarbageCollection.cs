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
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.IF
{
    /// <summary>
    /// ゴミ処理を実施する。
    /// </summary>
    public interface IGarbageCollection
    {
        /// <summary>
        /// ゴミ処理。
        /// </summary>
        /// <param name="garbageCollectionLevel">どんだけ処理するか。</param>
        /// <param name="cacheSpan">対象キャッシュ時間。</param>
        /// <returns>処理の成功・失敗と成功時に処理サイズ</returns>
        CheckResultModel<long> GarbageCollection(GarbageCollectionLevel garbageCollectionLevel, CacheSpan cacheSpan);
    }
}
