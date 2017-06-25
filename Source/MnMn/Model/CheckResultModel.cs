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
using ContentTypeTextNet.MnMn.Library.Bridging.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.Model
{
    public class CheckResultModel<T>: CheckModel, IReadOnlyCheckResult<T>
    {
        public CheckResultModel(bool isSuccess, T result, object detail, string message)
            : base(isSuccess, detail, message)
        {
            Result = result;
        }

        #region property

        public T Result { get; private set; }

        #endregion

        #region function

        #endregion
    }

    public static class CheckResultModel
    {
        public static IReadOnlyCheckResult<T> Success<T>(T result)
        {
            return new CheckResultModel<T>(true, result, null, null);
        }

        public static IReadOnlyCheckResult<T> Failure<T>()
        {
            return new CheckResultModel<T>(false, default(T), null, null);
        }

        public static IReadOnlyCheckResult<T> Failure<T>(string message)
        {
            return new CheckResultModel<T>(false, default(T), null, message);
        }
    }
}
