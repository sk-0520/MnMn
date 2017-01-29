using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.MnMn.Data.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.Define;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.WebNavigator
{
    public class WebNavigatorClickParameterModel: WebNavigatorParameterModel
    {
        protected WebNavigatorClickParameterModel(Uri currentUri, EventArgs e, WebNavigatorEngine engine, WebNavigatorParameterKind kind)
            : base(currentUri, e, engine, kind)
        { }

        public WebNavigatorClickParameterModel(Uri currentUri, EventArgs e, WebNavigatorEngine engine)
            : this(currentUri, e, engine, WebNavigatorParameterKind.Click)
        { }

        #region property

        public MouseButton MouseButton { get; set; }

        public SimpleHtmlElement Element { get; set; }

        public IList<SimpleHtmlElement> RootNodes { get; set; }

        #endregion
    }
}
