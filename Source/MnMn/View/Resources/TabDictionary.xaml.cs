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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;

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
            var tabItems = UIUtility.FindChildren<TabItem>(tabControl);
            var baseItem = tabItems.FirstOrDefault();

            if(baseItem != null) {
                return tabItems.Where(t => t.Parent == baseItem.Parent).ToList();
            }

            return new List<TabItem>();
        }

        static int GetNextIndex(IList<TabItem> tabItems, int currentIndex, bool isUp)
        {
            if(isUp) {
                return currentIndex == tabItems.Count - 1
                    ? 0
                    : currentIndex + 1
                ;
            } else {
                return currentIndex == 0
                    ? tabItems.Count - 1
                    : currentIndex - 1
                ;
            }
        }

        #endregion

        private void DockPanel_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var tabControl = GetTabControl((DependencyObject)sender);
            var tabItems = GetTabItems(tabControl);
            if(1 < tabItems.Count) {
                //var selectedItem = tabItems.First(t => t.IsSelected);
                //var selectedIndex = tabItems.FindIndex(i => i == selectedItem);
                var selectedIndex = tabControl.SelectedIndex;

                var nextIndex = GetNextIndex(tabItems, selectedIndex, 0 < e.Delta);
                //Debug.WriteLine($"{tabItems.Count}: {selectedIndex} -> {nextIndex}");

                tabControl.SelectedIndex = nextIndex;
            }
        }
    }
}
