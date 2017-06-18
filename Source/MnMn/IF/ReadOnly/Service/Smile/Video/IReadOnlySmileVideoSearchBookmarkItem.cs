using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video
{
    public interface IReadOnlySmileVideoSearchBookmarkItem
    {
        #region property

        string Query { get; }
        SearchType SearchType { get; }

        bool IsCheckUpdate { get; }

        IReadOnlyList<string> VideoIds { get; }

        #endregion
    }
}
