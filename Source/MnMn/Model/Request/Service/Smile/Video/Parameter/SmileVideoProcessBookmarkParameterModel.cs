using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls.Service.Smile.Video.Bookmark;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter
{
    public class SmileVideoProcessBookmarkParameterModel: SmileVideoProcessParameterModelBase
    {
        public SmileVideoProcessBookmarkParameterModel(SmileVideoBookmarkNodeViewModel bookmark, IEnumerable<SmileVideoVideoItemModel> videoItems)
            : base(SmileVideoProcess.Bookmark)
        {
            Bookmark = bookmark;
            VideoItems = videoItems;
        }

        #region 

        public SmileVideoBookmarkNodeViewModel Bookmark { get; }
        public IEnumerable<SmileVideoVideoItemModel> VideoItems { get; }

        #endregion
    }
}
