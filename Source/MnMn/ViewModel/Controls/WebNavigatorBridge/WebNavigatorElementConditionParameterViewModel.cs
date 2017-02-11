using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public class WebNavigatorElementConditionParameterViewModel: WebNavigatorElementConditionTagViewModelBase<WebNavigatorElementConditionParameterModel>
    {
        public WebNavigatorElementConditionParameterViewModel(WebNavigatorElementConditionParameterModel model) 
            : base(model)
        { }

        #region property

        public string ParameterSource { get { return Model.ParameterSource; } }

        #endregion
    }
}
