using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Response
{
    public class WebNavigatorGestureResultModel : WebNavigatorResultModel
    {
        public WebNavigatorGestureResultModel(IReadOnlyList<IReadOnlyWebNavigatorGestureElement> items)
            : base(false)
        {
            GestureItems = items;
        }

        #region property

        public IReadOnlyList<IReadOnlyWebNavigatorGestureElement> GestureItems { get; }

        #endregion
    }
}
