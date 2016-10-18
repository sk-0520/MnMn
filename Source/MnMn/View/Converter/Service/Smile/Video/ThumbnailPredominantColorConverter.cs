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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.View.Converter;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter.Service.Smile.Video
{
    [ValueConversion(typeof(BitmapSource), typeof(Color))]
    public class ThumbnailPredominantColorConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var baseConverter = new PredominantColorConverter();

            var bitmap = CastUtility.GetCastWPFValue(value, default(BitmapSource));
            if(bitmap == null) {
                return baseConverter.Convert(value, targetType, parameter, culture);
            }

            var barTop = 12;
            var barBottom = 14;
            var croppedBitmap = new CroppedBitmap(bitmap, new Int32Rect(0, barTop, bitmap.PixelWidth, bitmap.PixelHeight - (barTop + barBottom)));
            var result = baseConverter.Convert(croppedBitmap, targetType, parameter, culture);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
