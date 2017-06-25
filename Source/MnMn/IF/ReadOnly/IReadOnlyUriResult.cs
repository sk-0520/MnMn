using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.MnMn.Define;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyUriResult
    {
        #region property

        string Uri { get; }

        ParameterType RequestParameterType { get; }

        bool SafetyUri { get; }

        bool SafetyHeader { get; }

        bool SafetyParameter { get; }

        #endregion
    }
}
