using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// AcceptWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AcceptWindow: Window
    {
        public AcceptWindow(Mediation mediation)
        {
            InitializeComponent();
            Mediation = mediation;
            Initialize();
        }

        #region property

        Mediation Mediation { get; }

        #endregion

        #region command

        public ICommand OpenWebLinkCommand
        {
            get
            {
                return new DelegateCommand(
                    o => { }
                );
            }
        }


        #endregion

        #region function

        void InitializedOriginLicense()
        {
            var path = System.IO.Path.Combine(Constants.ApplicationDocDirectoryPath, "License", "MnMn-GPLv3.html");
            this.docOriginalLicense.Navigate(new Uri(path));
        }

        void Initialize()
        {
            InitializedOriginLicense();
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
