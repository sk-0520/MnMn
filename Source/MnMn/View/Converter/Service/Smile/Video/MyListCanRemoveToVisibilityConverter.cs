using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter.Service.Smile.Video
{
    [ValueConversion(typeof(SmileVideoMyListFinderViewModelBase), typeof(Visibility))]
    public class MyListCanRemoveToVisibilityConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var finder = value as SmileVideoMyListFinderViewModelBase;
            if(finder != null) {
                var converter = new BooleanToVisibilityConverter();
                return converter.Convert(finder.CanRemove, targetType, parameter, culture);
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
