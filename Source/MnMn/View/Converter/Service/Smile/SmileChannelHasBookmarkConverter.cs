using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.User;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Channel;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter.Service.Smile
{
    public class SmileChannelHasBookmarkConverter : IMultiValueConverter
    {
        #region IValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var information = (SmileChannelInformationViewModel)values[0];
            if(information == null) {
                return false;
            }

            var items = CastUtility.GetCastWPFValue<ICollectionView>(values[1], default(ICollectionView));
            if(items == null) {
                return false;
            }

            return items.SourceCollection
                .Cast<SmileChannelBookmarkItemViewModel>()
                .Any(i => i.ChannelId == information.ChannelId)
            ;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
