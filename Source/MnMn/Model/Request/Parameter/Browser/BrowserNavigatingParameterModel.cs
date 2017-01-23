using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.Browser
{
    public class BrowserNavigatingParameterModel: BrowserParameterModelBase
    {
        public BrowserNavigatingParameterModel(EventArgs e, WebNavigatorEngine engine)
            : base(e, engine)
        { }

        #region property

        public Uri Uri { get; set; }

        #endregion
    }
}
