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
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;

namespace ContentTypeTextNet.MnMn.MnMn.IF
{
    /// <summary>
    /// 通信用IF。
    /// </summary>
    public interface ICommunication
    {
        /// <summary>
        /// 何かしら要求して何かしらを返してもらう。
        /// </summary>
        /// <param name="request">要求。</param>
        /// <returns>応答。</returns>
        ResponseModel Request(RequestModel request);

        /// <summary>
        /// 何かしらを命令する。
        /// <para><<see cref="Request(RequestModel)"/>とは異なり命令を実施したか拒否されたかのみが結果となる。</para>
        /// </summary>
        /// <param name="order">命令。</param>
        /// <returns>真で実施、偽で拒否。</returns>
        bool Order(OrderModel order);
    }
}
