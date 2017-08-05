using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public class KeywordTextItemViewModel : ViewModelBase, IReadOnlyKeywordTextItem
    {
        #region variable

        string _keyword;
        string _value;
        string _tooltip;

        #endregion

        #region IReadOnlyKeywordTextItem

        public string Keyword
        {
            get { return this._keyword; }
            set { SetVariableValue(ref this._keyword, value); }
        }

        public string Value
        {
            get { return this._value; }
            set { SetVariableValue(ref this._value, value); }
        }


        public string ToolTip
        {
            get { return this._tooltip; }
            set { SetVariableValue(ref this._tooltip, value); }
        }


        #endregion
    }

    public class KeywordTextItemDefinedViewModel : DefinedElementViewModelBase, IReadOnlyKeywordTextItem
    {
        public KeywordTextItemDefinedViewModel(IReadOnlyDefinedElement model)
            : base(model)
        { }

        #region IReadOnlyKeywordTextItem

        public string Keyword => Model.Extends["keyword"];

        public string Value => Model.Key;

        public string ToolTip => Model.Extends["tooltip"];

        #endregion
    }
}
