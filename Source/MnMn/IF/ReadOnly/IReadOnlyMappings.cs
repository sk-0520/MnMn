using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyMappings
    {
        #region property

        IReadOnlyList<IReadOnlyMapping> Mappings { get; }

        #endregion
    }
}
