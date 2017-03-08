using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContentTypeTextNet.MnMn.MnMn.IF.Control
{
    public interface IWindowMessage
    {
        bool PreProcessMessage(ref Message msg);
    }
}
