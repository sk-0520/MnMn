using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter
{
    [Obsolete]
    [ValueConversion(typeof(double), typeof(double))]
    public class NegativeDoubleConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var num = (double)value;
            return -num;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var num = (double)value;
            return -num;
        }

        #endregion
    }
}
