using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter
{
    [ValueConversion(typeof(string), typeof(Color))]
    public class TextToColorConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorText = (string)value;
            return RawValueUtility.ConvertColor(colorText);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = CastUtility.GetCastWPFValue(value, Colors.Transparent);
            return color.ToString();
        }

        #endregion
    }
}
