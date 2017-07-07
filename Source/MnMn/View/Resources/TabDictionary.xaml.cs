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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;

namespace ContentTypeTextNet.MnMn.MnMn.View.Resources
{
    partial class TabDictionary
    {
        #region function

        static TabControl GetTabControl(DependencyObject childElement)
        {
            var visual = childElement;

            do {
                var tabControl = visual as TabControl;
                if(tabControl != null) {
                    return tabControl;
                }
                visual = VisualTreeHelper.GetParent(visual);
            } while(true);

            throw new Exception($"{nameof(TabDictionary)}としては使用スコープ的に到達不可");
        }

        static IList<TabItem> GetTabItems(TabControl tabControl)
        {
            var tabItems = tabControl.Items.OfType<TabItem>();
            if(!tabItems.Any()) {
                tabItems = UIUtility.FindVisualChildren<TabItem>(tabControl);
            }

            var baseItem = tabItems.FirstOrDefault();

            if(baseItem != null) {
                return tabItems.Where(t => t.Parent == baseItem.Parent).ToEvaluatedSequence();
            }

            return new List<TabItem>();
        }

        static int GetNextIndex(IList<TabItem> tabItems, int currentIndex, bool isUp)
        {
            if(isUp) {
                return currentIndex == 0
                    ? tabItems.Count - 1
                    : currentIndex - 1
                ;
            } else {
                return currentIndex == tabItems.Count - 1
                    ? 0
                    : currentIndex + 1
                ;
            }
        }

        //void SelectTabMenuItem(TabControl tabControl, TabItem tabItem, int index)
        //{
        //    tabControl.SelectedIndex = index;
        //}

        #endregion

        private void DockPanel_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(!Constants.FinderTabTabHeaderEnabledMouseWheel) {
                return;
            }

            var tabControl = GetTabControl((DependencyObject)sender);
            var tabItems = GetTabItems(tabControl);
            if(1 < tabItems.Count) {
                //var selectedItem = tabItems.First(t => t.IsSelected);
                //var selectedIndex = tabItems.FindIndex(i => i == selectedItem);
                var selectedIndex = tabControl.SelectedIndex;

                var currentIndex = selectedIndex;
                var nextIndex = GetNextIndex(tabItems, selectedIndex, 0 < e.Delta);
                while((nextIndex != -1 && nextIndex < tabItems.Count) && (!tabItems[nextIndex].IsEnabled || tabItems[nextIndex].Visibility != Visibility.Visible)) {
                    nextIndex = GetNextIndex(tabItems, nextIndex, 0 < e.Delta);
                }

                //Debug.WriteLine($"{tabItems.Count}: {selectedIndex} -> {nextIndex}");

                tabControl.SelectedIndex = nextIndex;
            }

            e.Handled = true;
        }

        //private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        //{
        //    var tabControl = GetTabControl((DependencyObject)sender);
        //    var tabs = GetTabItems(tabControl)
        //        //.Reverse()
        //        .Select((t, i) => new { TabItem = t, Index = i })
        //    ;
        //    //var itemssss = tabControl.Items;
        //    var contextMenu = ((FrameworkElement)sender).ContextMenu;

        //    var menuItems = new List<MenuItem>();
        //    foreach(var tab in tabs) {
        //        var menuItem = new MenuItem();
        //        var text = UIUtility.FindChildren<TextBlock>(tab.TabItem).First();
        //        menuItem.DataContext = menuItem;
        //        menuItem.Header = text.Text;
        //        menuItem.Command = new DelegateCommand(
        //            o => SelectTabMenuItem(tabControl, tab.TabItem, tab.Index),
        //            o => menuItem.IsEnabled
        //        );
        //        menuItems.Add(menuItem);
        //    }
        //    foreach(var menuItem in contextMenu.Items.Cast<MenuItem>()) {
        //        menuItem.Command = null;
        //    }

        //    contextMenu.ItemsSource = menuItems;
        //}

        void FindTabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.MiddleButton == MouseButtonState.Pressed) {
                var tabItem = (TabItem)sender;

                var closeButton = UIUtility.FindChildren<Button>(tabItem)
                    .FirstOrDefault(b => b.Tag == Constants.TagTabCloseButton)
                ;
                if(closeButton != null) {
                    closeButton.Command.TryExecute(tabItem.DataContext);
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var senderElement = (UIElement)sender;
            var senderTabControl = GetTabControl(senderElement);
            var tabItems = GetTabItems(senderTabControl);
        }
    }
}
