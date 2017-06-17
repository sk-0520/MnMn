using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Search
{
    public class SmileVideoSearchBookmarkItemViewModel : SingleModelWrapperViewModelBase<SmileVideoSearchBookmarkItemModel>
    {
        public SmileVideoSearchBookmarkItemViewModel(SmileVideoSearchBookmarkItemModel model)
            : base(model)
        { }

        #region property

        public string Query => Model.Query;

        public bool IsCheckUpdate
        {
            get { return Model.IsCheckUpdate; }
            set { SetModelValue(value); }
        }

        #endregion
    }
}
