using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Parameter.WebNavigator
{
    public class WebNavigatorParameterModel: ModelBase
    {
        public WebNavigatorParameterModel(Uri currentUri, EventArgs e, WebNavigatorEngine engine, WebNavigatorParameterKind kind)
        {
            CurrentUri = currentUri;
            OriginalSourceBase = e;
            Engine = engine;
            Kind = kind;
        }

        #region property

        public Uri CurrentUri { get; }

        public EventArgs OriginalSourceBase { get; }

        public WebNavigatorEngine Engine { get; }

        public WebNavigatorParameterKind Kind { get; }

        #endregion
    }
}
