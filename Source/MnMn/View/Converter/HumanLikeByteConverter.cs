﻿/*
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter
{
    [ValueConversion(typeof(long), typeof(string))]
    public class HumanLikeByteConverter: IValueConverter
    {
        #region property

        public string SizeFormat { get; set; }

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var size = System.Convert.ToInt64(value);

            if(string.IsNullOrWhiteSpace(SizeFormat)) {
                return RawValueUtility.ConvertHumanLikeByte(size);
            }

            return RawValueUtility.ConvertHumanLikeByte(size, SizeFormat);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
