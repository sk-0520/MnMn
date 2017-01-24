using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;

namespace ContentTypeTextNet.MnMn.MnMn.Data.Browser
{
    public class BrowserContextMenuItem: BrowserContextMenuBase
    {
        public BrowserContextMenuItem(string key, ServiceType serviceType) 
            : base(key, serviceType)
        { }

        #region property

        public string Header { get; set; }

        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }

        #endregion
    }
}
