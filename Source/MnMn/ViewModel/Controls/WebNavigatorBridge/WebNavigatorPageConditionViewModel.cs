﻿using System;
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
    public class WebNavigatorPageConditionViewModel: SingleModelWrapperViewModelBase<WebNavigatorPageConditionModel>
    {
        #region variable

        Regex _uriRegex;

        #endregion

        public WebNavigatorPageConditionViewModel(WebNavigatorPageConditionModel model) 
            : base(model)
        { }

        #region property

        public string ParameterSource => Model.ParameterSource;

        Regex UriRegex
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
