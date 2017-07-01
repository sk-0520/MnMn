using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter
{
    public class WebNavigatorProcessParameterModel: ProcessParameterModelBase, IReadOnlyKey
    {
        #region property

        public string ParameterVaule { get; set; }

        #endregion

        #region IReadOnlyKey

        public string Key { get; set; }

        #endregion
    }
}
