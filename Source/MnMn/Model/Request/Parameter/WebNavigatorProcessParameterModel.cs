using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter
{
    public class WebNavigatorProcessParameterModel: ProcessParameterModelBase
    {
        #region property

        public string ParameterVaule { get; set; }
        public WebNavigatorContextMenuItemViewModel MenuItem { get; set; }

        #endregion
    }
}
