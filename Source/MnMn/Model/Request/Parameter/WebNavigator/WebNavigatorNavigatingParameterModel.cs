using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.WebNavigator
{
    public class WebNavigatorNavigatingParameterModel: WebNavigatorParameterModel
    {
        public WebNavigatorNavigatingParameterModel(Uri currentUri, EventArgs e, WebNavigatorEngine engine)
            : base(currentUri, e, engine, WebNavigatorParameterKind.Navigating)
        { }

        #region property

        public Uri NextUri { get; set; }

        #endregion
    }
}
