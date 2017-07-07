using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.View.Attachment;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel
{
    public class MenuTabItemViewModel: ViewModelBase
    {
        public MenuTabItemViewModel(TabItem tabItem)
        {
            TabItem = tabItem;
        }

        #region property

        TabItem TabItem { get; set; }

        public object Header
        {
            get
            {
                if(TabItem == null) {
                    return null;
                }

                var menuTabItemHeader = MenuTabItem.GetHeader(TabItem);
                if(menuTabItemHeader != null) {
                    return menuTabItemHeader;
                }

                var textHeader = TabItem.Header as string;
                if(textHeader != null) {
                    return textHeader;
                }


                return TabItem.Header;
            }
        }

        public object Icon
        {
            get
            {
                if(TabItem == null) {
                    return null;
                }

                var menuTabItemIcon = MenuTabItem.GetIcon(TabItem);
                if(menuTabItemIcon != null) {
                    var element = menuTabItemIcon as FrameworkElement;
                    if(element != null) {
                        element.ApplyTemplate();

                    }
                    return menuTabItemIcon;
                }

                return null;
            }
        }

        #endregion
    }
}
