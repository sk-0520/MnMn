using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Response
{
    public class WebNavigatorContextMenuDefineResultModel: WebNavigatorResultModel
    {
        public WebNavigatorContextMenuDefineResultModel(IReadOnlyList<WebNavigatorContextMenuItemModel> items)
            :base(false)
        {
            Items = items;
        }

        #region property

        public IReadOnlyList<WebNavigatorContextMenuItemModel> Items { get; }

        #endregion
    }
}
