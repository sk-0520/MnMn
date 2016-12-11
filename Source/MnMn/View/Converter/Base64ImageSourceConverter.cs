using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter
{
    [Obsolete("インフラ作成時に作ったけどあきらめたので出番なし")]
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class Base64ImageSourceConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (string)value;
            var binary = System.Convert.FromBase64String(source);
            using(var stream = new MemoryStream(binary)) {
                return BitmapFrame.Create(stream);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
