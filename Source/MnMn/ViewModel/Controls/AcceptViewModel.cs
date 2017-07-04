using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.MnMn.Library.Bridging.Define;
using ContentTypeTextNet.MnMn.MnMn.Data;
using ContentTypeTextNet.MnMn.MnMn.Define;
using ContentTypeTextNet.MnMn.MnMn.IF.Control;
using ContentTypeTextNet.MnMn.MnMn.IF.ReadOnly;
using ContentTypeTextNet.MnMn.MnMn.Logic;
using ContentTypeTextNet.MnMn.MnMn.Logic.Extensions;
using ContentTypeTextNet.MnMn.MnMn.Logic.Utility;
using ContentTypeTextNet.MnMn.MnMn.Model;
using ContentTypeTextNet.MnMn.MnMn.Model.Request;
using ContentTypeTextNet.MnMn.MnMn.Model.Setting;
using ContentTypeTextNet.MnMn.MnMn.View.Controls;
using Gecko;

namespace ContentTypeTextNet.MnMn.MnMn.ViewModel.Controls
{
    public class AcceptViewModel : ViewModelBase, ISetView
    {
        public AcceptViewModel(Mediation mediation, IReadOnlyAcceptVersion acceptVersion)
        {
            Mediation = mediation;
            AcceptVersion = acceptVersion;

            Setting = Mediation.GetResultFromRequest<AppSettingModel>(new RequestModel(RequestKind.Setting, ServiceType.Application));
        }

        #region property

        Mediation Mediation { get; }
        IReadOnlyAcceptVersion AcceptVersion { get; }
        AppSettingModel Setting { get; }

        AcceptWindow View { get; set; }

        FlowDocument TopDocument { get; set; }

        WebNavigator BrowserCultureLicense { get; set; }
        WebNavigator BrowserOriginalLicense { get; set; }

        public double ViewScale
        {
            get { return Setting.ViewScale; }
            set
            {
                if(SetPropertyValue(Setting, value)) {
                    WebNavigatorUtility.ApplyWebNavigatorScale(View, ViewScale);
                }
            }
        }

        public Uri DevelopmentUri => new Uri(Constants.AppUriDevelopment);

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
            get
            {
                return CreateCommand(o => {
                    Setting.RunningInformation.Accept = true;
                    View.DialogResult = true;
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return CreateCommand(o => {
                    Setting.RunningInformation.Accept = false;
                    View.DialogResult = false;
                });
            }
        }

        public ICommand OpenDevelopmentUriCommand
        {
            get
            {
                return CreateCommand(o => {
                    ShellUtility.OpenUriInDefaultBrowser(DevelopmentUri, Mediation.Logger);
                });
            }
        }

        public ICommand CopyDevelopmentUriCommand
        {
            get
            {
                return CreateCommand(o => {
                    ShellUtility.SetClipboard(DevelopmentUri.OriginalString, Mediation.Logger);
                });
            }
        }

        #endregion

        #region function

        /// <summary>
        /// 使用許諾表示理由。
        /// </summary>
        void InitializeTopDocumentForceShow()
        {
            // 初回起動時は前回との違いなんぞ何もない
            if(Setting.RunningInformation.LastExecuteVersion == null) {
                return;
            }

            var lastVersion = new Version(
                Setting.RunningInformation.LastExecuteVersion.Major,
                Setting.RunningInformation.LastExecuteVersion.Minor,
                Setting.RunningInformation.LastExecuteVersion.Build
            );

            var versionItems = AcceptVersion.Items
                .Select(e => new {
                    Version = new Version(e.Key),
                    Texts = e.Extends
                        .Select(p => new {
                            Sort = p.Key,
                            Text = p.Value
                        })
                        .OrderBy(i => i.Sort)
                        .Select(i => i.Text)
                })
                .Where(i => lastVersion <= i.Version) // 知ってるバージョンは除外する
                .OrderByDescending(i => i.Version)
            ;

            var sections = new List<Section>();
            foreach(var versionItem in versionItems) {
                var versionText = new Run(versionItem.Version.ToString());
                var versionParent = new Paragraph(versionText);

                var textsParent = new List();
                foreach(var text in versionItem.Texts) {
                    var textLine = new Run(text);
                    var textParent = new Paragraph(textLine);
                    var listItem = new ListItem(textParent);
                    textsParent.ListItems.Add(listItem);
                }

                var section = new Section();
                section.Blocks.Add(versionParent);
                section.Blocks.Add(textsParent);

                sections.Add(section);
            }

            if(sections.Any()) {
                var acceptVersionParent = new Section();
                acceptVersionParent.Blocks.AddRange(sections);

                TopDocument.Blocks.Add(acceptVersionParent);
            }
        }

        void InitializeTopDocument()
        {
            InitializeTopDocumentForceShow();
        }

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

        public void Initialize()
        {
            InitializeTopDocument();
            InitializedOriginLicense();
            InitializedCultureLicense();
        }

        #endregion


        #region ISetView

        public void SetView(FrameworkElement view)
        {
            View = (AcceptWindow)view;

            TopDocument = View.topDocument;
            BrowserCultureLicense = View.docCultureLicense;
            BrowserOriginalLicense = View.docOriginalLicense;

            WebNavigatorUtility.ApplyWebNavigatorScale(View, ViewScale);
        }

        #endregion
    }
}
