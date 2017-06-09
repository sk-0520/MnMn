using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter
{
    [ValueConversion(typeof(KeyBinding), typeof(string))]
    public class KeyBindingToGestureTextConverter : IValueConverter
    {
        #region define

        static KeyBinding DefaultKeyBinding { get; } = new KeyBinding();

        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var keyBinding = CastUtility.GetCastWPFValue<KeyBinding>(value, null);
            if(keyBinding == null) {
                return value;
            }

            return $"{keyBinding.Modifiers} + {keyBinding.Key}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
