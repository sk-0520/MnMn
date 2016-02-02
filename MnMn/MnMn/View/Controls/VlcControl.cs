using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using System.Windows.Threading;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    public class VlcControl: WindowsFormsHost
    {
        public Vlc.DotNet.Forms.VlcControl MediaPlayer { get; private set; }

        public VlcControl()
        {
            MediaPlayer = new Vlc.DotNet.Forms.VlcControl();
            this.Child = MediaPlayer;

        //    IsRedirected = "True"
        //CompositionMode = "OutputOnly"
        
        }

        public bool IsRedirected { get; set; }

    }
}
