using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.MyList
{
    public class SmileVideoMyListBookmarkFilterViewModel: ViewModelBase, ICheckable
    {
        #region variable

        bool _isChecked;

        #endregion

        public SmileVideoMyListBookmarkFilterViewModel(string tagName)
        {
            TagName = tagName;
        }

        #region property

        public string TagName { get; }

        public bool? IsChecked
        {
            get { return this._isChecked; }
            set { SetVariableValue(ref this._isChecked, value.GetValueOrDefault()); }
        }

        #endregion
    }
}
