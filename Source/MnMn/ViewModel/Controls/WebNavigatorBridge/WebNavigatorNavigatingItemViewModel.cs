using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public class WebNavigatorNavigatingItemViewModel: DefinedElementViewModelBase<WebNavigatorNavigatingItemModel>
    {
        public WebNavigatorNavigatingItemViewModel(WebNavigatorNavigatingItemModel model)
            : base(model)
        {
            Conditions = DefinedModel.Conditions
                .Select(i => new WebNavigatorPageConditionViewModel(i))
                .ToList()
            ;
        }

        #region property

        public IReadOnlyList<WebNavigatorPageConditionViewModel> Conditions { get; }

        #endregion
    }
}
