using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video
{
    public interface IReadOnlySmileVideoCategoryGroup
    {
        #region property

        /// <summary>
        /// カテゴリ一覧。
        /// </summary>
        IReadOnlyList<IReadOnlyDefinedElement> Categories { get; }

        /// <summary>
        /// カテゴリは一つだけか。
        /// </summary>
        bool IsSingleCategory { get; }

        #endregion
    }
}
