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
    [ValueConversion(typeof(string), typeof(string))]
    public class BookmarkLevelNameConverter: IMultiValueConverter
    {
        #region IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (string)values[0];
            var level = (int)values[1];

            if(string.IsNullOrWhiteSpace(name)) {
                return name;
            }

            var safeName = name.Replace("_", "__");
            var indent = new string(' ', level);

            return indent + safeName;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
