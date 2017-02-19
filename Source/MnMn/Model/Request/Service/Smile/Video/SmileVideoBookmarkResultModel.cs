using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video
{
    public class SmileVideoBookmarkResultModel:ModelBase
    {
        public SmileVideoBookmarkResultModel(CollectionModel<SmileVideoBookmarkSystemNodeViewModel> systemNodes, CollectionModel<SmileVideoBookmarkNodeViewModel> userNodes)
        {
            SystemNodes = systemNodes;
            UserNodes = userNodes;
        }

        #region property

        public IReadOnlyList<SmileVideoBookmarkSystemNodeViewModel> SystemNodes { get; }
        public IReadOnlyList<SmileVideoBookmarkNodeViewModel> UserNodes { get; }

        #endregion
    }
}
