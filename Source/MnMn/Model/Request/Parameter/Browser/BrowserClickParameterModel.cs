using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Data.Browser;
using ContentTypeTextNet.MnMn.MnMn.Define;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.Browser
{
    public class BrowserClickParameterModel: BrowserParameterModelBase
    {
        public BrowserClickParameterModel(EventArgs e, WebNavigatorEngine engine)
            : base(e, engine)
        { }

        #region property

        public MouseButton MouseButton { get; set; }

        public SimpleHtmlElement Element { get; set; }

        public IList<SimpleHtmlElement> RootNodes { get; set; }

        #endregion
    }
}
