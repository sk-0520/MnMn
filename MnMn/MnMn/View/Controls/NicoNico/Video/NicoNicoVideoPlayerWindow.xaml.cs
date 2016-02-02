using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls.NicoNico.Video
{
    /// <summary>
    /// NicoNicoVideoPlayerWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NicoNicoVideoPlayerWindow: System.Windows.Window
    {
        public NicoNicoVideoPlayerWindow()
        {
            InitializeComponent();
            //this.player.MediaPlayer.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;
            //this.player.MediaPlayer.EndInit();
        }
        //private void OnVlcControlNeedsLibDirectory(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        //{
        //    var currentAssembly = Assembly.GetEntryAssembly();
        //    var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
        //    if(currentDirectory == null)
        //        return;
        //    if(AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)
        //        e.VlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"lib\lib\x86\"));
        //    else
        //        e.VlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"lib\lib\x64\"));
        //}
    }
}
