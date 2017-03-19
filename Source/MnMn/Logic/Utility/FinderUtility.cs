using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.Logic.Utility
{
    public static class FinderUtility
    {
        #region function

        public static void ScrollDataContextChanged(ItemsControl itemsControl, object oldValue, object newValue)
        {
            var scrollViewer = UIUtility.FindVisualChildren<ScrollViewer>(itemsControl).FirstOrDefault();
            if(scrollViewer != null) {
                var oldFinder = (FinderViewModelBase)oldValue;
                if(oldFinder != null) {
                    oldFinder.ScrollPositionY = scrollViewer.VerticalOffset;
                }

                var newFinder = (FinderViewModelBase)newValue;
                if(newFinder != null) {
                    scrollViewer.ScrollToVerticalOffset(newFinder.ScrollPositionY);
                }
            }
        }

        public static void ScrollItemsControlLoaded(ItemsControl itemsControl, object dataContext)
        {
            var scrollViewer = UIUtility.FindVisualChildren<ScrollViewer>(itemsControl).FirstOrDefault();
            if(scrollViewer != null) {
                var finder = (FinderViewModelBase)dataContext;
                if(finder != null) {
                    scrollViewer.ScrollToVerticalOffset(finder.ScrollPositionY);
                }
            }
        }

        #endregion
    }
}
