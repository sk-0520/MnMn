using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.Browser
{
    public class BrowserContextMenuItemParameterModel: BrowserClickParameterModel
    {
        public BrowserContextMenuItemParameterModel(Uri currentUri, EventArgs e, WebNavigatorEngine engine)
            : base(currentUri, e, engine)
        { }

        #region property

        public string Key { get; set; }

        #endregion
    }
}
