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
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using Gecko;
using MahApps.Metro.Controls;

namespace ContentTypeTextNet.MnMn.MnMn.View.Controls
{
    /// <summary>
    /// AcceptWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AcceptWindow: MetroWindow
    {
        public AcceptWindow()
        {
            InitializeComponent();
        }

        #region property

        Mediation Mediation { get; }

        #endregion

        #region command

        public ICommand DomLoadedCommand
        {
            get
            {
                return new DelegateCommand(
                    o => {
                        var e = (WebNavigatorEventDataBase)o;
                        switch(e.Engine) {
                            case Define.WebNavigatorEngine.Default:
                                break;

                            case Define.WebNavigatorEngine.GeckoFx: {
                                    var ex = (WebNavigatorEventData<DomEventArgs>)e;
                                    var browser = (GeckoWebBrowser)ex.Sender;
                                    using(var context = new AutoJSContext(browser.Window)) {
                                        context.EvaluateScript(Properties.Resources.File_JavaScript_GeckoFx_AcceptLink);
                                    }
                                }
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                    }
                );
            }
        }

        public ICommand NewWindowCommand
        {
            get {
                return new DelegateCommand(
                    o => {
                        var data = (WebNavigatorEventDataBase)o;
                        WebNavigatorUtility.OpenNewWindowWrapper(data, Mediation.Logger);
                    }
                );
            }
        }


        #endregion

        #region function

        void InitializedOriginLicense()
        {
            var path = System.IO.Path.Combine(Constants.ApplicationDocDirectoryPath, "License", "MnMn-GPLv3.html");
            this.docOriginalLicense.HomeSource = new Uri(path);
            this.docOriginalLicense.Navigate(this.docOriginalLicense.HomeSource);
        }

        void InitializedCultureLicense()
        {
            // TODO: カルチャと銘打ったものの日本語おんりー
            var path = System.IO.Path.Combine(Constants.ApplicationDocDirectoryPath, "License", "MnMn-GPLv3.ja-jp.html");
            this.docCultureLicense.HomeSource = new Uri(path);
            this.docCultureLicense.Navigate(this.docCultureLicense.HomeSource);
        }

        void Initialize()
        {
            InitializedOriginLicense();
            InitializedCultureLicense();
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
