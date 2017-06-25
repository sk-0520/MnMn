using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define.Service.Smile.Video;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Setting;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video
{
    public interface IReadOnlySmileVideoFinderFilteringItem: IReadOnlyFilteringItem
    {
        #region property

        /// <summary>
        /// 対象。
        /// </summary>
        SmileVideoFinderFilteringTarget Target { get; }

        #endregion
    }
}
