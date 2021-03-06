﻿/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    /// <summary>
    /// それっぽい変換屋さん。
    /// </summary>
    public static class CovertUtility
    {
        /// <summary>
        /// オブジェクトをバイナリに変換する。
        /// </summary>
        /// <param name="obj">http://stackoverflow.com/questions/4865104/convert-any-object-to-a-byte?answertab=votes#tab-top</param>
        /// <returns></returns>
        public static byte[] ToByteArray(object obj)
        {
            CheckUtility.EnforceNotNull(obj);

            BinaryFormatter bf = new BinaryFormatter();
            using(var ms = new MemoryStream()) {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
