using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.Library.Bridging.IF.ReadOnly
{
    public interface IReadOnlyCodeInitialize
    {
        #region property

        string DomainName { get; }

        string Identifier { get; }

        string Sequence { get; }

        #endregion
    }
}
