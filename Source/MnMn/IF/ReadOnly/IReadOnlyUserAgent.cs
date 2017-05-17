using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly
{
    public interface IReadOnlyUserAgent
    {
        bool UsingCustomUserAgent { get; }
        string CustomUserAgentFormat { get; }
    }
}
