using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Response
{
    public class WebNavigatorGestureResultModel : WebNavigatorResultModel
    {
        public WebNavigatorGestureResultModel(IReadOnlyList<WebNavigatorGestureElementModel> items)
            : base(false)
        {
            GestureItems = items;
        }

        #region property

        public IReadOnlyList<WebNavigatorGestureElementModel> GestureItems { get; }

        #endregion
    }
}
