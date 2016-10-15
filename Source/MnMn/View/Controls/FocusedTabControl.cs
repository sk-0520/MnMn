using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Package.stackoverflow.com;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    public class FocusedTabControl: TabControl
    {
        #region TabControl

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            var tabControl = e.Source as TabControl;
            if(tabControl != null && e.AddedItems.Count > 0) {
                var item = e.AddedItems[0];
                tabControl.ScrollToCenterOfView(item, true, true);
            }
            base.OnSelectionChanged(e);
        }

        #endregion
    }
}
