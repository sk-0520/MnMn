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
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Logic
{
    public class CacheSpan
    {
        #region define

        /// <summary>
        /// 期間なし。
        /// </summary>
        public static CacheSpan NoCache => new CacheSpan();
        /// <summary>
        /// 無限。
        /// </summary>
        public static CacheSpan InfinityCache => new CacheSpan(DateTime.MaxValue, TimeSpan.MaxValue);

        #endregion

        CacheSpan()
        {
            IsEnabled = false;
        }

        public CacheSpan(DateTime baseTIme, TimeSpan expires)
        {
            IsEnabled = true;

            BaseTime = baseTIme;
            Expires = expires;
        }

        #region property

        /// <summary>
        /// 有効か。
        /// </summary>
        public bool IsEnabled { get; }
        /// <summary>
        /// 基準時間。
        /// </summary>
        public DateTime BaseTime { get; }
        /// <summary>
        /// 基準時間からの有効範囲時間。
        /// </summary>
        public TimeSpan Expires { get; }

        /// <summary>
        /// 有効期限なしか。
        /// <para><see cref="NoCache"/>と異なりユーザー設定が 00:00:00 を意味し、<see cref="IsCacheTime(DateTime)"/>が無条件成立する。</para>
        /// </summary>
        public bool IsNoExpiration => Expires == TimeSpan.Zero;

        #endregion

        #region function

        /// <summary>
        /// キャッシュ時間内か。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public bool IsCacheTime(DateTime dateTime)
        {
            return IsEnabled && BaseTime - dateTime < Expires;
        }

        #endregion

        #region Object

        public override string ToString()
        {
            return $"{this.GetType().Name}: {nameof(IsEnabled)} = {IsEnabled}, {nameof(BaseTime)} = {BaseTime}, {nameof(Expires)} = {Expires}";
        }

        #endregion
    }
}
