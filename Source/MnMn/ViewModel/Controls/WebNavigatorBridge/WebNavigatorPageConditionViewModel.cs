using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public class WebNavigatorPageConditionViewModel: SingleModelWrapperViewModelBase<IReadOnlyWebNavigatorPageCondition>
    {
        #region variable

        Regex _uriRegex;

        #endregion

        public WebNavigatorPageConditionViewModel(IReadOnlyWebNavigatorPageCondition model) 
            : base(model)
        { }

        #region property

        public string ParameterSource => Model.ParameterSource;

        public Regex UriRegex
        {
            get
            {
                if(this._uriRegex == null) {
                    this._uriRegex = WebNavigatorUtility.CreateConditionRegex(Model.UriPattern);
                }

                return this._uriRegex;
            }
        }

        #endregion

    }
}
