using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video
{
    /// <summary>
    /// VideoList.xaml の相互作用ロジック
    /// </summary>
    public partial class SmileVideoFinderControl : UserControl
    {
        public SmileVideoFinderControl()
        {
            InitializeComponent();

            //popupFilter.Visibility = Visibility.Visible;
        }

        #region HeaderContentProperty

        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(HeaderContentProperty)),
            typeof(object),
            typeof(SmileVideoFinderControl),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnHeaderContentChanged))
        );

        private static void OnHeaderContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoFinderControl;
            if(control != null) {
                control.HeaderContent = e.NewValue;
            }
        }

        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        #endregion

        #region FooterContentProperty

        public static readonly DependencyProperty FooterContentProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FooterContentProperty)),
            typeof(object),
            typeof(SmileVideoFinderControl),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnFooterContentChanged))
        );

        private static void OnFooterContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoFinderControl;
            if(control != null) {
                control.FooterContent = e.NewValue;
            }
        }

        public object FooterContent
        {
            get { return GetValue(FooterContentProperty); }
            set { SetValue(FooterContentProperty, value); }
        }

        #endregion

        #region CheckableProperty

        public static readonly DependencyProperty CheckableProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(CheckableProperty)),
            typeof(bool),
            typeof(SmileVideoFinderControl),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnFCheckableChanged))
        );

        private static void OnFCheckableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SmileVideoFinderControl;
            if(control != null) {
                control.Checkable = (bool)e.NewValue;
            }
        }

        public bool Checkable
        {
            get { return (bool)GetValue(CheckableProperty); }
            set { SetValue(CheckableProperty, value); }
        }

        #endregion

        #region function

        void OpenSubMenu(MenuItem menuItem)
        {
            if(menuItem.IsEnabled) {
                menuItem.IsSubmenuOpen = true;
            }
        }

        void ResetCommandProperty(MenuItem parentMenuItem)
        {
            var subMenuItems = parentMenuItem.Items
                .OfType<MenuItem>()
                .Where(m => m != null)
            ;

            foreach(var subMenuItem in subMenuItems) {
                var bind = BindingOperations.GetBinding(subMenuItem, MenuItem.CommandProperty);
                if(bind != null) {
                    BindingOperations.ClearBinding(subMenuItem, MenuItem.CommandProperty);
                    BindingOperations.SetBinding(subMenuItem, MenuItem.CommandProperty, bind);
                }
            }

        }

        #endregion

        private void UIElement_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void BookmarkMenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            OpenSubMenu(menuItem);
        }

        private void BookmarkMenuItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            OpenSubMenu(menuItem);
        }

        private void root_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FinderUtility.ScrollDataContextChanged(this.PART_List, e.OldValue, e.NewValue);
        }

        private void PART_List_Loaded(object sender, RoutedEventArgs e)
        {
            FinderUtility.ScrollItemsControlLoaded(this.PART_List, DataContext);
        }

        private void CopyMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            menuItem.SubmenuOpened -= CopyMenuItem_SubmenuOpened;

            ResetCommandProperty(menuItem);
        }

        private void FilterMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            menuItem.SubmenuOpened -= FilterMenuItem_SubmenuOpened;

            ResetCommandProperty(menuItem);
        }

        private void SearchMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            menuItem.SubmenuOpened -= SearchMenuItem_SubmenuOpened;

            ResetCommandProperty(menuItem);
        }

        //ContextMenuOpening="PART_List_ContextMenuOpening"
        //private void PART_List_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        //{
        //    var mainWindow = (MainWindow)UIUtility.GetVisualClosest<MainWindow>(this);
        //    var element = (FrameworkElement)sender;
        //    if(mainWindow.selectScale.IsChecked.GetValueOrDefault()) {
        //        element.ContextMenu.LayoutTransform = new ScaleTransform(mainWindow.scale.Value, mainWindow.scale.Value);
        //    } else {
        //        element.ContextMenu.LayoutTransform = new ScaleTransform(1, 1);
        //    }
        //}
    }
}
