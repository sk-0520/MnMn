using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyNetworkProxy: IReadOnlyPassword
    {
        bool UsingCustomProxy { get; }
        string ServerAddress { get; }
        int ServerPort { get; }

        bool UsingAuth { get; }
        string UserName { get; }

        DateTime ChangedTimestamp { get; }
    }
}
