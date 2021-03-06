﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.WebNavigatorBridge;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public class WebNavigatorElementConditionTagViewModel: WebNavigatorElementConditionTagViewModelBase<IReadOnlyWebNavigatorElementConditionTag>
    {
        public WebNavigatorElementConditionTagViewModel(IReadOnlyWebNavigatorElementConditionTag model) 
            : base(model)
        { }

        #region property
        #endregion
    }
}
