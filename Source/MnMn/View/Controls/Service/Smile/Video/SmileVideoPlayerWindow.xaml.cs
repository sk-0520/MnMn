using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ContentTypeTextNet.MnMn.MnMn.Logic.View;
using MahApps.Metro.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.Service.Smile.Video
{
    /// <summary>
    /// SmileVideoPlayerWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SmileVideoPlayerWindow: MetroWindow
    {
        #region property

        CursorHider PlayerCursorHider { get; }

        #endregion

        public SmileVideoPlayerWindow()
        {
            InitializeComponent();

            PlayerCursorHider = new CursorHider(this.player);
        }

        protected override void OnClosed(EventArgs e)
        {
            PlayerCursorHider.Dispose();

            base.OnClosed(e);
        }
    }
}
