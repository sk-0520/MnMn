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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.View.Attachment
{
    public class PasswordBoxPasswordBehavior: Behavior<PasswordBox>
    {
        #region define

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(PasswordProperty)),
            typeof(string), 
            typeof(PasswordBoxPasswordBehavior), 
            new UIPropertyMetadata(null)
        );

        #endregion

        #region property

        public string Password
        {
            get { return GetValue(PasswordProperty) as string; }
            set { SetValue(PasswordProperty, value); }
        }

        #endregion

        #region Behavior

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Password = (string)GetValue(PasswordProperty);

            this.AssociatedObject.PasswordChanged -= PasswordChanged;
            this.AssociatedObject.PasswordChanged += PasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PasswordChanged -= PasswordChanged;
        }

        #endregion

        void PasswordChanged(object sender, RoutedEventArgs e)
        {
            SetValue(PasswordProperty, ((PasswordBox)sender).Password);
        }
    }
}
