using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public class WebNavigatorNavigatingItemViewModel: WebNavigatorDefinedElementViewModelBase<WebNavigatorNavigatingItemModel>
    {
        #region variable

        IReadOnlyList<WebNavigatorPageConditionViewModel> _conditions;

        #endregion

        public WebNavigatorNavigatingItemViewModel(WebNavigatorNavigatingItemModel model)
            : base(model)
        { }

        #region property

        public IReadOnlyList<WebNavigatorPageConditionViewModel> Conditions
        {
            get
            {
                if(this._conditions == null) {
                    this._conditions = DefinedModel.Conditions
                        .Select(i => new WebNavigatorPageConditionViewModel(i))
                        .ToEvalSequence()
                    ;
                }

                return this._conditions;
            }
        }

        #endregion
    }
}
