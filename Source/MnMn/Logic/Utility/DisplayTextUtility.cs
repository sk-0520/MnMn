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
using ContentTypeTextNet.MnMn.MnMn.Attribute;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    /// <summary>
    /// <see cref="DisplayTextAttributeBase"/>に対する処理。
    /// </summary>
    public static class DisplayTextUtility
    {
        public static string GetDisplayText(object value)
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString()).FirstOrDefault();
            if(memberInfo != null) {
                var attrs = memberInfo.GetCustomAttributes(typeof(DisplayTextAttributeBase), true);
                if(attrs != null && attrs.Length > 0) {
                    var display = ((DisplayTextAttributeBase)attrs[0]);
                    return display.Text;
                }
            }

            return value.ToString();
        }
    }
}
