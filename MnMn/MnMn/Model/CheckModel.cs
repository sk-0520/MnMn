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

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    /// <summary>
    /// 結果を格納。
    /// <para>成功と状態、その他もろもろ。</para>
    /// </summary>
    public class CheckModel: ModelBase
    {
        public CheckModel(bool isSuccess, object detail, string message)
        {
            IsSuccess = isSuccess;
            Detail = detail;
            Message = message;
        }

        #region property

        /// <summary>
        /// 成功状態。
        /// <para>必須項目。</para>
        /// </summary>
        public bool IsSuccess { get; private set; }
        /// <summary>
        /// 詳細。
        /// </summary>
        public object Detail { get; private set; }
        /// <summary>
        /// メッセージ。
        /// </summary>
        public string Message { get; private set; }

        #endregion

        #region function 

        public static CheckModel Success()
        {
            return new CheckModel(true, null, null);
        }

        public static CheckModel Failure()
        {
            return new CheckModel(false, null, null);
        }

        #endregion
    }
}
