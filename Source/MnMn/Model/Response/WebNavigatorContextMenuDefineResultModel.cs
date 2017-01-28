using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Response
{
    public class WebNavigatorContextMenuDefineResultModel: WebNavigatorResultModel
    {
        public WebNavigatorContextMenuDefineResultModel(IReadOnlyDictionary<string, WebNavigatorContextMenuItemViewModel> items)
            :base(false)
        {
            Items = items;
        }

        #region property

        public IReadOnlyDictionary<string, WebNavigatorContextMenuItemViewModel> Items { get; }

        #endregion
    }
}
