using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    public class ApplicationWindow: MetroWindow
    {
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if(WindowState == System.Windows.WindowState.Normal) {
                BorderThickness = Constants.WindowDefaultThickness;
            }
        }
    }
}
