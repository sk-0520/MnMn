using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter.Service.Smile.Video
{
    [ValueConversion(typeof(int), typeof(Thickness))]
    public class BookmarkLevelToPaddingConverter: BookmarkLevelConverter
    {
        #region BookmarkLevelConverter

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var width = (double)base.Convert(value, targetType, parameter, culture);
            return new Thickness(width, 0, 0, 0);
        }

        #endregion
    }
}
