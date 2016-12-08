using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContentTypeTextNet.MnMn.MnMn.IF
{
    public interface IDescription
    {
        ICommand OpenWebLinkCommand { get; }
        ICommand MenuOpenWebLinkCommand { get; }
        ICommand MenuOpenWebLinkAppBrowserCmmand { get; }
    }
}
