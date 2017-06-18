using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;

namespace ContentTypeTextNet.MnMn.MnMn.Model.Request.Service.Smile.Video.Parameter
{
    public class SmileVideoProcessSearchBookmarkParameterModel : SmileVideoProcessParameterModelBase
    {
        public SmileVideoProcessSearchBookmarkParameterModel(bool addBookmark, string query, SearchType searchType)
            : base(SmileVideoProcess.SearchBookmark)
        {
            AddBookmark = addBookmark;
            SearchType = searchType;
            Query = query;
        }

        #region property

        public bool AddBookmark { get; set; }

        public SearchType SearchType { get; set; }

        public string Query { get; set; }

        #endregion
    }
}
