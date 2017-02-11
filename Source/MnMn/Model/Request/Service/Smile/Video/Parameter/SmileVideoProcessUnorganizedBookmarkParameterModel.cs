using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter
{
    public class SmileVideoProcessUnorganizedBookmarkParameterModel: SmileVideoProcessParameterModelBase
    {
        public SmileVideoProcessUnorganizedBookmarkParameterModel(SmileVideoVideoItemModel videoItem)
            : base(SmileVideoProcess.UnorganizedBookmark)
        {
            VideoItem = videoItem;
        }

        #region property

        public SmileVideoVideoItemModel VideoItem { get; }

        #endregion
    }
}
