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
using ContentTypeTextNet.MnMn.MnMn.IF;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Extensions
{
    public static class ICommunicationExtensions
    {
        /// <summary>
        /// ICommunication.RequestからRequestModel.Resultを取得する糖衣。
        /// </summary>
        /// <typeparam name="TResult">キャスト可能であること前提。</typeparam>
        /// <param name="communication"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static TResult GetResultFromRequest<TResult>(this ICommunication communication, RequestModel request)
        {
            var responce = communication.Request(request);
            var result = (TResult)responce.Result;

            return result;
        }
    }
}
