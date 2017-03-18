using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public class AcceptViewModel : ViewModelBase, ISetView
    {
        public AcceptViewModel(Mediation mediation, RunningInformationSettingModel runningInformation)
        {
            Mediation = mediation;
            RunningInformation = runningInformation;
        }

        #region property

        Mediation Mediation { get; }

        AcceptWindow View { get; set; }

        WebNavigator BrowserCultureLicense { get; set; }
        WebNavigator BrowserOriginalLicense { get; set; }

        RunningInformationSettingModel RunningInformation { get; }

        #endregion

        #region command

        public ICommand NewWindowCommand
        {
            get
            {
                return CreateCommand(o => {
                    var data = (WebNavigatorEventDataBase)o;
                    WebNavigatorUtility.OpenNewWindowWrapper(data, Mediation.Logger);
                });
            }
        }

        public ICommand DomLoadedCommand
        {
            get
            {
                return CreateCommand(o => {
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
                });
            }
        }

        public ICommand AcceptCommand
        {
            get {
                return CreateCommand(o => {

                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return CreateCommand(o => {

                });
            }
        }

        #endregion

        #region function

        void InitializedOriginLicense()
        {
            var path = System.IO.Path.Combine(Constants.ApplicationDocDirectoryPath, "License", "MnMn-GPLv3.html");
            BrowserOriginalLicense.HomeSource = new Uri(path);
            BrowserOriginalLicense.Navigate(BrowserOriginalLicense.HomeSource);
        }

        void InitializedCultureLicense()
        {
            // TODO: カルチャと銘打ったものの日本語おんりー
            var path = System.IO.Path.Combine(Constants.ApplicationDocDirectoryPath, "License", "MnMn-GPLv3.ja-jp.html");
            BrowserCultureLicense.HomeSource = new Uri(path);
            BrowserCultureLicense.Navigate(BrowserCultureLicense.HomeSource);
        }

        void Initialize()
        {
            InitializedOriginLicense();
            InitializedCultureLicense();
        }

        #endregion


        #region ISetView

        public void SetView(FrameworkElement view)
        {
            View = (AcceptWindow)view;

            BrowserCultureLicense = View.docCultureLicense;
            BrowserOriginalLicense = View.docOriginalLicense;
        }

        #endregion
    }
}
