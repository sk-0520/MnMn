using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video
{
    public interface IReadOnlySmileVideoRanking
    {
        #region property

        IReadOnlySmileVideoRankingGroup Periods { get; }

        IReadOnlySmileVideoRankingGroup Targets { get; }

        IReadOnlyList<IReadOnlySmileVideoCategoryGroup> Items { get; }

        #endregion
    }
}
