using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.WebNavigatorBridge;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.WebNavigatorBridge
{
    public class WebNavigatorElementConditionItemViewModel: SingleModelWrapperViewModelBase<WebNavigatorElementConditionItemModel>
    {
        #region variable

        IReadOnlyList<WebNavigatorElementConditionTagViewModel> _targetItems;
        WebNavigatorElementConditionParameterViewModel _parameter;
        Regex _baseUriRegex;
        Regex _tagNameRegex;

        #endregion

        public WebNavigatorElementConditionItemViewModel(WebNavigatorElementConditionItemModel model)
            : base(model)
        { }

        #region property

        public bool IsVisible { get { return Model.IsVisible; } }

        public bool IsEnabledBaseUri => !string.IsNullOrEmpty(Model.BaseUriPattern);

        public Regex BaseUriRegex
        {
            get
            {
                if(this._baseUriRegex == null) {
                    this._baseUriRegex = WebNavigatorUtility.CreateConditionRegex(Model.BaseUriPattern);
                }
                return this._baseUriRegex;
            }
        }

        public bool IsEnabledTagName => !string.IsNullOrEmpty(Model.TagNamePattern);

        public Regex TagNameRegex
        {
            get
            {
                if(this._tagNameRegex == null) {
                    this._tagNameRegex = WebNavigatorUtility.CreateConditionRegex(Model.TagNamePattern);
                }

                return this._tagNameRegex;
            }
        }


        public IReadOnlyList<WebNavigatorElementConditionTagViewModel> TargetItems
        {
            get
            {
                if(this._targetItems == null) {
                    this._targetItems = Model.TargetItems
                        .Select(i => new WebNavigatorElementConditionTagViewModel(i))
                        .ToEvalSequence()
                    ;
                }

                return this._targetItems;
            }
        }

        public WebNavigatorElementConditionParameterViewModel Parameter
        {
            get
            {
                if(this._parameter == null) {
                    this._parameter = new WebNavigatorElementConditionParameterViewModel(Model.Parameter);
                }

                return this._parameter;
            }
        }


        #endregion
    }
}
