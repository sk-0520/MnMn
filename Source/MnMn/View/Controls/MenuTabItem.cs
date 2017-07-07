using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    public class MenuTabItem: TabItem
    {
        #region MenuItemHeaderProperty

        public static readonly DependencyProperty MenuItemHeaderProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(MenuItemHeaderProperty)),
            typeof(object),
            typeof(MenuTabItem),
            new FrameworkPropertyMetadata(default(object), new PropertyChangedCallback(OnMenuItemHeaderChanged))
        );

        private static void OnMenuItemHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MenuTabItem;
            if(control != null) {
                control.MenuItemHeader = e.NewValue;
            }
        }

        public object MenuItemHeader
        {
            get { return GetValue(MenuItemHeaderProperty); }
            set { SetValue(MenuItemHeaderProperty, value); }
        }

        #endregion

        #region MenuItemIconProperty

        public static readonly DependencyProperty MenuItemIconProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(MenuItemIconProperty)),
            typeof(object),
            typeof(MenuTabItem),
            new FrameworkPropertyMetadata(default(object), new PropertyChangedCallback(OnMenuItemIconChanged))
        );

        private static void OnMenuItemIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MenuTabItem;
            if(control != null) {
                control.MenuItemIcon = e.NewValue;
            }
        }

        public object MenuItemIcon
        {
            get { return GetValue(MenuItemIconProperty); }
            set { SetValue(MenuItemIconProperty, value); }
        }

        #endregion
    }
}
