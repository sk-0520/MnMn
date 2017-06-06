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
        public SmileVideoProcessBookmarkParameterModel(SmileVideoBookmarkNodeViewModel parentBookmark, IEnumerable<SmileVideoVideoItemModel> videoItems, bool addItems)
            : base(SmileVideoProcess.Bookmark)
        {
            IsNewNode = false;

            ParentBookmark = parentBookmark;
            VideoItems = videoItems;
            AddItems = addItems;
        }

        public SmileVideoProcessBookmarkParameterModel(SmileVideoBookmarkNodeViewModel parentBookmark, SmileVideoBookmarkItemSettingModel newNode)
            : base(SmileVideoProcess.Bookmark)
        {
            IsNewNode = true;

            ParentBookmark = parentBookmark;
            NewNode = newNode;
        }

        #region

        public bool IsNewNode { get; }

        public SmileVideoBookmarkNodeViewModel ParentBookmark { get; }
        public IEnumerable<SmileVideoVideoItemModel> VideoItems { get; }
        public SmileVideoBookmarkItemSettingModel NewNode { get; }
        public bool AddItems { get; }

        #endregion
    }
}
