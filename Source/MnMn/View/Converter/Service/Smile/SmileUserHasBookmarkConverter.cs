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
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.User;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.User;

namespace ContentTypeTextNet.MnMn.MnMn.View.Converter.Service.Smile
{
    public class SmileUserHasBookmarkConverter: IMultiValueConverter
    {
        #region IValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var information = values[0] as SmileUserInformationViewModel;
            if(information == null) {
                return false;
            }

            var items = CastUtility.GetCastWPFValue<ICollectionView>(values[1], default(ICollectionView));
            if(items == null) {
                return false;
            }

            return items.SourceCollection
                .Cast<SmileUserBookmarkItemViewModel>()
                .Any(i => i.UserId == information.UserId)
            ;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
