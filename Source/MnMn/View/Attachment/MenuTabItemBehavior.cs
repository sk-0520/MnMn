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
    public class MenuTabItem
    {
        #region HeaderProperty

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.RegisterAttached(
            DependencyPropertyUtility.GetName(nameof(HeaderProperty)),
            typeof(object),
            typeof(MenuTabItem),
            new FrameworkPropertyMetadata(default(object), new PropertyChangedCallback(OnHeaderChanged))
        );

        static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetHeader(d, e.NewValue);
        }

        public static object GetHeader(DependencyObject obj)
        {
            return obj.GetValue(HeaderProperty);
        }

        public static void SetHeader(DependencyObject obj, object value)
        {
            obj.SetValue(HeaderProperty, value);
        }

        #endregion

        #region IconProperty

        public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached(
            DependencyPropertyUtility.GetName(nameof(IconProperty)),
            typeof(object),
            typeof(MenuTabItem),
            new FrameworkPropertyMetadata(default(object), new PropertyChangedCallback(OnIconChanged))
        );

        static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetIcon(d, e.NewValue);
        }

        public static object GetIcon(DependencyObject obj)
        {
            return obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, object value)
        {
            obj.SetValue(IconProperty, value);
        }

        #endregion
    }
}
