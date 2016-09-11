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
using System.Windows.Data;
using ContentTypeTextNet.MnMn.MnMn.Model.MultiCommandParameter;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter
{
    public class MultiCommandParameterConverter: IMultiValueConverter
    {
        #region IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var baseNamespace = typeof(MultiCommandParameterBase).Namespace;
            var extensionName = (string)parameter;

            var typeName = baseNamespace + "." + extensionName.TrimStart('.');

            var converterType = Type.GetType(typeName, true, false);
            Debug.Assert(!converterType.IsAbstract);

            var result = Activator.CreateInstance(converterType, values, targetType, culture);
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
