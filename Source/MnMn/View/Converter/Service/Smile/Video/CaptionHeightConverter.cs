using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter.Service.Smile.Video
{
    public class CaptionHeightConverter: IMultiValueConverter
    {
        #region IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isNormalWindow = (bool)values[0];
            var realHeight = (double)values[1];

            if(isNormalWindow) {
                return realHeight;
            } else {
                return (double)parameter;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
