﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public class WebNavigatorContextMenuItemViewModel: WebNavigatorDefinedElementViewModelBase<WebNavigatorContextMenuItemModel>
    {
        #region variable

        List<WebNavigatorElementConditionItemViewModel> _conditions;

        #endregion

        public WebNavigatorContextMenuItemViewModel(WebNavigatorContextMenuItemModel model) 
            : base(model)
        { }

        #region property

        public bool IsSeparator { get { return DefinedModel.IsSeparator; } }

        public IReadOnlyList<WebNavigatorElementConditionItemViewModel> Conditions {
            get
            {
                if(this._conditions == null) {
                    this._conditions = DefinedModel.Conditions
                        .Select(c => new WebNavigatorElementConditionItemViewModel(c))
                        .ToList()
                    ;
                }

                return this._conditions;
            }
        }

        #endregion
    }
}