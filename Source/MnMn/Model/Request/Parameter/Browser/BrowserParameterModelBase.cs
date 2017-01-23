using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.Browser
{
    public abstract class BrowserParameterModelBase: ModelBase
    {
        public BrowserParameterModelBase(EventArgs e, WebNavigatorEngine engine)
        {
            OriginalSourceBase = e;
            Engine = engine;
        }

        #region property

        public EventArgs OriginalSourceBase { get; }

        public WebNavigatorEngine Engine { get; }

        #endregion
    }
}
