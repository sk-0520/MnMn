using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Data.Browser
{
    public abstract class BrowserContextMenuBase
    {
        public BrowserContextMenuBase(string key, ServiceType serviceType)
        {
            Key = key;
            ServiceType = serviceType;
        }

        #region property

        public string Key { get; }

        public ServiceType ServiceType { get; }

        #endregion
    }
}
