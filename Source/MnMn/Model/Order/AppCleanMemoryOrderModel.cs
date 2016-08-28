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
using ContentTypeTextNet.MnMn.MnMn.Model.Request;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Order
{
    /// <summary>
    /// GC用指示内容。
    /// </summary>
    public class AppCleanMemoryOrderModel: OrderModel
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="isTargetLargeObjectHeap">LOHを含めるか</param>
        public AppCleanMemoryOrderModel(bool isTargetLargeObjectHeap)
            : base(Define.OrderKind.CleanMemory, Define.ServiceType.Application)
        {
            IsTargetLargeObjectHeap = isTargetLargeObjectHeap;
        }

        #region property

        /// <summary>
        /// LOHを含めるか。
        /// </summary>
        public bool IsTargetLargeObjectHeap { get; }

        #endregion
    }
}
