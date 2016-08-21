﻿/*
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
using System.Windows.Interactivity;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.View.Attachment
{
    public class WindowTitleBehavior: Behavior<Window>
    {

        #region ServiceProperty

        public static readonly DependencyProperty ServiceProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(ServiceProperty)),
            typeof(ServiceType),
            typeof(WindowTitleBehavior),
            new FrameworkPropertyMetadata(default(ServiceType), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnServiceChanged))
        );

        private static void OnServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WindowTitleBehavior;
            if(control != null) {
                control.Service = (ServiceType)e.NewValue;
            }
        }

        public ServiceType Service
        {
            get { return (ServiceType)GetValue(ServiceProperty); }
            set { SetValue(ServiceProperty, value); }
        }

        #endregion

        // 要素にアタッチされたときの処理。大体イベントハンドラの登録処理をここでやる
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Title = Service.ToString();
        }
    }
}
