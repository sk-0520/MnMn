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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.MultiCommandParameter
{
    public abstract class MultiCommandParameterModelBase: ModelBase
    {
        public MultiCommandParameterModelBase(object[] values, Type targetType, CultureInfo culture)
        {
            ParameterMap = values
                .OfType<string>()
                .Where(s => !string.IsNullOrEmpty(s) && s.Any(c => c == ':'))
                .Select(s => s.Split(new[] { ':' }, 2))
                .ToDictionary(
                    kv => kv[0],
                    kv => kv[1]
                )
            ;
        }

        #region property

        /// <summary>
        /// キーと値を : で区切って格納。
        /// <para>キーは重複しない前提。</para>
        /// </summary>
        IDictionary<string, string> ParameterMap { get; }

        #endregion
    }
}
