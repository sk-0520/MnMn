using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Data.Browser
{
    public sealed class BrowserContextMenuSeparator: BrowserContextMenuBase
    {
        public BrowserContextMenuSeparator(string key, ServiceType serviceType) 
            : base(key, serviceType)
        { }
    }
}
