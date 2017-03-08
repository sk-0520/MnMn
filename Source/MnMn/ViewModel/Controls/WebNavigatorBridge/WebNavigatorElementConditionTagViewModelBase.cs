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
    public abstract class WebNavigatorElementConditionTagViewModelBase<TModel>: SingleModelWrapperViewModelBase<TModel>
        where TModel : WebNavigatorElementConditionTagModel
    {
        #region variable

        //Regex _tagNameRegex;
        Regex _valueRegex;

        #endregion

        public WebNavigatorElementConditionTagViewModelBase(TModel model)
            : base(model)
        { }

        #region property

        public string Attribute { get { return Model.Attribute; } }

        public Regex ValueRegex
        {
            get
            {
                if(this._valueRegex == null) {
                    this._valueRegex = WebNavigatorUtility.CreateConditionRegex(Model.ValuePattern);
                }

                return this._valueRegex;
            }
        }

        #endregion
    }
}
