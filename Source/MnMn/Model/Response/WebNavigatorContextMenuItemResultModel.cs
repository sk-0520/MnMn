using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Response
{
    public class WebNavigatorContextMenuItemResultModel: WebNavigatorResultModel
    {
        public WebNavigatorContextMenuItemResultModel(bool cancel, bool isEnabled, bool isVisible)
            :base(cancel)
        {
            IsEnabled = isEnabled;
            IsVisible = isVisible;
        }

        #region property

        public bool IsEnabled { get; }
        public bool IsVisible { get; }

        #endregion
    }
}
