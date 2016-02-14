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

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class FileNameUtility
    {
        static string CreateFileName_Impl(string name, string roll, string extension)
        {
            return $"{name}{(roll == null ? string.Empty : "-" + roll)}.{extension}";
        }
        public static string CreateFileName(string name, string roll, string extension)
        {
            CheckUtility.EnforceNotNullAndNotEmpty(roll);
            return CreateFileName_Impl(name, roll, extension);
        }
        public static string CreateFileName(string name, string extension)
        {
            return CreateFileName_Impl(name, null, extension);
        }
    }
}
