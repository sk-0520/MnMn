﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Setting
{
    public class SmileVideoCommentFilteringElementViewModel: DefinedElementViewModelBase
    {
        #region variable

        bool _isChecked;

        #endregion

        public SmileVideoCommentFilteringElementViewModel(DefinedElementModel model)
            : base(model)
        { }

        #region property

        public bool IsChecked
        {
            get { return this._isChecked; }
            set { SetVariableValue(ref this._isChecked, value); }
        }

        public string Source
        {
            get { return Model.Extends["pattern"]; }
        }


        #endregion
    }
}
