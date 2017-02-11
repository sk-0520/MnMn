using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Response
{
    public class WebNavigatorNavigatingResultModel: WebNavigatorResultModel
    {
        public WebNavigatorNavigatingResultModel(bool cancel, WebNavigatorNavigatingItemViewModel navigatingItem, string parameter)
            : base(cancel)
        {
            NavigatingItem = navigatingItem;
            Parameter = parameter;
        }

        #region property

        public WebNavigatorNavigatingItemViewModel NavigatingItem { get; }
        public string Parameter { get; set; }

        #endregion
    }
}
