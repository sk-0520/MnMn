using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.MnMn.MnMn.Model;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly.Service.Smile.Video
{
    public interface IReadOnlySmileVideoSearchDefine
    {
        #region proeprty

        int MaximumIndex { get; }
        int MaximumCount { get; }

        IReadOnlyList<DefinedElementModel> Methods { get; }
        IReadOnlyList<DefinedElementModel> Sort { get; }
        IReadOnlyList<DefinedElementModel> Type { get; }

        #endregion
    }
}
