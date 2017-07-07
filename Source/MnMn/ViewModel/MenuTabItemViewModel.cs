using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

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

        #endregion
    }
}
